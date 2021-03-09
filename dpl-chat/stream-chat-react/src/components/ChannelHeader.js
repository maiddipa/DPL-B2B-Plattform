import React, { PureComponent } from 'react';
import PropTypes from 'prop-types';
import { Avatar } from './Avatar';
import { withChannelContext } from '../context';
import { languageChangedEvent } from '../utils';
import { FormattedMessage, injectIntl } from 'react-intl';
// import chevrondown from '../assets/str-chat__icon-chevron-down.svg';
/**
 * ChannelHeader - Render some basic information about this channel
 *
 * @example ./docs/ChannelHeader.md
 * @extends PureComponent
 */
class ChannelHeader extends PureComponent {
  constructor(props) {
    super(props);
    this.state = {
      isLoading: false,
    };
  }
  static propTypes = {
    /** Set title manually */
    title: PropTypes.string,
    /** Show a little indicator that the channel is live right now */
    live: PropTypes.bool,
    /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#chat)** */
    channel: PropTypes.object.isRequired,
    /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#chat)** */
    watcher_count: PropTypes.number,
    language: PropTypes.string,
  };
  static defaultProps = {
    language: localStorage.getItem('language'),
  };

  render() {
    const { intl } = this.props;
    return (
      <div className="str-chat__header-livestream">
        {this.props.channel.data.image && (
          <Avatar
            image={this.props.channel.data.image}
            shape="rounded"
            size={this.props.channel.type === 'commerce' ? 60 : 40}
          />
        )}
        <div className="str-chat__header-livestream-left">
          <p className="str-chat__header-livestream-left--title">
            <a className="channel-header-link" onClick={() => this.jumpTo()}>
              {this.props.title || this.props.channel.data.name}{' '}
            </a>
            {this.props.live && (
              <span className="str-chat__header-livestream-left--livelabel">
                <FormattedMessage
                  id="channel_header.live"
                  defaultMessage="live"
                />
              </span>
            )}
          </p>
          {this.props.channel.data.subtitle && (
            <p className="str-chat__header-livestream-left--subtitle">
              {this.props.channel.data.subtitle}
            </p>
          )}
          <p className="str-chat__header-livestream-left--members">
            {!this.props.live && this.props.channel.data.member_count > 0 && (
              <>
                <FormattedMessage
                  id="channel_header.members"
                  defaultMessage="{count} members"
                  values={{ count: this.props.channel.data.member_count }}
                />
                ,{' '}
              </>
            )}
            <FormattedMessage
              id="channel_header.watchers"
              defaultMessage="{count} online"
              values={{ count: this.props.watcher_count }}
            />
          </p>
          <div className="str-chat__channel_header_responsive_member-label--all">
            <label>
              {this.props.channel.data.responsiveMember
                ? 'In Bearbeitung' + '  (dies sehen alle)'
                : ''}
            </label>
          </div>
        </div>
        <div id="channel_header_responsive_member">
          {this.props.client.user.role === 'admin' &&
          this.props.client.user.name !==
            this.props.channel.data.responsiveMember &&
          !this.state.isLoading ? (
            <>
              <button onClick={this.SetResponsive}>
                {this.props.channel.data.responsiveMember
                  ? 'Übernehme Bearbeitung'
                  : 'Starte Bearbeitung'}
              </button>
              {/* <label>
                {this.props.channel.data.responsiveMember
                  ? 'Aktuell in Bearbeitung von ' +
                    this.props.channel.data.responsiveMember
                  : ''}
              </label> */}
            </>
          ) : (
            <label></label>
          )}
        </div>
        <div id="channel_header_left_panel">
          <this.LanguageSelect></this.LanguageSelect>
        </div>
      </div>
    );
  }
  // deleteChannel = async () => {
  //   const response = await this.channel.delete();
  //   console.log('channel is deleted', response);
  // };
  SetResponsive = async () => {
    await this.setState({ isLoading: true });
    await this.props.channel.update(
      {
        name: this.props.channel.data.name,
        extraType: this.props.channel.data.extraType,
        referenceId: this.props.channel.data.referenceId,
        assignFlag: true,
        languages: this.props.channel.data.languages,
        responsiveMember: this.props.client.user.name,
      },
      {
        text: this.props.client.user.name + ' ist jetzt der neue Bearbeiter',
        user_id: this.props.client.user.id,
      },
    );
    await this.setState({ isLoading: false });
  };

  LanguageSelect = () => (
    <select
      id="select"
      className="str-chat__channel-list-team__header--button"
      onChange={this.onLanguageChange}
      defaultValue={this.props.language}
    >
      {this.languageOptions}
    </select>
  );

  jumpTo = () => {
    const { extraType, referenceId } = this.props.channel.data;
    window.dplChatConfig.goToDetails(extraType, referenceId);
  };

  onLanguageChange(event) {
    window.dplChatConfig.language = event.target.value;
    window.dispatchEvent(languageChangedEvent);
    localStorage.setItem('language', window.dplChatConfig.language);
  }

  languageOptions = window.dplChatConfig.languages.map((option, j) => {
    if (option.value === '') {
      return (
        <option key={j} value={option.value}>
          {option.label}
        </option>
      );
    }
    if (this.props.channel.data.languages !== null) {
      if (
        this.props.channel.data.languages.find(
          (element) => element === option.value,
        )
      ) {
        return (
          <option key={j} value={option.value}>
            {option.label}
          </option>
        );
      } else {
        return null;
      }
    } else {
      return null;
    }
  });
}

ChannelHeader = withChannelContext(ChannelHeader);
export { ChannelHeader };
