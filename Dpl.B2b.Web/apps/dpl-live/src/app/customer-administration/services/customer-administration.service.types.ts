import { CustomerAdministrationService } from './customer-administration.service';

export interface CustomerAdminOrganizationLookup {
  id: number;
  name: string;
}

export interface CustomerAdminOrganizationUnitHierarchy {
  id: string;
  name: string;
  scope: CustomerAdminScope;
  children: CustomerAdminOrganizationUnitHierarchy[];
  data?: CustomerAdminOrganizationUnit;
}

export interface CustomerAdminOrganizationUnitFlatNode {
  id: string;
  name: string;
  scope: CustomerAdminScope;
  expandable: boolean;
  level: number;
  data?: CustomerAdminOrganizationUnit;
}

export interface CustomerAdminOrganizationUnit {
  id?: number;
  idString?: string;
  name: string;
  scope: CustomerAdminScope;
  parent: number;
  parentIdString?: string;
  organization?: number;
}

export enum CustomerAdminScope {
  Organization = 'Organization',
  OrganizationUsers = 'OrganizationUsers',
  OrganizationGeneral = 'OrganizationGeneral',
  OrganizationGroups = "OrganizationGroups",
  OrganizationGroupMembership = "OrganizationGroupMembership",
  OrganizationCustomerAdd = 'OrganizationCustomerAdd',
  OrganizationCustomers = 'OrganizationCustomers',
  Customer = 'Customer',
  CustomerGeneral = 'CustomerGeneral',
  CustomerUsers = 'CustomerUsers',
  CustomerGroups = 'CustomerGroups',
  CustomerGroupMembership = 'CustomerGroupMembership',
  CustomerNumberSequences = 'CustomerNumberSequences',
  CustomerPostingAccounts = 'CustomerPostingAccounts',
  CustomerDivisionAdd = 'CustomerDivisionAdd',
  CustomerDocumentSettings = 'DocumentSettings',
  Division = 'Division',
  DivisionGeneral = 'DivisionGeneral',
  DivisionGroups = 'DivisionGroups',
  DivisionGroupMembership = 'DivisionGroupMembership',
  DivisionDocumentSettings = 'DivisionDocumentSettings',
  DivisionLoadingLocationAdd = 'DivisionLoadingLocationAdd',
  LoadingLocation = 'LoadingLocation',
  LoadingLocationGeneral = 'LoadingLocationGeneral',
  LoadingLocationBusinessHours = 'LoadingLocationBusinessHours',
  LoadingLocationBusinessHoursExceptions = 'LoadingLocationBusinessHoursExceptions',
}

export interface CustomerAdminGroup {
  id: number;
  name: string;
}
