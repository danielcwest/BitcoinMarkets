import { Injectable } from '@angular/core';
import { Headers, Http, Response } from '@angular/http';

import { ContextService } from './context.service';
import { Order } from '../models/order';
import { AppContext } from '../models/app-context';

import * as Collections from 'typescript-collections';

@Injectable()
export class OrderService {

  context: AppContext;

  constructor(private http: Http, private contextService: ContextService) {
	  this.contextService.context$.subscribe(context => {
		  this.context = context;
	  });
  }

  getOrdersForPair(pairId: number): Promise<Order[]> {
	  return this.http.get(`/api/order/get?pairId=${pairId}`).toPromise().then(response => {
		  let result = response.json() as Order[];
		  return result;
	  }).catch(this.handleError);
  }

  private handleError(error: any): Promise<any> {
	  console.log(error);
	  //console.error('An error occurred', error);
	  return Promise.reject(error.message || error);
  }
}
