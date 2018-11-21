var threadsVue = new Vue({
    el: 'main',
    data: {
        threads: [],
        view: '',
    },
    created: function () {
        axios.get('/Threads/GetThreads')
            .then(response => {
                this.threads = response.data.map(thread =>
                    ({
                        ...thread,
                        Following: true,
                    }));
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
    methods: {
        toggleHidden: function (thread) {
            axios.post("/Threads/ToggleHidden", { ThreadID: thread.ID })
                .then(response => {
                    console.log("Toggle hidden for thread " + response.data);
                })
                .catch(error => {
                    console.error("Error toggling hidden: " + error);
                });
            thread.Hidden = !thread.Hidden;
        }
    }
});