import {
  Component,
  ChangeDetectionStrategy,
  Input,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { CustomerAdministrationService } from '../../services/customer-administration.service';
import { CustomerAdminOrganizationUnit } from '../../services/customer-administration.service.types';

@Component({
  selector: 'dpl-customer-administration-breadcrumb',
  templateUrl: './customer-administration-breadcrumb.component.html',
  styleUrls: ['./customer-administration-breadcrumb.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationBreadcrumbComponent implements OnChanges {
  @Input() unit: CustomerAdminOrganizationUnit;
  @Input() allUnits: CustomerAdminOrganizationUnit[];

  parentUnit: CustomerAdminOrganizationUnit;

  constructor(private service: CustomerAdministrationService) {}
  ngOnChanges(changes: SimpleChanges): void {
    this.parentUnit = this.allUnits.find(
      (x) => x.idString === this.unit.parentIdString
    );
  }

  navigate() {
    this.service.setActiveUnit(this.unit.idString);
  }
}
