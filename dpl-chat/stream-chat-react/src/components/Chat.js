import React, { PureComponent } from 'react';
import PropTypes from 'prop-types';
import { ChatContext } from '../context';
import { IntlProvider } from 'react-intl';
import messages_en from '../translations/en.json';
import messages_de from '../translations/de.json';
import messages_es from '../translations/es.json';
import messages_it from '../translations/it.json';
import messages_pl from '../translations/pl.json';
import messages_cs from '../translations/cs.json';
import messages_da from '../translations/da.json';
import messages_fr from '../translations/fr.json';
import messages_nl from '../translations/nl.json';
import messages_pt from '../translations/pt.json';
import messages_ru from '../translations/ru.json';
import messages_zh from '../translations/zh.json';

const messages = {
  'en': messages_en,
  'de': messages_de,
  'es': messages_es,
  'it': messages_it,
  'pl': messages_pl,
  'cs': messages_cs,
  'da': messages_da,
  'fr': messages_fr,
  'nl': messages_nl,
  'pt': messages_pt,
  'ru': messages_ru,
  'zh': messages_zh
};
/**
 * Chat - Wrapper component for Chat. The needs to be placed around any other chat components.
 * This Chat component provides the ChatContext to all other components.
 *
 * The ChatContext provides the following props:
 *
 * - client (the client connection)
 * - channels (the list of channels)
 * - setActiveChannel (a function to set the currently active channel)
 * - channel (the currently active channel)
 *
 * It also exposes the withChatContext HOC which you can use to consume the ChatContext
 *
 * @example ./docs/Chat.md
 * @extends PureComponent
 */
export class Chat extends PureComponent {
  static propTypes = {
    /** The StreamChat client object */
    client: PropTypes.object.isRequired,
    /**
     *
     * Theme could be used for custom styling of the components.
     *
     * You can override the classes used in our components under parent theme class.
     *
     * e.g. If you want to build a theme where background of message is black
     *
     * ```
     *  <Chat client={client} theme={demo}>
     *    <Channel>
     *      <MessageList />
     *    </Channel>
     *  </Chat>
     * ```
     *
     * ```scss
     *  .demo.str-chat {
     *    .str-chat__message-simple {
     *      &-text-inner {
     *        background-color: black;
     *      }
     *    }
     *  }
     * ```
     *
     * Built in available themes:
     *
     *  - `messaging light`
     *  - `messaging dark`
     *  - `team light`
     *  - `team dark`
     *  - `commerce light`
     *  - `commerce dark`
     *  - `gaming light`
     *  - `gaming dark`
     *  - `livestream light`
     *  - `livestream dark`
     */
    theme: PropTypes.string,
    locale: PropTypes.string,
    messages: PropTypes.object,
  };

  static defaultProps = {
    theme: 'messaging light',
    locale: 'de',
  };

  constructor(props) {
    super(props);

    this.state = {
      // currently active channel
      channel: {},
      search: undefined,
      error: false,
    };
  }

  setActiveChannel = async (channel, watchers = {}, e) => {
    if (e !== undefined && e.preventDefault) {
      e.preventDefault();
    }
    if (Object.keys(watchers).length) {
      await channel.query({ watch: true, watchers });
    }
    if (this.state.channel === channel) {
      this.setState(() => ({
        channel: {},
      }));
    }
    this.setState(() => ({
      channel,
    }));
  };

  setActiveSearch = async (search, e) => {
    if (e !== undefined && e.preventDefault) {
      e.preventDefault();
    }
    if (search) {
      this.setState(() => ({
        search,
      }));
    } else {
      this.setState(() => ({
        search: undefined,
      }));
    }
  };

  getContext = () => ({
    client: this.props.client,
    channel: this.state.channel,
    search: this.state.search,
    setActiveChannel: this.setActiveChannel,
    setActiveSearch: this.setActiveSearch,
    theme: this.props.theme,
  });

  render() {
    return (
      <IntlProvider
        locale={this.props.locale}
        messages={messages[this.props.locale]}
      >
        <ChatContext.Provider value={this.getContext()}>
          {this.props.children}
        </ChatContext.Provider>
      </IntlProvider>
    );
  }
}
