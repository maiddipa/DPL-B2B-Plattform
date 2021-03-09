import { Injectable } from '@angular/core';
import * as _ from 'lodash';

import { Observable, of } from 'rxjs';
import { filter, map, switchMap, tap } from 'rxjs/operators';
import { deNormalizeUnits } from './normalize';
import { OrganizationsQuery } from '../state/organizations.query';
import { OrganizationsStore } from '../state/organizations.store';
import { UnitsStore } from '../state/units.store';
import { UnitsQuery } from '../state/units.query';
import {
  CustomerAdminOrganizationLookup,
  CustomerAdminOrganizationUnit,
  CustomerAdminOrganizationUnitFlatNode,
  CustomerAdminOrganizationUnitHierarchy,
  CustomerAdminScope,
} from './customer-administration.service.types';
import {
  DomainScope,
  OrganizationsAdministrationApiService,
} from '../../core/services/dpl-api-services';
import { MatDialog } from '@angular/material/dialog';
import {
  DynamicConfirmationDialogComponent,
  DynamicConfirmationDialogData,
  DynamicConfirmationDialogResult,
} from '../../shared';

@Injectable({
  providedIn: 'root',
})
export class CustomerAdministrationService {
  constructor(
    private service: OrganizationsAdministrationApiService,
    private orgsStore: OrganizationsStore,
    private orgsQuery: OrganizationsQuery,
    private unitsStore: UnitsStore,
    private unitsQuery: UnitsQuery,
    private dialog: MatDialog
  ) {}

  refreshOrganizations() {
    return this.service.get().pipe(
      tap((items) => {
        this.orgsStore.set(items);
      })
    );
  }

  setActiveOrganization(org?: CustomerAdminOrganizationLookup) {
    this.orgsStore.setActive(org ? org.id : null);
  }

  setActiveOrganizationById(orgId?: number) {
    this.orgsStore.setActive(orgId ? orgId : null);
  }

  refreshUnits() {
    return this.orgsQuery.selectActive().pipe(
      switchMap((org) => {
        if (org) {
          return this.service.getById(org.id).pipe(
            map((scopedUnits) => {
              return scopedUnits.map((unit) => {
                const clientUnit: CustomerAdminOrganizationUnit = {
                  id: unit.id,
                  name: unit.name,
                  parent: unit.parentId,
                  organization: unit.organizationId,
                  scope: this.mapDomainScopeToCustomerAdminScope(unit.scope),
                };
                return clientUnit;
              });
            })
          );
        }
        return of([]);
      }),
      tap((units) => {
        // transform organization units
        const unitsWithVirtualUnits = this.getUnitsWithVirtual(units);
        this.unitsStore.setActive(null);
        this.unitsStore.set(unitsWithVirtualUnits);
        if (unitsWithVirtualUnits.length > 0) {
          this.unitsStore.setActive(
            unitsWithVirtualUnits.find(
              (x) => x.scope === CustomerAdminScope.Organization
            ).idString
          );
        }
      })
    );
  }

  private mapDomainScopeToCustomerAdminScope(domainScope: DomainScope) {
    switch (domainScope) {
      case DomainScope.Customer:
        return CustomerAdminScope.Customer;
      case DomainScope.Division:
        return CustomerAdminScope.Division;
      case DomainScope.LoadingLocation:
        return CustomerAdminScope.LoadingLocation;
      case DomainScope.Organization:
        return CustomerAdminScope.Organization;
    }
  }

  setActiveUnit(id?: string) {
    this.unitsStore.setActive(id ? id : null);
  }

  getUnitsHierachy() {
    return this.unitsQuery.selectAll().pipe(
      map((unitsWithVirtualUnits) => {
        const dict = this.getDict(unitsWithVirtualUnits);
        const test = unitsWithVirtualUnits.map((unit) => {
          const children = this.getChildren(dict, unit);
          const { name, scope, idString } = unit;
          return {
            name,
            scope,
            id: idString,
            children,
            data: unit,
          };
        });

        const normalized = deNormalizeUnits(
          test.filter(
            (i) => i.scope === CustomerAdminScope.Organization
          ) as any,
          {
            units: _(test)
              .keyBy((i) => i.id)
              .value() as any,
          }
        );
        return normalized;
      })
    );
  }

