import { Injectable, OnInit } from '@angular/core';
import { Headers, Http, RequestOptions, Response } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { Employee } from './Employee'; 
import { ResourceAction, ResourceParams, Resource, ResourceMethod } from 'ngx-resource';
import { RequestMethod } from '@angular/http';
import { environment } from '../../environments/environment';
import { RestClient } from '../shared/rest-client';

import { Observable } from 'rxjs/Observable';

/**
 * Service for notify and subscribe to events.
 */
@Injectable()
@ResourceParams({
    url: environment.baseUrl,
    headers: { 'Access-Control-Allow-Origin': 'http://localhost:52391' },
})
export class EmployeeService extends RestClient {

    @ResourceAction({
        method: RequestMethod.Get,
        path: '/Employee',
        isArray: true,
        auth: true
    })
    getEmployees: ResourceMethod<any, Employee[]>;

    /* return Promise.resolve (Employee)*/ 
 
    public addEmployee() {
        console.log('add employee');
    }

    public editEmployee() {
        console.log('edit employee');
    }
    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
    }
}