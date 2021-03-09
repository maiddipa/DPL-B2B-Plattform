import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserSettingsRoutingModule } from './user-settings-routing.module';
import { UserSettingsComponent } from './user-settings.component';
import { SharedModule } from '@app/shared';

@NgModule({
  declarations: [UserSettingsComponent],
  imports: [CommonModule, SharedModule, UserSettingsRoutingModule],
})
export class UserSettingsModule {}
