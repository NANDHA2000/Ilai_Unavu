import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CompanyRoutingModule } from './company-routing.module';
import { CompanyMainComponent } from './company-main/company-main.component';
import { ManageEmployeesComponent } from './manage-employees/manage-employees.component';
import { CompanyFoodComponent } from './company-food/company-food.component';
import { EmployeeReportComponent } from './employee-report/employee-report.component';
import { CompanyNavComponent } from './company-nav/company-nav.component';
import { CompanySideComponent } from './company-side/company-side.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, NgForm } from '@angular/forms';
import { HttpParams } from '@angular/common/http';
import { NgxPaginationModule } from 'ngx-pagination';




@NgModule({
  declarations: [
    CompanyMainComponent,
    ManageEmployeesComponent,
    CompanyFoodComponent,
    EmployeeReportComponent,
    CompanyNavComponent,
    CompanySideComponent,
    
  ],
  imports: [
    CommonModule,
    CompanyRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    NgxPaginationModule,
    

  ]
})
export class CompanyModule { }
