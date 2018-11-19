new Vue({
    el: 'main',
    data: { threads: [] },
    created: function () {
        axios.get('/Threads/GetThreads')
            .then(response => this.threads = response.data);
    },
});