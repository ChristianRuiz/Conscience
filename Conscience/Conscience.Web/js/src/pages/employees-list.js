import React from 'react';
import ConsciencePage from './conscience-page';
import Header from '../components/header';
import EmployeesList from '../components/employees-list';

const EmployeesPage = () => <div>
  <div><Header /></div>
  <EmployeesList />
</div>;

new ConsciencePage(EmployeesPage);
