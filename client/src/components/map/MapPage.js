import React from 'react';
import ConscienceMap from './ConscienceMap';

import RolesValidation from '../common/RolesValidation';
import Roles from '../../enums/roles';

const MapPage = () =>
  <div>
    <RolesValidation allowed={[Roles.CompanyQA, Roles.Admin]}>
      <ConscienceMap />
    </RolesValidation>
  </div>;

export default MapPage;
