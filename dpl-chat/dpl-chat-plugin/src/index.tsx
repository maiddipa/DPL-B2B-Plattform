import React from "react";
import ReactDOM from "react-dom";
import "./index.css";
import "../node_modules/stream-chat-react/dist/css/index.css";
import App from "./App";
import * as serviceWorker from "./serviceWorker";
import { window } from "./types";

window.addEventListener(
  "dpl-chat-plugin-start",
  () => {
    ReactDOM.render(
      <App
        ref={dplChat => {
          if (dplChat) {
            window.dplChat = dplChat;
          }
        }}
      />,
      document.getElementById("dpl-chat-plugin")
    );

    // setTimeout(() => {
    //     const select = document;
    //     if (!select) {
    //         return;
    //     }
    //     select.addEventListener("change", () => {

    //         window.dplChat.forceUpdate();
    //     });
    // }, 5000);
  },
  false
);

window.addEventListener(
  "dpl-chat-plugin-remove",
  () => {
    const chat = document.getElementById("dpl-chat-plugin");

    if (!chat || !chat.parentNode) {
      return;
    }

    chat.childNodes.forEach(node => chat.removeChild(node));
  },
  false
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();

// Available built in themes:
// 'messaging light'
// | 'messaging dark'
// | 'team light'
// | 'team dark'
// | 'commerce light'
// | 'commerce dark'
// | 'gaming light'
// | 'gaming dark'
// | 'livestream light'
// | 'livestream dark'
