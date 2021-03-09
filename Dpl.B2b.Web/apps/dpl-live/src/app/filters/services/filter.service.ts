import { Injectable } from '@angular/core';
import {
  Filter,
  FilterPosition,
  FilterType,
  FilterConnector,
  FilterTemplate,
  FilterChoiceItem,
  FilterContext,
  FilterValueDateRange,
  FilterValueNumberRange,
} from './filter.service.types';
import { of, combineLatest, Observable } from 'rxjs';
import { map, tap, switchMap, first } from 'rxjs/operators';
import * as _ from 'lodash';
import { FilterQuery } from '../state/filter.query';
import { FilterStore } from '../state/filter.store';
import { applyTransaction, ID } from '@datorama/akita';

import * as moment from 'moment';

import {
  VoucherType,
  VoucherStatus,
  OrderStatus,
  OrderTransportType,
  TransportOfferingStatus,
  PartnerType,
  // AccountingRecordStatus,
} from '@app/api/dpl';
import { LocalizationService } from '../../core/services/localization.service';
import { LoadCarriersService } from '../../master-data/load-carriers/services/load-carriers.service';
import { VoucherReasonTypesService } from '../../master-data/voucher-reason-types/services/voucher-reason-types.service';

import { parseMomentToUtcDate } from '@app/shared';
import { CustomersService } from '../../customers/services/customers.service';

@Injectable({
  providedIn: 'root',
})
export class FilterService {
  // should be loaded from server based on customer/role --> observable for future use
  // owm service for possible filters?

  constructor(
    private query: FilterQuery,
    private store: FilterStore,
    private localizationService: LocalizationService,
    private loadCarrierService: LoadCarriersService,
    private voucherReasonService: VoucherReasonTypesService,
    private customersService: CustomersService
  ) {}

