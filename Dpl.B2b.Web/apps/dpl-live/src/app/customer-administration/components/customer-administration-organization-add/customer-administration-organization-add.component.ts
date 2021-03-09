import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
} from '@angular/core';
import { map, switchMap, tap } from 'rxjs/operators';
import { Organization } from '../../../core/services/dpl-api-services';
import { DplApiService } from '../../../core/services/dpl-api.service';
import { CustomerAdministrationService } from '../../services/customer-administration.service';

@Component({
  selector: 'dpl-customer-administration-organization-add',
  templateUrl: './customer-administration-organization-add.component.html',
  styleUrls: ['./customer-administration-organization-add.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationOrganizationAddComponent implements OnInit {
  popupVisible = false;

  constructor(
    private dpl: DplApiService,
    private cd: ChangeDetectorRef,
    private customerAdministrationService: CustomerAdministrationService
  ) {}

  ngOnInit(): void {}

  formSubmit(formData: Organization) {
    this.dpl.organizationsAdministrationService
      .post({
        ...formData,
      })
      .pipe(
        switchMap((newOrganization) => {
          this.popupVisible = false;
          this.cd.detectChanges();
          return this.customerAdministrationService.refreshOrganizations().pipe(
            tap(() => {
              this.customerAdministrationService.setActiveOrganizationById(
                newOrganization.id
              );
            })
          );
        })
      )
      .subscribe();
  }
}
