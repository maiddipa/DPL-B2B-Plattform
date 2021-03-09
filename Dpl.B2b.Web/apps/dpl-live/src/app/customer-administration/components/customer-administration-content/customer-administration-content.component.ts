import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { combineLatest, Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import {
  CustomerAdminOrganizationUnit,
  CustomerAdminScope,
} from '../../services/customer-administration.service.types';
import { UnitsQuery } from '../../state/units.query';
type ViewData = {
  unit: CustomerAdminOrganizationUnit;
  childs: CustomerAdminOrganizationUnit[];
  allUnits: CustomerAdminOrganizationUnit[];
};
@Component({
  selector: 'dpl-customer-administration-content',
  templateUrl: './customer-administration-content.component.html',
  styleUrls: ['./customer-administration-content.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationContentComponent implements OnInit {
  viewData$: Observable<ViewData>;
  scope = CustomerAdminScope;

  constructor(private unitsQuery: UnitsQuery) {}

  ngOnInit(): void {
    const allUnits$ = this.unitsQuery.selectAll();

    const activeUnit$ = this.unitsQuery.selectActive().pipe(filter((x) => !!x));

    this.viewData$ = combineLatest([allUnits$, activeUnit$]).pipe(
      map((latest) => {
        const [allUnits, activeUnit] = latest;
        const viewData: ViewData = {
          unit: activeUnit,
          childs: allUnits.filter(
            (x) => x.parentIdString === activeUnit?.idString
          ),
          allUnits,
        };
        return viewData;
      })
    );
  }

  getOrganizationCustomerUnits(viewData: ViewData) {
    return viewData.allUnits.filter(
      (unit) =>
        unit.parentIdString === viewData.unit.parentIdString &&
        unit.scope === CustomerAdminScope.Customer
    );
  }
}
