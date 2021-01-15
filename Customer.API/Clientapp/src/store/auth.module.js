import AuthService from '../services/auth.service';

const user = JSON.parse(localStorage.getItem('user'));
const initialState = user ? { status: { loggedIn: true }, user } : { status: { loggedIn: false }, user: null };

export const auth = {
  namespaced: true,
  state: initialState,

  actions: {
    signin({ commit }, user) {
        return AuthService.signin(user).then(
            response => {
                commit('loginSuccess', response);
                return Promise.resolve(response);
            },
            error => {
                commit('loginFailure');
                return Promise.reject(error);
            }
        );
    },
    
    refresh({ commit }, user) {
        return AuthService.refresh(user).then(
            response => {
                commit('refreshSuccess', response);
                return Promise.resolve(response);
            },
            error => {
                commit('refreshFailure');
                return Promise.reject(error);
            }
        );
    },

    logout({ commit, dispatch }) {
        return AuthService.logout().then(
            async response => {
                if (response.status === 401) {
                    try {
                        var resp = await dispatch('auth/refresh', null, { root: true } );

                        console.log('Result of refresh', resp);

                        if (resp !== undefined)
                            return await dispatch('auth/logout', null,  { root: true } );
                    } catch (e) {
                        return Promise.reject(e);
                    }
                }

                commit('logout', response);
                return Promise.resolve(response);
            },
            error => {
                return Promise.reject(error);
            });
    },
  },

  mutations: {
    loginSuccess(state, user) {
        state.status.loggedIn = true;
        state.user = user;
    },
    loginFailure(state) {
        state.status.loggedIn = false;
        state.user = null;
    },

    refreshSuccess(state, user) {
        state.status.loggedIn = true;
        state.user = user;
    },
    refreshFailure(state) {
        state.status.loggedIn = false;
        state.user = null;
    },
    
    logout(state) {
        state.status.loggedIn = false;
        state.user = null;
    },
  }
};