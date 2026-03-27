import axios from 'axios';
import { environment } from '../environment';

const httpClient = axios.create({
  baseURL: environment.apiHost,
  withCredentials: true, // WAŻNE! Jak w Angular
});

// Response interceptor - obsługa 401
httpClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      console.warn('Session expired. Redirecting to login...');
      window.location.href = `${environment.uiHost}/login`;
    }
    return Promise.reject(error);
  }
);

export default httpClient;