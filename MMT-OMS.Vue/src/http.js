import axios from 'axios';
import { router } from './router.js';

axios.defaults.headers.common['Authorization'] = 'Bearer ' + localStorage.getItem('access_key');
axios.interceptors.response.use(function(res){
    return res;
}, function(err){
    if(err.response.status == 401){
        localStorage.removeItem('access_key');
        router.push({ name : 'login' });
    }
    return Promise.reject(err);
})

export default axios;