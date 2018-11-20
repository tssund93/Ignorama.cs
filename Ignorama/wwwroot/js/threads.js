new Vue({
    el: 'main',
    data: { threads: [] },
    created: function () {
        axios.get('/Threads/GetThreads')
            .then(response => this.threads = response.data);
    },
    filters: {
        date: function (date) {
            if (!date) return '';
            date = new Date(date);
            return date.toLocaleString(); 
        }
    }
});