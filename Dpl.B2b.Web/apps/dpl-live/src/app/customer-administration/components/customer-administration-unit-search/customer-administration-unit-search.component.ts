import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import ArrayStore from 'devextreme/data/array_store';
import DataSource from 'devextreme/data/data_source';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { OrganizationScopedDataSet } from '../../../core/services/dpl-api-services';
import { CustomerAdministrationService } from '../../services/customer-administration.service';
import {
  CustomerAdminOrganizationUnit,
  CustomerAdminScope,
} from '../../services/customer-administration.service.types';
import { OrganizationsQuery } from '../../state/organizations.query';
import { UnitsQuery } from '../../state/units.query';

type ViewData = {
  unitDataSource: DataSource;
  units: CustomerAdminOrganizationUnit[];
};

@Component({
  selector: 'dpl-customer-administration-unit-search',
  templateUrl: './customer-administration-unit-search.component.html',
  styleUrls: ['./customer-administration-unit-search.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationUnitSearchComponent implements OnInit {
  viewData$: Observable<ViewData>;

  constructor(
    private customerAdministrationService: CustomerAdministrationService,
    private unitsQuery: UnitsQuery
  ) {}

  ngOnInit(): void {
    this.viewData$ = this.unitsQuery.selectOnlyPhysicalUnits().pipe(
      map((units) => {
        // ToDo remove virtual units from array
        const unitDataSource = new DataSource({
          store: new ArrayStore({
            key: 'idString',
            data: units,
          }),
          group: 'scope',
        });
        const viewData: ViewData = {
          unitDataSource,
          units,
        };
        return viewData;
      })
    );
  }

  unitSelected(e) {
    this.customerAdministrationService.setActiveUnit(e.value);
  }
}
