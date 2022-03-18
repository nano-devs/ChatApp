import Vue from 'vue'
import VueRouter from 'vue-router'
import BlankMain from '../components/chats/BlankMain.vue'
import ChatLayout from '../components/chats/Layout.vue'
import PrivateChat from '../components/chats/PrivateChat.vue'
import Login from '../components/authentications/Login.vue'
import NotFound from '../components/errors/NotFound.vue'

Vue.use(VueRouter)

const routes = [
    {
        path: '/',
        component: {template: '<h1>This is home page</h1>'}
    },
    {
        path: '/dm',
        component: ChatLayout,
        children: [
            {
                path: ':contactId',
                component: PrivateChat
            },
            {
                path: '*',
                component: BlankMain
            },
        ]
    },
    {
        path: '/login',
        component: Login,
    },
    {
        path: '*',
        component: NotFound,
    },
]

export default new VueRouter({
    mode: 'history',
    routes
})
