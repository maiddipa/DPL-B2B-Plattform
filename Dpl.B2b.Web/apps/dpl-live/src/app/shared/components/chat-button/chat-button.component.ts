import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import {
  ChannelInfoData,
  ChannelInfoDataType,
} from '../../../chat/services/chat.service.types';

@Component({
  selector: 'dpl-chat-button',
  templateUrl: './chat-button.component.html',
  styleUrls: ['./chat-button.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChatButtonComponent {
  @Input() type: ChannelInfoDataType;
  @Input() data: ChannelInfoData;
  constructor() {}
}
