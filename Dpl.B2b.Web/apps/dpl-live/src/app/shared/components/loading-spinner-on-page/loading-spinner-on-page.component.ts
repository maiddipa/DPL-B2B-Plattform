import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'dpl-loading-spinner-on-page',
  templateUrl: './loading-spinner-on-page.component.html',
  styleUrls: ['./loading-spinner-on-page.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoadingSpinnerOnPageComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
