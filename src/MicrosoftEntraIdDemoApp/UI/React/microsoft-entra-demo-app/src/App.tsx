import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { Suspense, lazy } from 'react';

// Lazy loading komponentów (jak loadComponent w Angular)
const LoginComponent = lazy(() => import('./login/LoginComponent'));
const UserCheckComponent = lazy(() => import('./UserCheck/UserCheckComponent'));

export function App() {
  return (
    <BrowserRouter>
      <Suspense fallback={<div>Loading...</div>}>
        <Routes>
          <Route path="/login" element={<LoginComponent />} />
          <Route path="/usercheck" element={<UserCheckComponent />} />
          <Route path="/" element={<Navigate to="/login" replace />} />
        </Routes>
      </Suspense>
    </BrowserRouter>
  );
}
