var threadID = function () {
    var url = window.location.href.replace(/\/$/, '');
    return url.substr(url.lastIndexOf('/') + 1).replace(/#.*$/, '');
}();

var defaultRefresh = 5000;
var refreshInterval = defaultRefresh;

var quickreplyState = 'slid-in';

var postsVue = new Vue({
    el: 'main',
    data: {
        posts: [],
        page: 1,
        perPage: 20,
    },
    created: function () {
        this.getPosts(threadID, () => {
            this.updatePage();
            var id = parseInt(lastSeenPostID);
            var newPost = this.posts.find(post =>
                post.ID > id);
            if (id) {
                setTimeout(function () {
                    if (newPost)
                        postsVue.$scrollTo('#post' + newPost.ID, 1);
                    else
                        window.scrollTo(0, document.body.scrollHeight);
                }, 100);
            }
        });
    },
    watch: {
        posts: function (val) {
            var startPost = (this.page - 1) * this.perPage;
            var pagePosts = val.slice(startPost, startPost + this.perPage);
            var lspID = Math.max(...pagePosts.map(p => p.ID));
            this.follow(threadID, lspID);
        },
        page: function (val) {
            var startPost = (val - 1) * this.perPage;
            var pagePosts = this.posts.slice(startPost, startPost + this.perPage);
            var lspID = Math.max(...pagePosts.map(p => p.ID));
            this.follow(threadID, lspID);
        },
    },
    methods: {
        getPosts: function (threadID, callback) {
            axios.get('/Threads/GetPosts/' + threadID)
                .then(response => {
                    var newPosts = response.data;
                    newPosts.forEach(function (post) {
                        if (post.ID <= lastSeenPostID)
                            post.Seen = true;
                    });
                    var areNewPosts = JSON.stringify(this.posts) !== JSON.stringify(newPosts);
                    if (areNewPosts) {
                        this.posts = response.data;
                        refreshInterval = defaultRefresh;
                    }
                    if (callback) callback(areNewPosts);
                });
        },
        follow: function (threadID, lastSeenID) {
            axios.post("/Threads/Follow", { ThreadID: threadID, LastSeenPostID: lastSeenID })
                .then(response => {
                    console.log("Followed thread " + response.data);
                })
                .catch(error => {
                    console.error("Error following thread: " + error);
                });
        },
        updatePage: function () {
            if (lastSeenPostID !== '') {
                var id = parseInt(lastSeenPostID);
                var newPage = Math.ceil((this.posts.findIndex(post =>
                    post.ID > id) + 1) / this.perPage);
                this.page = newPage !== 0 ? newPage : Math.ceil(this.posts.length / this.perPage);
            }
        },
        viewPost: function (postID) {
            var newPage = Math.ceil((this.posts.findIndex(p =>
                p.ID > postID)) / this.perPage);
            new Promise(resolve => {
                this.page = newPage;
                resolve();
            })
                .then(() => {
                    this.posts.forEach(post =>
                        post.Highlighted = post.ID == postID ? true : false);
                    this.$scrollTo('#post' + postID);
                });
        },
        expand: function () {
            if ($("#quickreply").hasClass("expanded")) {
                $("#quickreply").attr("class", quickreplyState);
                $("#replyLink").html("<a href='#' onclick='event.preventDefault(); slide();'>Reply <span id='replyCaret' class='caret'></span></a>");
                $("#replyExpand").html("<span class='expand-icon glyphicon glyphicon-resize-full'></span>");
                if (quickreplyState === "slid-in") {
                    $("#replyCaret").addClass("caret-up");
                }
            }
            else {
                quickreplyState = $("#quickreply").attr("class");
                $("#quickreply #postfield").focus();
                $("#quickreply").attr("class", "expanded");
                $("#replyLink").html("Reply");
                $("#replyExpand").html("<span class='expand-icon glyphicon glyphicon-resize-small'></span>");
            }
            return false;
        },
    }
});

var slideOut = function () {
    $("#quickreply").attr("class", "slid-out");
    $("#replyCaret").removeClass('caret-up');
    $("#quickreply #postfield").focus();
    return false;
};

var slideIn = function () {
    $("#quickreply").attr("class", "slid-in");
    $("#replyCaret").addClass('caret-up');
    return false;
};

var slide = function () {
    if ($("#quickreply").hasClass("slid-out"))
        slideIn();
    else
        slideOut();
    return false;
};

$('#postform').submit(function (e) {
    e.preventDefault();
    $.ajax({
        url: '/Posts/New/' + threadID,
        type: 'post',
        data: $('#postform').serialize(),
        success: function (result) {
            if (result.error) {
                alert("Error: " + result.error)
            }
            else {
                $('#postfield').val('');
                $('input[name=Bump]').prop('checked', false);
                $('input[name=RevealOP]').prop('checked', false);
                slideIn();

                postsVue.getPosts(threadID);
            }
        },
        error: function (_, e) {
            console.log($('#postform').serialize())
            alert('Unable to submit reply: ' + e);
        }
    });
});

function getPostsAndIncreaseInterval() {
    postsVue.getPosts(threadID, function (areNewPosts) {
        if (areNewPosts) {
            refreshInterval = defaultRefresh;
        }
        else {
            refreshInterval *= 2;
        }
        console.log("Refreshing thread in " + (refreshInterval / 1000) + " seconds");
        setTimeout(getPostsAndIncreaseInterval, refreshInterval);
    });
}

$(function () {
    setTimeout(getPostsAndIncreaseInterval, refreshInterval);
})