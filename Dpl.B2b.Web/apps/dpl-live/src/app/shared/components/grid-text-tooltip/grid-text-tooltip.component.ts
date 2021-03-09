import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
} from '@angular/core';

@Component({
  selector: 'dpl-grid-text-tooltip',
  templateUrl: './grid-text-tooltip.component.html',
  styleUrls: ['./grid-text-tooltip.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GridTextTooltipComponent implements OnInit {
  @Input() values: string[];
  @Input() displayCount? = 3;

  displayValue: string;
  popOverVisible = false;
  randomId = Math.floor(Math.random() * 100 + 1);

  constructor() {}

  ngOnInit(): void {
    if (this.values?.length <= this.displayCount) {
      this.displayValue = this.values.join(', ');
    } else {
      this.displayValue = this.values.slice(0, this.displayCount).join(', ');
    }
  }
}
