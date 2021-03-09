import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  FilterTemplate,
  FilterTemplateDialogData,
  FilterContext,
} from '../../services/filter.service.types';
import { FilterTemplateService } from '../../services/filter-template.service';
import { combineLatest, Observable, of } from 'rxjs';
import {
  map,
  tap,
  first,
  switchMap,
  publishReplay,
  refCount,
} from 'rxjs/operators';
import { FormBuilder, FormGroup } from '@angular/forms';

interface ViewData {
  activeTemplate: FilterTemplate;
  allTemplates: FilterTemplate[];
}

@Component({
  selector: 'app-voucher-filter-template-save-dialog',
  templateUrl: './voucher-filter-template-save-dialog.component.html',
  styleUrls: ['./voucher-filter-template-save-dialog.component.css'],
})
export class VoucherFilterTemplateSaveDialogComponent implements OnInit {
  viewData$: Observable<ViewData>;
  form: FormGroup;
  context: FilterContext;
  templateId: string;
  templateTitle: string;

  constructor(
    private readonly dialogRef: MatDialogRef<
      VoucherFilterTemplateSaveDialogComponent
    >,
    @Inject(MAT_DIALOG_DATA) data: FilterTemplateDialogData,
    private readonly voucherFilterTemplateService: FilterTemplateService,
    private readonly formBuilder: FormBuilder
  ) {
    if (data) {
      this.context = data.context;
      this.templateId = data.templateId;
      this.templateTitle = data.templateTitle;
    }
  }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      templateTitle: [this.templateTitle], // todo Validator: check existing name, override
    });
  }

  save(): void {
    // todo check name is used
    // todo check template is overrideable
    if (this.form.valid) {
      // todo check title is --> active template must be overrideable
      this.voucherFilterTemplateService
        .saveTemplateFilter(
          this.templateTitle === this.form.value.templateTitle
            ? this.templateId
            : undefined,
          this.form.value.templateTitle,
          this.context
        )
        .subscribe((result) => this.dialogRef.close());
    }
  }

  close(): void {
    this.dialogRef.close();
  }
}
