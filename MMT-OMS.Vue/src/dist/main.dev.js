"use strict";

var _vue = _interopRequireDefault(require("vue"));

var _App = _interopRequireDefault(require("./App.vue"));

var _vuetify = _interopRequireDefault(require("./plugins/vuetify"));

var _vueRouter = _interopRequireDefault(require("vue-router"));

var _Login = _interopRequireDefault(require("./components/Login.vue"));

var _Order = _interopRequireDefault(require("./components/Order.vue"));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { "default": obj }; }

// routes
_vue["default"].use(_vueRouter["default"]);

_vue["default"].config.productionTip = false;
var routes = [{
  path: '/order',
  component: _Order["default"]
}, {
  path: '/login',
  component: _Login["default"]
}];
var router = new _vueRouter["default"]({
  routes: routes
});
new _vue["default"]({
  vuetify: _vuetify["default"],
  router: router,
  render: function render(h) {
    return h(_App["default"]);
  }
}).$mount('#app');