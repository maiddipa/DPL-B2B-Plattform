import {
  AfterViewInit,
  Component,
  Inject,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Address, CustomerPartner, PartnerType } from '@app/api/dpl';
import { Subscription } from 'rxjs';

import { PartnerService } from '../../../partners/services/partner.service';
import { PartnerPickerContext } from '../../../partners/services/partner.service.types';
import { PartnerCreateFormComponent } from '../partner-create-form/partner-create-form.component';
import { WrappedError } from '../../../core/services/app-error-handler.service';

export interface PartnerDialog {
  companyName: string;
  partnerDirectoryId?: number | null;
  id?: number;
  street?: string | null;
  postalCode?: string | null;
  city?: string | null;
  country?: number;
  valid: boolean;
}

@Component({
  selector: 'dpl-partner-create-dialog',
  templateUrl: './partner-create-dialog.component.html',
  styleUrls: ['./partner-create-dialog.component.scss'],
})
export class PartnerCreateDialogComponent
  implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild('form')
  form: PartnerCreateFormComponent;

  // Sperren von Submit, vermeiden von doppelten aufruf
  lockSubmit = false;
  afterClosed$: Subscription;

  constructor(
    public dialogRef: MatDialogRef<PartnerCreateDialogComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: { partner: CustomerPartner; context: PartnerPickerContext },
    private partnerService: PartnerService
  ) {}

  partnerDialog: PartnerDialog = {
    city: null,
    country: null,
    id: null,
    postalCode: null,
    street: null,
    companyName: null,
    partnerDirectoryId: null,
    valid: false,
  };

  onCancel(): void {
    this.dialogRef.close();
    this.lockSubmit = false;
  }

  onCreatePartner(event: Event): void {
    // Note: AS Besser mit einem exhaustMap habe ich auf die schnelle aber nicht lösen können
    // Doppelten aufruf unterbinden
    if (this.lockSubmit) return;

    this.lockSubmit = true;

    const {
      id,
      companyName,
      street,
      postalCode,
      city,
      country,
      partnerDirectoryId,
    } = this.partnerDialog;

    const type = this.getPartnerType();

    let address: Address = null;

    // Service akzeptiert leere Adresse nur wenn kein Objekt versendet wird
    if (
      (city && city.length > 0) ||
      (postalCode && postalCode.length > 0) ||
      (street && street.length > 0) ||
      isNaN(country)
    ) {
      address = {
        city,
        country,
        postalCode,
        state: null,
        street1: street,
        street2: null,
      } as Address;
    }

    const partner = {
      id,
      address,
      companyName,
      type,
      partnerDirectoryId,
    } as CustomerPartner;

    const request = {
      directoryId: partnerDirectoryId,
      partner: partner,
      type: type,
    };

    const createPartner$ = this.partnerService.createPartner(request);

    createPartner$.subscribe({
      next: (success) => {
        this.dialogRef.close(success);
      },
      error: (error: WrappedError) => {
        this.lockSubmit = false;
        throw error; // Display error is handled by app-error-handler
      },
      complete: () => (this.lockSubmit = false),
    });
  }

  getPartnerType(): PartnerType {
    switch (this.data.context) {
      case 'recipient':
        return PartnerType.Recipient;
      case 'shipper':
        return PartnerType.Shipper;
      case 'subShipper':
        return PartnerType.SubShipper;
      case 'supplier':
        return PartnerType.Supplier;
      case 'prefill':
      default:
        return PartnerType.Default;
    }
  }

  submitDisabled(): boolean {
    // Nach aufruf von onCreatePartner => lockSubmit=true
    if (this.lockSubmit) return true;

    // Rendern kann vor Form initialisierung passieren
    if (!this.form || !this.form.formGroup) return true;

    // Form ist valide
    return !(this.form.formGroup.valid && this.partnerDialog.valid);
  }

  onChange(partner: PartnerDialog) {
    this.partnerDialog = partner;
  }
  ngAfterViewInit(): void {}

  ngOnInit(): void {
    this.afterClosed$ = this.dialogRef.afterClosed().subscribe(() => {
      this.lockSubmit = false;
    });
  }

  ngOnDestroy(): void {
    this.afterClosed$.unsubscribe();
  }
}
