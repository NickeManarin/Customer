import CustomerService from '../services/customer.service';

export const customer = {
    namespaced: true,
    state: { },

    actions: {
      getCustomers({ commit, dispatch }, { params }) {
          return CustomerService.getCustomers(params).then(
                async response => {
                    if (response.status === 401) {
                        try {
                            var resp = await dispatch('auth/refresh', null, { root: true } );

                            console.log('Result of refresh', resp);

                            if (resp !== undefined)
                                return await dispatch('customer/getCustomers', { params }, { root: true } );
                        } catch (e) {
                            return Promise.reject(e);
                        }
                    }

                    commit('getAllCustomersSuccess', response);
                    
                    return Promise.resolve(response);
                },
                error => {
                    return Promise.reject(error);
                }
          );
      }
    },
  
    mutations: {
        getAllCustomersSuccess(state, response) {
            state.customerLoadedTimestamp = Date.now();
            state.customers = response.customers;
        },
    }
  };