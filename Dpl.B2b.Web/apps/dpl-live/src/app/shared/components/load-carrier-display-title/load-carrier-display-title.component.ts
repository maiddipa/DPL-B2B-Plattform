import { Component, Input, OnInit } from '@angular/core';
import { LoadCarrierPickerContext } from '@app/shared';
import { ILoadCarrierQuality } from '../../../master-data/load-carriers/state/load-carrier-quality.model';
import { ILoadCarrierType } from '../../../master-data/load-carriers/state/load-carrier-type.model';
import { ILoadCarrier } from '../../../master-data/load-carriers/state/load-carrier.model';

@Component({
  selector: 'dpl-load-carrier-display-title',
  templateUrl: './load-carrier-display-title.component.html',
  styleUrls: ['./load-carrier-display-title.component.scss'],
})
export class LoadCarrierDisplayTitleComponent implements OnInit {
  @Input() context: LoadCarrierPickerContext;
  @Input() loadCarrier: ILoadCarrier<ILoadCarrierType, ILoadCarrierQuality>;

  constructor() {}

  ngOnInit(): void {}
}
