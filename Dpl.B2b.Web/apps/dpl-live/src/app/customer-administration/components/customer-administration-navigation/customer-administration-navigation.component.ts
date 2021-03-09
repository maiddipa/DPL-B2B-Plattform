import { FlatTreeControl } from '@angular/cdk/tree';
import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import {
  MatTreeFlatDataSource,
  MatTreeFlattener,
} from '@angular/material/tree';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { CustomerAdministrationService } from '../../services/customer-administration.service';
import {
  CustomerAdminOrganizationUnitFlatNode,
  CustomerAdminOrganizationUnitHierarchy,
} from '../../services/customer-administration.service.types';
import { UnitsQuery } from '../../state/units.query';

type ViewData = {
  dataSource: MatTreeFlatDataSource<
    CustomerAdminOrganizationUnitHierarchy,
    CustomerAdminOrganizationUnitFlatNode
  >;
  treeControl: FlatTreeControl<
    CustomerAdminOrganizationUnitFlatNode,
    CustomerAdminOrganizationUnitFlatNode
  >;
};

@Component({
  selector: 'dpl-customer-administration-navigation',
  templateUrl: './customer-administration-navigation.component.html',
  styleUrls: ['./customer-administration-navigation.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationNavigationComponent implements OnInit {
  viewData$: Observable<ViewData>;
  isActiveNode
  
  constructor(private service: CustomerAdministrationService, private query:UnitsQuery) {}
  hasChild = (_: number, node: CustomerAdminOrganizationUnitFlatNode) =>
    node.expandable;

  private _transformer = (
    node: CustomerAdminOrganizationUnitHierarchy,
    level: number
  ) => {
    return {
      expandable: !!node.children && node.children.length > 0,
      name: node.name,
      level: level,
      id: node.id,
      scope: node.scope,
    };
  };

  ngOnInit(): void {
    this.viewData$ = this.service.getUnitsHierachy().pipe(
      map((unitsHierarchy) => {
        const treeControl = new FlatTreeControl<
          CustomerAdminOrganizationUnitFlatNode
        >(
          (node) => node.level,
          (node) => node.expandable
        );

        const treeFlattener = new MatTreeFlattener(
          this._transformer,
          (node) => node.level,
          (node) => node.expandable,
          (node) => node.children
        );

        const dataSource = new MatTreeFlatDataSource(
          treeControl,
          treeFlattener
        );

        dataSource.data = unitsHierarchy;

        const viewData: ViewData = {
          dataSource,
          treeControl,
        };
        return viewData;
      }),
      tap(viewData=>{
        viewData.treeControl.expandAll();        
      })
    );
  }

  
}
