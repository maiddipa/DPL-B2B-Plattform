import App from "./App";

export type SelectOption = {
  label: string;
  labelShort: string;
  value: string;
};

const windowCustom: Window &
  typeof globalThis & {
    dplChat: App;
    dplChatConfig: {
      streamKey: string;
      userId: string;
      channelId: string;
      channelName: string;
      userName: string;
      userImage: string;
      userToken: string | null;
      userLanguages: string[];
      userLanguage: string;
      userEmail: string;
      language: string;
      channelTypes?: SelectOption[];
      languageTypes?: SelectOption[];
      goToDetails?: (type: string, id: string) => void;
    };
  } = window as any;

export { windowCustom as window };
