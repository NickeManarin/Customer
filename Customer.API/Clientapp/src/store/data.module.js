import DataService from '../services/data.service';

export const data = {
    namespaced: true,
    state: { },

    actions: {
      getAll({ commit, dispatch }, { path }) {
          return DataService.getAll(path).then(
                async response => {
                    if (response.status === 401) {
                        try {
                            var resp = await dispatch('auth/refresh', null, { root: true } );

                            console.log('Result of refresh', resp);

                            if (resp !== undefined)
                                return await dispatch('data/getAll', null,  { root: true } );
                        } catch (e) {
                            return Promise.reject(e);
                        }
                    }

                    switch (path) {
                        case 'user':
                            commit('getAllUsersSuccess', response);
                            break;

                        case 'gender':
                            commit('getAllGendersSuccess', response);
                            break;
                    
                        case 'classification':
                            commit('getAllClassificationsSuccess', response);
                            break;

                        case 'region':
                            commit('getAllRegionsSuccess', response);
                            break;

                        case 'city':
                            commit('getAllCitiesSuccess', response);
                            break;

                        default:
                            commit('getAllCustomersSuccess', response);
                            break;
                    }
                    
                    return Promise.resolve(response);
                },
                error => {
                    return Promise.reject(error);
                }
          );
      }
    },
  
    mutations: {
        getAllUsersSuccess(state, response) {
            state.userLoadedTimestamp = Date.now();
            state.users = response.users;
        },
        getAllGendersSuccess(state, response) {
            state.genderLoadedTimestamp = Date.now();
            state.genders = response.genders;
        },
        getAllRegionsSuccess(state, response) {
            state.regionLoadedTimestamp = Date.now();
            state.regions = response.regions;
        },
        getAllCitiesSuccess(state, response) {
            state.citieLoadedTimestamp = Date.now();
            state.cities = response.cities;
        },
        getAllClassificationsSuccess(state, response) {
            state.classificationLoadedTimestamp = Date.now();
            state.classifications = response.classifications;
        },
        getAllCustomersSuccess(state, response) {
            state.customerLoadedTimestamp = Date.now();
            state.customers = response.customers;
        },
    }
  };