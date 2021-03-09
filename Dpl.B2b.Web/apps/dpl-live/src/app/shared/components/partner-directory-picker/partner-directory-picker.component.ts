import { Component, Inject, OnInit, Optional } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { Partner } from '@app/api/dpl';
import {
  hostFlexColumn,
  LoadingService,
  PaginationResult,
  TypedFormControl,
} from '@dpl/dpl-lib';
import { combineLatest, Observable, ReplaySubject } from 'rxjs';
import { CustomerPartner } from '@app/api/dpl';
import {
  first,
  map,
  publishReplay,
  refCount,
  startWith,
  switchMap,
  tap,
} from 'rxjs/operators';

import { PartnerService } from '../../../partners/services/partner.service';
import { PartnerPickerContext } from '../../../partners/services/partner.service.types';
import { IPartnerDirectory } from '../../../partners/state/partner-directory.model';
import { IPartner } from '../../../partners/state/partner.model';

type ViewData = {
  partnerDirectories: IPartnerDirectory[];
  selectedPartnerDirectoryId?: number;
  partners: PaginationResult<IPartner[]>;
  selectedPartnerId?: number;
};

@Component({
  selector: 'app-partner-picker',
  templateUrl: './partner-directory-picker.component.html',
  styles: [
    hostFlexColumn,
    `
      .selected {
        background-color: #3f51b5;
        color: white;
      }
      .mat-card .mat-divider-horizontal {
        position: inherit;
      }
      .mat-form-field-appearance-legacy .mat-form-field-wrapper {
        margin-top: -10px;
      }
      .mat-card-header .mat-card-title {
        margin-top: 22px;
      }
      .verzeichnis {
        margin-bottom: 7px;
      }
      .scrollable {
        overflow-y: auto;
      }
    `,
  ],
})
export class PartnerDirectoryPickerComponent implements OnInit {
  directoryNameFormControl: TypedFormControl<string>;
  companyNameFormControl: TypedFormControl<string>;

  pageIndex$ = new ReplaySubject<number>();

  viewData$: Observable<ViewData>;
  constructor(
    @Optional()
    public dialogRef: MatDialogRef<PartnerDirectoryPickerComponent, CustomerPartner>,
    @Optional()
    @Inject(MAT_DIALOG_DATA)
    public context: PartnerPickerContext = 'prefill',
    private partnerService: PartnerService,
    private loadingService: LoadingService
  ) {}

  ngOnInit() {
    this.loadingService.setLoading(true);
    this.directoryNameFormControl = new FormControl();
    this.companyNameFormControl = new FormControl();

    const directoryName$ = this.directoryNameFormControl.valueChanges.pipe(
      //debounceTime(200),
      startWith('')
    );

    const partnerDirectories$ = directoryName$.pipe(
      map((directoryName) => directoryName.toLocaleLowerCase()),
      switchMap((directoryName) => {
        return this.partnerService.getPartnerDirectories().pipe(
          map((directories) => {
            const alleI18n = $localize`:All|Label Alle@@All:Alle`;

            const virtualAllDirectory: IPartnerDirectory = {
              id: null, // to be null to match value from aktika when no partner diretory selected
              name: alleI18n,
            };

            const filteredDirectories = directories.filter(
              (i) =>
                i.name &&
                i.name.length > 0 &&
                i.name.toLocaleLowerCase().includes(directoryName)
            );
            return [virtualAllDirectory, ...filteredDirectories];
          })
        );
      })
    );

    const selectedPartnerDirectory$ = this.partnerService
      .getActivePartnerDirectory(this.context)
      .pipe(
        publishReplay(1),
        refCount()
      );

    const companyName$ = this.companyNameFormControl.valueChanges.pipe(
      //debounceTime(200),
      startWith('')
    );

    const filter$ = companyName$.pipe(
      tap(() => this.pageIndex$.next(0)),
      switchMap((companyName) => {
        return this.pageIndex$.pipe(
          map((pageIndex) => {
            return {
              companyName,
              pageIndex,
            };
          })
        );
      }),
    );

    const partners$ = combineLatest([selectedPartnerDirectory$, filter$]).pipe(
      switchMap(([directory, { companyName, pageIndex }]) => {
        const directoryId = directory ? directory.id : null;
        return this.partnerService.searchPartners({
          customerPartnerDirectoryId: directoryId,
          companyName,
          limit: 10,
          page: pageIndex + 1,
        });
      }),
      publishReplay(1),
      refCount()
    );

    const selectedPartner$ = this.partnerService
      .getActivePartner(this.context)
      .pipe(
        publishReplay(1),
        refCount()
      );

    this.viewData$ = combineLatest([
      partnerDirectories$,
      selectedPartnerDirectory$,
      partners$,
      selectedPartner$,
    ]).pipe(
      map(
        ([
          partnerDirectories,
          selectedPartnerDirectory,
          partners,
          selectedPartner,
        ]) => {
          return <ViewData>{
            partnerDirectories,
            selectedPartnerDirectoryId: selectedPartnerDirectory
              ? selectedPartnerDirectory.id
              : null,
            partners,
            selectedPartnerId: selectedPartner ? selectedPartner.id : null,
          };
        }
      ),
      tap(() => this.loadingService.setLoading(false))
    );
  }

  onPartnerDirectorySelected(directoryId: number) {
    this.partnerService.setActivePartnerDirectory(this.context, directoryId);
  }

  onPartnerSelected(partner: CustomerPartner) {
    this.partnerService.setActivePartner(this.context, partner.id);
  }

  onClose() {
    this.partnerService
      .getActivePartner(this.context)
      .pipe(first())
      .subscribe({
        next: (partner) => this.dialogRef.close(partner),
      });
  }

  onCancel() {
    this.dialogRef.close();
  }

  trackById(index, item) {
    return item.id;
  }

  onPaginate(event: PageEvent) {
    this.pageIndex$.next(event.pageIndex);
  }
}
