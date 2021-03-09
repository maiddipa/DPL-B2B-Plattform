import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SessionService {
  private _id: number;
  constructor() {
    this._id = 0;
  }

  /**
   * This method is used to retrieve one Id
   *
   * @returns Returns one Id
   * @memberof SessionService
   */
  getNextId() {
    return ++this._id;
  }

  /**
   * This method is used to retrieve a batch of Ids instead of just one
   *
   * @param {number} count Number of Ids to return
   * @returns Returns an array of Ids
   * @memberof SessionService
   */
  getNextIds(count: number) {
    const currentId = this._id;
    this._id = this._id + count;
    return new Array(count).map((v, index) => currentId + index);
  }
}
