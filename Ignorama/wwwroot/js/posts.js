var threadID = function () {
    var url = window.location.href.replace(/\/$/, '');
    return url.substr(url.lastIndexOf('/') + 1).replace(/#.*$/, '');
}();

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
    },
    methods: {
        getPosts: function (threadID, callback) {
            axios.get('/Threads/GetPosts/' + threadID)
                .then(response => {
                    this.posts = response.data;
                    if (callback) callback();
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
        }
    }
});

function slideOut() {
    $("#quickreply").stop(true).animate({ bottom: -1 }, 200).attr("class", "slid-out");
    $("#replyCaret").removeClass('caret-up');
    return false;
}

function slideIn() {
    $("#quickreply").animate({ bottom: -249 }, 200).delay(200).queue(function (next) { $(this).attr("class", "slid-in"); next(); });
    $("#replyCaret").addClass('caret-up');
    return false;
}

function slide() {
    if ($("#quickreply").hasClass("slid-out"))
        slideIn();
    else
        slideOut();
    return false;
}

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
                slide();

                postsVue.getPosts(threadID,
                    () => {
                        postsVue.page = Math.ceil((postsVue.posts.length + 1) / postsVue.perPage);
                        setTimeout(function () {
                            window.scrollTo(0, document.body.scrollHeight);
                        }, 100);
                    });
            }
        },
        error: function (_, e) {
            console.log($('#postform').serialize())
            alert('Unable to submit reply: ' + e);
        }
    });
});

var escape = document.createElement('textarea');
function escapeHTML(html) {
    escape.textContent = html;
    return escape.innerHTML;
}

function unescapeHTML(html) {
    escape.innerHTML = html;
    return escape.textContent;
}