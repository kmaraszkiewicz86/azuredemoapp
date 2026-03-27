import { useCallback } from 'react';
import { environment } from '../shared/environment';

export const useLogin = () => {
  return useCallback(() => {
    window.location.href = `${environment.apiHost}/login`;
  }, []);
};