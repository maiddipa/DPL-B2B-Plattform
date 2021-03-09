import { IUserSettings } from '../../user/state/user.model';
// import { AccountingRecordStatus } from '@app/api/dpl';

export enum FilterType {
  ChoiceSingle = 'ChoiceSingle',
  Date = 'Date',
  Text = 'Text',
  Number = 'Number',
  Boolean = 'Boolean',
}

export enum FilterPosition {
  primary = 'primary',
  extended = 'extended',
}

export enum FilterConnector {
  and = 'and',
}

export interface FilterChoiceItem {
  title: string;
  value: string;
  order?: number;
}

export interface Filter {
  propertyName: string;
  value?: string[];
  title: string;
  type: FilterType;
  isMulti: boolean;
  isChoice: boolean;
  choices?: FilterChoiceItem[];
  connector?: FilterConnector;
  // display values
  positions: FilterPosition;
  order: number;
  contexts: FilterContext[];
}

export interface FilterTemplate {
  id: string;
  title: string;
  filters: FilterTemplateItem[];
}

export interface FilterTemplateItem {
  propName: string;
  value: string[];
}

export type FilterContext = keyof IUserSettings['filterTemplates'];

export interface FilterTemplateDialogData {
  context: FilterContext;
  templateId: string;
  templateTitle: string;
}

export interface FilterValueRange<TData> {
  from: TData | undefined;
  to: TData | undefined;
}

export interface FilterValueNumberRange extends FilterValueRange<number> {}

export interface FilterValueDateRange extends FilterValueRange<Date> {}
