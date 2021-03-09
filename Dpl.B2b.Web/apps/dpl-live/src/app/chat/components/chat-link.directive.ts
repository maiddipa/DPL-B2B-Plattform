import {
  ChangeDetectorRef,
  Directive,
  HostBinding,
  HostListener,
  Input,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { untilDestroyed } from 'ngx-take-until-destroy';
import { tap } from 'rxjs/operators';

import { ChatService } from '../services/chat.service';
import {
  ChannelType,
  ChatConversationInfo,
  ChannelInfoData,
  ChannelInfoDataType,
  ChatConversationCreateInfo,
} from '../services/chat.service.types';

export type ChatLinkData = [ChannelInfoDataType, ChannelInfoData];

@Directive({
  selector: '[chatLink]',
})
export class ChatLinkDirective implements OnInit, OnDestroy {
  @Input() chatLink: ChatLinkData;

  @HostBinding('class.has-message') private hasMessage = false;

  private info: ChatConversationCreateInfo;
  constructor(private chat: ChatService, private cd: ChangeDetectorRef) {}

  ngOnInit(): void {
    const [type, data] = this.chatLink;
    this.info = this.chat.getChatConversationCreateInfo(type, data);
    this.chat
      .conversationHasMessage(this.info.channelId)
      .pipe(
        untilDestroyed(this),
        tap((hasMessage) => {
          this.hasMessage = hasMessage;
          this.cd.markForCheck();
        })
      )
      .subscribe();
  }

  ngOnDestroy() {}

  @HostListener('click', ['$event']) onClick($event) {
    console.log($event);
    // make sure no other action is triggered by click
    $event.stopPropagation();

    this.chat.openConversation(this.info);
  }
}
