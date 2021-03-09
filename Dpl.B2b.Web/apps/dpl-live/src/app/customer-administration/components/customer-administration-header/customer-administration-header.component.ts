import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { MatSelect, MatSelectChange } from '@angular/material/select';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  Organization,
  OrganizationScopedDataSet,
} from '../../../core/services/dpl-api-services';
import { CustomerAdministrationService } from '../../services/customer-administration.service';
import { CustomerAdminOrganizationLookup } from '../../services/customer-administration.service.types';
import { OrganizationsQuery } from '../../state/organizations.query';

type ViewData = {
  orgs: OrganizationScopedDataSet[];
  selectedOrg?: Organization;
};

@Component({
  selector: 'dpl-customer-administration-header',
  templateUrl: './customer-administration-header.component.html',
  styleUrls: ['./customer-administration-header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationHeaderComponent implements OnInit {
  viewData$: Observable<ViewData>;
  selectedOrg: CustomerAdminOrganizationLookup;

  constructor(
    private organizationsQuery: OrganizationsQuery,
    private service: CustomerAdministrationService
  ) {}

  ngOnInit(): void {
    const orgs$ = this.organizationsQuery.selectAll();
    const selectedOrg$ = this.organizationsQuery.selectActive();
    this.viewData$ = combineLatest([orgs$, selectedOrg$]).pipe(
      map(([orgs, selectedOrg]) => {
        const viewData: ViewData = {
          orgs,
          selectedOrg,
        };
        return viewData;
      })
    );
  }

  organizationChange(event: MatSelectChange) {
    if (event) {
      console.log(event.value);
      this.selectedOrg = event.value;
      this.service.setActiveOrganization(this.selectedOrg);
    }
  }
}
