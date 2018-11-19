// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

new Vue({
    el: 'main',
    data: { threads: [] },
    created: function() {
            axios.get('/Threads/GetThreads')
                .then(response => this.threads = response.data);
    },
});