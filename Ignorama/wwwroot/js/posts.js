var threadID = function () {
    var url = window.location.href.replace(/\/$/, '');
    return url.substr(url.lastIndexOf('/') + 1);
}();

var postsVue = new Vue({
    el: 'main',
    data: {
        posts: [],
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
    methods: {
        getPosts: function (threadID) {
            axios.get('/Threads/GetPosts/' + threadID)
                .then(response => {
                    this.posts = response.data;
                });
        }
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