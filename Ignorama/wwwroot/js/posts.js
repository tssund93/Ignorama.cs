var threadID = function () {
    var url = window.location.href.replace(/\/$/, '');
    return url.substr(url.lastIndexOf('/') + 1);
}();

var postsVue = new Vue({
    el: 'main',
    data: {
        posts: [],
        page: 1,
        perPage: 5,
    },
    computed: {
        visiblePosts: function () {
            var startPost = (this.page - 1) * this.perPage;
            return this.posts.slice(startPost, startPost + this.perPage);
        }
    },
    created: function () {
        this.getPosts(threadID);
    },
    filters: {
        date: function (date) {
            if (!date) return '';
            date = new Date(date);
            return date.toLocaleString();
        },
        formatPost: function (post) {
            post = escapeHTML(post);
            post = post.replace(/\n/g, '<br>');

            //images
            post = post.replace(/\[img\](.*?)\[\/img\]/gi, '<a target="_blank" href="$1"><img href="$1" src="$1" class="img img-responsive" style="max-height: 480px;"></img></a>');
            //webm
            post = post.replace(/\[webm\](.*?)\[\/webm\]/gi, '<video preload="none" controls="controls" class="img img-responsive"><source type="video/webm" src="$1"></video>');
            //bold and italics
            post = post.replace(/\[i\](.*?)\[\/i\]/igs, "<i>$1</i>");
            post = post.replace(/\[b\](.*?)\[\/b\]/igs, "<b>$1</b>");
            post = post.replace(/\[u\](.*?)\[\/u\]/igs, "<u>$1</u>");
            //spoilers
            post = post.replace(/\[spoiler\](.*?)\[\/spoiler\]/igs, "<span class='spoiler' style='background-color:#333;'>$1</span>");
            //replies
            post = post.replace(/\[reply[=| ]([0-9]+)\]\R*\[\/reply\]/igs, "<a href='javascript:viewPost($1);'><b>$1</b></a>");
            post = post.replace(/\[reply post=([0-9]+) user=(.*?)\]\R*\[\/reply\]/igs, "<a href='javascript:viewPost($1);'><b>$2</b></a>");
            post = post.replace(/\[reply user=(.*?) post=([0-9]+)\]\R*\[\/reply\]/igs, "<a href='javascript:viewPost($2);'><b>$1</b></a>");
            post = post.replace(/&gt;&gt;([0-9]+)/igs, "<a href='javascript:viewPost($1);'><b>$1</b></a>");
            post = post.replace(/\[reply post=([0-9]+) user=(.*?)\]\R*(.*?)\R*\[\/reply\]\R?/igs, "<div style='padding: 5px;border: 1px solid #DDD;background-color:#F5F5F5'><b><a href='javascript:viewPost($1);'>$2</a> said:</b><br/>$3</div>");
            post = post.replace(/\[reply user=(.*?) post=([0-9]+)\]\R*(.*?)\R*\[\/reply\]\R?/igs, "<div style='padding: 5px;border: 1px solid #DDD;background-color:#F5F5F5'><b><a href='javascript:viewPost($2);'>$1</a> said:</b><br/>$3</div>");
            post = post.replace(/\[reply[=| ]([0-9]+)\]\R*(.*?)\R*\[\/reply\]\R?/igs, "<div style='padding: 5px;border: 1px solid #DDD;background-color:#F5F5F5'><b><a href='javascript:viewPost($1);'>$1</a> said:</b><br/>$2</div>");
            //quotes
            post = post.replace(/\[quote\]\R?(.*?)\R?\[\/quote\]\R?/igs, "<div style='padding: 5px;border: 1px solid #DDD;background-color:#F5F5F5'><b>Quote:</b><br/>$1</div>");
            //code
            post = post.replace(/\[code\]\R*(.*?)\R*\[\/code\]/igs, "<pre><code>$1</code></pre>");
            //colored text
            post = post.replace(/\[color=(.*?)\](.*?)\[\/color\]/g, "<span style='color:$1'>$2</span>");
            //url
            post = post.replace(/\[url=(http(s?):\/\/)?(.*?)\](.*?)\[\/url\]/g, "<a target='_blank' href='http$2://$3'>$4</a>");
            //youtube embed
            post = post.replace(/[a-zA-Z\/\/:\.]*(youtube.com\/watch\?v=|youtu.be\/)([a-zA-Z0-9\-_]+)([a-zA-Z0-9\/\*\-\_\?\&\;\%\=\.]*)/gi, "<div class='flex-video widescreen'><iframe width=\"560\" height=\"315\" src=\"//www.youtube.com/embed/$2\" frameborder=\"0\" allowfullscreen></iframe></div>");
            return post;
        }
    },
    watch: {
        visiblePosts: function (val) {
            this.follow(threadID, Math.max(...val.map(p => p.ID)));
        }
    },
    methods: {
        getPosts: function (threadID) {
            axios.get('/Threads/GetPosts/' + threadID)
                .then(response => {
                    this.posts = response.data;
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
    }
});

function slide() {
    if ($("#quickreply").hasClass("slid-out")) {
        $("#quickreply").animate({ bottom: -222 }, 200).delay(200).queue(function (next) { $(this).attr("class", "slid-in"); next(); });
    }
    else
        $("#quickreply").stop(true).animate({ bottom: -1 }, 200).attr("class", "slid-out");
}

$('#postform').submit(function (e) {
    e.preventDefault();
    $.ajax({
        url: '/Posts/New/' + threadID,
        type: 'post',
        data: $('#postform').serialize(),
        success: function () {
            $('#postfield').val('');
            $('input[name=Bump]').prop('checked', false);
            $('input[name=Anonymous]').prop('checked', false);
            $('input[name=RevealOP]').prop('checked', false);
            slide();

            postsVue.getPosts(threadID);
            postsVue.page = Math.ceil(postsVue.posts.length / postsVue.perPage);
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