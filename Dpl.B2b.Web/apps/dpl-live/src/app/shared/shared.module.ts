import { CommonModule, DatePipe, DecimalPipe } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NgxDaterangepickerMd } from 'ngx-daterangepicker-material';
import { NgxSubFormModule } from 'ngx-sub-form';

import { ChatModule } from '../chat/chat.module';
import { CoreModule } from '../core/core.module';
import { CustomersModule } from '../customers/customers.module';
import { PartnersModule } from '../partners/partners.module';
import {
  AddressCityFormComponent,
  AddressFormComponent,
  BasketListComponent,
  BusinessHoursComponent,
  ChatButtonComponent,
  ConfirmActionDialogComponent,
  CountryPickerComponent,
  DigitalCodeLookupComponent,
  DriverInfoFormComponent,
  DynamicConfirmationDialogComponent,
  ExpressCodeComponent,
  GoogleMapsPlacesLookupComponent,
  GoogleMapsPlacesSelectorDialogComponent,
  IssuerHeaderFormComponent,
  LicensePlateFormComponent,
  LoadCarrierFormComponent,
  LoadCarrierInOutFormComponent,
  LoadCarrierPickerComponent,
  LoadCarriersFormComponent,
  LoadingSpinnerComponent,
  PartnerDirectoryPickerComponent,
  PartnerLookupComponent,
  PostingAccountPickerComponent,
  PricingComponent,
  PrintLanguagePickerComponent,
  PrintSettingsFormComponent,
  RequiredHintComponent,
  ShipperFormComponent,
  TermsComponent,
  UserRole,
} from './components';
import { GridTextTooltipComponent } from './components/grid-text-tooltip/grid-text-tooltip.component';
import { AccountBalanceInfoComponent } from './components/account-balance-info/account-balance-info.component';
import { CancelButtonComponent } from './components/cancel-button/cancel-button.component';
import { CancelVoucherButtonComponent } from './components/cancel-voucher-button/cancel-voucher-button.component';
import { ColumnEmployeeNoteComponent } from './components/column-employee-note/column-employee-note.component';
import { CustomerSelectionComponent } from './components/customer-selection/customer-selection.component';
import { DateFromToDialogComponent } from './components/date-from-to-dialog/date-from-to-dialog.component';
import { DetailBookingComponent } from './components/detail-booking/detail-booking.component';
import { DetailLineComponent } from './components/detail-line/detail-line.component';
import { DetailLoadCarrierReceiptComponent } from './components/detail-load-carrier-receipt/detail-load-carrier-receipt.component';
import { DetailOrderLoadComponent } from './components/detail-order-load/detail-order-load.component';
import { DetailOrderComponent } from './components/detail-order/detail-order.component';
import { DetailProcessComponent } from './components/detail-process/detail-process.component';
import { InputFieldDialogComponent } from './components/input-field-dialog/input-field-dialog.component';
import { LoadCarrierDisplayTitleComponent } from './components/load-carrier-display-title/load-carrier-display-title.component';
import { LoadCarrierReceiptButtonComponent } from './components/load-carrier-receipt-button/load-carrier-receipt-button.component';
import { NoteInfoIconComponent } from './components/note-info-icon/note-info-icon.component';
import { NoteInfoComponent } from './components/note-info/note-info.component';
import { OnBehalfOfDialogComponent } from './components/on-behalf-of-dialog/on-behalf-of-dialog.component';
import { PartnerCreateDialogComponent } from './components/partner-create-dialog/partner-create-dialog.component';
import { PartnerCreateFormComponent } from './components/partner-create-form/partner-create-form.component';
import { SelectCustomerMessageComponent } from './components/select-customer-message/select-customer-message.component';
import { TriStateCheckboxComponent } from './components/tri-state-checkbox/tri-state-checkbox.component';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import {
  HasPermissionDirective,
  OnClickDisableUntilDirective,
  RouteHighlightDirective,
} from './directives';
import { AlphaOnlyDirective } from './directives/alpha-only.directive';
import { DigitOnlyDirective } from './directives/digit-only.directive';
import { ForbiddenEmptyFieldsCombinationValidatorDirective } from './directives/forbidden-empty-fields-combination.directive';
import { LowerCaseInputDirective } from './directives/lower-case-input.directive';
import { UpperCaseInputDirective } from './directives/upper-case-input.directive';
import {
  FooterComponent,
  HeaderComponent,
  HomeComponent,
  TopNavigationComponent,
  ImprintComponent,
  PrivacyComponent,
} from './layout';
import { DefaultContentComponent } from './layout/components/default-content/default-content.component';
import {
  AccountingRecordStatusPipe,
  AccountingRecordTypePipe,
  AddressPipe,
  CalendarWeekPipe,
  CountryPipe,
  DateExPipe,
  DayOfWeekPipe,
  DigitalCodePipe,
  DistancePipe,
  LanguagePipe,
  LoadCarrierPipe,
  LoadCarrierReceiptDepoPresetCategoryPipe,
  LoadCarrierReceiptTriggerPipe,
  LoadCarrierReceiptTypePipe,
  NumberExPipe,
  OrderLoadStatusPipe,
  OrderStatusPipe,
  OrderTypePipe,
  PartnerPipe,
  PartnerTypePipe,
  PermissionActionPipe,
  PermissionResourcePipe,
  SafePipe,
  TransportBidStatusPipe,
  TransportStatusPipe,
  UserRolePipe,
  BusinessHourExceptionPipe,
  VoucherReasonType,
  VoucherStatusPipe,
  VoucherTypePipe,

} from './pipes';
import { EmployeeNoteReasonPipe } from './pipes/employee-note-reason.pipe';
import { EmployeeNoteTypePipe } from './pipes/employee-note-type.pipe';
import { TransportTypePipe } from './pipes/transport-type.pipe';
import {
  DocumentsService,
  PrintService,
  TitleService,
  ValidationDataService,
} from './services';
import { CustomerCustomLabelComponent } from './components/customer-custom-label/customer-custom-label.component';
import { LoadingSpinnerOnPageComponent } from './components/loading-spinner-on-page/loading-spinner-on-page.component';
import { NonEmptyRuleDirective } from "@app/shared/directives/non-empty-rule.directive";
import { TrimFormFieldsDirective } from "@app/shared/directives/trim-form-fields-directive";




