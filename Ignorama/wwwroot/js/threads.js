var threadsVue = new Vue({
    el: 'main',
    data: {
        threads: [],
        view: this.$cookies.get('view') ? this.$cookies.get('view') : '',
    },
    created: function () {
        axios.get('/Threads/GetThreads')
            .then(response => {
                this.threads = response.data;
            });
    },
    filters: {
        date: function (date) {
            if (!date) return '';
            date = new Date(date);
            return date.toLocaleString();
        }
    },
    computed: {
        visibleThreads: function () {
            if (this.view === '')
                return this.threads.filter(thread => !thread.Hidden);
            else if (this.view === 'hidden')
                return this.threads.filter(thread => thread.Hidden);
            else if (this.view === 'following')
                return this.threads.filter(thread => thread.Following && !thread.Hidden);
            else
                return this.threads.filter(thread => !thread.Hidden);
        }
    },
    watch: {
        view: function (val) {
            this.$cookies.set('view', val);
        }
    },
    methods: {
        toggleHidden: function (thread) {
            axios.post("/Threads/ToggleHidden", { ThreadID: thread.ID })
                .then(response => {
                    console.log("Toggled hidden for thread " + response.data);
                })
                .catch(error => {
                    console.error("Error toggling hidden: " + error);
                });
            thread.Hidden = !thread.Hidden;
        },
        follow: function (thread, lastSeenID) {
            axios.post("/Threads/Follow", { ThreadID: thread.ID, LastSeenPostID: lastSeenID })
                .then(response => {
                    console.log("Followed thread " + response.data);
                })
                .catch(error => {
                    console.error("Error following thread: " + error);
                });
            thread.Following = true;
        },
        unfollow: function (thread) {
            axios.post("/Threads/Unfollow", { ThreadID: thread.ID })
                .then(response => {
                    console.log("Unfollowed thread " + response.data);
                })
                .catch(error => {
                    console.error("Error unfollowing thread: " + error);
                });
            thread.Following = false;
        }
    }
});