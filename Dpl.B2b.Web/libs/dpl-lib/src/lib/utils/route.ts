import { ActivatedRoute } from '@angular/router';
import { combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import * as _ from 'lodash';

export function getTypedParams<TRouteParams, TQueryParams = any>(
  activatedRoute: ActivatedRoute
) {
  return combineLatest(activatedRoute.params, activatedRoute.queryParams).pipe(
    map(([route, query]) => {
      return {
        route: route as TRouteParams,
        query: query as TQueryParams,
      };
    })
  );
}

type ParseParamFieldConfig = {
  type: 'string' | 'int';
  isArray?: boolean;
};

export type ParseParamConfig<
  TRouteParams extends {},
  TQueryParams extends {} = any
> = {
  route?: Partial<
    {
      [key in keyof TRouteParams]: ParseParamFieldConfig;
    }
  >;
  query?: Partial<
    {
      [key in keyof TQueryParams]: ParseParamFieldConfig;
    }
  >;
};

function parseParam<TParams extends {}>(
  params: TParams,
  config: Partial<
    {
      [key in keyof TParams]: ParseParamFieldConfig;
    }
  >
) {
  if (!config) {
    return params;
  }

  const parsedParams = _(params)
    .mapValues((value, key) => {
      if (config[key]) {
        const propConfig = config[key] as ParseParamFieldConfig;
        if (propConfig.isArray) {
          const arr = value
            ? Array.isArray(value)
              ? value
              : [value]
            : ((value as unknown) as any[]);
          if (!arr) {
            return arr;
          }
          return arr.map((arrValue) => parseParamValue(arrValue, propConfig));
        }

        return parseParamValue(value, propConfig);
      }
    })
    .value();

  return parsedParams as TParams;
}

function parseParamValue(value: any, config: ParseParamFieldConfig) {
  switch (config.type) {
    case 'int':
      return parseInt(value);
    default:
      return value;
  }
}

export function parseParams<
  TRouteParams extends {},
  TQueryParams extends {} = any
>(
  activatedRoute: ActivatedRoute,
  config?: ParseParamConfig<TRouteParams, TQueryParams>
) {
  const configSafe = config || {};
  return combineLatest(activatedRoute.params, activatedRoute.queryParams).pipe(
    map(([route, query]) => {
      return {
        route: parseParam(route as TRouteParams, configSafe.route),
        query: parseParam(query as TQueryParams, configSafe.query),
      };
    })
  );
}
