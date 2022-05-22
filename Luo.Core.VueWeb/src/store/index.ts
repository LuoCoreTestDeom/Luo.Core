import { createStore} from 'vuex'

export default createStore({
    state: {
        token: localStorage.getItem('token') ? localStorage.getItem('token') : ''
    },
    mutations:{
        setToken (state,token) {
            state.token =token;
            localStorage.setItem("token",token.token);     //存储token
        },
        delToken (state) {
            state.token = '';
            localStorage.removeItem("token");    //删除token
        }
    }
});
  
