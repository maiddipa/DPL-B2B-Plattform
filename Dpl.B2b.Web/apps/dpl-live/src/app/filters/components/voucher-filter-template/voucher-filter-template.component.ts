import {
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSelectChange } from '@angular/material/select';
import { combineLatest, Observable, ReplaySubject } from 'rxjs';
import { distinctUntilChanged, first, map, switchMap } from 'rxjs/operators';

import { FilterTemplateService } from '../../services/filter-template.service';
import { FilterService } from '../../services/filter.service';
import {
  FilterContext,
  FilterTemplateDialogData,
  FilterTemplate,
} from '../../services/filter.service.types';
import { VoucherFilterTemplateRemoveDialogComponent } from '../voucher-filter-template-remove-dialog/voucher-filter-template-remove-dialog.component';
import { VoucherFilterTemplateSaveDialogComponent } from '../voucher-filter-template-save-dialog/voucher-filter-template-save-dialog.component';

type ViewData = {
  selectedTemplateId: string;
};

@Component({
  selector: 'voucher-filter-template',
  templateUrl: './voucher-filter-template.component.html',
  styleUrls: ['./voucher-filter-template.component.css'],
})
export class VoucherFilterTemplateComponent implements OnInit, OnChanges {
  @Input() context: FilterContext;

  context$ = new ReplaySubject<FilterContext>();

  viewData$: Observable<ViewData>;
  allTemplateFilters$: Observable<FilterTemplate[]>;
  hasTemplateChanges$: Observable<boolean>;
  activeTemplate$: Observable<FilterTemplate>;

  constructor(
    private readonly voucherFilterTemplateService: FilterTemplateService,
    private readonly voucherFilterService: FilterService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    const context$ = this.context$.pipe(distinctUntilChanged());

    this.allTemplateFilters$ = context$.pipe(
      switchMap((context) =>
        this.voucherFilterTemplateService.getTemplateFilters(context)
      )
    );
    this.hasTemplateChanges$ = this.voucherFilterService.hasTemplateChanges();
    this.activeTemplate$ = this.voucherFilterTemplateService.getActiveTemplateFilter();

    this.viewData$ = combineLatest(this.activeTemplate$).pipe(
      map(([activeTemplate]) => {
        return {
          selectedTemplateId: activeTemplate ? activeTemplate.id : null,
        };
      })
    );
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.context$.next(this.context);
  }

  selectFilterTemplate(change: MatSelectChange): void {
    this.voucherFilterTemplateService.setActiveTemplateFilter(
      change.value,
      this.context
    );
  }

  resetFilterChanges(): void {
    this.activeTemplate$.pipe(first()).subscribe((template) => {
      this.voucherFilterService.resetFilter(template, this.context);
    });
  }

  saveTemplate(): void {
    this.activeTemplate$.pipe(first()).subscribe((template) => {
      this.dialog.open<
        VoucherFilterTemplateSaveDialogComponent,
        FilterTemplateDialogData
      >(VoucherFilterTemplateSaveDialogComponent, {
        width: '300px',
        data: {
          context: this.context,
          templateId: template ? template.id : undefined,
          templateTitle: template ? template.title : undefined,
        },
      });
    });
  }

  removeTemplate(event: Event): void {
    this.activeTemplate$.pipe(first()).subscribe((template) => {
      this.dialog.open<
        VoucherFilterTemplateRemoveDialogComponent,
        FilterTemplateDialogData
      >(VoucherFilterTemplateRemoveDialogComponent, {
        width: '300px',
        data: {
          context: this.context,
          templateId: template ? template.id : undefined,
          templateTitle: template ? template.title : undefined,
        },
      });
    });
  }
}
