Vue.component('posts-list', {
    props: ['posts', 'perPage', 'page'],
    computed: {
        visiblePosts: function () {
            var startPost = (this.page - 1) * this.perPage;
            return this.posts.slice(startPost, startPost + this.perPage);
        }
    },
    watch: {
        page: function () {
            window.scrollTo(0, 0);
        }
    },
    template: `
<div>
    <div class="row constrained" v-if="!posts.length">
        <div class="col-xs-12 thread loading">
            <img src="/loading.png" />
        </div>
    </div>
    <div class="row constrained" v-else-if="!visiblePosts.length" v-cloak>
        <div class="col-xs-12 thread">
            Nothing to see here!
        </div>
    </div>
    <div v-else class="row constrained" v-for="post in visiblePosts">
        <post :post="post"></post>
    </div>
</div>
`
})