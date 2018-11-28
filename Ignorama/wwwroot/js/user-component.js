Vue.component('user', {
    props: ['user', 'ip', 'anonymous', 'detailedView'],
    template: `
<span>
    <span v-if="detailedView">
        <span v-if="user">
            <a :href="'/History/ByUser/' + user.Id">{{ user.UserName }}</a>
            (<a :href="'/History/ByIP/' + ip">{{ ip }}</a>)
            <span v-if="anonymous || !user">| Anonymous</span>
        </span>
        <span v-else>
            <a :href="'/History/ByIP/' + ip">{{ ip }}</a> | Anonymous
        </span>
    </span>
    <span v-else>
        <span v-if="anonymous || !user">Anonymous</span>
        <span v-else>{{ user.UserName }}</span>
    </span>
</span>
`
})