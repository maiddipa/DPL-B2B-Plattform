import React, { PureComponent } from 'react';
import PropTypes from 'prop-types';

export class ChannelPreviewCountOnly extends PureComponent {
  static propTypes = {
    /** @see See [chat context](https://getstream.github.io/stream-chat-react/#chat) for doc */
    setActiveChannel: PropTypes.func,
    /** @see See [chat context](https://getstream.github.io/stream-chat-react/#chat) for doc */
    channel: PropTypes.object,
    unread: PropTypes.number,
  };

  render() {
    const unreadClass = this.props.unread >= 1 ? 'unread' : '';
    const name = this.props.channel.data.name || this.props.channel.cid;

    return (
      <div className={unreadClass}>
        <button
          onClick={this.props.setActiveChannel.bind(this, this.props.channel)}
        >
          {' '}
          {name} <span>{this.props.unread}</span>
        </button>
      </div>
    );
  }
}
