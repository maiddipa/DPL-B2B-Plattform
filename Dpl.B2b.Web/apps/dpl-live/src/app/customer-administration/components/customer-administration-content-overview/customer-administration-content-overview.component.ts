import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
} from '@angular/core';
import { CustomerAdministrationService } from '../../services/customer-administration.service';
import { CustomerAdminOrganizationUnit } from '../../services/customer-administration.service.types';

@Component({
  selector: 'dpl-customer-administration-content-overview',
  templateUrl: './customer-administration-content-overview.component.html',
  styleUrls: ['./customer-administration-content-overview.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationContentOverviewComponent implements OnInit {
  @Input() unit: CustomerAdminOrganizationUnit;
  @Input() childs: CustomerAdminOrganizationUnit[];

  constructor(private service: CustomerAdministrationService) {}

  ngOnInit(): void {}

  selectUnit(unit: CustomerAdminOrganizationUnit) {
    this.service.setActiveUnit(unit.idString);
  }

  selectTab(e) {
    console.log(e);
  }
}
