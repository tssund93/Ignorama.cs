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
        hideThread: function (thread) {
            console.log(thread.ID);
            axios.post("/Threads/Hide", { ThreadID: thread.ID })
                .then(response => {
                    console.log("Hid thread " + response.data);
                })
                .catch(error => {
                    console.log("Error hiding thread: " + error);
                });
            thread.Hidden = true;
        },
        unhideThread: function (thread) {
            thread.Hidden = false;
        }
    }
});