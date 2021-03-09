import React, { PureComponent } from 'react';
import PropTypes from 'prop-types';
import { LoadingChannels } from './LoadingChannels';
import { Avatar } from './Avatar';
import { ChatDown } from './ChatDown';
import { withChatContext } from '../context';
import { languageChangedEvent } from '../utils';
import { FormattedMessage, injectIntl } from 'react-intl';

/**
 * ChannelList - A preview list of channels, allowing you to select the channel you want to open
 * @example ./examples/ChannelList.md
 */
class ChannelListTeam extends PureComponent {
  constructor(props) {
    super(props);
    this.state = {
      searchOpen: false,
      searchInput: '',
      filter: { type: 'messaging' },
      adminFilter: { members: { $in: [this.props.client.user.id] } },
      sort: {},
      inputMembers: [],
      inputExtraTypes: [],
      disableSort: 'false',
      adminControlOpen: false,
    };
    this.handleChange = this.handleChange.bind(this);
  }
  static propTypes = {
    loading: PropTypes.bool,
    error: PropTypes.bool,
    /** Stream chat client object */
    client: PropTypes.object,
    showSidebar: PropTypes.bool,
    onSelectSort: PropTypes.func,
    onSelectFilter: PropTypes.func,
    onToggleSearchModus: PropTypes.func,
    isOn: PropTypes.bool,
    onChannelResult: PropTypes.func,
    noResult: PropTypes.bool,
    resetSearch: PropTypes.func,
  };

  static defaultProps = {
    error: false,
  };

  async onAdminFilterChange(event) {
    const value = event.target.value;
    this.setState({ searchInput: '' });
    this.props.setActiveSearch('', event);
    let adminFilter = {};
    switch (value) {
      case 'all':
        break;
      case 'my':
        adminFilter.members = { $in: [this.props.client.user.id] };
        break;
      case 'no':
        adminFilter.assignFlag = false;
        break;
      case 'me':
        adminFilter.responsiveMember = { $eq: this.props.client.user.name };
        break;
      case 'other':
        adminFilter.assignFlag = true;
        adminFilter.responsiveMember = { $ne: this.props.client.user.name };
        break;
      default:
        adminFilter.members = { $in: [this.props.client.user.id] };
        break;
    }
    let mergedFilter = {};
    if (this.state.filter !== {}) {
      await this.setState({ adminFilter });

      mergedFilter = { ...this.state.filter, ...this.state.adminFilter };
      // console.log(mergedFilter);
    } else {
      await this.setState({ ...adminFilter, filter: { type: 'messaging' } });

      mergedFilter = { ...this.state.filter, ...this.state.adminFilter };
      // console.log(mergedFilter);
    }

    // filter = extraType ? (filter.extraType = { $eq: event.target.value }) : {};
    // // this.setState({ filter });
    this.props.onSelectFilter(mergedFilter);
  }

  async onFilterChange(event) {
    const filterType = event.target.value;
    this.setState({ searchInput: '' });
    this.props.setActiveSearch('', event);
    let filter = {};
    if (this.props.client.user.role !== 'admin') {
      filter.members = { $in: [this.props.client.user.id] };
      filterType ? (filter.extraType = { $eq: filterType }) : {};
    } else {
      filterType
        ? (filter = {
            // members: { $in: [this.props.client.user.id] },
            extraType: { $eq: filterType },
          })
        : {};
    }
    // console.log(filter);
    let mergedFilter = {};
    if (this.state.adminFilter !== {}) {
      await this.setState({
        filter,
      });
      mergedFilter = { ...this.state.filter, ...this.state.adminFilter };
      // console.log(mergedFilter);
    } else {
      await this.setState({
        filter,
        adminFilter: { members: { $in: [this.props.client.user.id] } },
      });
      mergedFilter = { ...this.state.filter, ...this.state.adminFilter };
      // console.log(mergedFilter);
    }

    this.props.onSelectFilter(mergedFilter);
  }

  onSortChange(event) {
    this.setState({ searchInput: '' });
    this.props.setActiveSearch('', event);
    const sort = {};
    switch (event.target.value) {
      case 'last_message_at--new':
        sort['last_message_at'] = -1;
        break;
      case 'created_at--new':
        sort['created_at'] = -1;
        break;
      case 'last_message_at--old':
        sort['last_message_at'] = 1;
        break;
      case 'created_at--old':
        sort['created_at'] = 1;
        break;

      default:
        sort['last_message_at'] = -1;
        break;
    }
    this.setState({ sort });
    this.props.onSelectSort(sort);
  }

