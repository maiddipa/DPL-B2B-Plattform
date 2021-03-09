import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import {
  AccountingRecord,
  Order,
  OrderLoad,
  OrderType,
  TransportOffering,
  Voucher,
  LoadCarrierReceipt,
} from '@app/api/dpl';
import { LanguagePipe } from '@app/shared/pipes';
import { APP_CONFIG, DplLiveConfiguration } from 'apps/dpl-live/src/config';
import * as _ from 'lodash';
import { BehaviorSubject, EMPTY, Observable, Subject } from 'rxjs';
import {
  buffer,
  catchError,
  debounceTime,
  distinctUntilChanged,
  distinctUntilKeyChanged,
  filter,
  map,
  switchMap,
  tap,
} from 'rxjs/operators';
import { StreamChat } from 'stream-chat';

import { SessionState } from '../../core/session/state/session.store';
import { UserService } from '../../user/services/user.service';
import {
  ChannelInfoData,
  ChannelInfoDataType,
  ChannelType,
  ChannelTypeOption,
  ChatCache,
  ChatConversationCreateInfo,
  LanguageTypeOption,
} from './chat.service.types';

// copied from index.tsx (dpl-chat-plugin project)
declare const window: Window &
  typeof globalThis & {
    dplChat: {
      openChannel: (info: {
        type: ChannelType;
        referenceId: number | string;
        channelId: string;
        name: string;
        image?: string;
        userIds: string[];
        open: boolean;
      }) => Promise<void>;
      hide: () => void;
      channelChat: {
        client: StreamChat;
      };
    };
    dplChatConfig: {
      streamKey: string;
      userId: string;
      userEmail: string;
      userName: string;
      userImage?: string;
      userToken?: string | null;
      languages: LanguageTypeOption[];
      userLanguages: string[];
      userLanguage: string;
      language: string;
      channelId?: string;
      channelName?: string;
      channelTypes: ChannelTypeOption[];
      goToDetails: (type: ChannelType, id: number) => void;
    };
  };

