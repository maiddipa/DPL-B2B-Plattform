import { FlatTreeControl } from '@angular/cdk/tree';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import {
  MatTreeFlatDataSource,
  MatTreeFlattener,
} from '@angular/material/tree';
import {
  LoadCarrierReceipt,
  LoadCarrierReceiptType,
  Order,
  OrderLoad,
} from '@app/api/dpl';
import { DocumentsService } from '@app/shared';

import { ChatLinkData } from '../../../chat/components/chat-link.directive';
import { OrderGroupsSearchRequest } from '../../services/order-processes.service.types';
import { OrderProcess } from '../../state/order-process.model';

enum Column {
  Summary = 'Summary',
  Type = 'Type',
  Date = 'Date',
  Description = 'Description',
  ReferenceNumber = 'ReferenceNumber',
  DigitalCode = 'DigitalCode',
  LoadCarrier = 'LoadCarrier',
  Status = 'Status',
  Actions = 'Actions',
  Detail = 'Detail',
}

@Component({
  selector: 'dpl-order-processes-list',
  templateUrl: './order-processes-list.component.html',
  styleUrls: ['./order-processes-list.component.scss'],
})
export class OrderProcessesListComponent implements OnChanges {
  receiptType = LoadCarrierReceiptType;
  column = Column;

  displayedColumns: Column[];

  treeControl: FlatTreeControl<OrderProcess>;
  treeFlattener: MatTreeFlattener<OrderProcess, OrderProcess>;
  dataSource: MatTreeFlatDataSource<OrderProcess, OrderProcess>;

  constructor(private document: DocumentsService) {}

  @Input() showAsTable: boolean;
  @Input() orderProcesses: OrderProcess[];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['showAsTable']) {
      this.setColumns(this.showAsTable ? 'table' : 'summary');
      const levelAdjustment = this.showAsTable ? 0 : 2;

      this.treeControl = new FlatTreeControl<OrderProcess>(
        (node) => node.level,
        (node) => node.children.length > 0
      );

      this.treeFlattener = new MatTreeFlattener(
        (detail, level) => ({
          ...detail,
          level: detail.level - levelAdjustment,
        }),
        (node) => node.level,
        (node) => node.children.length > 0,
        (detail: OrderProcess) => detail.children
      );

      this.dataSource = new MatTreeFlatDataSource(
        this.treeControl,
        this.treeFlattener
      );
    }

    if (changes['orderProcesses']) {
      this.dataSource.data = this.orderProcesses;
      this.treeControl.expandAll();
    }
  }

  private setColumns(viewType: 'summary' | 'table') {
    switch (viewType) {
      case 'summary':
        this.displayedColumns = [Column.Summary];
        break;
      case 'table':
        this.displayedColumns = [
          Column.Type,
          Column.Date,
          Column.ReferenceNumber,
          Column.LoadCarrier,
          Column.Description,
          Column.Status,
          Column.Actions,
        ];
        break;
    }
  }

  getIndent(detail: OrderProcess, hasButton: boolean = false) {
    return detail.level * 35 + (hasButton ? 0 : 40);
  }

  showDetailRow = (index: number, detail: OrderProcess) => {
    //hack - always show detail
    // return detail.showExtendedDetails && this.treeControl.isExpanded(detail);
    return this.treeControl.isExpanded(detail);
  };

  openDocument(detail: OrderProcess) {
    let documentId;
    switch (detail.type) {
      case 'loadCarrierReceipt':
        documentId = (detail.data as LoadCarrierReceipt).documentId;
        break;
      default:
        throw new Error(`Type is not supported: ${detail.type}`);
    }

    this.document.print(documentId, false).subscribe();
  }

  getHighlightParameterName(
    detail: OrderProcess
  ): keyof OrderGroupsSearchRequest {
    switch (detail.type) {
      case 'orderGroup':
        return 'id';
      case 'order':
        return 'orderId';
      case 'orderLoad':
        return 'orderLoadId';
      case 'loadCarrierReceipt':
        return 'loadCarrierReceiptId';
      default:
        return undefined;
    }
  }

  getChatLinkData(detail: OrderProcess): ChatLinkData {
    switch (detail.type) {
      case 'order': {
        const order = detail.data as Order;
        return ['order', order];
      }
      case 'orderLoad': {
        const load = detail.data as OrderLoad;
        return ['orderLoad', load];
      }
      default:
        throw new Error(
          `Chat data creation not supported for type: ${detail.type}`
        );
    }
  }
}
