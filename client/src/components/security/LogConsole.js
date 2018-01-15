import React from 'react';
import { Link } from 'react-router-dom';

import ScrollableContainer from '../common/ScrollableContainer';
import RolesValidation from '../common/RolesValidation';
import Roles from '../../enums/roles';

import formatDate from '../../utls/DateFormatter';

import styles from '../../styles/components/security/logConsole.css';

const LogConsole = ({ logEntries, title, extraMargin }) => (<div className="logConsole">
  <ScrollableContainer extraMargin={extraMargin + 42}>
    <div>
      { title }
      <ul>
        {logEntries.map(log =>
          <li key={log.id}>
            <p>
              { `${formatDate(log.timeStamp)}: ` }
              {log.employee ? (<RolesValidation allowed={[Roles.Admin, Roles.CompanyAdmin, Roles.CompanyQA]}>
                <Link to={`/security-employee/${log.employee.id}`} >{log.employee.name}</Link>
              </RolesValidation>) : ''}
              {log.employee ? (<RolesValidation forbidden={[Roles.Admin, Roles.CompanyAdmin, Roles.CompanyQA]}>
                <span>{log.employee.name}</span>
              </RolesValidation>) : ''}
              { ` - ${log.description} - ` }
              {log.host ? <Link to={`/security-host/${log.host.id}`} >{log.host.account.userName}</Link> : ''}
              &nbsp;
              {log.host && log.host.currentCharacter ? <Link to={`/character-detail/${log.host.currentCharacter.character.id}`} >{log.host.currentCharacter.character.name}</Link> : ''}
            </p>
          </li>)}
      </ul>
    </div>
  </ScrollableContainer>
</div>);

LogConsole.defaultProps = {
  extraMargin: 0
};

LogConsole.propTypes = {
  logEntries: React.PropTypes.array.isRequired,
  title: React.PropTypes.string.isRequired,
  extraMargin: React.PropTypes.number
};

export default LogConsole;
