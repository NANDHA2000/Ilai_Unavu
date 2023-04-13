import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmployeeRoutingModule } from './employee-routing.module';
import { EmployeeMainComponent } from './employee-main/employee-main.component';
import { EmployeeNavComponent } from './employee-nav/employee-nav.component';
import { SubmitRequestComponent } from './submit-request/submit-request.component';
import { TimeExpiredComponent } from './time-expired/time-expired.component';


@NgModule({
  declarations: [
    EmployeeMainComponent,
    EmployeeNavComponent,
    SubmitRequestComponent,
    TimeExpiredComponent
  ],
  imports: [
    CommonModule,
    EmployeeRoutingModule
  ]
})
export class EmployeeModule { }
