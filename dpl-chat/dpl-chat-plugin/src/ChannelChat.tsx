// In this Code file the Chat-Key can be change
// If you want to change the Key of the Client change the export const config (starts on line 31)
// than commit your changes
// after that use the command "yarn build:clean" in dpl\dpl-chat\dpl-chat-plugin
// Remember: check index.js in \dpl\dpl-chat\server there you can Change Webhook Chat Keys

import React, { Component } from "react";
import { StreamChat } from "stream-chat";
import {
  Window,
  Chat,
  ChannelList,
  Channel,
  ChannelHeader,
  MessageList,
  Thread,
  TypingIndicator,
  MessageInput,
  ChannelPreviewMessenger,
} from "stream-chat-react";
import "./ChannelChat.css";
import Badge from "@material-ui/core/Badge";
import { window } from "./types";

interface IChatState {
  open: boolean;
  totalUnreadCount: number;
  sort: {};
}

export default class ChannelChat extends Component<{}, IChatState> {
  constructor(props) {
    super(props);
    this.state = {
      open: false,
      totalUnreadCount: 0,
      sort: { last_message_at: -1 },
    };
  }

  filters = {
    type: "messaging",
    members: { $in: [window.dplChatConfig.userId] },
  }; //channelFilter
  sort = { last_message_at: -1 };
  public chat?: Chat;

  Button = ({ open, onClick }) => (
    <div
      onClick={onClick}
      className={`button ${open ? "button--open" : "button--closed"}`}
    >
      {" "}
      <Badge badgeContent={this.state.totalUnreadCount} color="secondary">
        {open ? (
          <svg width="20" height="20" xmlns="https://www.w3.org/TR/SVG/">
            <path
              d="M19.333 2.547l-1.88-1.88L10 8.12 2.547.667l-1.88 1.88L8.12 10 .667 17.453l1.88 1.88L10 11.88l7.453 7.453 1.88-1.88L11.88 10z"
              fillRule="evenodd"
            />
          </svg>
        ) : (
          <svg
            xmlns="http://www.w3.org/2000/svg"
            width="30"
            height="30"
            viewBox="0 0 24 24"
          >
            <path d="M20 2H4c-1.1 0-1.99.9-1.99 2L2 22l4-4h14c1.1 0 2-.9 2-2V4c0-1.1-.9-2-2-2zM6 9h12v2H6V9zm8 5H6v-2h8v2zm4-6H6V6h12v2z" />
            <path d="M0 0h24v24H0z" fill="none" />
          </svg>
        )}
      </Badge>
    </div>
  );

  getClient() {
    //only chat-client to not overwrite names on testing if you not want to start the whole dpl web app
    if (
      window.dplChatConfig.userId ===
      "dpl-welcome-bot-7bc598d1-34f8-4882-ad69-47657fa0445e"
    ) {
      window.dplChatConfig.userName = "DPL Welcome Bot"; // name of the function used User to send messages
    }
    if (window.dplChatConfig.userId === "dpl_kundenbetreuung_at_dpl_ltms_com") {
      window.dplChatConfig.userName = "DPL Kundenbetreuung"; // name of the function used User to send messages
    }
    const chatClient = new StreamChat(window.dplChatConfig.streamKey);
    // const userToken = window.dplChatConfig.userToken;
    const userToken = chatClient.devToken(window.dplChatConfig.userId); // comment out: window.dplChatConfig.userToken ||
    //TODO: this is a reminder, only use the next line if the chat hase the same location in the Dashboard
    const setBaseUrl = chatClient.setBaseURL(
      "https://chat-proxy-dublin.stream-io-api.com"
    );
    chatClient
      .setUser(
        {
          id: window.dplChatConfig.userId,
          name: window.dplChatConfig.userName,
          image: window.dplChatConfig.userImage,
          email: window.dplChatConfig.userEmail,
          userLanguage: window.dplChatConfig.userLanguage,
        },

        userToken
        //Todo: set a aktive auth Token, need to change it after the develope mode
        //   async () => {
        //     const token = await fetchTokenFromSomeApi(); // you need to implement this api

        //     return token;
        // }
      )
      .then((value: any) => {
        this.updateUnreadCountTotal(value.me.total_unread_count);
      });
    return chatClient;
  }

  client = this.getClient();
  updateUnreadCountTotal = (count: number) => {
    this.setState({ ...this.state, ...{ totalUnreadCount: count } });
  };

  subscription = this.client.on((event) => {
    // console.log(event);
    switch (event.type) {
      case "notification.added_to_channel":
      case "notification.removed_from_channel":
      case "notification.message_new":
      case "notification.mark_read": {
        this.updateUnreadCountTotal(event.total_unread_count);
        break;
      }
      default:
        break;
    }
  });

  toggleDemo = () => {
    if (this.state.open) {
      this.setState(() => ({ open: false }));
    } else {
      this.setState(() => ({ open: true }));
    }
  };

  hide = {
    display: "none",
  };

  render() {
    return (
      <div>
        <div className={`wrapper ${this.state.open ? "wrapper--open" : ""}`}>
          <div
            className="wrapper-chat"
            style={this.state.open ? {} : this.hide} //TODO: remove hide
          >
            <Chat
              locale={window.dplChatConfig.language || "de"}
              client={this.client as any}
              theme={"messaging light"}
              ref={(chat) => {
                if (chat) {
                  this.chat = chat;
                }
              }}
            >
              <ChannelList
                filters={this.filters}
                sort={this.sort}
                Preview={ChannelPreviewMessenger}
              ></ChannelList>
              <Channel>
                <Window>
                  <ChannelHeader></ChannelHeader>
                  <MessageList TypingIndicator={TypingIndicator} />
                  <MessageInput />
                </Window>
                <Thread />
              </Channel>
            </Chat>
          </div>
          <this.Button onClick={this.toggleDemo} open={this.state.open} />
        </div>
      </div>
    );
  }
}
