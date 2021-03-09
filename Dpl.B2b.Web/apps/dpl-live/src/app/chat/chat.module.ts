import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { ChatService } from './services/chat.service';
import { ChatLinkDirective } from './components/chat-link.directive';

@NgModule({
  declarations: [ChatLinkDirective],
  imports: [CommonModule],
  providers: [ChatService],
  exports: [ChatLinkDirective],
})
export class ChatModule {}
