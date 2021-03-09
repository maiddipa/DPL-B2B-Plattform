import {
  Component,
  ElementRef,
  EventEmitter,
  Inject,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatAutocompleteTrigger } from '@angular/material/autocomplete';
import { MatDialog } from '@angular/material/dialog';
import { CustomerPartner } from '@app/api/dpl';
import { PartnerTypePipe } from '@app/shared/pipes';
import { filterNil } from '@datorama/akita';
import * as _ from 'lodash';
import {
  Controls,
  NgxSubFormRemapComponent,
  subformComponentProviders,
  SubFormGroup,
} from 'ngx-sub-form';
import { type } from 'os';
import { EMPTY, Observable, of } from 'rxjs';
import {
  debounceTime,
  distinctUntilChanged,
  filter,
  map,
  switchMap,
  tap,
} from 'rxjs/operators';

import { APP_CONFIG, DplLiveConfiguration } from '../../../../config';
import { PartnerService } from '../../../partners/services/partner.service';
import { PartnerPickerContext } from '../../../partners/services/partner.service.types';
import { CustomValidators } from '../../../validators';
import { PartnerCreateDialogComponent } from '../partner-create-dialog/partner-create-dialog.component';
import { PartnerDirectoryPickerComponent } from '../partner-directory-picker/partner-directory-picker.component';

type PartnerLookupForm = {
  option: PartnerLookupOption | string;
  address: CustomerPartner['address'];
  type: CustomerPartner['type'];
};

type PartnerLookupOption = {
  partner: CustomerPartner;
  directoryId: number;
  directoryName: string;
};

