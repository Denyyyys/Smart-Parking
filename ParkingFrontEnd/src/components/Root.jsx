import { Outlet } from 'react-router-dom';
import NavBar from './NavBar';

function Root() {
  return (
    <>
      <Outlet />
    </>
  );
}

export default Root;
