// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

new Vue({
    el: 'main',
    computed: {
        threads: function () {
            return [
                {
                    id: 1,
                    title: "test thread",
                    stickied: false,
                    locked: true,
                    deleted: false
                },
                {
                    id: 2,
                    title: "another test thread",
                    stickied: true,
                    locked: true,
                    deleted: false
                }
            ]
        }
    },
});