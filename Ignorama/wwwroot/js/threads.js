var threadsVue = new Vue({
    el: 'main',
    data: {
        threads: [],
        view: this.$cookies.get('view') ? this.$cookies.get('view') : '',
        tags: [],
        selectedTags: []
    },
    created: function () {
        axios.get('/Threads/GetThreads')
            .then(response => {
                this.threads = response.data;
            });
        axios.get('/Tags/GetTags')
            .then(response => {
                this.tags = response.data;
            });
        axios.get('/Tags/GetSelectedTags')
            .then(response => {
                this.selectedTags = response.data;
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
            viewThreads = [];
            if (this.view === '')
                viewThreads = this.threads.filter(thread => !thread.Hidden);
            else if (this.view === 'hidden')
                viewThreads = this.threads.filter(thread => thread.Hidden);
            else if (this.view === 'following')
                viewThreads = this.threads.filter(thread => thread.Following && !thread.Hidden);
            else
                viewThreads = this.threads.filter(thread => !thread.Hidden);

            return viewThreads.filter(thread => this.selectedTags.includes(thread.Tag.ID));
        }
    },
    watch: {
        view: function (val) {
            this.$cookies.set('view', val);
        }
    },
    methods: {
        toggleHidden: function (thread) {
            thread.Hidden = !thread.Hidden;
            axios.post("/Threads/ToggleHidden", { ThreadID: thread.ID })
                .then(response => {
                    console.log("Toggled hidden for thread " + response.data);
                })
                .catch(error => {
                    console.error("Error toggling hidden: " + error);
                });
        },
        unfollow: function (thread) {
            thread.Following = false;
            axios.post("/Threads/Unfollow", { ThreadID: thread.ID })
                .then(response => {
                    console.log("Unfollowed thread " + response.data);
                })
                .catch(error => {
                    console.error("Error unfollowing thread: " + error);
                });
        },
        updateSelectedTag: function (tag) {
            axios.post("/Tags/ToggleSelectedTag", { TagID: tag.ID })
                .then(response => {
                    console.log("Updated selected tag " + response.data);
                })
                .catch(error => {
                    console.error("Error updating selected tag: " + error);
                });
        }
    }
});