  getPossibleFilters(context: FilterContext): Observable<Filter[]> {
    return combineLatest([
      this.getChoicesData('AccountStatuses'),
      this.getChoicesData('OrderStatus'),
      this.getChoicesData('OrderTransportType'),
      this.getChoicesData('OrderLoadCarriers'),
      this.getChoicesData('OrderBaseLoadCarriers'),
      this.getChoicesData('Type'),
      this.getChoicesData('States'),
      this.getChoicesData('Reasons'),
      this.getChoicesData('LoadCarrierTypes'),
      this.getChoicesData('TransportOfferingStatus'),
      this.getChoicesData('RecipientType'),
      this.getChoicesData('YesNo'),
      this.customersService.getCustomDocumentLabel('CustomerReference'),
    ]).pipe(
      map((latest) => {
        const [
          accountStatuses,
          orderStatus,
          orderTransportType,
          orderLoadCarriers,
          orderBaseLoadCarriers,
          type,
          states,
          reasons,
          loadCarrierTypes,
          transportOfferingStatus,
          recipientTypes,
          yesNoChoices,
          customerReferenceTitle,
        ] = latest;
        switch (context) {
          case 'accountingRecords':
            return [
              {
                type: FilterType.Text,
                isChoice: true,
                isMulti: true,
                propertyName: 'Statuses',
                title: $localize`:Status|Label Status@@Status:Status`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 10,
                choices: accountStatuses,
              },
              {
                type: FilterType.Date,
                propertyName: 'AccountDate',
                title: $localize`:BookingDate|Label Buchungsdatum@@BookingDate:Buchungsdatum`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 20,
              },
              {
                type: FilterType.Number,
                propertyName: 'Credit',
                title: $localize`:InQuantity|Label Eingang@@InQuantity:Eingang`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 10,
              },
              {
                type: FilterType.Number,
                propertyName: 'Charge',
                title: $localize`:OutQuantity|Label Ausgang@@OutQuantity:Ausgang`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 20,
              },
              {
                type: FilterType.Text,
                propertyName: 'Description',
                title: $localize`:ExtDescription|Label Externe Beschreibung@@ExtDescription:Beschreibung`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 30,
              },
            ] as Filter[];
          case 'supplyOrders':
          case 'demandOrders':
            return [
              {
                type: FilterType.Text,
                isChoice: true,
                isMulti: true,
                propertyName: 'Status',
                title: $localize`:Status|Label Status@@Status:Status`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 10,
                choices: orderStatus,
              },
              // {
              //   type: FilterType.Text,
              //   isChoice: true,
              //   isMulti: true,
              //   propertyName: 'TransportType',
              //   title: $localize`:TransportType|Label Transporttyp@@TransportType:Transporttyp`,
              //   connector: FilterConnector.and,
              //   positions: FilterPosition.primary,
              //   order: 20,
              //   choices: orderTransportType
              // },
              {
                type: FilterType.Date,
                propertyName: 'AppointedDay',
                title: $localize`:ScheduledCollection|Label Geplante Abholung@@ScheduledCollection:Geplante Abholung`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 30,
              },
              {
                type: FilterType.Number,
                isChoice: true,
                isMulti: true,
                propertyName: 'LoadCarriers',
                title: $localize`:LoadCarrier|Label Ladungsträger@@LoadCarrier:Ladungsträger`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 40,
                choices: orderLoadCarriers,
              },
              {
                type: FilterType.Number,
                isChoice: true,
                isMulti: true,
                propertyName: 'BaseLoadCarriers',
                title: $localize`:VehiclePallet|Label Trägerpalette@@VehiclePallet:Trägerpalette`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 10,
                choices: orderBaseLoadCarriers,
              },
              {
                type: FilterType.Date,
                propertyName: 'CreatedAt',
                title: $localize`:TimeOfNotification|Label Meldezeitpunkt@@TimeOfNotification:Meldezeitpunkt`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 30,
              },
              {
                type: FilterType.Boolean,
                isChoice: true,
                propertyName: 'DplNote',
                title: $localize`:ModifyByDpl|Label Von Dpl bearbeitet@@ModifyByDpl:Bearbeitung durch DPL`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 40,
                choices: yesNoChoices,
              },
              // {
              // {
              //   type: FilterType.Number,
              //   propertyName: 'Quantity',
              //   title: $localize`:Quantity|Label Menge@@Quantity:Menge`,
              //   connector: FilterConnector.and,
              //   positions: FilterPosition.extended,
              //   order: 40
              // },
            ] as Filter[];
          case 'orders':
            return [
              {
                type: FilterType.Text,
                isChoice: true,
                isMulti: true,
                propertyName: 'Status',
                title: $localize`:Status|Label Status@@Status:Status`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 10,
                choices: orderStatus,
              },
              // {
              //   type: FilterType.Text,
              //   isChoice: true,
              //   isMulti: true,
              //   propertyName: 'TransportType',
              //   title: $localize`:TransportType|Label Transporttyp@@TransportType:Transporttyp`,
              //   connector: FilterConnector.and,
              //   positions: FilterPosition.primary,
              //   order: 20,
              //   choices: orderTransportType,
              // },
              {
                type: FilterType.Date,
                propertyName: 'FulfilmentDate',
                title: $localize`:ScheduledCollection|Label Geplante Abholung@@ScheduledCollection:Geplante Abholung`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 30,
              },
              {
                type: FilterType.Number,
                isChoice: true,
                isMulti: true,
                propertyName: 'LoadCarriers',
                title: $localize`:LoadCarrier|Label Ladungsträger@@LoadCarrier:Ladungsträger`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 40,
                choices: orderLoadCarriers,
              },
              {
                type: FilterType.Number,
                isChoice: true,
                isMulti: true,
                propertyName: 'BaseLoadCarriers',
                title: $localize`:VehiclePallet|Label Trägerpalette@@VehiclePallet:Trägerpalette`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 10,
                choices: orderBaseLoadCarriers,
              },
              {
                type: FilterType.Date,
                propertyName: 'CreatedAt',
                title: $localize`:TimeOfNotification|Label Meldezeitpunkt@@TimeOfNotification:Meldezeitpunkt`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 30,
              },
              {
                type: FilterType.Boolean,
                isChoice: true,
                propertyName: 'DplNote',
                title: $localize`:ModifyByDpl|Label Von Dpl bearbeitet@@ModifyByDpl:Bearbeitung durch DPL`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 40,
                choices: yesNoChoices,
              },
            ] as Filter[];
          case 'livePoolingOrders':
          case 'journal':
            return [
              // {
              //   type: FilterType.Text,
              //   isChoice: true,
              //   isMulti: true,
              //   propertyName: 'TransportType',
              //   title: $localize`:TransportType|Label Transporttyp@@TransportType:Transporttyp`,
              //   connector: FilterConnector.and,
              //   positions: FilterPosition.primary,
              //   order: 20,
              //   choices: orderTransportType
              // },
              {
                type: FilterType.Number,
                isChoice: true,
                isMulti: true,
                propertyName: 'LoadCarriers',
                title: $localize`:LoadCarrier|Label Ladungsträger@@LoadCarrier:Ladungsträger`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 40,
                choices: orderLoadCarriers,
              },
              {
                type: FilterType.Number,
                isChoice: true,
                isMulti: true,
                propertyName: 'BaseLoadCarriers',
                title: $localize`:VehiclePallet|Label Trägerpalette@@VehiclePallet:Trägerpalette`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 20,
                choices: orderBaseLoadCarriers,
              },
              {
                type: FilterType.Date,
                propertyName: 'FulfilmentDate',
                title: $localize`:ScheduledCollection|Label Geplante Abholung@@ScheduledCollection:Geplante Abholung`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 30,
              },
              {
                type: FilterType.Number,
                propertyName: 'Quantity',
                title: $localize`:Quantity|Label Menge@@Quantity:Menge`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 40,
              },
              {
                type: FilterType.Boolean,
                isChoice: true,
                propertyName: 'DplNote',
                title: $localize`:ModifyByDpl|Label Von Dpl bearbeitet@@ModifyByDpl:Bearbeitung durch DPL`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 40,
                choices: yesNoChoices,
              },
            ] as Filter[];
          case 'transports':
            return [
              {
                type: FilterType.Text,
                isChoice: true,
                isMulti: true,
                propertyName: 'TransportOfferingStatus',
                title: $localize`:Status|Label Status@@Status:Status`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 10,
                choices: transportOfferingStatus,
              },
              {
                type: FilterType.Date,
                propertyName: 'SupplyEarliestFulfillmentDate',
                title: $localize`:Filter Label für früheste Abholung@@FilterSupplyEarliestFulfillmentDateLabel:Früheste Abholung`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 30,
              },
              {
                type: FilterType.Date,
                propertyName: 'DemandLatestFulfillmentDate',
                title: $localize`:Filter Label für späteste Lieferung@@FilterDemandLatestFulfillmentDateLabel:Späteste Lieferung`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 30,
              },
              {
                type: FilterType.Text,
                propertyName: 'SupplyPostalCode',
                title: $localize`:Filter Label für Abholort PLZ@@FilterSupplyPostalCode:Abholort PLZ`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 60,
              },
              {
                type: FilterType.Text,
                propertyName: 'DemandPostalCode',
                title: $localize`:Filter Label für Lieferort PLZ@@FilterDemandPostalCode:Lieferort PLZ`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 60,
              },
              {
                type: FilterType.Number,
                isChoice: true,
                isMulti: true,
                propertyName: 'LoadCarrierTypes',
                title: $localize`:Article|Label Artikel@@Article:Artikel`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 60,
                choices: loadCarrierTypes,
              },
            ] as Filter[];
          case 'vouchers':
            return [
              // Text
              {
                type: FilterType.Text,
                propertyName: 'DocumentNumber',
                title: $localize`:Receipt|Label Beleg@@Receipt:Beleg` + ' #',
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 10,
              },
              // {
              //   type: FilterType.Text,
              //   propertyName: 'IssuedBy',
              //   title: $localize`:Issuer|Label Aussteller@@Issuer:Abteilung`,
              //   connector: FilterConnector.and,
              //   positions: FilterPosition.primary,
              //   order: 20
              // },
              {
                type: FilterType.Text,
                propertyName: 'shipper',
                title: $localize`:Shipper|Label Spediteur@@Shipper:Spediteur`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 30,
              },
              {
                type: FilterType.Text,
                propertyName: 'Supplier',
                title: $localize`:Supplier|Label Lieferant@@Supplier:Lieferant`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 32,
              },
              {
                type: FilterType.Text,
                propertyName: 'Recipient',
                title: 'Empfänger',
                // title: $localize`:Recipient|Label Empfänger@@Recipient`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 34,
              },
              {
                type: FilterType.Text,
                isChoice: true,
                isMulti: true,
                propertyName: 'States',
                title: $localize`:Status|Label Status@@Status:Status`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 40,
                choices: states,
              },
              {
                type: FilterType.Text,
                propertyName: 'CustomerReference',
                title: customerReferenceTitle,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 50,
              },
              {
                type: FilterType.Number,
                propertyName: 'Quantity',
                title: $localize`:Quantity|Label Menge@@Quantity:Menge`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 60,
              },
              {
                type: FilterType.Text,
                isChoice: true,
                isMulti: true,
                propertyName: 'RecipientType',
                title: $localize`:RecipientType|Label Empf@@RecipientType:Typ`,
                choices: recipientTypes,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 70,
              },
              {
                type: FilterType.Date,
                propertyName: 'ValidUntil',
                title: $localize`:Label Verfallsdatum|ValidUntil@@ValidUntil:Verfallsdatum`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 80,
              },
              //public int? QuantityFrom { get; set; } - new control
              //public int? QuantityTo { get; set; } - new control
              //Date
              {
                type: FilterType.Date,
                propertyName: 'IssueDate',
                title: $localize`:IssueDate|Label Ausstellungsdatum@@IssueDate:Ausstellungsdatum`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 20,
              },
              // this is single choice as there is an issue querying multi status for accounting records
              {
                type: FilterType.Text,
                isChoice: true,
                propertyName: 'Type',
                title: $localize`:RecordType|Label Belegtyp@@RecordType:Typ`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 30,
                choices: type,
              },
              // ToDo: read data from store
              {
                type: FilterType.Number,
                isChoice: true,
                isMulti: true,
                propertyName: 'Reasons',
                title: $localize`:VoucherReasonType|Label Nicht Tauschgrund@@VoucherReasonType:Nicht Tauschgrund`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 50,
                choices: reasons,
              },
              {
                type: FilterType.Number,
                isChoice: true,
                isMulti: true,
                propertyName: 'LoadCarrierTypes',
                title: $localize`:Article|Label Artikel@@Article:Artikel`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 60,
                choices: loadCarrierTypes,
              },
              {
                type: FilterType.Boolean,
                isChoice: true,
                propertyName: 'DplNote',
                title: $localize`:ModifyByDpl|Label Von Dpl bearbeitet@@ModifyByDpl:Bearbeitung durch DPL`,
                connector: FilterConnector.and,
                positions: FilterPosition.extended,
                order: 70,
                choices: yesNoChoices,
              },
            ] as Filter[];
          // toDo CustomerAdminUser Filters
          case 'customerAdminUsers':
            return [] as Filter[
              // upn
              // email
              // lastName
              // id
              // personId
            ];
          case 'customerAdminGroups':
            return [] as Filter[];
          case 'customerAdminGroupMembership':
            return [] as Filter[];
          default:
            return [
              {
                type: FilterType.Text,
                propertyName: 'DocumentNumber',
                title: $localize`:Default|Label Standard@@Default:Standard`,
                connector: FilterConnector.and,
                positions: FilterPosition.primary,
                order: 10,
              },
            ] as Filter[];
        }
      })
    );
  }

