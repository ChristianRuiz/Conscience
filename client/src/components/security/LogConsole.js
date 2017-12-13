import React from 'react';
import { Link } from 'react-router-dom';

import ScrollableContainer from '../common/ScrollableContainer';

class LogConsole extends React.Component {
  render() {
    return (<ScrollableContainer>
      <div>
        <h2>Logs</h2>
        <ul>
          {this.props.logEntries.map(log =>
            <li key={log.id}>
              <p>
                {log.employee ? <Link to={`/security-detail/${log.employee.id}`} >{log.employee.name}</Link> : ''}{ ` - ${log.description} - ` }
                {log.host ? <Link to={`/behaviour-detail/${log.host.id}`} >{log.host.account.userName}</Link> : ''}
                {log.host && log.host.currentCharacter ? <Link to={`/character-detail/${log.host.currentCharacter.character.id}`} >{` ${log.host.currentCharacter.character.name}`}</Link> : ''}
              </p>
            </li>)}
        </ul>
      </div>
    </ScrollableContainer>);
  }
}

LogConsole.propTypes = {
  logEntries: React.PropTypes.array.isRequired
};

export default LogConsole;
