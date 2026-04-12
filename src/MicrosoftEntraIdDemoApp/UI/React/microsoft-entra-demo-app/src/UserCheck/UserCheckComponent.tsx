// src/components/usercheck/UserCheckComponent.tsx
import { useEffect, useState } from 'react';
import httpClient from '../shared/api/httpClient';

export default function UserCheckComponent() {
  const [user, setUser] = useState('n/a');
  const [roles, setRoles] = useState<string[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await httpClient.post('/bff/user');
        setUser(response?.data?.name ?? 'n/a');
        setRoles(response?.data?.roles ?? []);
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

  return (
    <div>
      <div>Welcome, {user}!</div>
        <div>
          Roles:
          <ul>
              {roles.map((role) => (
                <li key={role}>{role}</li>
              ))}
          </ul>
        </div>
    </div>
  );
}