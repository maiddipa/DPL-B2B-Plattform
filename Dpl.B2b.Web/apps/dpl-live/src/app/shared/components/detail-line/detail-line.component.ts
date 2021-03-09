import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'dpl-detail-line',
  templateUrl: './detail-line.component.html',
  styleUrls: ['./detail-line.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DetailLineComponent implements OnInit {
  constructor() {}

  ngOnInit(): void {}
}
