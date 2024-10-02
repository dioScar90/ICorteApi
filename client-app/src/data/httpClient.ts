import axios, { AxiosInstance } from 'axios'

export type HttpClient = AxiosInstance

const httpClient = axios.create({
  baseURL: import.meta.env.BASE_URL,
  withCredentials: true,
})

/*
There is no need to set 'config.headers.Authorization = `Bearer ${token}`' because
once we're using cookies and 'withCredentials: true' the token will automatically
be sent.
*/

httpClient.interceptors.response.use(
  response => response,
  (error) => {
    const isAuthError = error.config.url !== '/auth/login' && error.response.status === 401

    if (!isAuthError) {
      return Promise.reject(error)
    }
    
    // const { state } = getStorage('auth-storage')
    // removeStorage('token', 'auth-storage')
    
    // const isAdmin = state?.user?.role?.name === 'ADMIN'
    // location.href = isAdmin ? '/dashboard/login' : '/login'
  }
)

export { httpClient }