  SortingSelect = () => (
    <div className="sortingSelect">
      <select
        className="str-chat__channel-list-team__header--button"
        id="flexgrow"
        onChange={(e) => this.onSortChange(e)}
      >
        <option
          value="last_message_at--new"
          label={this.props.intl.formatMessage({
            id: 'channel_sort.last',
            defaultMessage: 'newest',
          })}
        ></option>
        <option
          value="created_at--old"
          label={this.props.intl.formatMessage({
            id: 'channel_sort.old',
            defaultMessage: 'oldest',
          })}
        ></option>
      </select>
    </div>
  );

  filterOptions = window.dplChatConfig.channelTypes.map((option, i) => (
    <option key={i} value={option.value}>
      {option.label}
    </option>
  ));

  FilterSelect = () => (
    <div className="filterselect">
      <select
        className="str-chat__channel-list-team__header--button"
        id="filterselect"
        onChange={(e) => this.onFilterChange(e)}
      >
        {this.filterOptions}
      </select>
    </div>
  );

  AdminFilterSelect = () => {
    return (
      <div className="filterselect">
        <select
          className="str-chat__channel-list-team__header--button"
          id="adminfilterselect"
          onChange={(e) => this.onAdminFilterChange(e)}
        >
          <option
            value="my"
            label={this.props.intl.formatMessage({
              id: 'channel_filter.my',
              defaultMessage: 'my',
            })}
          ></option>
          <option
            value="all"
            label={this.props.intl.formatMessage({
              id: 'channel_filter.all',
              defaultMessage: 'all',
            })}
          ></option>
          <option
            value="me"
            label={this.props.intl.formatMessage({
              id: 'channel_filter.me',
              defaultMessage: 'assgined to: me',
            })}
          ></option>
          <option
            value="no"
            label={this.props.intl.formatMessage({
              id: 'channel_filter.no',
              defaultMessage: 'assgined to: none',
            })}
          ></option>
          <option
            value="other"
            label={this.props.intl.formatMessage({
              id: 'channel_filter.other',
              defaultMessage: 'assgined to: others',
            })}
          ></option>
        </select>
      </div>
    );
  };

  render() {
    const { showSidebar } = this.props;
    if (this.props.error) {
      return <ChatDown />;
    } else if (this.props.loading) {
      return <LoadingChannels />;
    } else {
      return (
        <div className="str-chat__channel-list-team">
          {showSidebar && (
            <div className="str-chat__channel-list-team__sidebar">
              <div className="str-chat__channel-list-team__sidebar--top">
                <Avatar
                  image="https://cdn.dribbble.com/users/610788/screenshots/5157282/spacex.png"
                  size={50}
                />
              </div>
            </div>
          )}

          <div className="str-chat__channel-list-team__main">
            <div className="str-chat__channel-list-team__header">
              <div className="str-chat__channel-list-team__header--left">
                <Avatar
                  source={this.props.client.user.image}
                  name={
                    this.props.client.user.name || this.props.client.user.id
                  }
                  size={40}
                />
              </div>
              <div className="str-chat__channel-list-team__header--middle">
                <div className="str-chat__channel-list-team__header--title">
                  {this.props.client.user.name || this.props.client.user.id}
                </div>
                <div
                  className={`str-chat__channel-list-team__header--status ${this.props.client.user.status}`}
                >
                  {this.props.client.user.status}
                </div>
              </div>
            </div>
            <div id="style-channellist-first-row">
              <div id="style-channel-selection">
                <this.SortingSelect></this.SortingSelect>
                <this.FilterSelect></this.FilterSelect>
              </div>
              {this.props.client.user.role === 'admin' ? (
                <div className="channel-list-admin-icons tooltip">
                  {this.state.adminControlOpen ? (
                    <span className="tooltiptext">
                      Filter für Mitarbeiter entfernen
                    </span>
                  ) : (
                    <span className="tooltiptext">
                      Filter für Mitarbeiter anzeigen
                    </span>
                  )}
                  <this.ChevronButton
                    onClick={this.toggleAdminControl}
                    open={this.state.adminControlOpen}
                  ></this.ChevronButton>
                </div>
              ) : (
                <div></div>
              )}
            </div>
            <div id="style-channellist-secound-row">
              {this.props.client.user.role === 'admin' &&
              this.state.adminControlOpen ? (
                <div className="channel-list-admin-filter">
                  <this.AdminFilterSelect></this.AdminFilterSelect>
                  {/* <this.ClearButton
                  onClick={this.clearFilterControl}
                  open={true}
                ></this.ClearButton> */}
                </div>
              ) : (
                <div></div>
              )}
            </div>
            <div id="style-channel-search-selection">
              <this.SearchBar></this.SearchBar>
              <this.Button
                onClick={this.toggleChannelSearch}
                open={this.state.searchOpen}
              />
            </div>
            <div
              id="style-list"
              className=".str-chat__channel-preview-messenger-list"
            >
              {this.props.children}
            </div>
          </div>
        </div>
      );
    }
  }

