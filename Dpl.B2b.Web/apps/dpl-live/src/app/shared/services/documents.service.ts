import { Injectable } from '@angular/core';
import { PrintService } from './print.service';
import { DplApiService } from '@app/core';
import { switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class DocumentsService {
  constructor(private dpl: DplApiService, private printService: PrintService) {}

  print(documentId: number, showConfirmation: boolean) {
    return this.dpl.documents
      .getDocumentDownload(documentId)
      .pipe(
        switchMap((url) => this.printService.printUrl(url, showConfirmation))
      );
  }
}
