// src/components/usercheck/UserCheckComponent.tsx
import { useEffect, useState } from 'react';
import httpClient from '../shared/api/httpClient';

export default function UserCheckComponent() {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await httpClient.get('/api/user');
        setUser(response.data);
      } catch (error) {
        console.error('Error fetching user:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchUser();
  }, []); // [] = tylko raz przy montowaniu, jak ngOnInit

  if (loading) return <div>Loading...</div>;
  if (!user) return <div>No user data</div>;

  return <div>Welcome, {user}!</div>;
}