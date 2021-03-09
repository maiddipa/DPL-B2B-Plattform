import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-availabilities-container',
  templateUrl: './availabilities-container.component.html',
  styleUrls: ['./availabilities-container.component.scss'],
})
export class AvailabilitiesContainerComponent implements OnInit {
  @Input() loads: number;
  @Input() form: FormGroup;

  constructor() {}

  ngOnInit() {}
}
