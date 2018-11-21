var postsVue = new Vue({
    el: 'main',
    data: {
        posts: [],
    },
    created: function () {
        var url = window.location.href.replace(/\/$/, '');
        var postID = url.substr(url.lastIndexOf('/') + 1);
        axios.get('/Threads/GetPosts/' + postID)
            .then(response => {
                this.posts = response.data;
            });
    },
    filters: {
        date: function (date) {
            if (!date) return '';
            date = new Date(date);
            return date.toLocaleString();
        }
    }
});