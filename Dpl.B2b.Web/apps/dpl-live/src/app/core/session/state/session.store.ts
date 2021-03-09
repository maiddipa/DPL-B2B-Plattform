import { Injectable } from '@angular/core';
import { Store, StoreConfig } from '@datorama/akita';

export interface SessionState {
  token: string;
  name: string;
  email: string;
}

export function createInitialSessionState(): SessionState {
  return <SessionState>{
    token: null,
    name: null,
    email: null,
  };
}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'session' })
export class SessionStore extends Store<SessionState> {
  constructor() {
    // if (window.self !== window.top){
    //   return;
    // }
    super(createInitialSessionState());
  }

  login(session: SessionState) {
    this.update(session);
  }

  logout() {
    this.reset();
  }
}
