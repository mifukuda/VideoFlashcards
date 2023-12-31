import axios from 'axios';

axios.defaults.withCredentials = true;
const api = axios.create({
    baseURL: 'http://localhost:5000',
})

// export const loginUser = (payload, token) => api.post('Auth/Login', payload, {headers: {'Authorization': 'Bearer ' + token}});
export const loginUser = (payload) => api.post('Auth/Login', payload);
export const registerUser = (payload) => api.post('Auth/Register', payload);

const apis = {
    loginUser,
    registerUser
}

export default apis;