@Component({
  selector: 'partner-lookup',
  templateUrl: './partner-lookup.component.html',
  styleUrls: ['./partner-lookup.component.scss'],
  providers: subformComponentProviders(PartnerLookupComponent),
})
export class PartnerLookupComponent
  extends NgxSubFormRemapComponent<CustomerPartner, PartnerLookupForm>
  implements OnChanges {
  @Input() context: PartnerPickerContext;
  @Input() title: string;
  @Input() placeholder: '';
  @Input() required = false;
  @Output() selectionChanged: EventEmitter<CustomerPartner> = new EventEmitter();

  @ViewChild('autocompleteInput')
  autocompleteInput: ElementRef;

  @ViewChild('autocompleteInput', {
    read: MatAutocompleteTrigger,
  })
  autocompleteTrigger: MatAutocompleteTrigger;

  protected emitInitialValueOnInit = false;
  options$: Observable<PartnerLookupOption[]>;

  constructor(
    @Inject(APP_CONFIG) private config: DplLiveConfiguration,
    private partnerService: PartnerService,
    private dialog: MatDialog,
    private partnerType: PartnerTypePipe
  ) {
    super();
  }

  ngOnChanges(changes: SimpleChanges) {
    super.ngOnChanges(changes);
    this.setRequiredState(this.required);
    // the reason we need to disable the controls manually is
    // because a form group cannot be created in disabled state
    // this is a reactive forms limitation and also applies to sub form groups
    // hack not sure why a timeout is needed
    setTimeout(() => {
      this.formGroup.controls.address.disable();
    }, 200);

    this.options$ = this.formGroup.controls.option.valueChanges.pipe(
      distinctUntilChanged(),
      switchMap((value: string | PartnerLookupOption) => {
        // if value is string is a search string, we need to return a list of Partners
        if (!value || typeof value === 'string') {
          return of(value).pipe(
            tap(() => {
              if (this.formGroup.controls.address.value) {
                this.formGroup.controls.address.reset();
              }

              if (this.formGroup.controls.type.value) {
                this.formGroup.controls.type.reset();
              }

              this.partnerService.setActivePartnerDirectory(this.context, null);
              this.partnerService.setActivePartner(this.context, null);
            }),
            filterNil,
            filter(
              (name: string) =>
                name.length >= this.config.app.ui.autoComplete.minLength
            ),
            debounceTime(this.config.app.ui.autoComplete.debounceTime),
            switchMap((companyName) =>
              this.partnerService.searchPartners({ companyName, limit: 50 })
            ),
            map((partners) => {
              // this block ensures that we get one row per partner/directory combination
              const optionsArray = partners.data?.map((partner) =>
                partner.directoryNames.map((directoryName, index) => {
                  return {
                    directoryName,
                    directoryId: partner.directoryIds[index],
                    partner,
                  } as PartnerLookupOption;
                })
              );

              return _(optionsArray).flatten().value();
            })
          );
        }

        // if value is a partner (=not a string), it means a selection was made
        // after emitting the partner, we do NOT want to return anything from the observable (=EMPTY)
        return of(value).pipe(
          tap((partnerLookupOption) => {
            console.log(partnerLookupOption);
            this.selectionChanged.emit(partnerLookupOption.partner);

            this.formGroup.controls.address.patchValue(value.partner.address);
            this.formGroup.controls.type.patchValue(
              this.partnerType.transform(value.partner.type)
            );

            // TODO chekck this line
            //if (this.partnerFormGroup) {
            if (partnerLookupOption.directoryId) {
              this.partnerService.setActivePartnerDirectory(
                this.context,
                partnerLookupOption.directoryId
              );
              this.partnerService.setActivePartner(
                this.context,
                partnerLookupOption.partner.id
              );
            }
          }),
          switchMap(() => EMPTY)
        );
      })
    );
  }

  getLabel(option: PartnerLookupOption) {
    if (option === undefined || option === null || option.partner == null) {
      return null;
    }

    return option.partner.companyName;
  }

  onSelectFromPartnerDirectory() {
    this.dialog
      .open<PartnerDirectoryPickerComponent, PartnerPickerContext, CustomerPartner>(
        PartnerDirectoryPickerComponent,
        {
          data: this.context,
          // HACK this should be dynamic based on content, check if component needs to be re layouted to not scroll
          height: '800px',
        }
      )
      .afterClosed()
      .pipe(
        tap((partner) => {
          if (!partner) {
            return;
          }
          this.formGroup.controls.option.patchValue({ partner });
        })
      )
      .subscribe();
  }

  onCreateNewPartner() {
    const dialog = this.dialog.open<
      PartnerCreateDialogComponent,
      { context: PartnerPickerContext; partner: CustomerPartner },
      CustomerPartner
    >(PartnerCreateDialogComponent, {
      data: { context: this.context, partner: null },
      width: '400px',
    });

    dialog
      .afterClosed()
      .pipe(
        tap((response) => {
          this.autocompleteInput.nativeElement.blur();
          this.autocompleteTrigger.closePanel();

          if (!response) {
            return;
          }

          const fixedPartner = { ...response };

          // State kann null sein. Da beim serialisieren der wert nicht übertragen wird ist das Property state undefined.
          if (
            fixedPartner.address &&
            fixedPartner.address.state === undefined
          ) {
            fixedPartner.address = { state: null, ...fixedPartner.address };
          }

          this.formGroup.controls.option.patchValue({ partner: fixedPartner });
        })
      )
      .subscribe();
  }

  protected getFormControls(): Controls<PartnerLookupForm> {
    return {
      option: new FormControl(null, [
        // Validators.required,
        // CustomValidators.isObject
      ]),
      address: new SubFormGroup(null),
      type: new FormControl({ value: null, disabled: true }),
    };
  }

  public setRequiredState(isRequired: boolean) {
    const option = this.formGroup.controls.option;
    option.setValidators(
      isRequired
        ? [Validators.required, CustomValidators.isObject]
        : [CustomValidators.isObject]
    );
    option.updateValueAndValidity({ onlySelf: true, emitEvent: false });
  }

  protected transformFromFormGroup(value: PartnerLookupForm): CustomerPartner {
    if (!value) {
      return null;
    }

    if (!value.option || typeof value.option === 'string') {
      return null;
    }

    return value.option.partner;
  }

  protected transformToFormGroup(
    value: CustomerPartner,
    defaultValues: Partial<PartnerLookupForm>
  ): PartnerLookupForm {
    if (!value) {
      return defaultValues as PartnerLookupForm;
    }

    return {
      option: {
        directoryId: null,
        directoryName: null,
        partner: value,
      },
      address: value.address,
      type: value.type,
    };
  }
}
