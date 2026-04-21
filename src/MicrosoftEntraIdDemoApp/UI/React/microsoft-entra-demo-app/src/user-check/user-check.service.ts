import httpClient from '../shared/api/httpClient';

export const checkUser = async () => {
  try {
    const response = await httpClient.get('/bff/user');
    return response.data;
  } catch (error) {
    console.error('Error checking user:', error);
    throw error;
  }
};