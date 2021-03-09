import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FilterTemplateDialogData } from '../../services/filter.service.types';
import { FilterTemplateService } from '../../services/filter-template.service';

@Component({
  selector: 'app-voucher-filter-template-remove-dialog',
  templateUrl: './voucher-filter-template-remove-dialog.component.html',
  styleUrls: ['./voucher-filter-template-remove-dialog.component.scss'],
})
export class VoucherFilterTemplateRemoveDialogComponent implements OnInit {
  dialogData: FilterTemplateDialogData;

  constructor(
    private readonly dialogRef: MatDialogRef<
      VoucherFilterTemplateRemoveDialogComponent
    >,
    @Inject(MAT_DIALOG_DATA) data: FilterTemplateDialogData,
    private readonly voucherFilterTemplateService: FilterTemplateService
  ) {
    this.dialogData = data;
  }

  ngOnInit(): void {}

  close(): void {
    console.log('close ' + this.dialogData.templateTitle);
    this.dialogRef.close();
  }

  delete(): void {
    this.voucherFilterTemplateService.removeTemplateFilter(
      this.dialogData.templateId,
      this.dialogData.context
    );
    this.dialogRef.close();
  }
}