const dplChatEvents = {
  init: new Event('dpl-chat-plugin-start'),
  remove: new Event('dpl-chat-plugin-remove'),
};

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  channelToQuery$ = new Subject<string>();
  hasMessageCache$ = new BehaviorSubject<ChatCache>({});

  userIds: string[];

  constructor(
    private user: UserService,
    private languagePipe: LanguagePipe,
    private router: Router,
    private http: HttpClient,
    @Inject(APP_CONFIG) private config: DplLiveConfiguration
  ) {
    this.initCache();
  }

  private navigateToDetail(type: ChannelType, referenceId: number | string) {
    const orderProcessUrl = '/order-process';
    switch (type) {
      case 'supply':
      case 'demand':
        this.router.navigate([orderProcessUrl], {
          queryParams: { orderId: referenceId },
        });
        break;
      case 'order-load':
        this.router.navigate([orderProcessUrl], {
          queryParams: { orderLoadId: referenceId },
        });
        break;
      default:
        break;
    }

    this.hide();
  }

  private convertEmailToUserId(email: string) {
    // stream user ids can only contain alphanumeric + _
    return (
      email
        .replace(/[^\w\@_]/g, '_')
        // we are replacing the at seperately to make user ids more readable
        .replace('@', '_at_')
    );
  }

  private initCache() {
    this.hasMessageCache$.next({});
    const flush$ = this.channelToQuery$.asObservable().pipe(debounceTime(500));
    this.channelToQuery$
      .asObservable()
      .pipe(
        buffer(flush$),
        filter((ids) => ids.length > 0),
        map((ids) => _(ids).uniq().value()),
        switchMap((ids) => {
          return this.http.post<ChatCache>(
            this.config.chat.functions.getChannelsWithMessages,
            ids
          );
        }),
        tap(
          (cache) => {
            // update cache
            this.hasMessageCache$.next({
              ...this.hasMessageCache$.value,
              ...cache,
            });
          },
          (e) => {
            console.log(e);
          }
        )
      )
      .subscribe();
  }

  getCurrentUserId() {
    return window.dplChatConfig.userId;
  }

  getDplUserIdForConversation(type: string, referenceId: number | string) {
    return this.convertEmailToUserId('dpl.kundenbetreuung@dpl-ltms.com');
  }

  getUserIdsForConversation(type: string, referenceId: number | string) {
    // TODO: remove this block when serverSide is implementet
    // Hardcoded User Emails
    const contact = this.getDplUserIdForConversation(type, referenceId);
    const userIds = [contact];

    const currentUserId = this.getCurrentUserId();

    if (userIds.indexOf(currentUserId) === -1) {
      //push the activ User to the userIds
      userIds.push(currentUserId);
    }
    return userIds;
  }

  conversationHasMessage(channelId: string) {
    return this.hasMessageCache$.asObservable().pipe(
      map((cache) => cache[channelId]),
      distinctUntilChanged(),
      tap(
        (hasMessage) => {
          if (!hasMessage) {
            this.channelToQuery$.next(channelId);
          } else {
            console.log(hasMessage);
          }
        },
        (e) => {
          console.log(e);
        }
      )
    );
  }

  load() {
    return this.user.getCurrentUser().pipe(
      map((user) => (!!user ? user : ({} as SessionState))),
      distinctUntilKeyChanged('email'),
      switchMap((user) =>
        this.loadChatInternal(<SessionState>user, this.languagePipe)
      )
    );
  }

  private loadChatInternal(
    user: SessionState,
    languagePipe: LanguagePipe
  ) {
    const loadChat = (user: SessionState) => {
      const streamKey = this.config.stream.key;
      const userLanguages = ['de', 'en', 'es', 'it', 'pl'];
      const language = localStorage.getItem('language') || 'de';

      const channelTypes: ChannelTypeOption[] = [
        {
          label: $localize`:ChatNoFilter|@@ChatNoFilter:Nachrichten`,
          labelShort: 'O',
          value: '',
        },
        {
          label: $localize`:ChatBooking|@@ChatBooking:Buchung`,
          labelShort: 'BUCH',
          value: 'accounting-record',
        },
        {
          label: $localize`:ChatDPG|@@ChatDPG:Gutschrift`,
          labelShort: 'DPG',
          value: 'voucher',
        },
        {
          label: $localize`:ChatBedarf|@@ChatBedarf:Bedarf`,
          labelShort: 'BED',
          value: 'demand',
        },
        {
          label: $localize`:ChatVerfügbarkeit|@@ChatVerfügbarkeit:Verfügbarkeit`,
          labelShort: 'VER',
          value: 'supply',
        },
        {
          label: $localize`:Label für Channel Type Dropdown im Chat@@ChatTransport:Transport`,
          labelShort: 'TRA',
          value: 'transport',
        },
        {
          label: $localize`:Label für Channel Type Dropdown im Chat@@ChatChannelTypeLoadCarrierReceipt:Quittung`,
          labelShort: 'LCR',
          value: 'load-carrier-receipt',
        },
      ];

      const languages = [
        {
          label: $localize`:chatOrginal|@@ChatOrginal:Orginal`,
          labelShort: 'O',
          value: '',
        },
        { label: languagePipe.transform(1), labelShort: 'de', value: 'de' },
        { label: languagePipe.transform(2), labelShort: 'en', value: 'en' },
        { label: languagePipe.transform(3), labelShort: 'fr', value: 'fr' },
        { label: languagePipe.transform(4), labelShort: 'pl', value: 'pl' },
      ];

      if (!user.email) {
        return window.dispatchEvent(dplChatEvents.remove);
      }
      const userLanguage = 'de';

      const userId = this.convertEmailToUserId(user.email);

      window.dplChatConfig = {
        streamKey,
        userId,
        userEmail: user.email,
        userName: user.name,
        languages,
        userLanguages,
        userLanguage: userLanguage,
        language,
        channelTypes,
        goToDetails: this.navigateToDetail.bind(this),
      };

      window.dispatchEvent(dplChatEvents.init);
    };

    return Observable.create((obs) => {
      loadChat(user);
      obs.next(true);
      obs.complete();
      return () => {};
    });
  }

  hide() {
    window.dplChat.hide();
  }

  getChatConversationCreateInfo(
    type: ChannelInfoDataType,
    data: ChannelInfoData
  ): ChatConversationCreateInfo {
    if (data) {
      switch (type) {
        case 'accounting-record': {
          return this.getAccountingRecordChatInfo(data as AccountingRecord);
        }
        case 'order': {
          return this.getOrderChatInfo(data as Order);
        }
        case 'orderLoad': {
          return this.getOrderLoadChatInfo(data as OrderLoad);
        }
        case 'loadCarrierReceipt': {
          return this.getOrderLoadCarrierReceiptChatInfo(
            data as LoadCarrierReceipt
          );
        }
        case 'transport': {
          return this.getTransportChatInfo(data as TransportOffering);
        }
        case 'voucher': {
          return this.getVoucherChatInfo(data as Voucher);
        }
        case 'general': {
          return this.getWelcomeInfo(data as undefined);
        }
        default: {
          throw new Error(`Unkown chat info data: ${type}`);
        }
      }
    } else {
      if (type === 'general') {
        return this.getWelcomeInfo(data as undefined);
      } else {
        throw new Error(`Unkown chat info type: ${type}`);
      }
    }
  }

  private getAccountingRecordChatInfo(data: AccountingRecord) {
    const type: ChannelType = 'accounting-record';
    //TODO: REMEMBER TO CHANGE NAME
    // What name would we need here? processId referenceNumber transactionId
    const name = `Buchung ${data.referenceNumber}`;
    const info: ChatConversationCreateInfo = {
      channelId: this.getChannelId(type, data.id),
      referenceId: data.id,
      type,
      data,
      name,
      userIds: [],
    };
    return info;
  }

  private getOrderChatInfo(data: Order) {
    const type: ChannelType =
      data.type === OrderType.Supply ? 'supply' : 'demand';

    const name =
      data.type === OrderType.Supply
        ? `Verfügbarkeit ${data.orderNumber}`
        : `Bedarf ${data.orderNumber}`;

    const info: ChatConversationCreateInfo = {
      channelId: this.getChannelId(type, data.id),
      referenceId: data.id,
      data,
      type,
      name,
      userIds: [],
    };

    return info;
  }

  private getOrderLoadChatInfo(data: OrderLoad) {
    const type: ChannelType = 'order-load';

    const info: ChatConversationCreateInfo = {
      channelId: this.getChannelId(type, data.id),
      referenceId: data.id,
      type,
      data,
      name: `Ladung ${data.digitalCode || data.id}`,
      userIds: [],
    };
    return info;
  }

  private getOrderLoadCarrierReceiptChatInfo(data: LoadCarrierReceipt) {
    const type: ChannelType = 'load-carrier-receipt';

    const info: ChatConversationCreateInfo = {
      channelId: this.getChannelId(type, data.id),
      referenceId: data.id,
      type,
      data,
      name: `Quittung ${data.documentNumber}`,
      userIds: [],
    };
    return info;
  }

  private getTransportChatInfo(data: TransportOffering) {
    const type: ChannelType = 'transport';

    const info: ChatConversationCreateInfo = {
      channelId: this.getChannelId(type, data.id),
      referenceId: data.id,
      type,
      data,
      // TODO update with transport number when its implemented in backend
      name: `Transport TR-${data.id}`,
      userIds: [],
    };
    return info;
  }

  private getVoucherChatInfo(data: Voucher) {
    const type: ChannelType = 'voucher';

    const info: ChatConversationCreateInfo = {
      channelId: this.getChannelId(type, data.id),
      referenceId: data.id,
      type,
      data,
      name: `Gutschrift ${data.documentNumber}`,
      userIds: [],
    };
    return info;
  }

  private getWelcomeInfo(data: any) {
    const type: ChannelType = 'general';

    const info: ChatConversationCreateInfo = {
      channelId: window['dplChatConfig'].userId,
      referenceId: undefined,
      type,
      data,
      name: 'Willkommen ' + window['dplChatConfig'].userName,
      userIds: [],
    };
    return info;
  }

  openConversation(info: ChatConversationCreateInfo) {
    const { channelId, type, referenceId, name } = info;

    const currentUserId = this.getCurrentUserId();
    const contactUserId = this.getDplUserIdForConversation(type, referenceId);

    const userIds = [currentUserId]; //this.getUserIdsForConversation(type, referenceId);

    const createChannelOrAddMeAsMemberPayLoad = {
      internalUserId: contactUserId,
      externalUserId: currentUserId,
      name: name,
      type: type,
      referenceId: referenceId.toString(),
      language: 'de',
    };

    this.http
      .post<void>(
        this.config.chat.functions.createChannelOrAddMeAsMember,
        createChannelOrAddMeAsMemberPayLoad,
        { observe: 'response' }
      )
      .pipe(
        filter((response) => response.ok),
        tap((response) => {
          window.dplChat.openChannel({
            type,
            referenceId,
            channelId,
            name,
            userIds,
            open: true,
          });
        }),
        catchError((err, obs) => {
          // TODO check what kind of errors could appear here
          console.log(err);
          return EMPTY;
        })
      )
      .subscribe();
  }

  getChannelId(type: ChannelType, id: number | string) {
    return `${type}-${id.toString()}`;
  }
}
