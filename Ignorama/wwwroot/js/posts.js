﻿var threadID = function () {
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
        },
        error: function (_, e) {
            console.log($('#postform').serialize())
            alert('Unable to submit reply: ' + e);
        }
    });
});