  getActualChildren(
    dict: ReturnType<CustomerAdministrationService['getDict']>,
    scope: CustomerAdminScope,
    idString: string
  ) {
    return (dict[scope]?.[idString] || []).map((unit) => {
      return unit.idString;
    });
  }

  getChildren(
    dict: ReturnType<CustomerAdministrationService['getDict']>,
    unit: CustomerAdminOrganizationUnit
  ) {
    switch (unit.scope) {
      case CustomerAdminScope.Organization:
        return [
          ...dict[CustomerAdminScope.OrganizationGeneral][unit.idString].map(
            (i) => i.idString
          ),
          ...dict[CustomerAdminScope.OrganizationUsers][unit.idString].map(
            (i) => i.idString
          ),
          ...dict[CustomerAdminScope.OrganizationGroups][unit.idString].map(
            (i) => i.idString
          ),
          ...dict[CustomerAdminScope.OrganizationGroupMembership][
            unit.idString
          ].map((i) => i.idString),
          ...dict[CustomerAdminScope.OrganizationCustomerAdd][
            unit.idString
          ].map((i) => i.idString),
          ...dict[CustomerAdminScope.OrganizationCustomers][unit.idString].map(
            (i) => i.idString
          ),
        ];
      case CustomerAdminScope.OrganizationCustomers:
        return this.getActualChildren(
          dict,
          CustomerAdminScope.Customer,
          unit.parentIdString
        );

      case CustomerAdminScope.Customer:
        return [
          ...dict[CustomerAdminScope.CustomerGeneral][unit.idString].map(
            (i) => i.idString
          ),
          ...dict[CustomerAdminScope.CustomerUsers][unit.idString].map(
            (i) => i.idString
          ),
          ...dict[CustomerAdminScope.CustomerGroups][unit.idString].map(
            (i) => i.idString
          ),
          ...dict[CustomerAdminScope.CustomerGroupMembership][
            unit.idString
          ].map((i) => i.idString),
          ...dict[CustomerAdminScope.CustomerNumberSequences][
            unit.idString
          ].map((i) => i.idString),
          ...dict[CustomerAdminScope.CustomerDocumentSettings][
            unit.idString
          ].map((i) => i.idString),
          ...dict[CustomerAdminScope.CustomerPostingAccounts][
            unit.idString
          ].map((i) => i.idString),
          ...dict[CustomerAdminScope.CustomerDivisionAdd][unit.idString].map(
            (i) => i.idString
          ),
          ...this.getActualChildren(
            dict,
            CustomerAdminScope.Division,
            unit.idString
          ),
        ];
      case CustomerAdminScope.Division:
        return [
          ...dict[CustomerAdminScope.DivisionGeneral][unit.idString].map(
            (i) => i.idString
          ),
          ...dict[CustomerAdminScope.DivisionGroups][unit.idString].map(
            (i) => i.idString
          ),
          ...dict[CustomerAdminScope.DivisionGroupMembership][
            unit.idString
          ].map((i) => i.idString),
          ...dict[CustomerAdminScope.DivisionDocumentSettings][
            unit.idString
          ].map((i) => i.idString),
          ...dict[CustomerAdminScope.DivisionLoadingLocationAdd][
            unit.idString
          ].map((i) => i.idString),
          ...this.getActualChildren(
            dict,
            CustomerAdminScope.LoadingLocation,
            unit.idString
          ),
        ];
      case CustomerAdminScope.LoadingLocation:
        return [
          ...dict[CustomerAdminScope.LoadingLocationGeneral][unit.idString].map(
            (i) => i.idString
          ),
          ...dict[CustomerAdminScope.LoadingLocationBusinessHours][
            unit.idString
          ].map((i) => i.idString),
          ...dict[CustomerAdminScope.LoadingLocationBusinessHoursExceptions][
            unit.idString
          ].map((i) => i.idString),
        ];
      // case CustomerAdminScope.LoadingLocation:
      //   childScope=CustomerAdminScope.Customer;
      //   break;

      default:
        return [];
    }
  }

  getUnitsWithVirtual(organizationUnits: CustomerAdminOrganizationUnit[]) {
    return _(organizationUnits)
      .map((unit) => {
        const virtualUnits = this.getVirtualUnits(unit.scope, unit.id);
        const physicalUnit = {
          ...unit,
          idString: unit.scope + unit.id,
          parentIdString: this.getParentScope(unit.scope) + unit.parent,
        };
        return [...virtualUnits, physicalUnit];
      })
      .flatten()
      .value();
  }