  getChoicesData(propertyName: string): Observable<FilterChoiceItem[]> {
    // context specific service
    switch (propertyName) {
      case 'Type':
        return of(this.getFilterOptionsFromEnum(VoucherType, 'VoucherType'));
      case 'States':
        const voucherStatuses = this.getFilterOptionsFromEnum(
          VoucherStatus,
          'VoucherStatus'
        );

        return of(
          voucherStatuses.filter((x) => x.value !== VoucherStatus.Submitted)
        );
      // case 'AccountStatuses':
      //   const statuses = this.getFilterOptionsFromEnum(
      //     AccountingRecordStatus,
      //     'AccountingRecordStatus'
      //   );
      //   return of(
      //     statuses.filter((x) => x.value !== AccountingRecordStatus.Pending)
      //   );
      case 'OrderStatus':
        // remove Matched from Order Status Filter
        // remove PartiallyMatched from Order Status Filter
        // remove Confirmed from Order Status Filter (clientside replacement with pending)
        return of(
          this.getFilterOptionsFromEnum(OrderStatus, 'OrderStatus').filter(
            (x) =>
              x.value !== OrderStatus.Matched &&
              x.value !== OrderStatus.Confirmed &&
              x.value !== OrderStatus.PartiallyMatched &&
              x.value !== OrderStatus.Expired
          )
        );
      case 'OrderTransportType':
        return of(
          this.getFilterOptionsFromEnum(
            OrderTransportType,
            'OrderTransportType'
          )
        );
      case 'Reasons':
        const voucherReasons$ = this.voucherReasonService
          .getVoucherReasonTypes()
          .pipe(
            map((reasons) => {
              return reasons.map((reason) => {
                const filterItem: FilterChoiceItem = {
                  value: reason.id.toString(),
                  title: this.localizationService.getTranslation(
                    'VoucherReasonTypes',
                    reason.id.toString()
                  ),
                };
                return filterItem;
              });
            })
          );
        return voucherReasons$;
      case 'LoadCarrierTypes':
        // read Types from MasterData Store and map to FilterChoiceItems
        const loadCarrierTypes$ = this.loadCarrierService
          .getLoadCarrierTypes()
          .pipe(
            map((types) => {
              return types.map((type) => {
                const filterItem: FilterChoiceItem = {
                  value: type.id.toString(),
                  title: this.localizationService.getTranslation(
                    'LoadCarrierTypes',
                    type.id.toString()
                  ),
                };
                return filterItem;
              });
            })
          );
        return loadCarrierTypes$;
      case 'OrderLoadCarriers':
      case 'OrderBaseLoadCarriers':
        // read Types from MasterData Store and map to FilterChoiceItems
        const loadCarriers$ = this.loadCarrierService.getLoadCarriers().pipe(
          map((carriers) => {
            return carriers.map((carrier) => {
              const filterItem: FilterChoiceItem = {
                value: carrier.id.toString(),
                title:
                  this.localizationService.getTranslation(
                    'LoadCarrierTypes',
                    carrier.type.id.toString()
                  ) +
                  ' ' +
                  this.localizationService.getTranslation(
                    'LoadCarrierQualities',
                    carrier.quality.id.toString()
                  ),
              };
              return filterItem;
            });
          })
        );
        return loadCarriers$;
      case 'TransportOfferingStatus':
        return of(
          this.getFilterOptionsFromEnum(
            TransportOfferingStatus,
            'TransportOfferingStatus'
          )
        );
      case 'RecipientType':
        return of(
          this.getFilterOptionsFromEnum(
            PartnerType,
            'PartnerType'
            // hack - only supplier and shipper possible for dor filter
          ).filter(
            (x) =>
              x.value === PartnerType.Shipper ||
              x.value === PartnerType.Supplier
          )
        );
      case 'YesNo':
        const items: FilterChoiceItem[] = [
          {
            title: 'Ja',
            value: 'true',
            order: 1,
          },
          {
            title: 'Nein',
            value: 'false',
            order: 1,
          },
        ];
        return of(items);
      default:
        return of([]);
    }
  }

