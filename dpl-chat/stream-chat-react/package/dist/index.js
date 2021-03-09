'use strict';

Object.defineProperty(exports, '__esModule', { value: true });

function _interopDefault (ex) { return (ex && (typeof ex === 'object') && 'default' in ex) ? ex['default'] : ex; }

var _classCallCheck = _interopDefault(require('@babel/runtime/helpers/classCallCheck'));
var _createClass = _interopDefault(require('@babel/runtime/helpers/createClass'));
var _possibleConstructorReturn = _interopDefault(require('@babel/runtime/helpers/possibleConstructorReturn'));
var _getPrototypeOf = _interopDefault(require('@babel/runtime/helpers/getPrototypeOf'));
var _assertThisInitialized = _interopDefault(require('@babel/runtime/helpers/assertThisInitialized'));
var _inherits = _interopDefault(require('@babel/runtime/helpers/inherits'));
var _defineProperty = _interopDefault(require('@babel/runtime/helpers/defineProperty'));
var React = require('react');
var React__default = _interopDefault(React);
var PropTypes = _interopDefault(require('prop-types'));
var _extends = _interopDefault(require('@babel/runtime/helpers/extends'));
var _toConsumableArray = _interopDefault(require('@babel/runtime/helpers/toConsumableArray'));
var _regeneratorRuntime = _interopDefault(require('@babel/runtime/regenerator'));
var _asyncToGenerator = _interopDefault(require('@babel/runtime/helpers/asyncToGenerator'));
var ReactPlayer = _interopDefault(require('react-player'));
var sanitizeUrl = require('@braintree/sanitize-url');
var Carousel = require('react-images');
var Carousel__default = _interopDefault(Carousel);
var reactFileUtils = require('react-file-utils');
var prettybytes = _interopDefault(require('pretty-bytes'));
var anchorme = _interopDefault(require('anchorme'));
var emojiRegex = _interopDefault(require('emoji-regex'));
var ReactMarkdown = _interopDefault(require('react-markdown/with-html'));
var truncate = _interopDefault(require('lodash/truncate'));
var data = _interopDefault(require('emoji-mart/data/all.json'));
var reactIntl = require('react-intl');
var emojiMart = require('emoji-mart');
var _slicedToArray = _interopDefault(require('@babel/runtime/helpers/slicedToArray'));
var _typeof = _interopDefault(require('@babel/runtime/helpers/typeof'));
var getCaretCoordinates = _interopDefault(require('textarea-caret'));
var CustomEvent = _interopDefault(require('custom-event'));
var Textarea = _interopDefault(require('react-textarea-autosize'));
var Immutable = _interopDefault(require('seamless-immutable'));
var uniq = _interopDefault(require('lodash/uniq'));
var streamChat = require('stream-chat');
var moment = _interopDefault(require('moment'));
require('moment/locale/de');
var deepequal = _interopDefault(require('deep-equal'));
var _objectWithoutProperties = _interopDefault(require('@babel/runtime/helpers/objectWithoutProperties'));
var uuidv4 = _interopDefault(require('uuid/v4'));
var Visibility = _interopDefault(require('visibilityjs'));
var debounce = _interopDefault(require('lodash/debounce'));
var throttle = _interopDefault(require('lodash/throttle'));
var uniqBy = _interopDefault(require('lodash.uniqby'));

/**
 * Avatar - A round avatar image with fallback to username's first letter
 *
 * @example ./docs/Avatar.md
 * @extends PureComponent
 */

var Avatar =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(Avatar, _React$PureComponent);

  function Avatar() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, Avatar);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(Avatar)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "state", {
      errored: false,
      loaded: false
    });

    _defineProperty(_assertThisInitialized(_this), "getInitials", function (name) {
      return name ? name.split(' ').slice(0, 1).map(function (name) {
        return name.charAt(0);
      }) : null;
    });

    _defineProperty(_assertThisInitialized(_this), "onLoad", function () {
      _this.setState({
        loaded: true
      });
    });

    _defineProperty(_assertThisInitialized(_this), "onError", function () {
      _this.setState({
        errored: true
      });
    });

    return _this;
  }

  _createClass(Avatar, [{
    key: "componentDidUpdate",
    value: function componentDidUpdate(prevProps) {
      if (prevProps.image !== this.props.image) {
        this.setState({
          loaded: false,
          errored: false
        });
      }
    }
  }, {
    key: "render",
    value: function render() {
      var _this$props = this.props,
          size = _this$props.size,
          name = _this$props.name,
          shape = _this$props.shape,
          image = _this$props.image;
      var initials = this.getInitials(name);
      return React__default.createElement("div", {
        className: "str-chat__avatar str-chat__avatar--".concat(shape),
        title: name,
        style: {
          width: size,
          height: size,
          flexBasis: size,
          lineHeight: size + 'px',
          fontSize: size / 2
        }
      }, image && !this.state.errored ? React__default.createElement("img", {
        src: image,
        alt: initials,
        className: 'str-chat__avatar-image' + (this.state.loaded ? ' str-chat__avatar-image--loaded' : ''),
        style: {
          width: size,
          height: size,
          flexBasis: size,
          objectFit: 'cover'
        },
        onLoad: this.onLoad,
        onError: this.onError
      }) : React__default.createElement("div", {
        className: "str-chat__avatar-fallback"
      }, initials));
    }
  }]);

  return Avatar;
}(React__default.PureComponent);

_defineProperty(Avatar, "propTypes", {
  /** image url */
  image: PropTypes.string,

  /** name of the picture, used for title tag fallback */
  name: PropTypes.string,

  /** shape of the avatar, circle, rounded or square */
  shape: PropTypes.oneOf(['circle', 'rounded', 'square']),

  /** size in pixels */
  size: PropTypes.number
});

_defineProperty(Avatar, "defaultProps", {
  size: 32,
  shape: 'circle'
});

/**
 * SafeAnchor - In all ways similar to a regular anchor tag.
 * The difference is that it sanitizes the href value and prevents XSS
 * @extends PureComponent
 */

var SafeAnchor =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(SafeAnchor, _React$PureComponent);

  function SafeAnchor() {
    _classCallCheck(this, SafeAnchor);

    return _possibleConstructorReturn(this, _getPrototypeOf(SafeAnchor).apply(this, arguments));
  }

  _createClass(SafeAnchor, [{
    key: "render",
    value: function render() {
      var href = sanitizeUrl.sanitizeUrl(this.props.href);
      return React__default.createElement("a", _extends({}, this.props, {
        href: href
      }), this.props.children);
    }
  }]);

  return SafeAnchor;
}(React__default.PureComponent);

var giphyLogo = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAAAkCAYAAAB/up84AAAACXBIWXMAAAsTAAALEwEAmpwYAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAABVhJREFUeNrsW6GS20AMdToGBgEGAQYBBgEBBQYFgQcP5hMO9jP6CYWFBwsPBgYUGBQEFAQUGAQYBBgYGHjmutt5O6NupbXXcZJrx5rJXGyv11o96Ukr52avr6/BJG9HZmMBMpvNYnxt1JzNZNoRAYFxM2Z8rT6FuueMcaH6s1KfhBn3U42r1Jg1rrfq+Bt5xgf1Z64+uQFQndNzLc1Ydfwg6F2p6wd1PVXfU+b6Gc9vHGuh8+jrsXVN61Sq64XggNw95tlH9XmP6y3W2OI+qvN3db6mN7/zBFAb8L2aNMJxRsDQC6jIuEyN039LnAvV8QJKRRijZUHmT8iiLpEFDHKJaB1TGN732WuAokEIDQCwhwGjsMEIMNgp6qY9JlsTgyXquCFG1d54IsbOsKAVPLDBcQJjUxB0RJwAltGntNQ46GhzqPnb0y0954RG/1iLQ7SRCkR+guiPtW6GFRg5gAlCrFvbJEZ0ngDAUn0/Y77fDCJFXuiB/AmGDC3PLg0YWLRW5CcJWWPglNxDKS6C59AcZBs/sYxbOQBqHHkuZYCsGCds4SQJDDx3RK3RjUb9EfMUcLQ57BHS64MAIYtYkvEt+d4wCzqr++ipkoTtkihOI2chREfA5KiC0GOAaMig05zoWJPjgMk39jxcPphDR0mSDrtq438g51iq8omQlEnYJfHoiAFxYQGko6bCPSmZ5wS+TRx0Zc5R4CtmHbEVJT+0p1uOYdNE1SMfOKNO0zXWEmItptApsfYa1LV0UZUPIHSCmlRYJhfokNWhX5IcsmIWbEAMCQWerWirhZK57MghNQyzgke3QuQWPUv4EAac9wCuJjmkNmvUNEwiobX+DgdEWoQGgNDPGtTWWhRRWONX5JlnePCZhP1JUCOzPN1O0C2MohP7xuiko8Qy9INUDBg2YPJMlzP8pRv0qYeUdu+Cy+RAKIYmtVqojM5kkS0DwkXlLuY0ICzgAEOlAd8fPe+rYJdppz61TiZ5G4AgcScWz05RcUtAkOwyJMtI4FzNt3suCWKfwLUhdqRC0yA/enB1CZ4vBZ2fhIptJ4x/5PYVavyzQ39N0V8ddnsQ+m3sfX02hjo3bIVJ7d5PhqojZxYdC3NEdv9oQMQWFFjHHqpLEsc9BZyF23c9cG0ZOJjUIN15V1mY8OOAhW0E77yWaP2eoO9VBFG/d6yX6/xuHT2z3AsQ5ImnjrZBF6XcUiKAEl0RlJzZ7ZtnbxmqSgRqfxmyD9k6wNDGzuExecB3Z1/ukBOjQH73MZbspChFp9nQ/EYY9+LaaIYOqlo7JjwISX+LcBwbjAOz2ZKS7BpOcq0o0R2HvZAbHpHPJGc+dm00paQuedmOA4O0WD5fyQ4V08Ip4ATxhYl8CCh76/0QLVyehlBVFyCpYJTcKmPjHoY8XNE2VQ8dbIkdr4Z95npBwcNVahKzNEMBSYSyz46iLm8sLunreG5O+xYTsaMUHYu6bMn79sRCT2+8l6SMV2cCT5e3UspBXbbd9n3nDIN/Q1KP3JDfWLcd8kZwCVX12hjeOlmOIMe+L6FGjJLC4QS5rz6hg/tThjZiU0Pr/g7D65/uCUafKgaUJu0lHjvox/XsjXA+GAOQUogIXV8/v7GoKOGJfYuHxvHjt7t3rEMHD2+E5PoR+5GCLCS+8g6Z2xgGt6anuwGC99MSKAl6RrfUs/ofje+b1PcjlJBlMMk4gKBUe77AqKVP/T1Jj30IQPmCTdkm6NeKb5BkJzCGdCA8XuFGZIOWCBEh/mwGiZ/rFZXk3xHEdkjHb6MknVOhypJe+Sac03XlL4fe3r81mH518q9GyCS3kV8CDADlsrVaJhTLAgAAAABJRU5ErkJggg==";

/**
 * Card - Simple Card Layout
 *
 * @example ./docs/Card.md
 * @extends PureComponent
 */

var Card =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(Card, _React$PureComponent);

  function Card() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, Card);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(Card)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "trimUrl", function (url) {
      var trimmedUrl;

      if (url !== undefined && url !== null) {
        trimmedUrl = url.replace(/^(?:https?:\/\/)?(?:www\.)?/i, '').split('/')[0];
      }

      return trimmedUrl;
    });

    return _this;
  }

  _createClass(Card, [{
    key: "render",
    value: function render() {
      var _this$props = this.props,
          title = _this$props.title,
          title_link = _this$props.title_link,
          text = _this$props.text,
          type = _this$props.type,
          image_url = _this$props.image_url,
          thumb_url = _this$props.thumb_url,
          og_scrape_url = _this$props.og_scrape_url;
      var image = thumb_url || image_url;

      if (!title && !title_link && !image) {
        return React__default.createElement("div", {
          className: "str-chat__message-attachment-card str-chat__message-attachment-card--".concat(type)
        }, React__default.createElement("div", {
          className: "str-chat__message-attachment-card--content"
        }, React__default.createElement("div", {
          className: "str-chat__message-attachment-card--text"
        }, "this content could not be displayed")));
      }

      if (!title_link && !og_scrape_url) {
        return null;
      }

      return React__default.createElement("div", {
        className: "str-chat__message-attachment-card str-chat__message-attachment-card--".concat(type)
      }, image && React__default.createElement("div", {
        className: "str-chat__message-attachment-card--header"
      }, React__default.createElement("img", {
        src: image,
        alt: image
      })), React__default.createElement("div", {
        className: "str-chat__message-attachment-card--content"
      }, React__default.createElement("div", {
        className: "str-chat__message-attachment-card--flex"
      }, title && React__default.createElement("div", {
        className: "str-chat__message-attachment-card--title"
      }, title), text && React__default.createElement("div", {
        className: "str-chat__message-attachment-card--text"
      }, text), (title_link || og_scrape_url) && React__default.createElement(SafeAnchor, {
        href: title_link || og_scrape_url,
        target: "_blank",
        rel: "noopener noreferrer",
        className: "str-chat__message-attachment-card--url"
      }, this.trimUrl(title_link || og_scrape_url))), type === 'giphy' && React__default.createElement("img", {
        className: "str-chat__message-attachment-card__giphy-logo",
        src: giphyLogo,
        alt: "giphy logo"
      })));
    }
  }]);

  return Card;
}(React__default.PureComponent);

_defineProperty(Card, "propTypes", {
  /** Title returned by the OG scraper */
  title: PropTypes.string.isRequired,

  /** Link returned by the OG scraper */
  title_link: PropTypes.string,

  /** The scraped url, used as a fallback if the OG-data doesn't include a link */
  og_scrape_url: PropTypes.string,

  /** The url of the full sized image */
  image_url: PropTypes.string,

  /** The url for thumbnail sized image*/
  thumb_url: PropTypes.string,

  /** Description returned by the OG scraper */
  text: PropTypes.string
});

/**
 * Image - Small wrapper around an image tag, supports thumbnails
 *
 * @example ./docs/Image.md
 * @extends PureComponent
 */

var Image =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(Image, _React$PureComponent);

  function Image() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, Image);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(Image)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "state", {
      modalIsOpen: false,
      currentIndex: 0
    });

    _defineProperty(_assertThisInitialized(_this), "toggleModal", function () {
      _this.setState(function (state) {
        return {
          modalIsOpen: !state.modalIsOpen
        };
      });
    });

    return _this;
  }

  _createClass(Image, [{
    key: "render",
    value: function render() {
      var _this$props = this.props,
          image_url = _this$props.image_url,
          thumb_url = _this$props.thumb_url,
          fallback = _this$props.fallback;
      var formattedArray = [{
        src: image_url || thumb_url
      }];
      return React__default.createElement(React__default.Fragment, null, React__default.createElement("img", {
        className: "str-chat__message-attachment--img",
        onClick: this.toggleModal,
        src: thumb_url || image_url,
        alt: fallback
      }), React__default.createElement(Carousel.ModalGateway, null, this.state.modalIsOpen ? React__default.createElement(Carousel.Modal, {
        onClose: this.toggleModal
      }, React__default.createElement(Carousel__default, {
        views: formattedArray
      })) : null));
    }
  }]);

  return Image;
}(React__default.PureComponent);

_defineProperty(Image, "propTypes", {
  /** The full size image url */
  image_url: PropTypes.string,

  /** The thumb url */
  thumb_url: PropTypes.string,

  /** The text fallback for the image */
  fallback: PropTypes.string
});

/**
 * AttachmentActions - The actions you can take on an attachment
 *
 * @example ./docs/AttachmentActions.md
 * @extends PureComponent
 */

var AttachmentActions =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(AttachmentActions, _React$PureComponent);

  function AttachmentActions() {
    _classCallCheck(this, AttachmentActions);

    return _possibleConstructorReturn(this, _getPrototypeOf(AttachmentActions).apply(this, arguments));
  }

  _createClass(AttachmentActions, [{
    key: "render",
    value: function render() {
      var _this = this;

      var _this$props = this.props,
          text = _this$props.text,
          id = _this$props.id,
          actions = _this$props.actions,
          actionHandler = _this$props.actionHandler;
      return React__default.createElement("div", {
        className: "str-chat__message-attachment-actions"
      }, React__default.createElement("form", {
        className: "str-chat__message-attachment-actions-form"
      }, React__default.createElement("span", {
        key: 0
      }, text), actions.map(function (action) {
        return React__default.createElement("button", {
          className: "str-chat__message-attachment-actions-button str-chat__message-attachment-actions-button--".concat(action.style),
          key: "".concat(id, "-").concat(action.value),
          "data-value": action.value,
          onClick: actionHandler.bind(_this, action.name, action.value)
        }, action.text);
      })));
    }
  }]);

  return AttachmentActions;
}(React__default.PureComponent);

_defineProperty(AttachmentActions, "propTypes", {
  // /** The id of the form input */
  // id: PropTypes.string.isRequired,

  /** The text for the form input */
  text: PropTypes.string,

  /** A list of actions */
  actions: PropTypes.array.isRequired,

  /**
   *
   * Handler for actions. Actions in combination with attachments can be used to build [commands](https://getstream.io/chat/docs/#channel_commands).
   *
   * @param name {string} Name of action
   * @param value {string} Value of action
   * @param event Dom event that triggered this handler
   */
  actionHandler: PropTypes.func.isRequired
});

var Audio =
/*#__PURE__*/
function (_React$Component) {
  _inherits(Audio, _React$Component);

  function Audio(props) {
    var _this;

    _classCallCheck(this, Audio);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(Audio).call(this, props));

    _defineProperty(_assertThisInitialized(_this), "playAudio", function () {
      if (_this.audioRef.current !== null) {
        _this.audioRef.current.pause();

        _this.updateProgress();

        _this.setState({
          playing: true,
          updateProgress: setInterval(_this.updateProgress, 500)
        }); //$FlowFixMe


        _this.audioRef.current.play();
      }
    });

    _defineProperty(_assertThisInitialized(_this), "pauseAudio", function () {
      if (_this.audioRef.current !== null) {
        _this.audioRef.current.pause();
      }

      _this.setState({
        playing: false
      });

      window.clearInterval(_this.state.updateProgress);
    });

    _defineProperty(_assertThisInitialized(_this), "updateProgress", function () {
      if (_this.audioRef.current !== null) {
        var position = _this.audioRef.current.currentTime;
        var duration = _this.audioRef.current.duration;
        var progress = 100 / duration * position;

        _this.setState({
          progress: progress
        });

        if (position === duration) {
          _this.pauseAudio();
        }
      }
    });

    _defineProperty(_assertThisInitialized(_this), "_handleClose", function (e) {
      if (_this.props.handleClose) {
        _this.props.handleClose(e);
      }
    });

    _this.state = {
      open: false,
      playing: false,
      progress: 0,
      updateProgress: null
    };
    _this.audioRef = React.createRef();
    return _this;
  }

  _createClass(Audio, [{
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      window.clearInterval(this.state.updateProgress);
    }
  }, {
    key: "render",
    value: function render() {
      var _this2 = this;

      var og = this.props.og;
      var url = og.asset_url;
      var image = og.image_url;
      return React.createElement("div", {
        className: "str-chat__audio"
      }, React.createElement("div", {
        className: "str-chat__audio__wrapper"
      }, React.createElement("audio", {
        ref: this.audioRef
      }, React.createElement("source", {
        src: url,
        type: "audio/mp3"
      })), React.createElement("div", {
        className: "str-chat__audio__image"
      }, React.createElement("div", {
        className: "str-chat__audio__image--overlay"
      }, !this.state.playing ? React.createElement("div", {
        onClick: function onClick() {
          return _this2.playAudio();
        },
        className: "str-chat__audio__image--button"
      }, React.createElement("svg", {
        width: "40",
        height: "40",
        viewBox: "0 0 64 64",
        xmlns: "http://www.w3.org/2000/svg"
      }, React.createElement("path", {
        d: "M32 58c14.36 0 26-11.64 26-26S46.36 6 32 6 6 17.64 6 32s11.64 26 26 26zm0 6C14.327 64 0 49.673 0 32 0 14.327 14.327 0 32 0c17.673 0 32 14.327 32 32 0 17.673-14.327 32-32 32zm13.237-28.412L26.135 45.625a3.27 3.27 0 0 1-4.426-1.4 3.319 3.319 0 0 1-.372-1.47L21 23.36c-.032-1.823 1.41-3.327 3.222-3.358a3.263 3.263 0 0 1 1.473.322l19.438 9.36a3.311 3.311 0 0 1 .103 5.905z",
        fillRule: "nonzero"
      }))) : React.createElement("div", {
        onClick: function onClick() {
          return _this2.pauseAudio();
        },
        className: "str-chat__audio__image--button"
      }, React.createElement("svg", {
        width: "40",
        height: "40",
        viewBox: "0 0 64 64",
        xmlns: "http://www.w3.org/2000/svg"
      }, React.createElement("path", {
        d: "M32 58.215c14.478 0 26.215-11.737 26.215-26.215S46.478 5.785 32 5.785 5.785 17.522 5.785 32 17.522 58.215 32 58.215zM32 64C14.327 64 0 49.673 0 32 0 14.327 14.327 0 32 0c17.673 0 32 14.327 32 32 0 17.673-14.327 32-32 32zm-7.412-45.56h2.892a2.17 2.17 0 0 1 2.17 2.17v23.865a2.17 2.17 0 0 1-2.17 2.17h-2.892a2.17 2.17 0 0 1-2.17-2.17V20.61a2.17 2.17 0 0 1 2.17-2.17zm12.293 0h2.893a2.17 2.17 0 0 1 2.17 2.17v23.865a2.17 2.17 0 0 1-2.17 2.17h-2.893a2.17 2.17 0 0 1-2.17-2.17V20.61a2.17 2.17 0 0 1 2.17-2.17z",
        fillRule: "nonzero"
      })))), React.createElement("img", {
        src: image,
        alt: "".concat(og.description)
      })), React.createElement("div", {
        className: "str-chat__audio__content"
      }, React.createElement("span", {
        className: "str-chat__audio__content--title"
      }, React.createElement("strong", null, og.title)), React.createElement("span", {
        className: "str-chat__audio__content--subtitle"
      }, og.text), React.createElement("div", {
        className: "str-chat__audio__content--progress"
      }, React.createElement("div", {
        style: {
          width: "".concat(this.state.progress, "%")
        }
      })))));
    }
  }]);

  return Audio;
}(React.Component);

/**
 * Attachment - The message attachment
 *
 * @example ./docs/Attachment.md
 * @extends PureComponent
 */

var Attachment =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(Attachment, _PureComponent);

  function Attachment() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, Attachment);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(Attachment)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "attachmentRef", React__default.createRef());

    _defineProperty(_assertThisInitialized(_this), "renderAttachmentActions", function (a) {
      return React__default.createElement(AttachmentActions, _extends({
        key: 'key-actions-' + a.id
      }, a, {
        actionHandler: _this.props.actionHandler
      }));
    });

    _defineProperty(_assertThisInitialized(_this), "renderAttachment", function (a) {
      return React__default.createElement("div", {
        className: "str-chat__attachment",
        key: "key-image-".concat(a.id)
      }, React__default.createElement(Card, _extends({}, a, {
        key: "key-card-".concat(a.id)
      })), _this.renderAttachmentActions(a));
    });

    return _this;
  }

  _createClass(Attachment, [{
    key: "attachmentType",
    value: function attachmentType(a) {
      var type, extra;

      if (a.actions && a.actions.length > 0) {
        extra = 'actions';
      }

      if (a.type === 'giphy' || a.type === 'imgur') {
        type = 'card';
      } else if (a.type === 'image' && (a.title_link || a.og_scrape_url)) {
        type = 'card';
      } else if (a.type === 'image') {
        type = 'image';
      } else if (a.type === 'file') {
        type = 'file';
      } else if (a.type === 'audio') {
        type = 'audio';
      } else if (a.type === 'video') {
        type = 'media';
      } else {
        type = 'card';
        extra = 'no-image';
      }

      return {
        type: type,
        extra: extra
      };
    }
  }, {
    key: "render",
    value: function render() {
      var a = this.props.attachment;

      if (!a) {
        return null;
      }

      var _this$attachmentType = this.attachmentType(a),
          type = _this$attachmentType.type,
          extra = _this$attachmentType.extra;

      if (type === 'card' && !a.title_link && !a.og_scrape_url) {
        return null;
      }

      var results = [];

      if (type === 'image') {
        if (a.actions && a.actions.length) {
          results.push(React__default.createElement("div", {
            className: "str-chat__attachment",
            key: "key-image-".concat(a.id)
          }, React__default.createElement(Image, a), this.renderAttachmentActions(a)));
        } else {
          results.push(React__default.createElement(Image, _extends({}, a, {
            key: "key-image-".concat(a.id)
          })));
        }
      } else if (type === 'file') {
        a.asset_url && results.push(React__default.createElement("div", {
          className: "str-chat__message-attachment-file--item",
          key: "key-file-".concat(a.id)
        }, React__default.createElement(reactFileUtils.FileIcon, {
          mimeType: a.mime_type,
          filename: a.title,
          big: true,
          size: 30
        }), React__default.createElement("div", {
          className: "str-chat__message-attachment-file--item-text"
        }, React__default.createElement(SafeAnchor, {
          href: a.asset_url,
          download: true
        }, a.title), a.file_size && React__default.createElement("span", null, prettybytes(a.file_size)))));
      } else if (type === 'audio') {
        results.push(React__default.createElement("div", {
          className: "str-chat__attachment",
          key: "key-video-".concat(a.id)
        }, React__default.createElement(Audio, {
          og: a
        })));
      } else if (type === 'media') {
        if (a.actions && a.actions.length) {
          results.push(React__default.createElement("div", {
            className: "str-chat__attachment",
            key: "key-video-".concat(a.id)
          }, React__default.createElement("div", {
            className: "str-chat__player-wrapper"
          }, React__default.createElement(ReactPlayer, {
            className: "react-player",
            url: a.asset_url,
            width: "100%",
            height: "100%",
            controls: true
          })), this.renderAttachmentActions(a)));
        } else {
          results.push(React__default.createElement("div", {
            className: "str-chat__player-wrapper",
            key: "key-video-".concat(a.id)
          }, React__default.createElement(ReactPlayer, {
            className: "react-player",
            url: a.asset_url,
            width: "100%",
            height: "100%",
            controls: true
          })));
        }
      } else {
        if (a.actions && a.actions.length) {
          results.push(this.renderAttachment(a));
        } else {
          results.push(React__default.createElement(Card, _extends({}, a, {
            key: "key-card-".concat(a.id)
          })));
        }
      }

      if (results.length === 0) return null;
      return React__default.createElement("div", {
        className: "str-chat__message-attachment str-chat__message-attachment--".concat(type, " str-chat__message-attachment--").concat(a.type, " str-chat__message-attachment--").concat(type, "--").concat(extra),
        ref: this.attachmentRef
      }, results);
    }
  }]);

  return Attachment;
}(React.PureComponent);

_defineProperty(Attachment, "propTypes", {
  /**
   * The attachment to render
   * @see See [Attachment structure](https://getstream.io/chat/docs/#message_format)
   *
   *  */
  attachment: PropTypes.object.isRequired,

  /**
   *
   * Handler for actions. Actions in combination with attachments can be used to build [commands](https://getstream.io/chat/docs/#channel_commands).
   *
   * @param name {string} Name of action
   * @param value {string} Value of action
   * @param event Dom event that triggered this handler
   */
  actionHandler: PropTypes.func.isRequired
});

Attachment.propTypes = {};

function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(source, true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(source).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }
var emojiSetDef = {
  spriteUrl: 'https://getstream.imgix.net/images/emoji-sprite.png',
  size: 20,
  sheetColumns: 2,
  sheetRows: 3,
  sheetSize: 64
};
var commonEmoji = {
  emoticons: [],
  short_names: [],
  custom: true
};
var defaultMinimalEmojis = [_objectSpread({
  id: 'like',
  name: 'like',
  colons: ':+1:',
  sheet_x: 0,
  sheet_y: 0
}, commonEmoji, {}, emojiSetDef), _objectSpread({
  id: 'love',
  name: 'love',
  colons: ':heart:',
  sheet_x: 1,
  sheet_y: 2
}, commonEmoji, {}, emojiSetDef), _objectSpread({
  id: 'haha',
  name: 'haha',
  colons: ':joy:',
  sheet_x: 1,
  sheet_y: 0
}, commonEmoji, {}, emojiSetDef), _objectSpread({
  id: 'wow',
  name: 'wow',
  colons: ':astonished:',
  sheet_x: 0,
  sheet_y: 2
}, commonEmoji, {}, emojiSetDef), _objectSpread({
  id: 'sad',
  name: 'sad',
  colons: ':pensive:',
  sheet_x: 0,
  sheet_y: 1
}, commonEmoji, {}, emojiSetDef), _objectSpread({
  id: 'angry',
  name: 'angry',
  colons: ':angry:',
  sheet_x: 1,
  sheet_y: 1
}, commonEmoji, {}, emojiSetDef)];
var d = Object.assign({}, data);
d.emojis = {}; // use this only for small lists like in ReactionSelector

var emojiData = d;
var isOnlyEmojis = function isOnlyEmojis(text) {
  if (!text) return false;
  var noEmojis = text.replace(emojiRegex(), '');
  var noSpace = noEmojis.replace(/[\s\n]/gm, '');
  return !noSpace;
};
var isPromise = function isPromise(thing) {
  return thing && typeof thing.then === 'function';
};
var byDate = function byDate(a, b) {
  return a.created_at - b.created_at;
}; // https://stackoverflow.com/a/29234240/7625485

var formatTypingArray = function formatTypingArray(intl, dict) {
  var arr2 = Object.keys(dict);
  var arr3 = [];
  arr2.forEach(function (item, i) {
    return arr3.push(dict[arr2[i]].user.name || dict[arr2[i]].user.id);
  });
  var and = intl.formatMessage({
    id: 'message_input.and',
    defaultMessage: 'and'
  });
  var typing = intl.formatMessage({
    id: 'message_input.typing',
    defaultMessage: '{count, plural, one {is typing...} other {are typing...}}'
  }, {
    count: arr3.length
  });
  var outStr = '';

  if (arr3.length === 1) {
    outStr = arr3[0] + ' ' + typing;
  } else if (arr3.length === 2) {
    //joins all with "and" but =no commas
    //example: "bob and sam"
    outStr = arr3.join(' ' + and + ' ') + ' ' + typing;
  } else if (arr3.length > 2) {
    //joins all with commas, but last one gets ", and" (oxford comma!)
    //example: "bob, joe, and sam"
    outStr = arr3.slice(0, -1).join(', ') + ', ' + and + ' ' + arr3.slice(-1) + ' ' + typing;
  }

  return outStr;
}; // https://stackoverflow.com/a/29234240/7625485

var formatStatusArray = function formatStatusArray(intl, arr) {
  var outStr = '';
  var slicedArr = arr.map(function (item) {
    return item.name || item.id;
  }).slice(0, 5);
  var restLength = arr.length - slicedArr.length;
  var and = intl.formatMessage({
    id: 'message_status.and',
    defaultMessage: 'and'
  });
  var more = intl.formatMessage({
    id: 'message_status.more',
    defaultMessage: 'and {count} more'
  }, {
    count: restLength
  });
  var lastStr = restLength > 0 ? ' ' + more : '';

  if (slicedArr.length === 1) {
    outStr = slicedArr[0] + ' ';
  } else if (slicedArr.length === 2) {
    //joins all with "and" but =no commas
    //example: "bob and sam"
    outStr = slicedArr.join(' ' + and + ' ') + ' ';
  } else if (slicedArr.length > 2) {
    //joins all with commas, but last one gets ", and" (oxford comma!)
    //example: "bob, joe, and sam"
    outStr = slicedArr.join(', ') + lastStr;
  }

  return outStr;
};
var renderText = function renderText(message) {
  // take the @ mentions and turn them into markdown?
  // translate links
  var language = window.dplChatConfig.language;
  var text = '';

  if (message.translations) {
    switch (language) {
      case 'en':
        text = message.translations.en ? message.translations.en : message.text;
        break;

      case 'de':
        text = message.translations.de ? message.translations.de : message.text;
        break;

      case 'it':
        text = message.translations.it ? message.translations.it : message.text;
        break;

      case 'es':
        text = message.translations.es ? message.translations.es : message.text;
        break;

      case 'pl':
        text = message.translations.pl ? message.translations.pl : message.text;
        break;

      case 'fr':
        text = message.translations.fr ? message.translations.fr : message.text;
        break;

      default:
        text = message.text;
    }
  } else {
    text = message.text;
  }

  var mentioned_users = message.mentioned_users;

  if (!text) {
    return;
  }

  var allowed = ['html', 'root', 'text', 'break', 'paragraph', 'emphasis', 'strong', 'link', 'list', 'listItem', 'code', 'inlineCode', 'blockquote'];
  var urls = anchorme(text, {
    list: true
  });
  var _iteratorNormalCompletion = true;
  var _didIteratorError = false;
  var _iteratorError = undefined;

  try {
    for (var _iterator = urls[Symbol.iterator](), _step; !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
      var urlInfo = _step.value;
      var displayLink = truncate(urlInfo.encoded.replace(/^(www\.)/, ''), {
        length: 20,
        omission: '...'
      });

      var _mkdown = "[".concat(displayLink, "](").concat(urlInfo.protocol).concat(urlInfo.encoded, ")");

      text = text.replace(urlInfo.raw, _mkdown);
    }
  } catch (err) {
    _didIteratorError = true;
    _iteratorError = err;
  } finally {
    try {
      if (!_iteratorNormalCompletion && _iterator.return != null) {
        _iterator.return();
      }
    } finally {
      if (_didIteratorError) {
        throw _iteratorError;
      }
    }
  }

  var newText = text;

  if (mentioned_users && mentioned_users.length) {
    for (var i = 0; i < mentioned_users.length; i++) {
      var username = mentioned_users[i].name || mentioned_users[i].id;
      var mkdown = "**@".concat(username, "**");
      var re = new RegExp("@".concat(username), 'g');
      newText = newText.replace(re, mkdown);
    }
  }

  return React__default.createElement(ReactMarkdown, {
    allowedTypes: allowed,
    source: newText,
    linkTarget: "_blank",
    plugins: [],
    escapeHtml: true,
    skipHtml: false
  });
}; // https://stackoverflow.com/a/6860916/2570866

function generateRandomId() {
  // prettier-ignore
  return S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4();
}

function S4() {
  return ((1 + Math.random()) * 0x10000 | 0).toString(16).substring(1);
}

var smartRender = function smartRender(ElementOrComponentOrLiteral, props, fallback) {
  if (ElementOrComponentOrLiteral === undefined) {
    ElementOrComponentOrLiteral = fallback;
  }

  if (React__default.isValidElement(ElementOrComponentOrLiteral)) {
    // Flow cast through any, to make flow believe it's a React.Element
    var element = ElementOrComponentOrLiteral; // eslint-disable-line

    return element;
  } // Flow cast through any to remove React.Element after previous check


  var ComponentOrLiteral = ElementOrComponentOrLiteral;

  if (typeof ComponentOrLiteral === 'string' || typeof ComponentOrLiteral === 'number' || typeof ComponentOrLiteral === 'boolean' || ComponentOrLiteral == null) {
    return ComponentOrLiteral;
  }

  return React__default.createElement(ComponentOrLiteral, props);
};
var MESSAGE_ACTIONS = {
  edit: 'edit',
  delete: 'delete',
  flag: 'flag',
  mute: 'mute'
};
var languageChangedEventName = 'dpl-chat-language-changed';
var languageChangedEvent = new Event(languageChangedEventName);

/**
 * MessageActionsBox - A component for taking action on a message
 *
 * @example ./docs/MessageActionsBox.md
 * @extends PureComponent
 */

var MessageActionsBox =
/*#__PURE__*/
function (_React$Component) {
  _inherits(MessageActionsBox, _React$Component);

  function MessageActionsBox() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, MessageActionsBox);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(MessageActionsBox)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "actionsBoxRef", React__default.createRef());

    _defineProperty(_assertThisInitialized(_this), "state", {
      reverse: false,
      rect: null
    });

    return _this;
  }

  _createClass(MessageActionsBox, [{
    key: "componentDidMount",
    value: function componentDidMount() {}
  }, {
    key: "componentDidUpdate",
    value: function () {
      var _componentDidUpdate = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee(prevProps) {
        var ml;
        return _regeneratorRuntime.wrap(function _callee$(_context) {
          while (1) {
            switch (_context.prev = _context.next) {
              case 0:
                if (!(!prevProps.open && this.props.open)) {
                  _context.next = 6;
                  break;
                }

                if (!(this.state.rect === null)) {
                  _context.next = 4;
                  break;
                }

                _context.next = 4;
                return this.setState({
                  rect: this.actionsBoxRef.current.getBoundingClientRect()
                });

              case 4:
                ml = this.props.messageListRect;

                if (this.props.mine) {
                  this.setState({
                    reverse: this.state.rect.left < ml.left ? true : false
                  });
                } else if (!this.props.mine) {
                  this.setState({
                    reverse: this.state.rect.right + 5 > ml.right ? true : false
                  });
                }

              case 6:
              case "end":
                return _context.stop();
            }
          }
        }, _callee, this);
      }));

      function componentDidUpdate(_x) {
        return _componentDidUpdate.apply(this, arguments);
      }

      return componentDidUpdate;
    }()
  }, {
    key: "render",
    value: function render() {
      var _this$props = this.props,
          handleFlag = _this$props.handleFlag,
          handleMute = _this$props.handleMute,
          handleEdit = _this$props.handleEdit,
          handleDelete = _this$props.handleDelete,
          getMessageActions = _this$props.getMessageActions;
      var messageActions = getMessageActions();
      return React__default.createElement("div", {
        className: "str-chat__message-actions-box\n          ".concat(this.props.open ? 'str-chat__message-actions-box--open' : '', "\n          ").concat(this.props.mine ? 'str-chat__message-actions-box--mine' : '', "\n          ").concat(this.state.reverse ? 'str-chat__message-actions-box--reverse' : '', "\n        "),
        ref: this.actionsBoxRef
      }, React__default.createElement("ul", {
        className: "str-chat__message-actions-list"
      }, messageActions.indexOf(MESSAGE_ACTIONS.flag) > -1 && React__default.createElement("button", {
        onClick: handleFlag
      }, React__default.createElement("li", {
        className: "str-chat__message-actions-list-item"
      }, React__default.createElement(reactIntl.FormattedMessage, {
        id: "message_actions.flag",
        defaultMessage: "Flag"
      }))), messageActions.indexOf(MESSAGE_ACTIONS.mute) > -1 && React__default.createElement("button", {
        onClick: handleMute
      }, React__default.createElement("li", {
        className: "str-chat__message-actions-list-item"
      }, React__default.createElement(reactIntl.FormattedMessage, {
        id: "message_actions.mute",
        defaultMessage: "Mute"
      }))), messageActions.indexOf(MESSAGE_ACTIONS.edit) > -1 && React__default.createElement("button", {
        onClick: handleEdit
      }, React__default.createElement("li", {
        className: "str-chat__message-actions-list-item"
      }, React__default.createElement(reactIntl.FormattedMessage, {
        id: "message_actions.edit",
        defaultMessage: "Edit Message"
      }))), messageActions.indexOf(MESSAGE_ACTIONS.delete) > -1 && React__default.createElement("button", {
        onClick: handleDelete
      }, React__default.createElement("li", {
        className: "str-chat__message-actions-list-item"
      }, React__default.createElement(reactIntl.FormattedMessage, {
        id: "message_actions.delete",
        defaultMessage: "Delete"
      })))));
    }
  }]);

  return MessageActionsBox;
}(React__default.Component);

_defineProperty(MessageActionsBox, "propTypes", {
  /** If the message actions box should be open or not */
  open: PropTypes.bool.isRequired,

  /**
   * @deprecated
   *
   *  The message component, most logic is delegated to this component and MessageActionsBox uses the following functions explicitly:
   *  `handleFlag`, `handleMute`, `handleEdit`, `handleDelete`, `canDeleteMessage`, `canEditMessage`, `isMyMessage`, `isAdmin`
   */
  Message: PropTypes.oneOfType([PropTypes.node, PropTypes.func, PropTypes.object]).isRequired,

  /** If message belongs to current user. */
  mine: PropTypes.bool,

  /** DOMRect object for parent MessageList component */
  messageListRect: PropTypes.object
});

_defineProperty(MessageActionsBox, "defaultProps", {
  open: false
});

function ownKeys$1(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread$1(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys$1(source, true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys$1(source).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }
var ReactionsList =
/*#__PURE__*/
function (_React$Component) {
  _inherits(ReactionsList, _React$Component);

  function ReactionsList(props) {
    var _this;

    _classCallCheck(this, ReactionsList);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(ReactionsList).call(this, props));

    _defineProperty(_assertThisInitialized(_this), "_renderReactions", function (reactions) {
      var reactionsByType = {};
      reactions.map(function (item) {
        if (reactions[item.type] === undefined) {
          return reactionsByType[item.type] = [item];
        } else {
          return reactionsByType[item.type] = [].concat(_toConsumableArray(reactionsByType[item.type]), [item]);
        }
      });

      var reactionsEmojis = _this.props.reactionOptions.reduce(function (acc, cur) {
        return _objectSpread$1({}, acc, _defineProperty({}, cur.id, cur));
      }, {});

      return Object.keys(reactionsByType).map(function (type) {
        return reactionsEmojis[type] ? React__default.createElement("li", {
          key: reactionsEmojis[type].id
        }, React__default.createElement(emojiMart.NimbleEmoji, _extends({
          emoji: reactionsEmojis[type]
        }, emojiSetDef, {
          size: 16,
          data: emojiData
        })), ' ', "\xA0") : null;
      });
    });

    _defineProperty(_assertThisInitialized(_this), "_getReactionCount", function () {
      var reaction_counts = _this.props.reaction_counts;
      var count = null;

      if (reaction_counts !== null && reaction_counts !== undefined && Object.keys(reaction_counts).length > 0) {
        count = 0;
        Object.keys(reaction_counts).map(function (key) {
          return count += reaction_counts[key];
        });
      }

      return count;
    });

    return _this;
  }

  _createClass(ReactionsList, [{
    key: "render",
    value: function render() {
      return React__default.createElement("div", {
        className: "str-chat__reaction-list ".concat(this.props.reverse ? 'str-chat__reaction-list--reverse' : ''),
        onClick: this.props.onClick,
        ref: this.reactionList
      }, React__default.createElement("ul", null, this._renderReactions(this.props.reactions), React__default.createElement("li", null, React__default.createElement("span", {
        className: "str-chat__reaction-list--counter"
      }, this._getReactionCount()))));
    }
  }]);

  return ReactionsList;
}(React__default.Component);

_defineProperty(ReactionsList, "propTypes", {
  reactions: PropTypes.array,

  /** Provide a list of reaction options [{name: 'angry', emoji: 'angry'}] */
  reactionOptions: PropTypes.array,
  reverse: PropTypes.bool,
  reaction_counts: PropTypes.object
});

_defineProperty(ReactionsList, "defaultProps", {
  reactionOptions: defaultMinimalEmojis,
  emojiSetDef: emojiSetDef,
  reverse: false
});

var Tooltip =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(Tooltip, _React$PureComponent);

  function Tooltip() {
    _classCallCheck(this, Tooltip);

    return _possibleConstructorReturn(this, _getPrototypeOf(Tooltip).apply(this, arguments));
  }

  _createClass(Tooltip, [{
    key: "render",
    value: function render() {
      return React__default.createElement("div", {
        className: "str-chat__tooltip"
      }, this.props.children);
    }
  }]);

  return Tooltip;
}(React__default.PureComponent);

/**
 * LoadingIndicator - Just a simple loading spinner..
 *
 * @example ./docs/LoadingIndicator.md
 * @extends PureComponent
 */

var LoadingIndicator =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(LoadingIndicator, _React$PureComponent);

  function LoadingIndicator() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, LoadingIndicator);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(LoadingIndicator)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "stopRef", React__default.createRef());

    return _this;
  }

  _createClass(LoadingIndicator, [{
    key: "render",
    value: function render() {
      var _this$props = this.props,
          size = _this$props.size,
          color = _this$props.color;
      return React__default.createElement("div", {
        className: 'str-chat__loading-indicator ' + color,
        style: {
          width: size,
          height: size
        }
      }, React__default.createElement("svg", {
        width: size,
        height: size,
        viewBox: "0 0 30 30",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("defs", null, React__default.createElement("linearGradient", {
        x1: "50%",
        y1: "0%",
        x2: "50%",
        y2: "100%",
        id: "a"
      }, React__default.createElement("stop", {
        stopColor: "#FFF",
        stopOpacity: "0",
        offset: "0%"
      }), React__default.createElement("stop", {
        ref: this.stopRef,
        offset: "100%",
        stopColor: color,
        stopOpacity: "1",
        style: {
          stopColor: color
        }
      }))), React__default.createElement("path", {
        d: "M2.518 23.321l1.664-1.11A12.988 12.988 0 0 0 15 28c7.18 0 13-5.82 13-13S22.18 2 15 2V0c8.284 0 15 6.716 15 15 0 8.284-6.716 15-15 15-5.206 0-9.792-2.652-12.482-6.679z",
        fill: "url(#a)",
        fillRule: "evenodd"
      })));
    }
  }]);

  return LoadingIndicator;
}(React__default.PureComponent);

_defineProperty(LoadingIndicator, "propTypes", {
  /** The size of the loading icon */
  size: PropTypes.number,

  /** Set the color of the LoadingIndicator */
  color: PropTypes.string
});

_defineProperty(LoadingIndicator, "defaultProps", {
  size: 15,
  color: '#006CFF'
});

/**
 * Gallery - displays up to 6 images in a simple responsive grid with a lightbox to view the images.
 * @example ./docs/Gallery.md
 * @extends PureComponent
 */

var Gallery =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(Gallery, _React$PureComponent);

  function Gallery() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, Gallery);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(Gallery)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "state", {
      modalIsOpen: false,
      currentIndex: 0
    });

    _defineProperty(_assertThisInitialized(_this), "toggleModal", function (index) {
      _this.setState(function (state) {
        return {
          modalIsOpen: !state.modalIsOpen,
          currentIndex: index
        };
      });
    });

    return _this;
  }

  _createClass(Gallery, [{
    key: "render",
    value: function render() {
      var _this2 = this;

      var images = this.props.images;
      var formattedArray = images.map(function (image) {
        return {
          src: image.image_url || image.thumb_url
        };
      });
      return React__default.createElement("div", {
        className: "str-chat__gallery"
      }, images.slice(0, 3).map(function (image, i) {
        return React__default.createElement("div", {
          className: "str-chat__gallery-image",
          key: "gallery-image-".concat(i),
          onClick: function onClick() {
            return _this2.toggleModal(i);
          }
        }, React__default.createElement("img", {
          src: image.image_url || image.thumb_url
        }));
      }), images.length > 3 && React__default.createElement("div", {
        className: "str-chat__gallery-placeholder",
        style: {
          backgroundImage: "url(".concat(images[3].image_url, ")")
        },
        onClick: function onClick() {
          return _this2.toggleModal(3);
        }
      }, React__default.createElement("p", null, images.length - 3, " more")), React__default.createElement(Carousel.ModalGateway, null, this.state.modalIsOpen ? React__default.createElement(Carousel.Modal, {
        onClose: this.toggleModal,
        closeOnBackdropClick: true
      }, React__default.createElement(Carousel__default, {
        views: formattedArray,
        currentIndex: this.state.currentIndex
      })) : null));
    }
  }]);

  return Gallery;
}(React__default.PureComponent);

_defineProperty(Gallery, "propTypes", {
  images: PropTypes.array.isRequired
});

/**
 * ReactionSelector - A component for selecting a reaction. Examples are love, wow, like etc.
 *
 * @example ./docs/ReactionSelector.md
 * @extends PureComponent
 */

var ReactionSelector =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(ReactionSelector, _PureComponent);

  function ReactionSelector(props) {
    var _this;

    _classCallCheck(this, ReactionSelector);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(ReactionSelector).call(this, props));

    _defineProperty(_assertThisInitialized(_this), "showTooltip",
    /*#__PURE__*/
    function () {
      var _ref = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee2(e, users) {
        var target;
        return _regeneratorRuntime.wrap(function _callee2$(_context2) {
          while (1) {
            switch (_context2.prev = _context2.next) {
              case 0:
                target = e.target.getBoundingClientRect();
                _context2.next = 3;
                return _this.setState(function () {
                  return {
                    showTooltip: true,
                    users: users
                  };
                },
                /*#__PURE__*/
                _asyncToGenerator(
                /*#__PURE__*/
                _regeneratorRuntime.mark(function _callee() {
                  return _regeneratorRuntime.wrap(function _callee$(_context) {
                    while (1) {
                      switch (_context.prev = _context.next) {
                        case 0:
                          _context.next = 2;
                          return _this.setTooltipPosition(target);

                        case 2:
                          _context.next = 4;
                          return _this.setArrowPosition(target);

                        case 4:
                        case "end":
                          return _context.stop();
                      }
                    }
                  }, _callee);
                })));

              case 3:
              case "end":
                return _context2.stop();
            }
          }
        }, _callee2);
      }));

      return function (_x, _x2) {
        return _ref.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "hideTooltip", function () {
      _this.setState(function () {
        return {
          showTooltip: false,
          users: [],
          arrowPosition: 0,
          position: 0,
          positionCaculated: false
        };
      });
    });

    _defineProperty(_assertThisInitialized(_this), "getUsersPerReaction", function (reactions, type) {
      return reactions && reactions.filter(function (item) {
        return item.type === type;
      });
    });

    _defineProperty(_assertThisInitialized(_this), "getLatestUser", function (reactions, type) {
      var filtered = _this.getUsersPerReaction(reactions, type);

      if (filtered && filtered[0] && filtered[0].user) {
        return filtered[0].user;
      } else {
        return 'NotFound';
      }
    });

    _defineProperty(_assertThisInitialized(_this), "getUserNames", function (reactions, type) {
      var filtered = _this.getUsersPerReaction(reactions, type);

      return filtered && filtered.map(function (item) {
        return item.user || 'NotFound';
      });
    });

    _defineProperty(_assertThisInitialized(_this), "getContainerDimensions", function () {
      return _this.reactionSelector.current.getBoundingClientRect();
    });

    _defineProperty(_assertThisInitialized(_this), "getToolTipDimensions", function () {
      return _this.reactionSelectorTooltip.current.getBoundingClientRect();
    });

    _defineProperty(_assertThisInitialized(_this), "setTooltipPosition",
    /*#__PURE__*/
    function () {
      var _ref3 = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee3(target) {
        var container, tooltip, position;
        return _regeneratorRuntime.wrap(function _callee3$(_context3) {
          while (1) {
            switch (_context3.prev = _context3.next) {
              case 0:
                _context3.next = 2;
                return _this.getContainerDimensions();

              case 2:
                container = _context3.sent;
                _context3.next = 5;
                return _this.getToolTipDimensions();

              case 5:
                tooltip = _context3.sent;

                if (tooltip.width === container.width || tooltip.x < container.x) {
                  position = 0;
                } else {
                  position = target.left + target.width / 2 - container.left - tooltip.width / 2;
                }

                _context3.next = 9;
                return _this.setState(function () {
                  return {
                    position: position
                  };
                });

              case 9:
              case "end":
                return _context3.stop();
            }
          }
        }, _callee3);
      }));

      return function (_x3) {
        return _ref3.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "setArrowPosition",
    /*#__PURE__*/
    function () {
      var _ref4 = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee4(target) {
        var tooltip, position;
        return _regeneratorRuntime.wrap(function _callee4$(_context4) {
          while (1) {
            switch (_context4.prev = _context4.next) {
              case 0:
                tooltip = _this.reactionSelectorTooltip.current.getBoundingClientRect();
                position = target.x - tooltip.x + target.width / 2;
                _context4.next = 4;
                return _this.setState(function () {
                  return {
                    arrowPosition: position,
                    positionCaculated: true
                  };
                });

              case 4:
              case "end":
                return _context4.stop();
            }
          }
        }, _callee4);
      }));

      return function (_x4) {
        return _ref4.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "renderReactionItems", function () {
      var _this$props = _this.props,
          reaction_counts = _this$props.reaction_counts,
          latest_reactions = _this$props.latest_reactions;
      return _this.props.reactionOptions.map(function (reaction) {
        var users = _this.getUserNames(latest_reactions, reaction.id);

        var latestUser = _this.getLatestUser(latest_reactions, reaction.id);

        var count = reaction_counts && reaction_counts[reaction.id];
        return React__default.createElement("li", {
          key: "item-".concat(reaction.id),
          className: "str-chat__message-reactions-list-item",
          "data-text": reaction.id,
          onClick: _this.props.handleReaction.bind(_assertThisInitialized(_this), reaction.id)
        }, Boolean(count) && _this.props.detailedView && React__default.createElement(React__default.Fragment, null, React__default.createElement("div", {
          className: "latest-user",
          onMouseEnter: function onMouseEnter(e) {
            return _this.showTooltip(e, users);
          },
          onMouseLeave: _this.hideTooltip
        }, latestUser !== 'NotFound' ? React__default.createElement(Avatar, {
          image: latestUser.image,
          alt: latestUser.id,
          size: 20,
          name: latestUser.id
        }) : React__default.createElement("div", {
          className: "latest-user-not-found"
        }))), React__default.createElement(emojiMart.NimbleEmoji, _extends({
          emoji: reaction
        }, emojiSetDef, {
          data: emojiData
        })), Boolean(count) && _this.props.detailedView && React__default.createElement("span", {
          className: "str-chat__message-reactions-list-item__count"
        }, count || ''));
      });
    });

    _defineProperty(_assertThisInitialized(_this), "renderUsers", function (users) {
      return users.map(function (user, i) {
        var text = user.name || user.id;

        if (i + 1 < users.length) {
          text += ', ';
        }

        return React__default.createElement("span", {
          className: "latest-user-username",
          key: "key-".concat(i, "-").concat(user)
        }, text);
      });
    });

    _this.state = {
      showTooltip: false,
      users: [],
      position: 0,
      arrowPosition: 0,
      positionCalculated: false
    };
    _this.reactionSelector = React__default.createRef();
    _this.reactionSelectorInner = React__default.createRef();
    _this.reactionSelectorTooltip = React__default.createRef();
    return _this;
  }

  _createClass(ReactionSelector, [{
    key: "render",
    value: function render() {
      return React__default.createElement("div", {
        className: "str-chat__reaction-selector ".concat(this.props.reverse ? 'str-chat__reaction-selector--reverse' : ''),
        ref: this.reactionSelector
      }, this.props.detailedView && React__default.createElement("div", {
        className: "str-chat__reaction-selector-tooltip",
        ref: this.reactionSelectorTooltip,
        style: {
          visibility: this.state.showTooltip ? 'visible' : 'hidden',
          left: this.state.position,
          opacity: this.state.showTooltip && this.state.positionCaculated ? 1 : 0.01
        }
      }, React__default.createElement("div", {
        className: "arrow",
        style: {
          left: this.state.arrowPosition
        }
      }), this.renderUsers(this.state.users)), React__default.createElement("ul", {
        className: "str-chat__message-reactions-list"
      }, this.renderReactionItems()));
    }
  }]);

  return ReactionSelector;
}(React.PureComponent);

_defineProperty(ReactionSelector, "propTypes", {
  /**
   * Array of latest reactions.
   * Reaction object has following structure:
   *
   * ```json
   * {
   *  "type": "love",
   *  "user_id": "demo_user_id",
   *  "user": {
   *    ...userObject
   *  },
   *  "created_at": "datetime";
   * }
   * ```
   * */
  latest_reactions: PropTypes.array,

  /** Object/map of reaction id/type (e.g. 'like' | 'love' | 'haha' | 'wow' | 'sad' | 'angry') vs count */
  reaction_counts: PropTypes.object,

  /**
   * Callback to handle the reaction
   *
   * @param type e.g. 'like' | 'love' | 'haha' | 'wow' | 'sad' | 'angry'
   * */
  handleReaction: PropTypes.func.isRequired,

  /** Enable the avatar display */
  detailedView: PropTypes.bool,

  /** Provide a list of reaction options [{name: 'angry', emoji: 'angry'}] */
  reactionOptions: PropTypes.array,
  reverse: PropTypes.bool
});

_defineProperty(ReactionSelector, "defaultProps", {
  detailedView: true,
  reactionOptions: defaultMinimalEmojis,
  reverse: false,
  emojiSetDef: emojiSetDef
});

var MessageRepliesCountButton =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(MessageRepliesCountButton, _React$PureComponent);

  function MessageRepliesCountButton() {
    _classCallCheck(this, MessageRepliesCountButton);

    return _possibleConstructorReturn(this, _getPrototypeOf(MessageRepliesCountButton).apply(this, arguments));
  }

  _createClass(MessageRepliesCountButton, [{
    key: "render",
    value: function render() {
      var reply_count = this.props.reply_count;

      if (reply_count && reply_count !== 0) {
        return React__default.createElement("button", {
          className: "str-chat__message-replies-count-button",
          onClick: this.props.onClick
        }, React__default.createElement("svg", {
          width: "18",
          height: "15",
          xmlns: "http://www.w3.org/2000/svg"
        }, React__default.createElement("path", {
          d: "M.56 10.946H.06l-.002-.498L.025.92a.5.5 0 1 1 1-.004l.032 9.029H9.06v-4l9 4.5-9 4.5v-4H.56z",
          fillRule: "nonzero"
        })), React__default.createElement(reactIntl.FormattedMessage, {
          id: "message_replies.count_button",
          defaultMessage: "{count, plural, one {# reply} other {# replies}}",
          values: {
            count: reply_count
          }
        }));
      }

      return null;
    }
  }]);

  return MessageRepliesCountButton;
}(React__default.PureComponent);

_defineProperty(MessageRepliesCountButton, "propTypes", {
  reply_count: PropTypes.number,
  onClick: PropTypes.func
});

_defineProperty(MessageRepliesCountButton, "defaultProps", {
  reply_count: 0
});

var Modal =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(Modal, _React$PureComponent);

  function Modal() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, Modal);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(Modal)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "innerRef", React__default.createRef());

    _defineProperty(_assertThisInitialized(_this), "handleClick", function (e) {
      if (!_this.innerRef.current.contains(e.target)) {
        _this.props.onClose();

        document.removeEventListener('keyPress', _this.handleEscKey, false);
      }
    });

    return _this;
  }

  _createClass(Modal, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      document.addEventListener('keyPress', this.handleEscKey, false);
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      document.removeEventListener('keyPress', this.handleEscKey, false);
    }
  }, {
    key: "handleEscKey",
    value: function handleEscKey(e) {
      if (e.keyCode === 27) {
        this.props.onClose();
        document.removeEventListener('keyPress', this.handleEscKey, false);
      }
    }
  }, {
    key: "render",
    value: function render() {
      var openClasses = this.props.open ? 'str-chat__modal--open' : 'str-chat__modal--closed';
      return React__default.createElement("div", {
        className: "str-chat__modal ".concat(openClasses),
        onClick: this.handleClick
      }, React__default.createElement("div", {
        className: "str-chat__modal__close-button"
      }, React__default.createElement(reactIntl.FormattedMessage, {
        id: "modal.close",
        defaultMessage: "Close"
      }), React__default.createElement("svg", {
        width: "10",
        height: "10",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        d: "M9.916 1.027L8.973.084 5 4.058 1.027.084l-.943.943L4.058 5 .084 8.973l.943.943L5 5.942l3.973 3.974.943-.943L5.942 5z",
        fillRule: "evenodd"
      }))), React__default.createElement("div", {
        className: "str-chat__modal__inner",
        ref: this.innerRef
      }, this.props.children));
    }
  }]);

  return Modal;
}(React__default.PureComponent);

function ownKeys$2(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread$2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys$2(source, true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys$2(source).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }
var ChatContext = React__default.createContext({
  client: null
});
function withChatContext(OriginalComponent) {
  var ContextAwareComponent = function ContextComponent(props) {
    return React__default.createElement(ChatContext.Consumer, null, function (context) {
      var mergedProps = _objectSpread$2({}, context, {}, props);

      return React__default.createElement(OriginalComponent, mergedProps);
    });
  };

  ContextAwareComponent.displayName = OriginalComponent.displayName || OriginalComponent.name || 'Component';
  ContextAwareComponent.displayName = ContextAwareComponent.displayName.replace('Base', '');
  return ContextAwareComponent;
}
var ChannelContext = React__default.createContext({});
function withChannelContext(OriginalComponent) {
  var ContextAwareComponent = function ContextComponent(props) {
    return React__default.createElement(ChannelContext.Consumer, null, function (channelContext) {
      return React__default.createElement(OriginalComponent, _extends({}, channelContext, props));
    });
  };

  ContextAwareComponent.displayName = OriginalComponent.displayName || OriginalComponent.name || 'Component';
  ContextAwareComponent.displayName = ContextAwareComponent.displayName.replace('Base', '');
  return ContextAwareComponent;
}

var KEY_CODES = {
  ESC: 27,
  UP: 38,
  DOWN: 40,
  ENTER: 13,
  TAB: 9,
  SPACE: 32
}; // This is self-made key shortcuts manager, used for caching key strokes

var Listener = function Listener() {
  var _this = this;

  _classCallCheck(this, Listener);

  _defineProperty(this, "startListen", function () {
    if (!_this.refCount) {
      // prevent multiple listeners in case of multiple TextareaAutocomplete components on page
      document.addEventListener('keydown', _this.f);
    }

    _this.refCount++;
  });

  _defineProperty(this, "stopListen", function () {
    _this.refCount--;

    if (!_this.refCount) {
      // prevent disable listening in case of multiple TextareaAutocomplete components on page
      document.removeEventListener('keydown', _this.f);
    }
  });

  _defineProperty(this, "add", function (keyCodes, fn) {
    var keyCode = keyCodes;
    if (_typeof(keyCode) !== 'object') keyCode = [keyCode];
    _this.listeners[_this.index] = {
      keyCode: keyCode,
      fn: fn
    };
    _this.index += 1;
    return _this.index;
  });

  _defineProperty(this, "remove", function (id) {
    delete _this.listeners[id];
  });

  _defineProperty(this, "removeAll", function () {
    _this.listeners = {};
    _this.index = 0;
  });

  this.index = 0;
  this.listeners = {};
  this.refCount = 0;

  this.f = function (e) {
    var code = e.keyCode || e.which;
    Object.values(_this.listeners).forEach(function (_ref) {
      var keyCode = _ref.keyCode,
          fn = _ref.fn;
      if (keyCode.includes(code)) fn(e);
    });
  };
};

var Listeners = new Listener();

var Item =
/*#__PURE__*/
function (_React$Component) {
  _inherits(Item, _React$Component);

  function Item() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, Item);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(Item)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "selectItem", function () {
      var _this$props = _this.props,
          item = _this$props.item,
          onSelectHandler = _this$props.onSelectHandler;
      onSelectHandler(item);
    });

    return _this;
  }

  _createClass(Item, [{
    key: "render",
    value: function render() {
      var _this$props2 = this.props,
          Component = _this$props2.component,
          style = _this$props2.style,
          onClickHandler = _this$props2.onClickHandler,
          item = _this$props2.item,
          selected = _this$props2.selected,
          className = _this$props2.className,
          innerRef = _this$props2.innerRef;
      return React__default.createElement("li", {
        className: "rta__item ".concat(className || ''),
        style: style
      }, React__default.createElement("div", {
        className: "rta__entity ".concat(selected === true ? 'rta__entity--selected' : ''),
        role: "button",
        tabIndex: 0,
        onClick: onClickHandler,
        onFocus: this.selectItem,
        onMouseEnter: this.selectItem
        /* $FlowFixMe */
        ,
        ref: innerRef
      }, React__default.createElement(Component, {
        selected: selected,
        entity: item
      })));
    }
  }]);

  return Item;
}(React__default.Component);

var List =
/*#__PURE__*/
function (_React$Component) {
  _inherits(List, _React$Component);

  function List() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, List);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(List)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "state", {
      selectedItem: null
    });

    _defineProperty(_assertThisInitialized(_this), "cachedValues", null);

    _defineProperty(_assertThisInitialized(_this), "onPressEnter", function (e) {
      if (e && e.preventDefault) {
        e.preventDefault();
      }

      var values = _this.props.values;

      _this.modifyText(values[_this.getPositionInList()]);
    });

    _defineProperty(_assertThisInitialized(_this), "getPositionInList", function () {
      var values = _this.props.values;
      var selectedItem = _this.state.selectedItem;
      if (!selectedItem) return 0;
      return values.findIndex(function (a) {
        return _this.getId(a) === _this.getId(selectedItem);
      });
    });

    _defineProperty(_assertThisInitialized(_this), "getId", function (item) {
      var textToReplace = _this.props.getTextToReplace(item);

      if (textToReplace.key) {
        return textToReplace.key;
      }

      if (typeof item === 'string' || !item.key) {
        return textToReplace.text;
      }

      return item.key;
    });

    _defineProperty(_assertThisInitialized(_this), "listeners", []);

    _defineProperty(_assertThisInitialized(_this), "itemsRef", {});

    _defineProperty(_assertThisInitialized(_this), "modifyText", function (value) {
      if (!value) return;
      var _this$props = _this.props,
          onSelect = _this$props.onSelect,
          getTextToReplace = _this$props.getTextToReplace,
          getSelectedItem = _this$props.getSelectedItem;
      onSelect(getTextToReplace(value));

      if (getSelectedItem) {
        getSelectedItem(value);
      }
    });

    _defineProperty(_assertThisInitialized(_this), "selectItem", function (item) {
      var keyboard = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : false;

      _this.setState({
        selectedItem: item
      }, function () {
        if (keyboard) {
          _this.props.dropdownScroll(_this.itemsRef[_this.getId(item)]);
        }
      });
    });

    _defineProperty(_assertThisInitialized(_this), "scroll", function (e) {
      if (e && e.preventDefault) {
        e.preventDefault();
      }

      var values = _this.props.values;
      var code = e.keyCode || e.which;

      var oldPosition = _this.getPositionInList();

      var newPosition;

      switch (code) {
        case KEY_CODES.DOWN:
          newPosition = oldPosition + 1;
          break;

        case KEY_CODES.UP:
          newPosition = oldPosition - 1;
          break;

        default:
          newPosition = oldPosition;
          break;
      }

      newPosition = (newPosition % values.length + values.length) % values.length; // eslint-disable-line

      _this.selectItem(values[newPosition], [KEY_CODES.DOWN, KEY_CODES.UP].includes(code));
    });

    _defineProperty(_assertThisInitialized(_this), "isSelected", function (item) {
      var selectedItem = _this.state.selectedItem;
      if (!selectedItem) return false;
      return _this.getId(selectedItem) === _this.getId(item);
    });

    _defineProperty(_assertThisInitialized(_this), "renderHeader", function (value) {
      if (value[0] === '/') {
        return "Commands matching <strong>".concat(value.replace('/', ''), "</strong>");
      }

      if (value[0] === ':') {
        return "Emoji matching <strong>".concat(value.replace(':', ''), "</strong>");
      }

      if (value[0] === '@') {
        return "Searching for people";
      }
    });

    return _this;
  }

  _createClass(List, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      this.listeners.push(Listeners.add([KEY_CODES.DOWN, KEY_CODES.UP], this.scroll), Listeners.add([KEY_CODES.ENTER, KEY_CODES.TAB], this.onPressEnter));
      var values = this.props.values;
      if (values && values[0]) this.selectItem(values[0]);
    }
  }, {
    key: "UNSAFE_componentWillReceiveProps",
    value: function UNSAFE_componentWillReceiveProps(_ref) {
      var _this2 = this;

      var values = _ref.values;
      var newValues = values.map(function (val) {
        return _this2.getId(val);
      }).join('');

      if (this.cachedValues !== newValues && values && values[0]) {
        this.selectItem(values[0]);
        this.cachedValues = newValues;
      }
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      var listener;

      while (this.listeners.length) {
        listener = this.listeners.pop();
        Listeners.remove(listener);
      }
    }
  }, {
    key: "render",
    value: function render() {
      var _this3 = this;

      var _this$props2 = this.props,
          values = _this$props2.values,
          component = _this$props2.component,
          style = _this$props2.style,
          itemClassName = _this$props2.itemClassName,
          className = _this$props2.className,
          itemStyle = _this$props2.itemStyle;
      return React__default.createElement("ul", {
        className: "rta__list ".concat(className || ''),
        style: style
      }, React__default.createElement("li", {
        className: "rta__list-header",
        dangerouslySetInnerHTML: {
          __html: this.renderHeader(this.props.value)
        }
      }), values.map(function (item) {
        return React__default.createElement(Item, {
          key: _this3.getId(item),
          innerRef: function innerRef(ref) {
            _this3.itemsRef[_this3.getId(item)] = ref;
          },
          selected: _this3.isSelected(item),
          item: item,
          className: itemClassName,
          style: itemStyle,
          onClickHandler: _this3.onPressEnter,
          onSelectHandler: _this3.selectItem,
          component: component
        });
      }));
    }
  }]);

  return List;
}(React__default.Component);

//
function defaultScrollToItem(container, item) {
  var itemHeight = parseInt(getComputedStyle(item).getPropertyValue('height'), 10);
  var containerHight = parseInt(getComputedStyle(container).getPropertyValue('height'), 10) - itemHeight;
  var itemOffsetTop = item.offsetTop;
  var actualScrollTop = container.scrollTop;

  if (itemOffsetTop < actualScrollTop + containerHight && actualScrollTop < itemOffsetTop) {
    return;
  } // eslint-disable-next-line


  container.scrollTop = itemOffsetTop;
}

function ownKeys$3(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread$3(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys$3(source, true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys$3(source).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }
var DEFAULT_CARET_POSITION = 'next';

var errorMessage = function errorMessage(message) {
  return console.error("RTA: dataProvider fails: ".concat(message, "\n    \nCheck the documentation or create issue if you think it's bug. https://github.com/webscopeio/react-textarea-autocomplete/issues"));
};

var ReactTextareaAutocomplete =
/*#__PURE__*/
function (_React$Component) {
  _inherits(ReactTextareaAutocomplete, _React$Component);

  function ReactTextareaAutocomplete(_props) {
    var _this;

    _classCallCheck(this, ReactTextareaAutocomplete);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(ReactTextareaAutocomplete).call(this, _props));

    _defineProperty(_assertThisInitialized(_this), "getSelectionPosition", function () {
      if (!_this.textareaRef) return null;
      return {
        selectionStart: _this.textareaRef.selectionStart,
        selectionEnd: _this.textareaRef.selectionEnd
      };
    });

    _defineProperty(_assertThisInitialized(_this), "getSelectedText", function () {
      if (!_this.textareaRef) return null;
      var _this$textareaRef = _this.textareaRef,
          selectionStart = _this$textareaRef.selectionStart,
          selectionEnd = _this$textareaRef.selectionEnd;
      if (selectionStart === selectionEnd) return null;
      return _this.state.value.substr(selectionStart, selectionEnd - selectionStart);
    });

    _defineProperty(_assertThisInitialized(_this), "setCaretPosition", function () {
      var position = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : 0;
      if (!_this.textareaRef) return;

      _this.textareaRef.focus();

      _this.textareaRef.setSelectionRange(position, position);
    });

    _defineProperty(_assertThisInitialized(_this), "getCaretPosition", function () {
      if (!_this.textareaRef) {
        return 0;
      }

      return _this.textareaRef.selectionEnd;
    });

    _defineProperty(_assertThisInitialized(_this), "_onEnter", function (event) {
      var trigger = _this.state.currentTrigger;

      if (!_this.textareaRef) {
        return;
      }

      var hasFocus = _this.textareaRef.matches(':focus'); // don't submit if the element has focus or the shift key is pressed


      if (!hasFocus || event.shiftKey === true) {
        return;
      }

      if (!trigger) {
        // trigger a submit
        _this._replaceWord();

        if (_this.textareaRef) {
          _this.textareaRef.selectionEnd = 0;
        }

        _this.props.handleSubmit(event);
      }
    });

    _defineProperty(_assertThisInitialized(_this), "_onSpace", function () {
      if (!_this.props.replaceWord) {
        return;
      }

      if (!_this.textareaRef) {
        return;
      }

      var hasFocus = _this.textareaRef.matches(':focus'); // don't change characters if the element doesn't have focus


      if (!hasFocus) {
        return;
      }

      _this._replaceWord();
    });

    _defineProperty(_assertThisInitialized(_this), "_replaceWord", function () {
      var lastWordRegex = /([^\s]+)(\s*)$/;
      var value = _this.state.value;
      var match = lastWordRegex.exec(value.slice(0, _this.getCaretPosition()));
      var lastWord = match && match[1];

      if (!lastWord) {
        return;
      }

      var spaces = match[2];

      var newWord = _this.props.replaceWord(lastWord);

      if (newWord == null) {
        return;
      }

      var textBeforeWord = value.slice(0, _this.getCaretPosition() - match[0].length);
      var textAfterCaret = value.slice(_this.getCaretPosition(), -1);
      var newText = textBeforeWord + newWord + spaces + textAfterCaret;

      _this.setState({
        value: newText
      }, function () {
        // fire onChange event after successful selection
        var e = new CustomEvent('change', {
          bubbles: true
        });

        _this.textareaRef.dispatchEvent(e);

        if (_this.props.onChange) _this.props.onChange(e);
      });
    });

    _defineProperty(_assertThisInitialized(_this), "_onSelect", function (newToken) {
      var _this$state = _this.state,
          selectionEnd = _this$state.selectionEnd,
          currentTrigger = _this$state.currentTrigger,
          textareaValue = _this$state.value;
      var _this$props = _this.props,
          onChange = _this$props.onChange,
          trigger = _this$props.trigger;
      if (!currentTrigger) return;

      var computeCaretPosition = function computeCaretPosition(position, token, startToken) {
        switch (position) {
          case 'start':
            return startToken;

          case 'next':
          case 'end':
            return startToken + token.length;

          default:
            if (!Number.isInteger(position)) {
              throw new Error('RTA: caretPosition should be "start", "next", "end" or number.');
            }

            return position;
        }
      };

      var textToModify = textareaValue.slice(0, selectionEnd);
      var startOfTokenPosition = textToModify.search(
      /**
       * It's important to escape the currentTrigger char for chars like [, (,...
       */
      new RegExp("\\".concat(currentTrigger, "[^\\".concat(currentTrigger).concat(trigger[currentTrigger].allowWhitespace ? '' : '\\s', "]"), "*$"))); // we add space after emoji is selected if a caret position is next

      var newTokenString = newToken.caretPosition === 'next' ? "".concat(newToken.text, " ") : newToken.text;
      var newCaretPosition = computeCaretPosition(newToken.caretPosition, newTokenString, startOfTokenPosition);
      var modifiedText = textToModify.substring(0, startOfTokenPosition) + newTokenString; // set the new textarea value and after that set the caret back to its position

      _this.setState({
        value: textareaValue.replace(textToModify, modifiedText),
        dataLoading: false
      }, function () {
        // fire onChange event after successful selection
        var e = new CustomEvent('change', {
          bubbles: true
        });

        _this.textareaRef.dispatchEvent(e);

        if (onChange) onChange(e);

        _this.setCaretPosition(newCaretPosition);
      });

      _this._closeAutocomplete();
    });

    _defineProperty(_assertThisInitialized(_this), "_getItemOnSelect", function () {
      var currentTrigger = _this.state.currentTrigger;

      var triggerSettings = _this._getCurrentTriggerSettings();

      if (!currentTrigger || !triggerSettings) return null;
      var callback = triggerSettings.callback;
      if (!callback) return null;
      return function (item) {
        if (typeof callback !== 'function') {
          throw new Error('Output functor is not defined! You have to define "output" function. https://github.com/webscopeio/react-textarea-autocomplete#trigger-type');
        }

        if (callback) {
          return callback(item, currentTrigger);
        }

        return null;
      };
    });

    _defineProperty(_assertThisInitialized(_this), "_getTextToReplace", function () {
      var _this$state2 = _this.state,
          currentTrigger = _this$state2.currentTrigger,
          actualToken = _this$state2.actualToken;

      var triggerSettings = _this._getCurrentTriggerSettings();

      if (!currentTrigger || !triggerSettings) return null;
      var output = triggerSettings.output;
      return function (item) {
        if (_typeof(item) === 'object' && (!output || typeof output !== 'function')) {
          throw new Error('Output functor is not defined! If you are using items as object you have to define "output" function. https://github.com/webscopeio/react-textarea-autocomplete#trigger-type');
        }

        if (output) {
          var textToReplace = output(item, currentTrigger);

          if (!textToReplace || typeof textToReplace === 'number') {
            throw new Error("Output functor should return string or object in shape {text: string, caretPosition: string | number}.\nGot \"".concat(String(textToReplace), "\". Check the implementation for trigger \"").concat(currentTrigger, "\" and its token \"").concat(actualToken, "\"\n\nSee https://github.com/webscopeio/react-textarea-autocomplete#trigger-type for more informations.\n"));
          }

          if (typeof textToReplace === 'string') {
            return {
              text: textToReplace,
              caretPosition: DEFAULT_CARET_POSITION
            };
          }

          if (!textToReplace.text) {
            throw new Error("Output \"text\" is not defined! Object should has shape {text: string, caretPosition: string | number}. Check the implementation for trigger \"".concat(currentTrigger, "\" and its token \"").concat(actualToken, "\"\n"));
          }

          if (!textToReplace.caretPosition) {
            throw new Error("Output \"caretPosition\" is not defined! Object should has shape {text: string, caretPosition: string | number}. Check the implementation for trigger \"".concat(currentTrigger, "\" and its token \"").concat(actualToken, "\"\n"));
          }

          return textToReplace;
        }

        if (typeof item !== 'string') {
          throw new Error('Output item should be string\n');
        }

        return {
          text: "".concat(currentTrigger).concat(item).concat(currentTrigger),
          caretPosition: DEFAULT_CARET_POSITION
        };
      };
    });

    _defineProperty(_assertThisInitialized(_this), "_getCurrentTriggerSettings", function () {
      var currentTrigger = _this.state.currentTrigger;
      if (!currentTrigger) return null;
      return _this.props.trigger[currentTrigger];
    });

    _defineProperty(_assertThisInitialized(_this), "_getValuesFromProvider", function () {
      var _this$state3 = _this.state,
          currentTrigger = _this$state3.currentTrigger,
          actualToken = _this$state3.actualToken;

      var triggerSettings = _this._getCurrentTriggerSettings();

      if (!currentTrigger || !triggerSettings) {
        return;
      }

      var dataProvider = triggerSettings.dataProvider,
          component = triggerSettings.component;

      if (typeof dataProvider !== 'function') {
        throw new Error('Trigger provider has to be a function!');
      }

      _this.setState({
        dataLoading: true
      }); // Modified: send the full text to support / style commands


      var providedData = dataProvider(actualToken, _this.state.value);

      if (!(providedData instanceof Promise)) {
        providedData = Promise.resolve(providedData);
      }

      providedData.then(function (data$$1) {
        if (!Array.isArray(data$$1)) {
          throw new Error('Trigger provider has to provide an array!');
        }

        if (typeof component !== 'function') {
          throw new Error('Component should be defined!');
        } // throw away if we resolved old trigger


        if (currentTrigger !== _this.state.currentTrigger) return; // if we haven't resolved any data let's close the autocomplete

        if (!data$$1.length) {
          _this._closeAutocomplete();

          return;
        }

        _this.setState({
          dataLoading: false,
          data: data$$1,
          component: component
        });
      }).catch(function (e) {
        return errorMessage(e.message);
      });
    });

    _defineProperty(_assertThisInitialized(_this), "_getSuggestions", function () {
      var _this$state4 = _this.state,
          currentTrigger = _this$state4.currentTrigger,
          data$$1 = _this$state4.data;
      if (!currentTrigger || !data$$1 || data$$1 && !data$$1.length) return null;
      return data$$1;
    });

    _defineProperty(_assertThisInitialized(_this), "_createRegExp", function () {
      var trigger = _this.props.trigger; // negative lookahead to match only the trigger + the actual token = "bladhwd:adawd:word test" => ":word"
      // https://stackoverflow.com/a/8057827/2719917

      _this.tokenRegExp = new RegExp("([".concat(Object.keys(trigger).join(''), "])(?:(?!\\1)[^\\s])*$"));
    });

    _defineProperty(_assertThisInitialized(_this), "_closeAutocomplete", function () {
      _this.setState({
        data: null,
        dataLoading: false,
        currentTrigger: null,
        top: null,
        left: null
      });
    });

    _defineProperty(_assertThisInitialized(_this), "_cleanUpProps", function () {
      var props = _objectSpread$3({}, _this.props);

      var notSafe = ['loadingComponent', 'containerStyle', 'minChar', 'scrollToItem', 'ref', 'innerRef', 'onChange', 'onCaretPositionChange', 'className', 'value', 'trigger', 'listStyle', 'itemStyle', 'containerStyle', 'loaderStyle', 'className', 'containerClassName', 'listClassName', 'itemClassName', 'loaderClassName', 'closeOnClickOutside', 'dropdownStyle', 'dropdownClassName', 'movePopupAsYouType', 'handleSubmit', 'replaceWord', 'grow']; // eslint-disable-next-line

      for (var prop in props) {
        if (notSafe.includes(prop)) delete props[prop];
      }

      return props;
    });

    _defineProperty(_assertThisInitialized(_this), "_changeHandler", function (e) {
      var _this$props2 = _this.props,
          trigger = _this$props2.trigger,
          onChange = _this$props2.onChange,
          minChar = _this$props2.minChar,
          onCaretPositionChange = _this$props2.onCaretPositionChange,
          movePopupAsYouType = _this$props2.movePopupAsYouType;
      var _this$state5 = _this.state,
          top = _this$state5.top,
          left = _this$state5.left;
      var textarea = e.target;
      var selectionEnd = textarea.selectionEnd,
          selectionStart = textarea.selectionStart;
      var value = textarea.value;

      if (onChange) {
        e.persist();
        onChange(e);
      }

      if (onCaretPositionChange) {
        var caretPosition = _this.getCaretPosition();

        onCaretPositionChange(caretPosition);
      }

      _this.setState({
        value: value
      });

      var tokenMatch = _this.tokenRegExp.exec(value.slice(0, selectionEnd));

      var lastToken = tokenMatch && tokenMatch[0];
      var currentTrigger = lastToken && Object.keys(trigger).find(function (a) {
        return a === lastToken[0];
      }) || null;
      /*
       if we lost the trigger token or there is no following character we want to close
       the autocomplete
      */

      if ((!lastToken || lastToken.length <= minChar) && ( // check if our current trigger disallows whitespace
      _this.state.currentTrigger && !trigger[_this.state.currentTrigger].allowWhitespace || !_this.state.currentTrigger)) {
        _this._closeAutocomplete();

        return;
      }
      /**
       * This code has to be sync that is the reason why we obtain the currentTrigger
       * from currentTrigger not this.state.currentTrigger
       *
       * Check if the currently typed token has to be afterWhitespace, or not.
       */


      if (currentTrigger && value[tokenMatch.index - 1] && trigger[currentTrigger].afterWhitespace && !value[tokenMatch.index - 1].match(/\s/)) {
        _this._closeAutocomplete();

        return;
      }
      /**
        If our current trigger allows whitespace
        get the correct token for DataProvider, so we need to construct new RegExp
       */


      if (_this.state.currentTrigger && trigger[_this.state.currentTrigger].allowWhitespace) {
        tokenMatch = new RegExp("\\".concat(_this.state.currentTrigger, "[^").concat(_this.state.currentTrigger, "]*$")).exec(value.slice(0, selectionEnd));
        lastToken = tokenMatch && tokenMatch[0];

        if (!lastToken) {
          _this._closeAutocomplete();

          return;
        }

        currentTrigger = Object.keys(trigger).find(function (a) {
          return a === lastToken[0];
        }) || null;
      }

      var actualToken = lastToken.slice(1); // if trigger is not configured step out from the function, otherwise proceed

      if (!currentTrigger) {
        return;
      }

      if (movePopupAsYouType || top === null && left === null || // if we have single char - trigger it means we want to re-position the autocomplete
      lastToken.length === 1) {
        var _getCaretCoordinates = getCaretCoordinates(textarea, selectionEnd),
            newTop = _getCaretCoordinates.top,
            newLeft = _getCaretCoordinates.left;

        _this.setState({
          // make position relative to textarea
          top: newTop - _this.textareaRef.scrollTop || 0,
          left: newLeft
        });
      }

      _this.setState({
        selectionEnd: selectionEnd,
        selectionStart: selectionStart,
        currentTrigger: currentTrigger,
        actualToken: actualToken
      }, function () {
        try {
          _this._getValuesFromProvider();
        } catch (err) {
          errorMessage(err.message);
        }
      });
    });

    _defineProperty(_assertThisInitialized(_this), "_selectHandler", function (e) {
      var _this$props3 = _this.props,
          onCaretPositionChange = _this$props3.onCaretPositionChange,
          onSelect = _this$props3.onSelect;

      if (onCaretPositionChange) {
        var caretPosition = _this.getCaretPosition();

        onCaretPositionChange(caretPosition);
      }

      if (onSelect) {
        e.persist();
        onSelect(e);
      }
    });

    _defineProperty(_assertThisInitialized(_this), "_onClickAndBlurHandler", function (e) {
      var _this$props4 = _this.props,
          closeOnClickOutside = _this$props4.closeOnClickOutside,
          onBlur = _this$props4.onBlur; // If this is a click: e.target is the textarea, and e.relatedTarget is the thing
      // that was actually clicked. If we clicked inside the autoselect dropdown, then
      // that's not a blur, from the autoselect's point of view, so then do nothing.

      var el = e.relatedTarget;

      if (_this.dropdownRef && el instanceof Node && _this.dropdownRef.contains(el)) {
        return;
      }

      if (closeOnClickOutside) {
        _this._closeAutocomplete();
      }

      if (onBlur) {
        e.persist();
        onBlur(e);
      }
    });

    _defineProperty(_assertThisInitialized(_this), "_onScrollHandler", function () {
      _this._closeAutocomplete();
    });

    _defineProperty(_assertThisInitialized(_this), "_dropdownScroll", function (item) {
      var scrollToItem = _this.props.scrollToItem;
      if (!scrollToItem) return;

      if (scrollToItem === true) {
        defaultScrollToItem(_this.dropdownRef, item);
        return;
      }

      if (typeof scrollToItem !== 'function' || scrollToItem.length !== 2) {
        throw new Error('`scrollToItem` has to be boolean (true for default implementation) or function with two parameters: container, item.');
      }

      scrollToItem(_this.dropdownRef, item);
    });

    var _this$props5 = _this.props,
        loadingComponent = _this$props5.loadingComponent,
        _trigger = _this$props5.trigger,
        _value = _this$props5.value; // TODO: it would be better to have the parent control state...
    // if (value) this.state.value = value;

    _this._createRegExp();

    if (!loadingComponent) {
      throw new Error('RTA: loadingComponent is not defined');
    }

    if (!_trigger) {
      throw new Error('RTA: trigger is not defined');
    }

    _this.state = {
      top: null,
      left: null,
      currentTrigger: null,
      actualToken: '',
      data: null,
      value: _value || '',
      dataLoading: false,
      selectionEnd: 0,
      selectionStart: 0,
      component: null,
      listenerIndex: 0
    };
    return _this;
  }

  _createClass(ReactTextareaAutocomplete, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      var _this2 = this;

      Listeners.add(KEY_CODES.ESC, function () {
        return _this2._closeAutocomplete();
      });
      Listeners.add(KEY_CODES.SPACE, function () {
        return _this2._onSpace();
      });
      var listenerIndex = Listeners.add(KEY_CODES.ENTER, function (e) {
        return _this2._onEnter(e);
      });
      this.setState({
        listenerIndex: listenerIndex
      });
      Listeners.startListen();
    }
  }, {
    key: "UNSAFE_componentWillReceiveProps",
    value: function UNSAFE_componentWillReceiveProps(nextProps) {
      this._update(nextProps);
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      Listeners.stopListen();
      Listeners.remove(this.state.listenerIndex);
    }
  }, {
    key: "_update",
    // TODO: This is an anti pattern in react, should come up with a better way
    value: function _update(_ref) {
      var value = _ref.value,
          trigger = _ref.trigger;
      var oldValue = this.state.value;
      var oldTrigger = this.props.trigger;
      if (value !== oldValue || !oldValue) this.setState({
        value: value
      });
      /**
       * check if trigger chars are changed, if so, change the regexp accordingly
       */

      if (Object.keys(trigger).join('') !== Object.keys(oldTrigger).join('')) {
        this._createRegExp();
      }
    }
    /**
     * Close autocomplete, also clean up trigger (to avoid slow promises)
     */

  }, {
    key: "render",
    value: function render() {
      var _this3 = this;

      var _this$props6 = this.props,
          Loader = _this$props6.loadingComponent,
          style = _this$props6.style,
          className = _this$props6.className,
          itemStyle = _this$props6.itemStyle,
          listClassName = _this$props6.listClassName,
          itemClassName = _this$props6.itemClassName,
          dropdownClassName = _this$props6.dropdownClassName,
          dropdownStyle = _this$props6.dropdownStyle,
          containerStyle = _this$props6.containerStyle,
          containerClassName = _this$props6.containerClassName,
          loaderStyle = _this$props6.loaderStyle,
          loaderClassName = _this$props6.loaderClassName;
      var _this$state6 = this.state,
          dataLoading = _this$state6.dataLoading,
          currentTrigger = _this$state6.currentTrigger,
          component = _this$state6.component,
          value = _this$state6.value;

      var suggestionData = this._getSuggestions();

      var textToReplace = this._getTextToReplace();

      var selectedItem = this._getItemOnSelect();

      var maxRows = this.props.maxRows;

      if (!this.props.grow) {
        maxRows = 1;
      }

      return React__default.createElement("div", {
        className: "rta ".concat(dataLoading === true ? 'rta--loading' : '', " ").concat(containerClassName || ''),
        style: containerStyle
      }, (dataLoading || suggestionData) && currentTrigger && React__default.createElement("div", {
        ref: function ref(_ref2) {
          // $FlowFixMe
          _this3.dropdownRef = _ref2;
        },
        style: _objectSpread$3({}, dropdownStyle),
        className: "rta__autocomplete ".concat(dropdownClassName || '')
      }, suggestionData && component && textToReplace && React__default.createElement(List, {
        value: value,
        values: suggestionData,
        component: component,
        className: listClassName,
        itemClassName: itemClassName,
        itemStyle: itemStyle,
        getTextToReplace: textToReplace,
        getSelectedItem: selectedItem,
        onSelect: this._onSelect,
        dropdownScroll: this._dropdownScroll
      }), dataLoading && React__default.createElement("div", {
        className: "rta__loader ".concat(suggestionData !== null ? 'rta__loader--suggestion-data' : 'rta__loader--empty-suggestion-data', " ").concat(loaderClassName || ''),
        style: loaderStyle
      }, React__default.createElement(Loader, {
        data: suggestionData
      }))), React__default.createElement(Textarea, _extends({}, this._cleanUpProps(), {
        inputRef: function inputRef(ref) {
          _this3.props.innerRef && _this3.props.innerRef(ref);
          _this3.textareaRef = ref;
        },
        maxRows: maxRows,
        className: "rta__textarea ".concat(className || ''),
        onChange: this._changeHandler,
        onSelect: this._selectHandler,
        onScroll: this._onScrollHandler,
        onClick: // The textarea itself is outside the autoselect dropdown.
        this._onClickAndBlurHandler,
        onBlur: this._onClickAndBlurHandler,
        onFocus: this.props.onFocus,
        value: value,
        style: style
      })));
    }
  }]);

  return ReactTextareaAutocomplete;
}(React__default.Component);

_defineProperty(ReactTextareaAutocomplete, "defaultProps", {
  closeOnClickOutside: true,
  movePopupAsYouType: false,
  value: '',
  minChar: 1,
  scrollToItem: true,
  maxRows: 10
});

var triggerPropsCheck = function triggerPropsCheck(_ref3) {
  var trigger = _ref3.trigger;
  if (!trigger) return Error('Invalid prop trigger. Prop missing.');
  var triggers = Object.entries(trigger);

  for (var i = 0; i < triggers.length; i += 1) {
    var _triggers$i = _slicedToArray(triggers[i], 2),
        triggerChar = _triggers$i[0],
        settings = _triggers$i[1];

    if (typeof triggerChar !== 'string' || triggerChar.length !== 1) {
      return Error('Invalid prop trigger. Keys of the object has to be string / one character.');
    } // $FlowFixMe


    var triggerSetting = settings;
    var component = triggerSetting.component,
        dataProvider = triggerSetting.dataProvider,
        output = triggerSetting.output,
        callback = triggerSetting.callback,
        afterWhitespace = triggerSetting.afterWhitespace,
        allowWhitespace = triggerSetting.allowWhitespace;

    if (!component || typeof component !== 'function') {
      return Error('Invalid prop trigger: component should be defined.');
    }

    if (!dataProvider || typeof dataProvider !== 'function') {
      return Error('Invalid prop trigger: dataProvider should be defined.');
    }

    if (output && typeof output !== 'function') {
      return Error('Invalid prop trigger: output should be a function.');
    }

    if (callback && typeof callback !== 'function') {
      return Error('Invalid prop trigger: callback should be a function.');
    }

    if (afterWhitespace && allowWhitespace) {
      return Error('Invalid prop trigger: afterWhitespace and allowWhitespace can be used together');
    }
  }

  return null;
};

ReactTextareaAutocomplete.propTypes = {
  loadingComponent: PropTypes.func.isRequired,
  minChar: PropTypes.number,
  onChange: PropTypes.func,
  onSelect: PropTypes.func,
  onBlur: PropTypes.func,
  onCaretPositionChange: PropTypes.func,
  className: PropTypes.string,
  containerStyle: PropTypes.object,
  containerClassName: PropTypes.string,
  closeOnClickOutside: PropTypes.bool,
  style: PropTypes.object,
  listStyle: PropTypes.object,
  itemStyle: PropTypes.object,
  loaderStyle: PropTypes.object,
  dropdownStyle: PropTypes.object,
  listClassName: PropTypes.string,
  itemClassName: PropTypes.string,
  loaderClassName: PropTypes.string,
  dropdownClassName: PropTypes.string,
  value: PropTypes.string,
  trigger: triggerPropsCheck //eslint-disable-line

};

var EmoticonItem =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(EmoticonItem, _PureComponent);

  function EmoticonItem() {
    _classCallCheck(this, EmoticonItem);

    return _possibleConstructorReturn(this, _getPrototypeOf(EmoticonItem).apply(this, arguments));
  }

  _createClass(EmoticonItem, [{
    key: "render",
    value: function render() {
      return React__default.createElement("div", {
        className: "str-chat__emoji-item"
      }, React__default.createElement("span", {
        className: "str-chat__emoji-item--entity"
      }, this.props.entity.native), React__default.createElement("span", {
        className: "str-chat__emoji-item--name"
      }, this.props.entity.name), React__default.createElement("span", {
        className: "str-chat__emoji-item--char"
      }, this.props.entity.char));
    }
  }]);

  return EmoticonItem;
}(React.PureComponent);

var UserItem =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(UserItem, _PureComponent);

  function UserItem() {
    _classCallCheck(this, UserItem);

    return _possibleConstructorReturn(this, _getPrototypeOf(UserItem).apply(this, arguments));
  }

  _createClass(UserItem, [{
    key: "render",
    value: function render() {
      var u = this.props.entity;
      return React__default.createElement("div", {
        className: "str-chat__user-item"
      }, React__default.createElement(Avatar, {
        size: 20,
        image: u.image
      }), React__default.createElement("div", null, React__default.createElement("strong", null, u.name), " ", !u.name ? u.id : ''));
    }
  }]);

  return UserItem;
}(React.PureComponent);

var CommandItem =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(CommandItem, _PureComponent);

  function CommandItem() {
    _classCallCheck(this, CommandItem);

    return _possibleConstructorReturn(this, _getPrototypeOf(CommandItem).apply(this, arguments));
  }

  _createClass(CommandItem, [{
    key: "render",
    value: function render() {
      var c = this.props.entity;
      return React__default.createElement("div", {
        className: "str-chat__slash-command"
      }, React__default.createElement("span", {
        className: "str-chat__slash-command-header"
      }, React__default.createElement("strong", null, c.name), " ", c.args), React__default.createElement("br", null), React__default.createElement("span", {
        className: "str-chat__slash-command-description"
      }, c.description));
    }
  }]);

  return CommandItem;
}(React.PureComponent);

/**
 * Textarea component with included autocomplete options. You can set your own commands and
 * @example ./docs/ChatAutoComplete.md
 */

var ChatAutoComplete =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(ChatAutoComplete, _PureComponent);

  function ChatAutoComplete() {
    _classCallCheck(this, ChatAutoComplete);

    return _possibleConstructorReturn(this, _getPrototypeOf(ChatAutoComplete).apply(this, arguments));
  }

  _createClass(ChatAutoComplete, [{
    key: "getTriggers",
    value: function getTriggers() {
      var _this = this;

      return {
        ':': {
          dataProvider: function dataProvider(q) {
            if (q.length === 0 || q.charAt(0).match(/[^a-zA-Z0-9+-]/)) {
              return [];
            }

            var emojis = emojiMart.emojiIndex.search(q) || [];
            return emojis.slice(0, 10);
          },
          component: EmoticonItem,
          output: function output(entity) {
            return {
              key: entity.id,
              text: "".concat(entity.native),
              caretPosition: 'next'
            };
          }
        },
        '@': {
          dataProvider: function dataProvider(q) {
            var matchingUsers = _this.props.users.filter(function (user) {
              if (user.name !== undefined && user.name.toLowerCase().indexOf(q.toLowerCase()) !== -1) {
                return true;
              } else if (user.id.toLowerCase().indexOf(q.toLowerCase()) !== -1) {
                return true;
              } else {
                return false;
              }
            });

            return matchingUsers.slice(0, 10);
          },
          component: UserItem,
          output: function output(entity) {
            return {
              key: entity.id,
              text: "@".concat(entity.name || entity.id),
              caretPosition: 'next'
            };
          },
          callback: function callback(item) {
            return _this.props.onSelectItem(item);
          }
        },
        '/': {
          dataProvider: function dataProvider(q, text) {
            if (text.indexOf('/') !== 0) {
              return [];
            }

            var selectedCommands = _this.props.commands.filter(function (c) {
              return c.name.indexOf(q) !== -1;
            }); // sort alphabetically unless the you're matching the first char


            selectedCommands.sort(function (a, b) {
              var nameA = a.name.toLowerCase();
              var nameB = b.name.toLowerCase();

              if (nameA.indexOf(q) === 0) {
                nameA = "0".concat(nameA);
              }

              if (nameB.indexOf(q) === 0) {
                nameB = "0".concat(nameB);
              }

              if (nameA < nameB) {
                return -1;
              }

              if (nameA > nameB) {
                return 1;
              }

              return 0;
            });
            return selectedCommands.slice(0, 10);
          },
          component: CommandItem,
          output: function output(entity) {
            return {
              key: entity.id,
              text: "/".concat(entity.name),
              caretPosition: 'next'
            };
          }
        }
      };
    }
  }, {
    key: "emojiReplace",
    value: function emojiReplace(word) {
      var found = emojiMart.emojiIndex.search(word) || [];
      var _iteratorNormalCompletion = true;
      var _didIteratorError = false;
      var _iteratorError = undefined;

      try {
        for (var _iterator = found.slice(0, 10)[Symbol.iterator](), _step; !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
          var emoji = _step.value;

          if (emoji.emoticons.includes(word)) {
            return emoji.native;
          }
        }
      } catch (err) {
        _didIteratorError = true;
        _iteratorError = err;
      } finally {
        try {
          if (!_iteratorNormalCompletion && _iterator.return != null) {
            _iterator.return();
          }
        } finally {
          if (_didIteratorError) {
            throw _iteratorError;
          }
        }
      }
    }
  }, {
    key: "render",
    value: function render() {
      var innerRef = this.props.innerRef;
      return React__default.createElement(ReactTextareaAutocomplete, {
        loadingComponent: LoadingIndicator,
        trigger: this.getTriggers(),
        replaceWord: this.emojiReplace,
        minChar: 0,
        maxRows: this.props.maxRows,
        innerRef: innerRef && function (ref) {
          innerRef.current = ref;
        },
        onFocus: this.props.onFocus,
        rows: this.props.rows,
        className: "str-chat__textarea__textarea",
        containerClassName: "str-chat__textarea",
        dropdownClassName: "str-chat__emojisearch",
        listClassName: "str-chat__emojisearch__list",
        itemClassName: "str-chat__emojisearch__item",
        placeholder: this.props.placeholder,
        onChange: this.props.onChange,
        handleSubmit: this.props.handleSubmit,
        onPaste: this.props.onPaste,
        value: this.props.value,
        grow: this.props.grow,
        disabled: this.props.disabled
      });
    }
  }]);

  return ChatAutoComplete;
}(React.PureComponent);

_defineProperty(ChatAutoComplete, "propTypes", {
  /** The number of rows you want the textarea to have */
  rows: PropTypes.number,

  /** Grow the number of rows of the textarea while you're typing */
  grow: PropTypes.bool,

  /** Maximum number of rows */
  maxRows: PropTypes.number,

  /** Make the textarea disabled */
  disabled: PropTypes.bool,

  /** The value of the textarea */
  value: PropTypes.string,

  /** Function to run on pasting within the textarea */
  onPaste: PropTypes.func,

  /** Function that runs on submit */
  handleSubmit: PropTypes.func,

  /** Function that runs on change */
  onChange: PropTypes.func,

  /** Placeholder for the textare */
  placeholder: PropTypes.string,

  /** What loading component to use for the auto complete when loading results. */
  LoadingIndicator: PropTypes.node,

  /** function to set up your triggers for autocomplete(eg. '@' for mentions, '/' for commands) */
  trigger: PropTypes.func,

  /** Minimum number of Character */
  minChar: PropTypes.number,

  /** Array of [user object](https://getstream.io/chat/docs/#chat-doc-set-user). Used for mentions suggestions */
  users: PropTypes.array,

  /**
   * Handler for selecting item from suggestions list
   *
   * @param item Selected item object.
   *  */
  onSelectItem: PropTypes.func,

  /** Array of [commands](https://getstream.io/chat/docs/#channel_commands) */
  commands: PropTypes.array,

  /** Listener for onfocus event on textarea */
  onFocus: PropTypes.object
});

_defineProperty(ChatAutoComplete, "defaultProps", {
  rows: 3
});

/**
 * MessageInputLarge - Large Message Input to be used for the MessageInput.
 * @example ./docs/MessageInputLarge.md
 */

exports.MessageInputLarge =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(MessageInputLarge, _PureComponent);

  function MessageInputLarge() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, MessageInputLarge);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(MessageInputLarge)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "renderUploads", function () {
      return React__default.createElement(React__default.Fragment, null, _this.props.imageOrder.length > 0 && React__default.createElement(reactFileUtils.ImagePreviewer, {
        imageUploads: _this.props.imageOrder.map(function (id) {
          return _this.props.imageUploads[id];
        }),
        handleRemove: _this.props.removeImage,
        handleRetry: _this.props.uploadImage,
        handleFiles: _this.props.uploadNewFiles,
        multiple: _this.props.multipleUploads,
        disabled: _this.props.numberOfUploads >= _this.props.maxNumberOfFiles ? true : false
      }), _this.props.fileOrder.length > 0 && React__default.createElement(reactFileUtils.FilePreviewer, {
        uploads: _this.props.fileOrder.map(function (id) {
          return _this.props.fileUploads[id];
        }),
        handleRemove: _this.props.removeFile,
        handleRetry: _this.props.uploadFile,
        handleFiles: _this.props.uploadNewFiles
      }));
    });

    _defineProperty(_assertThisInitialized(_this), "renderEmojiPicker", function () {
      if (_this.props.emojiPickerIsOpen) {
        return React__default.createElement("div", {
          className: "str-chat__input--emojipicker",
          ref: _this.props.emojiPickerRef
        }, React__default.createElement(emojiMart.Picker, {
          native: true,
          emoji: "point_up",
          title: "Pick your emoji\u2026",
          onSelect: _this.props.onSelectEmoji,
          color: "#006CFF",
          showPreview: false
        }));
      }
    });

    return _this;
  }

  _createClass(MessageInputLarge, [{
    key: "render",
    value: function render() {
      var _this2 = this;

      var intl = this.props.intl;
      var SendButton = this.props.SendButton;
      return React__default.createElement("div", {
        className: "str-chat__input-large"
      }, React__default.createElement(reactFileUtils.ImageDropzone, {
        accept: this.props.acceptedFiles,
        multiple: this.props.multipleUploads,
        disabled: this.props.numberOfUploads >= this.props.maxNumberOfFiles ? true : false,
        handleFiles: this.props.uploadNewFiles
      }, React__default.createElement("div", {
        className: "str-chat__input"
      }, this.renderUploads(), this.renderEmojiPicker(), React__default.createElement("div", {
        className: "str-chat__input--textarea-wrapper"
      }, React__default.createElement(ChatAutoComplete, {
        users: this.props.getUsers(),
        commands: this.props.getCommands(),
        innerRef: this.props.textareaRef,
        handleSubmit: function handleSubmit(e) {
          return _this2.props.handleSubmit(e);
        },
        onChange: this.props.handleChange,
        onSelectItem: this.props.onSelectItem,
        value: this.props.text,
        rows: 1,
        maxRows: this.props.maxRows,
        placeholder: intl.formatMessage({
          id: 'message_input.placeholder',
          defaultMessage: 'Type your message'
        }),
        onPaste: this.props.onPaste,
        grow: this.props.grow,
        disabled: this.props.disabled
      }), React__default.createElement("span", {
        className: "str-chat__input-emojiselect",
        onClick: this.props.openEmojiPicker
      }, React__default.createElement("svg", {
        width: "14",
        height: "14",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        d: "M11.108 8.05a.496.496 0 0 1 .212.667C10.581 10.147 8.886 11 7 11c-1.933 0-3.673-.882-4.33-2.302a.497.497 0 0 1 .9-.417C4.068 9.357 5.446 10 7 10c1.519 0 2.869-.633 3.44-1.738a.495.495 0 0 1 .668-.212zm.792-1.826a.477.477 0 0 1-.119.692.541.541 0 0 1-.31.084.534.534 0 0 1-.428-.194c-.106-.138-.238-.306-.539-.306-.298 0-.431.168-.54.307A.534.534 0 0 1 9.538 7a.544.544 0 0 1-.31-.084.463.463 0 0 1-.117-.694c.33-.423.742-.722 1.394-.722.653 0 1.068.3 1.396.724zm-7 0a.477.477 0 0 1-.119.692.541.541 0 0 1-.31.084.534.534 0 0 1-.428-.194c-.106-.138-.238-.306-.539-.306-.299 0-.432.168-.54.307A.533.533 0 0 1 2.538 7a.544.544 0 0 1-.31-.084.463.463 0 0 1-.117-.694c.33-.423.742-.722 1.394-.722.653 0 1.068.3 1.396.724zM7 0a7 7 0 1 1 0 14A7 7 0 0 1 7 0zm4.243 11.243A5.96 5.96 0 0 0 13 7a5.96 5.96 0 0 0-1.757-4.243A5.96 5.96 0 0 0 7 1a5.96 5.96 0 0 0-4.243 1.757A5.96 5.96 0 0 0 1 7a5.96 5.96 0 0 0 1.757 4.243A5.96 5.96 0 0 0 7 13a5.96 5.96 0 0 0 4.243-1.757z",
        fillRule: "evenodd"
      }))), React__default.createElement(reactFileUtils.FileUploadButton, {
        multiple: this.props.multipleUploads,
        disabled: this.props.numberOfUploads >= this.props.maxNumberOfFiles ? true : false,
        accepts: this.props.acceptedFiles,
        handleFiles: this.props.uploadNewFiles
      }, React__default.createElement("span", {
        className: "str-chat__input-fileupload"
      }, React__default.createElement("svg", {
        width: "14",
        height: "14",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        d: "M7 .5c3.59 0 6.5 2.91 6.5 6.5s-2.91 6.5-6.5 6.5S.5 10.59.5 7 3.41.5 7 .5zm0 12c3.031 0 5.5-2.469 5.5-5.5S10.031 1.5 7 1.5A5.506 5.506 0 0 0 1.5 7c0 3.034 2.469 5.5 5.5 5.5zM7.506 3v3.494H11v1.05H7.506V11h-1.05V7.544H3v-1.05h3.456V3h1.05z",
        fillRule: "nonzero"
      })))), SendButton && React__default.createElement(SendButton, {
        sendMessage: this.props.handleSubmit
      }))), React__default.createElement("div", null, React__default.createElement("div", {
        className: "str-chat__input-footer"
      }, React__default.createElement("span", {
        className: "str-chat__input-footer--count ".concat(!this.props.watcher_count ? 'str-chat__input-footer--count--hidden' : '')
      }, React__default.createElement(reactIntl.FormattedMessage, {
        id: "message_input.watching",
        defaultMessage: "{count} online",
        values: {
          count: this.props.watcher_count
        }
      })), React__default.createElement("span", {
        className: "str-chat__input-footer--typing"
      }, formatTypingArray(intl, this.props.typing))))));
    }
  }]);

  return MessageInputLarge;
}(React.PureComponent);

_defineProperty(exports.MessageInputLarge, "propTypes", {
  /** Set focus to the text input if this is enabled */
  focus: PropTypes.bool.isRequired,

  /** Grow the textarea while you're typing */
  grow: PropTypes.bool.isRequired,

  /** Specify the max amount of rows the textarea is able to grow */
  maxRows: PropTypes.number.isRequired,

  /** Make the textarea disabled */
  disabled: PropTypes.bool,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  imageOrder: PropTypes.array,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  imageUploads: PropTypes.object,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  removeImage: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  uploadImage: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  uploadNewFiles: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  numberOfUploads: PropTypes.number,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  fileOrder: PropTypes.array,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  fileUploads: PropTypes.object,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  removeFile: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  uploadFile: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  emojiPickerIsOpen: PropTypes.bool,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  emojiPickerRef: PropTypes.object,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  onSelectEmoji: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  getUsers: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  getCommands: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  textareaRef: PropTypes.object,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  handleSubmit: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  handleChange: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  onSelectItem: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  text: PropTypes.string,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  onPaste: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  openEmojiPicker: PropTypes.func,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  watcher_count: PropTypes.number,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  typing: PropTypes.object,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  multipleUploads: PropTypes.object,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  maxNumberOfFiles: PropTypes.object,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  acceptedFiles: PropTypes.object
});

exports.MessageInputLarge = reactIntl.injectIntl(exports.MessageInputLarge);

var SendButton = function SendButton(_ref) {
  var sendMessage = _ref.sendMessage;
  return React__default.createElement("button", {
    className: "str-chat__send-button",
    onClick: sendMessage
  });
};

if (!Element.prototype.matches) {
  Element.prototype.matches = Element.prototype.msMatchesSelector || Element.prototype.webkitMatchesSelector;
}
/**
 * MessageInput - Input a new message, support for all the rich features such as image uploads, @mentions, emoticons etc.
 * @example ./docs/MessageInput.md
 * @extends PureComponent
 */


exports.MessageInput =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(MessageInput, _PureComponent);

  function MessageInput(props) {
    var _this;

    _classCallCheck(this, MessageInput);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(MessageInput).call(this, props));

    _defineProperty(_assertThisInitialized(_this), "openEmojiPicker", function () {
      if (!_this.state.showEmojiPicker) {
        _this.setState(function () {
          return {
            emojiPickerIsOpen: true
          };
        }, function () {
          document.addEventListener('click', _this.closeEmojiPicker, false);
        });
      }
    });

    _defineProperty(_assertThisInitialized(_this), "closeEmojiPicker", function (e) {
      if (_this.emojiPickerRef.current && !_this.emojiPickerRef.current.contains(e.target)) {
        _this.setState({
          emojiPickerIsOpen: false
        }, function () {
          document.removeEventListener('click', _this.closeEmojiPicker, false);
        });
      }
    });

    _defineProperty(_assertThisInitialized(_this), "onSelectEmoji", function (emoji) {
      return _this.insertText(emoji.native);
    });

    _defineProperty(_assertThisInitialized(_this), "insertText",
    /*#__PURE__*/
    function () {
      var _ref = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee(textToInsert) {
        var newCursorPosition, textareaElement;
        return _regeneratorRuntime.wrap(function _callee$(_context) {
          while (1) {
            switch (_context.prev = _context.next) {
              case 0:
                _context.next = 2;
                return _this.setState(function (prevState) {
                  var prevText = prevState.text;
                  var textareaElement = _this.textareaRef.current;

                  if (!textareaElement) {
                    return {
                      text: prevText + textToInsert
                    };
                  } // Insert emoji at previous cursor position


                  var selectionStart = textareaElement.selectionStart,
                      selectionEnd = textareaElement.selectionEnd;
                  newCursorPosition = selectionStart + textToInsert.length;
                  return {
                    text: prevText.slice(0, selectionStart) + textToInsert + prevText.slice(selectionEnd)
                  };
                });

              case 2:
                textareaElement = _this.textareaRef.current;

                if (!(!textareaElement || newCursorPosition == null)) {
                  _context.next = 5;
                  break;
                }

                return _context.abrupt("return");

              case 5:
                // Update cursorPosition
                textareaElement.selectionStart = newCursorPosition;
                textareaElement.selectionEnd = newCursorPosition;

              case 7:
              case "end":
                return _context.stop();
            }
          }
        }, _callee);
      }));

      return function (_x) {
        return _ref.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "getCommands", function () {
      return _this.props.channel.getConfig().commands;
    });

    _defineProperty(_assertThisInitialized(_this), "getUsers", function () {
      var users = [];
      var members = _this.props.channel.state.members;
      var watchers = _this.props.channel.state.watchers;

      if (members && Object.values(members).length) {
        Object.values(members).forEach(function (member) {
          return users.push(member.user);
        });
      }

      if (watchers && Object.values(watchers).length) {
        users.push.apply(users, _toConsumableArray(Object.values(watchers)));
      } // make sure we don't list users twice


      var userMap = {};

      for (var _i = 0, _users = users; _i < _users.length; _i++) {
        var user = _users[_i];

        if (user !== undefined && !userMap[user.id]) {
          userMap[user.id] = user;
        }
      }

      return Object.values(userMap);
    });

    _defineProperty(_assertThisInitialized(_this), "handleChange", function (event) {
      event.preventDefault();

      if (!event || !event.target) {
        return '';
      }

      var text = event.target.value;

      _this.setState({
        text: text
      });

      if (text) {
        streamChat.logChatPromiseExecution(_this.props.channel.keystroke(), 'start typing event');
      }
    });

    _defineProperty(_assertThisInitialized(_this), "handleSubmit", function (event) {
      event.preventDefault();
      var editing = !!_this.props.message;

      var trimmedMessage = _this.state.text.trim();

      var isEmptyMessage = trimmedMessage === '' || trimmedMessage === '>' || trimmedMessage === '``````' || trimmedMessage === '``' || trimmedMessage === '**' || trimmedMessage === '____' || trimmedMessage === '__' || trimmedMessage === '****';
      var hasFiles = _this.state.imageOrder.length > 0 || _this.state.fileOrder.length > 0;

      if (isEmptyMessage && !hasFiles) {
        return;
      }

      var text = _this.state.text; // the channel component handles the actual sending of the message

      var attachments = _toConsumableArray(_this.state.attachments);

      var _iteratorNormalCompletion = true;
      var _didIteratorError = false;
      var _iteratorError = undefined;

      try {
        for (var _iterator = _this.state.imageOrder[Symbol.iterator](), _step; !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
          var _id2 = _step.value;
          var image = _this.state.imageUploads[_id2];

          if (!image || image.state === 'failed') {
            continue;
          }

          if (image.state === 'uploading') {
            // TODO: show error to user that they should wait until image is uploaded
            return;
          }

          attachments.push({
            type: 'image',
            image_url: image.url,
            fallback: image.file.name
          });
        }
      } catch (err) {
        _didIteratorError = true;
        _iteratorError = err;
      } finally {
        try {
          if (!_iteratorNormalCompletion && _iterator.return != null) {
            _iterator.return();
          }
        } finally {
          if (_didIteratorError) {
            throw _iteratorError;
          }
        }
      }

      var _iteratorNormalCompletion2 = true;
      var _didIteratorError2 = false;
      var _iteratorError2 = undefined;

      try {
        for (var _iterator2 = _this.state.fileOrder[Symbol.iterator](), _step2; !(_iteratorNormalCompletion2 = (_step2 = _iterator2.next()).done); _iteratorNormalCompletion2 = true) {
          var _id3 = _step2.value;
          var upload = _this.state.fileUploads[_id3];

          if (!upload || upload.state === 'failed') {
            continue;
          }

          if (upload.state === 'uploading') {
            // TODO: show error to user that they should wait until image is uploaded
            return;
          }

          attachments.push({
            type: 'file',
            asset_url: upload.url,
            title: upload.file.name,
            mime_type: upload.file.type,
            file_size: upload.file.size
          });
        }
      } catch (err) {
        _didIteratorError2 = true;
        _iteratorError2 = err;
      } finally {
        try {
          if (!_iteratorNormalCompletion2 && _iterator2.return != null) {
            _iterator2.return();
          }
        } finally {
          if (_didIteratorError2) {
            throw _iteratorError2;
          }
        }
      }

      if (editing) {
        var id = _this.props.message.id;
        var updatedMessage = {
          id: id
        };
        updatedMessage.text = text;
        updatedMessage.attachments = attachments;
        updatedMessage.mentioned_users = _this.state.mentioned_users; // TODO: Remove this line and show an error when submit fails

        _this.props.clearEditingState();

        var updateMessagePromise = _this.props.editMessage(updatedMessage).then(_this.props.clearEditingState);

        streamChat.logChatPromiseExecution(updateMessagePromise, 'update message');
      } else {
        var sendMessagePromise = _this.props.sendMessage({
          text: text,
          attachments: attachments,
          mentioned_users: uniq(_this.state.mentioned_users),
          parent: _this.props.parent
        });

        streamChat.logChatPromiseExecution(sendMessagePromise, 'send message');

        _this.setState({
          text: '',
          mentioned_users: [],
          imageUploads: Immutable({}),
          imageOrder: [],
          fileUploads: Immutable({}),
          fileOrder: []
        });
      }

      streamChat.logChatPromiseExecution(_this.props.channel.stopTyping(), 'stop typing');
    });

    _defineProperty(_assertThisInitialized(_this), "_uploadNewFiles", function (files) {
      var _iteratorNormalCompletion3 = true;
      var _didIteratorError3 = false;
      var _iteratorError3 = undefined;

      try {
        for (var _iterator3 = files[Symbol.iterator](), _step3; !(_iteratorNormalCompletion3 = (_step3 = _iterator3.next()).done); _iteratorNormalCompletion3 = true) {
          var file = _step3.value;

          if (file.type.startsWith('image/')) {
            _this._uploadNewImage(file);
          } else if (file instanceof File && !_this.props.noFiles) {
            _this._uploadNewFile(file);
          } else {
            return;
          }
        }
      } catch (err) {
        _didIteratorError3 = true;
        _iteratorError3 = err;
      } finally {
        try {
          if (!_iteratorNormalCompletion3 && _iterator3.return != null) {
            _iterator3.return();
          }
        } finally {
          if (_didIteratorError3) {
            throw _iteratorError3;
          }
        }
      }
    });

    _defineProperty(_assertThisInitialized(_this), "_uploadNewImage",
    /*#__PURE__*/
    function () {
      var _ref2 = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee2(file) {
        var id, reader;
        return _regeneratorRuntime.wrap(function _callee2$(_context2) {
          while (1) {
            switch (_context2.prev = _context2.next) {
              case 0:
                id = generateRandomId();
                _context2.next = 3;
                return _this.setState(function (prevState) {
                  return {
                    numberOfUploads: prevState.numberOfUploads + 1,
                    imageOrder: prevState.imageOrder.concat(id),
                    imageUploads: prevState.imageUploads.setIn([id], {
                      id: id,
                      file: file,
                      state: 'uploading'
                    })
                  };
                });

              case 3:
                if (FileReader) {
                  // TODO: Possibly use URL.createObjectURL instead. However, then we need
                  // to release the previews when not used anymore though.
                  reader = new FileReader();

                  reader.onload = function (event) {
                    _this.setState(function (prevState) {
                      return {
                        imageUploads: prevState.imageUploads.setIn([id, 'previewUri'], event.target.result)
                      };
                    });
                  };

                  reader.readAsDataURL(file);
                }

                return _context2.abrupt("return", _this._uploadImage(id));

              case 5:
              case "end":
                return _context2.stop();
            }
          }
        }, _callee2);
      }));

      return function (_x2) {
        return _ref2.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "_uploadNewFile",
    /*#__PURE__*/
    function () {
      var _ref3 = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee3(file) {
        var id;
        return _regeneratorRuntime.wrap(function _callee3$(_context3) {
          while (1) {
            switch (_context3.prev = _context3.next) {
              case 0:
                id = generateRandomId();
                _context3.next = 3;
                return _this.setState(function (prevState) {
                  return {
                    numberOfUploads: prevState.numberOfUploads + 1,
                    fileOrder: prevState.fileOrder.concat(id),
                    fileUploads: prevState.fileUploads.setIn([id], {
                      id: id,
                      file: file,
                      state: 'uploading'
                    })
                  };
                });

              case 3:
                return _context3.abrupt("return", _this._uploadFile(id));

              case 4:
              case "end":
                return _context3.stop();
            }
          }
        }, _callee3);
      }));

      return function (_x3) {
        return _ref3.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "_uploadImage",
    /*#__PURE__*/
    function () {
      var _ref4 = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee4(id) {
        var img, file, response, alreadyRemoved;
        return _regeneratorRuntime.wrap(function _callee4$(_context4) {
          while (1) {
            switch (_context4.prev = _context4.next) {
              case 0:
                img = _this.state.imageUploads[id];

                if (img) {
                  _context4.next = 3;
                  break;
                }

                return _context4.abrupt("return");

              case 3:
                file = img.file;
                _context4.next = 6;
                return _this.setState(function (prevState) {
                  return {
                    imageUploads: prevState.imageUploads.setIn([id, 'state'], 'uploading')
                  };
                });

              case 6:
                response = {};
                response = {};
                _context4.prev = 8;

                if (!_this.props.doImageUploadRequest) {
                  _context4.next = 15;
                  break;
                }

                _context4.next = 12;
                return _this.props.doImageUploadRequest(file, _this.props.channel);

              case 12:
                response = _context4.sent;
                _context4.next = 18;
                break;

              case 15:
                _context4.next = 17;
                return _this.props.channel.sendImage(file);

              case 17:
                response = _context4.sent;

              case 18:
                _context4.next = 28;
                break;

              case 20:
                _context4.prev = 20;
                _context4.t0 = _context4["catch"](8);
                console.warn(_context4.t0);
                alreadyRemoved = false;
                _context4.next = 26;
                return _this.setState(function (prevState) {
                  var image = prevState.imageUploads[id];

                  if (!image) {
                    alreadyRemoved = true;
                    return {
                      numberOfUploads: prevState.numberOfUploads - 1
                    };
                  }

                  return {
                    imageUploads: prevState.imageUploads.setIn([id, 'state'], 'failed'),
                    numberOfUploads: prevState.numberOfUploads - 1
                  };
                });

              case 26:
                if (!alreadyRemoved) {
                  _this.props.errorHandler(_context4.t0, 'upload-image', {
                    feedGroup: _this.props.feedGroup,
                    userId: _this.props.userId
                  });
                }

                return _context4.abrupt("return");

              case 28:
                _this.setState(function (prevState) {
                  return {
                    imageUploads: prevState.imageUploads.setIn([id, 'state'], 'finished').setIn([id, 'url'], response.file)
                  };
                });

              case 29:
              case "end":
                return _context4.stop();
            }
          }
        }, _callee4, null, [[8, 20]]);
      }));

      return function (_x4) {
        return _ref4.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "_uploadFile",
    /*#__PURE__*/
    function () {
      var _ref5 = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee5(id) {
        var upload, file, response, alreadyRemoved;
        return _regeneratorRuntime.wrap(function _callee5$(_context5) {
          while (1) {
            switch (_context5.prev = _context5.next) {
              case 0:
                upload = _this.state.fileUploads[id];

                if (upload) {
                  _context5.next = 3;
                  break;
                }

                return _context5.abrupt("return");

              case 3:
                file = upload.file;
                _context5.next = 6;
                return _this.setState(function (prevState) {
                  return {
                    imageUploads: prevState.imageUploads.setIn([id, 'state'], 'uploading')
                  };
                });

              case 6:
                response = {};
                response = {};
                _context5.prev = 8;

                if (!_this.props.doFileUploadRequest) {
                  _context5.next = 15;
                  break;
                }

                _context5.next = 12;
                return _this.props.doFileUploadRequest(file, _this.props.channel);

              case 12:
                response = _context5.sent;
                _context5.next = 18;
                break;

              case 15:
                _context5.next = 17;
                return _this.props.channel.sendFile(file);

              case 17:
                response = _context5.sent;

              case 18:
                _context5.next = 27;
                break;

              case 20:
                _context5.prev = 20;
                _context5.t0 = _context5["catch"](8);
                console.warn(_context5.t0);
                alreadyRemoved = false;
                _context5.next = 26;
                return _this.setState(function (prevState) {
                  var image = prevState.imageUploads[id];

                  if (!image) {
                    alreadyRemoved = true;
                    return {
                      numberOfUploads: prevState.numberOfUploads - 1
                    };
                  }

                  return {
                    numberOfUploads: prevState.numberOfUploads - 1,
                    fileUploads: prevState.fileUploads.setIn([id, 'state'], 'failed')
                  };
                });

              case 26:
                if (!alreadyRemoved) {
                  _this.props.errorHandler(_context5.t0, 'upload-file', {
                    feedGroup: _this.props.feedGroup,
                    userId: _this.props.userId
                  });
                }

              case 27:
                _this.setState(function (prevState) {
                  return {
                    fileUploads: prevState.fileUploads.setIn([id, 'state'], 'finished').setIn([id, 'url'], response.file)
                  };
                });

              case 28:
              case "end":
                return _context5.stop();
            }
          }
        }, _callee5, null, [[8, 20]]);
      }));

      return function (_x5) {
        return _ref5.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "_removeImage", function (id) {
      // TODO: cancel upload if still uploading
      _this.setState(function (prevState) {
        var img = prevState.imageUploads[id];

        if (!img) {
          return {};
        }

        return {
          numberOfUploads: prevState.numberOfUploads - 1,
          imageUploads: prevState.imageUploads.set(id, undefined),
          // remove
          imageOrder: prevState.imageOrder.filter(function (_id) {
            return id !== _id;
          })
        };
      });
    });

    _defineProperty(_assertThisInitialized(_this), "_removeFile", function (id) {
      // TODO: cancel upload if still uploading
      _this.setState(function (prevState) {
        var upload = prevState.fileUploads[id];

        if (!upload) {
          return {};
        }

        return {
          numberOfUploads: prevState.numberOfUploads - 1,
          fileUploads: prevState.fileUploads.set(id, undefined),
          // remove
          fileOrder: prevState.fileOrder.filter(function (_id) {
            return id !== _id;
          })
        };
      });
    });

    _defineProperty(_assertThisInitialized(_this), "_onPaste",
    /*#__PURE__*/
    function () {
      var _ref6 = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee6(event) {
        var items, plainTextPromise, _iteratorNormalCompletion4, _didIteratorError4, _iteratorError4, _loop, _iterator4, _step4, _ret, fileLikes, s;

        return _regeneratorRuntime.wrap(function _callee6$(_context6) {
          while (1) {
            switch (_context6.prev = _context6.next) {
              case 0:
                // TODO: Move this handler to package with ImageDropzone
                items = event.clipboardData.items;

                if (reactFileUtils.dataTransferItemsHaveFiles(items)) {
                  _context6.next = 3;
                  break;
                }

                return _context6.abrupt("return");

              case 3:
                event.preventDefault(); // Get a promise for the plain text in case no files are
                // found. This needs to be done here because chrome cleans
                // up the DataTransferItems after resolving of a promise.

                _iteratorNormalCompletion4 = true;
                _didIteratorError4 = false;
                _iteratorError4 = undefined;
                _context6.prev = 7;

                _loop = function _loop() {
                  var item = _step4.value;

                  if (item.kind === 'string' && item.type === 'text/plain') {
                    plainTextPromise = new Promise(function (resolve) {
                      item.getAsString(function (s) {
                        resolve(s);
                      });
                    });
                    return "break";
                  }
                };

                _iterator4 = items[Symbol.iterator]();

              case 10:
                if (_iteratorNormalCompletion4 = (_step4 = _iterator4.next()).done) {
                  _context6.next = 17;
                  break;
                }

                _ret = _loop();

                if (!(_ret === "break")) {
                  _context6.next = 14;
                  break;
                }

                return _context6.abrupt("break", 17);

              case 14:
                _iteratorNormalCompletion4 = true;
                _context6.next = 10;
                break;

              case 17:
                _context6.next = 23;
                break;

              case 19:
                _context6.prev = 19;
                _context6.t0 = _context6["catch"](7);
                _didIteratorError4 = true;
                _iteratorError4 = _context6.t0;

              case 23:
                _context6.prev = 23;
                _context6.prev = 24;

                if (!_iteratorNormalCompletion4 && _iterator4.return != null) {
                  _iterator4.return();
                }

              case 26:
                _context6.prev = 26;

                if (!_didIteratorError4) {
                  _context6.next = 29;
                  break;
                }

                throw _iteratorError4;

              case 29:
                return _context6.finish(26);

              case 30:
                return _context6.finish(23);

              case 31:
                _context6.next = 33;
                return reactFileUtils.dataTransferItemsToFiles(items);

              case 33:
                fileLikes = _context6.sent;

                if (!fileLikes.length) {
                  _context6.next = 37;
                  break;
                }

                _this._uploadNewFiles(fileLikes);

                return _context6.abrupt("return");

              case 37:
                if (!plainTextPromise) {
                  _context6.next = 42;
                  break;
                }

                _context6.next = 40;
                return plainTextPromise;

              case 40:
                s = _context6.sent;

                _this.insertText(s);

              case 42:
              case "end":
                return _context6.stop();
            }
          }
        }, _callee6, null, [[7, 19, 23, 31], [24,, 26, 30]]);
      }));

      return function (_x6) {
        return _ref6.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "_onSelectItem", function (item) {
      _this.setState(function (prevState) {
        return {
          mentioned_users: [].concat(_toConsumableArray(prevState.mentioned_users), [item.id])
        };
      });
    });

    var imageOrder = [];
    var imageUploads = {};
    var fileOrder = [];
    var fileUploads = {};
    var _attachments = [];
    var mentioned_users = [];
    var _text = '';

    if (props.message) {
      _text = props.message.text;
      var _iteratorNormalCompletion5 = true;
      var _didIteratorError5 = false;
      var _iteratorError5 = undefined;

      try {
        for (var _iterator5 = props.message.attachments[Symbol.iterator](), _step5; !(_iteratorNormalCompletion5 = (_step5 = _iterator5.next()).done); _iteratorNormalCompletion5 = true) {
          var attach = _step5.value;

          if (attach.type === 'image') {
            var id = generateRandomId();
            imageOrder.push(id);
            imageUploads[id] = {
              id: id,
              url: attach.image_url,
              state: 'finished',
              file: {
                name: attach.fallback
              }
            };
          } else if (attach.type === 'file') {
            var _id4 = generateRandomId();

            fileOrder.push(_id4);
            fileUploads[_id4] = {
              id: _id4,
              url: attach.asset_url,
              state: 'finished',
              file: {
                name: attach.title,
                type: attach.mime_type,
                size: attach.file_size
              }
            };
          } else {
            _attachments.push(attach);
          }
        }
      } catch (err) {
        _didIteratorError5 = true;
        _iteratorError5 = err;
      } finally {
        try {
          if (!_iteratorNormalCompletion5 && _iterator5.return != null) {
            _iterator5.return();
          }
        } finally {
          if (_didIteratorError5) {
            throw _iteratorError5;
          }
        }
      }

      var _iteratorNormalCompletion6 = true;
      var _didIteratorError6 = false;
      var _iteratorError6 = undefined;

      try {
        for (var _iterator6 = props.message.mentioned_users[Symbol.iterator](), _step6; !(_iteratorNormalCompletion6 = (_step6 = _iterator6.next()).done); _iteratorNormalCompletion6 = true) {
          var mention = _step6.value;
          mentioned_users.push(mention.id);
        }
      } catch (err) {
        _didIteratorError6 = true;
        _iteratorError6 = err;
      } finally {
        try {
          if (!_iteratorNormalCompletion6 && _iterator6.return != null) {
            _iterator6.return();
          }
        } finally {
          if (_didIteratorError6) {
            throw _iteratorError6;
          }
        }
      }
    }

    _this.state = {
      text: _text,
      attachments: _attachments,
      imageOrder: imageOrder,
      imageUploads: Immutable(imageUploads),
      fileOrder: fileOrder,
      fileUploads: Immutable(fileUploads),
      emojiPickerIsOpen: false,
      filePanelIsOpen: false,
      mentioned_users: mentioned_users,
      numberOfUploads: 0
    };
    _this.textareaRef = React__default.createRef();
    _this.emojiPickerRef = React__default.createRef();
    _this.panelRef = React__default.createRef();
    return _this;
  }

  _createClass(MessageInput, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      if (this.props.focus) {
        this.textareaRef.current.focus();
      }
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      document.removeEventListener('click', this.closeEmojiPicker, false);
      document.removeEventListener('click', this.hideFilePanel, false);
    }
  }, {
    key: "render",
    value: function render() {
      var Input = this.props.Input;
      var handlers = {
        uploadNewFiles: this._uploadNewFiles,
        removeImage: this._removeImage,
        uploadImage: this._uploadImage,
        removeFile: this._removeFile,
        uploadFile: this._uploadFile,
        emojiPickerRef: this.emojiPickerRef,
        panelRef: this.panelRef,
        textareaRef: this.textareaRef,
        onSelectEmoji: this.onSelectEmoji,
        getUsers: this.getUsers,
        getCommands: this.getCommands,
        handleSubmit: this.handleSubmit,
        handleChange: this.handleChange,
        onPaste: this._onPaste,
        onSelectItem: this._onSelectItem,
        openEmojiPicker: this.openEmojiPicker
      };
      return React__default.createElement(Input, _extends({}, this.props, this.state, handlers));
    }
  }]);

  return MessageInput;
}(React.PureComponent);

_defineProperty(exports.MessageInput, "propTypes", {
  /** Set focus to the text input if this is enabled */
  focus: PropTypes.bool.isRequired,

  /** Disable input */
  disabled: PropTypes.bool.isRequired,

  /** Grow the textarea while you're typing */
  grow: PropTypes.bool.isRequired,

  /** Set the maximum number of rows */
  maxRows: PropTypes.number.isRequired,

  /** Via Context: the channel that we're sending the message to */
  channel: PropTypes.object.isRequired,

  /** Via Context: the users currently typing, passed from the Channel component */
  typing: PropTypes.object.isRequired,
  // /** Set textarea to be disabled */
  // disabled: PropTypes.bool,

  /** The parent message object when replying on a thread */
  parent: PropTypes.object,

  /**
   * The component handling how the input is rendered
   *
   * Available built-in components (also accepts the same props as):
   *
   * 1. [MessageInputSmall](https://github.com/GetStream/stream-chat-react/blob/master/src/components/MessageInputSmall.js)
   * 2. [MessageInputLarge](https://github.com/GetStream/stream-chat-react/blob/master/src/components/MessageInputLarge.js) (default)
   * 3. [MessageInputFlat](https://github.com/GetStream/stream-chat-react/blob/master/src/components/MessageInputFlat.js)
   *
   * */
  Input: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /** Override image upload request */
  doImageUploadRequest: PropTypes.func,

  /** Override file upload request */
  doFileUploadRequest: PropTypes.func,

  /**
   * Custom UI component for send button.
   *
   * Defaults to and accepts same props as: [SendButton](https://getstream.github.io/stream-chat-react/#sendbutton)
   * */
  SendButton: PropTypes.oneOfType([PropTypes.node, PropTypes.func])
});

_defineProperty(exports.MessageInput, "defaultProps", {
  focus: false,
  disabled: false,
  grow: true,
  maxRows: 10,
  Input: exports.MessageInputLarge,
  SendButton: SendButton
});

exports.MessageInput = withChannelContext(exports.MessageInput);

/**
 * @example ./docs/EditMessageForm.md
 */

var EditMessageForm =
/*#__PURE__*/
function (_React$Component) {
  _inherits(EditMessageForm, _React$Component);

  function EditMessageForm() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, EditMessageForm);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(EditMessageForm)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "renderUploads", function () {
      return React__default.createElement(React__default.Fragment, null, _this.props.imageOrder.length > 0 && React__default.createElement(reactFileUtils.ImagePreviewer, {
        imageUploads: _this.props.imageOrder.map(function (id) {
          return _this.props.imageUploads[id];
        }),
        handleRemove: _this.props.removeImage,
        handleRetry: _this.props.uploadImage,
        handleFiles: _this.props.uploadNewFiles,
        multiple: _this.props.multipleUploads,
        disabled: _this.props.numberOfUploads >= _this.props.maxNumberOfFiles ? true : false
      }), _this.props.fileOrder.length > 0 && React__default.createElement(reactFileUtils.FilePreviewer, {
        uploads: _this.props.fileOrder.map(function (id) {
          return _this.props.fileUploads[id];
        }),
        handleRemove: _this.props.removeFile,
        handleRetry: _this.props.uploadFile,
        handleFiles: _this.props.uploadNewFiles
      }));
    });

    _defineProperty(_assertThisInitialized(_this), "renderEmojiPicker", function () {
      if (_this.props.emojiPickerIsOpen) {
        return React__default.createElement("div", {
          className: "str-chat__small-message-input-emojipicker",
          ref: _this.props.emojiPickerRef
        }, React__default.createElement(emojiMart.Picker, {
          native: true,
          emoji: "point_up",
          title: "Pick your emoji\u2026",
          onSelect: _this.props.onSelectEmoji,
          color: "#006CFF",
          showPreview: false
        }));
      }
    });

    return _this;
  }

  _createClass(EditMessageForm, [{
    key: "render",
    value: function render() {
      return React__default.createElement("div", {
        className: "str-chat__edit-message-form"
      }, React__default.createElement(reactFileUtils.ImageDropzone, {
        accept: this.props.acceptedFiles,
        multiple: this.props.multipleUploads,
        disabled: this.props.numberOfUploads >= this.props.maxNumberOfFiles ? true : false,
        handleFiles: this.props.uploadNewFiles
      }, React__default.createElement("form", {
        onSubmit: this.props.handleSubmit
      }, this.renderEmojiPicker(), this.renderUploads(), this.props.emojiPickerIsOpen && React__default.createElement("div", {
        className: "str-chat__input--emojipicker",
        ref: this.props.emojiPickerRef
      }, React__default.createElement(emojiMart.Picker, {
        native: true,
        emoji: "point_up",
        title: "Pick your emoji\u2026",
        onSelect: this.props.onSelectEmoji,
        color: "#006CFF"
      })), React__default.createElement(ChatAutoComplete, {
        users: this.props.getUsers(),
        commands: this.props.getCommands(),
        innerRef: this.props.textareaRef,
        handleSubmit: this.props.handleSubmit,
        onChange: this.props.handleChange,
        onSelectItem: this.props.onSelectItem,
        value: this.props.text,
        rows: 1,
        maxRows: this.props.maxRows,
        onPaste: this.props.onPaste,
        grow: this.props.grow
      }), React__default.createElement("div", {
        className: "str-chat__message-team-form-footer"
      }, React__default.createElement("div", {
        className: "str-chat__edit-message-form-options"
      }, React__default.createElement("span", {
        className: "str-chat__input-emojiselect",
        onClick: this.props.openEmojiPicker
      }, React__default.createElement("svg", {
        width: "14",
        height: "14",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        d: "M11.108 8.05a.496.496 0 0 1 .212.667C10.581 10.147 8.886 11 7 11c-1.933 0-3.673-.882-4.33-2.302a.497.497 0 0 1 .9-.417C4.068 9.357 5.446 10 7 10c1.519 0 2.869-.633 3.44-1.738a.495.495 0 0 1 .668-.212zm.792-1.826a.477.477 0 0 1-.119.692.541.541 0 0 1-.31.084.534.534 0 0 1-.428-.194c-.106-.138-.238-.306-.539-.306-.298 0-.431.168-.54.307A.534.534 0 0 1 9.538 7a.544.544 0 0 1-.31-.084.463.463 0 0 1-.117-.694c.33-.423.742-.722 1.394-.722.653 0 1.068.3 1.396.724zm-7 0a.477.477 0 0 1-.119.692.541.541 0 0 1-.31.084.534.534 0 0 1-.428-.194c-.106-.138-.238-.306-.539-.306-.299 0-.432.168-.54.307A.533.533 0 0 1 2.538 7a.544.544 0 0 1-.31-.084.463.463 0 0 1-.117-.694c.33-.423.742-.722 1.394-.722.653 0 1.068.3 1.396.724zM7 0a7 7 0 1 1 0 14A7 7 0 0 1 7 0zm4.243 11.243A5.96 5.96 0 0 0 13 7a5.96 5.96 0 0 0-1.757-4.243A5.96 5.96 0 0 0 7 1a5.96 5.96 0 0 0-4.243 1.757A5.96 5.96 0 0 0 1 7a5.96 5.96 0 0 0 1.757 4.243A5.96 5.96 0 0 0 7 13a5.96 5.96 0 0 0 4.243-1.757z",
        fillRule: "evenodd"
      }))), React__default.createElement(reactFileUtils.FileUploadButton, {
        multiple: this.props.multipleUploads,
        disabled: this.props.numberOfUploads >= this.props.maxNumberOfFiles ? true : false,
        accepts: this.props.acceptedFiles,
        handleFiles: this.props.uploadNewFiles
      }, React__default.createElement("span", {
        className: "str-chat__input-fileupload"
      }, React__default.createElement("svg", {
        width: "14",
        height: "14",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        d: "M7 .5c3.59 0 6.5 2.91 6.5 6.5s-2.91 6.5-6.5 6.5S.5 10.59.5 7 3.41.5 7 .5zm0 12c3.031 0 5.5-2.469 5.5-5.5S10.031 1.5 7 1.5A5.506 5.506 0 0 0 1.5 7c0 3.034 2.469 5.5 5.5 5.5zM7.506 3v3.494H11v1.05H7.506V11h-1.05V7.544H3v-1.05h3.456V3h1.05z",
        fillRule: "nonzero"
      }))))), React__default.createElement("div", null, React__default.createElement("button", {
        onClick: this.props.clearEditingState
      }, React__default.createElement(reactIntl.FormattedMessage, {
        id: "edit_message.cancel",
        defaultMessage: "Cancel"
      })), React__default.createElement("button", {
        type: "submit"
      }, React__default.createElement(reactIntl.FormattedMessage, {
        id: "edit_message.submit",
        defaultMessage: "Send"
      })))))));
    }
  }]);

  return EditMessageForm;
}(React__default.Component);

_defineProperty(EditMessageForm, "propTypes", {
  /** Set focus to the text input if this is enabled */
  focus: PropTypes.bool,

  /** Grow the textarea while you're typing */
  grow: PropTypes.bool,

  /** Specify the max amount of rows the textarea is able to grow */
  maxRows: PropTypes.number.isRequired,

  /** Disable the textarea */
  disabled: PropTypes.bool,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  imageOrder: PropTypes.array,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  imageUploads: PropTypes.object,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  removeImage: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  uploadImage: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  uploadNewFiles: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  numberOfUploads: PropTypes.number,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  fileOrder: PropTypes.array,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  fileUploads: PropTypes.object,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  removeFile: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  uploadFile: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  emojiPickerIsOpen: PropTypes.bool,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  emojiPickerRef: PropTypes.object,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  onSelectEmoji: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  getUsers: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  getCommands: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  textareaRef: PropTypes.object,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  handleSubmit: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  handleChange: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  onSelectItem: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  text: PropTypes.string,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  onPaste: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  openEmojiPicker: PropTypes.func,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  watcher_count: PropTypes.number,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  typing: PropTypes.object,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  multipleUploads: PropTypes.object,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  maxNumberOfFiles: PropTypes.object,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  acceptedFiles: PropTypes.object
});

_defineProperty(EditMessageForm, "defaultProps", {
  grow: true,
  focus: false,
  disabled: false
});

function ownKeys$4(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread$4(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys$4(source, true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys$4(source).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }
/**
 * MessageSimple - Render component, should be used together with the Message component
 *
 * @example ./docs/MessageSimple.md
 * @extends PureComponent
 */

exports.MessageSimple =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(MessageSimple, _PureComponent);

  function MessageSimple() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, MessageSimple);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(MessageSimple)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "_isMounted", false);

    _defineProperty(_assertThisInitialized(_this), "state", {
      isFocused: false,
      language: window.dplChatConfig.language,
      actionsBoxOpen: false,
      showDetailedReactions: false
    });

    _defineProperty(_assertThisInitialized(_this), "onLanguageChanged", function () {
      if (_this._isMounted) {
        _this.setState({
          state: _objectSpread$4({}, _this.state, {}, {
            language: window.dplChatConfig.language
          })
        });
      }
    });

    _defineProperty(_assertThisInitialized(_this), "messageActionsRef", React__default.createRef());

    _defineProperty(_assertThisInitialized(_this), "reactionSelectorRef", React__default.createRef());

    _defineProperty(_assertThisInitialized(_this), "_onClickOptionsAction", function () {
      _this.setState({
        actionsBoxOpen: true
      }, function () {
        return document.addEventListener('click', _this.hideOptions, false);
      });
    });

    _defineProperty(_assertThisInitialized(_this), "_hideOptions", function () {
      _this.setState({
        actionsBoxOpen: false
      });

      document.removeEventListener('click', _this.hideOptions, false);
    });

    _defineProperty(_assertThisInitialized(_this), "_clickReactionList", function () {
      _this.setState(function () {
        return {
          showDetailedReactions: true
        };
      }, function () {
        document.addEventListener('click', _this._closeDetailedReactions);
        document.addEventListener('touchend', _this._closeDetailedReactions);
      });
    });

    _defineProperty(_assertThisInitialized(_this), "_closeDetailedReactions", function (e) {
      if (!_this.reactionSelectorRef.current.reactionSelector.current.contains(e.target)) {
        _this.setState(function () {
          return {
            showDetailedReactions: false
          };
        }, function () {
          document.removeEventListener('click', _this._closeDetailedReactions);
          document.removeEventListener('touchend', _this._closeDetailedReactions);
        });
      } else {
        return {};
      }
    });

    _defineProperty(_assertThisInitialized(_this), "renderStatus", function () {
      var _this$props = _this.props,
          readBy = _this$props.readBy,
          client = _this$props.client,
          message = _this$props.message,
          threadList = _this$props.threadList,
          lastReceivedId = _this$props.lastReceivedId,
          intl = _this$props.intl;

      if (!_this.isMine() || message.type === 'error') {
        return null;
      }

      var justReadByMe = readBy.length === 1 && readBy[0].id === client.user.id;

      if (message.status === 'sending') {
        return React__default.createElement("span", {
          className: "str-chat__message-simple-status"
        }, React__default.createElement(Tooltip, null, React__default.createElement(reactIntl.FormattedMessage, {
          id: "message.status.sending",
          defaultMessage: "Sending..."
        })), React__default.createElement(LoadingIndicator, null));
      } else if (readBy.length !== 0 && !threadList && !justReadByMe) {
        var readByOthers = readBy.filter(function (item) {
          return item.id !== client.user.id;
        });
        var lastReadUser = readByOthers[0];
        return React__default.createElement("span", {
          className: "str-chat__message-simple-status"
        }, React__default.createElement(Tooltip, null, formatStatusArray(intl, readByOthers)), React__default.createElement("svg", {
          width: "16",
          height: "16",
          xmlns: "http://www.w3.org/2000/svg"
        }, React__default.createElement("path", {
          d: "M8 0a8 8 0 1 1 0 16A8 8 0 0 1 8 0zm3.72 6.633a.955.955 0 1 0-1.352-1.352L6.986 8.663 5.633 7.31A.956.956 0 1 0 4.28 8.663l2.029 2.028a.956.956 0 0 0 1.353 0l4.058-4.058z",
          fill: "#3f51b5",
          fillRule: "evenodd"
        })), readBy.length - 1 > 1 && React__default.createElement("span", {
          className: "str-chat__message-simple-status-number"
        }, readBy.length - 1));
      } else if (message.status === 'received' && message.id === lastReceivedId && !threadList) {
        return React__default.createElement("span", {
          className: "str-chat__message-simple-status"
        }, React__default.createElement(Tooltip, null, React__default.createElement(reactIntl.FormattedMessage, {
          id: "message.status.delivered",
          defaultMessage: "Delivered"
        })), React__default.createElement("svg", {
          width: "16",
          height: "16",
          xmlns: "http://www.w3.org/2000/svg"
        }, React__default.createElement("path", {
          d: "M8 0a8 8 0 1 1 0 16A8 8 0 0 1 8 0zm3.72 6.633a.955.955 0 1 0-1.352-1.352L6.986 8.663 5.633 7.31A.956.956 0 1 0 4.28 8.663l2.029 2.028a.956.956 0 0 0 1.353 0l4.058-4.058z",
          fill: "grey",
          fillRule: "evenodd"
        })));
      } else {
        return null;
      }
    });

    _defineProperty(_assertThisInitialized(_this), "renderMessageActions", function () {
      var _this$props2 = _this.props,
          Message = _this$props2.Message,
          getMessageActions = _this$props2.getMessageActions,
          messageListRect = _this$props2.messageListRect,
          handleFlag = _this$props2.handleFlag,
          handleMute = _this$props2.handleMute,
          handleEdit = _this$props2.handleEdit,
          handleDelete = _this$props2.handleDelete;
      var messageActions = getMessageActions();

      if (messageActions.length === 0) {
        return;
      }

      return React__default.createElement("div", null);
    });

    return _this;
  }

  _createClass(MessageSimple, [{
    key: "componentDidMount",
    // constructor() {
    //   super();
    //   window.addEventListener(languageChangedEventName, this.onLanguageChanged);
    // }
    value: function componentDidMount() {
      this._isMounted = true;
      window.addEventListener(languageChangedEventName, this.onLanguageChanged);
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      this._isMounted = false;

      if (!this.props.message.deleted_at) {
        document.removeEventListener('click', this._closeDetailedReactions);
        document.removeEventListener('touchend', this._closeDetailedReactions);
      }

      document.removeEventListener(languageChangedEventName, this.onLanguageChanged);
    }
  }, {
    key: "isMine",
    value: function isMine() {
      return this.props.isMyMessage(this.props.message);
    } // formatArray = (arr) => {
    //   let outStr = '';
    //   const slicedArr = arr
    //     .filter((item) => item.id !== this.props.client.user.id)
    //     .map((item) => item.name || item.id)
    //     .slice(0, 5);
    //   const restLength = arr.length - slicedArr.length;
    //   const lastStr = restLength > 0 ? ' and ' + restLength + ' more' : '';
    //   if (slicedArr.length === 1) {
    //     outStr = slicedArr[0] + ' ';
    //   } else if (slicedArr.length === 2) {
    //     //joins all with "and" but =no commas
    //     //example: "bob and sam"
    //     outStr = slicedArr.join(' and ') + ' ';
    //   } else if (slicedArr.length > 2) {
    //     //joins all with commas, but last one gets ", and" (oxford comma!)
    //     //example: "bob, joe, and sam"
    //     outStr = slicedArr.join(', ') + lastStr;
    //   }
    //   return outStr;
    // };

  }, {
    key: "renderOptions",
    value: function renderOptions() {
      var _this$props3 = this.props,
          message = _this$props3.message,
          initialMessage = _this$props3.initialMessage,
          channelConfig = _this$props3.channelConfig,
          threadList = _this$props3.threadList,
          handleOpenThread = _this$props3.handleOpenThread;

      if (message.type === 'error' || message.type === 'system' || message.type === 'ephemeral' || message.status === 'failed' || message.status === 'sending' || initialMessage) {
        return;
      }

      if (this.isMine()) {
        return React__default.createElement("div", {
          className: "str-chat__message-simple__actions"
        }, this.renderMessageActions(), !threadList && channelConfig && channelConfig.replies && React__default.createElement("div", {
          onClick: handleOpenThread,
          className: "str-chat__message-simple__actions__action str-chat__message-simple__actions__action--thread"
        }, React__default.createElement("svg", {
          width: "14",
          height: "10",
          xmlns: "http://www.w3.org/2000/svg"
        }, React__default.createElement("path", {
          d: "M8.516 3c4.78 0 4.972 6.5 4.972 6.5-1.6-2.906-2.847-3.184-4.972-3.184v2.872L3.772 4.994 8.516.5V3zM.484 5l4.5-4.237v1.78L2.416 5l2.568 2.125v1.828L.484 5z",
          fillRule: "evenodd"
        }))), channelConfig && channelConfig.reactions && React__default.createElement("div", {
          className: "str-chat__message-simple__actions__action str-chat__message-simple__actions__action--reactions",
          onClick: this._clickReactionList
        }, React__default.createElement("svg", {
          width: "16",
          height: "14",
          viewBox: "0 0 16 14",
          xmlns: "http://www.w3.org/2000/svg"
        }, React__default.createElement("path", {
          d: "M11.108 8.05a.496.496 0 0 1 .212.667C10.581 10.147 8.886 11 7 11c-1.933 0-3.673-.882-4.33-2.302a.497.497 0 0 1 .9-.417C4.068 9.357 5.446 10 7 10c1.519 0 2.869-.633 3.44-1.738a.495.495 0 0 1 .668-.212zm.792-1.826a.477.477 0 0 1-.119.692.541.541 0 0 1-.31.084.534.534 0 0 1-.428-.194c-.106-.138-.238-.306-.539-.306-.298 0-.431.168-.54.307A.534.534 0 0 1 9.538 7a.544.544 0 0 1-.31-.084.463.463 0 0 1-.117-.694c.33-.423.742-.722 1.394-.722.653 0 1.068.3 1.396.724zm-7 0a.477.477 0 0 1-.119.692.541.541 0 0 1-.31.084.534.534 0 0 1-.428-.194c-.106-.138-.238-.306-.539-.306-.299 0-.432.168-.54.307A.533.533 0 0 1 2.538 7a.544.544 0 0 1-.31-.084.463.463 0 0 1-.117-.694c.33-.423.742-.722 1.394-.722.653 0 1.068.3 1.396.724zM7 0a7 7 0 1 1 0 14A7 7 0 0 1 7 0zm4.243 11.243A5.96 5.96 0 0 0 13 7a5.96 5.96 0 0 0-1.757-4.243A5.96 5.96 0 0 0 7 1a5.96 5.96 0 0 0-4.243 1.757A5.96 5.96 0 0 0 1 7a5.96 5.96 0 0 0 1.757 4.243A5.96 5.96 0 0 0 7 13a5.96 5.96 0 0 0 4.243-1.757z",
          fillRule: "evenodd"
        }))));
      } else {
        return React__default.createElement("div", {
          className: "str-chat__message-simple__actions"
        }, channelConfig && channelConfig.reactions && React__default.createElement("div", {
          className: "str-chat__message-simple__actions__action str-chat__message-simple__actions__action--reactions",
          onClick: this._clickReactionList
        }, React__default.createElement("svg", {
          width: "14",
          height: "14",
          xmlns: "http://www.w3.org/2000/svg"
        }, React__default.createElement("path", {
          d: "M11.108 8.05a.496.496 0 0 1 .212.667C10.581 10.147 8.886 11 7 11c-1.933 0-3.673-.882-4.33-2.302a.497.497 0 0 1 .9-.417C4.068 9.357 5.446 10 7 10c1.519 0 2.869-.633 3.44-1.738a.495.495 0 0 1 .668-.212zm.792-1.826a.477.477 0 0 1-.119.692.541.541 0 0 1-.31.084.534.534 0 0 1-.428-.194c-.106-.138-.238-.306-.539-.306-.298 0-.431.168-.54.307A.534.534 0 0 1 9.538 7a.544.544 0 0 1-.31-.084.463.463 0 0 1-.117-.694c.33-.423.742-.722 1.394-.722.653 0 1.068.3 1.396.724zm-7 0a.477.477 0 0 1-.119.692.541.541 0 0 1-.31.084.534.534 0 0 1-.428-.194c-.106-.138-.238-.306-.539-.306-.299 0-.432.168-.54.307A.533.533 0 0 1 2.538 7a.544.544 0 0 1-.31-.084.463.463 0 0 1-.117-.694c.33-.423.742-.722 1.394-.722.653 0 1.068.3 1.396.724zM7 0a7 7 0 1 1 0 14A7 7 0 0 1 7 0zm4.243 11.243A5.96 5.96 0 0 0 13 7a5.96 5.96 0 0 0-1.757-4.243A5.96 5.96 0 0 0 7 1a5.96 5.96 0 0 0-4.243 1.757A5.96 5.96 0 0 0 1 7a5.96 5.96 0 0 0 1.757 4.243A5.96 5.96 0 0 0 7 13a5.96 5.96 0 0 0 4.243-1.757z",
          fillRule: "evenodd"
        }))), !threadList && channelConfig && channelConfig.replies && React__default.createElement("div", {
          onClick: handleOpenThread,
          className: "str-chat__message-simple__actions__action str-chat__message-simple__actions__action--thread"
        }, React__default.createElement("svg", {
          width: "14",
          height: "10",
          xmlns: "http://www.w3.org/2000/svg"
        }, React__default.createElement("path", {
          d: "M8.516 3c4.78 0 4.972 6.5 4.972 6.5-1.6-2.906-2.847-3.184-4.972-3.184v2.872L3.772 4.994 8.516.5V3zM.484 5l4.5-4.237v1.78L2.416 5l2.568 2.125v1.828L.484 5z",
          fillRule: "evenodd"
        }))), this.renderMessageActions());
      }
    } // eslint-disable-next-line

  }, {
    key: "render",
    value: function render() {
      var _this$props4 = this.props,
          message = _this$props4.message,
          Attachment$$1 = _this$props4.Attachment,
          editing = _this$props4.editing,
          clearEditingState = _this$props4.clearEditingState,
          handleRetry = _this$props4.handleRetry,
          updateMessage = _this$props4.updateMessage,
          handleReaction = _this$props4.handleReaction,
          actionsEnabled = _this$props4.actionsEnabled,
          messageListRect = _this$props4.messageListRect,
          handleAction = _this$props4.handleAction,
          onMentionsHoverMessage = _this$props4.onMentionsHoverMessage,
          onMentionsClickMessage = _this$props4.onMentionsClickMessage,
          unsafeHTML = _this$props4.unsafeHTML,
          threadList = _this$props4.threadList,
          handleOpenThread = _this$props4.handleOpenThread;
      var when = moment(message.created_at).calendar();
      var messageClasses = this.isMine() ? 'str-chat__message str-chat__message--me str-chat__message-simple str-chat__message-simple--me' : 'str-chat__message str-chat__message-simple';
      var hasAttachment = Boolean(message && message.attachments && message.attachments.length);
      var images = hasAttachment && message.attachments.filter(function (item) {
        return item.type === 'image';
      });
      var hasReactions = Boolean(message.latest_reactions && message.latest_reactions.length);

      if (message.type === 'message.read' || message.type === 'message.date') {
        return null;
      }

      if (message.deleted_at) {
        return React__default.createElement(React__default.Fragment, null, React__default.createElement("div", {
          key: message.id,
          className: "".concat(messageClasses, " str-chat__message--deleted ").concat(message.type, " ")
        }, React__default.createElement("div", {
          className: "str-chat__message--deleted-inner"
        }, React__default.createElement(reactIntl.FormattedMessage, {
          id: "message.deleted",
          defaultMessage: "This message was deleted..."
        }))));
      }

      return React__default.createElement(React__default.Fragment, null, editing && React__default.createElement(Modal, {
        open: editing,
        onClose: clearEditingState
      }, React__default.createElement(exports.MessageInput, _extends({
        Input: EditMessageForm,
        message: message,
        clearEditingState: clearEditingState,
        updateMessage: updateMessage
      }, this.props.additionalMessageInputProps))), React__default.createElement("div", {
        key: message.id,
        className: "\n\t\t\t\t\t\t".concat(messageClasses, "\n\t\t\t\t\t\tstr-chat__message--").concat(message.type, "\n\t\t\t\t\t\tstr-chat__message--").concat(message.status, "\n\t\t\t\t\t\t").concat(message.text ? 'str-chat__message--has-text' : 'has-no-text', "\n\t\t\t\t\t\t").concat(hasAttachment ? 'str-chat__message--has-attachment' : '', "\n\t\t\t\t\t\t").concat(hasReactions ? 'str-chat__message--with-reactions' : '', "\n\t\t\t\t\t").trim(),
        onMouseLeave: this._hideOptions,
        ref: this.messageRef
      }, this.renderStatus(), React__default.createElement(Avatar, {
        image: message.user.image,
        name: message.user.name || message.user.id
      }), React__default.createElement("div", {
        className: "str-chat__message-inner",
        onClick: message.status === 'failed' ? handleRetry.bind(this, message) : null
      }, !message.text && React__default.createElement(React__default.Fragment, null, this.renderOptions(), hasReactions && !this.state.showDetailedReactions && React__default.createElement(ReactionsList, {
        reactions: message.latest_reactions,
        reaction_counts: message.reaction_counts,
        onClick: this._clickReactionList,
        reverse: true
      }), this.state.showDetailedReactions && React__default.createElement(ReactionSelector, {
        handleReaction: handleReaction,
        detailedView: true,
        reaction_counts: message.reaction_counts,
        latest_reactions: message.latest_reactions,
        messageList: messageListRect,
        ref: this.reactionSelectorRef
      })), React__default.createElement("div", {
        className: "str-chat__message-attachment-container"
      }, hasAttachment && message.attachments.map(function (attachment, index) {
        if (attachment.type === 'image' && images.length > 1) return null;
        return React__default.createElement(Attachment$$1, {
          key: "".concat(message.id, "-").concat(index),
          attachment: attachment,
          actionHandler: handleAction
        });
      })), images.length > 1 && React__default.createElement(Gallery, {
        images: images
      }), message.text && React__default.createElement("div", {
        className: "str-chat__message-text"
      }, React__default.createElement("div", {
        className: "\n\t\t\t\t\t\t\t\t\tstr-chat__message-text-inner str-chat__message-simple-text-inner\n\t\t\t\t\t\t\t\t\t".concat(this.state.isFocused ? 'str-chat__message-text-inner--focused' : '', "\n\t\t\t\t\t\t\t\t\t").concat(hasAttachment ? 'str-chat__message-text-inner--has-attachment' : '', "\n\t\t\t\t\t\t\t\t\t").concat(isOnlyEmojis(message.text) ? 'str-chat__message-simple-text-inner--is-emoji' : '', "\n                ").trim(),
        onMouseOver: onMentionsHoverMessage,
        onClick: onMentionsClickMessage
      }, message.type === 'error' && React__default.createElement("div", {
        className: "str-chat__simple-message--error-message"
      }, React__default.createElement(reactIntl.FormattedMessage, {
        id: "message.error",
        defaultMessage: "Error \xB7 Unsent"
      })), message.status === 'failed' && React__default.createElement("div", {
        className: "str-chat__simple-message--error-message"
      }, React__default.createElement(reactIntl.FormattedMessage, {
        id: "message.failed",
        defaultMessage: "Message Failed \xB7 Click to try again"
      })), unsafeHTML ? React__default.createElement("div", {
        dangerouslySetInnerHTML: {
          __html: message.html
        }
      }) : renderText(message), hasReactions && !this.state.showDetailedReactions && React__default.createElement(ReactionsList, {
        reactions: message.latest_reactions,
        reaction_counts: message.reaction_counts,
        onClick: this._clickReactionList,
        reverse: true
      }), this.state.showDetailedReactions && React__default.createElement(ReactionSelector, {
        mine: this.isMine(),
        handleReaction: handleReaction,
        actionsEnabled: actionsEnabled,
        detailedView: true,
        reaction_counts: message.reaction_counts,
        latest_reactions: message.latest_reactions,
        messageList: messageListRect,
        ref: this.reactionSelectorRef
      })), message.text && this.renderOptions()), !threadList && message.reply_count !== 0 && React__default.createElement("div", {
        className: "str-chat__message-simple-reply-button"
      }, React__default.createElement(MessageRepliesCountButton, {
        onClick: handleOpenThread,
        reply_count: message.reply_count
      })), React__default.createElement("div", {
        className: "str-chat__message-data str-chat__message-simple-data"
      }, !this.isMine() ? React__default.createElement("span", {
        className: "str-chat__message-simple-name"
      }, message.user.name || message.user.id) : null, React__default.createElement("span", {
        className: "str-chat__message-simple-timestamp"
      }, when)))));
    }
  }]);

  return MessageSimple;
}(React.PureComponent);

_defineProperty(exports.MessageSimple, "propTypes", {
  /** The [message object](https://getstream.io/chat/docs/#message_format) */
  message: PropTypes.object,

  /**
   * The attachment UI component.
   * Default: [Attachment](https://github.com/GetStream/stream-chat-react/blob/master/src/components/Attachment.js)
   * */
  Attachment: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   * @deprecated Its not recommended to use this anymore. All the methods in this HOC are provided explicitly.
   *
   * The higher order message component, most logic is delegated to this component
   * @see See [Message HOC](https://getstream.github.io/stream-chat-react/#message) for example
   *
   * */
  Message: PropTypes.oneOfType([PropTypes.node, PropTypes.func, PropTypes.object]).isRequired,

  /** render HTML instead of markdown. Posting HTML is only allowed server-side */
  unsafeHTML: PropTypes.bool,

  /** Client object */
  client: PropTypes.object,

  /** If its parent message in thread. */
  initialMessage: PropTypes.bool,

  /** Channel config object */
  channelConfig: PropTypes.object,

  /** If component is in thread list */
  threadList: PropTypes.bool,

  /** Function to open thread on current messxage */
  handleOpenThread: PropTypes.func,

  /** If the message is in edit state */
  editing: PropTypes.bool,

  /** Function to exit edit state */
  clearEditingState: PropTypes.func,

  /** Returns true if message belongs to current user */
  isMyMessage: PropTypes.func,

  /** Returns all allowed actions on message by current user e.g., [edit, delete, flag, mute] */
  getMessageActions: PropTypes.func,

  /**
   * Function to publish updates on message to channel
   *
   * @param message Updated [message object](https://getstream.io/chat/docs/#message_format)
   * */
  updateMessage: PropTypes.func,

  /**
   * Reattempt sending a message
   * @param message A [message object](https://getstream.io/chat/docs/#message_format) to resent.
   */
  handleRetry: PropTypes.func,

  /**
   * Add or remove reaction on message
   *
   * @param type Type of reaction - 'like' | 'love' | 'haha' | 'wow' | 'sad' | 'angry'
   * @param event Dom event which triggered this function
   */
  handleReaction: PropTypes.func,

  /** If actions such as edit, delete, flag, mute are enabled on message */
  actionsEnabled: PropTypes.bool,

  /** DOMRect object for parent MessageList component */
  messageListRect: PropTypes.object,

  /**
   * Handler for actions. Actions in combination with attachments can be used to build [commands](https://getstream.io/chat/docs/#channel_commands).
   *
   * @param name {string} Name of action
   * @param value {string} Value of action
   * @param event Dom event that triggered this handler
   */
  handleAction: PropTypes.func,

  /**
   * The handler for hover event on @mention in message
   *
   * @param event Dom hover event which triggered handler.
   * @param user Target user object
   */
  onMentionsHoverMessage: PropTypes.func,

  /**
   * The handler for click event on @mention in message
   *
   * @param event Dom click event which triggered handler.
   * @param user Target user object
   */
  onMentionsClickMessage: PropTypes.func,

  /**
   * Additional props for underlying MessageInput component.
   * Available props - https://getstream.github.io/stream-chat-react/#messageinput
   * */
  additionalMessageInputProps: PropTypes.object
});

_defineProperty(exports.MessageSimple, "defaultProps", {
  Attachment: Attachment
});

exports.MessageSimple = reactIntl.injectIntl(exports.MessageSimple);

/**
 * Message - A high level component which implements all the logic required for a message.
 * The actual rendering of the message is delegated via the "Message" property
 *
 * @example ./docs/Message.md
 * @extends Component
 */

exports.Message =
/*#__PURE__*/
function (_Component) {
  _inherits(Message, _Component);

  function Message(props) {
    var _this;

    _classCallCheck(this, Message);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(Message).call(this, props));

    _defineProperty(_assertThisInitialized(_this), "isMyMessage", function (message) {
      return _this.props.client.user.id === message.user.id;
    });

    _defineProperty(_assertThisInitialized(_this), "isAdmin", function () {
      return _this.props.client.user.role === 'admin' || _this.props.members && _this.props.members[_this.props.client.user.id] && _this.props.members[_this.props.client.user.id].role === 'admin';
    });

    _defineProperty(_assertThisInitialized(_this), "isOwner", function () {
      return _this.props.members && _this.props.members[_this.props.client.user.id] && _this.props.members[_this.props.client.user.id].role === 'owner';
    });

    _defineProperty(_assertThisInitialized(_this), "isModerator", function () {
      return _this.props.members && _this.props.members[_this.props.client.user.id] && _this.props.members[_this.props.client.user.id].role === 'moderator';
    });

    _defineProperty(_assertThisInitialized(_this), "canEditMessage", function (message) {
      return _this.isMyMessage(message) || _this.isModerator() || _this.isOwner() || _this.isAdmin();
    });

    _defineProperty(_assertThisInitialized(_this), "canDeleteMessage", function (message) {
      return _this.canEditMessage(message);
    });

    _defineProperty(_assertThisInitialized(_this), "validateAndGetNotificationMessage", function (func, args) {
      if (!func || typeof func !== 'function') return false;
      var returnValue = func.apply(null, args);
      if (typeof returnValue !== 'string') return false;
      return returnValue;
    });

    _defineProperty(_assertThisInitialized(_this), "handleFlag",
    /*#__PURE__*/
    function () {
      var _ref = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee(event) {
        var _this$props, getFlagMessageSuccessNotification, getFlagMessageErrorNotification, intl, message, successMessage, errorMessage;

        return _regeneratorRuntime.wrap(function _callee$(_context) {
          while (1) {
            switch (_context.prev = _context.next) {
              case 0:
                event.preventDefault();
                _this$props = _this.props, getFlagMessageSuccessNotification = _this$props.getFlagMessageSuccessNotification, getFlagMessageErrorNotification = _this$props.getFlagMessageErrorNotification, intl = _this$props.intl;
                message = _this.props.message;
                _context.prev = 3;
                _context.next = 6;
                return _this.props.client.flagMessage(message.id);

              case 6:
                successMessage = _this.validateAndGetNotificationMessage(getFlagMessageSuccessNotification, [message]);

                _this.props.addNotification(successMessage ? successMessage : intl.formatMessage({
                  id: 'message.flag.success',
                  defaultMessage: 'Message has been successfully flagged'
                }), 'success');

                _context.next = 14;
                break;

              case 10:
                _context.prev = 10;
                _context.t0 = _context["catch"](3);
                errorMessage = _this.validateAndGetNotificationMessage(getFlagMessageErrorNotification, [message]);

                _this.props.addNotification(errorMessage ? errorMessage : intl.formatMessage({
                  id: 'message.flag.error',
                  defaultMessage: 'Error adding flag: Either the flag already exist or there is issue with network connection ...'
                }), 'error');

              case 14:
              case "end":
                return _context.stop();
            }
          }
        }, _callee, null, [[3, 10]]);
      }));

      return function (_x) {
        return _ref.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "handleMute",
    /*#__PURE__*/
    function () {
      var _ref2 = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee2(event) {
        var _this$props2, getMuteUserSuccessNotification, getMuteUserErrorNotification, intl, message, successMessage, errorMessage;

        return _regeneratorRuntime.wrap(function _callee2$(_context2) {
          while (1) {
            switch (_context2.prev = _context2.next) {
              case 0:
                event.preventDefault();
                _this$props2 = _this.props, getMuteUserSuccessNotification = _this$props2.getMuteUserSuccessNotification, getMuteUserErrorNotification = _this$props2.getMuteUserErrorNotification, intl = _this$props2.intl;
                message = _this.props.message;
                _context2.prev = 3;
                _context2.next = 6;
                return _this.props.client.muteUser(message.user.id);

              case 6:
                successMessage = _this.validateAndGetNotificationMessage(getMuteUserSuccessNotification, [message.user]);

                _this.props.addNotification(successMessage ? successMessage : intl.formatMessage({
                  id: 'message.mute.success',
                  defaultMessage: 'User with id {id} has been muted'
                }, {
                  id: message.user.id
                }), 'success');

                _context2.next = 14;
                break;

              case 10:
                _context2.prev = 10;
                _context2.t0 = _context2["catch"](3);
                errorMessage = _this.validateAndGetNotificationMessage(getMuteUserErrorNotification, [message.user]);

                _this.props.addNotification(errorMessage ? errorMessage : intl.formatMessage({
                  id: 'message.mute.error',
                  defaultMessage: 'Error muting a user ...'
                }), 'error');

              case 14:
              case "end":
                return _context2.stop();
            }
          }
        }, _callee2, null, [[3, 10]]);
      }));

      return function (_x2) {
        return _ref2.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "handleEdit", function (event) {
      if (event !== undefined && event.preventDefault) {
        event.preventDefault();
      }

      _this.props.setEditingState(_this.props.message);
    });

    _defineProperty(_assertThisInitialized(_this), "handleDelete",
    /*#__PURE__*/
    function () {
      var _ref3 = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee3(event) {
        var message, data$$1;
        return _regeneratorRuntime.wrap(function _callee3$(_context3) {
          while (1) {
            switch (_context3.prev = _context3.next) {
              case 0:
                event.preventDefault();
                message = _this.props.message;
                _context3.next = 4;
                return _this.props.client.deleteMessage(message.id);

              case 4:
                data$$1 = _context3.sent;

                _this.props.updateMessage(data$$1.message);

              case 6:
              case "end":
                return _context3.stop();
            }
          }
        }, _callee3);
      }));

      return function (_x3) {
        return _ref3.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "handleReaction",
    /*#__PURE__*/
    function () {
      var _ref4 = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee4(reactionType, event) {
        var userExistingReaction, currentUser, _iteratorNormalCompletion, _didIteratorError, _iteratorError, _iterator, _step, _reaction, originalMessage, reactionChangePromise, messageID, reaction;

        return _regeneratorRuntime.wrap(function _callee4$(_context4) {
          while (1) {
            switch (_context4.prev = _context4.next) {
              case 0:
                if (event !== undefined && event.preventDefault) {
                  event.preventDefault();
                }

                userExistingReaction = null;
                currentUser = _this.props.client.userID;
                _iteratorNormalCompletion = true;
                _didIteratorError = false;
                _iteratorError = undefined;
                _context4.prev = 6;

                for (_iterator = _this.props.message.own_reactions[Symbol.iterator](); !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
                  _reaction = _step.value;

                  // own user should only ever contain the current user id
                  // just in case we check to prevent bugs with message updates from breaking reactions
                  if (currentUser === _reaction.user.id && _reaction.type === reactionType) {
                    userExistingReaction = _reaction;
                  } else if (currentUser !== _reaction.user.id) {
                    console.warn("message.own_reactions contained reactions from a different user, this indicates a bug");
                  }
                }

                _context4.next = 14;
                break;

              case 10:
                _context4.prev = 10;
                _context4.t0 = _context4["catch"](6);
                _didIteratorError = true;
                _iteratorError = _context4.t0;

              case 14:
                _context4.prev = 14;
                _context4.prev = 15;

                if (!_iteratorNormalCompletion && _iterator.return != null) {
                  _iterator.return();
                }

              case 17:
                _context4.prev = 17;

                if (!_didIteratorError) {
                  _context4.next = 20;
                  break;
                }

                throw _iteratorError;

              case 20:
                return _context4.finish(17);

              case 21:
                return _context4.finish(14);

              case 22:
                originalMessage = _this.props.message;

                /*
                - Add the reaction to the local state
                - Make the API call in the background
                - If it fails, revert to the old message...
                 */
                if (userExistingReaction) {
                  // this.props.channel.state.removeReaction(userExistingReaction);
                  reactionChangePromise = _this.props.channel.deleteReaction(_this.props.message.id, userExistingReaction.type);
                } else {
                  // add the reaction
                  messageID = _this.props.message.id;
                  reaction = {
                    type: reactionType
                  }; // this.props.channel.state.addReaction(tmpReaction, this.props.message);

                  reactionChangePromise = _this.props.channel.sendReaction(messageID, reaction);
                }

                _context4.prev = 24;
                _context4.next = 27;
                return reactionChangePromise;

              case 27:
                _context4.next = 32;
                break;

              case 29:
                _context4.prev = 29;
                _context4.t1 = _context4["catch"](24);

                // revert to the original message if the API call fails
                _this.props.updateMessage(originalMessage);

              case 32:
              case "end":
                return _context4.stop();
            }
          }
        }, _callee4, null, [[6, 10, 14, 22], [15,, 17, 21], [24, 29]]);
      }));

      return function (_x4, _x5) {
        return _ref4.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "handleAction",
    /*#__PURE__*/
    function () {
      var _ref5 = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee5(name, value, event) {
        var messageID, formData, data$$1;
        return _regeneratorRuntime.wrap(function _callee5$(_context5) {
          while (1) {
            switch (_context5.prev = _context5.next) {
              case 0:
                event.preventDefault();
                messageID = _this.props.message.id;
                formData = {};
                formData[name] = value;
                _context5.next = 6;
                return _this.props.channel.sendAction(messageID, formData);

              case 6:
                data$$1 = _context5.sent;

                if (data$$1 && data$$1.message) {
                  _this.props.updateMessage(data$$1.message);
                } else {
                  _this.props.removeMessage(_this.props.message);
                }

              case 8:
              case "end":
                return _context5.stop();
            }
          }
        }, _callee5);
      }));

      return function (_x6, _x7, _x8) {
        return _ref5.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "handleRetry",
    /*#__PURE__*/
    function () {
      var _ref6 = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee6(message) {
        return _regeneratorRuntime.wrap(function _callee6$(_context6) {
          while (1) {
            switch (_context6.prev = _context6.next) {
              case 0:
                _context6.next = 2;
                return _this.props.retrySendMessage(message);

              case 2:
              case "end":
                return _context6.stop();
            }
          }
        }, _callee6);
      }));

      return function (_x9) {
        return _ref6.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "onMentionsClick", function (e) {
      if (typeof _this.props.onMentionsClick !== 'function') {
        return;
      }

      _this.props.onMentionsClick(e, _this.props.message.mentioned_users);
    });

    _defineProperty(_assertThisInitialized(_this), "onMentionsHover", function (e) {
      if (typeof _this.props.onMentionsHover !== 'function') {
        return;
      }

      _this.props.onMentionsHover(e, _this.props.message.mentioned_users);
    });

    _defineProperty(_assertThisInitialized(_this), "getMessageActions", function () {
      var _this$props3 = _this.props,
          message = _this$props3.message,
          messageActionsProps = _this$props3.messageActions;
      var messageActionsAfterPermission = [];
      var messageActions = [];

      if (messageActionsProps && typeof messageActionsProps === 'boolean') {
        // If value of messageActionsProps is true, then populate all the possible values
        messageActions = Object.keys(MESSAGE_ACTIONS);
      } else if (messageActionsProps && messageActionsProps.length > 0) {
        messageActions = _toConsumableArray(messageActionsProps);
      } else {
        return [];
      }

      if (_this.canEditMessage(message) && messageActions.indexOf(MESSAGE_ACTIONS.edit) > -1) {
        messageActionsAfterPermission.push(MESSAGE_ACTIONS.edit);
      }

      if (_this.canDeleteMessage(message) && messageActions.indexOf(MESSAGE_ACTIONS.delete) > -1) {
        messageActionsAfterPermission.push(MESSAGE_ACTIONS.delete);
      }

      if (!_this.isMyMessage(message) && messageActions.indexOf(MESSAGE_ACTIONS.flag) > -1) {
        messageActionsAfterPermission.push(MESSAGE_ACTIONS.flag);
      }

      if (!_this.isMyMessage(message) && messageActions.indexOf(MESSAGE_ACTIONS.mute) > -1) {
        messageActionsAfterPermission.push(MESSAGE_ACTIONS.mute);
      }

      return messageActionsAfterPermission;
    });

    _this.state = {
      loading: false
    };
    return _this;
  }

  _createClass(Message, [{
    key: "shouldComponentUpdate",
    value: function shouldComponentUpdate(nextProps) {
      // since there are many messages its important to only rerender messages when needed.
      var shouldUpdate = nextProps.message !== this.props.message;


      if (!shouldUpdate && !deepequal(nextProps.readBy, this.props.readBy)) {
        shouldUpdate = true;
      } // group style often changes for the last 3 messages...


      if (!shouldUpdate && !deepequal(nextProps.groupStyles, this.props.groupStyles)) {
        shouldUpdate = true;
      } // if lastreceivedId changesm, message should update.


      if (!shouldUpdate && !deepequal(nextProps.lastReceivedId, this.props.lastReceivedId)) {
        shouldUpdate = true;
      } // editing is the last one which can trigger a change..


      if (!shouldUpdate && nextProps.editing !== this.props.editing) {
        shouldUpdate = true;
      } // editing is the last one which can trigger a change..


      if (!shouldUpdate && nextProps.messageListRect !== this.props.messageListRect) {
        shouldUpdate = true;
      }

      return shouldUpdate;
    }
  }, {
    key: "render",
    value: function render() {
      var config = this.props.channel.getConfig();
      var message = this.props.message;
      var actionsEnabled = message.type === 'regular' && message.status === 'received';
      var Component = this.props.Message;
      return (//Give all Messages a id to scroll to
        React__default.createElement("div", {
          id: this.props.message.id
        }, React__default.createElement(Component, _extends({}, this.props, {
          actionsEnabled: actionsEnabled,
          Message: this,
          handleReaction: this.handleReaction,
          getMessageActions: this.getMessageActions,
          handleFlag: this.handleFlag,
          handleMute: this.handleMute,
          handleAction: this.handleAction,
          handleDelete: this.handleDelete,
          handleEdit: this.handleEdit,
          handleRetry: this.handleRetry,
          handleOpenThread: this.props.openThread && this.props.openThread.bind(this, message),
          isMyMessage: this.isMyMessage,
          channelConfig: config,
          onMentionsClickMessage: this.onMentionsClick,
          onMentionsHoverMessage: this.onMentionsHover
        })))
      );
    }
  }]);

  return Message;
}(React.Component);

_defineProperty(exports.Message, "propTypes", {
  /** The message object */
  message: PropTypes.object.isRequired,

  /** The client connection object for connecting to Stream */
  client: PropTypes.object.isRequired,

  /** The current channel this message is displayed in */
  channel: PropTypes.object.isRequired,

  /** A list of users that have read this message **/
  readBy: PropTypes.array,

  /** groupStyles, a list of styles to apply to this message. ie. top, bottom, single etc */
  groupStyles: PropTypes.array,

  /** Editing, if the message is currently being edited */
  editing: PropTypes.bool,

  /**
   * Message UI component to display a message in message list.
   * Available from [channel context](https://getstream.github.io/stream-chat-react/#channelcontext)
   * */
  Message: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   * Attachment UI component to display attachment in individual message.
   * Available from [channel context](https://getstream.github.io/stream-chat-react/#channelcontext)
   * */
  Attachment: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /** render HTML instead of markdown. Posting HTML is only allowed server-side */
  unsafeHTML: PropTypes.bool,

  /**
   * Array of allowed actions on message. e.g. ['edit', 'delete', 'mute', 'flag']
   * If all the actions need to be disabled, empty array or false should be provided as value of prop.
   * */
  messageActions: PropTypes.oneOfType([PropTypes.bool, PropTypes.array]),

  /**
   * Function that returns message/text as string to be shown as notification, when request for flagging a message is successful
   *
   * This function should accept following params:
   *
   * @param message A [message object](https://getstream.io/chat/docs/#message_format) which is flagged.
   *
   * */
  getFlagMessageSuccessNotification: PropTypes.func,

  /**
   * Function that returns message/text as string to be shown as notification, when request for flagging a message runs into error
   *
   * This function should accept following params:
   *
   * @param message A [message object](https://getstream.io/chat/docs/#message_format) which is flagged.
   *
   * */
  getFlagMessageErrorNotification: PropTypes.func,

  /**
   * Function that returns message/text as string to be shown as notification, when request for muting a user is successful
   *
   * This function should accept following params:
   *
   * @param user A user object which is being muted
   *
   * */
  getMuteUserSuccessNotification: PropTypes.func,

  /**
   * Function that returns message/text as string to be shown as notification, when request for muting a user runs into error
   *
   * This function should accept following params:
   *
   * @param user A user object which is being muted
   *
   * */
  getMuteUserErrorNotification: PropTypes.func,

  /** Latest message id on current channel */
  lastReceivedId: PropTypes.string,

  /** DOMRect object for parent MessageList component */
  messageListRect: PropTypes.object,

  /** @see See [Channel Context](https://getstream.github.io/stream-chat-react/#channelcontext) */
  members: PropTypes.object,

  /**
   * Function to add custom notification on messagelist
   *
   * @param text Notification text to display
   * @param type Type of notification. 'success' | 'error'
   * */
  addNotification: PropTypes.func,

  /** Sets the editing state */
  setEditingState: PropTypes.func,

  /** @see See [Channel Context](https://getstream.github.io/stream-chat-react/#channelcontext) */
  updateMessage: PropTypes.func,

  /** @see See [Channel Context](https://getstream.github.io/stream-chat-react/#channelcontext) */
  removeMessage: PropTypes.func,

  /** @see See [Channel Context](https://getstream.github.io/stream-chat-react/#channelcontext) */
  retrySendMessage: PropTypes.func,

  /** @see See [Channel Context](https://getstream.github.io/stream-chat-react/#channelcontext) */
  onMentionsClick: PropTypes.func,

  /** @see See [Channel Context](https://getstream.github.io/stream-chat-react/#channelcontext) */
  onMentionsHover: PropTypes.func,

  /** @see See [Channel Context](https://getstream.github.io/stream-chat-react/#channelcontext) */
  openThread: PropTypes.func,

  /**
   * Additional props for underlying MessageInput component.
   * Available props - https://getstream.github.io/stream-chat-react/#messageinput
   * */
  additionalMessageInputProps: PropTypes.object
});

_defineProperty(exports.Message, "defaultProps", {
  Message: exports.MessageSimple,
  readBy: [],
  groupStyles: [],
  Attachment: Attachment,
  editing: false,
  messageActions: Object.keys(MESSAGE_ACTIONS)
});

exports.Message = reactIntl.injectIntl(exports.Message);

var EmptyStateIndicator = function EmptyStateIndicator(_ref) {
  var listType = _ref.listType;
  var Indicator;

  switch (listType) {
    case 'channel':
      Indicator = React__default.createElement("p", null, "You have no channels currently");
      break;

    case 'message':
      Indicator = null;
      break;

    default:
      Indicator = React__default.createElement("p", null, "No items exist");
      break;
  }

  return Indicator;
};

var ReverseInfiniteScroll =
/*#__PURE__*/
function (_Component) {
  _inherits(ReverseInfiniteScroll, _Component);

  function ReverseInfiniteScroll(props) {
    var _this;

    _classCallCheck(this, ReverseInfiniteScroll);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(ReverseInfiniteScroll).call(this, props));
    _this.scrollListener = _this.scrollListener.bind(_assertThisInitialized(_this));
    _this.scrollEventCount = 0;
    return _this;
  }

  _createClass(ReverseInfiniteScroll, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      this.pageLoaded = this.props.pageStart;
      this.attachScrollListener();
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      this.detachScrollListener();
      this.detachMousewheelListener();
    } // Set a defaut loader for all your `InfiniteScroll` components

  }, {
    key: "setDefaultLoader",
    value: function setDefaultLoader(loader) {
      this.defaultLoader = loader;
    }
  }, {
    key: "detachMousewheelListener",
    value: function detachMousewheelListener() {
      var scrollEl = window;

      if (this.props.useWindow === false) {
        scrollEl = this.scrollComponent.parentNode;
      }

      scrollEl.removeEventListener('mousewheel', this.mousewheelListener, this.props.useCapture);
    }
  }, {
    key: "detachScrollListener",
    value: function detachScrollListener() {
      var scrollEl = window;

      if (this.props.useWindow === false) {
        scrollEl = this.getParentElement(this.scrollComponent);
      }

      scrollEl.removeEventListener('scroll', this.scrollListener, this.props.useCapture);
      scrollEl.removeEventListener('resize', this.scrollListener, this.props.useCapture);
    }
  }, {
    key: "getParentElement",
    value: function getParentElement(el) {
      return el && el.parentNode;
    }
  }, {
    key: "filterProps",
    value: function filterProps(props) {
      return props;
    }
  }, {
    key: "attachScrollListener",
    value: function attachScrollListener() {
      if (!this.props.hasMore || this.props.isLoading || !this.getParentElement(this.scrollComponent)) {
        return;
      }

      var scrollEl = window;

      if (this.props.useWindow === false) {
        scrollEl = this.getParentElement(this.scrollComponent);
      }

      scrollEl.addEventListener('mousewheel', this.mousewheelListener, this.props.useCapture);
      scrollEl.addEventListener('scroll', this.scrollListener, this.props.useCapture);
      scrollEl.addEventListener('resize', this.scrollListener, this.props.useCapture);

      if (this.props.initialLoad) {
        this.scrollListener();
      }
    }
  }, {
    key: "mousewheelListener",
    value: function mousewheelListener(e) {
      // Prevents Chrome hangups
      // See: https://stackoverflow.com/questions/47524205/random-high-content-download-time-in-chrome/47684257#47684257
      if (e.deltaY === 1) {
        e.preventDefault();
      }
    }
  }, {
    key: "scrollListener",
    value: function scrollListener() {
      var el = this.scrollComponent;
      var parentNode = this.getParentElement(el);
      this.scrollEventCount += 1;
      var offset;
      var reverseOffset = parentNode.scrollTop;
      var standardOffset = el.scrollHeight - parentNode.scrollTop - parentNode.clientHeight;

      if (this.props.isReverse) {
        offset = reverseOffset;
      } else {
        offset = standardOffset;
      }

      if (this.props.listenToScroll) {
        this.props.listenToScroll(standardOffset, reverseOffset);
      } // a reverse infinite scroll element always starts out at position 0
      // this counter prevent you from loading content before the user even scrolled


      if (this.scrollEventCount < 2) {
        return;
      } // prevent crazy repeat requests in case you don't have more


      if (!this.props.hasMore || this.props.isLoading) {
        return;
      } // Here we make sure the element is visible as well as checking the offset


      if (offset < Number(this.props.threshold) && el && el.offsetParent !== null) {
        //this.detachScrollListener();
        // Call loadMore after detachScrollListener to allow for non-async loadMore functions
        if (typeof this.props.loadMore === 'function') {
          this.props.loadMore(this.pageLoaded += 1);
        }
      }
    }
  }, {
    key: "calculateOffset",
    value: function calculateOffset(el, scrollTop) {
      if (!el) {
        return 0;
      }

      return this.calculateTopPosition(el) + (el.offsetHeight - scrollTop - window.innerHeight);
    }
  }, {
    key: "calculateTopPosition",
    value: function calculateTopPosition(el) {
      if (!el) {
        return 0;
      }

      return el.offsetTop + this.calculateTopPosition(el.offsetParent);
    }
  }, {
    key: "render",
    value: function render() {
      var _this2 = this;

      var renderProps = this.filterProps(this.props);

      var children = renderProps.children,
          element = renderProps.element,
          hasMore = renderProps.hasMore,
          initialLoad = renderProps.initialLoad,
          isReverse = renderProps.isReverse,
          loader = renderProps.loader,
          loadMore = renderProps.loadMore,
          pageStart = renderProps.pageStart,
          ref = renderProps.ref,
          threshold = renderProps.threshold,
          useCapture = renderProps.useCapture,
          useWindow = renderProps.useWindow,
          listenToScroll = renderProps.listenToScroll,
          isLoading = renderProps.isLoading,
          props = _objectWithoutProperties(renderProps, ["children", "element", "hasMore", "initialLoad", "isReverse", "loader", "loadMore", "pageStart", "ref", "threshold", "useCapture", "useWindow", "listenToScroll", "isLoading"]);

      props.ref = function (node) {
        _this2.scrollComponent = node;

        if (ref) {
          ref(node);
        }
      };

      var childrenArray = [children];

      if (isLoading) {
        if (loader) {
          isReverse ? childrenArray.unshift(loader) : childrenArray.push(loader);
        } else if (this.defaultLoader) {
          isReverse ? childrenArray.unshift(this.defaultLoader) : childrenArray.push(this.defaultLoader);
        }
      }

      return React__default.createElement(element, props, childrenArray);
    }
  }]);

  return ReverseInfiniteScroll;
}(React.Component);

_defineProperty(ReverseInfiniteScroll, "propTypes", {
  children: PropTypes.node.isRequired,
  element: PropTypes.node,

  /** Weather there are more elements to be loaded or not */
  hasMore: PropTypes.bool,
  initialLoad: PropTypes.bool,
  isReverse: PropTypes.bool,
  loader: PropTypes.node,
  loadMore: PropTypes.func.isRequired,
  pageStart: PropTypes.number,
  ref: PropTypes.func,
  threshold: PropTypes.number,
  useCapture: PropTypes.bool,
  useWindow: PropTypes.bool,
  className: PropTypes.string,

  /** The function is called when the list scrolls */
  listenToScroll: PropTypes.func
});

_defineProperty(ReverseInfiniteScroll, "defaultProps", {
  element: 'div',
  hasMore: false,
  initialLoad: true,
  pageStart: 0,
  ref: null,
  threshold: 250,
  useWindow: true,
  isReverse: true,
  useCapture: false,
  loader: null,
  className: 'str-chat__reverse-infinite-scroll'
});

var MessageNotification =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(MessageNotification, _PureComponent);

  function MessageNotification() {
    _classCallCheck(this, MessageNotification);

    return _possibleConstructorReturn(this, _getPrototypeOf(MessageNotification).apply(this, arguments));
  }

  _createClass(MessageNotification, [{
    key: "render",
    value: function render() {
      if (!this.props.showNotification) {
        return null;
      } else {
        return React__default.createElement("button", {
          className: "str-chat__message-notification",
          onClick: this.props.onClick
        }, this.props.children);
      }
    }
  }]);

  return MessageNotification;
}(React.PureComponent);

_defineProperty(MessageNotification, "propTypes", {
  /** If we should show the notification or not */
  showNotification: PropTypes.bool,

  /** Onclick handler */
  onClick: PropTypes.func.isRequired
});

_defineProperty(MessageNotification, "defaultProps", {
  showNotification: true
});

/**
 * DateSeparator - A simple date separator
 *
 * @example ./docs/DateSeparator.md
 * @extends PureComponent
 */

var DateSeparator =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(DateSeparator, _React$PureComponent);

  function DateSeparator() {
    _classCallCheck(this, DateSeparator);

    return _possibleConstructorReturn(this, _getPrototypeOf(DateSeparator).apply(this, arguments));
  }

  _createClass(DateSeparator, [{
    key: "render",
    value: function render() {
      var _this$props = this.props,
          intl = _this$props.intl,
          position = _this$props.position;

      if (!Date.parse(this.props.date)) {
        return null;
      }

      return React__default.createElement("div", {
        className: "str-chat__date-separator"
      }, (position === 'right' || position === 'center') && React__default.createElement("hr", {
        className: "str-chat__date-separator-line"
      }), React__default.createElement("div", {
        className: "str-chat__date-separator-date"
      }, this.props.formatDate ? this.props.formatDate(this.props.date) : moment(this.props.date.toISOString()).calendar(null, {
        nextWeek: intl.formatMessage({
          id: 'date_separator.next_week',
          defaultMessage: 'dddd'
        }),
        nextDay: intl.formatMessage({
          id: 'date_separator.tomorrow',
          defaultMessage: '[Tomorrow]'
        }),
        sameDay: intl.formatMessage({
          id: 'date_separator.today',
          defaultMessage: '[Today]'
        }),
        lastDay: intl.formatMessage({
          id: 'date_separator.yesterday',
          defaultMessage: '[Yesterday]'
        }),
        lastWeek: intl.formatMessage({
          id: 'date_separator.last_week',
          defaultMessage: '[Last] dddd'
        }),
        sameElse: intl.formatMessage({
          id: 'date_separator.else',
          defaultMessage: 'L'
        })
      })), (position === 'left' || position === 'center') && React__default.createElement("hr", {
        className: "str-chat__date-separator-line"
      }));
    }
  }]);

  return DateSeparator;
}(React__default.PureComponent);

_defineProperty(DateSeparator, "propTypes", {
  /** The date to format */
  date: PropTypes.instanceOf(Date),

  /** Set the position of the date in the separator */
  position: PropTypes.oneOf(['left', 'center', 'right']),

  /** Override the default formatting of the date. This is a function that has access to the original date object. Returns a string or Node  */
  formatDate: PropTypes.func
});

_defineProperty(DateSeparator, "defaultProps", {
  position: 'right'
});

DateSeparator = reactIntl.injectIntl(DateSeparator);

exports.EventComponent =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(EventComponent, _React$PureComponent);

  function EventComponent() {
    _classCallCheck(this, EventComponent);

    return _possibleConstructorReturn(this, _getPrototypeOf(EventComponent).apply(this, arguments));
  }

  _createClass(EventComponent, [{
    key: "render",
    value: function render() {
      var _this$props = this.props,
          message = _this$props.message,
          intl = _this$props.intl;

      if (message.type === 'system') {
        return React__default.createElement("div", {
          className: this.props.message.id
        }, React__default.createElement("div", {
          className: "str-chat__message--system"
        }, React__default.createElement("div", {
          className: "str-chat__message--system__text"
        }, React__default.createElement("div", {
          className: "str-chat__message--system__line"
        }), React__default.createElement("p", null, message.text), React__default.createElement("div", {
          className: "str-chat__message--system__line"
        })), React__default.createElement("div", {
          className: "str-chat__message--system__date"
        }, React__default.createElement("strong", null, moment(message.created_at).format('dddd'), " "), React__default.createElement("strong", null, moment(message.created_at).format('L'), " "), intl.formatMessage({
          id: 'event_at_label',
          defaultMessage: 'at'
        }), ' ', moment(message.created_at).format('HH:mm'))));
      }

      if (message.type === 'channel.event' && (message.event.type === 'member.removed' || message.event.type === 'member.added')) {
        var sentence;

        switch (message.event.type) {
          case 'member.removed':
            sentence = intl.formatMessage({
              id: 'event_component.member_removed',
              defaultMessage: '{user} was removed from the chat'
            }, {
              user: message.event.user.name || message.event.user.id
            });
            break;

          case 'member.added':
            sentence = intl.formatMessage({
              id: 'event_component.member_added',
              defaultMessage: '{user} has joined the chat'
            }, {
              user: message.event.user.name || message.event.user.id
            });
            break;

          default:
            break;
        }

        return React__default.createElement("div", {
          className: "str-chat__event-component__channel-event"
        }, React__default.createElement(Avatar, {
          image: message.event.user.image,
          name: message.event.user.name || message.event.user.id
        }), React__default.createElement("div", {
          className: "str-chat__event-component__channel-event__content"
        }, React__default.createElement("em", {
          className: "str-chat__event-component__channel-event__sentence"
        }, sentence), React__default.createElement("div", {
          className: "str-chat__event-component__channel-event__date"
        }, moment(message.created_at).format('HH:mm'))));
      }

      return null;
    }
  }]);

  return EventComponent;
}(React__default.PureComponent);

exports.EventComponent = reactIntl.injectIntl(exports.EventComponent);

/* eslint sonarjs/no-duplicate-string: 0 */

/**
 * MessageList - The message list components renders a list of messages. Its a consumer of [Channel Context](https://getstream.github.io/stream-chat-react/#channel)
 *
 * @example ./docs/MessageList.md
 * @extends PureComponent
 */

exports.MessageList =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(MessageList, _PureComponent);

  function MessageList(props) {
    var _this;

    _classCallCheck(this, MessageList);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(MessageList).call(this, props));

    _defineProperty(_assertThisInitialized(_this), "connectionChanged", function (event) {
      if (_this.state.online !== event.online) {
        _this.setState({
          online: event.online
        });
      }
    });

    _defineProperty(_assertThisInitialized(_this), "keypress", function (event) {
      if (event.keyCode === KEY_CODES.ESC && _this.state.editing) {
        _this.clearEditingState();
      }
    });

    _defineProperty(_assertThisInitialized(_this), "scrollToBottom", function () {
      _this._scrollToRef(_this.bottomRef, _this.messageList);
    });

    _defineProperty(_assertThisInitialized(_this), "_scrollToRef", function (el, parent) {
      function scrollDown() {
        if (el && el.current && parent && parent.current) {
          this.scrollToTarget(el.current, parent.current);
        }
      }

      scrollDown.call(_assertThisInitialized(_this)); // scroll down after images load again

      setTimeout(scrollDown.bind(_assertThisInitialized(_this)), 200);
    });

    _defineProperty(_assertThisInitialized(_this), "scrollToTarget", function (target, containerEl) {
      // Moved up here for readability:
      var isElement = target && target.nodeType === 1,
          isNumber = Object.prototype.toString.call(target) === '[object Number]';

      if (isElement) {
        containerEl.scrollTop = target.offsetTop - 600; // here you can fix the offset problem
      } else if (isNumber) {
        containerEl.scrollTop = target;
      } else if (target === 'bottom') {
        containerEl.scrollTop = containerEl.scrollHeight - containerEl.offsetHeight;
      } else if (target === 'top') {
        containerEl.scrollTop = 0;
      }
    });

    _defineProperty(_assertThisInitialized(_this), "setEditingState", function (message) {
      _this.setState({
        editing: message.id
      });
    });

    _defineProperty(_assertThisInitialized(_this), "clearEditingState", function (e) {
      if (e && e.preventDefault) {
        e.preventDefault();
      }

      _this.setState({
        editing: ''
      });
    });

    _defineProperty(_assertThisInitialized(_this), "insertDates", function (messages) {
      var newMessages = [];
      var _iteratorNormalCompletion = true;
      var _didIteratorError = false;
      var _iteratorError = undefined;

      try {
        for (var _iterator = messages.entries()[Symbol.iterator](), _step; !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
          var _step$value = _slicedToArray(_step.value, 2),
              i = _step$value[0],
              message = _step$value[1];

          if (message.type === 'message.read' || message.deleted_at) {
            newMessages.push(message);
            continue;
          }

          var messageDate = message.created_at.getDay();
          var prevMessageDate = messageDate;

          if (i > 0) {
            prevMessageDate = messages[i - 1].created_at.getDay();
          }

          if (i === 0 || messageDate !== prevMessageDate) {
            newMessages.push({
              type: 'message.date',
              date: message.created_at
            }, message);
          } else {
            newMessages.push(message);
          }

          var eventsNextToMessage = _this.props.eventHistory[message.id || 'first'];

          if (eventsNextToMessage && eventsNextToMessage.length > 0) {
            eventsNextToMessage.forEach(function (e) {
              newMessages.push({
                type: 'channel.event',
                event: e
              });
            });
          }
        }
      } catch (err) {
        _didIteratorError = true;
        _iteratorError = err;
      } finally {
        try {
          if (!_iteratorNormalCompletion && _iterator.return != null) {
            _iterator.return();
          }
        } finally {
          if (_didIteratorError) {
            throw _iteratorError;
          }
        }
      }

      return newMessages;
    });

    _defineProperty(_assertThisInitialized(_this), "insertIntro", function (messages) {
      var newMessages = messages || []; // if no headerPosition is set, HeaderComponent will go at the top

      if (!_this.props.headerPosition) {
        newMessages.unshift({
          type: 'channel.intro' // created_at: new Date(0),

        });
        return newMessages;
      } // if no messages, intro get's inserted


      if (!newMessages.length) {
        newMessages.unshift({
          type: 'channel.intro'
        });
        return newMessages;
      } // else loop over the messages


      var _iteratorNormalCompletion2 = true;
      var _didIteratorError2 = false;
      var _iteratorError2 = undefined;

      try {
        for (var _iterator2 = messages.entries()[Symbol.iterator](), _step2; !(_iteratorNormalCompletion2 = (_step2 = _iterator2.next()).done); _iteratorNormalCompletion2 = true) {
          var _step2$value = _slicedToArray(_step2.value, 2),
              i = _step2$value[0],
              message = _step2$value[1];

          var messageTime = message.created_at ? message.created_at.getTime() : null;
          var nextMessageTime = messages[i + 1] && messages[i + 1].created_at ? messages[i + 1].created_at.getTime() : null;
          var headerPosition = _this.props.headerPosition; // headerposition is smaller than message time so comes after;

          if (messageTime < headerPosition) {
            // if header position is also smaller than message time continue;
            if (nextMessageTime < headerPosition) {
              if (messages[i + 1] && messages[i + 1].type === 'message.date') continue;

              if (!nextMessageTime) {
                newMessages.push({
                  type: 'channel.intro'
                });
                return newMessages;
              }

              continue;
            } else {
              newMessages.splice(i + 1, 0, {
                type: 'channel.intro'
              });
              return newMessages;
            }
          }
        }
      } catch (err) {
        _didIteratorError2 = true;
        _iteratorError2 = err;
      } finally {
        try {
          if (!_iteratorNormalCompletion2 && _iterator2.return != null) {
            _iterator2.return();
          }
        } finally {
          if (_didIteratorError2) {
            throw _iteratorError2;
          }
        }
      }

      return newMessages;
    });

    _defineProperty(_assertThisInitialized(_this), "goToNewMessages",
    /*#__PURE__*/
    _asyncToGenerator(
    /*#__PURE__*/
    _regeneratorRuntime.mark(function _callee() {
      return _regeneratorRuntime.wrap(function _callee$(_context) {
        while (1) {
          switch (_context.prev = _context.next) {
            case 0:
              _context.next = 2;
              return _this.scrollToBottom();

            case 2:
              _this.setState({
                newMessagesNotification: false
              });

            case 3:
            case "end":
              return _context.stop();
          }
        }
      }, _callee);
    })));

    _defineProperty(_assertThisInitialized(_this), "getReadStates", function (messages) {
      // create object with empty array for each message id
      var readData = {};
      var _iteratorNormalCompletion3 = true;
      var _didIteratorError3 = false;
      var _iteratorError3 = undefined;

      try {
        for (var _iterator3 = messages[Symbol.iterator](), _step3; !(_iteratorNormalCompletion3 = (_step3 = _iterator3.next()).done); _iteratorNormalCompletion3 = true) {
          var message = _step3.value;
          readData[message.id] = [];
        }
      } catch (err) {
        _didIteratorError3 = true;
        _iteratorError3 = err;
      } finally {
        try {
          if (!_iteratorNormalCompletion3 && _iterator3.return != null) {
            _iterator3.return();
          }
        } finally {
          if (_didIteratorError3) {
            throw _iteratorError3;
          }
        }
      }

      for (var _i = 0, _Object$values = Object.values(_this.props.read); _i < _Object$values.length; _i++) {
        var readState = _Object$values[_i];

        if (readState.last_read == null) {
          break;
        }

        var userLastReadMsgId = void 0;
        var _iteratorNormalCompletion4 = true;
        var _didIteratorError4 = false;
        var _iteratorError4 = undefined;

        try {
          for (var _iterator4 = messages[Symbol.iterator](), _step4; !(_iteratorNormalCompletion4 = (_step4 = _iterator4.next()).done); _iteratorNormalCompletion4 = true) {
            var msg = _step4.value;

            if (msg.updated_at < readState.last_read) {
              userLastReadMsgId = msg.id;
            }
          }
        } catch (err) {
          _didIteratorError4 = true;
          _iteratorError4 = err;
        } finally {
          try {
            if (!_iteratorNormalCompletion4 && _iterator4.return != null) {
              _iterator4.return();
            }
          } finally {
            if (_didIteratorError4) {
              throw _iteratorError4;
            }
          }
        }

        if (userLastReadMsgId != null) {
          readData[userLastReadMsgId] = [].concat(_toConsumableArray(readData[userLastReadMsgId]), [readState.user]);
        }
      }

      return readData;
    });

    _defineProperty(_assertThisInitialized(_this), "userScrolledUp", function () {
      return _this.scrollOffset > 310;
    });

    _defineProperty(_assertThisInitialized(_this), "listenToScroll", function (offset) {
      _this.scrollOffset = offset;

      if (_this.state.newMessagesNotification && !_this.userScrolledUp()) {
        _this.setState({
          newMessagesNotification: false
        });
      }
    });

    _defineProperty(_assertThisInitialized(_this), "getLastReceived", function (messages) {
      var l = messages.length;
      var lastReceivedId = null;

      for (var i = l; i > 0; i--) {
        if (messages[i] !== undefined && messages[i].status !== undefined && messages[i].status === 'received') {
          lastReceivedId = messages[i].id;
          break;
        }
      }

      return lastReceivedId;
    });

    _defineProperty(_assertThisInitialized(_this), "getGroupStyles", function (m) {
      var l = m.length;
      var messageGroupStyles = {};

      var messages = _toConsumableArray(m);

      for (var i = 0; i < l; i++) {
        var previousMessage = messages[i - 1];
        var message = messages[i];
        var nextMessage = messages[i + 1];
        var groupStyles = [];

        if (message.type === 'message.date') {
          continue;
        }

        if (message.type === 'channel.event') {
          continue;
        }

        if (message.type === 'channel.intro') {
          continue;
        }

        var userId = message.user.id;
        var isTopMessage = !previousMessage || previousMessage.type === 'channel.intro' || previousMessage.type === 'message.date' || previousMessage.type === 'system' || previousMessage.type === 'channel.event' || previousMessage.attachments.length !== 0 || userId !== previousMessage.user.id || previousMessage.type === 'error' || previousMessage.deleted_at;
        var isBottomMessage = !nextMessage || nextMessage.type === 'message.date' || nextMessage.type === 'system' || nextMessage.type === 'channel.event' || nextMessage.type === 'channel.intro' || nextMessage.attachments.length !== 0 || userId !== nextMessage.user.id || nextMessage.type === 'error' || nextMessage.deleted_at;

        if (isTopMessage) {
          groupStyles.push('top');
        }

        if (isBottomMessage) {
          if (isTopMessage || message.deleted_at || message.type === 'error') {
            groupStyles.splice(0, groupStyles.length);
            groupStyles.push('single');
          } else {
            groupStyles.push('bottom');
          }
        }

        if (!isTopMessage && !isBottomMessage) {
          if (message.deleted_at || message.type === 'error') {
            groupStyles.splice(0, groupStyles.length);
            groupStyles.push('single');
          } else {
            groupStyles.splice(0, groupStyles.length);
            groupStyles.push('middle');
          }
        }

        if (message.attachments.length !== 0) {
          groupStyles.splice(0, groupStyles.length);
          groupStyles.push('single');
        }

        if (_this.props.noGroupByUser) {
          groupStyles.splice(0, groupStyles.length);
          groupStyles.push('single');
        }

        messageGroupStyles[message.id] = groupStyles;
      }

      return messageGroupStyles;
    });

    _defineProperty(_assertThisInitialized(_this), "_onMentionsHoverOrClick", function (e, mentioned_users) {
      if (!_this.props.onMentionsHover || !_this.props.onMentionsClick) return;
      var tagName = e.target.tagName.toLowerCase();
      var textContent = e.target.innerHTML.replace('*', '');

      if (tagName === 'strong' && textContent[0] === '@') {
        var userName = textContent.replace('@', '');
        var user = mentioned_users.find(function (user) {
          return user.name === userName || user.id === userName;
        });

        if (_this.props.onMentionsHover && e.type === 'mouseover') {
          _this.props.onMentionsHover(e, user);
        }

        if (_this.props.onMentionsClick && e.type === 'click') {
          _this.props.onMentionsHover(e, user);
        }
      }
    });

    _defineProperty(_assertThisInitialized(_this), "addNotification", function (notificationText, type) {
      if (typeof notificationText !== 'string') return;
      if (type !== 'success' && type !== 'error') return;
      var nextIndex = new Date();

      var newNotifications = _toConsumableArray(_this.state.notifications);

      newNotifications.push({
        id: nextIndex,
        text: notificationText,
        type: type
      });

      _this.setState({
        notifications: newNotifications
      }); // remove the notification after 5000 ms


      var ct = setTimeout(function () {
        var index = _this.state.notifications.findIndex(function (notification) {
          if (notification.id === nextIndex) return true;
          return false;
        });

        var newNotifications = _toConsumableArray(_this.state.notifications);

        newNotifications.splice(index, 1);

        _this.setState({
          notifications: newNotifications
        });
      }, 5000);

      _this.notificationTimeouts.push(ct);
    });

    _defineProperty(_assertThisInitialized(_this), "_loadMore", function () {
      return _this.props.messageLimit ? _this.props.loadMore(_this.props.messageLimit) : _this.props.loadMore();
    });

    _defineProperty(_assertThisInitialized(_this), "hide", {
      display: 'none'
    });

    _defineProperty(_assertThisInitialized(_this), "SearchBar", function () {
      var intl = _this.props.intl;
      return React__default.createElement("div", {
        id: "style-search-bar"
      }, React__default.createElement("div", {
        className: "str-chat__channel-search",
        id: "style-channel-search"
      }, React__default.createElement("div", {
        id: "style-channel-search-input"
      }, React__default.createElement("input", {
        id: "searchInput",
        className: "searchInput",
        placeholder: intl.formatMessage({
          id: 'channel_search.placeholder',
          defaultMessage: 'Search'
        }),
        value: _this.state.searchInput || '',
        type: "text",
        name: "inputSearch",
        onChange: _this.handleChange,
        onKeyPress: _this.handleSearch.bind(_assertThisInitialized(_this)),
        onKeyDown: _this.handleSearch.bind(_assertThisInitialized(_this))
      }))));
    });

    _defineProperty(_assertThisInitialized(_this), "toggleSearch", function () {
      if (!_this.state.open && _this.state.searchInput) {
        _this.setState({
          open: true
        });

        _this.search(_this.state.searchInput, _this.props.channel);
      } else {
        if (!_this.messageRefs[_this.state.activeSearchResultId]) {
          _this.setState(function (state) {
            return {
              open: false,
              searchInput: null,
              activeSearchResultId: undefined,
              searchResultId: []
            };
          });
        } else {
          if (_this.state.activeSearchResultId !== undefined) {
            if (_this.messageRefs[_this.state.activeSearchResultId].current !== null) {
              _this.messageRefs[_this.state.activeSearchResultId].current.classList.remove('search-result');
            }
          }

          _this.setState(function () {
            return {
              open: false,
              searchInput: null,
              activeSearchResultId: undefined,
              searchResultId: []
            };
          });
        }

        _this.scrollToBottom();
      }
    });

    _defineProperty(_assertThisInitialized(_this), "Button", function (_ref2) {
      var open = _ref2.open,
          onClick = _ref2.onClick;
      return React__default.createElement("div", {
        onClick: onClick,
        style: {
          display: 'flex'
        }
      }, open ? React__default.createElement("div", null, React__default.createElement("svg", {
        id: "style-channel-search-icon",
        width: "18",
        height: "18",
        viewBox: "0 0 18 18",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        stroke: "black",
        d: "M 19.333 2.547 l -1.88 -1.88 L 10 8.12 L 2.547 0.667 l -1.88 1.88 L 8.12 10 L 0.667 17.453 l 1.88 1.88 L 10 11.88 l 7.453 7.453 l 1.88 -1.88 L 11.88 10 Z"
      }))) : React__default.createElement("div", null, React__default.createElement("svg", {
        id: "style-channel-search-icon",
        width: "18",
        height: "17",
        viewBox: "0 0 18 17",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        stroke: "black",
        d: "M18.125,15.804l-4.038-4.037c0.675-1.079,1.012-2.308,1.01-3.534C15.089,4.62,12.199,1.75,8.584,1.75C4.815,1.75,1.982,4.726,2,8.286c0.021,3.577,2.908,6.549,6.578,6.549c1.241,0,2.417-0.347,3.44-0.985l4.032,4.026c0.167,0.166,0.43,0.166,0.596,0l1.479-1.478C18.292,16.234,18.292,15.968,18.125,15.804 M8.578,13.99c-3.198,0-5.716-2.593-5.733-5.71c-0.017-3.084,2.438-5.686,5.74-5.686c3.197,0,5.625,2.493,5.64,5.624C14.242,11.548,11.621,13.99,8.578,13.99 M16.349,16.981l-3.637-3.635c0.131-0.11,0.721-0.695,0.876-0.884l3.642,3.639L16.349,16.981z"
      }))));
    });

    _this.state = {
      newMessagesNotification: false,
      editing: '',
      online: true,
      notifications: [],
      searchInput: '',
      searchResultId: [],
      activeSearchResultCount: -1,
      activeSearchResultId: undefined,
      open: false,
      updatedComponent: false
    };
    _this.handleChange = _this.handleChange.bind(_assertThisInitialized(_this));
    _this.bottomRef = React__default.createRef();
    _this.messageList = React__default.createRef();
    _this.messageRefs = {};
    _this.notificationTimeouts = [];
    return _this;
  }

  _createClass(MessageList, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      window.addEventListener(languageChangedEventName, this.onLanguageChanged);
      var messageListRect = this.messageList.current.getBoundingClientRect();
      this.setState({
        messageListRect: messageListRect
      });

      if (this.props.search) {
        this.setState({
          searchInput: this.props.search,
          open: true,
          updatedComponent: false
        }, function () {});
        this.search(this.props.search, this.props.channel);
      } else {
        this.scrollToBottom();
      }

      this.props.client.on('connection.changed', this.connectionChanged);
      document.addEventListener('keydown', this.keypress);
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      this.props.client.off('connection.changed', this.connectionChanged);
      document.removeEventListener('keydown', this.keypress);
      this.notificationTimeouts.forEach(function (ct) {
        clearTimeout(ct);
      });
    }
  }, {
    key: "getSnapshotBeforeUpdate",
    value: function getSnapshotBeforeUpdate(prevProps) {
      if (this.props.threadList) {
        return null;
      } // Are we adding new items to the list?
      // Capture the scroll position so we can adjust scroll later.


      if (prevProps.messages.length < this.props.messages.length || !deepequal(this.props.eventHistory, prevProps.eventHistory)) {
        var list = this.messageList.current;
        return {
          offsetTop: list.scrollTop,
          offsetBottom: list.scrollHeight - list.scrollTop
        };
      }

      return null;
    }
  }, {
    key: "componentDidUpdate",
    value: function componentDidUpdate(prevProps, prevState, snapshot) {
      // If we have a snapshot value, we've just added new items.
      // Adjust scroll so these new items don't push the old ones out of view.
      // (snapshot here is the value returned from getSnapshotBeforeUpdate)
      var userScrolledUp = this.userScrolledUp();
      var currentLastMessage = this.props.messages[this.props.messages.length - 1];
      var previousLastMessage = prevProps.messages[prevProps.messages.length - 1];

      if (!previousLastMessage || !currentLastMessage) {
        return;
      }

      var hasNewMessage = currentLastMessage.id !== previousLastMessage.id;
      var isOwner = currentLastMessage.user.id === this.props.client.userID;
      var list = this.messageList.current; // always scroll down when it's your own message that you added...

      var scrollToBottom = hasNewMessage && (isOwner || !userScrolledUp);

      if (this.state.searchInput === '') {
        if (scrollToBottom) {
          this.scrollToBottom(); // Scroll further once attachments are laoded.

          setTimeout(this.scrollToBottom, 100); // remove the scroll notification if we already scrolled down...

          this.state.newMessagesNotification && this.setState({
            newMessagesNotification: false
          });
          return;
        }

        if (snapshot !== null) {
          // Maintain the offsetTop of scroll so that content in viewport doesn't move.
          // This is for the case where user has scroll up significantly and a new message arrives from someone.
          if (hasNewMessage) {
            this.scrollToTarget(snapshot.offsetTop, this.messageList.current);
          } else {
            // Maintain the bottomOffset of scroll.
            // This is for the case of pagination, when more messages get loaded.
            this.scrollToTarget(list.scrollHeight - snapshot.offsetBottom, this.messageList.current);
          }
        }
      }

      if (!this.state.activeSearchResultId && !this.state.updatedComponent) {
        this.search(this.state.searchInput, this.props.channel);
        this.setState({
          updatedComponent: true
        });
      }

      if (this.state.activeSearchResultId) {
        this.messageRefs[this.state.activeSearchResultId].current.classList.add('search-result');

        this._scrollToRef(this.messageRefs[this.state.activeSearchResultId], this.messageList);
      } // Check the scroll position... if you're scrolled up show a little notification


      if (hasNewMessage && !this.state.newMessagesNotification) {
        this.setState({
          newMessagesNotification: true
        });
      }
    }
  }, {
    key: "render",
    // eslint-disable-next-line
    value: function render() {
      var _this2 = this;

      var allMessages = _toConsumableArray(this.props.messages);

      var MessageSystem = this.props.MessageSystem;
      allMessages = this.insertDates(allMessages);

      if (this.props.HeaderComponent) {
        allMessages = this.insertIntro(allMessages);
      }

      var messageGroupStyles = this.getGroupStyles(allMessages);
      var _this$props = this.props,
          TypingIndicator = _this$props.TypingIndicator,
          DateSeparator$$1 = _this$props.dateSeparator,
          HeaderComponent = _this$props.HeaderComponent,
          EmptyStateIndicator$$1 = _this$props.EmptyStateIndicator; // sort by date

      allMessages.sort(function (a, b) {
        return a.created_at - b.created_at;
      }); // get the readData

      var readData = this.getReadStates(allMessages);
      var lastReceivedId = this.getLastReceived(allMessages);
      var elements = []; // let sytemMessageRefID = 0;
      // loop over the messages

      var _iteratorNormalCompletion5 = true;
      var _didIteratorError5 = false;
      var _iteratorError5 = undefined;

      try {
        for (var _iterator5 = allMessages[Symbol.iterator](), _step5; !(_iteratorNormalCompletion5 = (_step5 = _iterator5.next()).done); _iteratorNormalCompletion5 = true) {
          var message = _step5.value;

          if (message.id) {
            this.messageRefs[message.id] = React__default.createRef();
          } //  else {
          //   sytemMessageRefID = sytemMessageRefID + 1;
          //   this.messageRefs[sytemMessageRefID] = React.createRef();
          // }


          if (message.type === 'message.date') {
            if (this.props.threadList) {
              continue;
            }

            elements.push(React__default.createElement("li", {
              key: message.date.toISOString() + '-i'
            }, React__default.createElement(DateSeparator$$1, {
              date: message.date
            })));
          } else if (message.type === 'channel.intro') {
            elements.push(React__default.createElement("li", {
              key: "intro"
            }, React__default.createElement(HeaderComponent, null)));
          } else if (message.type === 'channel.event' || message.type === 'system') {
            MessageSystem && elements.push(React__default.createElement("li", {
              className: "systemmessage",
              key: message.id || message.type === 'system' ? message.created_at : message.type === 'channel.event' ? message.event.created_at : '',
              ref: this.messageRefs[message.id]
            }, React__default.createElement(MessageSystem, {
              message: message
            })));
          } else if (message.type !== 'message.read') {
            var groupStyles = messageGroupStyles[message.id];

            if (!groupStyles) {
              groupStyles = [];
            }

            var readBy = readData[message.id] || [];
            elements.push(React__default.createElement("li", {
              className: "str-chat__li str-chat__li--".concat(groupStyles),
              key: message.id || message.created_at,
              ref: this.messageRefs[message.id]
            }, React__default.createElement(exports.Message, {
              client: this.props.client,
              openThread: this.props.openThread,
              members: this.props.members,
              watchers: this.props.watchers,
              message: message,
              groupStyles: groupStyles,
              readBy: readBy,
              lastReceivedId: lastReceivedId === message.id ? lastReceivedId : null,
              editing: !!(this.state.editing && this.state.editing === message.id),
              clearEditingState: this.clearEditingState,
              setEditingState: this.setEditingState,
              messageListRect: this.state.messageListRect,
              channel: this.props.channel,
              threadList: this.props.threadList,
              retrySendMessage: this.props.retrySendMessage,
              addNotification: this.addNotification,
              updateMessage: this.props.updateMessage,
              removeMessage: this.props.removeMessage,
              Message: this.props.Message,
              unsafeHTML: this.props.unsafeHTML,
              Attachment: this.props.Attachment,
              onMentionsClick: this.props.onMentionsClick,
              onMentionsHover: this.props.onMentionsHover,
              messageActions: this.props.messageActions,
              additionalMessageInputProps: this.props.additionalMessageInputProps,
              getFlagMessageSuccessNotification: this.props.getFlagMessageSuccessNotification,
              getFlagMessageErrorNotification: this.props.getFlagMessageErrorNotification,
              getMuteUserSuccessNotification: this.props.getMuteUserSuccessNotification,
              getMuteUserErrorNotification: this.props.getMuteUserErrorNotification
            })));
          }
        }
      } catch (err) {
        _didIteratorError5 = true;
        _iteratorError5 = err;
      } finally {
        try {
          if (!_iteratorNormalCompletion5 && _iterator5.return != null) {
            _iterator5.return();
          }
        } finally {
          if (_didIteratorError5) {
            throw _iteratorError5;
          }
        }
      }

      return React__default.createElement(React__default.Fragment, null, React__default.createElement("div", {
        style: {
          background: 'white'
        }
      }, React__default.createElement("div", {
        className: 'str-chat__header'
      }, React__default.createElement("div", {
        id: "search"
      }, React__default.createElement(this.SearchBar, null)), React__default.createElement(this.Button, {
        onClick: this.toggleSearch,
        open: this.state.open
      }), this.state.open ? React__default.createElement("div", {
        id: "style-lastResult-button-right"
      }, React__default.createElement("button", {
        disabled: this.state.activeSearchResultCount >= this.state.searchResultId.length - 1 || this.state.open === false,
        onClick: this.nextSearchResult.bind(this),
        id: "style-lastResult-button-left"
      }, React__default.createElement("svg", {
        id: "style-channel-search-arrows",
        className: "search-arrow-left",
        width: "18",
        height: "18",
        viewBox: "0 0 448 512",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        stroke: "black",
        d: "M413.1 222.5l22.2 22.2c9.4 9.4 9.4 24.6 0 33.9L241 473c-9.4 9.4-24.6 9.4-33.9 0L12.7 278.6c-9.4-9.4-9.4-24.6 0-33.9l22.2-22.2c9.5-9.5 25-9.3 34.3.4L184 343.4V56c0-13.3 10.7-24 24-24h32c13.3 0 24 10.7 24 24v287.4l114.8-120.5c9.3-9.8 24.8-10 34.3-.4z"
      }))), React__default.createElement("button", {
        disabled: this.state.activeSearchResultCount <= 0 || this.state.open === false,
        onClick: this.previousSearchResult.bind(this),
        id: "style-lastResult-button-right"
      }, React__default.createElement("svg", {
        id: "style-channel-search-arrows",
        className: "search-arrow-right",
        width: "18",
        height: "18",
        viewBox: "0 0 448 512",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        stroke: "black",
        d: "M34.9 289.5l-22.2-22.2c-9.4-9.4-9.4-24.6 0-33.9L207 39c9.4-9.4 24.6-9.4 33.9 0l194.3 194.3c9.4 9.4 9.4 24.6 0 33.9L413 289.4c-9.5 9.5-25 9.3-34.3-.4L264 168.6V456c0 13.3-10.7 24-24 24h-32c-13.3 0-24-10.7-24-24V168.6L69.2 289.1c-9.3 9.8-24.8 10-34.3.4z"
      })))) : React__default.createElement(React__default.Fragment, null))), React__default.createElement("div", {
        className: "str-chat__list ".concat(this.props.threadList ? 'str-chat__list--thread' : ''),
        ref: this.messageList
      }, !elements.length ? React__default.createElement(EmptyStateIndicator$$1, {
        listType: "message"
      }) : React__default.createElement(ReverseInfiniteScroll, {
        loadMore: this._loadMore,
        hasMore: this.props.hasMore,
        isLoading: this.props.loadingMore,
        listenToScroll: this.listenToScroll,
        useWindow: false,
        loader: React__default.createElement(Center, {
          key: "loadingindicator"
        }, React__default.createElement(LoadingIndicator, {
          size: 20
        }))
      }, React__default.createElement("ul", {
        className: "str-chat__ul"
      }, elements), this.props.TypingIndicator && React__default.createElement(TypingIndicator, {
        typing: this.props.typing,
        client: this.props.client
      }), React__default.createElement("div", {
        key: "bottom",
        ref: this.bottomRef
      }))), React__default.createElement("div", {
        className: "str-chat__list-notifications"
      }, this.state.notifications.map(function (notification) {
        return React__default.createElement(Notification, {
          active: true,
          key: notification.id,
          type: notification.type
        }, notification.text);
      }), React__default.createElement(Notification, {
        active: !this.state.online,
        type: "error"
      }, "Connection failure, reconnecting now..."), React__default.createElement(MessageNotification, {
        showNotification: this.state.newMessagesNotification,
        onClick: function onClick() {
          return _this2.goToNewMessages();
        }
      }, "New Messages!")));
    }
  }, {
    key: "nextSearchResult",
    value: function () {
      var _nextSearchResult = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee2() {
        return _regeneratorRuntime.wrap(function _callee2$(_context2) {
          while (1) {
            switch (_context2.prev = _context2.next) {
              case 0:
                if (!(this.state.activeSearchResultCount < this.state.searchResultId.length - 1)) {
                  _context2.next = 8;
                  break;
                }

                _context2.next = 3;
                return this.setState({
                  activeSearchResultCount: this.state.activeSearchResultCount + 1
                }, function () {});

              case 3:
                _context2.next = 5;
                return this.setState({
                  activeSearchResultId: this.state.searchResultId[this.state.activeSearchResultCount]
                }, function () {});

              case 5:
                this._scrollToRef(this.messageRefs[this.state.activeSearchResultId], this.messageList);

                if (this.messageRefs[this.state.searchResultId[this.state.activeSearchResultCount - 1]].current !== null) {
                  this.messageRefs[this.state.searchResultId[this.state.activeSearchResultCount - 1]].current.classList.remove('search-result');
                }

                this.messageRefs[this.state.activeSearchResultId].current.classList.add('search-result');

              case 8:
              case "end":
                return _context2.stop();
            }
          }
        }, _callee2, this);
      }));

      function nextSearchResult() {
        return _nextSearchResult.apply(this, arguments);
      }

      return nextSearchResult;
    }()
  }, {
    key: "previousSearchResult",
    value: function () {
      var _previousSearchResult = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee3() {
        return _regeneratorRuntime.wrap(function _callee3$(_context3) {
          while (1) {
            switch (_context3.prev = _context3.next) {
              case 0:
                if (!(this.state.activeSearchResultCount > 0)) {
                  _context3.next = 8;
                  break;
                }

                _context3.next = 3;
                return this.setState({
                  activeSearchResultCount: this.state.activeSearchResultCount - 1
                });

              case 3:
                _context3.next = 5;
                return this.setState({
                  activeSearchResultId: this.state.searchResultId[this.state.activeSearchResultCount]
                }, function () {});

              case 5:
                this._scrollToRef(this.messageRefs[this.state.activeSearchResultId], this.messageList);

                if (this.messageRefs[this.state.searchResultId[this.state.activeSearchResultCount + 1]].current !== null) {
                  this.messageRefs[this.state.searchResultId[this.state.activeSearchResultCount + 1]].current.classList.remove('search-result');
                }

                this.messageRefs[this.state.activeSearchResultId].current.classList.add('search-result');

              case 8:
              case "end":
                return _context3.stop();
            }
          }
        }, _callee3, this);
      }));

      function previousSearchResult() {
        return _previousSearchResult.apply(this, arguments);
      }

      return previousSearchResult;
    }()
  }, {
    key: "handleChange",
    value: function () {
      var _handleChange = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee4(event) {
        return _regeneratorRuntime.wrap(function _callee4$(_context4) {
          while (1) {
            switch (_context4.prev = _context4.next) {
              case 0:
                this.cleanSearchResult();
                _context4.next = 3;
                return this.setState({
                  searchInput: event.target.value
                }, function () {});

              case 3:
              case "end":
                return _context4.stop();
            }
          }
        }, _callee4, this);
      }));

      function handleChange(_x) {
        return _handleChange.apply(this, arguments);
      }

      return handleChange;
    }()
  }, {
    key: "cleanSearchResult",
    value: function () {
      var _cleanSearchResult = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee5() {
        var ref;
        return _regeneratorRuntime.wrap(function _callee5$(_context5) {
          while (1) {
            switch (_context5.prev = _context5.next) {
              case 0:
                _context5.next = 2;
                return this.setState({
                  open: false,
                  activeSearchResultCount: undefined,
                  activeSearchResultId: undefined
                });

              case 2:
                for (ref in this.messageRefs) {
                  if (this.messageRefs[ref].current !== null) {
                    this.messageRefs[ref].current.classList.remove('search-result');
                  }
                }

              case 3:
              case "end":
                return _context5.stop();
            }
          }
        }, _callee5, this);
      }));

      function cleanSearchResult() {
        return _cleanSearchResult.apply(this, arguments);
      }

      return cleanSearchResult;
    }()
  }, {
    key: "handleSearch",
    value: function handleSearch(event) {
      this.cleanSearchResult();
      this.setState({
        activeSearchResultCount: undefined | null,
        open: true
      });
      var code = event.keyCode || event.which;

      if (code === 13) {
        this.setState({
          open: true
        });
        this.search(this.state.searchInput, this.props.channel);
      }
    }
  }, {
    key: "search",
    value: function () {
      var _search = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee6(searchValue, channel) {
        var language, resultIdArray;
        return _regeneratorRuntime.wrap(function _callee6$(_context6) {
          while (1) {
            switch (_context6.prev = _context6.next) {
              case 0:
                if (!(searchValue === '' | null | undefined)) {
                  _context6.next = 2;
                  break;
                }

                return _context6.abrupt("return");

              case 2:
                language = window.dplChatConfig.language;

                if (!(this.state.searchInput === '' | null | undefined)) {
                  _context6.next = 5;
                  break;
                }

                return _context6.abrupt("return");

              case 5:
                _context6.next = 7;
                return channel.state.messages.filter(function (element) {
                  if (!language && searchValue) {
                    return element.text.toLowerCase().includes(searchValue.toLowerCase());
                  }

                  if (!element.translations) {
                    return false;
                  }

                  var text = element.translations[language];

                  if (searchValue) {
                    if (!text.toLowerCase().includes(searchValue.toLowerCase())) {
                      return false;
                    }

                    return true;
                  }

                  return false;
                }).map(function (element) {
                  return element.id;
                });

              case 7:
                resultIdArray = _context6.sent;
                _context6.next = 10;
                return this.setState({
                  searchResultId: resultIdArray
                }, function () {});

              case 10:
                if (resultIdArray.length < 1) {
                  this.setState({
                    resultIdArray: undefined,
                    activeSearchResultCount: undefined
                  });
                  this.scrollToBottom();
                }

                _context6.next = 13;
                return this.setState({
                  activeSearchResultId: resultIdArray[resultIdArray.length - 1],
                  activeSearchResultCount: resultIdArray.length - 1
                }, function () {});

              case 13:
              case "end":
                return _context6.stop();
            }
          }
        }, _callee6, this);
      }));

      function search(_x2, _x3) {
        return _search.apply(this, arguments);
      }

      return search;
    }()
  }]);

  return MessageList;
}(React.PureComponent);

_defineProperty(exports.MessageList, "propTypes", {
  /**
   * Typing indicator UI component to render
   *
   * Defaults to and accepts same props as: [TypingIndicator](https://github.com/GetStream/stream-chat-react/blob/master/src/components/TypingIndicator.js)
   * */
  TypingIndicator: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   * Date separator UI component to render
   *
   * Defaults to and accepts same props as: [DateSeparator](https://github.com/GetStream/stream-chat-react/blob/master/src/components/DateSeparator.js)
   * */
  dateSeparator: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /** Turn off grouping of messages by user */
  noGroupByUser: PropTypes.bool,

  /** render HTML instead of markdown. Posting HTML is only allowed server-side */
  unsafeHTML: PropTypes.bool,

  /** Set the limit to use when paginating messages */
  messageLimit: PropTypes.number,

  /**
   * Array of allowed actions on message. e.g. ['edit', 'delete', 'mute', 'flag']
   * If all the actions need to be disabled, empty array or false should be provided as value of prop.
   * */
  messageActions: PropTypes.oneOfType([PropTypes.bool, PropTypes.array]),

  /**
   * Boolean weather current message list is a thread.
   */
  threadList: PropTypes.bool,

  /**
   * Function that returns message/text as string to be shown as notification, when request for flagging a message is successful
   *
   * This function should accept following params:
   *
   * @param message A [message object](https://getstream.io/chat/docs/#message_format) which is flagged.
   *
   * */
  getFlagMessageSuccessNotification: PropTypes.func,

  /**
   * Function that returns message/text as string to be shown as notification, when request for flagging a message runs into error
   *
   * This function should accept following params:
   *
   * @param message A [message object](https://getstream.io/chat/docs/#message_format) which is flagged.
   *
   * */
  getFlagMessageErrorNotification: PropTypes.func,

  /**
   * Function that returns message/text as string to be shown as notification, when request for muting a user is successful
   *
   * This function should accept following params:
   *
   * @param user A user object which is being muted
   *
   * */
  getMuteUserSuccessNotification: PropTypes.func,

  /**
   * Function that returns message/text as string to be shown as notification, when request for muting a user runs into error
   *
   * This function should accept following params:
   *
   * @param user A user object which is being muted
   *
   * */
  getMuteUserErrorNotification: PropTypes.func,

  /** **Available from [chat context](https://getstream.github.io/stream-chat-react/#chat)** */
  client: PropTypes.object,

  /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)** */
  Attachment: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)** */
  Message: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   * Custom UI component to display system messages.
   *
   * Defaults to and accepts same props as: [EventComponent](https://github.com/GetStream/stream-chat-react/blob/master/src/components/EventComponent.js)
   */
  MessageSystem: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   * The UI Indicator to use when MessagerList or ChannelList is empty
   * */
  EmptyStateIndicator: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   * Component to render at the top of the MessageList
   * */
  HeaderComponent: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)** */
  messages: PropTypes.array.isRequired,

  /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)** */
  channel: PropTypes.object.isRequired,

  /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)** */
  updateMessage: PropTypes.func.isRequired,

  /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)** */
  retrySendMessage: PropTypes.func,

  /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)** */
  removeMessage: PropTypes.func,

  /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)** */
  onMentionsClick: PropTypes.func,

  /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)** */
  onMentionsHover: PropTypes.func,

  /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)** */
  openThread: PropTypes.func,

  /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)** */
  members: PropTypes.object,

  /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)** */
  watchers: PropTypes.object,

  /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)** */
  read: PropTypes.object,

  /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)** */
  typing: PropTypes.object,

  /**
   * Additional props for underlying MessageInput component.
   * Available props - https://getstream.github.io/stream-chat-react/#messageinput
   * */
  additionalMessageInputProps: PropTypes.object
});

_defineProperty(exports.MessageList, "defaultProps", {
  Message: exports.MessageSimple,
  MessageSystem: exports.EventComponent,
  threadList: false,
  Attachment: Attachment,
  dateSeparator: DateSeparator,
  EmptyStateIndicator: EmptyStateIndicator,
  unsafeHTML: false,
  noGroupByUser: false,
  messageActions: Object.keys(MESSAGE_ACTIONS)
});

exports.MessageList = withChannelContext(reactIntl.injectIntl(exports.MessageList));

var Center = function Center(_ref3) {
  var children = _ref3.children;
  return React__default.createElement("div", {
    className: "str-chat__list__center"
  }, children);
};

var Notification = function Notification(_ref4) {
  var children = _ref4.children,
      active = _ref4.active,
      type = _ref4.type;

  if (active) {
    return React__default.createElement("div", {
      className: "str-chat__custom-notification notification-".concat(type)
    }, children);
  }

  return null;
};

var messages_en = {
	"channel_filter.accounting-record": "accounting record",
	"channel_filter.demand": "demand",
	"channel_filter.empty": "no filter",
	"channel_filter.supply": "supply",
	"channel_filter.voucher": "voucher",
	"channel_header.live": "live",
	"channel_header.members": "{count} members",
	"channel_header.watchers": "{count} online",
	"channel_preview.latest_message.attachment": "Attachment...",
	"channel_preview.latest_message.command": "/{command}",
	"channel_preview.latest_message.deleted": "Message deleted...",
	"channel_preview.latest_message.empty": "Empty message...",
	"channel_preview.latest_message.none": "Nothing yet...",
	"channel_search.placeholder": "Search...",
	"channel_sort.last": "newest",
	"channel_sort.old": "oldest",
	"chat_down.connection_error": "Connection Error",
	"chat_down.connection_error_text": "Error connecting to chat, refresh the page to try again.",
	"command.error": "<strong>/{command}</strong> is not a valid command",
	"date_separator.else": "L",
	"date_separator.last_week": "[Last] dddd",
	"date_separator.next_week": "dddd",
	"date_separator.today": "[Today]",
	"date_separator.tomorrow": "[Tomorrow]",
	"date_separator.yesterday": "[Yesterday]",
	"edit_message.cancel": "Cancel",
	"edit_message.submit": "Send",
	"event_component.member_added": "{user} has joined the chat",
	"event_component.member_removed": "{user} was removed from the chat",
	"message.deleted": "This message was deleted...",
	"message.error": "Error · Unsent",
	"message.failed": "Message Failed · Click to try again",
	"message.flag.error": "Error adding flag: Either the flag already exist or there is issue with network connection ...",
	"message.flag.success": "Message has been successfully flagged",
	"message.mute.error": "Error muting a user ...",
	"message.mute.success": "User with id {id} has been muted",
	"message.status.delivered": "Delivered",
	"message.status.sending": "Sending...",
	"message.title.message_actions": "Message actions",
	"message.title.open_thread": "Start a thread",
	"message.title.reactions": "Reactions",
	"message_actions.delete": "Delete",
	"message_actions.edit": "Edit Message",
	"message_actions.flag": "Flag",
	"message_actions.mute": "Mute",
	"message_input.and": "and",
	"message_input.placeholder": "Type your message",
	"message_input.typing": "{count, plural, one {is typing...} other {are typing...}}",
	"message_input.watching": "{count} online",
	"message_replies.count_button": "{count, plural, one {# reply} other {# replies}}",
	"message_status.and": "and",
	"message_status.more": "and {count} more",
	"modal.close": "Close"
};

var event_at_label = " um ";
var messages_de = {
	event_at_label: event_at_label,
	"channel_filter.accounting-record": "Buchung",
	"channel_filter.demand": "Bedarf",
	"channel_filter.empty": "Nachrichten(Alle)",
	"channel_filter.supply": "Verfügbarkeit",
	"channel_filter.voucher": "Gutschrift",
	"channel_header.live": "live",
	"channel_header.members": "{count} Mitglieder",
	"channel_header.watchers": "{count} online",
	"channel_preview.latest_message.attachment": "Anhang...",
	"channel_preview.latest_message.command": "/{command}",
	"channel_preview.latest_message.deleted": "Nachricht gelöscht...",
	"channel_preview.latest_message.empty": "Leere Nachricht...",
	"channel_preview.latest_message.none": "Bis jetzt noch nichts...",
	"channel_search.placeholder": "Suche...",
	"channel_sort.last": "neuste",
	"channel_sort.old": "älteste",
	"channel_filter.my": "Meine",
	"channel_filter.all": "Alle",
	"channel_filter.me": "Bearbeiter: Ich",
	"channel_filter.no": "Bearbeiter: Kein",
	"channel_filter.other": "Bearbeiter: Andere",
	"channel_preview.responsiveMember.none": "Noch nicht in Bearbeitung",
	"channel_preview.responsiveMember.label": "In Bearbeitung von",
	"channel_filter.responsiveMember.assignMe": "Übernehme Bearbeitung",
	"chat_down.connection_error": "Verbindungs Fehler",
	"chat_down.connection_error_text": "Fehler beim Verbinden zum Chat, erneuere die Site um es nochmal zu versuchen.",
	"command.error": "<strong>/{command}</strong> kein erlaubter Befehl",
	"date_separator.else": "L",
	"date_separator.last_week": "[letzten] dddd",
	"date_separator.next_week": "dddd",
	"date_separator.today": "[heute]",
	"date_separator.tomorrow": "[morgen]",
	"date_separator.yesterday": "[gestern]",
	"edit_message.cancel": "Abgebrochen",
	"edit_message.submit": "Senden",
	"event_component.member_added": "{user} ist dem Chat beigetretten",
	"event_component.member_removed": "{user} hat den Chat verlassen",
	"message.deleted": "Diese Nachricht wurde gelöscht...",
	"message.error": "Fehler · Nicht gesendet",
	"message.failed": "Nachricht gescheitert · Klicken um es nochmal zu versuchen",
	"message.flag.error": "Fehler beim markieren: Wahrscheinlich gibt es die Markierung schon oder es besteht ein Problem mit der Internet verbindung ...",
	"message.flag.success": "Die Nachricht wurde erfolgreich markiert",
	"message.mute.error": "Fehler beim Stummschalten des Teilnehmers ...",
	"message.mute.success": "Teilnehmer mit der Nummer {id} wurde Stumm geschaltet",
	"message.status.delivered": "Angekommen",
	"message.status.sending": "am Senden...",
	"message.title.message_actions": "Nachrichten Aktionen",
	"message.title.open_thread": "Starten eines Threads",
	"message.title.reactions": "Reactionen",
	"message_actions.delete": "Löschen",
	"message_actions.edit": "Bearbeiten einer Nachricht",
	"message_actions.flag": "Markierung",
	"message_actions.mute": "Stumm",
	"message_input.and": "und",
	"message_input.placeholder": "Tippe deine Nachricht",
	"message_input.typing": "{count, plural, one {am Tippen...} other {sind am Tippen...}}",
	"message_input.watching": "{count} online",
	"message_replies.count_button": "{count, plural, one {# reply} other {# replies}}",
	"message_status.and": "und",
	"message_status.more": "und {count} mehr",
	"modal.close": "Geschlossen"
};

var messages_es = {
	"channel_filter.accounting-record": "registro de contabilidad",
	"channel_filter.demand": "demanda",
	"channel_filter.empty": "sin filtro",
	"channel_filter.supply": "suministro",
	"channel_filter.voucher": "Vale",
	"channel_header.live": "VIVIR",
	"channel_header.members": "{count} miembros",
	"channel_header.watchers": "{count} en línea",
	"channel_preview.latest_message.attachment": "Adjunto archivo...",
	"channel_preview.latest_message.command": "/{command}",
	"channel_preview.latest_message.deleted": "Mensaje borrado...",
	"channel_preview.latest_message.empty": "Mensaje vacío ...",
	"channel_preview.latest_message.none": "Nada aún...",
	"channel_search.placeholder": "Buscar...",
	"channel_sort.last": "El más nuevo",
	"channel_sort.old": "Más antiguo",
	"chat_down.connection_error": "error de conexión",
	"chat_down.connection_error_text": "Error al conectarse al chat, actualice la página para volver a intentarlo.",
	"command.error": "<strong>/{command}</strong> no es un comando válido",
	"date_separator.else": "L",
	"date_separator.last_week": "[Último] dddd",
	"date_separator.next_week": "dddd",
	"date_separator.today": "[Hoy]",
	"date_separator.tomorrow": "[Mañana]",
	"date_separator.yesterday": "[Ayer]",
	"edit_message.cancel": "Cancelar",
	"edit_message.submit": "Enviar",
	"event_component.member_added": "{user} se ha unido al chat",
	"event_component.member_removed": "{user} se eliminó del chat",
	"message.deleted": "Este mensaje fue eliminado ...",
	"message.error": "Error · No enviado",
	"message.failed": "Mensaje fallido · Haga clic para intentarlo de nuevo",
	"message.flag.error": "Error al agregar indicador: O el indicador ya existe o hay un problema con la conexión de red ...",
	"message.flag.success": "El mensaje ha sido marcado exitosamente",
	"message.mute.error": "Error al silenciar a un usuario ...",
	"message.mute.success": "El usuario con id {id} ha sido silenciado",
	"message.status.delivered": "Entregado",
	"message.status.sending": "Enviando...",
	"message.title.message_actions": "Acciones del mensaje",
	"message.title.open_thread": "Comience un hilo",
	"message.title.reactions": "Reacciones",
	"message_actions.delete": "Borrar",
	"message_actions.edit": "Editar mensaje",
	"message_actions.flag": "Bandera",
	"message_actions.mute": "Mudo",
	"message_input.and": "y",
	"message_input.placeholder": "escribe tu mensaje",
	"message_input.typing": "{count, plural, one {is typing...} other {are typing...}}",
	"message_input.watching": "{count} en línea",
	"message_replies.count_button": "{count, plural, one {# reply} other {# replies}}",
	"message_status.and": "y",
	"message_status.more": "y {count} más",
	"modal.close": "Cerrar"
};

var messages_it = {
	"channel_filter.accounting-record": "registrazione contabile",
	"channel_filter.demand": "Richiesta",
	"channel_filter.empty": "Senza filtro",
	"channel_filter.supply": "fornitura",
	"channel_filter.voucher": "Voucher; tagliando",
	"channel_header.live": "Vivere",
	"channel_header.members": "{count} membri",
	"channel_header.watchers": "{count} online",
	"channel_preview.latest_message.attachment": "Allegato ...",
	"channel_preview.latest_message.command": "/{command}",
	"channel_preview.latest_message.deleted": "Messaggio cancellato...",
	"channel_preview.latest_message.empty": "Messaggio vuoto ...",
	"channel_preview.latest_message.none": "Ancora niente...",
	"channel_search.placeholder": "Ricerca...",
	"channel_sort.last": "i più nuovi",
	"channel_sort.old": "il più vecchio",
	"chat_down.connection_error": "errore di connessione",
	"chat_down.connection_error_text": "Errore durante la connessione alla chat, aggiorna la pagina per riprovare.",
	"command.error": "<strong>/{command}</strong> non è un comando valido",
	"date_separator.else": "L",
	"date_separator.last_week": "[Ultimo] dddd",
	"date_separator.next_week": "dddd",
	"date_separator.today": "[Oggi]",
	"date_separator.tomorrow": "[Domani]",
	"date_separator.yesterday": "[Ieri]",
	"edit_message.cancel": "Annulla",
	"edit_message.submit": "inviare",
	"event_component.member_added": "{user} si è unito alla chat",
	"event_component.member_removed": "{user} è stato rimosso dalla chat",
	"message.deleted": "Questo messaggio è stato cancellato ...",
	"message.error": "Errore · Non inviato",
	"message.failed": "Messaggio non riuscito · Fare clic per riprovare",
	"message.flag.error": "Errore durante l'aggiunta del flag: il flag esiste già o c'è un problema con la connessione di rete ...",
	"message.flag.success": "Il messaggio è stato contrassegnato correttamente",
	"message.mute.error": "Errore durante il silenziamento di un utente ...",
	"message.mute.success": "L'utente con ID {id} è stato disattivato",
	"message.status.delivered": "consegnato",
	"message.status.sending": "Invio ...",
	"message.title.message_actions": "Azioni sui messaggi",
	"message.title.open_thread": "Inizia una discussione",
	"message.title.reactions": "reazioni",
	"message_actions.delete": "Elimina",
	"message_actions.edit": "Modifica messaggio",
	"message_actions.flag": "bandiera",
	"message_actions.mute": "muto",
	"message_input.and": "e",
	"message_input.placeholder": "Scrivi il tuo messaggio",
	"message_input.typing": "{count, plural, one {is typing...} other {are typing...}}",
	"message_input.watching": "{count} online",
	"message_replies.count_button": "{count, plural, one {# reply} other {# replies}}",
	"message_status.and": "e",
	"message_status.more": "e altri {count} ",
	"modal.close": "Vicino"
};

var messages_pl = {
	"channel_filter.accounting-record": "Zapis księgowy",
	"channel_filter.demand": "Zadanie",
	"channel_filter.empty": "bez filtra",
	"channel_filter.supply": "Dostawa",
	"channel_filter.voucher": "Talon",
	"channel_header.live": "relacja na żywo",
	"channel_header.members": "{count} członków",
	"channel_header.watchers": "{count} online",
	"channel_preview.latest_message.attachment": "Przywiązanie...",
	"channel_preview.latest_message.command": "/{command}",
	"channel_preview.latest_message.deleted": "Wiadomość usunięta...",
	"channel_preview.latest_message.empty": "Pusta wiadomość ...",
	"channel_preview.latest_message.none": "Jeszcze nic...",
	"channel_search.placeholder": "Szukaj...",
	"channel_sort.last": "Najnowsza",
	"channel_sort.old": "najstarszy",
	"chat_down.connection_error": "błąd połączenia",
	"chat_down.connection_error_text": "Błąd połączenia z czatem, odśwież stronę, aby spróbować ponownie.",
	"command.error": "<strong>/{command}</strong> nie jest prawidłowym poleceniem",
	"date_separator.else": "L",
	"date_separator.last_week": "[Last] dddd",
	"date_separator.next_week": "dddd",
	"date_separator.today": "[Dzisiaj]",
	"date_separator.tomorrow": "[Jutro]",
	"date_separator.yesterday": "[Wczoraj]",
	"edit_message.cancel": "Anuluj",
	"edit_message.submit": "wysłać",
	"event_component.member_added": "{user} dołączył do czatu",
	"event_component.member_removed": "{user} został usunięty z czatu",
	"message.deleted": "Ta wiadomość została usunięta ...",
	"message.error": "Błąd · Niewysłany",
	"message.failed": "Wiadomość nie powiodła się · Kliknij, aby spróbować ponownie",
	"message.flag.error": "Błąd podczas dodawania flagi: albo flaga już istnieje, albo występuje problem z połączeniem sieciowym ...",
	"message.flag.success": "Wiadomość została pomyślnie oflagowana",
	"message.mute.error": "Błąd wyciszenia użytkownika ...",
	"message.mute.success": "Użytkownik o identyfikatorze {id} został wyciszony",
	"message.status.delivered": "Dostarczone",
	"message.status.sending": "Wysyłanie...",
	"message.title.message_actions": "Akcje wiadomości",
	"message.title.open_thread": "Rozpocznij wątek",
	"message.title.reactions": "Reakcje",
	"message_actions.delete": "Kasować",
	"message_actions.edit": "Edytuj wiadomość",
	"message_actions.flag": "Flaga",
	"message_actions.mute": "Niemy",
	"message_input.and": "i",
	"message_input.placeholder": "Wpisz swoją wiadomość",
	"message_input.typing": "{count, plural, one {is typing...} other {are typing...}}",
	"message_input.watching": "{count} online",
	"message_replies.count_button": "{count, plural, one {# reply} other {# replies}}",
	"message_status.and": "i",
	"message_status.more": "i jeszcze {count} ",
	"modal.close": "Blisko"
};

var messages_cs = {
	"channel_filter.accounting-record": "účetní záznam",
	"channel_filter.demand": "poptávka",
	"channel_filter.empty": "Žádný filtr",
	"channel_filter.supply": "Zásobování",
	"channel_filter.voucher": "poukaz",
	"channel_header.live": "žít",
	"channel_header.members": "{count} členů",
	"channel_header.watchers": "{count} online",
	"channel_preview.latest_message.attachment": "Příloha...",
	"channel_preview.latest_message.command": "/{command}",
	"channel_preview.latest_message.deleted": "Zpráva smazána...",
	"channel_preview.latest_message.empty": "Prázdná zpráva ...",
	"channel_preview.latest_message.none": "Zatím nic...",
	"channel_search.placeholder": "Vyhledávání...",
	"channel_sort.last": "Nejnovější",
	"channel_sort.old": "Nejstarší",
	"chat_down.connection_error": "Chyba připojení",
	"chat_down.connection_error_text": "Při připojování k chatu došlo k chybě, obnovte stránku a zkuste to znovu.",
	"command.error": "<strong>/{command}</strong> není platný příkaz",
	"date_separator.else": "L",
	"date_separator.last_week": "[Poslední] dddd",
	"date_separator.next_week": "dddd",
	"date_separator.today": "[Dnes]",
	"date_separator.tomorrow": "[Zítra]",
	"date_separator.yesterday": "[Včera]",
	"edit_message.cancel": "zrušení",
	"edit_message.submit": "Poslat",
	"event_component.member_added": "{user} se připojil k chatu",
	"event_component.member_removed": "{user} byl odstraněn z chatu",
	"message.deleted": "Tato zpráva byla smazána ...",
	"message.error": "Chyba · Neodesláno",
	"message.failed": "Zpráva se nezdařila · Klepnutím to zkuste znovu",
	"message.flag.error": "Chyba při přidávání příznaku: Příznak již existuje nebo je problém se síťovým připojením ...",
	"message.flag.success": "Zpráva byla úspěšně označena",
	"message.mute.error": "Chyba při ignorování uživatele ...",
	"message.mute.success": "Uživatel s ID {id} byl ignorován",
	"message.status.delivered": "Dodáno",
	"message.status.sending": "Odesílání ...",
	"message.title.message_actions": "Akce zpráv",
	"message.title.open_thread": "Začněte vlákno",
	"message.title.reactions": "Reakce",
	"message_actions.delete": "Odstranit",
	"message_actions.edit": "Upravit zprávu",
	"message_actions.flag": "Vlajka",
	"message_actions.mute": "Ztlumit",
	"message_input.and": "A",
	"message_input.placeholder": "Napište zprávu",
	"message_input.typing": "{count, plural, one {is typing...} other {are typing...}}",
	"message_input.watching": "{count} online",
	"message_replies.count_button": "{count, plural, one {# reply} other {# replies}}",
	"message_status.and": "A",
	"message_status.more": "a {count} dalších",
	"modal.close": "Zavřít"
};

var messages_da = {
	"channel_filter.accounting-record": "regnskabspost",
	"channel_filter.demand": "efterspørgsel",
	"channel_filter.empty": "intet filter",
	"channel_filter.supply": "levere",
	"channel_filter.voucher": "Rabatkupon",
	"channel_header.live": "Direkte",
	"channel_header.members": "{count} medlemmer",
	"channel_header.watchers": "{count} online",
	"channel_preview.latest_message.attachment": "Vedhæftet fil...",
	"channel_preview.latest_message.command": "/{command}",
	"channel_preview.latest_message.deleted": "Beskeden blev slettet ...",
	"channel_preview.latest_message.empty": "Tom besked ...",
	"channel_preview.latest_message.none": "Ingenting endnu...",
	"channel_search.placeholder": "Søg...",
	"channel_sort.last": "nyeste",
	"channel_sort.old": "ældste",
	"chat_down.connection_error": "Forbindelsesfejl",
	"chat_down.connection_error_text": "Fejl ved forbindelse til chat, opdater siden for at prøve igen.",
	"command.error": "<strong>/{command}</strong> er ikke en gyldig kommando",
	"date_separator.else": "L",
	"date_separator.last_week": "[Sidste] dddd",
	"date_separator.next_week": "dddd",
	"date_separator.today": "[I dag]",
	"date_separator.tomorrow": "[I morgen]",
	"date_separator.yesterday": "[I går]",
	"edit_message.cancel": "afbestille",
	"edit_message.submit": "Sende",
	"event_component.member_added": "{user} er blevet medlem af chatten",
	"event_component.member_removed": "{user} blev fjernet fra chatten",
	"message.deleted": "Denne meddelelse blev slettet ...",
	"message.error": "Fejl · Ikke sendt",
	"message.failed": "Beskeden mislykkedes · Klik for at prøve igen",
	"message.flag.error": "Fejl ved tilføjelse af flag: Enten findes flaget allerede, eller der er problemer med netværksforbindelse ...",
	"message.flag.success": "Beskeden er blevet markeret",
	"message.mute.error": "Fejl ved dæmpning af en bruger ...",
	"message.mute.success": "Bruger med id {id} er slået fra",
	"message.status.delivered": "leveres",
	"message.status.sending": "Sender ...",
	"message.title.message_actions": "Beskedhandlinger",
	"message.title.open_thread": "Start en tråd",
	"message.title.reactions": "Reaktioner",
	"message_actions.delete": "Slet",
	"message_actions.edit": "Rediger meddelelse",
	"message_actions.flag": "Flag",
	"message_actions.mute": "Stum",
	"message_input.and": "og",
	"message_input.placeholder": "Skriv din besked",
	"message_input.typing": "{count, plural, one {is typing...} other {are typing...}}",
	"message_input.watching": "{count} online",
	"message_replies.count_button": "{count, plural, one {# reply} other {# replies}}",
	"message_status.and": "og",
	"message_status.more": "og {count} mere",
	"modal.close": "Tæt"
};

var messages_fr = {
	"channel_filter.accounting-record": "enregistrement comptable",
	"channel_filter.demand": "Demande",
	"channel_filter.empty": "Pas de filtre",
	"channel_filter.supply": "la fourniture",
	"channel_filter.voucher": "Bon",
	"channel_header.live": "Vivre",
	"channel_header.members": "{count} membres",
	"channel_header.watchers": "{count} en ligne",
	"channel_preview.latest_message.attachment": "Attachement...",
	"channel_preview.latest_message.command": "/{command}",
	"channel_preview.latest_message.deleted": "Message supprimé ...",
	"channel_preview.latest_message.empty": "Message vide ...",
	"channel_preview.latest_message.none": "Rien pour le moment...",
	"channel_search.placeholder": "Chercher...",
	"channel_sort.last": "Plus récent",
	"channel_sort.old": "Le plus ancien",
	"chat_down.connection_error": "Erreur de connexion",
	"chat_down.connection_error_text": "Erreur de connexion au chat, actualisez la page pour réessayer.",
	"command.error": "<strong>/{command}</strong> n'est pas une commande valide",
	"date_separator.else": "L",
	"date_separator.last_week": "[Dernier] jjjj",
	"date_separator.next_week": "dddd",
	"date_separator.today": "[Aujourd'hui]",
	"date_separator.tomorrow": "[Demain]",
	"date_separator.yesterday": "[Hier]",
	"edit_message.cancel": "annuler",
	"edit_message.submit": "Envoyer",
	"event_component.member_added": "{user} a rejoint le chat",
	"event_component.member_removed": "{user} a été supprimé du chat",
	"message.deleted": "Ce message a été supprimé ...",
	"message.error": "Erreur · Non envoyé",
	"message.failed": "Échec du message · Cliquez pour réessayer",
	"message.flag.error": "Erreur lors de l'ajout de l'indicateur: l'indicateur existe déjà ou il y a un problème avec la connexion réseau ...",
	"message.flag.success": "Le message a été signalé avec succès",
	"message.mute.error": "Erreur lors de la coupure d'un utilisateur ...",
	"message.mute.success": "L'utilisateur avec l'identifiant {id} a été mis en sourdine",
	"message.status.delivered": "Livré",
	"message.status.sending": "Envoi...",
	"message.title.message_actions": "Actions de message",
	"message.title.open_thread": "Démarrer un fil",
	"message.title.reactions": "Les réactions",
	"message_actions.delete": "Effacer",
	"message_actions.edit": "Modifier le message",
	"message_actions.flag": "Drapeau",
	"message_actions.mute": "Muet",
	"message_input.and": "et",
	"message_input.placeholder": "Tapez votre message",
	"message_input.typing": "{count, plural, one {is typing...} other {are typing...}}",
	"message_input.watching": "{count} en ligne",
	"message_replies.count_button": "{count, plural, one {# reply} other {# replies}}",
	"message_status.and": "et",
	"message_status.more": "et {count} plus",
	"modal.close": "Fermer"
};

var messages_nl = {
	"channel_filter.accounting-record": "boekhouding",
	"channel_filter.demand": "vraag naar",
	"channel_filter.empty": "Geen filter",
	"channel_filter.supply": "Levering",
	"channel_filter.voucher": "bon",
	"channel_header.live": "LEVEN",
	"channel_header.members": "{count} leden",
	"channel_header.watchers": "{count} online",
	"channel_preview.latest_message.attachment": "Hechting...",
	"channel_preview.latest_message.command": "/{command}",
	"channel_preview.latest_message.deleted": "Bericht verwijderd...",
	"channel_preview.latest_message.empty": "Leeg bericht ...",
	"channel_preview.latest_message.none": "Nog niets...",
	"channel_search.placeholder": "Zoeken...",
	"channel_sort.last": "nieuwste",
	"channel_sort.old": "oudste",
	"chat_down.connection_error": "Verbindingsfout",
	"chat_down.connection_error_text": "Fout bij verbinden met chat, vernieuw de pagina om het opnieuw te proberen.",
	"command.error": "<strong>/{command}</strong> is geen geldige opdracht",
	"date_separator.else": "L",
	"date_separator.last_week": "[Last] dddd",
	"date_separator.next_week": "dddd",
	"date_separator.today": "[Vandaag]",
	"date_separator.tomorrow": "[Morgen]",
	"date_separator.yesterday": "[Gisteren]",
	"edit_message.cancel": "annuleren",
	"edit_message.submit": "Sturen",
	"event_component.member_added": "{user} neemt deel aan de chat",
	"event_component.member_removed": "{user} is verwijderd uit de chat",
	"message.deleted": "Dit bericht is verwijderd ...",
	"message.error": "Fout · Niet verzonden",
	"message.failed": "Bericht mislukt · Klik om het opnieuw te proberen",
	"message.flag.error": "Fout bij het toevoegen van vlag: de vlag bestaat al of er is een probleem met de netwerkverbinding ...",
	"message.flag.success": "Bericht is succesvol gemarkeerd",
	"message.mute.error": "Fout bij het dempen van een gebruiker ...",
	"message.mute.success": "Gebruiker met id {id} is gedempt",
	"message.status.delivered": "Geleverd",
	"message.status.sending": "Bezig met verzenden...",
	"message.title.message_actions": "Bericht acties",
	"message.title.open_thread": "Start een draad",
	"message.title.reactions": "Reacties",
	"message_actions.delete": "Verwijder",
	"message_actions.edit": "Bewerk bericht",
	"message_actions.flag": "Vlag",
	"message_actions.mute": "Dempen",
	"message_input.and": "en",
	"message_input.placeholder": "Schrijf je bericht",
	"message_input.typing": "{count, plural, one {is typing...} other {are typing...}}",
	"message_input.watching": "{count} online",
	"message_replies.count_button": "{count, plural, one {# reply} other {# replies}}",
	"message_status.and": "en",
	"message_status.more": "en {count} meer",
	"modal.close": "Dichtbij"
};

var messages_pt = {
	"channel_filter.accounting-record": "Registro contábil",
	"channel_filter.demand": "exigem",
	"channel_filter.empty": "Sem filtro",
	"channel_filter.supply": "Fornecem",
	"channel_filter.voucher": "Comprovante",
	"channel_header.live": "Viver",
	"channel_header.members": "{count} membros",
	"channel_header.watchers": "{count} online",
	"channel_preview.latest_message.attachment": "Anexo...",
	"channel_preview.latest_message.command": "/{command}",
	"channel_preview.latest_message.deleted": "Mensagem apagada...",
	"channel_preview.latest_message.empty": "Mensagem vazia ...",
	"channel_preview.latest_message.none": "Nada ainda...",
	"channel_search.placeholder": "Procurar...",
	"channel_sort.last": "O mais novo",
	"channel_sort.old": "Mais antigas",
	"chat_down.connection_error": "erro de conexão",
	"chat_down.connection_error_text": "Erro ao conectar-se ao bate-papo, atualize a página para tentar novamente.",
	"command.error": "<strong>/{command}</strong> não é um comando válido",
	"date_separator.else": "Eu",
	"date_separator.last_week": "[Last] dddd",
	"date_separator.next_week": "dddd",
	"date_separator.today": "[Hoje]",
	"date_separator.tomorrow": "[Amanhã]",
	"date_separator.yesterday": "[Ontem]",
	"edit_message.cancel": "Cancelar",
	"edit_message.submit": "Mandar",
	"event_component.member_added": "{user} entrou no chat",
	"event_component.member_removed": "{user} foi removido do chat",
	"message.deleted": "Esta mensagem foi excluída ...",
	"message.error": "Erro · Não enviado",
	"message.failed": "Mensagem falhou · Clique para tentar novamente",
	"message.flag.error": "Erro ao adicionar sinalizador: o sinalizador já existe ou há um problema com a conexão de rede ...",
	"message.flag.success": "A mensagem foi sinalizada com sucesso",
	"message.mute.error": "Erro ao silenciar um usuário ...",
	"message.mute.success": "O usuário com o ID {id} foi silenciado",
	"message.status.delivered": "Entregue",
	"message.status.sending": "Enviando ...",
	"message.title.message_actions": "Ações de mensagem",
	"message.title.open_thread": "Iniciar um tópico",
	"message.title.reactions": "Reacções",
	"message_actions.delete": "Excluir",
	"message_actions.edit": "Editar mensagem",
	"message_actions.flag": "Bandeira",
	"message_actions.mute": "Mudo",
	"message_input.and": "e",
	"message_input.placeholder": "Digite sua mensagem",
	"message_input.typing": "{count, plural, one {is typing...} other {are typing...}}",
	"message_input.watching": "{count} online",
	"message_replies.count_button": "{count, plural, one {# reply} other {# replies}}",
	"message_status.and": "e",
	"message_status.more": "e {count} mais",
	"modal.close": "Perto"
};

var messages_ru = {
	"channel_filter.accounting-record": "учетная запись",
	"channel_filter.demand": "требование",
	"channel_filter.empty": "нет фильтра",
	"channel_filter.supply": "ПОСТАВКА",
	"channel_filter.voucher": "ваучер",
	"channel_header.live": "Жить",
	"channel_header.members": "{count} участников",
	"channel_header.watchers": "{count} онлайн",
	"channel_preview.latest_message.attachment": "Вложение ...",
	"channel_preview.latest_message.command": "/{command}",
	"channel_preview.latest_message.deleted": "Сообщение удалено...",
	"channel_preview.latest_message.empty": "Пустое сообщение ...",
	"channel_preview.latest_message.none": "Пока ничего...",
	"channel_search.placeholder": "Поиск...",
	"channel_sort.last": "новейший",
	"channel_sort.old": "самый старший",
	"chat_down.connection_error": "Ошибка соединения",
	"chat_down.connection_error_text": "Ошибка подключения к чату, обновите страницу, чтобы повторить попытку.",
	"command.error": "<strong>/{command}</strong> не является допустимой командой",
	"date_separator.else": "L",
	"date_separator.last_week": "[Last] DDDD",
	"date_separator.next_week": "дддд",
	"date_separator.today": "[Сегодня]",
	"date_separator.tomorrow": "[Завтра]",
	"date_separator.yesterday": "[Вчера]",
	"edit_message.cancel": "отменить",
	"edit_message.submit": "послать",
	"event_component.member_added": "{user} присоединился к чату",
	"event_component.member_removed": "{user} был удален из чата",
	"message.deleted": "Это сообщение было удалено ...",
	"message.error": "Ошибка · Не отправлено",
	"message.failed": "Сообщение не удалось · Нажмите, чтобы повторить попытку",
	"message.flag.error": "Ошибка добавления флага: либо флаг уже существует, либо проблема с сетевым подключением ...",
	"message.flag.success": "Сообщение было успешно помечено",
	"message.mute.error": "Ошибка приглушения пользователя ...",
	"message.mute.success": "Пользователь с идентификатором {id} отключен",
	"message.status.delivered": "доставлен",
	"message.status.sending": "Посылка ...",
	"message.title.message_actions": "Действия сообщения",
	"message.title.open_thread": "Начать тему",
	"message.title.reactions": "Реакции",
	"message_actions.delete": "удалять",
	"message_actions.edit": "Редактировать сообщение",
	"message_actions.flag": "Флаг",
	"message_actions.mute": "безгласный",
	"message_input.and": "а также",
	"message_input.placeholder": "Введите ваше сообщение",
	"message_input.typing": "{count, plural, one {is typing...} other {are typing...}}",
	"message_input.watching": "{count} онлайн",
	"message_replies.count_button": "{count, plural, one {# reply} other {# replies}}",
	"message_status.and": "а также",
	"message_status.more": "и еще {count} ",
	"modal.close": "близко"
};

var messages_zh = {
	"channel_filter.accounting-record": "会计记录",
	"channel_filter.demand": "需求",
	"channel_filter.empty": "没有过滤器",
	"channel_filter.supply": "供应",
	"channel_filter.voucher": "凭证",
	"channel_header.live": "生活",
	"channel_header.members": "{count} 名成员",
	"channel_header.watchers": " 在线{count} ",
	"channel_preview.latest_message.attachment": "附件...",
	"channel_preview.latest_message.command": "/{command}",
	"channel_preview.latest_message.deleted": "讯息已删除...",
	"channel_preview.latest_message.empty": "空消息...",
	"channel_preview.latest_message.none": "还没有...",
	"channel_search.placeholder": "搜索...",
	"channel_sort.last": "最新",
	"channel_sort.old": "最老的",
	"chat_down.connection_error": "连接错误",
	"chat_down.connection_error_text": "连接聊天时出错，请刷新页面以重试。",
	"command.error": "<strong>/{command}</strong> 是无效命令",
	"date_separator.else": "大号",
	"date_separator.last_week": "[最后] dddd",
	"date_separator.next_week": "dddd",
	"date_separator.today": "[今天]",
	"date_separator.tomorrow": "[明天]",
	"date_separator.yesterday": "[昨天]",
	"edit_message.cancel": "取消",
	"edit_message.submit": "发送",
	"event_component.member_added": "{user} 已加入聊天",
	"event_component.member_removed": "{user} 已从聊天中删除",
	"message.deleted": "此消息已删除...",
	"message.error": "错误·未发送",
	"message.failed": "消息失败·单击以重试",
	"message.flag.error": "错误添加标记：该标记已经存在或网络连接有问题...",
	"message.flag.success": "消息已被成功标记",
	"message.mute.error": "屏蔽用户时出错...",
	"message.mute.success": "ID为 {id} 的用户已被静音",
	"message.status.delivered": "交付",
	"message.status.sending": "发送中...",
	"message.title.message_actions": "讯息动作",
	"message.title.open_thread": "启动主题",
	"message.title.reactions": "反应",
	"message_actions.delete": "删除",
	"message_actions.edit": "编辑讯息",
	"message_actions.flag": "旗",
	"message_actions.mute": "静音",
	"message_input.and": "和",
	"message_input.placeholder": "输入您的讯息",
	"message_input.typing": "{count, plural, one {is typing...} other {are typing...}}",
	"message_input.watching": " 在线{count} ",
	"message_replies.count_button": "{count, plural, one {# reply} other {# replies}}",
	"message_status.and": "和",
	"message_status.more": "还有另外 {count} 个",
	"modal.close": "关"
};

var messages = {
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

var Chat =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(Chat, _PureComponent);

  function Chat(props) {
    var _this;

    _classCallCheck(this, Chat);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(Chat).call(this, props));

    _defineProperty(_assertThisInitialized(_this), "setActiveChannel",
    /*#__PURE__*/
    function () {
      var _ref = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee(channel) {
        var watchers,
            e,
            _args = arguments;
        return _regeneratorRuntime.wrap(function _callee$(_context) {
          while (1) {
            switch (_context.prev = _context.next) {
              case 0:
                watchers = _args.length > 1 && _args[1] !== undefined ? _args[1] : {};
                e = _args.length > 2 ? _args[2] : undefined;

                if (e !== undefined && e.preventDefault) {
                  e.preventDefault();
                }

                if (!Object.keys(watchers).length) {
                  _context.next = 6;
                  break;
                }

                _context.next = 6;
                return channel.query({
                  watch: true,
                  watchers: watchers
                });

              case 6:
                if (_this.state.channel === channel) {
                  _this.setState(function () {
                    return {
                      channel: {}
                    };
                  });
                }

                _this.setState(function () {
                  return {
                    channel: channel
                  };
                });

              case 8:
              case "end":
                return _context.stop();
            }
          }
        }, _callee);
      }));

      return function (_x) {
        return _ref.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "setActiveSearch",
    /*#__PURE__*/
    function () {
      var _ref2 = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee2(search, e) {
        return _regeneratorRuntime.wrap(function _callee2$(_context2) {
          while (1) {
            switch (_context2.prev = _context2.next) {
              case 0:
                if (e !== undefined && e.preventDefault) {
                  e.preventDefault();
                }

                if (search) {
                  _this.setState(function () {
                    return {
                      search: search
                    };
                  });
                } else {
                  _this.setState(function () {
                    return {
                      search: undefined
                    };
                  });
                }

              case 2:
              case "end":
                return _context2.stop();
            }
          }
        }, _callee2);
      }));

      return function (_x2, _x3) {
        return _ref2.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "getContext", function () {
      return {
        client: _this.props.client,
        channel: _this.state.channel,
        search: _this.state.search,
        setActiveChannel: _this.setActiveChannel,
        setActiveSearch: _this.setActiveSearch,
        theme: _this.props.theme
      };
    });

    _this.state = {
      // currently active channel
      channel: {},
      search: undefined,
      error: false
    };
    return _this;
  }

  _createClass(Chat, [{
    key: "render",
    value: function render() {
      return React__default.createElement(reactIntl.IntlProvider, {
        locale: this.props.locale,
        messages: messages[this.props.locale]
      }, React__default.createElement(ChatContext.Provider, {
        value: this.getContext()
      }, this.props.children));
    }
  }]);

  return Chat;
}(React.PureComponent);

_defineProperty(Chat, "propTypes", {
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
  messages: PropTypes.object
});

_defineProperty(Chat, "defaultProps", {
  theme: 'messaging light',
  locale: 'de'
});

/**
 * LoadingErrorIndicator - UI component for error indicator in Channel.
 *
 * @example ./docs/LoadingErrorIndicator.md
 * @extends PureComponent
 */

var LoadingErrorIndicator =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(LoadingErrorIndicator, _React$PureComponent);

  function LoadingErrorIndicator() {
    _classCallCheck(this, LoadingErrorIndicator);

    return _possibleConstructorReturn(this, _getPrototypeOf(LoadingErrorIndicator).apply(this, arguments));
  }

  _createClass(LoadingErrorIndicator, [{
    key: "render",
    value: function render() {
      if (!this.props.error) return null;
      return React__default.createElement("div", null, "Error: ", this.props.error.message);
    }
  }]);

  return LoadingErrorIndicator;
}(React__default.PureComponent);

_defineProperty(LoadingErrorIndicator, "propTypes", {
  /** Error object */
  error: PropTypes.oneOfType([PropTypes.object, PropTypes.bool])
});

_defineProperty(LoadingErrorIndicator, "defaultProps", {
  error: false
});

function ownKeys$5(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread$5(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys$5(source, true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys$5(source).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }
/**
 * Channel - Wrapper component for a channel. It needs to be place inside of the Chat component.
 * ChannelHeader, MessageList, Thread and MessageInput should be used as children of the Channel component.
 *
 * @example ./docs/Channel.md
 * @extends PureComponent
 */

exports.Channel =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(Channel, _PureComponent);

  function Channel(props) {
    var _this;

    _classCallCheck(this, Channel);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(Channel).call(this, props));
    _this.state = {
      error: false
    };
    return _this;
  }

  _createClass(Channel, [{
    key: "render",
    value: function render() {
      if (this.props.channel === undefined) {
        return null;
      }

      if (!this.props.channel.cid) {
        return null; // <div>Select a channel</div>;
      } // We use a wrapper to make sure the key variable is set.
      // this ensures that if you switch channel the component is recreated


      return React__default.createElement(ChannelInner, _extends({}, this.props, {
        key: this.props.channel.cid
      }));
    }
  }]);

  return Channel;
}(React.PureComponent);

_defineProperty(exports.Channel, "propTypes", {
  /** Which channel to connect to, will initialize the channel if it's not initialized yet */
  channel: PropTypes.shape({
    watch: PropTypes.func
  }).isRequired,

  /** Client is passed automatically via the Chat Context */
  client: PropTypes.object.isRequired,

  /**
   * Error indicator UI component. This will be shown on the screen if channel query fails.
   *
   * Defaults to and accepts same props as: [LoadingErrorIndicator](https://getstream.github.io/stream-chat-react/#loadingerrorindicator)
   *
   * */
  LoadingErrorIndicator: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   * Loading indicator UI component. This will be shown on the screen until the messages are
   * being queried from channelœ. Once the messages are loaded, loading indicator is removed from the screen
   * and replaced with children of the Channel component.
   *
   * Defaults to and accepts same props as: [LoadingIndicator](https://github.com/GetStream/stream-chat-react/blob/master/src/components/LoadingIndicator.js)
   */
  LoadingIndicator: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   * Message UI component to display a message in message list.
   *
   * Available built-in components (also accepts the same props as):
   *
   * 1. [MessageSimple](https://github.com/GetStream/stream-chat-react/blob/master/src/components/MessageSimple.js) (default)
   * 2. [MessageTeam](https://github.com/GetStream/stream-chat-react/blob/master/src/components/MessageTeam.js)
   * 3. [MessageLivestream](https://github.com/GetStream/stream-chat-react/blob/master/src/components/MessageLivestream.js)
   * 3. [MessageCommerce](https://github.com/GetStream/stream-chat-react/blob/master/src/components/MessageCommerce.js)
   *
   * */
  Message: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   * Attachment UI component to display attachment in individual message.
   *
   * Defaults to and accepts same props as: [Attachment](https://github.com/GetStream/stream-chat-react/blob/master/src/components/Attachment.js)
   * */
  Attachment: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   * Handle for click on @mention in message
   *
   * @param {Event} event DOM Click event
   * @param {User} user   Target [user object](https://getstream.io/chat/docs/#chat-doc-set-user) which is clicked
   */
  onMentionsClick: PropTypes.func,

  /**
   * Handle for hover on @mention in message
   *
   * @param {Event} event DOM hover event
   * @param {User} user   Target [user object](https://getstream.io/chat/docs/#chat-doc-set-user) which is hovered
   */
  onMentionsHover: PropTypes.func,

  /** Weather to allow multiple attachment uploads */
  multipleUploads: PropTypes.bool,

  /** List of accepted file types */
  acceptedFiles: PropTypes.array,

  /** Maximum number of attachments allowed per message */
  maxNumberOfFiles: PropTypes.number,

  /** Override send message request (Advanced usage only)
   *
   * @param {String} channelId full channel ID in format of `type:id`
   * @param {Object} message
   */
  doSendMessageRequest: PropTypes.func,

  /** Override update(edit) message request (Advanced usage only)
   *
   * @param {String} channelId full channel ID in format of `type:id`
   * @param {Object} updatedMessage
   */
  doUpdateMessageRequest: PropTypes.func
});

_defineProperty(exports.Channel, "defaultProps", {
  LoadingIndicator: LoadingIndicator,
  LoadingErrorIndicator: LoadingErrorIndicator,
  Message: exports.MessageSimple,
  Attachment: Attachment
});

var ChannelInner =
/*#__PURE__*/
function (_PureComponent2) {
  _inherits(ChannelInner, _PureComponent2);

  function ChannelInner(props) {
    var _this2;

    _classCallCheck(this, ChannelInner);

    _this2 = _possibleConstructorReturn(this, _getPrototypeOf(ChannelInner).call(this, props));

    _defineProperty(_assertThisInitialized(_this2), "openThread", function (message, e) {
      if (e && e.preventDefault) {
        e.preventDefault();
      }

      var channel = _this2.props.channel;
      var threadMessages = channel.state.threads[message.id] || [];

      _this2.setState({
        thread: message,
        threadMessages: threadMessages
      });
    });

    _defineProperty(_assertThisInitialized(_this2), "loadMoreThread",
    /*#__PURE__*/
    _asyncToGenerator(
    /*#__PURE__*/
    _regeneratorRuntime.mark(function _callee() {
      var channel, parentID, oldMessages, oldestMessageID, limit, queryResponse, hasMore, threadMessages;
      return _regeneratorRuntime.wrap(function _callee$(_context) {
        while (1) {
          switch (_context.prev = _context.next) {
            case 0:
              if (!_this2.state.threadLoadingMore) {
                _context.next = 2;
                break;
              }

              return _context.abrupt("return");

            case 2:
              _this2.setState({
                threadLoadingMore: true
              });

              channel = _this2.props.channel;
              parentID = _this2.state.thread.id;
              oldMessages = channel.state.threads[parentID] || [];
              oldestMessageID = oldMessages[0] ? oldMessages[0].id : null;
              limit = 50;
              _context.next = 10;
              return channel.getReplies(parentID, {
                limit: limit,
                id_lt: oldestMessageID
              });

            case 10:
              queryResponse = _context.sent;
              hasMore = queryResponse.messages.length === limit;
              threadMessages = channel.state.threads[parentID] || []; // next set loadingMore to false so we can start asking for more data...

              _this2._loadMoreThreadFinishedDebounced(hasMore, threadMessages);

            case 14:
            case "end":
              return _context.stop();
          }
        }
      }, _callee);
    })));

    _defineProperty(_assertThisInitialized(_this2), "loadMoreThreadFinished", function (threadHasMore, threadMessages) {
      _this2.setState({
        threadLoadingMore: false,
        threadHasMore: threadHasMore,
        threadMessages: threadMessages
      });
    });

    _defineProperty(_assertThisInitialized(_this2), "closeThread", function (e) {
      if (e && e.preventDefault) {
        e.preventDefault();
      }

      _this2.setState({
        thread: null,
        threadMessages: []
      });
    });

    _defineProperty(_assertThisInitialized(_this2), "updateMessage", function (updatedMessage, extraState) {
      var channel = _this2.props.channel;
      extraState = extraState || {}; // adds the message to the local channel state..
      // this adds to both the main channel state as well as any reply threads

      channel.state.addMessageSorted(updatedMessage); // update the Channel component state

      if (_this2.state.thread && updatedMessage.parent_id) {
        extraState.threadMessages = channel.state.threads[updatedMessage.parent_id] || [];
      }

      _this2.setState(_objectSpread$5({
        messages: channel.state.messages
      }, extraState));
    });

    _defineProperty(_assertThisInitialized(_this2), "removeMessage", function (message) {
      var channel = _this2.props.channel;
      channel.state.removeMessage(message);

      _this2.setState({
        messages: channel.state.messages
      });
    });

    _defineProperty(_assertThisInitialized(_this2), "createMessagePreview", function (text, attachments, parent, mentioned_users) {
      // create a preview of the message
      var clientSideID = "".concat(_this2.props.client.userID, "-") + uuidv4();
      var message = {
        text: text,
        html: text,
        __html: text,
        //id: tmpID,
        id: clientSideID,
        type: 'regular',
        status: 'sending',
        user: _objectSpread$5({
          id: _this2.props.client.userID
        }, _this2.props.client.user),
        created_at: new Date(),
        attachments: attachments,
        mentioned_users: mentioned_users,
        reactions: []
      };

      if (parent && parent.id) {
        message.parent_id = parent.id;
      }

      return message;
    });

    _defineProperty(_assertThisInitialized(_this2), "editMessage", function (updatedMessage) {
      if (_this2.props.doUpdateMessageRequest) {
        return Promise.resolve(_this2.props.doUpdateMessageRequest(_this2.props.channel.cid, updatedMessage));
      }

      return _this2.props.client.updateMessage(updatedMessage);
    });

    _defineProperty(_assertThisInitialized(_this2), "_sendMessage",
    /*#__PURE__*/
    function () {
      var _ref2 = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee2(message) {
        var text, attachments, id, parent_id, mentioned_users, messageData, messageResponse;
        return _regeneratorRuntime.wrap(function _callee2$(_context2) {
          while (1) {
            switch (_context2.prev = _context2.next) {
              case 0:
                text = message.text, attachments = message.attachments, id = message.id, parent_id = message.parent_id, mentioned_users = message.mentioned_users;
                messageData = {
                  text: text,
                  attachments: attachments,
                  mentioned_users: mentioned_users,
                  id: id,
                  parent_id: parent_id
                };
                _context2.prev = 2;

                if (!_this2.props.doSendMessageRequest) {
                  _context2.next = 9;
                  break;
                }

                _context2.next = 6;
                return _this2.props.doSendMessageRequest(_this2.props.channel.cid, messageData);

              case 6:
                messageResponse = _context2.sent;
                _context2.next = 12;
                break;

              case 9:
                _context2.next = 11;
                return _this2.props.channel.sendMessage(messageData);

              case 11:
                messageResponse = _context2.sent;

              case 12:
                // replace it after send is completed
                if (messageResponse.message) {
                  messageResponse.message.status = 'received';

                  _this2.updateMessage(messageResponse.message);
                }

                _context2.next = 19;
                break;

              case 15:
                _context2.prev = 15;
                _context2.t0 = _context2["catch"](2);
                // set the message to failed..
                message.status = 'failed';

                _this2.updateMessage(message);

              case 19:
              case "end":
                return _context2.stop();
            }
          }
        }, _callee2, null, [[2, 15]]);
      }));

      return function (_x) {
        return _ref2.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this2), "sendMessage",
    /*#__PURE__*/
    function () {
      var _ref4 = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee3(_ref3) {
        var text, _ref3$attachments, attachments, _ref3$mentioned_users, mentioned_users, parent, messagePreview;

        return _regeneratorRuntime.wrap(function _callee3$(_context3) {
          while (1) {
            switch (_context3.prev = _context3.next) {
              case 0:
                text = _ref3.text, _ref3$attachments = _ref3.attachments, attachments = _ref3$attachments === void 0 ? [] : _ref3$attachments, _ref3$mentioned_users = _ref3.mentioned_users, mentioned_users = _ref3$mentioned_users === void 0 ? [] : _ref3$mentioned_users, parent = _ref3.parent;

                // remove error messages upon submit
                _this2.props.channel.state.filterErrorMessages(); // create a local preview message to show in the UI


                messagePreview = _this2.createMessagePreview(text, attachments, parent, mentioned_users); // first we add the message to the UI

                _this2.updateMessage(messagePreview, {
                  messageInput: '',
                  commands: [],
                  userAutocomplete: []
                });

                _context3.next = 6;
                return _this2._sendMessage(messagePreview);

              case 6:
              case "end":
                return _context3.stop();
            }
          }
        }, _callee3);
      }));

      return function (_x2) {
        return _ref4.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this2), "retrySendMessage",
    /*#__PURE__*/
    function () {
      var _ref5 = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee4(message) {
        return _regeneratorRuntime.wrap(function _callee4$(_context4) {
          while (1) {
            switch (_context4.prev = _context4.next) {
              case 0:
                // set the message status to sending
                message = message.asMutable();
                message.status = 'sending';

                _this2.updateMessage(message); // actually try to send the message...


                _context4.next = 5;
                return _this2._sendMessage(message);

              case 5:
              case "end":
                return _context4.stop();
            }
          }
        }, _callee4);
      }));

      return function (_x3) {
        return _ref5.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this2), "handleEvent", function (e) {
      var channel = _this2.props.channel;
      var threadMessages = [];
      var threadState = {};

      if (_this2.state.thread) {
        threadMessages = channel.state.threads[_this2.state.thread.id] || [];
        threadState['threadMessages'] = threadMessages;
      }

      if (_this2.state.thread && e.message && e.message.id === _this2.state.thread.id) {
        threadState['thread'] = channel.state.messageToImmutable(e.message);
      }

      if (Object.keys(threadState).length > 0) {
        // TODO: in theory we should do 1 setState call not 2,
        // However the setStateThrottled doesn't support this
        _this2.setState(threadState);
      }

      if (e.type === 'message.new') {
        var mainChannelUpdated = true;

        if (e.message.parent_id && !e.message.show_in_channel) {
          mainChannelUpdated = false;
        }

        if (mainChannelUpdated && e.message.user.id !== _this2.props.client.userID) {
          if (Visibility.state() === 'visible') {
            _this2._markReadThrottled(channel);
          } else {
            var unread = channel.countUnread(_this2.lastRead);
            document.title = "(".concat(unread, ") ").concat(_this2.originalTitle);
          }
        }
      }

      if (e.type === 'member.added') {
        _this2.addToEventHistory(e);
      }

      if (e.type === 'member.removed') {
        _this2.addToEventHistory(e);
      }

      _this2._setStateThrottled({
        messages: channel.state.messages,
        watchers: channel.state.watchers,
        read: channel.state.read,
        typing: channel.state.typing,
        watcher_count: channel.state.watcher_count
      });
    });

    _defineProperty(_assertThisInitialized(_this2), "addToEventHistory", function (e) {
      _this2.setState(function (prevState) {
        if (!prevState.message || !prevState.message.length) {
          return;
        }

        var lastMessageId = prevState.messages[prevState.messages.length - 1].id;
        if (!prevState.eventHistory[lastMessageId]) return _objectSpread$5({}, prevState, {
          eventHistory: _objectSpread$5({}, prevState.eventHistory, _defineProperty({}, lastMessageId, [e]))
        });
        return _objectSpread$5({}, prevState, {
          eventHistory: _objectSpread$5({}, prevState.eventHistory, _defineProperty({}, lastMessageId, [].concat(_toConsumableArray(prevState.eventHistory[lastMessageId]), [e])))
        });
      });
    });

    _defineProperty(_assertThisInitialized(_this2), "markRead", function (channel) {
      if (!channel.getConfig().read_events) {
        return;
      }

      _this2.lastRead = new Date();
      streamChat.logChatPromiseExecution(channel.markRead(), 'mark read');

      if (_this2.originalTitle) {
        document.title = _this2.originalTitle;
      }
    });

    _defineProperty(_assertThisInitialized(_this2), "loadMore",
    /*#__PURE__*/
    _asyncToGenerator(
    /*#__PURE__*/
    _regeneratorRuntime.mark(function _callee5() {
      var limit,
          oldestMessage,
          oldestID,
          perPage,
          queryResponse,
          hasMore,
          _args5 = arguments;
      return _regeneratorRuntime.wrap(function _callee5$(_context5) {
        while (1) {
          switch (_context5.prev = _context5.next) {
            case 0:
              limit = _args5.length > 0 && _args5[0] !== undefined ? _args5[0] : 100;

              if (!_this2.state.loadingMore) {
                _context5.next = 3;
                break;
              }

              return _context5.abrupt("return");

            case 3:
              _this2.setState({
                loadingMore: true
              });

              oldestMessage = _this2.state.messages[0];

              if (!(oldestMessage && oldestMessage.status !== 'received')) {
                _context5.next = 8;
                break;
              }

              _this2.setState({
                loadingMore: false
              });

              return _context5.abrupt("return");

            case 8:
              oldestID = oldestMessage ? oldestMessage.id : null;
              perPage = limit;
              _context5.prev = 10;
              _context5.next = 13;
              return _this2.props.channel.query({
                messages: {
                  limit: perPage,
                  id_lt: oldestID
                }
              });

            case 13:
              queryResponse = _context5.sent;
              _context5.next = 21;
              break;

            case 16:
              _context5.prev = 16;
              _context5.t0 = _context5["catch"](10);
              console.warn('message pagination request failed with error', _context5.t0);

              _this2.setState({
                loadingMore: false
              });

              return _context5.abrupt("return");

            case 21:
              hasMore = queryResponse.messages.length === perPage; // recursive call load more untill all messagens loaded

              if (!hasMore) {
                _context5.next = 28;
                break;
              }

              _this2.setState({
                loadingMore: false,
                hasMore: hasMore,
                messages: _this2.props.channel.state.messages
              });

              _context5.next = 26;
              return _this2.loadMore();

            case 26:
              _context5.next = 29;
              break;

            case 28:
              _this2._loadMoreFinishedDebounced(hasMore, _this2.props.channel.state.messages);

            case 29:
            case "end":
              return _context5.stop();
          }
        }
      }, _callee5, null, [[10, 16]]);
    })));

    _defineProperty(_assertThisInitialized(_this2), "_onMentionsHoverOrClick", function (e, mentioned_users) {
      if (!_this2.props.onMentionsHover && !_this2.props.onMentionsClick) return;
      var tagName = e.target.tagName.toLowerCase();
      var textContent = e.target.innerHTML.replace('*', '');

      if (tagName === 'strong' && textContent[0] === '@') {
        var userName = textContent.replace('@', '');
        var user = mentioned_users.find(function (user) {
          return user.name === userName || user.id === userName;
        });

        if (_this2.props.onMentionsHover && typeof _this2.props.onMentionsHover === 'function' && e.type === 'mouseover') {
          _this2.props.onMentionsHover(e, user);
        }

        if (_this2.props.onMentionsClick && e.type === 'click' && typeof _this2.props.onMentionsClick === 'function') {
          _this2.props.onMentionsClick(e, user);
        }
      }
    });

    _defineProperty(_assertThisInitialized(_this2), "loadMoreFinished", function (hasMore, messages) {
      _this2.setState({
        loadingMore: false,
        hasMore: hasMore,
        messages: messages
      });
    });

    _defineProperty(_assertThisInitialized(_this2), "getContext", function () {
      return _objectSpread$5({}, _this2.state, {
        client: _this2.props.client,
        channel: _this2.props.channel,
        search: _this2.props.search,
        Message: _this2.props.Message,
        Attachment: _this2.props.Attachment,
        multipleUploads: _this2.props.multipleUploads,
        acceptedFiles: _this2.props.acceptedFiles,
        maxNumberOfFiles: _this2.props.maxNumberOfFiles,
        updateMessage: _this2.updateMessage,
        removeMessage: _this2.removeMessage,
        sendMessage: _this2.sendMessage,
        editMessage: _this2.editMessage,
        retrySendMessage: _this2.retrySendMessage,
        loadMore: _this2.loadMore,
        // thread related
        openThread: _this2.openThread,
        closeThread: _this2.closeThread,
        loadMoreThread: _this2.loadMoreThread,
        onMentionsClick: _this2._onMentionsHoverOrClick,
        onMentionsHover: _this2._onMentionsHoverOrClick
      });
    });

    _defineProperty(_assertThisInitialized(_this2), "renderComponent", function () {
      return _this2.props.children;
    });

    _this2.state = {
      error: false,
      // Loading the initial content of the channel
      loading: true,
      // Loading more messages
      loadingMore: false,
      hasMore: true,
      messages: Immutable([]),
      online: true,
      typing: Immutable({}),
      watchers: Immutable({}),
      members: Immutable({}),
      read: Immutable({}),
      eventHistory: {},
      thread: false,
      threadMessages: [],
      threadLoadingMore: false,
      threadHasMore: true
    }; // hard limit to prevent you from scrolling faster than 1 page per 2 seconds

    _this2._loadMoreFinishedDebounced = debounce(_this2.loadMoreFinished, 2000, {
      leading: true,
      trailing: true
    }); // hard limit to prevent you from scrolling faster than 1 page per 2 seconds

    _this2._loadMoreThreadFinishedDebounced = debounce(_this2.loadMoreThreadFinished, 2000, {
      leading: true,
      trailing: true
    });
    _this2._markReadThrottled = throttle(_this2.markRead, 500, {
      leading: true,
      trailing: true
    });
    _this2._setStateThrottled = throttle(_this2.setState, 500, {
      leading: true,
      trailing: true
    });
    return _this2;
  }

  _createClass(ChannelInner, [{
    key: "componentDidMount",
    value: function () {
      var _componentDidMount = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee6() {
        var channel, errored;
        return _regeneratorRuntime.wrap(function _callee6$(_context6) {
          while (1) {
            switch (_context6.prev = _context6.next) {
              case 0:
                channel = this.props.channel;
                errored = false;

                if (channel.initialized) {
                  _context6.next = 12;
                  break;
                }

                _context6.prev = 3;
                _context6.next = 6;
                return channel.watch();

              case 6:
                _context6.next = 12;
                break;

              case 8:
                _context6.prev = 8;
                _context6.t0 = _context6["catch"](3);
                this.setState({
                  error: _context6.t0
                });
                errored = true;

              case 12:
                this.originalTitle = document.title;
                this.lastRead = new Date();

                if (errored) {
                  _context6.next = 19;
                  break;
                }

                this.copyChannelState();
                this.listenToChanges(); // get all Messages for in memory search

                _context6.next = 19;
                return this.loadMore();

              case 19:
              case "end":
                return _context6.stop();
            }
          }
        }, _callee6, this, [[3, 8]]);
      }));

      function componentDidMount() {
        return _componentDidMount.apply(this, arguments);
      }

      return componentDidMount;
    }()
  }, {
    key: "componentDidUpdate",
    value: function componentDidUpdate() {
      // If there is an active thread, then in that case we should sync
      // it with updated state of channel.
      if (this.state.thread) {
        for (var i = this.state.messages.length - 1; i >= 0; i--) {
          if (this.state.messages[i].id === this.state.thread.id) {
            this.setState({
              thread: this.state.messages[i]
            });
            break;
          }
        }
      }
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      this.props.client.off('connection.recovered', this.handleEvent);
      this.props.channel.off(this.handleEvent);

      this._loadMoreFinishedDebounced.cancel();

      this._loadMoreThreadFinishedDebounced.cancel();

      if (this.visibilityListener || this.visibilityListener === 0) {
        Visibility.unbind(this.visibilityListener);
      }
    }
  }, {
    key: "copyChannelState",
    value: function copyChannelState() {
      var channel = this.props.channel;
      this.setState({
        messages: channel.state.messages,
        read: channel.state.read,
        watchers: channel.state.watchers,
        members: channel.state.members,
        watcher_count: channel.state.watcher_count,
        loading: false,
        typing: Immutable({})
      });
      if (channel.countUnread() > 0) channel.markRead();
    }
  }, {
    key: "removeEphemeralMessages",
    value: function removeEphemeralMessages() {
      var c = this.props.channel;
      c.state.selectRegularMessages();
      this.setState({
        messages: c.state.messages
      });
    }
  }, {
    key: "listenToChanges",
    value: function listenToChanges() {
      var _this3 = this;

      // The more complex sync logic is done in chat.js
      // listen to client.connection.recovered and all channel events
      this.props.client.on('connection.recovered', this.handleEvent);
      var channel = this.props.channel;
      channel.on(this.handleEvent);
      this.boundMarkRead = this.markRead.bind(this, channel);
      this.visibilityListener = Visibility.change(function (e, state) {
        if (state === 'visible') {
          _this3.boundMarkRead();
        }
      });
    }
  }, {
    key: "render",
    value: function render() {
      var core;
      var LoadingIndicator$$1 = this.props.LoadingIndicator;
      var LoadingErrorIndicator$$1 = this.props.LoadingErrorIndicator;

      if (this.state.error) {
        core = React__default.createElement(LoadingErrorIndicator$$1, {
          error: this.state.error
        });
      } else if (this.state.loading) {
        core = React__default.createElement(LoadingIndicator$$1, {
          size: 25,
          isLoading: true
        });
      } else if (!this.props.channel || !this.props.channel.watch) {
        core = React__default.createElement("div", null, "Channel Missing");
      } else {
        core = React__default.createElement(ChannelContext.Provider, {
          value: this.getContext()
        }, React__default.createElement("div", {
          className: "str-chat__container"
        }, this.renderComponent()));
      }

      return React__default.createElement("div", {
        className: "str-chat str-chat-channel ".concat(this.props.theme)
      }, core);
    }
  }]);

  return ChannelInner;
}(React.PureComponent);

_defineProperty(ChannelInner, "propTypes", {
  /** Which channel to connect to */
  channel: PropTypes.shape({
    watch: PropTypes.func
  }).isRequired,

  /** Client is passed via the Chat Context */
  client: PropTypes.object.isRequired,

  /** The loading indicator to use */
  LoadingIndicator: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),
  LoadingErrorIndicator: PropTypes.oneOfType([PropTypes.node, PropTypes.func])
});

exports.Channel = withChatContext(exports.Channel);

/**
 * ChannelHeader - Render some basic information about this channel
 *
 * @example ./docs/ChannelHeader.md
 * @extends PureComponent
 */

exports.ChannelHeader =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(ChannelHeader, _PureComponent);

  function ChannelHeader(props) {
    var _this;

    _classCallCheck(this, ChannelHeader);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(ChannelHeader).call(this, props));

    _defineProperty(_assertThisInitialized(_this), "SetResponsive",
    /*#__PURE__*/
    _asyncToGenerator(
    /*#__PURE__*/
    _regeneratorRuntime.mark(function _callee() {
      return _regeneratorRuntime.wrap(function _callee$(_context) {
        while (1) {
          switch (_context.prev = _context.next) {
            case 0:
              _context.next = 2;
              return _this.setState({
                isLoading: true
              });

            case 2:
              _context.next = 4;
              return _this.props.channel.update({
                name: _this.props.channel.data.name,
                extraType: _this.props.channel.data.extraType,
                referenceId: _this.props.channel.data.referenceId,
                assignFlag: true,
                languages: _this.props.channel.data.languages,
                responsiveMember: _this.props.client.user.name
              }, {
                text: _this.props.client.user.name + ' ist jetzt der neue Bearbeiter',
                user_id: _this.props.client.user.id
              });

            case 4:
              _context.next = 6;
              return _this.setState({
                isLoading: false
              });

            case 6:
            case "end":
              return _context.stop();
          }
        }
      }, _callee);
    })));

    _defineProperty(_assertThisInitialized(_this), "LanguageSelect", function () {
      return React__default.createElement("select", {
        id: "select",
        className: "str-chat__channel-list-team__header--button",
        onChange: _this.onLanguageChange,
        defaultValue: _this.props.language
      }, _this.languageOptions);
    });

    _defineProperty(_assertThisInitialized(_this), "jumpTo", function () {
      var _this$props$channel$d = _this.props.channel.data,
          extraType = _this$props$channel$d.extraType,
          referenceId = _this$props$channel$d.referenceId;
      window.dplChatConfig.goToDetails(extraType, referenceId);
    });

    _defineProperty(_assertThisInitialized(_this), "languageOptions", window.dplChatConfig.languages.map(function (option, j) {
      if (option.value === '') {
        return React__default.createElement("option", {
          key: j,
          value: option.value
        }, option.label);
      }

      if (_this.props.channel.data.languages !== null) {
        if (_this.props.channel.data.languages.find(function (element) {
          return element === option.value;
        })) {
          return React__default.createElement("option", {
            key: j,
            value: option.value
          }, option.label);
        } else {
          return null;
        }
      } else {
        return null;
      }
    }));

    _this.state = {
      isLoading: false
    };
    return _this;
  }

  _createClass(ChannelHeader, [{
    key: "render",
    value: function render() {
      var _this2 = this;

      var intl = this.props.intl;
      return React__default.createElement("div", {
        className: "str-chat__header-livestream"
      }, this.props.channel.data.image && React__default.createElement(Avatar, {
        image: this.props.channel.data.image,
        shape: "rounded",
        size: this.props.channel.type === 'commerce' ? 60 : 40
      }), React__default.createElement("div", {
        className: "str-chat__header-livestream-left"
      }, React__default.createElement("p", {
        className: "str-chat__header-livestream-left--title"
      }, React__default.createElement("a", {
        className: "channel-header-link",
        onClick: function onClick() {
          return _this2.jumpTo();
        }
      }, this.props.title || this.props.channel.data.name, ' '), this.props.live && React__default.createElement("span", {
        className: "str-chat__header-livestream-left--livelabel"
      }, React__default.createElement(reactIntl.FormattedMessage, {
        id: "channel_header.live",
        defaultMessage: "live"
      }))), this.props.channel.data.subtitle && React__default.createElement("p", {
        className: "str-chat__header-livestream-left--subtitle"
      }, this.props.channel.data.subtitle), React__default.createElement("p", {
        className: "str-chat__header-livestream-left--members"
      }, !this.props.live && this.props.channel.data.member_count > 0 && React__default.createElement(React__default.Fragment, null, React__default.createElement(reactIntl.FormattedMessage, {
        id: "channel_header.members",
        defaultMessage: "{count} members",
        values: {
          count: this.props.channel.data.member_count
        }
      }), ",", ' '), React__default.createElement(reactIntl.FormattedMessage, {
        id: "channel_header.watchers",
        defaultMessage: "{count} online",
        values: {
          count: this.props.watcher_count
        }
      })), React__default.createElement("div", {
        className: "str-chat__channel_header_responsive_member-label--all"
      }, React__default.createElement("label", null, this.props.channel.data.responsiveMember ? 'In Bearbeitung' + '  (dies sehen alle)' : ''))), React__default.createElement("div", {
        id: "channel_header_responsive_member"
      }, this.props.client.user.role === 'admin' && this.props.client.user.name !== this.props.channel.data.responsiveMember && !this.state.isLoading ? React__default.createElement(React__default.Fragment, null, React__default.createElement("button", {
        onClick: this.SetResponsive
      }, this.props.channel.data.responsiveMember ? 'Übernehme Bearbeitung' : 'Starte Bearbeitung')) : React__default.createElement("label", null)), React__default.createElement("div", {
        id: "channel_header_left_panel"
      }, React__default.createElement(this.LanguageSelect, null)));
    } // deleteChannel = async () => {
    //   const response = await this.channel.delete();
    //   console.log('channel is deleted', response);
    // };

  }, {
    key: "onLanguageChange",
    value: function onLanguageChange(event) {
      window.dplChatConfig.language = event.target.value;
      window.dispatchEvent(languageChangedEvent);
      localStorage.setItem('language', window.dplChatConfig.language);
    }
  }]);

  return ChannelHeader;
}(React.PureComponent);

_defineProperty(exports.ChannelHeader, "propTypes", {
  /** Set title manually */
  title: PropTypes.string,

  /** Show a little indicator that the channel is live right now */
  live: PropTypes.bool,

  /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#chat)** */
  channel: PropTypes.object.isRequired,

  /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#chat)** */
  watcher_count: PropTypes.number,
  language: PropTypes.string
});

_defineProperty(exports.ChannelHeader, "defaultProps", {
  language: localStorage.getItem('language')
});

exports.ChannelHeader = withChannelContext(exports.ChannelHeader);

/**
 * MessageInputFlat - Large Message Input to be used for the MessageInput.
 * @example ./docs/MessageInputFlat.md
 */

exports.MessageInputFlat =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(MessageInputFlat, _PureComponent);

  function MessageInputFlat() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, MessageInputFlat);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(MessageInputFlat)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "renderUploads", function () {
      return React__default.createElement(React__default.Fragment, null, _this.props.imageOrder.length > 0 && React__default.createElement(reactFileUtils.ImagePreviewer, {
        imageUploads: _this.props.imageOrder.map(function (id) {
          return _this.props.imageUploads[id];
        }),
        handleRemove: _this.props.removeImage,
        handleRetry: _this.props.uploadImage,
        handleFiles: _this.props.uploadNewFiles,
        multiple: _this.props.multipleUploads,
        disabled: _this.props.numberOfUploads >= _this.props.maxNumberOfFiles
      }), _this.props.fileOrder.length > 0 && React__default.createElement("div", {
        className: "str-chat__file-uploads"
      }, React__default.createElement(reactFileUtils.FilePreviewer, {
        uploads: _this.props.fileOrder.map(function (id) {
          return _this.props.fileUploads[id];
        }),
        handleRemove: _this.props.removeFile,
        handleRetry: _this.props.uploadFile,
        handleFiles: _this.props.uploadNewFiles
      })));
    });

    _defineProperty(_assertThisInitialized(_this), "renderEmojiPicker", function () {
      if (_this.props.emojiPickerIsOpen) {
        return React__default.createElement("div", {
          className: "str-chat__input-flat--emojipicker",
          ref: _this.props.emojiPickerRef
        }, React__default.createElement(emojiMart.Picker, {
          native: true,
          emoji: "point_up",
          title: "Pick your emoji\u2026",
          onSelect: _this.props.onSelectEmoji,
          color: "#006CFF",
          showPreview: false
        }));
      }
    });

    return _this;
  }

  _createClass(MessageInputFlat, [{
    key: "render",
    value: function render() {
      var _this2 = this;

      var intl = this.props.intl;
      var SendButton = this.props.SendButton;
      return React__default.createElement("div", {
        className: "str-chat__input-flat ".concat(SendButton ? 'str-chat__input-flat--send-button-active' : null)
      }, React__default.createElement(reactFileUtils.ImageDropzone, {
        accept: this.props.acceptedFiles,
        multiple: this.props.multipleUploads,
        disabled: this.props.numberOfUploads >= this.props.maxNumberOfFiles,
        handleFiles: this.props.uploadNewFiles
      }, React__default.createElement("div", {
        className: "str-chat__input-flat-wrapper"
      }, this.renderUploads(), this.renderEmojiPicker(), React__default.createElement("div", {
        className: "str-chat__input-flat--textarea-wrapper"
      }, React__default.createElement(ChatAutoComplete, {
        users: this.props.getUsers(),
        commands: this.props.getCommands(),
        innerRef: this.props.textareaRef,
        handleSubmit: function handleSubmit(e) {
          return _this2.props.handleSubmit(e);
        },
        onSelectItem: this.props.onSelectItem,
        onChange: this.props.handleChange,
        value: this.props.text,
        rows: 1,
        maxRows: this.props.maxRows,
        placeholder: intl.formatMessage({
          id: 'message_input.placeholder',
          defaultMessage: 'Type your message'
        }),
        onPaste: this.props.onPaste,
        grow: this.props.grow,
        onFocus: this.props.onFocus,
        disabled: this.props.disabled
      }), React__default.createElement("span", {
        className: "str-chat__input-flat-emojiselect",
        onClick: this.props.openEmojiPicker
      }, React__default.createElement("svg", {
        width: "28",
        height: "28",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        d: "M22.217 16.1c.483.25.674.849.423 1.334C21.163 20.294 17.771 22 14 22c-3.867 0-7.347-1.765-8.66-4.605a.994.994 0 0 1 .9-1.407c.385 0 .739.225.9.575C8.135 18.715 10.892 20 14 20c3.038 0 5.738-1.267 6.879-3.476a.99.99 0 0 1 1.338-.424zm1.583-3.652c.341.443.235 1.064-.237 1.384a1.082 1.082 0 0 1-.62.168c-.338 0-.659-.132-.858-.389-.212-.276-.476-.611-1.076-.611-.598 0-.864.337-1.08.614-.197.254-.517.386-.854.386-.224 0-.438-.045-.62-.167-.517-.349-.578-.947-.235-1.388.66-.847 1.483-1.445 2.789-1.445 1.305 0 2.136.6 2.79 1.448zm-14 0c.341.443.235 1.064-.237 1.384a1.082 1.082 0 0 1-.62.168c-.339 0-.659-.132-.858-.389C7.873 13.335 7.61 13 7.01 13c-.598 0-.864.337-1.08.614-.197.254-.517.386-.854.386-.224 0-.438-.045-.62-.167-.518-.349-.579-.947-.235-1.388C4.88 11.598 5.703 11 7.01 11c1.305 0 2.136.6 2.79 1.448zM14 0c7.732 0 14 6.268 14 14s-6.268 14-14 14S0 21.732 0 14 6.268 0 14 0zm8.485 22.485A11.922 11.922 0 0 0 26 14c0-3.205-1.248-6.219-3.515-8.485A11.922 11.922 0 0 0 14 2a11.922 11.922 0 0 0-8.485 3.515A11.922 11.922 0 0 0 2 14c0 3.205 1.248 6.219 3.515 8.485A11.922 11.922 0 0 0 14 26c3.205 0 6.219-1.248 8.485-3.515z",
        fillRule: "evenodd"
      }))), React__default.createElement(reactFileUtils.FileUploadButton, {
        multiple: this.props.multipleUploads,
        disabled: this.props.numberOfUploads >= this.props.maxNumberOfFiles,
        accepts: this.props.acceptedFiles,
        handleFiles: this.props.uploadNewFiles
      }, React__default.createElement("span", {
        className: "str-chat__input-flat-fileupload"
      }, React__default.createElement("svg", {
        width: "14",
        height: "14",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        d: "M1.667.333h10.666c.737 0 1.334.597 1.334 1.334v10.666c0 .737-.597 1.334-1.334 1.334H1.667a1.333 1.333 0 0 1-1.334-1.334V1.667C.333.93.93.333 1.667.333zm2 1.334a1.667 1.667 0 1 0 0 3.333 1.667 1.667 0 0 0 0-3.333zm-2 9.333v1.333h10.666v-4l-2-2-4 4-2-2L1.667 11z",
        fillRule: "nonzero"
      })))), SendButton && React__default.createElement(SendButton, {
        sendMessage: this.props.handleSubmit
      })))));
    }
  }]);

  return MessageInputFlat;
}(React.PureComponent);

_defineProperty(exports.MessageInputFlat, "propTypes", {
  /** Set focus to the text input if this is enabled */
  focus: PropTypes.bool,

  /** Grow the textarea while you're typing */
  grow: PropTypes.bool,

  /** Disable the textarea */
  disabled: PropTypes.bool,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  imageOrder: PropTypes.array,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  imageUploads: PropTypes.object,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  removeImage: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  uploadImage: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  uploadNewFiles: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  numberOfUploads: PropTypes.number,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  fileOrder: PropTypes.array,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  fileUploads: PropTypes.object,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  removeFile: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  uploadFile: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  emojiPickerIsOpen: PropTypes.bool,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  emojiPickerRef: PropTypes.object,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  onSelectEmoji: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  getUsers: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  getCommands: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  textareaRef: PropTypes.object,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  handleSubmit: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  handleChange: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  onSelectItem: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  text: PropTypes.string,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  onPaste: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  openEmojiPicker: PropTypes.func,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  watcher_count: PropTypes.number,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  typing: PropTypes.object,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  multipleUploads: PropTypes.object,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  maxNumberOfFiles: PropTypes.object,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  acceptedFiles: PropTypes.object
});

_defineProperty(exports.MessageInputFlat, "defaultProps", {
  grow: true,
  disabled: false
});

exports.MessageInputFlat = reactIntl.injectIntl(exports.MessageInputFlat);

/**
 * MessageInputSmall - compact design to be used for the MessageInput. It has all the features of MessageInput minus the typing indicator.
 * @example ./docs/MessageInputSmall.md
 */

exports.MessageInputSmall =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(MessageInputSmall, _PureComponent);

  function MessageInputSmall() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, MessageInputSmall);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(MessageInputSmall)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "renderUploads", function () {
      return React__default.createElement(React__default.Fragment, null, _this.props.imageOrder.length > 0 && React__default.createElement(reactFileUtils.ImagePreviewer, {
        imageUploads: _this.props.imageOrder.map(function (id) {
          return _this.props.imageUploads[id];
        }),
        handleRemove: _this.props.removeImage,
        handleRetry: _this.props.uploadImage,
        handleFiles: _this.props.uploadNewFiles,
        multiple: _this.props.multipleUploads,
        disabled: _this.props.numberOfUploads >= _this.props.maxNumberOfFiles ? true : false
      }), _this.props.fileOrder.length > 0 && React__default.createElement(reactFileUtils.FilePreviewer, {
        uploads: _this.props.fileOrder.map(function (id) {
          return _this.props.fileUploads[id];
        }),
        handleRemove: _this.props.removeFile,
        handleRetry: _this.props.uploadFile,
        handleFiles: _this.props.uploadNewFiles
      }));
    });

    _defineProperty(_assertThisInitialized(_this), "renderEmojiPicker", function () {
      if (_this.props.emojiPickerIsOpen) {
        return React__default.createElement("div", {
          className: "str-chat__small-message-input-emojipicker",
          ref: _this.props.emojiPickerRef
        }, React__default.createElement(emojiMart.Picker, {
          native: true,
          emoji: "point_up",
          title: "Pick your emoji\u2026",
          onSelect: _this.props.onSelectEmoji,
          color: "#006CFF",
          showPreview: false
        }));
      }
    });

    return _this;
  }

  _createClass(MessageInputSmall, [{
    key: "render",
    value: function render() {
      var intl = this.props.intl;
      var SendButton = this.props.SendButton;
      return React__default.createElement("div", {
        className: "str-chat__small-message-input__wrapper"
      }, React__default.createElement(reactFileUtils.ImageDropzone, {
        accept: this.props.acceptedFiles,
        multiple: this.props.multipleUploads,
        disabled: this.props.numberOfUploads >= this.props.maxNumberOfFiles ? true : false,
        handleFiles: this.props.uploadNewFiles
      }, React__default.createElement("div", {
        className: "str-chat__small-message-input ".concat(SendButton ? 'str-chat__small-message-input--send-button-active' : null)
      }, this.renderUploads(), this.renderEmojiPicker(), React__default.createElement("div", {
        className: "str-chat__small-message-input--textarea-wrapper"
      }, React__default.createElement(ChatAutoComplete, {
        users: this.props.getUsers(),
        commands: this.props.getCommands(),
        innerRef: this.props.textareaRef,
        handleSubmit: this.props.handleSubmit,
        onChange: this.props.handleChange,
        value: this.props.text,
        rows: 1,
        maxRows: this.props.maxRows,
        onSelectItem: this.props.onSelectItem,
        placeholder: intl.formatMessage({
          id: 'message_input.placeholder',
          defaultMessage: 'Type your message'
        }),
        onPaste: this.props.onPaste,
        grow: this.props.grow,
        disabled: this.props.disabled
      }), React__default.createElement("span", {
        className: "str-chat__small-message-input-emojiselect",
        onClick: this.props.openEmojiPicker
      }, React__default.createElement("svg", {
        width: "14",
        height: "14",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        d: "M11.108 8.05a.496.496 0 0 1 .212.667C10.581 10.147 8.886 11 7 11c-1.933 0-3.673-.882-4.33-2.302a.497.497 0 0 1 .9-.417C4.068 9.357 5.446 10 7 10c1.519 0 2.869-.633 3.44-1.738a.495.495 0 0 1 .668-.212zm.792-1.826a.477.477 0 0 1-.119.692.541.541 0 0 1-.31.084.534.534 0 0 1-.428-.194c-.106-.138-.238-.306-.539-.306-.298 0-.431.168-.54.307A.534.534 0 0 1 9.538 7a.544.544 0 0 1-.31-.084.463.463 0 0 1-.117-.694c.33-.423.742-.722 1.394-.722.653 0 1.068.3 1.396.724zm-7 0a.477.477 0 0 1-.119.692.541.541 0 0 1-.31.084.534.534 0 0 1-.428-.194c-.106-.138-.238-.306-.539-.306-.299 0-.432.168-.54.307A.533.533 0 0 1 2.538 7a.544.544 0 0 1-.31-.084.463.463 0 0 1-.117-.694c.33-.423.742-.722 1.394-.722.653 0 1.068.3 1.396.724zM7 0a7 7 0 1 1 0 14A7 7 0 0 1 7 0zm4.243 11.243A5.96 5.96 0 0 0 13 7a5.96 5.96 0 0 0-1.757-4.243A5.96 5.96 0 0 0 7 1a5.96 5.96 0 0 0-4.243 1.757A5.96 5.96 0 0 0 1 7a5.96 5.96 0 0 0 1.757 4.243A5.96 5.96 0 0 0 7 13a5.96 5.96 0 0 0 4.243-1.757z",
        fillRule: "evenodd"
      }))), React__default.createElement(reactFileUtils.FileUploadButton, {
        multiple: this.props.multipleUploads,
        disabled: this.props.numberOfUploads >= this.props.maxNumberOfFiles ? true : false,
        accepts: this.props.acceptedFiles,
        handleFiles: this.props.uploadNewFiles
      }, React__default.createElement("span", {
        className: "str-chat__small-message-input-fileupload",
        onClick: this.props.openFilePanel
      }, React__default.createElement("svg", {
        width: "14",
        height: "14",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        d: "M7 .5c3.59 0 6.5 2.91 6.5 6.5s-2.91 6.5-6.5 6.5S.5 10.59.5 7 3.41.5 7 .5zm0 12c3.031 0 5.5-2.469 5.5-5.5S10.031 1.5 7 1.5A5.506 5.506 0 0 0 1.5 7c0 3.034 2.469 5.5 5.5 5.5zM7.506 3v3.494H11v1.05H7.506V11h-1.05V7.544H3v-1.05h3.456V3h1.05z",
        fillRule: "nonzero"
      })))), SendButton && React__default.createElement(SendButton, {
        sendMessage: this.props.handleSubmit
      })))));
    }
  }]);

  return MessageInputSmall;
}(React.PureComponent);

_defineProperty(exports.MessageInputSmall, "propTypes", {
  /** Set focus to the text input if this is enabled */
  focus: PropTypes.bool.isRequired,

  /** Grow the textarea while you're typing */
  grow: PropTypes.bool.isRequired,

  /** Specify the max amount of rows the textarea is able to grow */
  maxRows: PropTypes.number.isRequired,

  /** Make the textarea disabled */
  disabled: PropTypes.bool,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  imageOrder: PropTypes.array,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  imageUploads: PropTypes.object,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  removeImage: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  uploadImage: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  uploadNewFiles: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  numberOfUploads: PropTypes.number,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  fileOrder: PropTypes.array,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  fileUploads: PropTypes.object,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  removeFile: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  uploadFile: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  emojiPickerIsOpen: PropTypes.bool,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  emojiPickerRef: PropTypes.object,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  onSelectEmoji: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  getUsers: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  getCommands: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  textareaRef: PropTypes.object,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  handleSubmit: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  handleChange: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  onSelectItem: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  text: PropTypes.string,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  onPaste: PropTypes.func,

  /** @see See [MessageInput](https://getstream.github.io/stream-chat-react/#messageinput) for doc */
  openEmojiPicker: PropTypes.func,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  watcher_count: PropTypes.number,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  typing: PropTypes.object,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  multipleUploads: PropTypes.object,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  maxNumberOfFiles: PropTypes.object,

  /** @see See [channel context](https://getstream.github.io/stream-chat-react/#channel) doc */
  acceptedFiles: PropTypes.object
});

exports.MessageInputSmall = reactIntl.injectIntl(exports.MessageInputSmall);

exports.ChannelSearch =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(ChannelSearch, _React$PureComponent);

  function ChannelSearch() {
    _classCallCheck(this, ChannelSearch);

    return _possibleConstructorReturn(this, _getPrototypeOf(ChannelSearch).apply(this, arguments));
  }

  _createClass(ChannelSearch, [{
    key: "render",
    value: function render() {
      return React__default.createElement("div", {
        className: "str-chat__channel-search"
      }, React__default.createElement("input", {
        type: "text",
        placeholder: intl.formatMessage({
          id: 'channel_search.placeholder',
          defaultMessage: 'Search'
        })
      }), React__default.createElement("button", {
        type: "submit"
      }, React__default.createElement("svg", {
        width: "18",
        height: "17",
        viewBox: "0 0 18 17",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        d: "M0 17.015l17.333-8.508L0 0v6.617l12.417 1.89L0 10.397z",
        fillRule: "evenodd"
      }))));
    }
  }]);

  return ChannelSearch;
}(React__default.PureComponent);

exports.ChannelSearch = reactIntl.injectIntl(exports.ChannelSearch);

/**
 * Used as preview component for channel item in [ChannelList](#channellist) component.
 *
 * @example ./docs/ChannelPreviewLastMessage.md
 * @extends PureComponent
 */

var ChannelPreviewLastMessage =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(ChannelPreviewLastMessage, _PureComponent);

  function ChannelPreviewLastMessage() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, ChannelPreviewLastMessage);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(ChannelPreviewLastMessage)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "channelPreviewButton", React__default.createRef());

    _defineProperty(_assertThisInitialized(_this), "onSelectChannel", function () {
      _this.props.setActiveChannel(_this.props.channel, _this.props.watchers);

      _this.channelPreviewButton.current.blur();
    });

    return _this;
  }

  _createClass(ChannelPreviewLastMessage, [{
    key: "render",
    value: function render() {
      var intl = this.props.intl;
      var unreadClass = this.props.unread_count >= 1 ? 'str-chat__channel-preview--unread' : '';
      var activeClass = this.props.active ? 'str-chat__channel-preview--active' : '';
      var name = this.props.channel.data.name || this.props.channel.cid;
      return React__default.createElement("div", {
        className: "str-chat__channel-preview ".concat(unreadClass, " ").concat(activeClass)
      }, React__default.createElement("button", {
        onClick: this.onSelectChannel,
        ref: this.channelPreviewButton
      }, this.props.unread_count >= 1 && React__default.createElement("div", {
        className: "str-chat__channel-preview--dot"
      }), React__default.createElement(Avatar, {
        image: this.props.channel.data.image
      }), React__default.createElement("div", {
        className: "str-chat__channel-preview-info"
      }, React__default.createElement("span", {
        className: "str-chat__channel-preview-title"
      }, name), React__default.createElement("span", {
        className: "str-chat__channel-preview-last-message"
      }, !this.props.channel.state.messages[0] ? intl.formatMessage({
        id: 'channel_preview.latest_message.none',
        defaultMessage: 'Nothing yet...'
      }) : this.props.latestMessage), this.props.unread_count >= 1 && React__default.createElement("span", {
        className: "str-chat__channel-preview-unread-count"
      }, this.props.unread_count))));
    }
  }]);

  return ChannelPreviewLastMessage;
}(React.PureComponent);

_defineProperty(ChannelPreviewLastMessage, "propTypes", {
  /** **Available from [chat context](https://getstream.github.io/stream-chat-react/#chat)** */
  setActiveChannel: PropTypes.func,

  /** **Available from [chat context](https://getstream.github.io/stream-chat-react/#chat)** */
  channel: PropTypes.object,
  closeMenu: PropTypes.func,
  unread_count: PropTypes.number,

  /** If channel of component is active (selected) channel */
  active: PropTypes.bool,
  latestMessage: PropTypes.string
});

ChannelPreviewLastMessage = reactIntl.injectIntl(ChannelPreviewLastMessage);

var ChannelPreviewCountOnly =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(ChannelPreviewCountOnly, _PureComponent);

  function ChannelPreviewCountOnly() {
    _classCallCheck(this, ChannelPreviewCountOnly);

    return _possibleConstructorReturn(this, _getPrototypeOf(ChannelPreviewCountOnly).apply(this, arguments));
  }

  _createClass(ChannelPreviewCountOnly, [{
    key: "render",
    value: function render() {
      var unreadClass = this.props.unread >= 1 ? 'unread' : '';
      var name = this.props.channel.data.name || this.props.channel.cid;
      return React__default.createElement("div", {
        className: unreadClass
      }, React__default.createElement("button", {
        onClick: this.props.setActiveChannel.bind(this, this.props.channel)
      }, ' ', name, " ", React__default.createElement("span", null, this.props.unread)));
    }
  }]);

  return ChannelPreviewCountOnly;
}(React.PureComponent);

_defineProperty(ChannelPreviewCountOnly, "propTypes", {
  /** @see See [chat context](https://getstream.github.io/stream-chat-react/#chat) for doc */
  setActiveChannel: PropTypes.func,

  /** @see See [chat context](https://getstream.github.io/stream-chat-react/#chat) for doc */
  channel: PropTypes.object,
  unread: PropTypes.number
});

function ownKeys$6(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread$6(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys$6(source, true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys$6(source).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

var ChannelPreview =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(ChannelPreview, _PureComponent);

  function ChannelPreview(props) {
    var _this;

    _classCallCheck(this, ChannelPreview);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(ChannelPreview).call(this, props));

    _defineProperty(_assertThisInitialized(_this), "handleEvent", function (event) {
      var channel = _this.props.channel;
      var isActive = _this.props.activeChannel.cid === channel.cid;

      if (!isActive) {
        var unread = channel.countUnread(_this.state.lastRead);

        _this.setState({
          lastMessage: event.message,
          unread: unread
        });
      } else {
        _this.setState({
          lastMessage: event.message,
          unread: 0
        });
      }
    });

    _defineProperty(_assertThisInitialized(_this), "getLatestMessage", function () {
      var _this$props = _this.props,
          channel = _this$props.channel,
          intl = _this$props.intl;
      var latestMessage = channel.state.messages[channel.state.messages.length - 1];

      if (!latestMessage) {
        return intl.formatMessage({
          id: 'channel_preview.latest_message.none',
          defaultMessage: 'Nothing yet...'
        });
      }

      if (latestMessage.deleted_at) {
        return intl.formatMessage({
          id: 'channel_preview.latest_message.deleted',
          defaultMessage: 'Message deleted...'
        });
      }

      if (latestMessage.text) {
        return latestMessage.text.slice(0, 20);
      } else {
        if (latestMessage.command) {
          return intl.formatMessage({
            id: 'channel_preview.latest_message.command',
            defaultMessage: '/{command}'
          }, {
            command: latestMessage.command
          });
        }

        if (latestMessage.attachments.length) {
          return intl.formatMessage({
            id: 'channel_preview.latest_message.attachment',
            defaultMessage: 'Attachment...'
          });
        }

        return intl.formatMessage({
          id: 'channel_preview.latest_message.empty',
          defaultMessage: 'Empty message...'
        });
      }
    });

    _this.state = {
      lastMessage: {},
      unread: 0,
      lastRead: new Date()
    };
    return _this;
  }

  _createClass(ChannelPreview, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      // listen to change...
      var channel = this.props.channel;
      var unread = channel.countUnread();
      this.setState({
        unread: unread
      });
      channel.on('message.new', this.handleEvent);
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      var channel = this.props.channel;
      channel.off('message.new', this.handleEvent);
    }
  }, {
    key: "componentDidUpdate",
    value: function componentDidUpdate(prevProps) {
      if (this.props.activeChannel.cid !== prevProps.activeChannel.cid) {
        var isActive = this.props.activeChannel.cid === this.props.channel.cid;

        if (isActive) {
          this.setState({
            unread: 0,
            lastRead: new Date()
          });
        }
      }
    }
  }, {
    key: "render",
    value: function render() {
      var props = _objectSpread$6({}, this.state, {}, this.props);

      var Preview = this.props.Preview;
      return React__default.createElement(Preview, _extends({}, props, {
        latestMessage: this.getLatestMessage(),
        active: this.props.activeChannel.cid === this.props.channel.cid
      }));
    }
  }]);

  return ChannelPreview;
}(React.PureComponent);

_defineProperty(ChannelPreview, "propTypes", {
  channel: PropTypes.object.isRequired,
  activeChannel: PropTypes.object.isRequired,
  setActiveChannel: PropTypes.func.isRequired,
  Preview: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),
  watchers: PropTypes.object
});

_defineProperty(ChannelPreview, "defaultProps", {
  Preview: ChannelPreviewCountOnly
});

ChannelPreview = reactIntl.injectIntl(ChannelPreview);

var LoadMoreButton =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(LoadMoreButton, _React$PureComponent);

  function LoadMoreButton() {
    _classCallCheck(this, LoadMoreButton);

    return _possibleConstructorReturn(this, _getPrototypeOf(LoadMoreButton).apply(this, arguments));
  }

  _createClass(LoadMoreButton, [{
    key: "render",
    value: function render() {
      return React__default.createElement("div", {
        className: "str-chat__load-more-button"
      }, React__default.createElement("button", {
        className: "str-chat__load-more-button__button",
        onClick: this.props.onClick,
        disabled: this.props.refreshing
      }, this.props.refreshing ? React__default.createElement(reactFileUtils.LoadingIndicator, null) : this.props.children));
    }
  }]);

  return LoadMoreButton;
}(React__default.PureComponent);

_defineProperty(LoadMoreButton, "propTypes", {
  onClick: PropTypes.func
});

_defineProperty(LoadMoreButton, "defaultProps", {
  children: 'Load more'
});

var LoadMorePaginator =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(LoadMorePaginator, _React$PureComponent);

  function LoadMorePaginator() {
    _classCallCheck(this, LoadMorePaginator);

    return _possibleConstructorReturn(this, _getPrototypeOf(LoadMorePaginator).apply(this, arguments));
  }

  _createClass(LoadMorePaginator, [{
    key: "render",
    value: function render() {
      return React.createElement(React.Fragment, null, !this.props.reverse && this.props.children, this.props.hasNextPage ? smartRender(this.props.LoadMoreButton, {
        refreshing: this.props.refreshing,
        onClick: this.props.loadNextPage
      }) : null, this.props.reverse && this.props.children);
    }
  }]);

  return LoadMorePaginator;
}(React.PureComponent);

_defineProperty(LoadMorePaginator, "propTypes", {
  LoadMoreButton: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /** callback to load the next page */
  loadNextPage: PropTypes.func,

  /** indicates if there is a next page to load */
  hasNextPage: PropTypes.bool,

  /** display the items in opposite order */
  reverse: PropTypes.bool
});

_defineProperty(LoadMorePaginator, "defaultProps", {
  LoadMoreButton: LoadMoreButton
});

/**
 * LoadingChannels - Fancy loading indicator for the channel list
 *
 * @example ./docs/LoadingChannels.md
 */

var LoadingChannels = function LoadingChannels() {
  return React__default.createElement("div", {
    className: "str-chat__loading-channels"
  }, React__default.createElement("div", {
    className: "str-chat__loading-channels-item"
  }, React__default.createElement("div", {
    className: "str-chat__loading-channels-avatar"
  }), React__default.createElement("div", {
    className: "str-chat__loading-channels-meta"
  }, React__default.createElement("div", {
    className: "str-chat__loading-channels-username"
  }), React__default.createElement("div", {
    className: "str-chat__loading-channels-status"
  }))), React__default.createElement("div", {
    className: "str-chat__loading-channels-item"
  }, React__default.createElement("div", {
    className: "str-chat__loading-channels-avatar"
  }), React__default.createElement("div", {
    className: "str-chat__loading-channels-meta"
  }, React__default.createElement("div", {
    className: "str-chat__loading-channels-username"
  }), React__default.createElement("div", {
    className: "str-chat__loading-channels-status"
  }))), React__default.createElement("div", {
    className: "str-chat__loading-channels-item"
  }, React__default.createElement("div", {
    className: "str-chat__loading-channels-avatar"
  }), React__default.createElement("div", {
    className: "str-chat__loading-channels-meta"
  }, React__default.createElement("div", {
    className: "str-chat__loading-channels-username"
  }), React__default.createElement("div", {
    className: "str-chat__loading-channels-status"
  }))));
};

var placeholder = "data:image/svg+xml,%3C%3Fxml%20version%3D%221.0%22%20encoding%3D%22UTF-8%22%3F%3E%3Csvg%20width%3D%2278px%22%20height%3D%2278px%22%20viewBox%3D%220%200%2078%2078%22%20version%3D%221.1%22%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20xmlns%3Axlink%3D%22http%3A%2F%2Fwww.w3.org%2F1999%2Fxlink%22%3E%20%20%20%20%20%20%20%20%3Ctitle%3ECombined%20Shape%3C%2Ftitle%3E%20%20%20%20%3Cdesc%3ECreated%20with%20Sketch.%3C%2Fdesc%3E%20%20%20%20%3Cg%20id%3D%22Interactions%22%20stroke%3D%22none%22%20stroke-width%3D%221%22%20fill%3D%22none%22%20fill-rule%3D%22evenodd%22%3E%20%20%20%20%20%20%20%20%3Cg%20id%3D%22Connection-Error-_-Connectivity%22%20transform%3D%22translate%28-270.000000%2C%20-30.000000%29%22%20fill%3D%22%23CF1F25%22%3E%20%20%20%20%20%20%20%20%20%20%20%20%3Cg%20id%3D%22109-network-connection%22%20transform%3D%22translate%28270.000000%2C%2030.000000%29%22%3E%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%3Cpath%20d%3D%22M66.4609744%2C11.414231%20C81.6225232%2C26.5757798%2081.6225232%2C51.157545%2066.4609744%2C66.3188467%20C51.2994256%2C81.4803954%2026.7176604%2C81.4803954%2011.5563587%2C66.3188467%20C-3.60519004%2C51.1572979%20-3.60519004%2C26.5755327%2011.5563587%2C11.414231%20C26.7179075%2C-3.74731776%2051.2996727%2C-3.74731776%2066.4609744%2C11.414231%20Z%20M54.7853215%2C45.8823776%20L54.7853215%2C40.5882574%20C54.7853215%2C39.613638%2053.9952341%2C38.8235506%2053.0206147%2C38.8235506%20L44.9576695%2C38.8235506%20L41.428256%2C42.3529641%20L51.255555%2C42.3529641%20L51.255555%2C45.8823776%20L54.7853215%2C45.8823776%20Z%20M40.6659027%2C43.1153174%20L37.8988425%2C45.8823776%20L40.6659027%2C45.8823776%20L40.6659027%2C43.1153174%20Z%20M51.1764962%2C56.4702653%20L58.2353232%2C56.4702653%20C59.2099355%2C56.4702653%2060.00003%2C55.6801708%2060.00003%2C54.7055585%20L60.00003%2C51.176145%20C60.00003%2C50.2015327%2059.2099355%2C49.4114382%2058.2353232%2C49.4114382%20L51.1764962%2C49.4114382%20C50.2018839%2C49.4114382%2049.4117894%2C50.2015327%2049.4117894%2C51.176145%20L49.4117894%2C54.7055585%20C49.4117894%2C55.6801708%2050.2018839%2C56.4702653%2051.1764962%2C56.4702653%20Z%20M35.2941353%2C56.4702653%20L42.3529624%2C56.4702653%20C43.3275746%2C56.4702653%2044.1176691%2C55.6801708%2044.1176691%2C54.7055585%20L44.1176691%2C51.176145%20C44.1176691%2C50.2015327%2043.3275746%2C49.4114382%2042.3529624%2C49.4114382%20L35.2941353%2C49.4114382%20C34.319523%2C49.4114382%2033.5294285%2C50.2015327%2033.5294285%2C51.176145%20L33.5294285%2C54.7055585%20C33.5294285%2C55.6801708%2034.319523%2C56.4702653%2035.2941353%2C56.4702653%20Z%20M56.6964989%2C19.0874231%20C56.007381%2C18.3985134%2054.8903216%2C18.3985134%2054.2012036%2C19.087423%20L45.882376%2C27.4062507%20L45.882376%2C19.4117761%20C45.882376%2C18.4371568%2045.0922885%2C17.6470693%2044.1176692%2C17.6470693%20L33.5294286%2C17.6470693%20C32.5548092%2C17.6470694%2031.7647218%2C18.4371568%2031.7647218%2C19.4117761%20L31.7647218%2C30.0000167%20C31.7647219%2C30.9746363%2032.5548092%2C31.7647237%2033.5294285%2C31.7647237%20L41.5239031%2C31.7647237%20L34.4650761%2C38.8235508%20L24.7058947%2C38.8235508%20C23.7312753%2C38.8235508%2022.9411879%2C39.6136382%2022.9411879%2C40.5882575%20L22.9411879%2C45.8823778%20L26.4706014%2C45.8823778%20L26.4706014%2C42.3529643%20L30.9356624%2C42.3529643%20L23.8768354%2C49.4117914%20L19.4117743%2C49.4117914%20C18.4371549%2C49.4117914%2017.6470675%2C50.2018788%2017.6470675%2C51.1764981%20L17.6470675%2C54.7059117%20C17.6504049%2C54.9674302%2017.7129076%2C55.2248042%2017.8298886%2C55.4587302%20L16.4456526%2C56.8429662%20C15.7446193%2C57.5200453%2015.7252005%2C58.6372282%2016.4022825%2C59.3382615%20C17.0793616%2C60.0392948%2018.1965445%2C60.0587136%2018.8975778%2C59.3816316%20C18.9122847%2C59.3674273%2018.9267436%2C59.3529684%2018.940948%2C59.3382615%20L56.6964963%2C21.5830662%20C57.3856425%2C20.8939094%2057.3856425%2C19.7765747%2056.6964963%2C19.0874179%20Z%22%20id%3D%22Combined-Shape%22%3E%3C%2Fpath%3E%20%20%20%20%20%20%20%20%20%20%20%20%3C%2Fg%3E%20%20%20%20%20%20%20%20%3C%2Fg%3E%20%20%20%20%3C%2Fg%3E%3C%2Fsvg%3E";

/**
 * ChatDown - Indicator that chat is down or your network isn't working
 *
 * @example ./docs/ChatDown.md
 * @extends PureComponent
 */

var ChatDown =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(ChatDown, _React$PureComponent);

  function ChatDown() {
    _classCallCheck(this, ChatDown);

    return _possibleConstructorReturn(this, _getPrototypeOf(ChatDown).apply(this, arguments));
  }

  _createClass(ChatDown, [{
    key: "render",
    value: function render() {
      var image = this.props.image;
      return React__default.createElement("div", {
        className: "str-chat__down"
      }, React__default.createElement(LoadingChannels, null), React__default.createElement("div", {
        className: "str-chat__down-main"
      }, React__default.createElement("img", {
        src: image
      }), React__default.createElement("h1", null, React__default.createElement(reactIntl.FormattedMessage, {
        id: "chat_down.connection_error",
        defaultMessage: "Connection Error"
      })), React__default.createElement("h3", null, React__default.createElement(reactIntl.FormattedMessage, {
        id: "chat_down.connection_error_text",
        defaultMessage: "Error connecting to chat, refresh the page to try again."
      }))));
    }
  }]);

  return ChatDown;
}(React__default.PureComponent);

_defineProperty(ChatDown, "propTypes", {
  /** The image url for this error */
  image: PropTypes.string
});

_defineProperty(ChatDown, "defaultProps", {
  image: placeholder
});

function ownKeys$7(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread$7(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys$7(source, true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys$7(source).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }
/**
 * ChannelList - A preview list of channels, allowing you to select the channel you want to open
 * @example ./examples/ChannelList.md
 */

exports.ChannelListTeam =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(ChannelListTeam, _PureComponent);

  function ChannelListTeam(props) {
    var _this;

    _classCallCheck(this, ChannelListTeam);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(ChannelListTeam).call(this, props));

    _defineProperty(_assertThisInitialized(_this), "SortingSelect", function () {
      return React__default.createElement("div", {
        className: "sortingSelect"
      }, React__default.createElement("select", {
        className: "str-chat__channel-list-team__header--button",
        id: "flexgrow",
        onChange: function onChange(e) {
          return _this.onSortChange(e);
        }
      }, React__default.createElement("option", {
        value: "last_message_at--new",
        label: _this.props.intl.formatMessage({
          id: 'channel_sort.last',
          defaultMessage: 'newest'
        })
      }), React__default.createElement("option", {
        value: "created_at--old",
        label: _this.props.intl.formatMessage({
          id: 'channel_sort.old',
          defaultMessage: 'oldest'
        })
      })));
    });

    _defineProperty(_assertThisInitialized(_this), "filterOptions", window.dplChatConfig.channelTypes.map(function (option, i) {
      return React__default.createElement("option", {
        key: i,
        value: option.value
      }, option.label);
    }));

    _defineProperty(_assertThisInitialized(_this), "FilterSelect", function () {
      return React__default.createElement("div", {
        className: "filterselect"
      }, React__default.createElement("select", {
        className: "str-chat__channel-list-team__header--button",
        id: "filterselect",
        onChange: function onChange(e) {
          return _this.onFilterChange(e);
        }
      }, _this.filterOptions));
    });

    _defineProperty(_assertThisInitialized(_this), "AdminFilterSelect", function () {
      return React__default.createElement("div", {
        className: "filterselect"
      }, React__default.createElement("select", {
        className: "str-chat__channel-list-team__header--button",
        id: "adminfilterselect",
        onChange: function onChange(e) {
          return _this.onAdminFilterChange(e);
        }
      }, React__default.createElement("option", {
        value: "my",
        label: _this.props.intl.formatMessage({
          id: 'channel_filter.my',
          defaultMessage: 'my'
        })
      }), React__default.createElement("option", {
        value: "all",
        label: _this.props.intl.formatMessage({
          id: 'channel_filter.all',
          defaultMessage: 'all'
        })
      }), React__default.createElement("option", {
        value: "me",
        label: _this.props.intl.formatMessage({
          id: 'channel_filter.me',
          defaultMessage: 'assgined to: me'
        })
      }), React__default.createElement("option", {
        value: "no",
        label: _this.props.intl.formatMessage({
          id: 'channel_filter.no',
          defaultMessage: 'assgined to: none'
        })
      }), React__default.createElement("option", {
        value: "other",
        label: _this.props.intl.formatMessage({
          id: 'channel_filter.other',
          defaultMessage: 'assgined to: others'
        })
      })));
    });

    _defineProperty(_assertThisInitialized(_this), "Button", function (_ref) {
      var open = _ref.open,
          onClick = _ref.onClick;
      return React__default.createElement("div", {
        onClick: onClick
      }, open ? React__default.createElement("div", null, React__default.createElement("svg", {
        id: "style-channel-search-icon",
        width: "18",
        height: "18",
        viewBox: "0 0 18 18",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        stroke: "black",
        d: "M 19.333 2.547 l -1.88 -1.88 L 10 8.12 L 2.547 0.667 l -1.88 1.88 L 8.12 10 L 0.667 17.453 l 1.88 1.88 L 10 11.88 l 7.453 7.453 l 1.88 -1.88 L 11.88 10 Z"
      }))) : React__default.createElement("div", null, React__default.createElement("svg", {
        id: "style-channel-search-icon",
        width: "18",
        height: "17",
        viewBox: "0 0 18 17",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        stroke: "black",
        d: "M18.125,15.804l-4.038-4.037c0.675-1.079,1.012-2.308,1.01-3.534C15.089,4.62,12.199,1.75,8.584,1.75C4.815,1.75,1.982,4.726,2,8.286c0.021,3.577,2.908,6.549,6.578,6.549c1.241,0,2.417-0.347,3.44-0.985l4.032,4.026c0.167,0.166,0.43,0.166,0.596,0l1.479-1.478C18.292,16.234,18.292,15.968,18.125,15.804 M8.578,13.99c-3.198,0-5.716-2.593-5.733-5.71c-0.017-3.084,2.438-5.686,5.74-5.686c3.197,0,5.625,2.493,5.64,5.624C14.242,11.548,11.621,13.99,8.578,13.99 M16.349,16.981l-3.637-3.635c0.131-0.11,0.721-0.695,0.876-0.884l3.642,3.639L16.349,16.981z"
      }))));
    });

    _defineProperty(_assertThisInitialized(_this), "ChevronButton", function (_ref2) {
      var open = _ref2.open,
          onClick = _ref2.onClick;
      return React__default.createElement("div", {
        onClick: onClick
      }, open ? React__default.createElement("div", null, React__default.createElement("svg", {
        id: "style-channel-admin-control-toggle",
        width: "16",
        height: "16",
        viewBox: "0 0 448 512",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        stroke: "black",
        d: "M436 192H312c-13.3 0-24-10.7-24-24V44c0-6.6 5.4-12 12-12h40c6.6 0 12 5.4 12 12v84h84c6.6 0 12 5.4 12 12v40c0 6.6-5.4 12-12 12zm-276-24V44c0-6.6-5.4-12-12-12h-40c-6.6 0-12 5.4-12 12v84H12c-6.6 0-12 5.4-12 12v40c0 6.6 5.4 12 12 12h124c13.3 0 24-10.7 24-24zm0 300V344c0-13.3-10.7-24-24-24H12c-6.6 0-12 5.4-12 12v40c0 6.6 5.4 12 12 12h84v84c0 6.6 5.4 12 12 12h40c6.6 0 12-5.4 12-12zm192 0v-84h84c6.6 0 12-5.4 12-12v-40c0-6.6-5.4-12-12-12H312c-13.3 0-24 10.7-24 24v124c0 6.6 5.4 12 12 12h40c6.6 0 12-5.4 12-12z"
      }))) : React__default.createElement("div", null, React__default.createElement("svg", {
        id: "style-channel-admin-control-toggle",
        width: "16",
        height: "16",
        viewBox: "0 0 448 512",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        stroke: "black",
        d: "M0 180V56c0-13.3 10.7-24 24-24h124c6.6 0 12 5.4 12 12v40c0 6.6-5.4 12-12 12H64v84c0 6.6-5.4 12-12 12H12c-6.6 0-12-5.4-12-12zM288 44v40c0 6.6 5.4 12 12 12h84v84c0 6.6 5.4 12 12 12h40c6.6 0 12-5.4 12-12V56c0-13.3-10.7-24-24-24H300c-6.6 0-12 5.4-12 12zm148 276h-40c-6.6 0-12 5.4-12 12v84h-84c-6.6 0-12 5.4-12 12v40c0 6.6 5.4 12 12 12h124c13.3 0 24-10.7 24-24V332c0-6.6-5.4-12-12-12zM160 468v-40c0-6.6-5.4-12-12-12H64v-84c0-6.6-5.4-12-12-12H12c-6.6 0-12 5.4-12 12v124c0 13.3 10.7 24 24 24h124c6.6 0 12-5.4 12-12z"
      }))));
    });

    _defineProperty(_assertThisInitialized(_this), "ClearButton", function (_ref3) {
      var open = _ref3.open,
          onClick = _ref3.onClick;
      return React__default.createElement("div", {
        onClick: onClick
      }, open ? React__default.createElement("div", null, React__default.createElement("svg", {
        id: "style-channel-filter-clear-icon",
        width: "18",
        height: "18",
        viewBox: "0 0 18 18",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        stroke: "black",
        d: "M 19.333 2.547 l -1.88 -1.88 L 10 8.12 L 2.547 0.667 l -1.88 1.88 L 8.12 10 L 0.667 17.453 l 1.88 1.88 L 10 11.88 l 7.453 7.453 l 1.88 -1.88 L 11.88 10 Z"
      }))) : React__default.createElement("div", null));
    });

    _defineProperty(_assertThisInitialized(_this), "SearchBar", function () {
      var intl = _this.props.intl;
      return React__default.createElement("div", {
        id: "style-search-bar"
      }, React__default.createElement("div", {
        className: "str-chat__channel-search",
        id: "style-channel-search"
      }, React__default.createElement("div", {
        id: "style-channel-search-input"
      }, React__default.createElement("input", {
        id: "searchbar",
        className: "searchInput",
        placeholder: intl.formatMessage({
          id: 'channel_search.placeholder',
          defaultMessage: 'Search'
        }),
        value: _this.state.searchInput || '',
        type: "text",
        name: "inputSearch",
        onChange: _this.handleChange,
        onKeyDown: _this.handleSearch.bind(_assertThisInitialized(_this))
      }))));
    });

    _defineProperty(_assertThisInitialized(_this), "toggleAdminControl",
    /*#__PURE__*/
    _asyncToGenerator(
    /*#__PURE__*/
    _regeneratorRuntime.mark(function _callee() {
      var mergedFilter;
      return _regeneratorRuntime.wrap(function _callee$(_context) {
        while (1) {
          switch (_context.prev = _context.next) {
            case 0:
              if (!_this.state.adminControlOpen) {
                _context.next = 7;
                break;
              }

              _context.next = 3;
              return _this.setState({
                adminControlOpen: false,
                adminFilter: {
                  members: {
                    $in: [_this.props.client.user.id]
                  }
                }
              });

            case 3:
              mergedFilter = _objectSpread$7({}, _this.state.filter, {}, _this.state.adminFilter); // console.log(mergedFilter);

              _this.props.onSelectFilter(mergedFilter);

              _context.next = 8;
              break;

            case 7:
              _this.setState({
                adminControlOpen: true
              });

            case 8:
            case "end":
              return _context.stop();
          }
        }
      }, _callee);
    })));

    _defineProperty(_assertThisInitialized(_this), "clearFilterControl",
    /*#__PURE__*/
    _asyncToGenerator(
    /*#__PURE__*/
    _regeneratorRuntime.mark(function _callee2() {
      var _mergedFilter, filter, mergedFilter;

      return _regeneratorRuntime.wrap(function _callee2$(_context2) {
        while (1) {
          switch (_context2.prev = _context2.next) {
            case 0:
              if (!(_this.props.client.user.role === 'admin')) {
                _context2.next = 6;
                break;
              }

              if (!_this.state.adminControlOpen) {
                _context2.next = 6;
                break;
              }

              _context2.next = 4;
              return _this.setState({
                adminControlOpen: false,
                adminFilter: {
                  members: {
                    $in: [_this.props.client.user.id]
                  }
                }
              });

            case 4:
              _mergedFilter = _objectSpread$7({}, _this.state.filter, {}, _this.state.adminFilter);

              _this.props.onSelectFilter(_mergedFilter);

            case 6:
              // default user filter: MY
              filter = {
                members: {
                  $in: [_this.props.client.user.id]
                }
              }; // Merge filter

              mergedFilter = _objectSpread$7({}, filter, {}, _this.state.filter); // console.log(mergedFilter);
              // Set Filter

              _this.props.onSelectFilter(mergedFilter);

            case 9:
            case "end":
              return _context2.stop();
          }
        }
      }, _callee2);
    })));

    _defineProperty(_assertThisInitialized(_this), "toggleChannelSearch", function (event) {
      if (_this.state.searchOpen) {
        _this.props.setActiveSearch('', event);

        _this.props.resetSearch();

        _this.setState({
          searchOpen: false,
          searchInput: ''
        });
      } else {
        _this.setState({
          searchOpen: true
        });

        window.dplChatConfig.language = '';
        window.dispatchEvent(languageChangedEvent);
        localStorage.setItem('language', window.dplChatConfig.language);

        _this.setState({
          disableSort: 'true',
          searchOpen: true
        });

        _this.search(_this.state.searchInput, _this.props.client);

        _this.props.setActiveSearch(_this.state.searchInput, event);
      }
    });

    _this.state = {
      searchOpen: false,
      searchInput: '',
      filter: {
        type: 'messaging'
      },
      adminFilter: {
        members: {
          $in: [_this.props.client.user.id]
        }
      },
      sort: {},
      inputMembers: [],
      inputExtraTypes: [],
      disableSort: 'false',
      adminControlOpen: false
    };
    _this.handleChange = _this.handleChange.bind(_assertThisInitialized(_this));
    return _this;
  }

  _createClass(ChannelListTeam, [{
    key: "onAdminFilterChange",
    value: function () {
      var _onAdminFilterChange = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee3(event) {
        var value, adminFilter, mergedFilter;
        return _regeneratorRuntime.wrap(function _callee3$(_context3) {
          while (1) {
            switch (_context3.prev = _context3.next) {
              case 0:
                value = event.target.value;
                this.setState({
                  searchInput: ''
                });
                this.props.setActiveSearch('', event);
                adminFilter = {};
                _context3.t0 = value;
                _context3.next = _context3.t0 === 'all' ? 7 : _context3.t0 === 'my' ? 8 : _context3.t0 === 'no' ? 10 : _context3.t0 === 'me' ? 12 : _context3.t0 === 'other' ? 14 : 17;
                break;

              case 7:
                return _context3.abrupt("break", 19);

              case 8:
                adminFilter.members = {
                  $in: [this.props.client.user.id]
                };
                return _context3.abrupt("break", 19);

              case 10:
                adminFilter.assignFlag = false;
                return _context3.abrupt("break", 19);

              case 12:
                adminFilter.responsiveMember = {
                  $eq: this.props.client.user.name
                };
                return _context3.abrupt("break", 19);

              case 14:
                adminFilter.assignFlag = true;
                adminFilter.responsiveMember = {
                  $ne: this.props.client.user.name
                };
                return _context3.abrupt("break", 19);

              case 17:
                adminFilter.members = {
                  $in: [this.props.client.user.id]
                };
                return _context3.abrupt("break", 19);

              case 19:
                mergedFilter = {};

                if (!(this.state.filter !== {})) {
                  _context3.next = 26;
                  break;
                }

                _context3.next = 23;
                return this.setState({
                  adminFilter: adminFilter
                });

              case 23:
                mergedFilter = _objectSpread$7({}, this.state.filter, {}, this.state.adminFilter); // console.log(mergedFilter);

                _context3.next = 29;
                break;

              case 26:
                _context3.next = 28;
                return this.setState(_objectSpread$7({}, adminFilter, {
                  filter: {
                    type: 'messaging'
                  }
                }));

              case 28:
                mergedFilter = _objectSpread$7({}, this.state.filter, {}, this.state.adminFilter); // console.log(mergedFilter);

              case 29:
                // filter = extraType ? (filter.extraType = { $eq: event.target.value }) : {};
                // // this.setState({ filter });
                this.props.onSelectFilter(mergedFilter);

              case 30:
              case "end":
                return _context3.stop();
            }
          }
        }, _callee3, this);
      }));

      function onAdminFilterChange(_x) {
        return _onAdminFilterChange.apply(this, arguments);
      }

      return onAdminFilterChange;
    }()
  }, {
    key: "onFilterChange",
    value: function () {
      var _onFilterChange = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee4(event) {
        var filterType, filter, mergedFilter;
        return _regeneratorRuntime.wrap(function _callee4$(_context4) {
          while (1) {
            switch (_context4.prev = _context4.next) {
              case 0:
                filterType = event.target.value;
                this.setState({
                  searchInput: ''
                });
                this.props.setActiveSearch('', event);
                filter = {};

                if (this.props.client.user.role !== 'admin') {
                  filter.members = {
                    $in: [this.props.client.user.id]
                  };
                  filterType ? filter.extraType = {
                    $eq: filterType
                  } : {};
                } else {
                  filterType ? filter = {
                    // members: { $in: [this.props.client.user.id] },
                    extraType: {
                      $eq: filterType
                    }
                  } : {};
                } // console.log(filter);


                mergedFilter = {};

                if (!(this.state.adminFilter !== {})) {
                  _context4.next = 12;
                  break;
                }

                _context4.next = 9;
                return this.setState({
                  filter: filter
                });

              case 9:
                mergedFilter = _objectSpread$7({}, this.state.filter, {}, this.state.adminFilter); // console.log(mergedFilter);

                _context4.next = 15;
                break;

              case 12:
                _context4.next = 14;
                return this.setState({
                  filter: filter,
                  adminFilter: {
                    members: {
                      $in: [this.props.client.user.id]
                    }
                  }
                });

              case 14:
                mergedFilter = _objectSpread$7({}, this.state.filter, {}, this.state.adminFilter); // console.log(mergedFilter);

              case 15:
                this.props.onSelectFilter(mergedFilter);

              case 16:
              case "end":
                return _context4.stop();
            }
          }
        }, _callee4, this);
      }));

      function onFilterChange(_x2) {
        return _onFilterChange.apply(this, arguments);
      }

      return onFilterChange;
    }()
  }, {
    key: "onSortChange",
    value: function onSortChange(event) {
      this.setState({
        searchInput: ''
      });
      this.props.setActiveSearch('', event);
      var sort = {};

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

      this.setState({
        sort: sort
      });
      this.props.onSelectSort(sort);
    }
  }, {
    key: "render",
    value: function render() {
      var showSidebar = this.props.showSidebar;

      if (this.props.error) {
        return React__default.createElement(ChatDown, null);
      } else if (this.props.loading) {
        return React__default.createElement(LoadingChannels, null);
      } else {
        return React__default.createElement("div", {
          className: "str-chat__channel-list-team"
        }, showSidebar && React__default.createElement("div", {
          className: "str-chat__channel-list-team__sidebar"
        }, React__default.createElement("div", {
          className: "str-chat__channel-list-team__sidebar--top"
        }, React__default.createElement(Avatar, {
          image: "https://cdn.dribbble.com/users/610788/screenshots/5157282/spacex.png",
          size: 50
        }))), React__default.createElement("div", {
          className: "str-chat__channel-list-team__main"
        }, React__default.createElement("div", {
          className: "str-chat__channel-list-team__header"
        }, React__default.createElement("div", {
          className: "str-chat__channel-list-team__header--left"
        }, React__default.createElement(Avatar, {
          source: this.props.client.user.image,
          name: this.props.client.user.name || this.props.client.user.id,
          size: 40
        })), React__default.createElement("div", {
          className: "str-chat__channel-list-team__header--middle"
        }, React__default.createElement("div", {
          className: "str-chat__channel-list-team__header--title"
        }, this.props.client.user.name || this.props.client.user.id), React__default.createElement("div", {
          className: "str-chat__channel-list-team__header--status ".concat(this.props.client.user.status)
        }, this.props.client.user.status))), React__default.createElement("div", {
          id: "style-channellist-first-row"
        }, React__default.createElement("div", {
          id: "style-channel-selection"
        }, React__default.createElement(this.SortingSelect, null), React__default.createElement(this.FilterSelect, null)), this.props.client.user.role === 'admin' ? React__default.createElement("div", {
          className: "channel-list-admin-icons tooltip"
        }, this.state.adminControlOpen ? React__default.createElement("span", {
          className: "tooltiptext"
        }, "Filter f\xFCr Mitarbeiter entfernen") : React__default.createElement("span", {
          className: "tooltiptext"
        }, "Filter f\xFCr Mitarbeiter anzeigen"), React__default.createElement(this.ChevronButton, {
          onClick: this.toggleAdminControl,
          open: this.state.adminControlOpen
        })) : React__default.createElement("div", null)), React__default.createElement("div", {
          id: "style-channellist-secound-row"
        }, this.props.client.user.role === 'admin' && this.state.adminControlOpen ? React__default.createElement("div", {
          className: "channel-list-admin-filter"
        }, React__default.createElement(this.AdminFilterSelect, null)) : React__default.createElement("div", null)), React__default.createElement("div", {
          id: "style-channel-search-selection"
        }, React__default.createElement(this.SearchBar, null), React__default.createElement(this.Button, {
          onClick: this.toggleChannelSearch,
          open: this.state.searchOpen
        })), React__default.createElement("div", {
          id: "style-list",
          className: ".str-chat__channel-preview-messenger-list"
        }, this.props.children)));
      }
    }
  }, {
    key: "handleSearch",
    value: function handleSearch(event) {
      var code = event.keyCode || event.which;

      if (code === 13) {
        window.dplChatConfig.language = '';
        window.dispatchEvent(languageChangedEvent);
        localStorage.setItem('language', window.dplChatConfig.language);

        if (this.state.searchInput.replace(/\s/g, '')) {
          this.setState({
            disableSort: 'true',
            searchOpen: true
          });
          this.props.setActiveSearch(this.state.searchInput.replace(/( +)$/, ''), event);
          this.search(this.state.searchInput.replace(/( +)$/, ''), this.props.client);
        }
      }
    }
  }, {
    key: "search",
    value: function () {
      var _search = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee5(searchValue, client) {
        var channelId, response, searchResultChannels, resultChannelArray;
        return _regeneratorRuntime.wrap(function _callee5$(_context5) {
          while (1) {
            switch (_context5.prev = _context5.next) {
              case 0:
                channelId = [];

                if (this.state.filter.hasOwnProperty('extraType')) {
                  _context5.next = 4;
                  break;
                }

                _context5.next = 4;
                return this.setState({
                  filter: {
                    type: 'messaging'
                  }
                });

              case 4:
                response = [];
                searchResultChannels = [];

                if (!searchValue) {
                  _context5.next = 10;
                  break;
                }

                _context5.next = 9;
                return client.search(this.state.filter, searchValue, {
                  limit: 1000,
                  offset: 0
                });

              case 9:
                response = _context5.sent;

              case 10:
                resultChannelArray = [];

                if (response) {
                  resultChannelArray = response.results.filter(function (element) {
                    return !!element.message.channel.cid;
                  }).map(function (element) {
                    return element.message.channel;
                  });
                }

                if (resultChannelArray.length > 0) {
                  channelId.push(resultChannelArray[0].cid);
                  searchResultChannels.push(resultChannelArray[0]);
                }

                _context5.next = 15;
                return resultChannelArray.forEach(function (element) {
                  if (!channelId.includes(element.cid)) {
                    channelId.push(element.cid);
                    searchResultChannels.push(element);
                  }
                });

              case 15:
                if (searchResultChannels.length > 0) {
                  this.props.onChannelResult(searchResultChannels);
                } else {
                  this.props.onChannelResult(null);
                }

              case 16:
              case "end":
                return _context5.stop();
            }
          }
        }, _callee5, this);
      }));

      function search(_x3, _x4) {
        return _search.apply(this, arguments);
      }

      return search;
    }()
  }, {
    key: "handleChange",
    value: function () {
      var _handleChange = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee6(event) {
        return _regeneratorRuntime.wrap(function _callee6$(_context6) {
          while (1) {
            switch (_context6.prev = _context6.next) {
              case 0:
                _context6.next = 2;
                return this.setState({
                  searchInput: event.target.value
                });

              case 2:
              case "end":
                return _context6.stop();
            }
          }
        }, _callee6, this);
      }));

      function handleChange(_x5) {
        return _handleChange.apply(this, arguments);
      }

      return handleChange;
    }()
  }]);

  return ChannelListTeam;
}(React.PureComponent);

_defineProperty(exports.ChannelListTeam, "propTypes", {
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
  resetSearch: PropTypes.func
});

_defineProperty(exports.ChannelListTeam, "defaultProps", {
  error: false
});

exports.ChannelListTeam = withChatContext(reactIntl.injectIntl(exports.ChannelListTeam));

function ownKeys$8(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread$8(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys$8(source, true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys$8(source).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }
/**
 * ChannelList - A preview list of channels, allowing you to select the channel you want to open
 * @extends PureComponent
 * @example ./docs/ChannelList.md
 */

exports.ChannelList =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(ChannelList, _PureComponent);

  function ChannelList(_props) {
    var _this;

    _classCallCheck(this, ChannelList);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(ChannelList).call(this, _props));

    _defineProperty(_assertThisInitialized(_this), "queryChannels",
    /*#__PURE__*/
    _asyncToGenerator(
    /*#__PURE__*/
    _regeneratorRuntime.mark(function _callee() {
      var options, _this$state, offset, sort, filters, newOptions, channelPromise, channelQueryResponse, customActiveChannel;

      return _regeneratorRuntime.wrap(function _callee$(_context) {
        while (1) {
          switch (_context.prev = _context.next) {
            case 0:
              options = _this.props.options;
              _this$state = _this.state, offset = _this$state.offset, sort = _this$state.sort, filters = _this$state.filters;

              _this.setState({
                refreshing: true
              });

              newOptions = _objectSpread$8({}, options);
              if (!options.limit) newOptions.limit = 30;
              channelPromise = _this.props.client.queryChannels(filters, sort, _objectSpread$8({}, newOptions, {
                offset: offset
              }));
              _context.prev = 6;
              channelQueryResponse = channelPromise;

              if (!isPromise(channelQueryResponse)) {
                _context.next = 12;
                break;
              }

              _context.next = 11;
              return channelPromise;

            case 11:
              channelQueryResponse = _context.sent;

            case 12:
              _this.setState(function (prevState) {
                var channels = [].concat(_toConsumableArray(prevState.channels), _toConsumableArray(channelQueryResponse));
                return {
                  channels: channels,
                  // not unique somehow needs more checking
                  loadingChannels: false,
                  offset: channels.length,
                  hasNextPage: channelQueryResponse.length >= newOptions.limit ? true : false,
                  refreshing: false
                };
              }); // Set a channel as active and move it to the top of the list.


              if (_this.props.customActiveChannel) {
                customActiveChannel = channelQueryResponse.filter(function (channel) {
                  return channel.id === _this.props.customActiveChannel;
                })[0];

                if (customActiveChannel) {
                  _this.props.setActiveChannel(customActiveChannel, _this.props.watchers);

                  _this.moveChannelUp(customActiveChannel.cid);
                }
              } else if (offset === 0 && _this.state.channels.length >= 1) {
                _this.props.setActiveChannel(_this.state.channels[0], _this.props.watchers);
              }

              _context.next = 20;
              break;

            case 16:
              _context.prev = 16;
              _context.t0 = _context["catch"](6);
              console.warn(_context.t0);

              _this.setState({
                error: true,
                refreshing: false
              });

            case 20:
            case "end":
              return _context.stop();
          }
        }
      }, _callee, null, [[6, 16]]);
    })));

    _defineProperty(_assertThisInitialized(_this), "handleEvent",
    /*#__PURE__*/
    function () {
      var _ref2 = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee2(e) {
        var newChannels, channel, _channel, channels, channelIndex, _channels, _channelIndex;

        return _regeneratorRuntime.wrap(function _callee2$(_context2) {
          while (1) {
            switch (_context2.prev = _context2.next) {
              case 0:
                if (e.type === 'user.presence.changed') {
                  newChannels = _this.state.channels;
                  newChannels = newChannels.map(function (channel) {
                    if (!channel.state.members[e.user.id]) return channel;
                    channel.state.members.setIn([e.user.id, 'user'], e.user);
                    return channel;
                  });

                  _this.setState({
                    channels: _toConsumableArray(newChannels)
                  });
                }

                if (e.type === 'message.new') {
                  !_this.props.lockChannelOrder && _this.moveChannelUp(e.cid);
                } // make sure to re-render the channel list after connection is recovered


                if (e.type === 'connection.recovered') {
                  _this.setState(function (prevState) {
                    return {
                      connectionRecoveredCount: prevState.connectionRecoveredCount + 1
                    };
                  });
                } // move channel to start


                if (!(e.type === 'notification.message_new')) {
                  _context2.next = 12;
                  break;
                }

                if (!(_this.props.onMessageNew && typeof _this.props.onMessageNew === 'function')) {
                  _context2.next = 8;
                  break;
                }

                _this.props.onMessageNew(_assertThisInitialized(_this), e);

                _context2.next = 12;
                break;

              case 8:
                _context2.next = 10;
                return _this.getChannel(e.channel.type, e.channel.id);

              case 10:
                channel = _context2.sent;

                // move channel to starting position
                _this.setState(function (prevState) {
                  return {
                    channels: uniqBy([channel].concat(_toConsumableArray(prevState.channels)), 'cid')
                  };
                });

              case 12:
                if (!(e.type === 'notification.added_to_channel')) {
                  _context2.next = 21;
                  break;
                }

                if (!(_this.props.onAddedToChannel && typeof _this.props.onAddedToChannel === 'function')) {
                  _context2.next = 17;
                  break;
                }

                _this.props.onAddedToChannel(_assertThisInitialized(_this), e);

                _context2.next = 21;
                break;

              case 17:
                _context2.next = 19;
                return _this.getChannel(e.channel.type, e.channel.id);

              case 19:
                _channel = _context2.sent;

                _this.setState(function (prevState) {
                  return {
                    channels: uniqBy([_channel].concat(_toConsumableArray(prevState.channels)), 'cid')
                  };
                });

              case 21:
                // remove from channel
                if (e.type === 'notification.removed_from_channel') {
                  if (_this.props.onRemovedFromChannel && typeof _this.props.onRemovedFromChannel === 'function') {
                    _this.props.onRemovedFromChannel(_assertThisInitialized(_this), e);
                  } else {
                    _this.setState(function (prevState) {
                      var channels = prevState.channels.filter(function (channel) {
                        return channel.cid !== e.channel.cid;
                      });
                      return {
                        channels: channels
                      };
                    });
                  }
                } // Update the channel with data


                if (e.type === 'channel.updated') {
                  channels = _this.state.channels;
                  channelIndex = channels.findIndex(function (channel) {
                    return channel.cid === e.channel.cid;
                  });
                  channels[channelIndex].data = Immutable(e.channel);

                  _this.setState({
                    channels: _toConsumableArray(channels),
                    channelUpdateCount: _this.state.channelUpdateCount + 1
                  });

                  if (_this.props.onChannelUpdated && typeof _this.props.onChannelUpdated === 'function') {
                    _this.props.onChannelUpdated(_assertThisInitialized(_this), e);
                  }
                } // Channel is deleted


                if (e.type === 'channel.deleted') {
                  if (_this.props.onChannelDeleted && typeof _this.props.onChannelDeleted === 'function') {
                    _this.props.onChannelDeleted(_assertThisInitialized(_this), e);
                  } else {
                    _channels = _this.state.channels;
                    _channelIndex = _channels.findIndex(function (channel) {
                      return channel.cid === e.channel.cid;
                    }); // Remove the deleted channel from the list.s

                    _channels.splice(_channelIndex, 1);

                    _this.setState({
                      channels: _toConsumableArray(_channels),
                      channelUpdateCount: _this.state.channelUpdateCount + 1
                    });
                  }
                }

                if (e.type === 'channel.truncated') {
                  _this.setState(function (prevState) {
                    return {
                      channels: _toConsumableArray(prevState.channels),
                      channelUpdateCount: prevState.channelUpdateCount + 1
                    };
                  });

                  if (_this.props.onChannelTruncated && typeof _this.props.onChannelTruncated === 'function') _this.props.onChannelTruncated(_assertThisInitialized(_this), e);
                }

                return _context2.abrupt("return", null);

              case 26:
              case "end":
                return _context2.stop();
            }
          }
        }, _callee2);
      }));

      return function (_x) {
        return _ref2.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "getChannel",
    /*#__PURE__*/
    function () {
      var _ref3 = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee3(type, id) {
        var channel;
        return _regeneratorRuntime.wrap(function _callee3$(_context3) {
          while (1) {
            switch (_context3.prev = _context3.next) {
              case 0:
                channel = _this.props.client.channel(type, id);
                _context3.next = 3;
                return channel.watch();

              case 3:
                return _context3.abrupt("return", channel);

              case 4:
              case "end":
                return _context3.stop();
            }
          }
        }, _callee3);
      }));

      return function (_x2, _x3) {
        return _ref3.apply(this, arguments);
      };
    }());

    _defineProperty(_assertThisInitialized(_this), "moveChannelUp", function (cid) {
      var channels = _this.state.channels; // get channel index

      var channelIndex = _this.state.channels.findIndex(function (channel) {
        return channel.cid === cid;
      });

      if (channelIndex <= 0) return; // get channel from channels

      var channel = channels[channelIndex]; //remove channel from current position

      channels.splice(channelIndex, 1); //add channel at the start

      channels.unshift(channel); // set new channel state

      _this.setState({
        channels: _toConsumableArray(channels)
      });
    });

    _defineProperty(_assertThisInitialized(_this), "loadNextPage", function () {
      _this.queryChannels();
    });

    _defineProperty(_assertThisInitialized(_this), "closeMenu", function () {
      _this.menuButton.current.checked = false;
    });

    _defineProperty(_assertThisInitialized(_this), "_renderChannel", function (item) {
      var _this$props = _this.props,
          Preview = _this$props.Preview,
          setActiveChannel = _this$props.setActiveChannel,
          channel = _this$props.channel,
          watchers = _this$props.watchers;
      if (!item) return;
      var props = {
        channel: item,
        activeChannel: channel,
        closeMenu: _this.closeMenu,
        Preview: Preview,
        setActiveChannel: setActiveChannel,
        watchers: watchers,
        key: item.id,
        // To force the update of preview component upon channel update.
        channelUpdateCount: _this.state.channelUpdateCount,
        connectionRecoveredCount: _this.state.connectionRecoveredCount
      };
      return smartRender(ChannelPreview, _objectSpread$8({}, props));
    });

    _this.state = {
      // list of channels
      channels: Immutable([]),
      oldChannels: undefined,
      // loading channels
      loadingChannels: true,
      // error loading channels
      refreshing: false,
      hasNextPage: false,
      offset: 0,
      error: false,
      connectionRecoveredCount: 0,
      channelUpdateCount: 0,
      sort: _this.props.sort,
      filters: _this.props.filters,
      searchModus: false,
      noResults: false
    };
    _this.onSortChange = _this.onSortChange.bind(_assertThisInitialized(_this));
    _this.onResetSearch = _this.onResetSearch.bind(_assertThisInitialized(_this));
    _this.onFilterChange = _this.onFilterChange.bind(_assertThisInitialized(_this));
    _this.onChannelSearchResult = _this.onChannelSearchResult.bind(_assertThisInitialized(_this));
    _this.menuButton = React__default.createRef();
    return _this;
  }

  _createClass(ChannelList, [{
    key: "componentDidCatch",
    value: function componentDidCatch(error, info) {
      console.warn(error, info);
    }
  }, {
    key: "componentDidMount",
    value: function () {
      var _componentDidMount = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee4() {
        return _regeneratorRuntime.wrap(function _callee4$(_context4) {
          while (1) {
            switch (_context4.prev = _context4.next) {
              case 0:
                this.setState({
                  sort: this.props.sort
                });
                this.setState({
                  filters: this.props.filters
                });
                _context4.next = 4;
                return this.queryChannels();

              case 4:
                this.listenToChanges();

              case 5:
              case "end":
                return _context4.stop();
            }
          }
        }, _callee4, this);
      }));

      function componentDidMount() {
        return _componentDidMount.apply(this, arguments);
      }

      return componentDidMount;
    }()
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      this.props.client.off(this.handleEvent);
    }
  }, {
    key: "onSortChange",
    value: function onSortChange(sort) {
      var _this2 = this;

      this.setState({
        sort: sort,
        offset: 0,
        hasNextPage: true,
        channels: [],
        oldChannels: undefined
      },
      /*#__PURE__*/
      _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee5() {
        return _regeneratorRuntime.wrap(function _callee5$(_context5) {
          while (1) {
            switch (_context5.prev = _context5.next) {
              case 0:
                _context5.next = 2;
                return _this2.queryChannels();

              case 2:
              case "end":
                return _context5.stop();
            }
          }
        }, _callee5);
      })));
    }
  }, {
    key: "onFilterChange",
    value: function onFilterChange(filter) {
      var _this3 = this;

      this.setState({
        filters: filter,
        offset: 0,
        hasNextPage: true,
        channels: [],
        oldChannels: undefined
      },
      /*#__PURE__*/
      _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee6() {
        return _regeneratorRuntime.wrap(function _callee6$(_context6) {
          while (1) {
            switch (_context6.prev = _context6.next) {
              case 0:
                _context6.next = 2;
                return _this3.queryChannels();

              case 2:
              case "end":
                return _context6.stop();
            }
          }
        }, _callee6);
      })));
    }
  }, {
    key: "onResetSearch",
    value: function onResetSearch() {
      var _this4 = this;

      this.setState({
        offset: 0,
        hasNextPage: true,
        channels: [],
        oldChannels: undefined
      },
      /*#__PURE__*/
      _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee7() {
        return _regeneratorRuntime.wrap(function _callee7$(_context7) {
          while (1) {
            switch (_context7.prev = _context7.next) {
              case 0:
                _context7.next = 2;
                return _this4.queryChannels();

              case 2:
              case "end":
                return _context7.stop();
            }
          }
        }, _callee7);
      }))); // this.props.setActiveSearch('');
    }
  }, {
    key: "listenToChanges",
    value: function listenToChanges() {
      this.props.client.on(this.handleEvent);
    } // eslint-disable-next-line sonarjs/cognitive-complexity

  }, {
    key: "onChannelSearchResult",
    value: function () {
      var _onChannelSearchResult = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee8(searchResultChannels) {
        var result, lastChannels;
        return _regeneratorRuntime.wrap(function _callee8$(_context8) {
          while (1) {
            switch (_context8.prev = _context8.next) {
              case 0:
                if (this.state.oldChannels === undefined) {
                  this.setState({
                    oldChannels: this.state.channels
                  });
                }

                if (this.state.oldChannels !== undefined) {
                  this.setState({
                    channels: this.state.oldChannels
                  });
                }

                result = [];

                if (searchResultChannels) {
                  result = this.state.channels.filter(function (channel) {
                    return searchResultChannels.find(function (i) {
                      return i.cid === channel.cid;
                    });
                  });
                }

                lastChannels = {};

                if (this.state.oldChannels !== undefined) {
                  lastChannels = this.state.oldChannels;
                } else {
                  lastChannels = this.state.channels;
                }

                if (!(result.length > 0)) {
                  _context8.next = 11;
                  break;
                }

                _context8.next = 9;
                return this.setState({
                  channels: result
                });

              case 9:
                _context8.next = 12;
                break;

              case 11:
                this.setState({
                  channels: []
                }, function () {});

              case 12:
                if (this.state.channels.length > 0) {
                  this.props.setActiveChannel(this.state.channels[0], this.props.watchers);
                } else {
                  this.props.setActiveChannel(lastChannels[0], this.props.watchers);
                }

              case 13:
              case "end":
                return _context8.stop();
            }
          }
        }, _callee8, this);
      }));

      function onChannelSearchResult(_x4) {
        return _onChannelSearchResult.apply(this, arguments);
      }

      return onChannelSearchResult;
    }() // new channel list // *********************************

  }, {
    key: "render",
    value: function render() {
      var _this5 = this;

      var _this$props2 = this.props,
          List = _this$props2.List,
          Paginator = _this$props2.Paginator;
      var _this$state2 = this.state,
          channels = _this$state2.channels,
          loadingChannels = _this$state2.loadingChannels,
          refreshing = _this$state2.refreshing,
          hasNextPage = _this$state2.hasNextPage,
          noResults = _this$state2.noResults;
      return React__default.createElement(React__default.Fragment, null, React__default.createElement("input", {
        type: "checkbox",
        id: "str-chat-channel-checkbox",
        ref: this.menuButton,
        className: "str-chat-channel-checkbox"
      }), React__default.createElement("label", {
        htmlFor: "str-chat-channel-checkbox",
        className: "str-chat-channel-list-burger"
      }, React__default.createElement("div", null)), React__default.createElement("div", {
        className: "str-chat str-chat-channel-list ".concat(this.props.theme, " ").concat(this.props.open ? 'str-chat-channel-list--open' : ''),
        ref: this.channelList
      }, React__default.createElement(List, {
        loading: loadingChannels,
        error: this.state.error,
        channels: channels,
        setActiveChannel: this.props.setActiveChannel,
        setActiveSearch: this.props.setActiveSearch,
        activeChannel: this.props.channel,
        showSidebar: this.props.showSidebar,
        onSelectSort: this.onSortChange,
        onSelectFilter: this.onFilterChange,
        onToggleSearchModus: this.onToggleSearchModus,
        isOn: false,
        onChannelResult: this.onChannelSearchResult,
        noResult: noResults,
        resetSearch: this.onResetSearch
      }, !channels.length ? React__default.createElement(EmptyStateIndicator, {
        listType: "channel"
      }) : smartRender(Paginator, {
        loadNextPage: this.loadNextPage,
        hasNextPage: hasNextPage,
        refreshing: refreshing,
        children: channels.map(function (item) {
          return _this5._renderChannel(item);
        })
      }))));
    }
  }], [{
    key: "getDerivedStateFromError",
    value: function getDerivedStateFromError() {
      return {
        error: true
      };
    }
  }]);

  return ChannelList;
}(React.PureComponent);

_defineProperty(exports.ChannelList, "propTypes", {
  /**
   *
   *
   * Indicator for Empty State
   * */
  EmptyStateIndicator: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   * Available built-in options (also accepts the same props as):
   *
   * 1. [ChannelPreviewCompact](https://getstream.github.io/stream-chat-react/#ChannelPreviewCompact) (default)
   * 2. [ChannelPreviewLastMessage](https://getstream.github.io/stream-chat-react/#ChannelPreviewLastMessage)
   * 3. [ChannelPreviewMessanger](https://getstream.github.io/stream-chat-react/#ChannelPreviewMessanger)
   *
   * The Preview to use, defaults to ChannelPreviewLastMessage
   * */
  Preview: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   * Loading indicator UI Component. It will be displayed until the channels are
   * being queried from API. Once the channels are loaded/queried, loading indicator is removed
   * and replaced with children of the Channel component.
   *
   * Defaults to and accepts same props as:
   * [LoadingIndicator](https://github.com/GetStream/stream-chat-react/blob/master/src/components/LoadingIndicator.js)
   *
   */
  LoadingIndicator: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   * Custom UI Component for container of list of channels. Note that, list (UI component) of channels is passed
   * to this component as children. This component is for the purpose of adding header to channel list or styling container
   * for list of channels.
   *
   * Available built-in options (also accepts the same props as):
   *
   * 1. [ChannelListTeam](https://github.com/GetStream/stream-chat-react/blob/master/src/components/ChannelListTeam.js) (default)
   * 2. [ChannelListMessenger](https://github.com/GetStream/stream-chat-react/blob/master/src/components/ChannelListMessenger.js)
   *
   * It has access to some additional props:
   *
   * - `setActiveChannel` {function} Check [chat context](https://getstream.github.io/stream-chat-react/#chat)
   * - `activeChannel` Currently active channel object
   * - `channels` {array} List of channels in channel list
   */
  List: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   * Paginator component for channels. It contains all the pagination logic such as
   * - fetching next page of results when needed e.g., when scroll reaches the end of list
   * - UI to display loading indicator when next page is being loaded
   * - call to action button to trigger loading of next page.
   *
   * Available built-in options (also accepts the same props as):
   *
   * 1. [LoadMorePaginator](https://github.com/GetStream/stream-chat-react/blob/master/src/components/LoadMorePaginator.js)
   * 2. [InfiniteScrollPaginator](https://github.com/GetStream/stream-chat-react/blob/master/src/components/InfiniteScrollPaginator.js)
   */
  Paginator: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   * Function that overrides default behaviour when new message is received on channel that is not being watched
   *
   * @param {Component} thisArg Reference to ChannelList component
   * @param {Event} event       [Event object](https://getstream.io/chat/docs/#event_object) corresponding to `notification.message_new` event
   * */
  onMessageNew: PropTypes.func,

  /**
   * Function that overrides default behaviour when users gets added to a channel
   *
   * @param {Component} thisArg Reference to ChannelList component
   * @param {Event} event       [Event object](https://getstream.io/chat/docs/#event_object) corresponding to `notification.added_to_channel` event
   * */
  onAddedToChannel: PropTypes.func,

  /**
   * Function that overrides default behaviour when users gets removed from a channel
   *
   * @param {Component} thisArg Reference to ChannelList component
   * @param {Event} event       [Event object](https://getstream.io/chat/docs/#event_object) corresponding to `notification.removed_from_channel` event
   * */
  onRemovedFromChannel: PropTypes.func,

  /**
   * Function that overrides default behaviour when channel gets updated
   *
   * @param {Component} thisArg Reference to ChannelList component
   * @param {Event} event       [Event object](https://getstream.io/chat/docs/#event_object) corresponding to `notification.channel_updated` event
   * */
  onChannelUpdated: PropTypes.func,

  /**
   * Function to customize behaviour when channel gets truncated
   *
   * @param {Component} thisArg Reference to ChannelList component
   * @param {Event} event       [Event object](https://getstream.io/chat/docs/#event_object) corresponding to `channel.truncated` event
   * */
  onChannelTruncated: PropTypes.func,

  /**
   * Function that overrides default behaviour when channel gets deleted. In absence of this prop, channel will be removed from the list.
   *
   * @param {Component} thisArg Reference to ChannelList component
   * @param {Event} event       [Event object](https://getstream.io/chat/docs/#event_object) corresponding to `channel.deleted` event
   * */
  onChannelDeleted: PropTypes.func,

  /**
   * Object containing query filters
   * @see See [Channel query documentation](https://getstream.io/chat/docs/#query_channels) for a list of available fields for filter.
   * */
  filters: PropTypes.object,

  /**
   * Object containing query options
   * @see See [Channel query documentation](https://getstream.io/chat/docs/#query_channels) for a list of available fields for options.
   * */
  options: PropTypes.object,

  /**
   * Object containing sort parameters
   * @see See [Channel query documentation](https://getstream.io/chat/docs/#query_channels) for a list of available fields for sort.
   * */
  sort: PropTypes.object,

  /**
   * Object containing watcher parameters
   * @see See [Pagination documentation](https://getstream.io/chat/docs/#channel_pagination) for a list of available fields for sort.
   * */
  watchers: PropTypes.object,

  /**
   * Set a Channel to be active and move it to the top of the list of channels by ID.
   * */
  customAciveChannel: PropTypes.string,

  /**
   * If true, channels won't be dynamically sorted by most recent message.
   */
  lockChannelOrder: PropTypes.bool
});

_defineProperty(exports.ChannelList, "defaultProps", {
  Preview: ChannelPreviewLastMessage,
  LoadingIndicator: LoadingIndicator,
  List: exports.ChannelListTeam,
  Paginator: LoadMorePaginator,
  EmptyStateIndicator: EmptyStateIndicator,
  filters: {},
  options: {},
  sort: {},
  watchers: {}
});

exports.ChannelList = withChatContext(exports.ChannelList);

/**
 * ChannelList - A preview list of channels, allowing you to select the channel you want to open
 * @example ./examples/ChannelList.md
 */

exports.ChannelListMessenger =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(ChannelListMessenger, _PureComponent);

  function ChannelListMessenger() {
    _classCallCheck(this, ChannelListMessenger);

    return _possibleConstructorReturn(this, _getPrototypeOf(ChannelListMessenger).apply(this, arguments));
  }

  _createClass(ChannelListMessenger, [{
    key: "render",
    value: function render() {
      if (this.props.error) {
        return React__default.createElement(ChatDown, null);
      } else if (this.props.loading) {
        return React__default.createElement(LoadingChannels, null);
      } else {
        return React__default.createElement("div", {
          className: "str-chat__channel-list-messenger"
        }, React__default.createElement("div", {
          className: "str-chat__channel-list-messenger__main"
        }, this.props.children));
      }
    }
  }]);

  return ChannelListMessenger;
}(React.PureComponent);

_defineProperty(exports.ChannelListMessenger, "propTypes", {
  loading: PropTypes.bool,
  error: PropTypes.bool
});

_defineProperty(exports.ChannelListMessenger, "defaultProps", {
  error: false
});

exports.ChannelListMessenger = withChatContext(exports.ChannelListMessenger);

/**
 *
 * @example ./docs/ChannelPreviewCompact.md
 * @extends PureComponent
 *
 */

var ChannelPreviewCompact =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(ChannelPreviewCompact, _React$PureComponent);

  function ChannelPreviewCompact() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, ChannelPreviewCompact);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(ChannelPreviewCompact)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "channelPreviewButton", React__default.createRef());

    _defineProperty(_assertThisInitialized(_this), "onSelectChannel", function () {
      _this.props.setActiveChannel(_this.props.channel, _this.props.watchers);

      _this.channelPreviewButton.current.blur();
    });

    return _this;
  }

  _createClass(ChannelPreviewCompact, [{
    key: "render",
    value: function render() {
      var unreadClass = this.props.unread_count >= 1 ? 'str-chat__channel-preview-compact--unread' : '';
      var activeClass = this.props.active ? 'str-chat__channel-preview-compact--active' : '';
      var name = this.props.channel.data.name || this.props.channel.cid;
      return React__default.createElement("button", {
        onClick: this.onSelectChannel,
        ref: this.channelPreviewButton,
        className: "str-chat__channel-preview-compact ".concat(unreadClass, " ").concat(activeClass)
      }, React__default.createElement("div", {
        className: "str-chat__channel-preview-compact--left"
      }, React__default.createElement(Avatar, {
        image: this.props.channel.data.image,
        size: 20
      })), React__default.createElement("div", {
        className: "str-chat__channel-preview-compact--right"
      }, name));
    }
  }]);

  return ChannelPreviewCompact;
}(React__default.PureComponent);

_defineProperty(ChannelPreviewCompact, "propTypes", {
  /** **Available from [chat context](https://getstream.github.io/stream-chat-react/#chat)** */
  setActiveChannel: PropTypes.func,

  /** **Available from [chat context](https://getstream.github.io/stream-chat-react/#chat)** */
  channel: PropTypes.object,
  closeMenu: PropTypes.func,
  unread_count: PropTypes.number,

  /** If channel of component is active (selected) channel */
  active: PropTypes.bool,
  latestMessage: PropTypes.string
});

/**
 * Used as preview component for channel item in [ChannelList](#channellist) component.
 * Its best suited for messenger type chat.
 *
 * @example ./docs/ChannelPreviewMessenger.md
 * @extends PureComponent
 */

exports.ChannelPreviewMessenger =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(ChannelPreviewMessenger, _PureComponent);

  function ChannelPreviewMessenger() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, ChannelPreviewMessenger);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(ChannelPreviewMessenger)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "channelPreviewButton", React__default.createRef());

    _defineProperty(_assertThisInitialized(_this), "onSelectChannel", function () {
      _this.props.setActiveChannel(_this.props.channel, _this.props.watchers);

      _this.channelPreviewButton.current.blur();

      _this.props.closeMenu();
    });

    _defineProperty(_assertThisInitialized(_this), "onDeleteChannel",
    /*#__PURE__*/
    _asyncToGenerator(
    /*#__PURE__*/
    _regeneratorRuntime.mark(function _callee() {
      var removeChannel;
      return _regeneratorRuntime.wrap(function _callee$(_context) {
        while (1) {
          switch (_context.prev = _context.next) {
            case 0:
              _context.next = 2;
              return _this.props.channel.delete();

            case 2:
              removeChannel = _context.sent;

            case 3:
            case "end":
              return _context.stop();
          }
        }
      }, _callee);
    })));

    return _this;
  }

  _createClass(ChannelPreviewMessenger, [{
    key: "render",
    value: function render() {
      var unreadClass = this.props.unread >= 1 ? 'str-chat__channel-preview-messenger--unread' : '';
      var activeClass = this.props.active ? 'str-chat__channel-preview-messenger--active' : '';
      var _this$props = this.props,
          channel = _this$props.channel,
          intl = _this$props.intl;
      return React__default.createElement("div", {
        id: "style-select-channel-list"
      }, React__default.createElement("button", {
        onClick: this.onSelectChannel,
        ref: this.channelPreviewButton,
        className: "str-chat__channel-preview-messenger ".concat(unreadClass, " ").concat(activeClass)
      }, React__default.createElement("div", {
        className: "str-chat__channel-preview-messenger--right"
      }, React__default.createElement("div", {
        className: "str-chat__channel-preview-messenger--name"
      }, React__default.createElement("span", null, truncate(channel.data.name, 29))), React__default.createElement("div", {
        id: "style-channel-preview"
      }, React__default.createElement("div", {
        className: "str-chat__channel-preview-messenger--last-message"
      }, !channel.state.messages[0] ? intl.formatMessage({
        id: 'channel_preview.latest_message.none',
        defaultMessage: 'Nothing yet...'
      }) : truncate(this.props.latestMessage, 14)), channel._client.user.role === 'admin' ? React__default.createElement("div", {
        className: "str-chat__channel-preview-messenger--responsive-member"
      }, channel.data.responsiveMember ? React__default.createElement("div", {
        className: "channel-list-show-responsive-member"
      }, React__default.createElement("div", {
        className: "str-chat__channel-preview-assigned-to"
      }, intl.formatMessage({
        id: 'channel_preview.responsiveMember.label',
        defaultMessage: 'assigned to'
      }) + ' ' + channel.data.responsiveMember, ' ')) : intl.formatMessage({
        id: 'channel_preview.responsiveMember.none',
        defaultMessage: 'not assigned'
      })) : React__default.createElement("div", null)))), React__default.createElement("div", {
        className: "style-unread-icon-section"
      }, channel._client.user.role === 'admin' ? React__default.createElement("div", {
        className: "channel-delete-tooltip"
      }, React__default.createElement("span", {
        className: "channel-delete-tooltiptext"
      }, "L\xF6schen des Channels"), React__default.createElement("div", {
        className: "str-chat__channel-preview-delete-channel-icon",
        onClick: this.onDeleteChannel
      }, React__default.createElement("svg", {
        id: "style-channel-delete-icon",
        width: "14",
        height: "14",
        viewBox: "0 0 18 18",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        stroke: "black",
        d: "M 19.333 2.547 l -1.88 -1.88 L 10 8.12 L 2.547 0.667 l -1.88 1.88 L 8.12 10 L 0.667 17.453 l 1.88 1.88 L 10 11.88 l 7.453 7.453 l 1.88 -1.88 L 11.88 10 Z"
      })))) : React__default.createElement("div", null), this.showUnread()));
    }
  }, {
    key: "showUnread",
    value: function showUnread() {
      if (!this.props.unread) {
        return;
      } else {
        return React__default.createElement("label", {
          id: "style-unread-message-button"
        }, !this.props.unread ? '' : truncate(this.props.unread, 5));
      }
    }
  }]);

  return ChannelPreviewMessenger;
}(React.PureComponent);

_defineProperty(exports.ChannelPreviewMessenger, "propTypes", {
  /** **Available from [chat context](https://getstream.github.io/stream-chat-react/#chat)** */
  setActiveChannel: PropTypes.func,

  /** **Available from [chat context](https://getstream.github.io/stream-chat-react/#chat)** */
  channel: PropTypes.object,
  closeMenu: PropTypes.func,
  unread: PropTypes.number,

  /** If channel of component is active (selected) channel */
  active: PropTypes.bool,
  latestMessage: PropTypes.string
});

exports.ChannelPreviewMessenger = reactIntl.injectIntl(exports.ChannelPreviewMessenger);

var InfiniteScroll =
/*#__PURE__*/
function (_Component) {
  _inherits(InfiniteScroll, _Component);

  function InfiniteScroll(props) {
    var _this;

    _classCallCheck(this, InfiniteScroll);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(InfiniteScroll).call(this, props));
    _this.scrollListener = _this.scrollListener.bind(_assertThisInitialized(_this));
    return _this;
  }

  _createClass(InfiniteScroll, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      this.pageLoaded = this.props.pageStart;
      this.attachScrollListener();
    }
  }, {
    key: "componentDidUpdate",
    value: function componentDidUpdate() {
      this.attachScrollListener();
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      this.detachScrollListener();
      this.detachMousewheelListener();
    } // Set a defaut loader for all your `InfiniteScroll` components

  }, {
    key: "setDefaultLoader",
    value: function setDefaultLoader(loader) {
      this.defaultLoader = loader;
    }
  }, {
    key: "detachMousewheelListener",
    value: function detachMousewheelListener() {
      var scrollEl = window;

      if (this.props.useWindow === false) {
        scrollEl = this.scrollComponent.parentNode;
      }

      scrollEl.removeEventListener('mousewheel', this.mousewheelListener, this.props.useCapture);
    }
  }, {
    key: "detachScrollListener",
    value: function detachScrollListener() {
      var scrollEl = window;

      if (this.props.useWindow === false) {
        scrollEl = this.getParentElement(this.scrollComponent);
      }

      scrollEl.removeEventListener('scroll', this.scrollListener, this.props.useCapture);
      scrollEl.removeEventListener('resize', this.scrollListener, this.props.useCapture);
    }
  }, {
    key: "getParentElement",
    value: function getParentElement(el) {
      return el && el.parentNode;
    }
  }, {
    key: "filterProps",
    value: function filterProps(props) {
      return props;
    }
  }, {
    key: "attachScrollListener",
    value: function attachScrollListener() {
      if (!this.props.hasMore || this.props.isLoading || !this.getParentElement(this.scrollComponent)) {
        return;
      }

      var scrollEl = window;

      if (this.props.useWindow === false) {
        scrollEl = this.getParentElement(this.scrollComponent);
      }

      scrollEl.addEventListener('mousewheel', this.mousewheelListener, this.props.useCapture);
      scrollEl.addEventListener('scroll', this.scrollListener, this.props.useCapture);
      scrollEl.addEventListener('resize', this.scrollListener, this.props.useCapture);

      if (this.props.initialLoad) {
        this.scrollListener();
      }
    }
  }, {
    key: "mousewheelListener",
    value: function mousewheelListener(e) {
      // Prevents Chrome hangups
      // See: https://stackoverflow.com/questions/47524205/random-high-content-download-time-in-chrome/47684257#47684257
      if (e.deltaY === 1) {
        e.preventDefault();
      }
    }
  }, {
    key: "scrollListener",
    value: function scrollListener() {
      var el = this.scrollComponent;
      var scrollEl = window;
      var parentNode = this.getParentElement(el);
      var offset;

      if (this.props.useWindow) {
        var doc = document.documentElement || document.body.parentNode || document.body;
        var scrollTop = scrollEl.pageYOffset !== undefined ? scrollEl.pageYOffset : doc.scrollTop;

        if (this.props.isReverse) {
          offset = scrollTop;
        } else {
          offset = this.calculateOffset(el, scrollTop);
        }
      } else if (this.props.isReverse) {
        offset = parentNode.scrollTop;
      } else {
        offset = el.scrollHeight - parentNode.scrollTop - parentNode.clientHeight;
      } // Here we make sure the element is visible as well as checking the offset


      if (offset < Number(this.props.threshold) && el && el.offsetParent !== null) {
        this.detachScrollListener(); // Call loadMore after detachScrollListener to allow for non-async loadMore functions

        if (typeof this.props.loadMore === 'function') {
          this.props.loadMore(this.pageLoaded += 1);
        }
      }
    }
  }, {
    key: "calculateOffset",
    value: function calculateOffset(el, scrollTop) {
      if (!el) {
        return 0;
      }

      return this.calculateTopPosition(el) + (el.offsetHeight - scrollTop - window.innerHeight);
    }
  }, {
    key: "calculateTopPosition",
    value: function calculateTopPosition(el) {
      if (!el) {
        return 0;
      }

      return el.offsetTop + this.calculateTopPosition(el.offsetParent);
    }
  }, {
    key: "render",
    value: function render() {
      var _this2 = this;

      var renderProps = this.filterProps(this.props);

      var children = renderProps.children,
          element = renderProps.element,
          hasMore = renderProps.hasMore,
          initialLoad = renderProps.initialLoad,
          isReverse = renderProps.isReverse,
          loader = renderProps.loader,
          loadMore = renderProps.loadMore,
          pageStart = renderProps.pageStart,
          ref = renderProps.ref,
          threshold = renderProps.threshold,
          useCapture = renderProps.useCapture,
          useWindow = renderProps.useWindow,
          isLoading = renderProps.isLoading,
          props = _objectWithoutProperties(renderProps, ["children", "element", "hasMore", "initialLoad", "isReverse", "loader", "loadMore", "pageStart", "ref", "threshold", "useCapture", "useWindow", "isLoading"]);

      props.ref = function (node) {
        _this2.scrollComponent = node;

        if (ref) {
          ref(node);
        }
      };

      var childrenArray = [children];

      if (isLoading) {
        if (loader) {
          isReverse ? childrenArray.unshift(loader) : childrenArray.push(loader);
        } else if (this.defaultLoader) {
          isReverse ? childrenArray.unshift(this.defaultLoader) : childrenArray.push(this.defaultLoader);
        }
      }

      return React__default.createElement(element, props, childrenArray);
    }
  }]);

  return InfiniteScroll;
}(React.Component);

_defineProperty(InfiniteScroll, "propTypes", {
  children: PropTypes.node.isRequired,
  element: PropTypes.node,
  hasMore: PropTypes.bool,
  initialLoad: PropTypes.bool,
  isReverse: PropTypes.bool,
  loader: PropTypes.node,
  loadMore: PropTypes.func.isRequired,
  pageStart: PropTypes.number,
  isLoading: PropTypes.bool,
  ref: PropTypes.func,
  threshold: PropTypes.number,
  useCapture: PropTypes.bool,
  useWindow: PropTypes.bool
});

_defineProperty(InfiniteScroll, "defaultProps", {
  element: 'div',
  hasMore: false,
  initialLoad: true,
  isLoading: false,
  pageStart: 0,
  ref: null,
  threshold: 250,
  useWindow: true,
  isReverse: false,
  useCapture: false,
  loader: null
});

var InfiniteScrollPaginator =
/*#__PURE__*/
function (_React$Component) {
  _inherits(InfiniteScrollPaginator, _React$Component);

  function InfiniteScrollPaginator() {
    _classCallCheck(this, InfiniteScrollPaginator);

    return _possibleConstructorReturn(this, _getPrototypeOf(InfiniteScrollPaginator).apply(this, arguments));
  }

  _createClass(InfiniteScrollPaginator, [{
    key: "render",
    value: function render() {
      return React__default.createElement(InfiniteScroll, {
        loadMore: this.props.loadNextPage,
        hasMore: this.props.hasNextPage,
        isLoading: this.props.refreshing,
        isReverse: this.props.reverse,
        threshold: this.props.threshold,
        useWindow: false,
        loader: React__default.createElement("div", {
          className: "str-chat__infinite-scroll-paginator",
          key: "loadingindicator"
        }, React__default.createElement(reactFileUtils.LoadingIndicator, null))
      }, this.props.children);
    }
  }]);

  return InfiniteScrollPaginator;
}(React__default.Component);

_defineProperty(InfiniteScrollPaginator, "propTypes", {
  /** callback to load the next page */
  loadNextPage: PropTypes.func,

  /** indicates if there is a next page to load */
  hasNextPage: PropTypes.bool,

  /** indicates if there there's currently any refreshing taking place */
  refreshing: PropTypes.bool,

  /** display the items in opposite order */
  reverse: PropTypes.bool,

  /** Offset from when to start the loadNextPage call */
  threshold: PropTypes.number,

  /** The loading indicator to use */
  LoadingIndicator: PropTypes.oneOfType([PropTypes.node, PropTypes.func])
});

_defineProperty(InfiniteScrollPaginator, "defaultProps", {
  LoadingIndicator: React__default.createElement(reactFileUtils.LoadingIndicator, null)
});

/**
 * MessageCommerce - Render component, should be used together with the Message component
 *
 * @example ./docs/MessageSimple.md
 * @extends PureComponent
 */

var MessageCommerce =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(MessageCommerce, _PureComponent);

  function MessageCommerce() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, MessageCommerce);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(MessageCommerce)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "state", {
      isFocused: false,
      actionsBoxOpen: false,
      showDetailedReactions: false
    });

    _defineProperty(_assertThisInitialized(_this), "messageActionsRef", React__default.createRef());

    _defineProperty(_assertThisInitialized(_this), "reactionSelectorRef", React__default.createRef());

    _defineProperty(_assertThisInitialized(_this), "_onClickOptionsAction", function () {
      _this.setState({
        actionsBoxOpen: true
      }, function () {
        return document.addEventListener('click', _this.hideOptions, false);
      });
    });

    _defineProperty(_assertThisInitialized(_this), "_hideOptions", function () {
      _this.setState({
        actionsBoxOpen: false
      });

      document.removeEventListener('click', _this.hideOptions, false);
    });

    _defineProperty(_assertThisInitialized(_this), "_clickReactionList", function () {
      _this.setState(function () {
        return {
          showDetailedReactions: true
        };
      }, function () {
        document.addEventListener('click', _this._closeDetailedReactions);
      });
    });

    _defineProperty(_assertThisInitialized(_this), "_closeDetailedReactions", function (e) {
      if (!_this.reactionSelectorRef.current.reactionSelector.current.contains(e.target)) {
        _this.setState(function () {
          return {
            showDetailedReactions: false
          };
        }, function () {
          document.removeEventListener('click', _this._closeDetailedReactions);
        });
      } else {
        return {};
      }
    });

    return _this;
  }

  _createClass(MessageCommerce, [{
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      if (!this.props.message.deleted_at) {
        document.removeEventListener('click', this._closeDetailedReactions);
      }
    }
  }, {
    key: "isMine",
    value: function isMine() {
      return !this.props.isMyMessage(this.props.message);
    }
  }, {
    key: "renderOptions",
    value: function renderOptions() {
      if (this.props.message.type === 'error' || this.props.message.type === 'system' || this.props.message.type === 'ephemeral' || this.props.message.status === 'sending' || this.props.message.status === 'failed' || !this.props.channelConfig.reactions || this.props.initialMessage) {
        return;
      }

      return React__default.createElement("div", {
        className: "str-chat__message-commerce__actions"
      }, React__default.createElement("div", {
        className: "str-chat__message-commerce__actions__action str-chat__message-commerce__actions__action--reactions",
        onClick: this._clickReactionList
      }, React__default.createElement("svg", {
        width: "14",
        height: "14",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        d: "M11.108 8.05a.496.496 0 0 1 .212.667C10.581 10.147 8.886 11 7 11c-1.933 0-3.673-.882-4.33-2.302a.497.497 0 0 1 .9-.417C4.068 9.357 5.446 10 7 10c1.519 0 2.869-.633 3.44-1.738a.495.495 0 0 1 .668-.212zm.792-1.826a.477.477 0 0 1-.119.692.541.541 0 0 1-.31.084.534.534 0 0 1-.428-.194c-.106-.138-.238-.306-.539-.306-.298 0-.431.168-.54.307A.534.534 0 0 1 9.538 7a.544.544 0 0 1-.31-.084.463.463 0 0 1-.117-.694c.33-.423.742-.722 1.394-.722.653 0 1.068.3 1.396.724zm-7 0a.477.477 0 0 1-.119.692.541.541 0 0 1-.31.084.534.534 0 0 1-.428-.194c-.106-.138-.238-.306-.539-.306-.299 0-.432.168-.54.307A.533.533 0 0 1 2.538 7a.544.544 0 0 1-.31-.084.463.463 0 0 1-.117-.694c.33-.423.742-.722 1.394-.722.653 0 1.068.3 1.396.724zM7 0a7 7 0 1 1 0 14A7 7 0 0 1 7 0zm4.243 11.243A5.96 5.96 0 0 0 13 7a5.96 5.96 0 0 0-1.757-4.243A5.96 5.96 0 0 0 7 1a5.96 5.96 0 0 0-4.243 1.757A5.96 5.96 0 0 0 1 7a5.96 5.96 0 0 0 1.757 4.243A5.96 5.96 0 0 0 7 13a5.96 5.96 0 0 0 4.243-1.757z",
        fillRule: "evenodd"
      }))));
    }
  }, {
    key: "render",
    value: function render() {
      var _this$props = this.props,
          message = _this$props.message,
          groupStyles = _this$props.groupStyles,
          Attachment$$1 = _this$props.Attachment,
          handleReaction = _this$props.handleReaction,
          handleAction = _this$props.handleAction,
          actionsEnabled = _this$props.actionsEnabled,
          onMentionsHoverMessage = _this$props.onMentionsHoverMessage,
          onMentionsClickMessage = _this$props.onMentionsClickMessage,
          unsafeHTML = _this$props.unsafeHTML,
          threadList = _this$props.threadList,
          handleOpenThread = _this$props.handleOpenThread;
      var when = moment(message.created_at).format('LT');
      var messageClasses = this.isMine() ? 'str-chat__message-commerce str-chat__message-commerce--left' : 'str-chat__message-commerce str-chat__message-commerce--right';
      var hasAttachment = Boolean(message.attachments && message.attachments.length);
      var images = hasAttachment && message.attachments.filter(function (item) {
        return item.type === 'image';
      });
      var hasReactions = Boolean(message.latest_reactions && message.latest_reactions.length);

      if (message.type === 'message.read' || message.deleted_at || message.type === 'message.date') {
        return null;
      }

      if (message.deleted_at) {
        return React__default.createElement(React__default.Fragment, null, React__default.createElement("span", {
          key: message.id,
          className: "".concat(messageClasses, " str-chat__message--deleted")
        }, React__default.createElement(reactIntl.FormattedMessage, {
          id: "message.deleted",
          defaultMessage: "This message was deleted..."
        })), React__default.createElement("div", {
          className: "clearfix"
        }));
      }

      return React__default.createElement(React__default.Fragment, null, React__default.createElement("div", {
        key: message.id,
        className: "\n\t\t\t\t\t\t".concat(messageClasses, "\n\t\t\t\t\t\tstr-chat__message-commerce--").concat(message.type, "\n\t\t\t\t\t\t").concat(message.text ? 'str-chat__message-commerce--has-text' : 'str-chat__message-commerce--has-no-text', "\n\t\t\t\t\t\t").concat(hasAttachment ? 'str-chat__message-commerce--has-attachment' : '', "\n\t\t\t\t\t\t").concat(hasReactions ? 'str-chat__message-commerce--with-reactions' : '', "\n\t\t\t\t\t\t", "str-chat__message-commerce--".concat(groupStyles[0]), "\n\t\t\t\t\t").trim(),
        onMouseLeave: this._hideOptions,
        ref: this.messageRef
      }, (groupStyles[0] === 'bottom' || groupStyles[0] === 'single') && React__default.createElement(Avatar, {
        image: message.user.image,
        size: 32,
        name: message.user.name || message.user.id
      }), React__default.createElement("div", {
        className: "str-chat__message-commerce-inner"
      }, !message.text && React__default.createElement(React__default.Fragment, null, this.renderOptions(), hasReactions && !this.state.showDetailedReactions && React__default.createElement(ReactionsList, {
        reactions: message.latest_reactions,
        reaction_counts: message.reaction_counts,
        onClick: this._clickReactionList
      }), this.state.showDetailedReactions && React__default.createElement(ReactionSelector, {
        reverse: false,
        handleReaction: handleReaction,
        actionsEnabled: actionsEnabled,
        detailedView: true,
        reaction_counts: message.reaction_counts,
        latest_reactions: message.latest_reactions,
        ref: this.reactionSelectorRef
      })), hasAttachment && images.length <= 1 && message.attachments.map(function (attachment, index) {
        return React__default.createElement(Attachment$$1, {
          key: "".concat(message.id, "-").concat(index),
          attachment: attachment,
          actionHandler: handleAction
        });
      }), images.length > 1 && React__default.createElement(Gallery, {
        images: images
      }), message.text && React__default.createElement("div", {
        className: "str-chat__message-commerce-text"
      }, React__default.createElement("div", {
        className: "str-chat__message-commerce-text-inner\n\t\t\t\t\t\t\t\t\t".concat(hasAttachment ? 'str-chat__message-commerce-text-inner--has-attachment' : '', "\n\t\t\t\t\t\t\t\t\t").concat(isOnlyEmojis(message.text) ? 'str-chat__message-commerce-text-inner--is-emoji' : '', "\n                ").trim(),
        onMouseOver: onMentionsHoverMessage,
        onClick: onMentionsClickMessage
      }, message.type === 'error' && React__default.createElement("div", {
        className: "str-chat__commerce-message--error-message"
      }, React__default.createElement(reactIntl.FormattedMessage, {
        id: "message.error",
        defaultMessage: "Error \xB7 Unsent"
      })), unsafeHTML ? React__default.createElement("div", {
        dangerouslySetInnerHTML: {
          __html: message.html
        }
      }) : renderText(message), hasReactions && !this.state.showDetailedReactions && React__default.createElement(ReactionsList, {
        reverse: true,
        reactions: message.latest_reactions,
        reaction_counts: message.reaction_counts,
        onClick: this._clickReactionList
      }), this.state.showDetailedReactions && React__default.createElement(ReactionSelector, {
        reverse: false,
        handleReaction: handleReaction,
        actionsEnabled: actionsEnabled,
        detailedView: true,
        reaction_counts: message.reaction_counts,
        latest_reactions: message.latest_reactions,
        ref: this.reactionSelectorRef
      })), message.text && this.renderOptions()), !threadList && React__default.createElement("div", {
        className: "str-chat__message-commerce-reply-button"
      }, React__default.createElement(MessageRepliesCountButton, {
        onClick: handleOpenThread,
        reply_count: message.reply_count
      })), React__default.createElement("div", {
        className: "str-chat__message-commerce-data"
      }, this.isMine() ? React__default.createElement("span", {
        className: "str-chat__message-commerce-name"
      }, message.user.name || message.user.id) : null, React__default.createElement("span", {
        className: "str-chat__message-commerce-timestamp"
      }, when)))));
    }
  }]);

  return MessageCommerce;
}(React.PureComponent);

_defineProperty(MessageCommerce, "propTypes", {
  /** The [message object](https://getstream.io/chat/docs/#message_format) */
  message: PropTypes.object,

  /**
   * The attachment UI component.
   * Default: [Attachment](https://github.com/GetStream/stream-chat-react/blob/master/src/components/Attachment.js)
   * */
  Attachment: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   *
   * @deprecated Its not recommended to use this anymore. All the methods in this HOC are provided explicitly.
   *
   * The higher order message component, most logic is delegated to this component
   * @see See [Message HOC](https://getstream.github.io/stream-chat-react/#message) for example
   *
   * */
  Message: PropTypes.oneOfType([PropTypes.node, PropTypes.func, PropTypes.object]).isRequired,

  /** render HTML instead of markdown. Posting HTML is only allowed server-side */
  unsafeHTML: PropTypes.bool,

  /** If its parent message in thread. */
  initialMessage: PropTypes.bool,

  /** Channel config object */
  channelConfig: PropTypes.object,

  /** If component is in thread list */
  threadList: PropTypes.bool,

  /** Function to open thread on current messxage */
  handleOpenThread: PropTypes.func,

  /** Returns true if message belongs to current user */
  isMyMessage: PropTypes.func,

  /** Returns all allowed actions on message by current user e.g., [edit, delete, flag, mute] */
  getMessageActions: PropTypes.func,

  /**
   * Add or remove reaction on message
   *
   * @param type Type of reaction - 'like' | 'love' | 'haha' | 'wow' | 'sad' | 'angry'
   * @param event Dom event which triggered this function
   */
  handleReaction: PropTypes.func,

  /** If actions such as edit, delete, flag, mute are enabled on message */
  actionsEnabled: PropTypes.bool,

  /**
   * Handler for actions. Actions in combination with attachments can be used to build [commands](https://getstream.io/chat/docs/#channel_commands).
   *
   * @param name {string} Name of action
   * @param value {string} Value of action
   * @param event Dom event that triggered this handler
   */
  handleAction: PropTypes.func,

  /**
   * The handler for hover event on @mention in message
   *
   * @param event Dom hover event which triggered handler.
   * @param user Target user object
   */
  onMentionsHoverMessage: PropTypes.func,

  /**
   * The handler for click event on @mention in message
   *
   * @param event Dom click event which triggered handler.
   * @param user Target user object
   */
  onMentionsClickMessage: PropTypes.func,

  /** Position of message in group. Possible values: top, bottom, middle, single */
  groupStyles: PropTypes.array
});

_defineProperty(MessageCommerce, "defaultProps", {
  Attachment: Attachment
});

function ownKeys$9(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread$9(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys$9(source, true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys$9(source).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }
var SimpleReactionsList =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(SimpleReactionsList, _React$PureComponent);

  function SimpleReactionsList() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, SimpleReactionsList);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(SimpleReactionsList)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "state", {
      showTooltip: false,
      users: []
    });

    _defineProperty(_assertThisInitialized(_this), "showTooltip", function () {
      _this.setState({
        showTooltip: true
      });
    });

    _defineProperty(_assertThisInitialized(_this), "hideTooltip", function () {
      _this.setState({
        showTooltip: false
      });
    });

    _defineProperty(_assertThisInitialized(_this), "handleReaction", function (e, type) {
      if (e !== undefined && e.preventDefault) {
        e.preventDefault();
      }

      _this.props.handleReaction(type);

      _this.setUsernames(type);
    });

    _defineProperty(_assertThisInitialized(_this), "getReactionCount", function () {
      var reaction_counts = _this.props.reaction_counts;
      var count = null;

      if (reaction_counts !== null && reaction_counts !== undefined && Object.keys(reaction_counts).length > 0) {
        count = 0;
        Object.keys(reaction_counts).map(function (key) {
          return count += reaction_counts[key];
        });
      }

      return count;
    });

    _defineProperty(_assertThisInitialized(_this), "renderUsers", function (users) {
      return users.map(function (user, i) {
        var text = user;

        if (i + 1 < users.length) {
          text += ', ';
        }

        return React__default.createElement("span", {
          className: "latest-user-username",
          key: "key-".concat(i, "-").concat(user)
        }, text);
      });
    });

    _defineProperty(_assertThisInitialized(_this), "getReactionsByType", function (reactions) {
      var reactionsByType = {};
      reactions.map(function (item) {
        if (reactionsByType[item.type] === undefined) {
          return reactionsByType[item.type] = [item];
        } else {
          return reactionsByType[item.type] = [].concat(_toConsumableArray(reactionsByType[item.type]), [item]);
        }
      });
      return reactionsByType;
    });

    _defineProperty(_assertThisInitialized(_this), "renderReactions", function (reactions) {
      var reactionsByType = _this.getReactionsByType(reactions);

      var reactionsEmojis = _this.props.reactionOptions.reduce(function (acc, cur) {
        return _objectSpread$9({}, acc, _defineProperty({}, cur.id, cur));
      }, {});

      return Object.keys(reactionsByType).map(function (type, i) {
        return React__default.createElement("li", {
          className: "str-chat__simple-reactions-list-item",
          key: "".concat(reactionsByType[type][0].id, "-").concat(i),
          onClick: function onClick(e) {
            return _this.handleReaction(e, type);
          }
        }, React__default.createElement("span", {
          onMouseEnter: function onMouseEnter() {
            return _this.setUsernames(type);
          }
        }, React__default.createElement(emojiMart.NimbleEmoji, _extends({
          emoji: reactionsEmojis[type]
        }, emojiSetDef, {
          size: 13,
          data: emojiData
        })), "\xA0"));
      });
    });

    _defineProperty(_assertThisInitialized(_this), "getUsernames", function (reactions) {
      return reactions.map(function (item) {
        return item.user !== null ? item.user.name || item.user.id : 'null';
      });
    });

    _defineProperty(_assertThisInitialized(_this), "setUsernames", function (type) {
      var reactionsByType = _this.getReactionsByType(_this.props.reactions);

      var reactions = reactionsByType[type];

      var users = _this.getUsernames(reactions);

      _this.setState({
        users: users
      }, function () {
        return _this.showTooltip();
      });
    });

    _defineProperty(_assertThisInitialized(_this), "renderUsernames", function (users) {
      return users.join(', ');
    });

    return _this;
  }

  _createClass(SimpleReactionsList, [{
    key: "render",
    value: function render() {
      var reactions = this.props.reactions;

      if (!reactions || reactions.length === 0) {
        return null;
      }

      return React__default.createElement("ul", {
        className: "str-chat__simple-reactions-list",
        onMouseLeave: this.hideTooltip
      }, this.state.showTooltip && React__default.createElement("div", {
        className: "str-chat__simple-reactions-list-tooltip",
        ref: this.reactionSelectorTooltip
      }, React__default.createElement("div", {
        className: "arrow"
      }), this.renderUsernames(this.state.users)), this.renderReactions(reactions), reactions.length !== 0 && React__default.createElement("li", {
        className: "str-chat__simple-reactions-list-item--last-number"
      }, this.getReactionCount()));
    }
  }]);

  return SimpleReactionsList;
}(React__default.PureComponent);

_defineProperty(SimpleReactionsList, "propTypes", {
  reactions: PropTypes.array,
  reaction_coutns: PropTypes.object,
  renderReactions: PropTypes.func,
  showTooltip: PropTypes.bool,

  /** Provide a list of reaction options [{name: 'angry', emoji: 'angry'}] */
  reactionOptions: PropTypes.array
});

_defineProperty(SimpleReactionsList, "defaultProps", {
  showTooltip: true,
  reactionOptions: defaultMinimalEmojis,
  emojiSetDef: emojiSetDef
});

var reactionSvg = '<svg width="14" height="14" xmlns="http://www.w3.org/2000/svg"><path d="M11.108 8.05a.496.496 0 0 1 .212.667C10.581 10.147 8.886 11 7 11c-1.933 0-3.673-.882-4.33-2.302a.497.497 0 0 1 .9-.417C4.068 9.357 5.446 10 7 10c1.519 0 2.869-.633 3.44-1.738a.495.495 0 0 1 .668-.212zm.792-1.826a.477.477 0 0 1-.119.692.541.541 0 0 1-.31.084.534.534 0 0 1-.428-.194c-.106-.138-.238-.306-.539-.306-.298 0-.431.168-.54.307A.534.534 0 0 1 9.538 7a.544.544 0 0 1-.31-.084.463.463 0 0 1-.117-.694c.33-.423.742-.722 1.394-.722.653 0 1.068.3 1.396.724zm-7 0a.477.477 0 0 1-.119.692.541.541 0 0 1-.31.084.534.534 0 0 1-.428-.194c-.106-.138-.238-.306-.539-.306-.299 0-.432.168-.54.307A.533.533 0 0 1 2.538 7a.544.544 0 0 1-.31-.084.463.463 0 0 1-.117-.694c.33-.423.742-.722 1.394-.722.653 0 1.068.3 1.396.724zM7 0a7 7 0 1 1 0 14A7 7 0 0 1 7 0zm4.243 11.243A5.96 5.96 0 0 0 13 7a5.96 5.96 0 0 0-1.757-4.243A5.96 5.96 0 0 0 7 1a5.96 5.96 0 0 0-4.243 1.757A5.96 5.96 0 0 0 1 7a5.96 5.96 0 0 0 1.757 4.243A5.96 5.96 0 0 0 7 13a5.96 5.96 0 0 0 4.243-1.757z" fillRule="evenodd"/></svg>';
var threadSvg = '<svg width="14" height="10" xmlns="http://www.w3.org/2000/svg"><path d="M8.516 3c4.78 0 4.972 6.5 4.972 6.5-1.6-2.906-2.847-3.184-4.972-3.184v2.872L3.772 4.994 8.516.5V3zM.484 5l4.5-4.237v1.78L2.416 5l2.568 2.125v1.828L.484 5z" fillRule="evenodd" /></svg>';
var optionsSvg = '<svg width="11" height="3" viewBox="0 0 11 3" xmlns="http://www.w3.org/2000/svg"><path d="M1.5 3a1.5 1.5 0 1 1 0-3 1.5 1.5 0 0 1 0 3zm4 0a1.5 1.5 0 1 1 0-3 1.5 1.5 0 0 1 0 3zm4 0a1.5 1.5 0 1 1 0-3 1.5 1.5 0 0 1 0 3z" fillRule="nonzero" /></svg>';
/**
 * MessageLivestream - Render component, should be used together with the Message component
 * Implements the look and feel for a livestream use case.
 *
 * @example ./docs/MessageLivestream.md
 * @extends PureComponent
 */

var MessageLivestream =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(MessageLivestream, _React$PureComponent);

  function MessageLivestream() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, MessageLivestream);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(MessageLivestream)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "state", {
      actionsBoxOpen: false,
      reactionSelectorOpen: false
    });

    _defineProperty(_assertThisInitialized(_this), "reactionSelectorRef", React__default.createRef());

    _defineProperty(_assertThisInitialized(_this), "editMessageFormRef", React__default.createRef());

    _defineProperty(_assertThisInitialized(_this), "onClickReactionsAction", function () {
      _this.setState({
        reactionSelectorOpen: true
      }, function () {
        return document.addEventListener('click', _this.hideReactions, false);
      });
    });

    _defineProperty(_assertThisInitialized(_this), "onClickOptionsAction", function () {
      _this.setState({
        actionsBoxOpen: true
      }, function () {
        return document.addEventListener('click', _this.hideOptions, false);
      });
    });

    _defineProperty(_assertThisInitialized(_this), "hideOptions", function () {
      _this.setState({
        actionsBoxOpen: false
      });

      document.removeEventListener('click', _this.hideOptions, false);
    });

    _defineProperty(_assertThisInitialized(_this), "hideReactions", function (e) {
      if (!_this.reactionSelectorRef.current.reactionSelector.current.contains(e.target)) {
        _this.setState({
          reactionSelectorOpen: false
        });

        document.removeEventListener('click', _this.hideReactions, false);
      }
    });

    _defineProperty(_assertThisInitialized(_this), "onMouseLeaveMessage", function () {
      _this.hideOptions();

      _this.setState({
        reactionSelectorOpen: false
      }, function () {
        return document.removeEventListener('click', _this.hideReactions, false);
      });
    });

    return _this;
  }

  _createClass(MessageLivestream, [{
    key: "isMine",
    value: function isMine() {
      return this.props.isMyMessage(this.props.message);
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      document.removeEventListener('click', this.hideOptions, false);
      document.removeEventListener('click', this.hideReactions, false);
    }
  }, {
    key: "render",
    value: function render() {
      var _this$props = this.props,
          Attachment$$1 = _this$props.Attachment,
          message = _this$props.message,
          groupStyles = _this$props.groupStyles,
          editing = _this$props.editing,
          clearEditingState = _this$props.clearEditingState,
          updateMessage = _this$props.updateMessage,
          initialMessage = _this$props.initialMessage,
          handleReaction = _this$props.handleReaction,
          actionsEnabled = _this$props.actionsEnabled,
          messageListRect = _this$props.messageListRect,
          channelConfig = _this$props.channelConfig,
          threadList = _this$props.threadList,
          handleOpenThread = _this$props.handleOpenThread,
          Message = _this$props.Message,
          onMentionsHoverMessage = _this$props.onMentionsHoverMessage,
          onMentionsClickMessage = _this$props.onMentionsClickMessage,
          unsafeHTML = _this$props.unsafeHTML,
          handleRetry = _this$props.handleRetry,
          handleAction = _this$props.handleAction,
          getMessageActions = _this$props.getMessageActions,
          isMyMessage = _this$props.isMyMessage,
          handleFlag = _this$props.handleFlag,
          handleMute = _this$props.handleMute,
          handleEdit = _this$props.handleEdit,
          handleDelete = _this$props.handleDelete;
      var hasAttachment = Boolean(message.attachments && message.attachments.length);
      var galleryImages = message.attachments.filter(function (item) {
        return item.type === 'image';
      });
      var attachments = message.attachments;

      if (galleryImages.length > 1) {
        attachments = message.attachments.filter(function (item) {
          return item.type !== 'image';
        });
      } else {
        galleryImages = [];
      }

      if (message.type === 'message.read') {
        return null;
      }

      if (message.type === 'message.date') {
        return null;
      }

      if (message.deleted_at) {
        return null;
      }

      if (editing) {
        return React__default.createElement("div", {
          className: "str-chat__message-team str-chat__message-team--".concat(groupStyles[0], " str-chat__message-team--editing"),
          onMouseLeave: this.onMouseLeaveMessage
        }, (groupStyles[0] === 'top' || groupStyles[0] === 'single') && React__default.createElement("div", {
          className: "str-chat__message-team-meta"
        }, React__default.createElement(Avatar, {
          image: message.user.image,
          name: message.user.name || message.user.id,
          size: 40
        })), React__default.createElement(exports.MessageInput, {
          Input: EditMessageForm,
          message: message,
          clearEditingState: clearEditingState,
          updateMessage: updateMessage
        }));
      }

      return React__default.createElement(React__default.Fragment, null, React__default.createElement("div", {
        className: "str-chat__message-livestream str-chat__message-livestream--".concat(groupStyles[0], " str-chat__message-livestream--").concat(message.type, " str-chat__message-livestream--").concat(message.status, " ").concat(initialMessage ? 'str-chat__message-livestream--initial-message' : ''),
        onMouseLeave: this.onMouseLeaveMessage
      }, this.state.reactionSelectorOpen && React__default.createElement(ReactionSelector, {
        reverse: false,
        handleReaction: handleReaction,
        actionsEnabled: actionsEnabled,
        detailedView: true,
        latest_reactions: message.latest_reactions,
        reaction_counts: message.reaction_counts,
        messageList: messageListRect,
        ref: this.reactionSelectorRef
      }), !initialMessage && message.type !== 'error' && message.type !== 'system' && message.type !== 'ephemeral' && message.status !== 'failed' && message.status !== 'sending' && React__default.createElement("div", {
        className: "str-chat__message-livestream-actions"
      }, React__default.createElement("span", {
        className: "str-chat__message-livestream-time"
      }, moment(message.created_at).format('h:mmA')), channelConfig && channelConfig.reactions && React__default.createElement("span", {
        onClick: this.onClickReactionsAction
      }, React__default.createElement("span", {
        dangerouslySetInnerHTML: {
          __html: reactionSvg
        }
      })), !threadList && channelConfig && channelConfig.replies && React__default.createElement("span", {
        dangerouslySetInnerHTML: {
          __html: threadSvg
        },
        onClick: function onClick(e) {
          return handleOpenThread(e, message);
        }
      }), getMessageActions().length > 0 && React__default.createElement("span", {
        onClick: this.onClickOptionsAction
      }, React__default.createElement("span", {
        dangerouslySetInnerHTML: {
          __html: optionsSvg
        }
      }), React__default.createElement(MessageActionsBox, {
        getMessageActions: getMessageActions,
        open: this.state.actionsBoxOpen,
        Message: Message,
        message: message,
        messageListRect: messageListRect,
        mine: isMyMessage(message),
        handleFlag: handleFlag,
        handleMute: handleMute,
        handleEdit: handleEdit,
        handleDelete: handleDelete
      }))), React__default.createElement("div", {
        className: "str-chat__message-livestream-left"
      }, React__default.createElement(Avatar, {
        image: message.user.image,
        name: message.user.name || message.user.id,
        size: 30
      })), React__default.createElement("div", {
        className: "str-chat__message-livestream-right"
      }, React__default.createElement("div", {
        className: "str-chat__message-livestream-content"
      }, React__default.createElement("div", {
        className: "str-chat__message-livestream-author"
      }, React__default.createElement("strong", null, message.user.name || message.user.id), message.type === 'error' && React__default.createElement("div", {
        className: "str-chat__message-team-error-header"
      }, "O", React__default.createElement(reactIntl.FormattedMessage, {
        id: "message.error_header",
        defaultMessage: "Only visible to you"
      }))), React__default.createElement("div", {
        className: isOnlyEmojis(message.text) ? 'str-chat__message-livestream-text--is-emoji' : '',
        onMouseOver: onMentionsHoverMessage,
        onClick: onMentionsClickMessage
      }, message.type !== 'error' && message.status !== 'failed' && !unsafeHTML && renderText(message), message.type !== 'error' && message.status !== 'failed' && unsafeHTML && React__default.createElement("div", {
        dangerouslySetInnerHTML: {
          __html: message.html
        }
      }), message.type === 'error' && !message.command && React__default.createElement("p", null, React__default.createElement("svg", {
        width: "14",
        height: "14",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        d: "M7 0a7 7 0 1 0 0 14A7 7 0 0 0 7 0zm.875 10.938a.438.438 0 0 1-.438.437h-.875a.438.438 0 0 1-.437-.438v-.874c0-.242.196-.438.438-.438h.875c.241 0 .437.196.437.438v.874zm0-2.626a.438.438 0 0 1-.438.438h-.875a.438.438 0 0 1-.437-.438v-5.25c0-.241.196-.437.438-.437h.875c.241 0 .437.196.437.438v5.25z",
        fill: "#EA152F",
        fillRule: "evenodd"
      })), message.text), message.type === 'error' && message.command && React__default.createElement("p", null, React__default.createElement("svg", {
        width: "14",
        height: "14",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        d: "M7 0a7 7 0 1 0 0 14A7 7 0 0 0 7 0zm.875 10.938a.438.438 0 0 1-.438.437h-.875a.438.438 0 0 1-.437-.438v-.874c0-.242.196-.438.438-.438h.875c.241 0 .437.196.437.438v.874zm0-2.626a.438.438 0 0 1-.438.438h-.875a.438.438 0 0 1-.437-.438v-5.25c0-.241.196-.437.438-.437h.875c.241 0 .437.196.437.438v5.25z",
        fill: "#EA152F",
        fillRule: "evenodd"
      })), React__default.createElement(reactIntl.FormattedMessage, {
        id: "command.error",
        defaultMessage: "<strong>/{command}</strong> is not a valid command",
        values: {
          command: message.command,
          strong: function strong() {
            for (var _len2 = arguments.length, chunks = new Array(_len2), _key2 = 0; _key2 < _len2; _key2++) {
              chunks[_key2] = arguments[_key2];
            }

            return React__default.createElement("strong", null, chunks);
          }
        }
      })), message.status === 'failed' && React__default.createElement("p", {
        onClick: handleRetry.bind(this, message)
      }, React__default.createElement("svg", {
        width: "14",
        height: "14",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        d: "M7 0a7 7 0 1 0 0 14A7 7 0 0 0 7 0zm.875 10.938a.438.438 0 0 1-.438.437h-.875a.438.438 0 0 1-.437-.438v-.874c0-.242.196-.438.438-.438h.875c.241 0 .437.196.437.438v.874zm0-2.626a.438.438 0 0 1-.438.438h-.875a.438.438 0 0 1-.437-.438v-5.25c0-.241.196-.437.438-.437h.875c.241 0 .437.196.437.438v5.25z",
        fill: "#EA152F",
        fillRule: "evenodd"
      })), React__default.createElement(reactIntl.FormattedMessage, {
        id: "message.failed",
        defaultMessage: "Message Failed \xB7 Click to try again"
      }))), hasAttachment && attachments.map(function (attachment, index) {
        return React__default.createElement(Attachment$$1, {
          key: "".concat(message.id, "-").concat(index),
          attachment: attachment,
          actionHandler: handleAction
        });
      }), galleryImages.length !== 0 && React__default.createElement(Gallery, {
        images: galleryImages
      }), React__default.createElement(SimpleReactionsList, {
        reaction_counts: message.reaction_counts,
        reactions: message.latest_reactions,
        handleReaction: handleReaction
      }), !initialMessage && React__default.createElement(MessageRepliesCountButton, {
        onClick: handleOpenThread,
        reply_count: message.reply_count
      })))));
    }
  }]);

  return MessageLivestream;
}(React__default.PureComponent);

_defineProperty(MessageLivestream, "propTypes", {
  /** The [message object](https://getstream.io/chat/docs/#message_format) */
  message: PropTypes.object,

  /**
   * The attachment UI component.
   * Default: [Attachment](https://github.com/GetStream/stream-chat-react/blob/master/src/components/Attachment.js)
   * */
  Attachment: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   *
   * @deprecated Its not recommended to use this anymore. All the methods in this HOC are provided explicitly.
   *
   * The higher order message component, most logic is delegated to this component
   * @see See [Message HOC](https://getstream.github.io/stream-chat-react/#message) for example
   *
   * */
  Message: PropTypes.oneOfType([PropTypes.node, PropTypes.func, PropTypes.object]).isRequired,

  /** render HTML instead of markdown. Posting HTML is only allowed server-side */
  unsafeHTML: PropTypes.bool,

  /** If its parent message in thread. */
  initialMessage: PropTypes.bool,

  /** Channel config object */
  channelConfig: PropTypes.object,

  /** If component is in thread list */
  threadList: PropTypes.bool,

  /** Function to open thread on current messxage */
  handleOpenThread: PropTypes.func,

  /** If the message is in edit state */
  editing: PropTypes.bool,

  /** Function to exit edit state */
  clearEditingState: PropTypes.func,

  /** Returns true if message belongs to current user */
  isMyMessage: PropTypes.func,

  /** Returns all allowed actions on message by current user e.g., [edit, delete, flag, mute] */
  getMessageActions: PropTypes.func,

  /**
   * Function to publish updates on message to channel
   *
   * @param message Updated [message object](https://getstream.io/chat/docs/#message_format)
   * */
  updateMessage: PropTypes.func,

  /**
   * Reattempt sending a message
   * @param message A [message object](https://getstream.io/chat/docs/#message_format) to resent.
   */
  handleRetry: PropTypes.func,

  /**
   * Add or remove reaction on message
   *
   * @param type Type of reaction - 'like' | 'love' | 'haha' | 'wow' | 'sad' | 'angry'
   * @param event Dom event which triggered this function
   */
  handleReaction: PropTypes.func,

  /** If actions such as edit, delete, flag, mute are enabled on message */
  actionsEnabled: PropTypes.bool,

  /** DOMRect object for parent MessageList component */
  messageListRect: PropTypes.object,

  /**
   * Handler for actions. Actions in combination with attachments can be used to build [commands](https://getstream.io/chat/docs/#channel_commands).
   *
   * @param name {string} Name of action
   * @param value {string} Value of action
   * @param event Dom event that triggered this handler
   */
  handleAction: PropTypes.func,

  /**
   * The handler for hover event on @mention in message
   *
   * @param event Dom hover event which triggered handler.
   * @param user Target user object
   */
  onMentionsHoverMessage: PropTypes.func,

  /**
   * The handler for click event on @mention in message
   *
   * @param event Dom click event which triggered handler.
   * @param user Target user object
   */
  onMentionsClickMessage: PropTypes.func
});

_defineProperty(MessageLivestream, "defaultProps", {
  Attachment: Attachment
});

var reactionSvg$1 = '<svg width="14" height="14" xmlns="http://www.w3.org/2000/svg"><path d="M11.108 8.05a.496.496 0 0 1 .212.667C10.581 10.147 8.886 11 7 11c-1.933 0-3.673-.882-4.33-2.302a.497.497 0 0 1 .9-.417C4.068 9.357 5.446 10 7 10c1.519 0 2.869-.633 3.44-1.738a.495.495 0 0 1 .668-.212zm.792-1.826a.477.477 0 0 1-.119.692.541.541 0 0 1-.31.084.534.534 0 0 1-.428-.194c-.106-.138-.238-.306-.539-.306-.298 0-.431.168-.54.307A.534.534 0 0 1 9.538 7a.544.544 0 0 1-.31-.084.463.463 0 0 1-.117-.694c.33-.423.742-.722 1.394-.722.653 0 1.068.3 1.396.724zm-7 0a.477.477 0 0 1-.119.692.541.541 0 0 1-.31.084.534.534 0 0 1-.428-.194c-.106-.138-.238-.306-.539-.306-.299 0-.432.168-.54.307A.533.533 0 0 1 2.538 7a.544.544 0 0 1-.31-.084.463.463 0 0 1-.117-.694c.33-.423.742-.722 1.394-.722.653 0 1.068.3 1.396.724zM7 0a7 7 0 1 1 0 14A7 7 0 0 1 7 0zm4.243 11.243A5.96 5.96 0 0 0 13 7a5.96 5.96 0 0 0-1.757-4.243A5.96 5.96 0 0 0 7 1a5.96 5.96 0 0 0-4.243 1.757A5.96 5.96 0 0 0 1 7a5.96 5.96 0 0 0 1.757 4.243A5.96 5.96 0 0 0 7 13a5.96 5.96 0 0 0 4.243-1.757z" fillRule="evenodd"/></svg>';
var threadSvg$1 = '<svg width="14" height="10" xmlns="http://www.w3.org/2000/svg"><path d="M8.516 3c4.78 0 4.972 6.5 4.972 6.5-1.6-2.906-2.847-3.184-4.972-3.184v2.872L3.772 4.994 8.516.5V3zM.484 5l4.5-4.237v1.78L2.416 5l2.568 2.125v1.828L.484 5z" fillRule="evenodd" /></svg>';
var optionsSvg$1 = '<svg width="11" height="3" viewBox="0 0 11 3" xmlns="http://www.w3.org/2000/svg"><path d="M1.5 3a1.5 1.5 0 1 1 0-3 1.5 1.5 0 0 1 0 3zm4 0a1.5 1.5 0 1 1 0-3 1.5 1.5 0 0 1 0 3zm4 0a1.5 1.5 0 1 1 0-3 1.5 1.5 0 0 1 0 3z" fillRule="nonzero" /></svg>';
/**
 * MessageTeam - Render component, should be used together with the Message component
 * Implements the look and feel for a team style collaboration environment
 *
 * @example ./docs/MessageTeam.md
 * @extends PureComponent
 */

exports.MessageTeam =
/*#__PURE__*/
function (_PureComponent) {
  _inherits(MessageTeam, _PureComponent);

  function MessageTeam() {
    var _getPrototypeOf2;

    var _this;

    _classCallCheck(this, MessageTeam);

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _possibleConstructorReturn(this, (_getPrototypeOf2 = _getPrototypeOf(MessageTeam)).call.apply(_getPrototypeOf2, [this].concat(args)));

    _defineProperty(_assertThisInitialized(_this), "state", {
      actionsBoxOpen: false,
      reactionSelectorOpen: false
    });

    _defineProperty(_assertThisInitialized(_this), "reactionSelectorRef", React__default.createRef());

    _defineProperty(_assertThisInitialized(_this), "editMessageFormRef", React__default.createRef());

    _defineProperty(_assertThisInitialized(_this), "onClickReactionsAction", function () {
      _this.setState({
        reactionSelectorOpen: true
      }, function () {
        return document.addEventListener('click', _this.hideReactions, false);
      });
    });

    _defineProperty(_assertThisInitialized(_this), "onClickOptionsAction", function () {
      _this.setState({
        actionsBoxOpen: true
      }, function () {
        return document.addEventListener('click', _this.hideOptions, false);
      });
    });

    _defineProperty(_assertThisInitialized(_this), "hideOptions", function () {
      _this.setState({
        actionsBoxOpen: false
      });

      document.removeEventListener('click', _this.hideOptions, false);
    });

    _defineProperty(_assertThisInitialized(_this), "hideReactions", function (e) {
      if (!_this.reactionSelectorRef.current.reactionSelector.current.contains(e.target)) {
        _this.setState({
          reactionSelectorOpen: false
        });

        document.removeEventListener('click', _this.hideReactions, false);
      }
    });

    _defineProperty(_assertThisInitialized(_this), "onMouseLeaveMessage", function () {
      _this.hideOptions();

      _this.setState({
        reactionSelectorOpen: false
      }, function () {
        return document.removeEventListener('click', _this.hideReactions, false);
      });
    });

    _defineProperty(_assertThisInitialized(_this), "renderStatus", function () {
      var _this$props = _this.props,
          readBy = _this$props.readBy,
          message = _this$props.message,
          threadList = _this$props.threadList,
          client = _this$props.client,
          lastReceivedId = _this$props.lastReceivedId,
          intl = _this$props.intl;

      if (!_this.isMine() || message.type === 'error') {
        return null;
      }

      var justReadByMe = readBy.length === 1 && readBy[0].id === client.user.id;

      if (message.status === 'sending') {
        return React__default.createElement("span", {
          className: "str-chat__message-team-status"
        }, React__default.createElement(Tooltip, null, React__default.createElement(reactIntl.FormattedMessage, {
          id: "message.status.sending",
          defaultMessage: "Sending..."
        })), React__default.createElement(LoadingIndicator, {
          isLoading: true
        }));
      } else if (readBy.length !== 0 && !threadList && !justReadByMe) {
        var readByOthers = readBy.filter(function (item) {
          return item.id !== client.user.id;
        });
        var lastReadUser = readByOthers[0];
        return React__default.createElement("span", {
          className: "str-chat__message-team-status"
        }, React__default.createElement(Tooltip, null, formatStatusArray(intl, readByOthers)), React__default.createElement(Avatar, {
          name: lastReadUser.name || lastReadUser.id,
          image: lastReadUser.image,
          size: 15
        }), readBy.length - 1 > 1 && React__default.createElement("span", {
          className: "str-chat__message-team-status-number"
        }, readBy.length - 1));
      } else if (message.status === 'received' && message.id === lastReceivedId && !threadList) {
        return React__default.createElement("span", {
          className: "str-chat__message-team-status"
        }, React__default.createElement(Tooltip, null, React__default.createElement(reactIntl.FormattedMessage, {
          id: "message.status.delivered",
          defaultMessage: "Delivered"
        })), React__default.createElement("svg", {
          width: "16",
          height: "16",
          xmlns: "http://www.w3.org/2000/svg"
        }, React__default.createElement("path", {
          d: "M8 0a8 8 0 1 1 0 16A8 8 0 0 1 8 0zm3.72 6.633a.955.955 0 1 0-1.352-1.352L6.986 8.663 5.633 7.31A.956.956 0 1 0 4.28 8.663l2.029 2.028a.956.956 0 0 0 1.353 0l4.058-4.058z",
          fill: "#006CFF",
          fillRule: "evenodd"
        })));
      } else {
        return null;
      }
    });

    return _this;
  }

  _createClass(MessageTeam, [{
    key: "isMine",
    value: function isMine() {
      return this.props.isMyMessage(this.props.message);
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      document.removeEventListener('click', this.hideOptions, false);
      document.removeEventListener('click', this.hideReactions, false);
    }
  }, {
    key: "renderAttachments",
    value: function renderAttachments(attachments) {
      var _this$props2 = this.props,
          Attachment$$1 = _this$props2.Attachment,
          message = _this$props2.message,
          handleAction = _this$props2.handleAction;
      return attachments.map(function (attachment, index) {
        return React__default.createElement(Attachment$$1, {
          key: "".concat(message.id, "-").concat(index),
          attachment: attachment,
          actionHandler: handleAction
        });
      });
    } // eslint-disable-next-line

  }, {
    key: "render",
    value: function render() {
      var _this$props3 = this.props,
          message = _this$props3.message,
          groupStyles = _this$props3.groupStyles,
          editing = _this$props3.editing,
          clearEditingState = _this$props3.clearEditingState,
          updateMessage = _this$props3.updateMessage,
          threadList = _this$props3.threadList,
          initialMessage = _this$props3.initialMessage,
          handleReaction = _this$props3.handleReaction,
          channelConfig = _this$props3.channelConfig,
          handleOpenThread = _this$props3.handleOpenThread,
          Message = _this$props3.Message,
          messageListRect = _this$props3.messageListRect,
          onMentionsHoverMessage = _this$props3.onMentionsHoverMessage,
          onMentionsClickMessage = _this$props3.onMentionsClickMessage,
          unsafeHTML = _this$props3.unsafeHTML,
          handleRetry = _this$props3.handleRetry,
          getMessageActions = _this$props3.getMessageActions,
          isMyMessage = _this$props3.isMyMessage,
          handleFlag = _this$props3.handleFlag,
          handleMute = _this$props3.handleMute,
          handleEdit = _this$props3.handleEdit,
          handleDelete = _this$props3.handleDelete,
          intl = _this$props3.intl;

      if (message.type === 'message.read') {
        return null;
      }

      var hasAttachment = Boolean(message.attachments && message.attachments.length);

      if (message.deleted_at) {
        return null;
      }

      var galleryImages = message.attachments && message.attachments.filter(function (item) {
        return item.type === 'image';
      });
      var attachments = message.attachments;

      if (galleryImages && galleryImages.length > 1) {
        attachments = message.attachments.filter(function (item) {
          return item.type !== 'image';
        });
      } else {
        galleryImages = [];
      } // determine reaction selector alignment


      var reactionDirection = 'left';

      if (editing) {
        return React__default.createElement("div", {
          className: "str-chat__message-team str-chat__message-team--".concat(groupStyles[0], " str-chat__message-team--editing"),
          onMouseLeave: this.onMouseLeaveMessage
        }, (groupStyles[0] === 'top' || groupStyles[0] === 'single') && React__default.createElement("div", {
          className: "str-chat__message-team-meta"
        }, React__default.createElement(Avatar, {
          image: message.user.image,
          name: message.user.name || message.user.id,
          size: 40
        })), React__default.createElement(exports.MessageInput, {
          Input: EditMessageForm,
          message: message,
          clearEditingState: clearEditingState,
          updateMessage: updateMessage
        }));
      }

      return React__default.createElement(React__default.Fragment, null, React__default.createElement("div", {
        className: "str-chat__message-team str-chat__message-team--".concat(groupStyles[0], " str-chat__message-team--").concat(message.type, " ").concat(threadList ? 'thread-list' : '', " str-chat__message-team--").concat(message.status),
        onMouseLeave: this.onMouseLeaveMessage
      }, React__default.createElement("div", {
        className: "str-chat__message-team-meta"
      }, groupStyles[0] === 'top' || groupStyles[0] === 'single' || initialMessage ? React__default.createElement("div", null, React__default.createElement(Avatar, {
        image: message.user.image,
        name: message.user.name || message.user.id,
        size: 40
      })) : React__default.createElement("div", {
        style: {
          width: 40,
          marginRight: 0
        }
      }), React__default.createElement("time", {
        dateTime: message.created_at,
        title: message.created_at
      }, Boolean(Date.parse(message.created_at)) && moment(message.created_at).format('h:mmA'))), React__default.createElement("div", {
        className: "str-chat__message-team-group"
      }, (groupStyles[0] === 'top' || groupStyles[0] === 'single' || initialMessage) && React__default.createElement("div", {
        className: "str-chat__message-team-author"
      }, React__default.createElement("strong", null, message.user.name || message.user.id), message.type === 'error' && React__default.createElement("div", {
        className: "str-chat__message-team-error-header"
      }, "Only visible to you")), React__default.createElement("div", {
        className: "str-chat__message-team-content str-chat__message-team-content--".concat(groupStyles[0], " str-chat__message-team-content--").concat(message.text === '' ? 'image' : 'text')
      }, !initialMessage && message.status !== 'sending' && message.status !== 'failed' && message.type !== 'system' && message.type !== 'ephemeral' && message.type !== 'error' && React__default.createElement("div", {
        className: "str-chat__message-team-actions"
      }, this.state.reactionSelectorOpen && React__default.createElement(ReactionSelector, {
        handleReaction: handleReaction,
        latest_reactions: message.latest_reactions,
        reaction_counts: message.reaction_counts,
        detailedView: true,
        direction: reactionDirection,
        ref: this.reactionSelectorRef
      }), channelConfig && channelConfig.reactions && React__default.createElement("span", {
        title: intl.formatMessage({
          id: 'message.title.reactions',
          defaultMessage: 'Reactions'
        }),
        dangerouslySetInnerHTML: {
          __html: reactionSvg$1
        },
        onClick: this.onClickReactionsAction
      }), !threadList && channelConfig && channelConfig.replies && React__default.createElement("span", {
        title: intl.formatMessage({
          id: 'message.title.open_thread',
          defaultMessage: 'Start a thread'
        }),
        dangerouslySetInnerHTML: {
          __html: threadSvg$1
        },
        onClick: function onClick(e) {
          return handleOpenThread(e, message);
        }
      }), getMessageActions().length > 0 && React__default.createElement("span", {
        onClick: this.onClickOptionsAction
      }, React__default.createElement("span", {
        title: intl.formatMessage({
          id: 'message.title.message_actions',
          defaultMessage: 'Message actions'
        }),
        dangerouslySetInnerHTML: {
          __html: optionsSvg$1
        }
      }), React__default.createElement(MessageActionsBox, {
        getMessageActions: getMessageActions,
        Message: Message,
        open: this.state.actionsBoxOpen,
        message: message,
        messageListRect: messageListRect,
        mine: isMyMessage(message),
        handleFlag: handleFlag,
        handleMute: handleMute,
        handleEdit: handleEdit,
        handleDelete: handleDelete
      }))), React__default.createElement("span", {
        className: isOnlyEmojis(message.text) ? 'str-chat__message-team-text--is-emoji' : '',
        onMouseOver: onMentionsHoverMessage,
        onClick: onMentionsClickMessage
      }, unsafeHTML ? React__default.createElement("div", {
        dangerouslySetInnerHTML: {
          __html: message.html
        }
      }) : renderText(message)), galleryImages.length !== 0 && React__default.createElement(Gallery, {
        images: galleryImages
      }), message.text === '' && this.renderAttachments(attachments), message.latest_reactions && message.latest_reactions.length !== 0 && message.text !== '' && React__default.createElement(SimpleReactionsList, {
        reaction_counts: message.reaction_counts,
        handleReaction: handleReaction,
        reactions: message.latest_reactions
      }), message.status === 'failed' && React__default.createElement("button", {
        className: "str-chat__message-team-failed",
        onClick: handleRetry.bind(this, message)
      }, React__default.createElement("svg", {
        width: "14",
        height: "14",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        d: "M7 0a7 7 0 1 0 0 14A7 7 0 0 0 7 0zm.875 10.938a.438.438 0 0 1-.438.437h-.875a.438.438 0 0 1-.437-.438v-.874c0-.242.196-.438.438-.438h.875c.241 0 .437.196.437.438v.874zm0-2.626a.438.438 0 0 1-.438.438h-.875a.438.438 0 0 1-.437-.438v-5.25c0-.241.196-.437.438-.437h.875c.241 0 .437.196.437.438v5.25z",
        fill: "#EA152F",
        fillRule: "evenodd"
      })), React__default.createElement(reactIntl.FormattedMessage, {
        id: "message.failed",
        defaultMessage: "Message Failed \xB7 Click to try again"
      }))), this.renderStatus(), message.text !== '' && hasAttachment && this.renderAttachments(attachments), message.latest_reactions && message.latest_reactions.length !== 0 && message.text === '' && React__default.createElement(SimpleReactionsList, {
        reaction_counts: message.reaction_counts,
        handleReaction: handleReaction,
        reactions: message.latest_reactions
      }), !threadList && React__default.createElement(MessageRepliesCountButton, {
        onClick: handleOpenThread,
        reply_count: message.reply_count
      }))));
    }
  }]);

  return MessageTeam;
}(React.PureComponent);

_defineProperty(exports.MessageTeam, "propTypes", {
  /** The [message object](https://getstream.io/chat/docs/#message_format) */
  message: PropTypes.object,

  /**
   * The attachment UI component.
   * Default: [Attachment](https://github.com/GetStream/stream-chat-react/blob/master/src/components/Attachment.js)
   * */
  Attachment: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   *
   * @deprecated Its not recommended to use this anymore. All the methods in this HOC are provided explicitly.
   *
   * The higher order message component, most logic is delegated to this component
   * @see See [Message HOC](https://getstream.github.io/stream-chat-react/#message) for example
   * */
  Message: PropTypes.oneOfType([PropTypes.node, PropTypes.func, PropTypes.object]).isRequired,

  /** render HTML instead of markdown. Posting HTML is only allowed server-side */
  unsafeHTML: PropTypes.bool,

  /** Client object */
  client: PropTypes.object,

  /** If its parent message in thread. */
  initialMessage: PropTypes.bool,

  /** Channel config object */
  channelConfig: PropTypes.object,

  /** If component is in thread list */
  threadList: PropTypes.bool,

  /** Function to open thread on current messxage */
  handleOpenThread: PropTypes.func,

  /** If the message is in edit state */
  editing: PropTypes.bool,

  /** Function to exit edit state */
  clearEditingState: PropTypes.func,

  /** Returns true if message belongs to current user */
  isMyMessage: PropTypes.func,

  /** Returns all allowed actions on message by current user e.g., [edit, delete, flag, mute] */
  getMessageActions: PropTypes.func,

  /**
   * Function to publish updates on message to channel
   *
   * @param message Updated [message object](https://getstream.io/chat/docs/#message_format)
   * */
  updateMessage: PropTypes.func,

  /**
   * Reattempt sending a message
   * @param message A [message object](https://getstream.io/chat/docs/#message_format) to resent.
   */
  handleRetry: PropTypes.func,

  /**
   * Add or remove reaction on message
   *
   * @param type Type of reaction - 'like' | 'love' | 'haha' | 'wow' | 'sad' | 'angry'
   * @param event Dom event which triggered this function
   */
  handleReaction: PropTypes.func,

  /** DOMRect object for parent MessageList component */
  messageListRect: PropTypes.object,

  /**
   * Handler for actions. Actions in combination with attachments can be used to build [commands](https://getstream.io/chat/docs/#channel_commands).
   *
   * @param name {string} Name of action
   * @param value {string} Value of action
   * @param event Dom event that triggered this handler
   */
  handleAction: PropTypes.func,

  /**
   * The handler for hover event on @mention in message
   *
   * @param event Dom hover event which triggered handler.
   * @param user Target user object
   */
  onMentionsHoverMessage: PropTypes.func,

  /**
   * The handler for click event on @mention in message
   *
   * @param event Dom click event which triggered handler.
   * @param user Target user object
   */
  onMentionsClickMessage: PropTypes.func,

  /** Position of message in group. Possible values: top, bottom, middle, single */
  groupStyles: PropTypes.array
});

_defineProperty(exports.MessageTeam, "defaultProps", {
  Attachment: Attachment,
  groupStyles: ['single']
});

exports.MessageTeam = reactIntl.injectIntl(exports.MessageTeam);

function ownKeys$a(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread$a(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys$a(source, true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys$a(source).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }
/**
 * Thread - The Thread renders a parent message with a list of replies. Use the standard message list of the main channel's messages.
 * The thread is only used for the list of replies to a message.
 * Underlying MessageList, MessageInput and Message components can be customized using props:
 * - additionalParentMessageProps
 * - additionalMessageListProps
 * - additionalMessageInputProps
 *
 * @example ./docs/Thread.md
 * @extends Component
 */

exports.Thread =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(Thread, _React$PureComponent);

  function Thread() {
    _classCallCheck(this, Thread);

    return _possibleConstructorReturn(this, _getPrototypeOf(Thread).apply(this, arguments));
  }

  _createClass(Thread, [{
    key: "render",
    value: function render() {
      if (!this.props.thread) {
        return null;
      }

      var parentID = this.props.thread.id;
      var cid = this.props.channel.cid;
      var key = "thread-".concat(parentID, "-").concat(cid); // We use a wrapper to make sure the key variable is set.
      // this ensures that if you switch thread the component is recreated

      return React__default.createElement(ThreadInner, _extends({}, this.props, {
        key: key
      }));
    }
  }]);

  return Thread;
}(React__default.PureComponent);

_defineProperty(exports.Thread, "propTypes", {
  /** Display the thread on 100% width of it's container. Useful for mobile style view */
  fullWidth: PropTypes.bool,

  /** Make input focus on mounting thread */
  autoFocus: PropTypes.bool,

  /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)** */
  channel: PropTypes.object.isRequired,

  /** **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)** */
  Message: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  /**
   * **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)**
   * The thread (the parent [message object](https://getstream.io/chat/docs/#message_format)) */
  thread: PropTypes.oneOfType([PropTypes.object, PropTypes.bool]),

  /**
   * **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)**
   * The array of immutable messages to render. By default they are provided by parent Channel component */
  threadMessages: PropTypes.array.isRequired,

  /**
   * **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)**
   *
   * Function which provides next page of thread messages.
   * loadMoreThread is called when infinite scroll wants to load more messages
   * */
  loadMoreThread: PropTypes.func.isRequired,

  /**
   * **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)**
   * If there are more messages available, set to false when the end of pagination is reached.
   * */
  threadHasMore: PropTypes.bool,

  /**
   * **Available from [channel context](https://getstream.github.io/stream-chat-react/#channel)**
   * If the thread is currently loading more messages. This is helpful to display a loading indicator on threadlist */
  threadLoadingMore: PropTypes.bool,

  /**
   * Additional props for underlying Message component of parent message at the top.
   * Available props - https://getstream.github.io/stream-chat-react/#message
   * */
  additionalParentMessageProps: PropTypes.object,

  /**
   * Additional props for underlying MessageList component.
   * Available props - https://getstream.github.io/stream-chat-react/#messagelist
   * */
  additionalMessageListProps: PropTypes.object,

  /**
   * Additional props for underlying MessageInput component.
   * Available props - https://getstream.github.io/stream-chat-react/#messageinput
   * */
  additionalMessageInputProps: PropTypes.object,

  /** Customized MessageInput component to used within Thread instead of default MessageInput */
  MessageInput: PropTypes.oneOfType([PropTypes.node, PropTypes.func])
});

_defineProperty(exports.Thread, "defaultProps", {
  threadHasMore: true,
  threadLoadingMore: true,
  fullWidth: false,
  autoFocus: true,
  MessageInput: exports.MessageInput
});

var ThreadInner =
/*#__PURE__*/
function (_React$PureComponent2) {
  _inherits(ThreadInner, _React$PureComponent2);

  function ThreadInner(props) {
    var _this;

    _classCallCheck(this, ThreadInner);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(ThreadInner).call(this, props));
    _this.messageList = React__default.createRef();
    return _this;
  }

  _createClass(ThreadInner, [{
    key: "componentDidMount",
    value: function () {
      var _componentDidMount = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee() {
        var parentID;
        return _regeneratorRuntime.wrap(function _callee$(_context) {
          while (1) {
            switch (_context.prev = _context.next) {
              case 0:
                parentID = this.props.thread.id;

                if (!(parentID && this.props.thread.reply_count)) {
                  _context.next = 4;
                  break;
                }

                _context.next = 4;
                return this.props.loadMoreThread();

              case 4:
              case "end":
                return _context.stop();
            }
          }
        }, _callee, this);
      }));

      function componentDidMount() {
        return _componentDidMount.apply(this, arguments);
      }

      return componentDidMount;
    }()
  }, {
    key: "getSnapshotBeforeUpdate",
    value: function getSnapshotBeforeUpdate(prevProps) {
      // Are we adding new items to the list?
      // Capture the scroll position so we can adjust scroll later.
      if (prevProps.threadMessages.length < this.props.threadMessages.length) {
        var list = this.messageList.current;
        return list.scrollHeight - list.scrollTop;
      }

      return null;
    }
  }, {
    key: "componentDidUpdate",
    value: function () {
      var _componentDidUpdate = _asyncToGenerator(
      /*#__PURE__*/
      _regeneratorRuntime.mark(function _callee2(prevProps, prevState, snapshot) {
        var parentID, list, scrollDown;
        return _regeneratorRuntime.wrap(function _callee2$(_context2) {
          while (1) {
            switch (_context2.prev = _context2.next) {
              case 0:
                parentID = this.props.thread.id;

                if (!(parentID && this.props.thread.reply_count > 0 && this.props.threadMessages.length === 0)) {
                  _context2.next = 4;
                  break;
                }

                _context2.next = 4;
                return this.props.loadMoreThread();

              case 4:
                // If we have a snapshot value, we've just added new items.
                // Adjust scroll so these new items don't push the old ones out of view.
                // (snapshot here is the value returned from getSnapshotBeforeUpdate)
                if (snapshot !== null) {
                  list = this.messageList.current;

                  scrollDown = function scrollDown() {
                    list.scrollTop = list.scrollHeight - snapshot;
                  };

                  scrollDown(); // scroll down after images load again

                  setTimeout(scrollDown, 100);
                }

              case 5:
              case "end":
                return _context2.stop();
            }
          }
        }, _callee2, this);
      }));

      function componentDidUpdate(_x, _x2, _x3) {
        return _componentDidUpdate.apply(this, arguments);
      }

      return componentDidUpdate;
    }()
  }, {
    key: "render",
    value: function render() {
      var _this2 = this;

      if (!this.props.thread) {
        return null;
      }

      var read = {};
      return React__default.createElement("div", {
        className: "str-chat__thread ".concat(this.props.fullWidth ? 'str-chat__thread--full' : '')
      }, React__default.createElement("div", {
        className: "str-chat__thread-header"
      }, React__default.createElement("div", {
        className: "str-chat__thread-header-details"
      }, React__default.createElement("strong", null, "Thread"), React__default.createElement("small", null, this.props.thread.reply_count, " replies")), React__default.createElement("button", {
        onClick: function onClick(e) {
          return _this2.props.closeThread(e);
        },
        className: "str-chat__square-button"
      }, React__default.createElement("svg", {
        width: "10",
        height: "10",
        xmlns: "http://www.w3.org/2000/svg"
      }, React__default.createElement("path", {
        d: "M9.916 1.027L8.973.084 5 4.058 1.027.084l-.943.943L4.058 5 .084 8.973l.943.943L5 5.942l3.973 3.974.943-.943L5.942 5z",
        fillRule: "evenodd"
      })))), React__default.createElement("div", {
        className: "str-chat__thread-list",
        ref: this.messageList
      }, React__default.createElement(exports.Message, _extends({
        message: this.props.thread,
        initialMessage: true,
        threadList: true,
        Message: this.props.Message // TODO: remove the following line in next release, since we already have additionalParentMessageProps now.

      }, this.props, this.props.additionalParentMessageProps)), React__default.createElement("div", {
        className: "str-chat__thread-start"
      }, "Start of a new thread"), React__default.createElement(exports.MessageList, _extends({
        messages: this.props.threadMessages,
        read: read,
        threadList: true,
        loadMore: this.props.loadMoreThread,
        hasMore: this.props.threadHasMore,
        loadingMore: this.props.threadLoadingMore,
        Message: this.props.Message
      }, this.props.additionalMessageListProps)), smartRender(this.props.MessageInput, _objectSpread$a({
        MessageInputSmall: exports.MessageInputSmall,
        parent: this.props.thread,
        focus: this.props.autoFocus
      }, this.props.additionalMessageInputProps))));
    }
  }]);

  return ThreadInner;
}(React__default.PureComponent);

_defineProperty(ThreadInner, "propTypes", {
  /** Channel is passed via the Channel Context */
  channel: PropTypes.object.isRequired,

  /** the thread (just a message) that we're rendering */
  thread: PropTypes.object.isRequired
});

exports.Thread = withChannelContext(exports.Thread);

var TypingIndicator =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(TypingIndicator, _React$PureComponent);

  function TypingIndicator() {
    _classCallCheck(this, TypingIndicator);

    return _possibleConstructorReturn(this, _getPrototypeOf(TypingIndicator).apply(this, arguments));
  }

  _createClass(TypingIndicator, [{
    key: "render",
    value: function render() {
      var _this = this;

      // if (Object.keys(this.props.typing).length <= 0) {
      //   return null;
      // }
      var typing = Object.values(this.props.typing);
      var show;

      if (typing.length === 0 || typing.length === 1 && typing[0].user.id === this.props.client.user.id) {
        show = false;
      } else {
        show = true;
      }

      return React__default.createElement("div", {
        className: "str-chat__typing-indicator ".concat(show ? 'str-chat__typing-indicator--typing' : '')
      }, React__default.createElement("div", {
        className: "str-chat__typing-indicator__avatars"
      }, typing.filter(function (_ref) {
        var user = _ref.user;
        return user.id !== _this.props.client.user.id;
      }).map(function (_ref2) {
        var user = _ref2.user;
        return React__default.createElement(Avatar, {
          image: user.image,
          size: 32,
          name: user.name || user.id,
          key: user.id
        });
      })), React__default.createElement("div", {
        className: "str-chat__typing-indicator__dots"
      }, React__default.createElement("span", {
        className: "str-chat__typing-indicator__dot"
      }), React__default.createElement("span", {
        className: "str-chat__typing-indicator__dot"
      }), React__default.createElement("span", {
        className: "str-chat__typing-indicator__dot"
      })));
    }
  }]);

  return TypingIndicator;
}(React__default.PureComponent);

/**
 * Window - A UI component for conditionally displaying thread or channel.
 *
 * @example ./docs/Window.md
 * @extends PureComponent
 */

exports.Window =
/*#__PURE__*/
function (_React$PureComponent) {
  _inherits(Window, _React$PureComponent);

  function Window() {
    _classCallCheck(this, Window);

    return _possibleConstructorReturn(this, _getPrototypeOf(Window).apply(this, arguments));
  }

  _createClass(Window, [{
    key: "render",
    value: function render() {
      // If thread is active and window should hide on thread. Return null
      if (this.props.thread && this.props.hideOnThread) {
        return null;
      }

      return React__default.createElement("div", {
        className: "str-chat__main-panel"
      }, this.props.children);
    }
  }]);

  return Window;
}(React__default.PureComponent);

_defineProperty(exports.Window, "propTypes", {
  /** show or hide the window when a thread is active */
  hideOnThread: PropTypes.bool,

  /** Flag if thread is open or not */
  thread: PropTypes.oneOfType([PropTypes.bool, PropTypes.object])
});

_defineProperty(exports.Window, "defaultProps", {
  hideOnThread: false
});

exports.Window = withChannelContext(exports.Window);

// Setup

exports.Avatar = Avatar;
exports.Chat = Chat;
exports.Attachment = Attachment;
exports.ChannelPreviewCompact = ChannelPreviewCompact;
exports.LoadMorePaginator = LoadMorePaginator;
exports.LoadingChannels = LoadingChannels;
exports.InfiniteScrollPaginator = InfiniteScrollPaginator;
exports.LoadingIndicator = LoadingIndicator;
exports.MessageCommerce = MessageCommerce;
exports.MessageLivestream = MessageLivestream;
exports.SendButton = SendButton;
exports.TypingIndicator = TypingIndicator;
exports.ReactionSelector = ReactionSelector;
exports.ReactionsList = ReactionsList;
exports.ChatAutoComplete = ChatAutoComplete;
exports.emojiSetDef = emojiSetDef;
exports.commonEmoji = commonEmoji;
exports.defaultMinimalEmojis = defaultMinimalEmojis;
exports.emojiData = emojiData;
exports.isOnlyEmojis = isOnlyEmojis;
exports.isPromise = isPromise;
exports.byDate = byDate;
exports.formatTypingArray = formatTypingArray;
exports.formatStatusArray = formatStatusArray;
exports.renderText = renderText;
exports.generateRandomId = generateRandomId;
exports.smartRender = smartRender;
exports.MESSAGE_ACTIONS = MESSAGE_ACTIONS;
exports.languageChangedEventName = languageChangedEventName;
exports.languageChangedEvent = languageChangedEvent;
exports.ChatContext = ChatContext;
exports.withChatContext = withChatContext;
exports.ChannelContext = ChannelContext;
exports.withChannelContext = withChannelContext;
//# sourceMappingURL=index.js.map
