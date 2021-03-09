import React, { PureComponent } from 'react';
import PropTypes from 'prop-types';
import { injectIntl } from 'react-intl';
import { Avatar } from './Avatar';

import truncate from 'lodash/truncate';

/**
 * Used as preview component for channel item in [ChannelList](#channellist) component.
 * Its best suited for messenger type chat.
 *
 * @example ./docs/ChannelPreviewMessenger.md
 * @extends PureComponent
 */
class ChannelPreviewMessenger extends PureComponent {
  static propTypes = {
    /** **Available from [chat context](https://getstream.github.io/stream-chat-react/#chat)** */
    setActiveChannel: PropTypes.func,
    /** **Available from [chat context](https://getstream.github.io/stream-chat-react/#chat)** */
    channel: PropTypes.object,
    closeMenu: PropTypes.func,
    unread: PropTypes.number,
    /** If channel of component is active (selected) channel */
    active: PropTypes.bool,
    latestMessage: PropTypes.string,
  };

  channelPreviewButton = React.createRef();

  onSelectChannel = () => {
    this.props.setActiveChannel(this.props.channel, this.props.watchers);
    this.channelPreviewButton.current.blur();
    this.props.closeMenu();
  };

  onDeleteChannel = async () => {
    const removeChannel = await this.props.channel.delete();
    // console.log(removeChannel);
  };

  render() {
    const unreadClass =
      this.props.unread >= 1
        ? 'str-chat__channel-preview-messenger--unread'
        : '';
    const activeClass = this.props.active
      ? 'str-chat__channel-preview-messenger--active'
      : '';

    const { channel, intl } = this.props;

    return (
      <div id="style-select-channel-list">
        <button
          onClick={this.onSelectChannel}
          ref={this.channelPreviewButton}
          className={`str-chat__channel-preview-messenger ${unreadClass} ${activeClass}`}
        >
          {/* <div className="str-chat__channel-preview-messenger--left">
            <Avatar image={channel.data.image} size={40} />
          </div> */}

          <div className="str-chat__channel-preview-messenger--right">
            <div className="str-chat__channel-preview-messenger--name">
              <span>{truncate(channel.data.name, 29)}</span>
            </div>
            <div id="style-channel-preview">
              <div className="str-chat__channel-preview-messenger--last-message">
                {!channel.state.messages[0]
                  ? intl.formatMessage({
                      id: 'channel_preview.latest_message.none',
                      defaultMessage: 'Nothing yet...',
                    })
                  : truncate(this.props.latestMessage, 14)}
              </div>
              {channel._client.user.role === 'admin' ? (
                <div className="str-chat__channel-preview-messenger--responsive-member">
                  {channel.data.responsiveMember ? (
                    <div className="channel-list-show-responsive-member">
                      <div className="str-chat__channel-preview-assigned-to">
                        {intl.formatMessage({
                          id: 'channel_preview.responsiveMember.label',
                          defaultMessage: 'assigned to',
                        }) +
                          ' ' +
                          channel.data.responsiveMember}{' '}
                      </div>
                    </div>
                  ) : (
                    intl.formatMessage({
                      id: 'channel_preview.responsiveMember.none',
                      defaultMessage: 'not assigned',
                    })
                  )}
                </div>
              ) : (
                <div></div>
              )}
            </div>
          </div>
        </button>
        <div className="style-unread-icon-section">
          {channel._client.user.role === 'admin' ? (
            <div className="channel-delete-tooltip">
              <span className="channel-delete-tooltiptext">
                Löschen des Channels
              </span>
              <div
                className="str-chat__channel-preview-delete-channel-icon"
                onClick={this.onDeleteChannel}
              >
                <svg
                  id="style-channel-delete-icon"
                  width="14"
                  height="14"
                  viewBox="0 0 18 18"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path
                    stroke="black"
                    d="M 19.333 2.547 l -1.88 -1.88 L 10 8.12 L 2.547 0.667 l -1.88 1.88 L 8.12 10 L 0.667 17.453 l 1.88 1.88 L 10 11.88 l 7.453 7.453 l 1.88 -1.88 L 11.88 10 Z"
                  ></path>
                </svg>
              </div>
            </div>
          ) : (
            <div></div>
          )}
          {this.showUnread()}
        </div>
      </div>
    );
  }

  showUnread() {
    if (!this.props.unread) {
      return;
    } else {
      return (
        <label id="style-unread-message-button">
          {!this.props.unread ? '' : truncate(this.props.unread, 5)}
        </label>
      );
    }
  }
}

ChannelPreviewMessenger = injectIntl(ChannelPreviewMessenger);
export { ChannelPreviewMessenger };