  Button = ({ open, onClick }) => (
    <div onClick={onClick}>
      {open ? (
        <div>
          <svg
            id="style-channel-search-icon"
            width="18"
            height="18"
            viewBox="0 0 18 18"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              stroke="black"
              d="M 19.333 2.547 l -1.88 -1.88 L 10 8.12 L 2.547 0.667 l -1.88 1.88 L 8.12 10 L 0.667 17.453 l 1.88 1.88 L 10 11.88 l 7.453 7.453 l 1.88 -1.88 L 11.88 10 Z"
            ></path>
          </svg>
        </div>
      ) : (
        <div>
          <svg
            id="style-channel-search-icon"
            width="18"
            height="17"
            viewBox="0 0 18 17"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              stroke="black"
              d="M18.125,15.804l-4.038-4.037c0.675-1.079,1.012-2.308,1.01-3.534C15.089,4.62,12.199,1.75,8.584,1.75C4.815,1.75,1.982,4.726,2,8.286c0.021,3.577,2.908,6.549,6.578,6.549c1.241,0,2.417-0.347,3.44-0.985l4.032,4.026c0.167,0.166,0.43,0.166,0.596,0l1.479-1.478C18.292,16.234,18.292,15.968,18.125,15.804 M8.578,13.99c-3.198,0-5.716-2.593-5.733-5.71c-0.017-3.084,2.438-5.686,5.74-5.686c3.197,0,5.625,2.493,5.64,5.624C14.242,11.548,11.621,13.99,8.578,13.99 M16.349,16.981l-3.637-3.635c0.131-0.11,0.721-0.695,0.876-0.884l3.642,3.639L16.349,16.981z"
            ></path>
          </svg>
        </div>
      )}
    </div>
  );

  ChevronButton = ({ open, onClick }) => (
    <div onClick={onClick}>
      {open ? (
        <div>
          <svg
            id="style-channel-admin-control-toggle"
            width="16"
            height="16"
            viewBox="0 0 448 512"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              stroke="black"
              d="M436 192H312c-13.3 0-24-10.7-24-24V44c0-6.6 5.4-12 12-12h40c6.6 0 12 5.4 12 12v84h84c6.6 0 12 5.4 12 12v40c0 6.6-5.4 12-12 12zm-276-24V44c0-6.6-5.4-12-12-12h-40c-6.6 0-12 5.4-12 12v84H12c-6.6 0-12 5.4-12 12v40c0 6.6 5.4 12 12 12h124c13.3 0 24-10.7 24-24zm0 300V344c0-13.3-10.7-24-24-24H12c-6.6 0-12 5.4-12 12v40c0 6.6 5.4 12 12 12h84v84c0 6.6 5.4 12 12 12h40c6.6 0 12-5.4 12-12zm192 0v-84h84c6.6 0 12-5.4 12-12v-40c0-6.6-5.4-12-12-12H312c-13.3 0-24 10.7-24 24v124c0 6.6 5.4 12 12 12h40c6.6 0 12-5.4 12-12z"
            ></path>
          </svg>
        </div>
      ) : (
        <div>
          <svg
            id="style-channel-admin-control-toggle"
            width="16"
            height="16"
            viewBox="0 0 448 512"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              stroke="black"
              d="M0 180V56c0-13.3 10.7-24 24-24h124c6.6 0 12 5.4 12 12v40c0 6.6-5.4 12-12 12H64v84c0 6.6-5.4 12-12 12H12c-6.6 0-12-5.4-12-12zM288 44v40c0 6.6 5.4 12 12 12h84v84c0 6.6 5.4 12 12 12h40c6.6 0 12-5.4 12-12V56c0-13.3-10.7-24-24-24H300c-6.6 0-12 5.4-12 12zm148 276h-40c-6.6 0-12 5.4-12 12v84h-84c-6.6 0-12 5.4-12 12v40c0 6.6 5.4 12 12 12h124c13.3 0 24-10.7 24-24V332c0-6.6-5.4-12-12-12zM160 468v-40c0-6.6-5.4-12-12-12H64v-84c0-6.6-5.4-12-12-12H12c-6.6 0-12 5.4-12 12v124c0 13.3 10.7 24 24 24h124c6.6 0 12-5.4 12-12z"
            ></path>
          </svg>
        </div>
      )}
    </div>
  );

