//disable for type: 'messaging' in Dashboard Reactions/Replies
//Commands because they doesn't work with translations
//and enable Automod version Simple
//enable webhook
// const uniqueLanguages = [];
const express = require("express");
const StreamChat = require("stream-chat").StreamChat;
const _ = require("lodash");
var TurndownService = require("turndown");
var turndownService = new TurndownService().remove("style");

//change key and secret
const dvitServer = new StreamChat(
  //Dvit
  //   "jdhkvzjuphpn",
  //   "7gufcayevwtwekf7e73mkmcgj5kq33v3eawxr4n7jw2byqyk35yjrxbjqmh54ppt"
  //Fait
  // "4jhp4s849ca5",
  // "x88mvquc76eys7826anvc9y6hb4rm2hshnfwsaw72c66ndjxnq5j8fkujppqdnev"
  //Demo
  // "6hgttn9mq2v7",
  // "rx59bzers8vbnf2pxz8vfhpc4f7aegewngfyydaktwj3c7kp9nawhjj6uj3z6c3t"
  //Prod
  "dxgaf3ukq69m",
  "8z2b598af2b5wxcapb2ajw2ecjjc2a3wkgyyx6rptwh6xy599hthxcagm78ye6ky"
);

const app = express();
const port = 3000;
app.use(express.json());
app.use(express.urlencoded());

console.log("Start Script", dvitServer);

logChannels();



async function logChannels() {
  const serverChannels = await dvitServer.queryChannels();
  console.log(serverChannels);
}

app.listen(port, () => {
  console.log(`Example app listening on port ${port}!`);
});
