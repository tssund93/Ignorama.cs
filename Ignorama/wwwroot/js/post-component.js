Vue.component('post', {
    props: ['post', 'highlighted', 'banReasons'],
    mixins: [dateMixin],
    methods: {
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
        deletePost: function (postID) {
            axios.post("/Posts/Delete/" + postID)
                .then(response => {
                    console.log("Deleted post " + response.data);
                    postsVue.getPosts(threadID);
                })
                .catch(error => {
                    console.error("Error deleting post: " + error);
                });
        },
        restorePost: function (postID) {
            axios.post("/Posts/Restore/" + postID)
                .then(response => {
                    console.log("Restored post " + response.data);
                    postsVue.getPosts(threadID);
                })
                .catch(error => {
                    console.error("Error restoring post: " + error);
                });
        },
        purgePost: function (postID) {
            if (confirm("Are you sure you'd like to permanently delete this post?")) {
                axios.post("/Posts/Purge/" + postID)
                    .then(response => {
                        console.log("Purged post " + response.data);
                        postsVue.getPosts(threadID);
                    })
                    .catch(error => {
                        console.error("Error purging post: " + error);
                    });
            }
        },
    },
    filters: {
        formatPost: function (post) {
            post = escapeHTML(post);

            //youtube embed
            post = post.replace(/[a-zA-Z\/\/:\.]*(youtube.com\/watch\?v=|youtu.be\/)([a-zA-Z0-9\-_]+)([a-zA-Z0-9\/\*\-\_\?\&\;\%\=\.]*)/gi, "<div class='flex-video widescreen'><iframe width=\"560\" height=\"315\" src=\"//www.youtube.com/embed/$2\" frameborder=\"0\" allowfullscreen></iframe></div>");
            //url
            post = post.replace(/(^|\s|\()((?:([a-z][\w-]+):(?:\/{1,3}|[a-z0-9%])|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,4}\/)(?:[^\s()<>]+|\(([^\s()<>]+|(\([^\s()<>]+\)))*\))+(?:\(([^\s()<>]+|(\([^\s()<>]+\)))*\)|[^\s`!()\[\]{};:'".,<>?«»“”‘’]))\b/ig, function (match, p1, p2, p3) {
                return p1 + "<a target='_blank' href='" + (p3 ? p2 : "http://" + p2) + "'>" + p2 + "</a>"
            });
            post = post.replace(/\[url=(http(s?):\/\/)?(.*?)\](.*?)\[\/url\]/ig, "<a target='_blank' href='http$2://$3'>$4</a>");
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
            post = post.replace(/\[(.*?)\|([0-9]+)\]/ig, "<a href='#' onclick='postsVue.viewPost($2);'><b>$1</b></a>");
            //replies
            post = post.replace(/\[reply user=(.*?) post=([0-9]+)\]\s*([\s\S]*?)\s*\[\/reply\]\s?/ig, "<div style='padding: 5px;border: 1px solid #DDD;background-color:#F5F5F5'><b><a href='#' onclick='postsVue.viewPost($2);'>$1</a> said:</b><br/>$3</div>");
            //quotes
            post = post.replace(/\[quote\]\s?([\s\S]*?)\s?\[\/quote\]\s?/ig, "<div style='padding: 5px;border: 1px solid #DDD;background-color:#F5F5F5'><b>Quote:</b><br/>$1</div>");
            //code
            post = post.replace(/\[code\]\s*([\s\S]*?)\s*\[\/code\]/ig, "<pre><code>$1</code></pre>");
            //colored text
            post = post.replace(/\[color=(.*?)\]([\s\S]*?)\[\/color\]/ig, "<span style='color:$1'>$2</span>");

            post = post.replace(/\n/g, '<br>');
            return post;
        },
    },
    template: `
<div :id="'post' + post.ID" class="col-xs-12 thread" :class="{ seen: post.Seen, highlighted: highlighted }" v-cloak>
    <div class="post-info">
        <user :user="post.User" :ip="post.IP" :anonymous="post.Anonymous" :detailed-view="post.Roles.includes('Moderator')" :banned="post.UserBans.length != 0" :ip-banned="post.IPBans.length != 0" :banned-post-id="post.ID"></user>
        <span v-if="post.RevealOP">| OP</span>
        <span v-else-if="post.Bump">| Bump</span>
        <span v-if="post.Roles.includes('Moderator')" class="btn-group thread-dropdown">
            <a class="btn btn-default btn-xs dropdown-toggle" data-toggle=dropdown>
                <span class=caret>
                </span>
            </a>
            <ul class="dropdown-menu pull-right">
                <li v-if="post.Roles.includes('Moderator')" v-for="reason in banReasons">
                    <a href="#" :onclick="'if (confirm(\\'Ban user for ' + reason.Text + '?\\')) banUser(' + post.ID + ', ' + reason.ID + ', event)'">Ban User for {{ reason.Text }}</a>
                </li>
                <li v-if="post.Roles.includes('Moderator')">
                    <a :href="'/Bans/New/' + post.ID">Ban User for Custom reason</a>
                </li>
                <li v-if="post.Roles.includes('Moderator') && !post.Deleted">
                    <a href="#" v-on:click.prevent="deletePost(post.ID)">Delete Post</a>
                </li>
                <li v-else-if="post.Roles.includes('Moderator')">
                    <a href="#" v-on:click.prevent="restorePost(post.ID)">Restore Post</a>
                </li>
                <li v-if="post.Roles.includes('Admin')">
                    <a href="#" v-on:click.prevent="purgePost(post.ID)">Purge Post</a>
                </li>
            </ul>
        </span>
        <span class="time">{{ post.Time | date }}</span>
        <br />
        <span v-if="!post.Locked && !post.Banned">
            <a href="#" v-on:click.prevent="reply(post, true)">reply</a>
            |
            <a href="#" v-on:click.prevent="reply(post)">reference poster</a>
        </span>
    </div>
    <br />
    <div v-html="$options.filters.formatPost(post.Text)"></div>
    <br v-if="post.AllBans.length" />
    <div v-for="ban in post.AllBans">
        <a :href="'/Bans/View/' + post.ID">
            <b>(User was banned for this post)</b>
        </a>
    </div>
</div>
`
})

var escape = document.createElement('textarea');
function escapeHTML(html) {
    escape.textContent = html;
    return escape.innerHTML;
}

function unescapeHTML(html) {
    escape.innerHTML = html;
    return escape.textContent;
}