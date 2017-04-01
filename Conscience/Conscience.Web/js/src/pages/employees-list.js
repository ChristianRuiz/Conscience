import Relay from 'react-relay'

import EmployeesList from '../components/employees-list'
import ConsciencePage from './conscience-page'

class EmployeesRoute extends Relay.Route {
    static routeName = 'Employees';
    static queries = {
        employees: (Component) => Relay.QL`
        query EmployeesQuery {
            employees {
                ${Component.getFragment('employees')}
            }
        }
        `
    }
}

new ConsciencePage(EmployeesList, EmployeesRoute);