import { WeekDay } from '@angular/common';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { admin_url } from 'src/app/admin/configs/admin-url.config';
import { company_url } from '../company-url.config';
import { CompanyService } from '../service/company.service';

@Component({
  selector: 'app-employee-report',
  templateUrl: './employee-report.component.html',
  styleUrls: ['./employee-report.component.css'],
})
export class EmployeeReportComponent implements OnInit {
  constructor(private services: CompanyService, private http: HttpClient) {}

  foodCount: any = [];
  
  companyDetails: any = [];
  checkweek:boolean=false;
  

  

  ngOnInit(): void {
    this.companyDetails = JSON.parse(sessionStorage.getItem('sessionData')!);

    this.getFoodCount();
  }
  getFoodCount(){
    this.foodCount.data = [];

    this.services
      .GetData(this.companyDetails.companyName,this.checkweek)
      .subscribe((data: any) => {
        this.foodCount=data
          });
          

        console.log(this.foodCount)
      
  }

  weekChange(event:any){
    this.checkweek=event.target.checked
    console.log(this.checkweek)
    this.getFoodCount();
  }

}
