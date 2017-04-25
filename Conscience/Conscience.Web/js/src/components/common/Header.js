import React from 'react';
import { Link } from 'react-router-dom';

import RolesValidation from './RolesValidation';
import Roles from '../../enums/roles';
import LogoutButton from '../login/LogoutButton';

const Header = () =>
  <div>
    <RolesValidation forbidden={[Roles.Host]}>
      <div>
        <Link to="/" >Map</Link>
        <Link to="/hosts" >Hosts</Link>
        <RolesValidation allowed={[Roles.Admin, Roles.CompanyAdmin, Roles.CompanyQA]}>
          <Link to="/employees" >Employees</Link>
        </RolesValidation>
      </div>
    </RolesValidation>
    <RolesValidation allowed={[Roles.Host]}>
      <Link to="/client-test" >Client test</Link>
    </RolesValidation>
    <LogoutButton />
  </div>;

export default Header;
