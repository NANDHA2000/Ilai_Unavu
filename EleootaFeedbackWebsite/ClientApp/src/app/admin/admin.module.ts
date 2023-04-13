import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminMainComponent } from './admin-main/admin-main.component';
import { ManageFoodmenuComponent } from './manage-foodmenu/manage-foodmenu.component';
import { ManageCompaniesComponent } from './manage-companies/manage-companies.component';
import { AdminNavComponent } from './admin-nav/admin-nav.component';
import { AdminSidenavComponent } from './admin-sidenav/admin-sidenav.component';
import { ManageWeekdaysComponent } from './manage-weekdays/manage-weekdays.component';
import { FormsModule, NgForm, ReactiveFormsModule } from '@angular/forms';
import { HttpParams } from '@angular/common/http';


@NgModule({
  declarations: [
    AdminMainComponent,
    ManageFoodmenuComponent,
    ManageCompaniesComponent,
    AdminNavComponent,
    AdminSidenavComponent,
    ManageWeekdaysComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    FormsModule,
    ReactiveFormsModule,
  ]
})
export class AdminModule { }
