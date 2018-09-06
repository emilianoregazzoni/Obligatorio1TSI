import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppSharedModule } from './app.common.module';
import { AppComponent } from './app/app.component';

@NgModule({
    bootstrap: [ AppComponent ],
    imports: [
        BrowserModule,
        AppSharedModule
    ],
    providers: [
        { provide: 'BASE_URL', useFactory: getBaseUrl }
    ]
})
export class AppModule {
}

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}
