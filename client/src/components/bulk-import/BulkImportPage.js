import React from 'react';
import RolesValidation from '../common/RolesValidation';
import BulkImport from './BulkImport';
import Roles from '../../enums/roles';

const BulkImportPage = () =>
  <div>
    <RolesValidation allow={Roles.Admin}>
      <BulkImport />
    </RolesValidation>
  </div>;

export default BulkImportPage;
