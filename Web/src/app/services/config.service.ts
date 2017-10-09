import { Injectable } from '@angular/core';
import { Headers, Http, Response } from '@angular/http';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';

import { ConfigGroup } from '../models/config-group';

@Injectable()
export class ConfigService {

    config: ConfigGroup[];

    constructor(private http: Http) { }

    getGroupConfig(): Promise<ConfigGroup[]> {
        return this.http.get(`/api/config/get`).toPromise().then(response => {
            let result = response.json() as ConfigGroup[];
            return result;
        }).catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.log(error);
        //console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }
}