  fillFilterStore(context: FilterContext, reset: boolean = false) {
    return this.getPossibleFilters(context).pipe(
      first(),
      tap((filters) => {
        applyTransaction(() => {
          if (
            reset ||
            context !== this.query.getValue().context ||
            !this.query.getHasCache()
          ) {
            this.store.set(filters);
            this.store.setActive(
              filters
                .filter(
                  (x) =>
                    x.positions === FilterPosition.primary ||
                    (x.value && x.value.length > 0)
                )
                .map((f) => f.propertyName)
            );
            this.store.update({ context });
          }
        });
      }),
      map(() => true)
    );
  }

  private getFilters(context: FilterContext, type: 'applied' | 'extended') {
    return this.fillFilterStore(context).pipe(
      switchMap(() => {
        return type === 'applied'
          ? this.query.appliedVoucherFilters$
          : this.query.extendedVoucherFilters$;
      })
    );
  }

  getAppliedFilters(context: FilterContext) {
    return this.getFilters(context, 'applied');
  }

  getExtendedFilters(context: FilterContext) {
    return this.getFilters(context, 'extended');
  }

  getActiveFilter<TFilter extends {}>(context: FilterContext) {
    const filters$ = this.getAppliedFilters(context);

    return filters$.pipe(
      map((filters) => {
        const filterObj: Partial<TFilter> = filters
          .filter((i) => i.value && i.value.length > 0)
          .reduce(
            (prev, current) => {
              prev[current.propertyName] = this.getFilterValue(current);
              return prev;
            },

            {} as Partial<TFilter>
          );
        return filterObj;
      })
    );
  }

