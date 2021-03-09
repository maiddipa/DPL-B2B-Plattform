import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
} from '@angular/core';
import { Observable } from 'rxjs';
import { filter, map, pluck, tap } from 'rxjs/operators';
import { CustomerAdministrationService } from '../../services/customer-administration.service';
import { CustomerAdminOrganizationUnitFlatNode } from '../../services/customer-administration.service.types';
import { UnitsQuery } from '../../state/units.query';

@Component({
  selector: 'dpl-customer-administration-node',
  templateUrl: './customer-administration-node.component.html',
  styleUrls: ['./customer-administration-node.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationNodeComponent implements OnInit {
  color$: Observable<string>;
  @Input() node: CustomerAdminOrganizationUnitFlatNode;

  constructor(
    private service: CustomerAdministrationService,
    private query: UnitsQuery
  ) {}

  ngOnInit(): void {
    this.color$ = this.query.selectActive().pipe(
      filter((x) => !!x),
      pluck('idString'),
      map((id) => (id === this.node.id ? 'primary' : 'default'))
    );
  }

  nodeSelected(node:CustomerAdminOrganizationUnitFlatNode) {
    this.service.setActiveUnit(node.id);
  }
}
