import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Suspense, lazy } from 'react';
import HomeComponent from './home/home.component';

// Lazy loading komponentów (jak loadComponent w Angular)
const LoginComponent = lazy(() => import('./login/LoginComponent'));
const UserCheckComponent = lazy(() => import('./user-check/user-check'));
const AuthTestComponent = lazy(() => import('./auth-test/auth-test.component'));

export function App() {
  return (
    <BrowserRouter>
      <HomeComponent />
      
      <Suspense fallback={<div>Loading...</div>}>
        <Routes>
          <Route path="/login" element={<LoginComponent />} />
          <Route path="/auth-test/:type" element={<AuthTestComponent />} />
          <Route path="/usercheck" element={<UserCheckComponent />} />
        </Routes>
      </Suspense>
    </BrowserRouter>
  );
}