  ClearButton = ({ open, onClick }) => (
    <div onClick={onClick}>
      {open ? (
        <div>
          <svg
            id="style-channel-filter-clear-icon"
            width="18"
            height="18"
            viewBox="0 0 18 18"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              stroke="black"
              d="M 19.333 2.547 l -1.88 -1.88 L 10 8.12 L 2.547 0.667 l -1.88 1.88 L 8.12 10 L 0.667 17.453 l 1.88 1.88 L 10 11.88 l 7.453 7.453 l 1.88 -1.88 L 11.88 10 Z"
            ></path>
          </svg>
        </div>
      ) : (
        <div></div>
      )}
    </div>
  );

  SearchBar = () => {
    const { intl } = this.props;
    return (
      <div id="style-search-bar">
        <div className="str-chat__channel-search" id="style-channel-search">
          <div id="style-channel-search-input">
            <input
              id="searchbar"
              className="searchInput"
              placeholder={intl.formatMessage({
                id: 'channel_search.placeholder',
                defaultMessage: 'Search',
              })}
              value={this.state.searchInput || ''}
              type="text"
              name="inputSearch"
              onChange={this.handleChange}
              onKeyDown={this.handleSearch.bind(this)}
            ></input>
          </div>
        </div>
      </div>
    );
  };

  handleSearch(event) {
    const code = event.keyCode || event.which;
    if (code === 13) {
      window.dplChatConfig.language = '';
      window.dispatchEvent(languageChangedEvent);
      localStorage.setItem('language', window.dplChatConfig.language);
      if (this.state.searchInput.replace(/\s/g, '')) {
        this.setState({ disableSort: 'true', searchOpen: true });
        this.props.setActiveSearch(
          this.state.searchInput.replace(/( +)$/, ''),
          event,
        );
        this.search(
          this.state.searchInput.replace(/( +)$/, ''),
          this.props.client,
        );
      }
    }
  }

  async search(searchValue, client) {
    const channelId = [];
    if (!this.state.filter.hasOwnProperty('extraType')) {
      await this.setState({ filter: { type: 'messaging' } });
    }
    let response = [];
    const searchResultChannels = [];
    if (searchValue) {
      response = await client.search(this.state.filter, searchValue, {
        limit: 1000,
        offset: 0,
      });
    }
    let resultChannelArray = [];
    if (response) {
      resultChannelArray = response.results
        .filter((element) => !!element.message.channel.cid)
        .map((element) => element.message.channel);
    }

    if (resultChannelArray.length > 0) {
      channelId.push(resultChannelArray[0].cid);
      searchResultChannels.push(resultChannelArray[0]);
    }

    await resultChannelArray.forEach((element) => {
      if (!channelId.includes(element.cid)) {
        channelId.push(element.cid);
        searchResultChannels.push(element);
      }
    });

    if (searchResultChannels.length > 0) {
      this.props.onChannelResult(searchResultChannels);
    } else {
      this.props.onChannelResult(null);
    }
  }

  async handleChange(event) {
    await this.setState({ searchInput: event.target.value });
  }

  toggleAdminControl = async () => {
    if (this.state.adminControlOpen) {
      await this.setState({
        adminControlOpen: false,
        adminFilter: { members: { $in: [this.props.client.user.id] } },
      });
      let mergedFilter = { ...this.state.filter, ...this.state.adminFilter };
      // console.log(mergedFilter);
      this.props.onSelectFilter(mergedFilter);
    } else {
      this.setState({ adminControlOpen: true });
    }
  };

  clearFilterControl = async () => {
    if (this.props.client.user.role === 'admin') {
      // close admin control
      if (this.state.adminControlOpen) {
        await this.setState({
          adminControlOpen: false,
          adminFilter: { members: { $in: [this.props.client.user.id] } },
        });
        let mergedFilter = {
          ...this.state.filter,
          ...this.state.adminFilter,
        };
        this.props.onSelectFilter(mergedFilter);
      }
    }

    // default user filter: MY
    const filter = {
      members: { $in: [this.props.client.user.id] },
    };
    // Merge filter
    let mergedFilter = { ...filter, ...this.state.filter };
    // console.log(mergedFilter);
    // Set Filter
    this.props.onSelectFilter(mergedFilter);
  };

  toggleChannelSearch = (event) => {
    if (this.state.searchOpen) {
      this.props.setActiveSearch('', event);
      this.props.resetSearch();
      this.setState({
        searchOpen: false,
        searchInput: '',
      });
    } else {
      this.setState({ searchOpen: true });
      window.dplChatConfig.language = '';
      window.dispatchEvent(languageChangedEvent);
      localStorage.setItem('language', window.dplChatConfig.language);
      this.setState({ disableSort: 'true', searchOpen: true });
      this.search(this.state.searchInput, this.props.client);
      this.props.setActiveSearch(this.state.searchInput, event);
    }
  };
}

ChannelListTeam = withChatContext(injectIntl(ChannelListTeam));
export { ChannelListTeam };
