import Vue from 'vue'
import VueRouter from 'vue-router';
import App from './App.vue'
import vuetify from './plugins/vuetify';
import { router } from './router.js'
import axios from './http.js'
import swal from 'sweetalert2/dist/sweetalert2.js'

Vue.config.productionTip = false
Vue.prototype.$axios = axios;
Vue.prototype.$swal = swal;
Vue.use(VueRouter);

new Vue({
  vuetify,
  router,
  render: h => h(App)
}).$mount('#app')