  private getParentScope(scope: CustomerAdminScope) {
    switch (scope) {
      case CustomerAdminScope.Customer:
        return CustomerAdminScope.Organization;
      case CustomerAdminScope.Division:
        return CustomerAdminScope.Customer;
      case CustomerAdminScope.LoadingLocation:
        return CustomerAdminScope.Division;
      default:
        return null;
    }
  }

  getVirtualUnits(
    scope: CustomerAdminScope,
    parent: number
  ): CustomerAdminOrganizationUnit[] {
    switch (scope) {
      case CustomerAdminScope.Organization:
        return [
          {
            idString: CustomerAdminScope.OrganizationGeneral + parent,
            name: 'Allgemein',
            scope: CustomerAdminScope.OrganizationGeneral,
            parent,
            parentIdString: CustomerAdminScope.Organization + parent,
          },
          {
            idString: CustomerAdminScope.OrganizationUsers + parent,
            name: 'Nutzer',
            scope: CustomerAdminScope.OrganizationUsers,
            parent,
            parentIdString: CustomerAdminScope.Organization + parent,
          },
          {
            idString: CustomerAdminScope.OrganizationGroups + parent,
            name: 'Gruppen',
            scope: CustomerAdminScope.OrganizationGroups,
            parent,
            parentIdString: CustomerAdminScope.Organization + parent,
          },
          {
            idString: CustomerAdminScope.OrganizationGroupMembership + parent,
            name: 'Gruppenzugehörigkeit',
            scope: CustomerAdminScope.OrganizationGroupMembership,
            parent,
            parentIdString: CustomerAdminScope.Organization + parent,
          },
          {
            idString: CustomerAdminScope.OrganizationCustomerAdd + parent,
            name: 'Neuer Kunde',
            scope: CustomerAdminScope.OrganizationCustomerAdd,
            parent,
            parentIdString: CustomerAdminScope.Organization + parent,
          },
          {
            idString: CustomerAdminScope.OrganizationCustomers + parent,
            name: 'Kunden',
            scope: CustomerAdminScope.OrganizationCustomers,
            parent,
            parentIdString: CustomerAdminScope.Organization + parent,
          },
        ];
      case CustomerAdminScope.Customer:
        return [
          {
            idString: CustomerAdminScope.CustomerGeneral + parent,
            name: 'Allgemein',
            scope: CustomerAdminScope.CustomerGeneral,
            parent,
            parentIdString: CustomerAdminScope.Customer + parent,
          },
          {
            idString: CustomerAdminScope.CustomerUsers + parent,
            name: 'Nutzer',
            scope: CustomerAdminScope.CustomerUsers,
            parent,
            parentIdString: CustomerAdminScope.Customer + parent,
          },
          {
            idString: CustomerAdminScope.CustomerGroups + parent,
            name: 'Gruppen',
            scope: CustomerAdminScope.CustomerGroups,
            parent,
            parentIdString: CustomerAdminScope.Customer + parent,
          },
          {
            idString: CustomerAdminScope.CustomerGroupMembership + parent,
            name: 'Gruppenzugehörigkeit',
            scope: CustomerAdminScope.CustomerGroupMembership,
            parent,
            parentIdString: CustomerAdminScope.Customer + parent,
          },
          {
            idString: CustomerAdminScope.CustomerNumberSequences + parent,
            name: 'Nummernkreise',
            scope: CustomerAdminScope.CustomerNumberSequences,
            parent,
            parentIdString: CustomerAdminScope.Customer + parent,
          },
          {
            idString: CustomerAdminScope.CustomerDocumentSettings + parent,
            name: 'Dokumente',
            scope: CustomerAdminScope.CustomerDocumentSettings,
            parent,
            parentIdString: CustomerAdminScope.Customer + parent,
          },
          {
            idString: CustomerAdminScope.CustomerPostingAccounts + parent,
            name: 'Konten',
            scope: CustomerAdminScope.CustomerPostingAccounts,
            parent,
            parentIdString: CustomerAdminScope.Customer + parent,
          },
          {
            idString: CustomerAdminScope.CustomerDivisionAdd + parent,
            name: 'Neue Abteilung',
            scope: CustomerAdminScope.CustomerDivisionAdd,
            parent,
            parentIdString: CustomerAdminScope.Customer + parent,
          },
        ];
      case CustomerAdminScope.Division:
        return [
          {
            idString: CustomerAdminScope.DivisionGeneral + parent,
            name: 'Allgemein',
            scope: CustomerAdminScope.DivisionGeneral,
            parent,
            parentIdString: CustomerAdminScope.Division + parent,
          },
          {
            idString: CustomerAdminScope.DivisionGroups + parent,
            name: 'Gruppen',
            scope: CustomerAdminScope.DivisionGroups,
            parent,
            parentIdString: CustomerAdminScope.Division + parent,
          },
          {
            idString: CustomerAdminScope.DivisionGroupMembership + parent,
            name: 'Gruppenzugehörigkeit',
            scope: CustomerAdminScope.DivisionGroupMembership,
            parent,
            parentIdString: CustomerAdminScope.Division + parent,
          },
          {
            idString: CustomerAdminScope.DivisionDocumentSettings + parent,
            name: 'Dokumente',
            scope: CustomerAdminScope.DivisionDocumentSettings,
            parent,
            parentIdString: CustomerAdminScope.Division + parent,
          },
          {
            idString: CustomerAdminScope.DivisionLoadingLocationAdd + parent,
            name: 'Neue Ladestelle',
            scope: CustomerAdminScope.DivisionLoadingLocationAdd,
            parent,
            parentIdString: CustomerAdminScope.Division + parent,
          },
        ];
      case CustomerAdminScope.LoadingLocation:
      default:
        return [
          {
            idString: CustomerAdminScope.LoadingLocationGeneral + parent,
            name: 'Allgemein',
            scope: CustomerAdminScope.LoadingLocationGeneral,
            parent,
            parentIdString: CustomerAdminScope.LoadingLocation + parent,
          },
          {
            idString: CustomerAdminScope.LoadingLocationBusinessHours + parent,
            name: 'Öffnungszeiten',
            scope: CustomerAdminScope.LoadingLocationBusinessHours,
            parent,
            parentIdString: CustomerAdminScope.LoadingLocation + parent,
          },
          {
            idString:
              CustomerAdminScope.LoadingLocationBusinessHoursExceptions +
              parent,
            name: 'Öffnungszeiten Ausnahmen',
            scope: CustomerAdminScope.LoadingLocationBusinessHoursExceptions,
            parent,
            parentIdString: CustomerAdminScope.LoadingLocation + parent,
          },
        ];
    }
  }

