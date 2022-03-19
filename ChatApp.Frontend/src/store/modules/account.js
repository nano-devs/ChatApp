import axios from 'axios'

export default {
    strict: true,
    namespaced: true,
    state: {
        accessToken: localStorage.getItem('accessToken'),
        refreshToken: localStorage.getItem('refreshToken'),
    },
    getters: {

    },
    mutations: {
        updateTokens: (state, data) => {
            state.accessToken = data.accessToken;
            state.refreshToken = data.refreshToken;
            
            localStorage.setItem('accessToken', state.accessToken);
            localStorage.setItem('refreshToken', state.refreshToken);

            axios.defaults.headers.common['Authorization'] = 'Bearer ' + state.accessToken;
        },
        destroyTokens: (state) => {
            state.accessToken = null;
            state.refreshToken = null;

            localStorage.removeItem('accessToken');
            localStorage.removeItem('refreshToken');

            axios.defaults.headers.common['Authorization'] = '';
        }
    },
    actions: {
        login: async (context, payload) => {
            const response = await axios.post('https://localhost:8000/login', payload);
            console.log(response.data);

            context.commit('updateTokens', response.data);
        },
        logout: async (context) => {
            const response = await axios.delete(
                'https://localhost:8000/logout',
            );
            console.log(response.data);

            context.commit('destroyTokens', response.data);
        },
        refresh: async (context, payload) => {
            const response = await axios.post('https://localhost:8000/refresh', payload);
            console.log(response.data);

            context.commit('updateTokens', response.data);
        },
        register: async (context, payload) => {
            // const payload_data = {
            //     "Email": "test@gmail.com",
            //     "Username": "test",
            //     "Password": "testtest",
            //     "ConfirmPassword": "testtest"
            // }

            const response = await axios.post('https://localhost:8000/register', payload);
            console.log(response.data);

            context.commit('updateTokens', response.data);
        },
    },
}
