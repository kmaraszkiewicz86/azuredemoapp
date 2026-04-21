import httpClient from '../shared/api/httpClient';
import axios from 'axios';

export const getData = async (type: string) => {
  try {
    const response = await httpClient.get(`/api/${type}`);
    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      const status = error.response?.status;
      
      if (status === 403) {
        throw new Error('403 - Forbidden');
      }
    }
    
    throw error;
  }
}