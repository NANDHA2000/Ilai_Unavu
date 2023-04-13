import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeMainComponent } from './employee-main/employee-main.component';
import { SubmitRequestComponent } from './submit-request/submit-request.component';
import { TimeExpiredComponent } from './time-expired/time-expired.component';

const routes: Routes = [
  {path:'',component:EmployeeMainComponent},
  {path:'employee-main',component:EmployeeMainComponent},
  {path:'submit-request',component:SubmitRequestComponent},
  {path:'time-expired',component:TimeExpiredComponent}

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeeRoutingModule { }
