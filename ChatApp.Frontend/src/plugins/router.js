import Vue from 'vue'
import VueRouter from 'vue-router'
import PrivateChat from '../components/PrivateChat.vue'

Vue.use(VueRouter)

const routes = [
    {
        path: '/pm',
        component: PrivateChat
    },
]

export default new VueRouter({
    mode: 'history',
    routes
})
