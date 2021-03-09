import { NgModule } from '@angular/core';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ScrollingModule } from '@angular/cdk/scrolling';

@NgModule({
  exports: [DragDropModule, ScrollingModule],
})
export class AngularCdkModule {}
