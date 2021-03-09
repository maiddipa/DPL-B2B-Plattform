import { Component, OnInit } from '@angular/core';
import { combineLatest, Observable } from 'rxjs';
import {
  CustomerAdminOrganizationLookup,
  CustomerAdminOrganizationUnit,
} from './services/customer-administration.service.types';
import { CustomerAdministrationService } from './services/customer-administration.service';
import { map } from 'rxjs/operators';
import { Organization } from '../core/services/dpl-api-services';

type ViewData = {
  organizations: Organization[];
  units: CustomerAdminOrganizationUnit[];
};

@Component({
  selector: 'dpl-customer-administration',
  templateUrl: './customer-administration.component.html',
  styleUrls: ['./customer-administration.component.scss'],
})
export class CustomerAdministrationComponent implements OnInit {
  viewData$: Observable<ViewData>;

  constructor(private customerAdminService: CustomerAdministrationService) {}

  ngOnInit(): void {
    // refresh organization store
    this.viewData$ = combineLatest([
      this.customerAdminService.refreshOrganizations(),
      this.customerAdminService.refreshUnits(),
    ]).pipe(
      map((latest) => {
        const [organizations, units] = latest;
        const viewData: ViewData = {
          organizations,
          units,
        };
        return viewData;
      })
    );
  }
}
