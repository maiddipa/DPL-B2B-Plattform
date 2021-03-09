import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomerAdministrationRoutingModule } from './customer-administration-routing.module';
import { CustomerAdministrationComponent } from './customer-administration.component';
import { CustomerAdministrationHeaderComponent } from './components/customer-administration-header/customer-administration-header.component';
import { CustomerAdministrationContentComponent } from './components/customer-administration-content/customer-administration-content.component';
import { CustomerAdministrationNavigationComponent } from './components/customer-administration-navigation/customer-administration-navigation.component';
import { SharedModule } from '@app/shared';
import { CustomerAdministrationNodeComponent } from './components/customer-administration-node/customer-administration-node.component';
import { CustomerAdministrationContentOverviewComponent } from './components/customer-administration-content-overview/customer-administration-content-overview.component';
import { CustomerAdministrationContentNoContentComponent } from './components/customer-administration-content-no-content/customer-administration-content-no-content.component';
import { FiltersModule } from '../filters/filters.module';
import { CustomerAdministrationContentUsersGridComponent } from './components/customer-administration-content-users-grid/customer-administration-content-users-grid.component';
import { CustomerAdministrationContentGroupsGridComponent } from './components/customer-administration-content-groups-grid/customer-administration-content-groups-grid.component';
import { CustomerAdministrationMembershipGridComponent } from './components/customer-administration-membership-grid/customer-administration-membership-grid.component';
import { CustomerAdministrationUnitSearchComponent } from './components/customer-administration-unit-search/customer-administration-unit-search.component';
import { CustomerAdministrationDivisionDocumentSettingsComponent } from './components/customer-administration-division-document-settings/customer-administration-division-document-settings.component';
import { CustomerAdministrationNumberSequencesComponent } from './components/customer-administration-number-sequences/customer-administration-number-sequences.component';
import { CustomerAdministrationDivisionGeneralComponent } from './components/customer-administration-division-general/customer-administration-division-general.component';
import { CustomerAdministrationCustomerGeneralComponent } from './components/customer-administration-customer-general/customer-administration-customer-general.component';
import { CustomerAdministrationOrganizationGeneralComponent } from './components/customer-administration-organization-general/customer-administration-organization-general.component';
import { CustomerAdministrationPostingAccountsComponent } from './components/customer-administration-posting-accounts/customer-administration-posting-accounts.component';
import { CustomerAdministrationLoadingLocationGeneralComponent } from './components/customer-administration-loading-location-general/customer-administration-loading-location-general.component';
import { CustomerAdministrationLoadingLocationBusinessHoursComponent } from './components/customer-administration-loading-location-business-hours/customer-administration-loading-location-business-hours.component';
import { CustomerAdministrationLoadingLocationBusinessHoursExceptionsComponent } from './components/customer-administration-loading-location-business-hours-exceptions/customer-administration-loading-location-business-hours-exceptions.component';
import { CustomerAdministrationDocumentSettingsComponent } from './components/customer-administration-document-settings/customer-administration-document-settings.component';
import { CustomerAdministrationOrganizationAddComponent } from './components/customer-administration-organization-add/customer-administration-organization-add.component';
import { CustomerAdministrationOrganizationFormComponent } from './components/customer-administration-organization-form/customer-administration-organization-form.component';
import { CustomerAdministrationCustomerAddComponent } from './components/customer-administration-customer-add/customer-administration-customer-add.component';
import { CustomerAdministrationCustomerFormComponent } from './components/customer-administration-customer-form/customer-administration-customer-form.component';
import { CustomerAdministrationDivisionAddComponent } from './components/customer-administration-division-add/customer-administration-division-add.component';
import { CustomerAdministrationDivisionFormComponent } from './components/customer-administration-division-form/customer-administration-division-form.component';
import { CustomerAdministrationLoadingLocationAddComponent } from './components/customer-administration-loading-location-add/customer-administration-loading-location-add.component';
import { CustomerAdministrationLoadingLocationFormComponent } from './components/customer-administration-loading-location-form/customer-administration-loading-location-form.component';
import { CustomerAdministrationBreadcrumbComponent } from './components/customer-administration-breadcrumb/customer-administration-breadcrumb.component';
@NgModule({
  declarations: [
    CustomerAdministrationComponent,
    CustomerAdministrationHeaderComponent,
    CustomerAdministrationContentComponent,
    CustomerAdministrationNavigationComponent,
    CustomerAdministrationNodeComponent,
    CustomerAdministrationContentOverviewComponent,
    CustomerAdministrationContentNoContentComponent,
    CustomerAdministrationContentUsersGridComponent,
    CustomerAdministrationContentGroupsGridComponent,
    CustomerAdministrationMembershipGridComponent,
    CustomerAdministrationUnitSearchComponent,
    CustomerAdministrationDivisionDocumentSettingsComponent,
    CustomerAdministrationNumberSequencesComponent,
    CustomerAdministrationDivisionGeneralComponent,
    CustomerAdministrationCustomerGeneralComponent,
    CustomerAdministrationOrganizationGeneralComponent,
    CustomerAdministrationPostingAccountsComponent,
    CustomerAdministrationLoadingLocationGeneralComponent,
    CustomerAdministrationLoadingLocationBusinessHoursComponent,
    CustomerAdministrationLoadingLocationBusinessHoursExceptionsComponent,
    CustomerAdministrationOrganizationFormComponent,
    CustomerAdministrationOrganizationAddComponent,
    CustomerAdministrationCustomerAddComponent,
    CustomerAdministrationCustomerFormComponent,
    CustomerAdministrationDivisionAddComponent,
    CustomerAdministrationDivisionFormComponent,
    CustomerAdministrationLoadingLocationAddComponent,
    CustomerAdministrationLoadingLocationFormComponent,
    CustomerAdministrationDocumentSettingsComponent,
    CustomerAdministrationBreadcrumbComponent,
  ],
  imports: [
    CommonModule,
    CustomerAdministrationRoutingModule,
    SharedModule,
    FiltersModule,
  ],
})
export class CustomerAdministrationModule {}
