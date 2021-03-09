import {
  ChangeDetectorRef,
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { MatExpansionPanel } from '@angular/material/expansion';
import { combineQueries } from '@datorama/akita';
import { LoadingService } from '@dpl/dpl-lib';
import { Observable, ReplaySubject } from 'rxjs';
import {
  debounceTime,
  distinctUntilChanged,
  map,
  publishReplay,
  refCount,
  switchMap,
  tap,
} from 'rxjs/operators';

import { FilterTemplateService } from '../../services/filter-template.service';
import { FilterService } from '../../services/filter.service';
import {
  FilterContext,
  FilterPosition,
  Filter,
} from '../../services/filter.service.types';

type ViewData = {
  context: FilterContext;
  appliedFilters: Filter[];
  extendedFilters: Filter[];
  moreExpanded: boolean;
};

@Component({
  selector: 'voucher-filter',
  templateUrl: './voucher-filter.component.html',
  styleUrls: ['./voucher-filter.component.scss'],
})
export class VoucherFilterComponent implements OnInit, OnChanges {
  // properties to expand panel custom way, filter templates btn in header row
  myPanel: MatExpansionPanel;
  @ViewChild('myPanel') set content(content: MatExpansionPanel) {
    this.myPanel = content;
  }
  matIcon = 'keyboard_arrow_down' || 'keyboard_arrow_up';

  @Input() context: FilterContext;
  viewData$: Observable<ViewData>;
  position = FilterPosition;
  hasChangedFilters$: Observable<boolean>;
  hasAvailableExtendedFilters$: Observable<boolean>;
  hasTemplateChanges$: Observable<boolean>;
  moreExpanded = false;

  context$ = new ReplaySubject<FilterContext>();

  constructor(
    private voucherFilterService: FilterService,
    private voucherFilterTemplateService: FilterTemplateService,
    private cdref: ChangeDetectorRef,
    private loadingService: LoadingService
  ) {}

  ngOnInit() {
    // input context
    this.hasChangedFilters$ = this.voucherFilterService.hasChangedFilters();
    this.hasAvailableExtendedFilters$ = this.voucherFilterService.hasAvailableExtendedFilters();
    this.hasTemplateChanges$ = this.voucherFilterService.hasTemplateChanges();

    // todo change filter-templates by context
    const context$ = this.context$.pipe(distinctUntilChanged());

    this.viewData$ = context$.pipe(
      switchMap((context) =>
        combineQueries([
          this.voucherFilterService.getAppliedFilters(context),
          this.voucherFilterService.getExtendedFilters(context),
        ]).pipe(
          debounceTime(100),
          map((latest) => {
            const [appliedFilters, extendedFilters] = latest;
            const viewData: ViewData = {
              context,
              appliedFilters,
              extendedFilters,
              moreExpanded: false,
            };
            return viewData;
          })
        )
      ),
      this.loadingService.showLoadingWhile(),
      tap(() => {
        setTimeout(() => {
          // logic to expand panel custom way, filter templates btn in header row
          this.myPanel.expandedChange.subscribe((data) => {
            this.matIcon = data ? 'keyboard_arrow_up' : 'keyboard_arrow_down';
          });
        }, 200);
      }),
      publishReplay(1),
      refCount()
    );
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.context$.next(this.context);
  }

  applyExtended() {
    this.voucherFilterService.applyFilter(this.context);
  }

  reset() {
    this.voucherFilterTemplateService.clearFilterTemplate(this.context);
  }

  expandPanel() {
    this.myPanel.expanded = !this.myPanel.expanded;
  }

  // mapFilterFormToModelValue(type: FilterType, valueForm: AbstractControl) {
  //   let value = [];

  //   switch (type) {
  //     case FilterType.Text:
  //       if (valueForm && valueForm.value && valueForm.value.filterValue) {
  //         value = [valueForm.value.filterValue];
  //       } else {
  //         value = [];
  //       }
  //       break;
  //     case FilterType.Date:
  //       if (valueForm && valueForm.value && valueForm.value.filterValueFrom) {
  //         if (valueForm.value.filterValueTo) {
  //           value = [
  //             valueForm.value.filterValueFrom,
  //             valueForm.value.filterValueTo
  //           ];
  //         } else {
  //           value = [
  //             valueForm.value.filterValueFrom,
  //             valueForm.value.filterValueFrom
  //           ];
  //         }
  //       } else {
  //         value = [];
  //       }
  //       break;
  //     case FilterType.ChoiceSingle:
  //       if (valueForm && valueForm.value && valueForm.value.filterValue) {
  //         value = [valueForm.value.filterValue];
  //       } else {
  //         value = [];
  //       }
  //       break;
  //     default:
  //       // ChoiceMulti
  //       if (valueForm && valueForm.value && valueForm.value.filterValue) {
  //         value = valueForm.value.filterValue;
  //       } else {
  //         value = [];
  //       }
  //       break;
  //   }
  //   return value;
  // }
}