  getDict(units: CustomerAdminOrganizationUnit[]) {
    const testDict = _(units)
      .groupBy((x) => x.scope)
      .mapValues((values) => {
        return _(values)
          .groupBy((x) => x.parentIdString)
          .value();
      })
      .value();
    return testDict;
  }

  transformFlatNode(node: CustomerAdminOrganizationUnitHierarchy) {
    const nodeLevel = this.getTreeLevelByScope(node.scope);
    const flatNode: CustomerAdminOrganizationUnitFlatNode = {
      expandable: !!node.children && node.children.length > 0,
      name: node.name,
      id: node.id,
      scope: node.scope,
      level: nodeLevel,
      data: node.data,
    };
    return flatNode;
  }

  getTreeLevelByScope(scope: CustomerAdminScope) {
    switch (scope) {
      case CustomerAdminScope.Organization:
        return 1;
      case CustomerAdminScope.OrganizationGeneral:
      case CustomerAdminScope.OrganizationUsers:
      case CustomerAdminScope.OrganizationCustomers:
        return 2;
      default:
        //todo switch all scopes
        return 3;
    }
  }

  getDeleteConfirmationDialog() {
    return this.dialog
      .open<
        DynamicConfirmationDialogComponent,
        DynamicConfirmationDialogData,
        DynamicConfirmationDialogResult
      >(DynamicConfirmationDialogComponent, {
        data: {
          labels: {
            title: 'Eintrag löschen',
            description: 'Wollen Sie wirklich den Eintrag löschen?',
            confirm: $localize`:@@Yes:Ja`,
            cancel: $localize`:@@No:Nein`,
          },
        },
      })
      .afterClosed();
  }
}
