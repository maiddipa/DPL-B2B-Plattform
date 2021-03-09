import { Injectable } from '@angular/core';
import {
  guid,
  applyTransaction,
  cacheable,
  persistState,
} from '@datorama/akita';
import { FilterTemplate, FilterContext } from './filter.service.types';
import {
  tap,
  switchMap,
  startWith,
  publishReplay,
  refCount,
  map,
  first,
} from 'rxjs/operators';
import { of, Observable } from 'rxjs';

import { FilterTemplateQuery } from '../state/filter-template.query';
import { FilterTemplateStore } from '../state/filter-template.store';
import { FilterService } from './filter.service';
import { FilterStore } from '../state/filter.store';
import * as _ from 'lodash';
import { DplApiService } from '@app/core';
import { UserService } from '../../user/services/user.service';

@Injectable({
  providedIn: 'root',
})
export class FilterTemplateService {
  constructor(
    private voucherFilterTemplateStore: FilterTemplateStore,
    private voucherFilterTemplateQuery: FilterTemplateQuery,
    private voucherFilterService: FilterService,
    private filterStore: FilterStore,
    private dpl: DplApiService,
    private userService: UserService
  ) {}

  // move to filter template service
  getDummyTemplateFilters(): Observable<FilterTemplate[]> {
    // only return dummy
    const dummy: FilterTemplate[] = [];
    return of(dummy);
  }

  getTemplateFilters(context: FilterContext) {
    return this.userService.getCurrentUser().pipe(
      first(),
      map((user) => {
        if (
          !user.settings ||
          !user.settings.filterTemplates ||
          !user.settings.filterTemplates[context] ||
          _.isEmpty(user.settings.filterTemplates[context])
        ) {
          return {
            activeTemplateId: undefined,
            templates: [],
          };
        }

        return user.settings.filterTemplates[context];
      }),
      tap((contextTemplate) => {
        this.voucherFilterTemplateStore.set(contextTemplate.templates);
        // set active template
        if (contextTemplate.activeTemplateId) {
          // removed - activate last active context filter
          // this.setActiveTemplateFilter(
          //   contextTemplate.activeTemplateId,
          //   context
          // );
          this.voucherFilterTemplateStore.setActive(null);
        }
      }),
      switchMap(() => this.voucherFilterTemplateQuery.allTemplates$)
    );
  }

  setActiveTemplateFilter(templateId: string, context: FilterContext) {
    // hole template
    this.voucherFilterTemplateQuery
      .selectEntity(templateId)
      .pipe(
        first(),
        tap((template) => {
          this.voucherFilterTemplateStore.setActive(
            template ? template.id : undefined
          );
          this.voucherFilterService.resetFilter(template, context);
        }),
        switchMap((active) =>
          // persist templates
          this.voucherFilterTemplateQuery.selectAll().pipe(
            switchMap((filters) => {
              const updateObject = {
                templates: filters,
                activeTemplateId: active ? active.id : undefined,
              };

              return this.userService.persistFilterTemplates(
                context,
                updateObject
              );
            })
          )
        ),
        first()
      )
      .subscribe();
  }

  getActiveTemplateFilter() {
    return this.voucherFilterTemplateQuery.activeTemplate$;
  }

  // templateId only filled on override
  saveTemplateFilter(
    templateId: string,
    templateTitle: string,
    context: FilterContext
  ) {
    return this.voucherFilterTemplateQuery.selectEntity(templateId).pipe(
      first(),
      switchMap((template) => {
        // TODO: Ask Kevin if a template will ever be saved for a not currently already active context
        // previous the below line grabbed the context from the store state, but this didnt seem o make sense
        return this.voucherFilterService.getAppliedFilters(context).pipe(
          first(),
          map((filters) => {
            const changedTemplate: FilterTemplate = {
              id: template ? template.id : guid(),
              title: templateTitle,
              filters: filters
                .filter((x) => x.value && x.value.length > 0)
                .map((x) => {
                  return { propName: x.propertyName, value: x.value };
                }),
            };

            this.voucherFilterTemplateStore.upsert(
              changedTemplate.id,
              changedTemplate
            );
            this.setActiveTemplateFilter(changedTemplate.id, context);
          })
        );
      })
    );
  }

  removeTemplateFilter(templateId: string, context: FilterContext) {
    this.voucherFilterTemplateStore.remove(templateId);
    this.setActiveTemplateFilter(undefined, context);
  }

  clearFilterTemplate(context: FilterContext) {
    this.voucherFilterTemplateQuery
      .selectActiveId()
      .pipe(
        first(),
        tap((id) => {
          if (id) {
            this.voucherFilterTemplateStore.removeActive(id);
          }
        })
      )
      .subscribe(() => this.setActiveTemplateFilter(undefined, context));
  }
}
