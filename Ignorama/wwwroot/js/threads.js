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
                        hidden: false,
                        following: true,
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
                return this.threads.filter(thread => !thread.hidden);
            else if (this.view === 'hidden')
                return this.threads.filter(thread => thread.hidden);
            else if (this.view === 'following')
                return this.threads.filter(thread => thread.following && !thread.hidden);
            else
                return this.threads.filter(thread => !thread.hidden);
        }
    },
    methods: {
        hideThread: function (thread) {
            thread.hidden = true
        },
        unhideThread: function (thread) {
            thread.hidden = false
        }
    }
});