const pipes = [
  AccountingRecordStatusPipe,
  AccountingRecordTypePipe,
  AddressPipe,
  CalendarWeekPipe,
  CountryPipe,
  DateExPipe,
  DayOfWeekPipe,
  DigitalCodePipe,
  DistancePipe,
  LanguagePipe,
  LoadCarrierPipe,
  LoadCarrierReceiptDepoPresetCategoryPipe,
  LoadCarrierReceiptTypePipe,
  LoadCarrierReceiptTriggerPipe,
  NumberExPipe,
  OrderLoadStatusPipe,
  OrderStatusPipe,
  OrderTypePipe,
  SafePipe,
  TransportStatusPipe,
  TransportBidStatusPipe,
  VoucherReasonType,
  VoucherStatusPipe,
  VoucherTypePipe,
  UserRolePipe,
  BusinessHourExceptionPipe,
  TransportTypePipe,
  PartnerPipe,
  PartnerTypePipe,
  EmployeeNoteTypePipe,
  EmployeeNoteReasonPipe,
  PermissionActionPipe,
  PermissionResourcePipe,
];

const components = [
  AccountBalanceInfoComponent,
  AddressCityFormComponent,
  AddressFormComponent,
  BasketListComponent,
  BusinessHoursComponent,
  CancelButtonComponent,
  ChatButtonComponent,
  ConfirmActionDialogComponent,
  CountryPickerComponent,
  CustomerSelectionComponent,
  DetailBookingComponent,
  DetailLineComponent,
  DetailLoadCarrierReceiptComponent,
  DetailOrderComponent,
  DetailOrderLoadComponent,
  DetailProcessComponent,
  DigitalCodeLookupComponent,
  DriverInfoFormComponent,
  DynamicConfirmationDialogComponent,
  ExpressCodeComponent,
  GoogleMapsPlacesLookupComponent,
  GoogleMapsPlacesSelectorDialogComponent,
  IssuerHeaderFormComponent,
  LicensePlateFormComponent,
  LoadCarrierFormComponent,
  LoadCarrierInOutFormComponent,
  LoadCarrierPickerComponent,
  LoadCarrierReceiptButtonComponent,
  LoadCarriersFormComponent,
  LoadingSpinnerComponent,
  PartnerDirectoryPickerComponent,
  PartnerLookupComponent,
  PostingAccountPickerComponent,
  PricingComponent,
  PrintLanguagePickerComponent,
  PrintSettingsFormComponent,
  ShipperFormComponent,
  SelectCustomerMessageComponent,
  TermsComponent,
  PartnerCreateFormComponent,
  PartnerCreateDialogComponent,
  UserRole,
  RequiredHintComponent,
  OnBehalfOfDialogComponent,
  NoteInfoComponent,
  CancelVoucherButtonComponent,
  GridTextTooltipComponent,
  TriStateCheckboxComponent
];

