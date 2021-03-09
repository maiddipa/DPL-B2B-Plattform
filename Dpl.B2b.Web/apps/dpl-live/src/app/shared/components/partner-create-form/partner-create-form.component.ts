import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  ChangeDetectorRef,
} from '@angular/core';
import { AbstractControl, FormControl, Validators } from '@angular/forms';
import { Controls, NgxAutomaticRootFormComponent } from 'ngx-sub-form';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { CountriesService } from '../../../master-data/countries/services/countries.service';
import { ICountry } from '../../../master-data/countries/state/country.model';
import { PartnerService } from '../../../partners/services/partner.service';
import { IPartnerDirectory } from '../../../partners/state/partner-directory.model';
import { IPartner } from '../../../partners/state/partner.model';
import { CountryPipeFormat } from '../../pipes';
import { ValidationDataService } from '../../services';
import { PartnerDialog } from '../partner-create-dialog/partner-create-dialog.component';
import has = Reflect.has;

//import { Partner } from '@app/shared/models/partner.model';
interface IViewData {
  partnerDirectories: IPartnerDirectory[];
  countries: ICountry[];
}

interface PartnerForm {
  companyName: string;
  partnerDirectory: number | null;
  country: number;
  city: string;
  street: string;
  postalCode: string;
}

export const hasValidator = (
  abstractControl: AbstractControl,
  field
): boolean => {
  if (field === null) {
    field = 'required';
  }

  if (abstractControl.validator) {
    const validator = abstractControl.validator({} as AbstractControl);
    if (validator && validator[field]) {
      return true;
    }
  }
  if (abstractControl['controls']) {
    for (const controlName in abstractControl['controls']) {
      if (abstractControl['controls'][controlName]) {
        if (hasValidator(abstractControl['controls'][controlName], field)) {
          return true;
        }
      }
    }
  }
  return false;
};

@Component({
  selector: 'dpl-partner-create-form',
  templateUrl: './partner-create-form.component.html',
  styleUrls: ['./partner-create-form.component.scss'],
})
export class PartnerCreateFormComponent
  extends NgxAutomaticRootFormComponent<PartnerDialog, PartnerForm>
  implements OnInit, OnDestroy {
  @Input('partner')
  dataInput: Required<PartnerDialog>;

  @Output('onChange')
  public dataOutput: EventEmitter<PartnerDialog> = new EventEmitter();

  viewData$: Observable<IViewData>;

  countryFormat: CountryPipeFormat = 'long';

  constructor(
    private partnerService: PartnerService,
    private countryService: CountriesService,
    private cd: ChangeDetectorRef
  ) {
    super(cd);
  }

  hasRequiredField = (abstractControl: AbstractControl) =>
    hasValidator(abstractControl, 'required');

  getDefaultValues() {
    const defaultValue: PartnerForm = {
      companyName: null,
      partnerDirectory: null,
      country: null,
      postalCode: null,
      city: null,
      street: null,
    };

    return defaultValue;
  }

  ngOnInit() {
    super.ngOnInit();

    const partnerDirectorys$ = this.partnerService.getPartnerDirectories();
    const countries$ = this.countryService.getCountries();

    this.viewData$ = combineLatest([partnerDirectorys$, countries$]).pipe(
      map(([partnerDirectorys, countries]) => {
        return { partnerDirectories: partnerDirectorys, countries };
      })
    );
  }

  ngOnDestroy() {
    super.ngOnDestroy();
  }

  protected getFormControls(): Controls<PartnerForm> {
    return {
      companyName: new FormControl(null, [
        Validators.required,
        Validators.maxLength(ValidationDataService.maxLength.companyName),
      ]),
      partnerDirectory: new FormControl(null, {
        validators: [Validators.required],
      }),
      country: new FormControl(null),
      postalCode: new FormControl(null, [
        Validators.maxLength(ValidationDataService.maxLength.postalCode),
      ]),
      city: new FormControl(null, [
        Validators.maxLength(ValidationDataService.maxLength.city),
      ]),
      street: new FormControl(null, [
        Validators.maxLength(ValidationDataService.maxLength.street),
      ]),
    };
  }

  protected transformToFormGroup(partner: PartnerDialog): PartnerForm {
    if (!partner) {
      return this.getDefaultValues();
    }

    const { companyName, street, postalCode, city, country } = partner;

    return {
      companyName,
      partnerDirectory: partner.partnerDirectoryId,
      country,
      city,
      postalCode,
      street,
    } as PartnerForm;
  }

  protected transformFromFormGroup(partnerForm: PartnerForm): PartnerDialog {
    return {
      id: null,
      street: partnerForm.street,
      postalCode: partnerForm.postalCode,
      city: partnerForm.city,
      country: partnerForm.country,
      companyName: partnerForm.companyName,
      partnerDirectoryId: partnerForm.partnerDirectory
        ? Number(partnerForm.partnerDirectory)
        : null,
      valid: this.formGroup.valid,
    };
  }
}
