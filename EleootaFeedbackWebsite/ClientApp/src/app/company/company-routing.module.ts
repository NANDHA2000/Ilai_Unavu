import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CompanyFoodComponent } from './company-food/company-food.component';
import { CompanyMainComponent } from './company-main/company-main.component';
import { EmployeeReportComponent } from './employee-report/employee-report.component';
import { ManageEmployeesComponent } from './manage-employees/manage-employees.component';


const routes: Routes = [
  {path:'',component:CompanyMainComponent,children:[
    {path:'',component:ManageEmployeesComponent},
    {path:'manage-employees',component:ManageEmployeesComponent},
  {path:'company-food',component:CompanyFoodComponent},
  {path:'employee-report',component:EmployeeReportComponent},


  ]}
  // {path:'company-main',component:CompanyMainComponent},

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CompanyRoutingModule { }