  private getFilterValue(filter: Filter) {
    if (!filter.value || filter.value.length === 0) {
      throw new Error('Filter must have value');
    }

    switch (filter.type) {
      case FilterType.Date: {
        const fromMoment = moment(filter.value[0]).startOf('day');
        const toMoment =
          filter.value.length === 2 && filter.value[1]
            ? moment(filter.value[1]).startOf('day')
            : undefined;

        // if no second date was provided use same day as from + 1 for to date
        // fromMoment.clone().add('day', 1);

        const tzOffset = fromMoment.utcOffset();

        return <FilterValueDateRange>{
          from: parseMomentToUtcDate(fromMoment),
          to: toMoment ? parseMomentToUtcDate(toMoment) : undefined,
        };
      }
      case FilterType.Number: {
        if (filter.isChoice) {
          break;
        }
        const from = filter.value[0] ? parseInt(filter.value[0]) : undefined;
        const to =
          filter.value.length === 2 && filter.value[1]
            ? parseInt(filter.value[1])
            : undefined;

        return <FilterValueNumberRange>{
          from,
          to,
        };
      }
      default:
        break;
    }

    if (filter.isMulti) {
      return filter.value.map((i) => this.getFilterValueByType(filter.type, i));
    }

    return this.getFilterValueByType(filter.type, filter.value[0]);
  }

