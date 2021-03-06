import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import { OrganizationsStore, OrganizationsState } from './organizations.store';

@Injectable({
  providedIn: 'root'
})
export class OrganizationsQuery extends QueryEntity<OrganizationsState> {

  constructor(protected store: OrganizationsStore) {
    super(store);
  }

}
