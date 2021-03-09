import React, { PureComponent } from 'react';
import PropTypes from 'prop-types';
import { injectIntl } from 'react-intl';
import { ChannelPreviewCountOnly } from './ChannelPreviewCountOnly';

class ChannelPreview extends PureComponent {
  constructor(props) {
    super(props);

    this.state = {
      lastMessage: {},
      unread: 0,
      lastRead: new Date(),
    };
  }

  static propTypes = {
    channel: PropTypes.object.isRequired,
    activeChannel: PropTypes.object.isRequired,
    setActiveChannel: PropTypes.func.isRequired,
    Preview: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),
    watchers: PropTypes.object,
  };

  static defaultProps = {
    Preview: ChannelPreviewCountOnly,
  };

  componentDidMount() {
    // listen to change...
    const channel = this.props.channel;
    const unread = channel.countUnread();

    this.setState({ unread });
    channel.on('message.new', this.handleEvent);
  }

  componentWillUnmount() {
    const channel = this.props.channel;
    channel.off('message.new', this.handleEvent);
  }

  handleEvent = (event) => {
    const channel = this.props.channel;
    const isActive = this.props.activeChannel.cid === channel.cid;
    if (!isActive) {
      const unread = channel.countUnread(this.state.lastRead);
      this.setState({ lastMessage: event.message, unread });
    } else {
      this.setState({ lastMessage: event.message, unread: 0 });
    }
  };

  componentDidUpdate(prevProps) {
    if (this.props.activeChannel.cid !== prevProps.activeChannel.cid) {
      const isActive = this.props.activeChannel.cid === this.props.channel.cid;
      if (isActive) {
        this.setState({ unread: 0, lastRead: new Date() });
      }
    }
  }

  getLatestMessage = () => {
    const { channel, intl } = this.props;

    const latestMessage =
      channel.state.messages[channel.state.messages.length - 1];

    if (!latestMessage) {
      return intl.formatMessage({
        id: 'channel_preview.latest_message.none',
        defaultMessage: 'Nothing yet...',
      });
    }
    if (latestMessage.deleted_at) {
      return intl.formatMessage({
        id: 'channel_preview.latest_message.deleted',
        defaultMessage: 'Message deleted...',
      });
    }
    if (latestMessage.text) {
      return latestMessage.text.slice(0, 20);
    } else {
      if (latestMessage.command) {
        return intl.formatMessage(
          {
            id: 'channel_preview.latest_message.command',
            defaultMessage: '/{command}',
          },
          {
            command: latestMessage.command,
          },
        );
      }
      if (latestMessage.attachments.length) {
        return intl.formatMessage({
          id: 'channel_preview.latest_message.attachment',
          defaultMessage: 'Attachment...',
        });
      }
      return intl.formatMessage({
        id: 'channel_preview.latest_message.empty',
        defaultMessage: 'Empty message...',
      });
    }
  };

  render() {
    const props = { ...this.state, ...this.props };

    const { Preview } = this.props;
    return (
      <Preview
        {...props}
        latestMessage={this.getLatestMessage()}
        active={this.props.activeChannel.cid === this.props.channel.cid}
      />
    );
  }
}

ChannelPreview = injectIntl(ChannelPreview);
export { ChannelPreview };