import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SortingRoutingModule } from './sorting-routing.module';
import { SortingComponent } from './sorting.component';
import { SortingFormComponent } from './components/sorting-form/sorting-form.component';
import { SharedModule } from '../shared';
import { SortingHeaderComponent } from './components/sorting-header/sorting-header.component';
import { SortingDialogComponent } from './components/sorting-dialog/sorting-dialog.component';


@NgModule({
  declarations: [SortingComponent, SortingFormComponent, SortingHeaderComponent, SortingDialogComponent],
  imports: [
    CommonModule,
    SortingRoutingModule,
    SharedModule
  ]
})
export class SortingModule { }
