import Vue from 'vue';
import Vuex from 'vuex';

import { auth } from './auth.module';
import { data } from './data.module';
import { customer } from './customer.module';

Vue.use(Vuex);

export default new Vuex.Store({
  modules: {
    auth,
    data,
    customer
  }
});