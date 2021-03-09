import { Component, OnInit, Input } from '@angular/core';
import { LoadCarriersService } from 'apps/dpl-live/src/app/master-data/load-carriers/services/load-carriers.service';
import { MatSelect } from '@angular/material/select';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-load-carrier-type-selector',
  templateUrl: './load-carrier-type-selector.component.html',
  styleUrls: ['./load-carrier-type-selector.component.scss'],
})
export class LoadCarrierTypeSelectorComponent implements OnInit {
  @Input() typeOptions: number[];
  selectedTypeId$: Observable<number>;

  constructor(private loadCarrierService: LoadCarriersService) {}

  ngOnInit() {
    // onChange benutzen oder OnChange nur auf typeOptions changes?
    this.selectedTypeId$ = this.loadCarrierService
      .getActiveLoadCarrierType()
      .pipe(
        map((carrierType) => {
          return carrierType ? carrierType.id : null;
        })
      );
  }

  selectLoadCarrierType(event: { source: MatSelect; value: number }) {
    this.loadCarrierService.setActiveLoadCarrierType(event.value);
  }
}
