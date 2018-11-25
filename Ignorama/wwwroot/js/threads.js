var threadsVue = new Vue({
    el: 'main',
    data: {
        threads: [],
        view: this.$cookies.get('view') ? this.$cookies.get('view') : '',
        tags: [],
        selectedTags: [],
        search: "",
    },
    mixins: [dateMixin],
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
    computed: {
        visibleThreads: function () {
            var viewThreads = [];
            if (this.view === '')
                viewThreads = this.threads.filter(thread => !thread.Hidden)
                    .sort((t1, t2) => t2.Stickied - t1.Stickied);
            else if (this.view === 'hidden')
                viewThreads = this.threads.filter(thread => thread.Hidden);
            else if (this.view === 'following')
                viewThreads = this.threads.filter(thread => thread.Following && !thread.Hidden)
                    .sort((t1, t2) => new Date(t2.LastPost.Time)
                                        - new Date(t1.LastPost.Time));
            else
                viewThreads = this.threads.filter(thread => !thread.Hidden);

            var searchPattern = new RegExp(".*?" + this.search.replace(/\s+/ig, ".*?") + ".*?", "i");
            return viewThreads
                .filter(thread => {
                    return (!this.selectedTags.length
                        ? true
                        : this.selectedTags.includes(thread.Tag.ID))
                      && (searchPattern.test(thread.Title + ' ' + thread.FirstPost.Text));
                    });
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
            return false;
        },
        toggleStickied: function (thread) {
            thread.Stickied = !thread.Stickied;
            axios.post("/Threads/ToggleStickied/" + thread.ID)
                .then(response => {
                    console.log("Toggled stickied for thread " + response.data);
                })
                .catch(error => {
                    console.error("Error toggling stickied: " + error);
                });
        },
        toggleLocked: function (thread) {
            thread.Locked = !thread.Locked;
            axios.post("/Threads/ToggleLocked/" + thread.ID)
                .then(response => {
                    console.log("Toggled locked for thread " + response.data);
                })
                .catch(error => {
                    console.error("Error toggling locked: " + error);
                });
            return false;
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
            return false;
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