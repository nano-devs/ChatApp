import Vue from 'vue'
import VueRouter from 'vue-router'
import BlankMain from '../components/BlankMain.vue'
import PrivateChat from '../components/PrivateChat.vue'

Vue.use(VueRouter)

const routes = [
    {
        path: '/',
        component: BlankMain,
    },
    {
        path: '/dm/:contactId',
        component: PrivateChat,
    },
    {
        path: '*',
        component: BlankMain,
    }
]

export default new VueRouter({
    mode: 'history',
    routes
})
