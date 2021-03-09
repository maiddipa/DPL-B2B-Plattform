export interface IBasket<T, R> {
  id: number;
  items: Array<IBasketItem<T, R>>;
  response?: IBasketCheckoutResponse;
}

export interface IBasketItem<T, R> {
  id: number;
  title: string;
  subTitles: Array<string>;
  data?: T;
  response?: IBasketCheckoutResponseItem<R>;

  // right now api doesnts support case like Mo or FR
  // days: [Date]
}

export interface IBasketCheckoutResponse {
  status: 'ok' | 'partial' | 'failed';
}

export interface IBasketCheckoutResponseItem<R> {
  status: 'ok' | 'failed';
  warningMessages?: string[];
  errorMessages?: string[];
  data?: R;
}
