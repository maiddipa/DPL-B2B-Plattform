import { EmployeeNoteType } from '@app/api/dpl';

export interface OnBehalfOfExecutionResult {
  reason: OnBehalfOfExecutionReason;
  person: string;
  date: Date;
  comment: string;
}

export enum OnBehalfOfExecutionReason {
  Mail = 'Mail',
  Phone = 'Phone',
  Fax = 'Fax',
}

export interface OnBehalfOfDialogInput {
  customerName: string;
  noteType: EmployeeNoteType;
}
