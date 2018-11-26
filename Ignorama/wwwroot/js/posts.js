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
    mixins: [dateMixin],
    computed: {
        visiblePosts: function () {
            var startPost = (this.page - 1) * this.perPage;
            return this.posts.slice(startPost, startPost + this.perPage);
        }
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
    filters: {
        formatPost: function (post) {
            post = escapeHTML(post);

            //images
            post = post.replace(/\[img\](.*?)\[\/img\]/gi, '<a target="_blank" href="$1"><img href="$1" src="$1" class="img img-responsive" style="max-height: 480px;"></img></a>');
            //webm
            post = post.replace(/\[webm\](.*?)\[\/webm\]/gi, '<video preload="none" controls="controls" class="img img-responsive"><source type="video/webm" src="$1"></video>');
            //bold and italics
            post = post.replace(/\[i\]([\s\S]*?)\[\/i\]/ig, "<i>$1</i>");
            post = post.replace(/\[b\]([\s\S]*?)\[\/b\]/ig, "<b>$1</b>");
            post = post.replace(/\[u\]([\s\S]*?)\[\/u\]/ig, "<u>$1</u>");
            //spoilers
            post = post.replace(/\[spoiler\]([\s\S]*?)\[\/spoiler\]/ig, "<span class='spoiler'>$1</span>");
            //user reference
            post = post.replace(/\[(.*?)\|([0-9]+)\]/ig, "<a href='javascript:postsVue.viewPost($2);'><b>$1</b></a>");
            //replies
            post = post.replace(/\[reply user=(.*?) post=([0-9]+)\]\s*([\s\S]*?)\s*\[\/reply\]\s?/ig, "<div style='padding: 5px;border: 1px solid #DDD;background-color:#F5F5F5'><b><a href='javascript:postsVue.viewPost($2);'>$1</a> said:</b><br/>$3</div>");
            //quotes
            post = post.replace(/\[quote\]\s?([\s\S]*?)\s?\[\/quote\]\s?/ig, "<div style='padding: 5px;border: 1px solid #DDD;background-color:#F5F5F5'><b>Quote:</b><br/>$1</div>");
            //code
            post = post.replace(/\[code\]\s*([\s\S]*?)\s*\[\/code\]/ig, "<pre><code>$1</code></pre>");
            //colored text
            post = post.replace(/\[color=(.*?)\]([\s\S]*?)\[\/color\]/ig, "<span style='color:$1'>$2</span>");
            //url
            post = post.replace(/\[url=(http(s?):\/\/)?(.*?)\](.*?)\[\/url\]/ig, "<a target='_blank' href='http$2://$3'>$4</a>");
            //youtube embed
            post = post.replace(/[a-zA-Z\/\/:\.]*(youtube.com\/watch\?v=|youtu.be\/)([a-zA-Z0-9\-_]+)([a-zA-Z0-9\/\*\-\_\?\&\;\%\=\.]*)/gi, "<div class='flex-video widescreen'><iframe width=\"560\" height=\"315\" src=\"//www.youtube.com/embed/$2\" frameborder=\"0\" allowfullscreen></iframe></div>");

            post = post.replace(/\n/g, '<br>');
            return post;
        }
    },
    watch: {
        visiblePosts: function (val) {
            var lspID = Math.max(...val.map(p => p.ID));
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
        reply: function (post, quote) {
            var quotelessText = post.Text
                .replace(/\s*(\[reply.*?\][\s\S]*?\[\/reply\])\s*/gi, "");
            if (quote) {
                $("#postfield").val($("#postfield").val() + '[reply user=' + (post.User && !post.Anonymous ? post.User.UserName : 'Anonymous') +
                    ' post=' + post.ID + ']\n' + quotelessText + '\n[/reply]\n').focus();
            }
            else {
                $("#postfield").val($("#postfield").val() + '[' + (post.User && !post.Anonymous ? post.User.UserName : 'Anonymous') + '|' + post.ID + '] ').focus();
            }
            slideOut();
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
                $('input[name=Anonymous]').prop('checked', false);
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