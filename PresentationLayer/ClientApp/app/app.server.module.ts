import { NgModule } from '@angular/core';
import { ServerModule } from '@angular/platform-server';
import { AppSharedModule } from './app.common.module';
import { AppComponent } from './app/app.component';

@NgModule({
    bootstrap: [ AppComponent ],
    imports: [
        ServerModule,
        AppSharedModule
    ]
})
export class AppModule {
}
