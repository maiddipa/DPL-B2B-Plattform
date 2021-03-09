import { Injectable } from '@angular/core';
import { Query, toBoolean } from '@datorama/akita';
import { SessionStore, SessionState } from './session.store';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class SessionQuery extends Query<SessionState> {
  isLoggedIn$ = this.select((state) => !!state.email);
  currentSession$ = this.select().pipe(map((i) => (i.email ? i : null)));

  constructor(protected store: SessionStore) {
    super(store);
  }
}
