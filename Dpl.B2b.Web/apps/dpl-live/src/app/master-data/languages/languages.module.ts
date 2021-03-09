import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LanguagesQuery } from './state/languages.query';
import { LanguagesStore } from './state/languages.store';
import { LanguagesService } from './services/languages.service';

@NgModule({
  declarations: [],
  imports: [CommonModule],
  providers: [LanguagesQuery, LanguagesStore, LanguagesService],
})
export class LanguagesModule {}
