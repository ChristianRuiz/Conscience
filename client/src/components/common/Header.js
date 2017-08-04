import React from 'react';
import { Link } from 'react-router-dom';

import RolesValidation from './RolesValidation';
import Roles from '../../enums/roles';
import LogoutButton from '../login/LogoutButton';

import styles from '../../styles/components/common/header.css';

const Header = () =>
  <div className="menuContainer">
    <RolesValidation forbidden={[Roles.Host]}>
      <div>
        <Link className="logo" to="/" ><img alt="Aleph" src="/content/images/menu/logo.png" /></Link>
        <div className="menuLinks">
          <Link to="/plots" ><img alt="Plot" src="/content/images/menu/plot.png" /></Link>
          <Link to="/characters" ><img alt="Character" src="/content/images/menu/character.png" /></Link>
          <Link to="/behaviour" ><img alt="Behaviour" src="/content/images/menu/behaviour.png" /></Link>
          <Link to="/maintenance" ><img alt="Maintenance" src="/content/images/menu/maintenance.png" /></Link>
          <RolesValidation allowed={[Roles.Admin, Roles.CompanyAdmin, Roles.CompanyQA]}>
            <Link to="/security" ><img alt="Security" src="/content/images/menu/security.png" /></Link>
          </RolesValidation>
          <img className="notifications" alt="Notifications" src="/content/images/menu/nots.png" />
        </div>
      </div>
    </RolesValidation>
    <RolesValidation allowed={[Roles.Host]}>
      <Link to="/client-test" >Client test</Link>
    </RolesValidation>
    {/* <RolesValidation allowed={[Roles.Admin]}>
      <Link to="/bulk-import" >Bulk import</Link>
    </RolesValidation> */}
    {/* <LogoutButton /> */}
  </div>;

export default Header;
