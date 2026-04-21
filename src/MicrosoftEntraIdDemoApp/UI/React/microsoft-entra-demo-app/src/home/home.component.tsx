import { Link, Outlet } from 'react-router-dom';
import './home.component.css';

export default function HomeComponent() {
  return (
    <>
      <nav className="app-nav">
        <ul>
          <li><Link to="/login">Login</Link></li>
          <li><Link to="/usercheck">User Check</Link></li>
          <li><Link to="/auth-test/checkGroup">Check Group</Link></li>
          <li><Link to="/auth-test/apptesters">App Testers</Link></li>
          <li><Link to="/auth-test/test">Test Group</Link></li>
          <li><Link to="/auth-test/admin">Admin Group</Link></li>
        </ul>
      </nav>

      <Outlet />
    </>
  );
}