import {
  Component,
  OnInit,
  EventEmitter,
  Input,
  Output,
  Inject,
  ChangeDetectorRef,
} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NgxAutomaticRootFormComponent, Controls } from 'ngx-sub-form';
import { FormControl, Validators } from '@angular/forms';
import {
  EmployeeNoteCreateRequest,
  EmployeeNoteReason,
  EmployeeNoteType,
} from '@app/api/dpl';
import { Observable } from 'rxjs';
import { UserService } from '../../../user/services/user.service';
import { map, tap } from 'rxjs/operators';
import { OnBehalfOfDialogInput } from '@app/shared/services/on-behalf-of.service.types';

type ViewData = {
  userId: number;
};

@Component({
  templateUrl: './on-behalf-of-dialog.component.html',
  styleUrls: ['./on-behalf-of-dialog.component.scss'],
})
export class OnBehalfOfDialogComponent
  extends NgxAutomaticRootFormComponent<EmployeeNoteCreateRequest>
  implements OnInit {
  @Input() dataInput: Required<EmployeeNoteCreateRequest>;
  @Output() dataOutput = new EventEmitter<EmployeeNoteCreateRequest>();
  reason = EmployeeNoteReason;
  customerName: string;
  noteType: EmployeeNoteType;

  constructor(
    private readonly dialogRef: MatDialogRef<
      OnBehalfOfDialogComponent,
      EmployeeNoteCreateRequest
    >,
    @Inject(MAT_DIALOG_DATA) public data: OnBehalfOfDialogInput,
    cd: ChangeDetectorRef
  ) {
    super(cd);

    if (data) {
      this.customerName = data.customerName;
      this.noteType = data.noteType;
    }
  }

  ngOnInit() {
    super.ngOnInit();
    this.formGroup.controls.type.patchValue(this.noteType);
  }

  now() {
    this.formGroup.controls.contactedAt.patchValue(new Date());
  }

  confirm() {
    this.dialogRef.close(this.formGroup.value);
  }

  cancel() {
    this.dialogRef.close();
  }

  protected getFormControls(): Controls<EmployeeNoteCreateRequest> {
    return {
      reason: new FormControl(EmployeeNoteReason.Mail, Validators.required),
      contact: new FormControl(null, Validators.required),
      contactedAt: new FormControl(null, Validators.required),
      text: new FormControl(null, Validators.required),
      type: new FormControl(null, Validators.required),
    };
  }
}
