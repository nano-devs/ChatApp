import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        accessToken: null,
        refreshToken: null,
    },
    getters: {

    },
    mutations: {
        updateTokens: (state, data) => {
            state.accessToken = data.accessToken;
            state.refreshToken = data.refreshToken;

            axios.defaults.headers.common['Authorization'] = 'Bearer ' + response.data.token;
        },
        destroyTokens: (state) => {
            state.accessToken = null;
            state.refreshToken = null;

            axios.defaults.headers.common['Authorization'] = '';
        }
    },
    actions: {
        login: async (context, payload) => {
            const response = await axios.post('http://localhost:8000/login', payload);
            console.log(response.data);

            context.commit('updateTokens', response.data);
        },
        logout: async (context, payload) => {
            const response = await axios.delete(
                'http://localhost:8000/logout',
            );
            console.log(response.data);

            context.commit('destroyTokens', response.data);
        },
        refresh: async (context, payload) => {
            const response = await axios.post('http://localhost:8000/refresh', payload);
            console.log(response.data);

            context.commit('updateTokens', response.data);
        },
        register: async (context, payload) => {
            const payload_data = {
                "Email": "test@gmail.com",
                "Username": "test",
                "Password": "testtest",
                "ConfirmPassword": "testtest"
            }

            const response = await axios.post('http://localhost:8000/register', payload);
            console.log(response.data);

            context.commit('updateTokens', response.data);
        },
    },
})
