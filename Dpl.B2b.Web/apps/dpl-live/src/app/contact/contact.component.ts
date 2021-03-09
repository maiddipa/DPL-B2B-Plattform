import { Component, OnInit } from '@angular/core';
import { ChatService } from '../chat/services/chat.service';
import { ChatLinkData } from '../chat/components/chat-link.directive';

@Component({
  selector: 'dpl-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss'],
})
export class ContactComponent implements OnInit {
  constructor(private chat: ChatService) {}

  infos = {
    name: 'Deutsche Paletten Logistik GmbH',
    postalCode: '59494',
    city: 'Soest',
    street: 'Overweg 12',
    state: 10,
    country: 1,
    email: 'info@dpl-pooling.com',
    phone: '+49 (0)2921 7899-178',
  };

  get locationText(): string {
    return `D-${this.infos.postalCode} ${this.infos.city}, ${this.infos.street}`;
  }

  ngOnInit() {}
}
