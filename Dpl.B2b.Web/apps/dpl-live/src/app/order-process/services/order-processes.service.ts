import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';
import {
  AccountingRecord,
  LoadCarrierReceipt,
  Order,
  OrderGroup,
  OrderLoad,
} from '@app/api/dpl';
import { DplApiService } from '@app/core';
import { AddressPipe } from '@app/shared';
import { applyTransaction, setLoading } from '@datorama/akita';
import * as _ from 'lodash';
import { tap } from 'rxjs/operators';

import { OrderProcess } from '../state/order-process.model';
import { normalizeOrderProcesses } from '../state/order-process.normalize';
import { OrderProcessesQuery } from '../state/order-processes.query';
import { OrderProcessesStore } from '../state/order-processes.store';
import { OrderGroupsSearchRequest } from './order-processes.service.types';

@Injectable({
  providedIn: 'root',
})
export class OrderProcessesService {
  constructor(
    private dpl: DplApiService,
    private store: OrderProcessesStore,
    private query: OrderProcessesQuery,
    private address: AddressPipe,
    private date: DatePipe
  ) {}

  resetActive() {
    //this.store.setActive([]);
  }

  load(request: OrderGroupsSearchRequest) {
    // HACK order-group change get by id to search based on order id, order-load-id, load-carrier-receipt-id
    this.dpl.orderGroups
      .search(request)
      .pipe(
        // this sets loading to true until observable completes
        setLoading(this.store),
        tap((orderGroups) => {
          const orderProcessesHierachial = _(orderGroups)
            .map((orderGroup) => {
              const orderProcessHierachial = this.mapToOrderProcess(
                'orderGroup',
                orderGroup,
                this.store.generateId('orderGroup', orderGroup.id)
              );
              return orderProcessHierachial;
            })
            .value();

          const { orderProcesses } = normalizeOrderProcesses(
            orderProcessesHierachial
          );

          const activeIds = orderProcessesHierachial.map((i) => i.id);

          applyTransaction(() => {
            this.store.upsertMany(orderProcesses);
            this.store.setActive(activeIds);
          });
        })
      )
      .subscribe();
  }

  getActiveOrderProcesses() {
    return this.query.activeOrderProcesses$;
  }

  private mapToOrderProcess(
    type: OrderProcess['type'],
    data: OrderProcess['data'],
    rootId: string
  ): OrderProcess {
    const getTypeSpecificData: () => Omit<
      OrderProcess,
      'id' | 'rootId' | 'type' | 'data'
    > = () => {
      switch (type) {
        case 'orderGroup': {
          const orderGroup = data as OrderGroup;
          const order = orderGroup.orders[0];
          const address =
            order.address ||
            (order.loads && order.loads.length > 0
              ? order.loads[0].address
              : null);
          return {
            level: 0,
            children: orderGroup.orders.map((o) =>
              this.mapToOrderProcess('order', o, rootId)
            ),
            date: order.createdAt,
            referenceNumber: null,
            description: this.address.transform(address, 'long'),
            showExtendedDetails: false,
          };
        }
        case 'order': {
          const order = data as Order;
          return {
            level: 1,
            children: order.loads.map((load) =>
              this.mapToOrderProcess('orderLoad', load, rootId)
            ),
            date: order.createdAt,
            referenceNumber: order.orderNumber,
            description:
              order.earliestFulfillmentDateTime ===
              order.latestFulfillmentDateTime
                ? `Durchführungsdatum: ${this.date.transform(
                    order.earliestFulfillmentDateTime
                  )}`
                : `Durchführungsdatum: ${this.date.transform(
                    order.earliestFulfillmentDateTime
                  )} - ${this.date.transform(order.latestFulfillmentDateTime)}`,
            showExtendedDetails: order.dplNotes.length > 0,
          };
        }
        case 'orderLoad': {
          const load = data as OrderLoad;
          const receipt = load.loadCarrierReceipt;
          return {
            level: 2,
            children: (receipt ? [receipt] : []).map((order) =>
              this.mapToOrderProcess('loadCarrierReceipt', order, rootId)
            ),
            date:
              load.actualFulfillmentDateTime || load.plannedFulfillmentDateTime,
            referenceNumber: load.digitalCode,
            description: null,
            showExtendedDetails: load.dplNotes.length > 0,
          };
        }
        case 'loadCarrierReceipt': {
          const receipt = data as LoadCarrierReceipt;
          return {
            level: 3,
            children: (receipt.accountingRecords || []).map((order) =>
              this.mapToOrderProcess('accountingRecord', order, rootId)
            ),
            date: receipt.issuedDate,
            referenceNumber: receipt.documentNumber,
            description: null,
            showExtendedDetails: receipt.dplNotes.length > 0,
          };
        }
        case 'accountingRecord': {
          const record = data as AccountingRecord;
          return {
            level: 4,
            children: [],
            date: record.date,
            referenceNumber: record.referenceNumber,
            description: record.extDescription,
            showExtendedDetails: !!record.dplNote,
          };
        }

        default:
          throw new Error(`Unsupported type: ${type}`);
      }
    };

    const typeSpecificData = getTypeSpecificData();

    const orderProcess: OrderProcess = {
      id: this.store.generateId(type, data.id),
      rootId,
      type,
      data,
      ...typeSpecificData,
    };

    return orderProcess;
  }
}