const directives = [
  HasPermissionDirective,
  OnClickDisableUntilDirective,
  RouteHighlightDirective,
  LowerCaseInputDirective,
  UpperCaseInputDirective,
  AlphaOnlyDirective,
  DigitOnlyDirective,
  NonEmptyRuleDirective,
  TrimFormFieldsDirective
];

const services = [
  DocumentsService,
  PrintService,
  ValidationDataService,
  TitleService,
];

@NgModule({
  entryComponents: [
    ConfirmActionDialogComponent,
    DynamicConfirmationDialogComponent,
    GoogleMapsPlacesSelectorDialogComponent,
    PartnerCreateDialogComponent,
    PartnerDirectoryPickerComponent,
    PricingComponent,
    TermsComponent,
    OnBehalfOfDialogComponent,
    LoadingSpinnerComponent,
  ],
  declarations: [
    HeaderComponent,
    FooterComponent,
    TopNavigationComponent,
    UserProfileComponent,
    HomeComponent,
    ImprintComponent,
    PrivacyComponent,
    // custom pipes
    ...pipes,

    // custom components
    ...components,

    // custom directives
    ...directives,

    // TODO check if we actually need these components + directives
    InputFieldDialogComponent,
    DateFromToDialogComponent,

    ForbiddenEmptyFieldsCombinationValidatorDirective,

    AccountBalanceInfoComponent,
    ColumnEmployeeNoteComponent,
    NoteInfoIconComponent,
    LoadCarrierDisplayTitleComponent,
    DefaultContentComponent,
    CustomerCustomLabelComponent,
    LoadingSpinnerOnPageComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    NgxDaterangepickerMd.forRoot(),

    // custom modules
    CoreModule,
    ChatModule,
    CustomersModule,
    PartnersModule,

    NgxSubFormModule,
  ],
  exports: [
    CoreModule,
    NgxDaterangepickerMd,
    ChatModule,

    NgxSubFormModule,

    HeaderComponent,
    FooterComponent,

    // custom pipes
    ...pipes,

    // custom components
    ...components,

    // custome directives
    ...directives,

    // TODO check if we need these exports
    InputFieldDialogComponent,
    DateFromToDialogComponent,

    ForbiddenEmptyFieldsCombinationValidatorDirective,
    ColumnEmployeeNoteComponent,
    NoteInfoIconComponent,
    CustomerCustomLabelComponent,
    LoadingSpinnerOnPageComponent,
  ],
  providers: [
    DecimalPipe,
    DatePipe,

    // custom pipes
    ...pipes,

    // custom services
    ...services,
  ],
})
export class SharedModule {}
