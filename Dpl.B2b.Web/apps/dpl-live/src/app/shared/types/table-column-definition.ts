export class TableColumnDefinition<TSort> {
  key: string;
  displayName: string;
  index: number;
  type: TableColumnType;
  alignment: TextAlignment;
  sortByOption?: TSort;

  constructor(
    key: string,
    displayName: string,
    index: number,
    type: TableColumnType,
    alignment: TextAlignment,
    sortByOption?: TSort
  ) {
    this.key = key;
    this.displayName = displayName;
    this.index = index;
    this.type = type;
    this.alignment = alignment;
    this.sortByOption = sortByOption;
  }
}

export enum TextAlignment {
  left,
  right,
  center,
}

export enum TableColumnType {
  date,
  icon,
  text,
  array,
  chat,
}
