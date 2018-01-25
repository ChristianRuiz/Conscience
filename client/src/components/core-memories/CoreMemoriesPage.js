import React from 'react';
import RolesValidation from '../common/RolesValidation';
import CoreMemories from './CoreMemories';
import Roles from '../../enums/roles';

const CoreMemoriesPage = () =>
  <div>
    <RolesValidation allow={Roles.Admin}>
      <CoreMemories />
    </RolesValidation>
  </div>;

export default CoreMemoriesPage;
