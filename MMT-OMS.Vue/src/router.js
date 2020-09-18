import VueRouter from 'vue-router';

import loginView from './views/Login.vue';
import orderView from './views/Order.vue';
import main from './views/Main.vue';
import customerOrderGrid from './views/CustomerOrderGrid.vue';
import customerOrderCapture from './views/CustomerOrderCapture.vue';
import customerOrderDetails from './views/CustomerOrderDetails.vue';
import customerOrderShades from './views/CustomerOrderShades.vue';
import customerOrderPayments from './views/CustomerOrderPayments.vue';
import printLayoutCapture from './views/PrintLayout.vue';
import paymentView from './views/Payment.vue';

export const routeNames = {
  main : 'main',
  customerOrders: 'customer-orders',
  customerOrder: 'customer-order',
  customerOrderDetails: 'customer-order-details',
  customerOrderShades: 'customer-order-shades',
  customerOrderPayments: 'customer-order-payments',
  printLayouts: 'print-layouts',
  orderForm: 'order-form',
  login: 'login',
  logout: 'logout',
  payment: 'payment'
}

const routes = [
    {
      path : '/',
      component : main,
      name : routeNames.main,
      meta: {
        requiresAuth: true,
      },
      children : [
        {
          path : 'customer-orders',
          component : customerOrderGrid,
          name : routeNames.customerOrders,
          meta: {
            requiresAuth: true,
          }
        },
        {
          path : 'customer-order/:id',
          component : customerOrderCapture,
          name : routeNames.customerOrder,
          meta: {
            requiresAuth: true,
          },
          children: [
            {
              path : 'details',
              component: customerOrderDetails,
              name : routeNames.customerOrderDetails
            },
            {
              path: 'shades',
              component: customerOrderShades,
              name: routeNames.customerOrderShades
            },
            {
              path: 'payments',
              component: customerOrderPayments,
              name: routeNames.customerOrderPayments
            }
          ]
        },
        {
          path : 'print-layouts',
          component : printLayoutCapture,
          name : routeNames.printLayouts,
          meta: {
            requiresAuth: true,
          }
        },
      ]
    },
    { path : '/order-form', component : orderView, name : routeNames.orderForm },
    { path : '/login', component : loginView, name : routeNames.login },
    { path : '/logout', name : routeNames.logout,
        beforeEnter: (to, from, next) => {
            localStorage.removeItem('access_key');
            next({ name: 'login' });
        }
    },
    { path : '/payment', component : paymentView, name : routeNames.payment },
  ];
  
export const router = new VueRouter({ routes });
router.beforeEach((to, from, next) => {
    if (to.matched.some((record) => record.meta.requiresAuth)) {
        if(!isAuthenticated()){
            next({ name : 'login' })
        }else{
            next();
        }
    } else {
        next()
    }
});

function isAuthenticated(){
    return !!localStorage.getItem('access_key');
}