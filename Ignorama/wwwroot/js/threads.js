var threadsVue = new Vue({
    el: 'main',
    data: { threads: [] },
    created: function () {
        axios.get('/Threads/GetThreads')
            .then(response => {
                this.threads = response.data.map(thread =>
                    ({
                        ...thread,
                        hidden: false
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
            return this.threads.filter(thread => !thread.hidden);
        }
    },
    methods: {
        hideThread: function (thread) {
            thread.hidden = true
        }
    }
});