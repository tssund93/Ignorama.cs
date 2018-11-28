Vue.component('user', {
    props: ['user', 'ip', 'anonymous', 'detailedView'],
    template: `
<span>
    <span v-if="detailedView">
        <span v-if="user">
            <b><a :href="'/History/ByUser/' + user.Id">{{ user.UserName }}</a></b>
            (<a :href="'/History/ByIP/' + ip">{{ ip }}</a>)
            <span v-if="anonymous || !user">| Anonymous</span>
        </span>
        <span v-else>
            <b><a :href="'/History/ByIP/' + ip">{{ ip }}</a></b> | Anonymous
        </span>
    </span>
    <span v-else>
        <b v-if="anonymous || !user">Anonymous</b>
        <b v-else>{{ user.UserName }}</b>
    </span>
</span>
`
})