import React, { Component } from "react";
import "./App.css";
import ChannelChat from "./ChannelChat";
import { window } from "./types";
type channelType =
  | ""
  | "accounting-record"
  | "voucher"
  | "order"
  | "demand"
  | "supply"
  | "transport"
  | "general"
  | "order-load"
  | "load-carrier-receipt";

class App extends Component {
  constructor(props) {
    super(props);
  }
  channelChat?: ChannelChat;
  render() {
    return (
      <>
        {/* <button
          onClick={() => {
            this.openChannel({
              type: "general",
              referenceId: 1,
              channelId: "test2-2",
              name: "Willkommen Test 2",
              userIds: ["kt_it_at_dpl_ltms_com", "test2"],
              open: true,
            });
          }}
        >
          Open Test
        </button> */}
        <ChannelChat
          ref={(channelChat) => {
            if (channelChat) {
              this.channelChat = channelChat;
            }
          }}
        />
      </>
    );
  }

  async openChannel(info: {
    type: channelType;
    referenceId: number | string;
    channelId: string;
    name: string;
    image?: string;
    userIds: string[];
    open: boolean;
  }) {
    const { referenceId, channelId, userIds, name, image, type, open } = info;

    if (!this.channelChat) return;
    if (!this.channelChat.chat) return;

    const channel = this.channelChat.chat.props.client.channel(
      "messaging",
      channelId,
      {
        members: userIds,
        image,
        name,
        extraType: type,
        referenceId,
        assignFlag: false,
        languages: ["de", "en"],
        responsiveMember: "",
      }
    );
    (this.channelChat.chat as any).getContext().setActiveChannel(channel);

    if (open && !this.channelChat.state.open) {
      this.channelChat.setState({ open: true });
    }
  }

  hide() {
    if (!this.channelChat || !this.channelChat.state.open) {
      return;
    }
    this.channelChat.setState({ open: false });
  }
}
export default App;
