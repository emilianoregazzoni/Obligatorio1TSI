import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Http, Request } from '@angular/http';
import { Resource, ResourceParams, ResourceAction, ResourceMethod, ResourceActionBase } from 'ngx-resource';
import 'rxjs/Rx';

import { AuthService } from '../auth/auth.service';
import { Subscriber } from 'rxjs/Rx';

@Injectable()
export class RestClient extends Resource {

    constructor(protected http: Http) {
        super(http);
    }
    static pendingRequest: Map<any, any> = new Map<any, any>();

    $requestInterceptor(req: Request, methodOptions?: ResourceActionBase): Request {
        AuthService.getTokenFromApi(this.http).subscribe(token => {
            token = token.replace(/^"(.*)"$/, '$1');
            req.headers.append('Authorization', 'Bearer ' + token);
        })

        return super.$requestInterceptor(req, methodOptions);
    }

    $getHeaders(methodOptions?: any): any {
        let headers = super.$getHeaders();
        headers['Authorization'] = ['Bearer ' + AuthService.getAccessToken()];
        return headers;
    }


    reSendRequest(token: any) {
        let request = RestClient.pendingRequest;
        request.forEach((i, k) => {
            k.headers.set('Authorization', ['Bearer ' + token]);
            this.http.request(k).subscribe(res => {
                i.next(res.text() ? res.json() : {});
            },
                (error) => {
                    this.manageErrors(i, error);
                });
        });

        RestClient.pendingRequest = new Map<any, any>();
    }

    $responseInterceptor(observable: Observable<any>, req: Request, methodOptions?: ResourceActionBase): Observable<any> {
        return Observable.create((subscriber) => {
            observable.subscribe((res: Response) => {
                subscriber.next(res.text() ? res.json() : {});
            }, (error: Response) => {
                if (error.status === 0) {
                    let errorJson: any = { ErrorCode: "service_unavailable" };
                    subscriber.error(errorJson);
                } else if (error.status === 401) { //Unauthorized
                    RestClient.pendingRequest.set(req, subscriber);
                    AuthService.getTokenFromApi(this.http).subscribe(
                        (token) => {
                            token = token.replace(/^"(.*)"$/, '$1');
                            this.reSendRequest(token);
                        },
                        (error: any) => {
                            //alert(error.error);
                        }
                    );
                } else if (error.text()) {
                    this.manageErrors(subscriber, error);
                } else {
                    subscriber.error();
                }
            });
        });
    }


    manageErrors(subscriber: any, error: any) {
        if (this.isJson(error.text().toString())) {
            var errorJson: any = error.json();
            if (errorJson.value) {
                subscriber.error(errorJson.value);
            }
            subscriber.error(errorJson);
        } else {
            subscriber.error(error.text());
        }
    }

    private isJson(json: string): boolean {
        try {
            JSON.parse(json);
        } catch (e) {
            return false;
        }
        return true;
    }
}