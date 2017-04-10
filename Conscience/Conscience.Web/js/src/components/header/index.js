import React from 'react';
import { Link } from 'react-router-dom';
import LogoutButton from '../logout-button';

const Header = () =>
  <div>
    <Link to="/" >Employees</Link>
    <Link to="/hosts" >Hosts</Link>
    <LogoutButton />
  </div>;

export default Header;
