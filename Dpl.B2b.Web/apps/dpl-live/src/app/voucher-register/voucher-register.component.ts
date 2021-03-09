import { Component, OnInit } from '@angular/core';
import { FilterContext } from '../filters/services/filter.service.types';
import { LoadCarriersService } from '../master-data/load-carriers/services/load-carriers.service';
import { ILoadCarrierType } from '../master-data/load-carriers/state/load-carrier-type.model';
import { Observable, combineLatest } from 'rxjs';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { tap, switchMap, map } from 'rxjs/operators';
import { UserService } from '../user/services/user.service';
import { UserRole } from '@app/api/dpl';
import { ILoadCarrier } from '../master-data/load-carriers/state/load-carrier.model';
import { ILoadCarrierQuality } from '../master-data/load-carriers/state/load-carrier-quality.model';

type ViewData = {
  activeType: ILoadCarrierType;
  userRole: UserRole;
  highlightId?: number;
  isDplEmployee: boolean;
  loadCarriers: ILoadCarrier<ILoadCarrierType, ILoadCarrierQuality>[];
};

@Component({
  selector: 'app-voucher-register',
  templateUrl: './voucher-register.component.html',
  styleUrls: ['./voucher-register.component.scss'],
})
export class VoucherRegisterComponent implements OnInit {
  filterContext: FilterContext = 'vouchers';
  activeType$: Observable<ILoadCarrierType>;
  userRole$: Observable<UserRole>;
  role = UserRole;
  viewData$: Observable<ViewData>;

  constructor(
    private readonly loadCarriersService: LoadCarriersService,
    private readonly route: ActivatedRoute,
    private userService: UserService
  ) {}

  // tslint:disable-next-line:typedef
  ngOnInit() {
    const userRole$ = this.userService
      .getCurrentUser()
      .pipe(map((user) => user.role));

    const activeType$ = this.route.paramMap.pipe(
      tap((params: ParamMap) => {
        const carrierTypeId = params.get('carrierType');
        if (carrierTypeId) {
          this.loadCarriersService.setActiveLoadCarrierType(
            // tslint:disable-next-line:radix
            parseInt(carrierTypeId)
          );
        }
      }),
      switchMap(() => this.loadCarriersService.getActiveLoadCarrierType())
    );

    const loadCarriers$ = this.loadCarriersService.getLoadCarriers();

    const isDplEmployee$ = this.userService.getIsDplEmployee();

    this.viewData$ = combineLatest([
      userRole$,
      activeType$,
      isDplEmployee$,
      loadCarriers$,
    ]).pipe(
      map(([userRole, activeType, isDplEmployee, loadCarriers]) => {
        const viewData: ViewData = {
          userRole,
          activeType,
          isDplEmployee,
          loadCarriers,
        };
        console.log('DOR ViewData', viewData);
        return viewData;
      })
    );
  }
}
