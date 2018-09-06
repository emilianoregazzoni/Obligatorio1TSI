import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { EmployeeService } from './employee/employee.service';
import { EmployeeListComponent } from './employee/list/employee-list.component';
import { ManageEmployeeComponent } from './employee/manage/manage-employee.component';
import { AppComponent } from './app/app.component';
import { AuthService } from './auth/auth.service';

@NgModule({
    declarations: [
        AppComponent,
        EmployeeListComponent,
        ManageEmployeeComponent
    ],
    providers: [
        EmployeeService,
        AuthService
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'employee', pathMatch: 'full' },
            { path: 'employee', component: EmployeeListComponent },
            { path: 'employee/manage', component: ManageEmployeeComponent },
            { path: '**', redirectTo: 'employee' }
        ])
    ]
})
export class AppSharedModule {
}
