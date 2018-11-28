var userID = function () {
    var url = window.location.href.replace(/\/$/, '');
    return url.substr(url.lastIndexOf('/') + 1).replace(/#.*$/, '');
}();

var userHistoryVue = new Vue({
    el: 'main',
    data: {
        posts: [],
        page: 1,
        perPage: 20,
    },
    created: function () {
        this.getPosts(userID);
    },
    methods: {
        getPosts: function (userID) {
            axios.get('/History/GetUserPosts/' + userID)
                .then(response => {
                    this.posts = response.data;
                });
        },
    },
});