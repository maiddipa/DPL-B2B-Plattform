import React, { PureComponent } from 'react';
import { withChatContext } from '../context';

class UserList extends PureComponent {


    constructor(){}

    render(){

        return(
            <React.Fragment>


            </React.Fragment>
        )
    }
}






UserList = withChatContext(UserList);
export default { UserList };