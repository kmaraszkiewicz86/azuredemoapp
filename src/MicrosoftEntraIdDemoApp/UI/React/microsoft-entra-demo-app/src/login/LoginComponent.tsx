import { useLogin } from './loginService';

export default function LoginComponent() {
  const login = useLogin();

  return (
    <div>
      <h1>Microsoft Entra Login</h1>
      <button onClick={login}>Login</button>
    </div>
  );
}