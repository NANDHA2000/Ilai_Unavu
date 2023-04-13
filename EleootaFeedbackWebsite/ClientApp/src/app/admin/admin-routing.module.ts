import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminMainComponent } from './admin-main/admin-main.component';
import { ManageCompaniesComponent } from './manage-companies/manage-companies.component';
import { ManageFoodmenuComponent } from './manage-foodmenu/manage-foodmenu.component';
import { ManageWeekdaysComponent } from './manage-weekdays/manage-weekdays.component';

const routes: Routes = [
  {
  path: "", component: AdminMainComponent, children: [
    {path:'',component:ManageFoodmenuComponent},
    {path:'manage-foodmenu',component:ManageFoodmenuComponent},
    {path:'manage-foodtypes',component:ManageWeekdaysComponent},
    {path:'manage-companies',component:ManageCompaniesComponent},

  ]
}
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
