import { Observable, from, SubscribableOrPromise } from 'rxjs';

export function delayCreation<O>(createObs: () => SubscribableOrPromise<O>) {
  return new Observable<O>((observer) => {
    return from(createObs()).subscribe(observer);
  });
}
