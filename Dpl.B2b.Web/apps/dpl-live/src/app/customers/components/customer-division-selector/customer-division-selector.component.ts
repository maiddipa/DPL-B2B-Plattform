import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { combineLatest, Observable } from 'rxjs';
import {
  distinctUntilKeyChanged,
  filter,
  first,
  map,
  switchMap,
  tap,
} from 'rxjs/operators';
import { SessionState } from 'apps/dpl-live/src/app/core/session/state/session.store';
import { UserService } from 'apps/dpl-live/src/app/user/services/user.service';

import { CustomerDivisionsService } from '../../services/customer-divisions.service';
import { ICustomerDivision } from '../../state/customer-division.model';
import { IUser } from 'apps/dpl-live/src/app/user/state/user.model';
import {
  DynamicConfirmationDialogComponent,
  DynamicConfirmationDialogData,
  DynamicConfirmationDialogResult,
} from '@app/shared/components/dynamic-confirmation-dialog/dynamic-confirmation-dialog.component';
import { MatDialog } from '@angular/material/dialog';

interface IViewData {
  user: IUser<number, number>;
  customerDivisions: ICustomerDivision[];
}

@Component({
  selector: 'customer-division-selector',
  templateUrl: './customer-division-selector.component.html',
  styleUrls: ['./customer-division-selector.component.scss'],
})
export class CustomerDivisionSelectorComponent implements OnInit {
  viewData$: Observable<IViewData>;
  form: FormControl;
  previouslySelectedDivisionId: number;

  constructor(
    private fb: FormBuilder,
    private customerDivisionService: CustomerDivisionsService,
    private user: UserService,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.form = this.fb.control(null);

    const user$ = this.user.getCurrentUser();
    const divisions$ = this.customerDivisionService.getDivisions();
    const activeDivision$ = divisions$.pipe(
      first(),
      switchMap((divisions) => {
        return this.customerDivisionService.getActiveDivision().pipe(
          map((division) => {
            if (!division) {
              if (!divisions || divisions.length === 0) {
                return null;
              }
              return divisions[0];
            }
            return division;
          }),
          filter((division) => !!division),
          distinctUntilKeyChanged('id'),
          tap((division) => {
            if (!division.id) {
              return;
            }
            this.form.patchValue(division.id);
          })
        );
      }),
      tap((division) => {
        this.previouslySelectedDivisionId = division.id;
      })
    );

    this.viewData$ = combineLatest(user$, divisions$, activeDivision$).pipe(
      map(([user, customerDivisions]) => {
        return <IViewData>{
          user,
          customerDivisions,
        };
      })
    );
  }

  onSelectionChange() {
    const id = this.form.value as number;

    if (this.previouslySelectedDivisionId === id) {
      return;
    }

    return this.dialog
      .open<
        DynamicConfirmationDialogComponent,
        DynamicConfirmationDialogData,
        DynamicConfirmationDialogResult
      >(DynamicConfirmationDialogComponent, {
        data: {
          labels: {
            title: $localize`:Titel für Dialog Abteilung wechseln@@ChangeDivisionDialogTitle:Abteilung wechseln`,
            description: $localize`:Frage für Dialog Abteilungswechsel bestätigen@@ChangeDivisionDialogDescription:Möchten Sie die Abteilung wechseln?`,
            confirm: $localize`:@@Yes:Ja`,
            cancel: $localize`:@@No:Nein`,
          },
        },
      })
      .afterClosed()
      .pipe(
        tap((result) => {
          if (!result?.confirmed) {
            this.form.patchValue(this.previouslySelectedDivisionId);
            return;
          }

          this.customerDivisionService.setActive(id);
          window.location.reload();
        })
      )
      .subscribe();
  }
}
