import { Injectable, OnInit } from '@angular/core';
import { SessionStorage, SessionStorageService } from 'ngx-webstorage';
import { CookieService } from 'ngx-cookie';
import { Headers, Http, RequestOptions, Response } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../environments/environment';

interface ITokenRequest {
    username: string;
}
@Injectable()
export class AuthService implements OnInit {

    @SessionStorage()
    private static token: string;

    @SessionStorage()
    private static clientToken: string;

    public static isRefreshing: boolean = false;

    static instance: AuthService;

    constructor(private router: Router,
        private sessionStorageService: SessionStorageService,
        protected http: Http,
        private cookieService: CookieService
    ) {
        AuthService.instance = this;
    }

    static getAccessToken(): string {
        if (AuthService.token) {
            return AuthService.token;
        }
        if (AuthService.clientToken) {
            return AuthService.clientToken;
        }
        return '';
    }

    static getTokenFromApi(http: Http): any {

        let headers: Headers = new Headers();
        headers.append('content-type', 'application/x-www-form-urlencoded');
        return http.get(environment.oauthUrl + 'Token?' + 'userName=Pudre', { headers })
            .map(response => AuthService.clientToken = response.text())
    }

    static getClientToken(http: Http): Observable<any> {

        var observable: Observable<any> = new Observable(observer => {

            let headers: Headers = new Headers();
            headers.append('content-type', 'application/x-www-form-urlencoded');

            http.get(environment.oauthUrl + 'Token?' + 'userName=Pudre', { headers })
                .map((data: Response) => {
                    AuthService.clientToken = <string>data.json();
                    observer.next(AuthService.clientToken);
                },
                    (error: Response) => {
                        console.log(error.json());
                        observer.error(error.json());
                    });
        });

        return observable;
    }

    static refreshToken(http: Http): Observable<any> {
        AuthService.isRefreshing = true;
        return Observable.create((observer) => {
            var param = new URLSearchParams();
            param.append('grant_type', 'refresh_token');
            param.append('client_id', 'webJS');
            //param.append('client_secret', 'secret');
            param.append('refresh_token', AuthService.token);
            //param.append('scope', 'ipcsApi');
            let headers: Headers = new Headers();
            headers.append('content-type', 'application/x-www-form-urlencoded');
            http.get(environment.oauthUrl + 'Token?' + 'userName=Pudre', { headers }).map(
                (data: Response) => {
                    AuthService.isRefreshing = false;
                    AuthService.token = <string>data.json();
                    observer.next(<string>data.json());
                },
                (error: Response) => {
                    AuthService.isRefreshing = false;
                    if (error.status === 400) {
                    }
                    observer.error(error.json());
                }
            );
        });
    }

    static isTakeUserToken(): boolean {
        return AuthService.token !== null;
    }

    ngOnInit() { }
}