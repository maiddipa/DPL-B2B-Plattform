import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
  Output,
  EventEmitter,
} from '@angular/core';
import { combineLatest, forkJoin, Observable, of } from 'rxjs';
import { map, switchMap, tap } from 'rxjs/operators';
import {
  AddressAdministration,
  CustomerDivision,
  LoadingLocation,
  LoadingLocationAdministration,
  PostingAccount,
} from '../../../core/services/dpl-api-services';
import { DplApiService } from '../../../core/services/dpl-api.service';
type ViewData = {
  postingAccounts: PostingAccount[];
  postingAccountSelectBoxOptions: any;
  loadingLocationsSelectBoxOptions: any;
};
@Component({
  selector: 'dpl-customer-administration-division-form',
  templateUrl: './customer-administration-division-form.component.html',
  styleUrls: ['./customer-administration-division-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationDivisionFormComponent implements OnInit {
  @Input() customerId: number;
  @Input() divisionId?: number;
  @Input() formData: CustomerDivision;
  @Output() formSubmit = new EventEmitter<CustomerDivision>();

  viewData$: Observable<ViewData>;

  buttonOptions: any = {
    text: 'Speichern',
    type: 'success',
    useSubmitBehavior: true,
  };

  constructor(private dpl: DplApiService) {}

  ngOnInit(): void {
    const postingAccounts$ = this.dpl.postingAccountsAdministrationService.getByCustomerId(
      this.customerId
    );

    const loadingLocations$ = this.divisionId
      ? this.dpl.loadingLocationsAdministrationService.getByCustomerDivisionId(
          this.divisionId
        )
      : of([] as LoadingLocationAdministration[]);

    const locationAdresses$ = this.divisionId
      ? loadingLocations$.pipe(
          switchMap((locations) => {
            return locations.length > 0
              ? forkJoin(
                  locations.map((location) =>
                    this.dpl.addressesAdministrationService.getById(
                      location.addressId
                    )
                  )
                )
              : of([] as AddressAdministration[]);
          })
        )
      : of([] as AddressAdministration[]);

    this.viewData$ = combineLatest([
      postingAccounts$,
      loadingLocations$,
      locationAdresses$,
    ]).pipe(
      map(([postingAccounts, loadingLocations, addresses]) => {
        const postingAccountSelectBoxOptions = {
          dataSource: postingAccounts,
          displayExpr: 'displayName',
          valueExpr: 'id',
          searchEnabled: true,
        };
        const loadingLocationsSelectBoxOptions = {
          dataSource: loadingLocations,
          displayExpr: (loadingLocation: LoadingLocationAdministration) => {
            if (loadingLocation) {
              const address = addresses.find(
                (x) => x.id === loadingLocation.addressId
              );
              return address
                ? `${address.city}, ${address.postalCode}, ${address.street1}`
                : '';
            }
          },
          valueExpr: 'id',
          searchEnabled: true,
        };

        const viewData: ViewData = {
          postingAccounts,
          postingAccountSelectBoxOptions,
          loadingLocationsSelectBoxOptions,
        };
        return viewData;
      })
    );
  }

  onFormSubmit(e) {
    e.preventDefault();
    this.formSubmit.emit(this.formData);
  }
}
