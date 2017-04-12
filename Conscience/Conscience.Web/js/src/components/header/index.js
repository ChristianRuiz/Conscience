import React from 'react';
import { Link } from 'react-router-dom';
import LogoutButton from '../login/LogoutButton';

const Header = () =>
  <div>
    <Link to="/" >Employees</Link>
    <Link to="/hosts" >Hosts</Link>
    <Link to="/map" >Map</Link>
    <LogoutButton />
  </div>;

export default Header;
