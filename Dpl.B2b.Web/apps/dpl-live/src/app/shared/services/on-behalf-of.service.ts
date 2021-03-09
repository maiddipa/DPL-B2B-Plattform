import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { OnBehalfOfDialogComponent } from '../components/on-behalf-of-dialog/on-behalf-of-dialog.component';
import { EmployeeNoteCreateRequest, EmployeeNoteType } from '@app/api/dpl';
import { CustomersService } from '../../customers/services/customers.service';
import { filter, switchMap, first } from 'rxjs/operators';
import { OnBehalfOfDialogInput } from './on-behalf-of.service.types';
import { UserService } from '../../user/services/user.service';
import { of, EMPTY } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class OnBehalfOfService {
  constructor(
    private dialog: MatDialog,
    private customersService: CustomersService,
    private userService: UserService
  ) {}

  openOnBehalfofDialog(noteType: EmployeeNoteType) {
    return this.userService.getIsDplEmployee().pipe(
      first(),
      switchMap((isEmployee) => {
        return isEmployee
          ? this.customersService.getActiveCustomer().pipe(
              filter((customer) => !!customer),
              first(),
              switchMap((customer) => {
                return this.dialog
                  .open<
                    OnBehalfOfDialogComponent,
                    OnBehalfOfDialogInput,
                    EmployeeNoteCreateRequest
                  >(OnBehalfOfDialogComponent, {
                    data: {
                      customerName: customer.name,
                      noteType: noteType,
                    },
                    disableClose:true,
                    autoFocus: false,
                  })
                  .afterClosed()
                  .pipe(
                    switchMap((data) => {
                      return data ? of(data) : EMPTY;
                    })
                  );
              })
            )
          : of(null as EmployeeNoteCreateRequest);
      })
    );
  }
}
