import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { of } from 'rxjs';
import { DynamicConfirmationDialogComponent, DynamicConfirmationDialogData, DynamicConfirmationDialogResult } from '../../shared';
import { dummyReceiptInput } from './sorting.service.types';

@Injectable({
  providedIn: 'root',
})
export class SortingService {
  constructor(private dialog:MatDialog) {
    
    
  }
  sortCompleteDialog(){
    return this.dialog
    .open<
      DynamicConfirmationDialogComponent,
      DynamicConfirmationDialogData,
      DynamicConfirmationDialogResult
    >(DynamicConfirmationDialogComponent, {
      data: {
        labels: {
          title: 'Sortierung abschließen und speichern',
          description: 'Der Vorgang wird abgeschlossen. Bitte bestätigen!',
          confirm: 'Ja',
          reject: 'Abbrechen',
          hideCancel: true     
        },          
      },
      hasBackdrop: false,
      autoFocus: false
    })
    .afterClosed()
  }

  sortCancelDialog(){
    return this.dialog
      .open<
        DynamicConfirmationDialogComponent,
        DynamicConfirmationDialogData,
        DynamicConfirmationDialogResult
      >(DynamicConfirmationDialogComponent, {
        data: {
          labels: {
            title: 'Sortierung abbrechen',
            description: 'Wenn Sie den Vorgang jetzt abbrechen, müssen Sie die Erfassung umgehend nachholen. Bitte bestätigen!',
            confirm: 'Ja, ich hole es umgehend nach.',
            reject: 'Abbrechen',
            hideCancel: true     
          },          
        },
        hasBackdrop: false,
        autoFocus: false
      })
      .afterClosed()
  }

  sortDisplayConfirmationDialog(){
    return this.dialog
      .open<
        DynamicConfirmationDialogComponent,
        DynamicConfirmationDialogData,
        DynamicConfirmationDialogResult
      >(DynamicConfirmationDialogComponent, {
        data: {
          labels: {
            title: 'Sortierung durchführen',
            description: 'Für diesen Vorgang wurde „Sortierung durchführen" festgelegt. Die Erfassungsmaske wird im nächsten Schritt geöffnet',
            confirm: 'OK',
            reject: 'Abbrechen, ich hole es umgehend nach.',
            hideCancel: true     
          },          
        },
        hasBackdrop: false,
        autoFocus: false
      })
      .afterClosed()
  }
}