  private getFilterValueByType(type: FilterType, value: string) {
    switch (type) {
      case FilterType.Boolean:
        return value === 'true';
      case FilterType.Number:
        return parseInt(value);
      case FilterType.Text:
        return value;
      default:
        throw new Error(`Not supported filter type: ${type}`);
    }
  }

  // update ui pending value
  voucherFilterChange(id: ID, value: string[]) {
    // this.voucherFilterStore.upsert(id, { value });
    // set store ui property 'changed' and pendingValue
    this.store.ui.upsert(id, {
      changed: true,
      pendingValue: value,
    });
  }

  applyFilter(context: FilterContext) {
    this.query.changedVoucherFilters$.pipe(first()).subscribe((data) => {
      applyTransaction(() => {
        for (const changedFilter of data) {
          // copy pending value
          this.store.update(changedFilter.id, {
            value: changedFilter.pendingValue,
          });
        }
        this.query.shouldActiveVoucherFilterIds$
          .pipe(
            first(),
            map((ids) => {
              this.store.setActive(ids);
              return ids;
            }),
            switchMap(() => this.query.selectAll())
          )
          .subscribe((filters) => {
            // reset ui store value for each entity
            for (const filter of filters) {
              //context
              this.store.ui.update(filter.propertyName, {
                changed: false,
                pendingValue: null,
              });
            }
          });
        // set global "templateChanged" ui property
        this.store.updateTemplateChanged(true, context);
      });
    });
  }

  resetFilter(template: FilterTemplate, context: FilterContext) {
    this.fillFilterStore(context, true)
      .pipe(
        tap(() => {
          this.store.updateTemplateChanged(false, context);

          if (template) {
            applyTransaction(() => {
              for (const templateFilter of template.filters) {
                this.store.update(templateFilter.propName, {
                  value: templateFilter.value,
                });
              }
              this.query
                .selectAll()
                .pipe(first())
                .subscribe((allFilters) => {
                  this.store.setActive(
                    allFilters
                      .filter(
                        (x) =>
                          x.positions === FilterPosition.primary ||
                          (x.value && x.value.length > 0)
                      )
                      .map((f) => f.propertyName)
                  );
                });
            });
          }
        }),
        first()
        //subscribe hack - without subscribe subscription is cold
      )
      .subscribe();
  }

  isFilterChanged(id) {
    return this.query.getIsFilterChanged(id);
  }

  hasChangedFilters() {
    return this.query.hasChangedFilters$;
  }

  hasAvailableExtendedFilters() {
    return this.query.hasAvailableExtendedFilters$;
  }

  hasTemplateChanges() {
    return this.query.hasTemplateChanges$;
  }
  getFilterOptionsFromEnum(optionsEnum, translationKey) {
    const options: FilterChoiceItem[] = [];
    for (const key of Object.keys(optionsEnum)) {
      options.push({
        value: key.toString(),
        title: this.localizationService.getTranslation(
          translationKey,
          key.toString()
        ),
      });
    }
    return options;
  }
}
