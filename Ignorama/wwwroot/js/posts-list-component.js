Vue.component('posts-list', {
    props: ['posts', 'perPage', 'page', 'highlightedId'],
    data: function () {
        return {
            banReasons: [],
        }
    },
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
    created: function () {
        axios.get('/Bans/GetReasons')
            .then(response => {
                this.banReasons = response.data;
            });
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
        <post :post="post" :highlighted="post.ID == highlightedId" :ban-reasons="banReasons"></post>
    </div>
</div>
`
})