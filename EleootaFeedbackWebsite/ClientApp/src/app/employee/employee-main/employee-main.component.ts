import { DatePipe,getLocaleDateFormat } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { delay } from 'rxjs';
import { FOOD_PAYLOAD_CONSTANT } from '../food.config';
import { EmployeeService } from '../service/employee.service';

@Component({
  selector: 'app-employee-main',
  templateUrl: './employee-main.component.html',
  styleUrls: ['./employee-main.component.css'],
  providers: [ToastrService],
})
export class EmployeeMainComponent implements OnInit {
  employeeDetails: any = [];
  FoodDaysData: any = [];
  obj: any;
  today: any;
  // today:number=Date.now();
  dateee:any;
  now: any;

  food = FOOD_PAYLOAD_CONSTANT;

  bool = true;

  constructor(
    private employeeservice: EmployeeService,
    private datepipe: DatePipe,
    private toastr: ToastrService,
    private router:Router
  ) {}

  ngOnInit(): void {
    this.employeeDetails = JSON.parse(sessionStorage.getItem('sessionData')!);
    console.log(this.employeeDetails);

    this.employeeDetails.empId;
    this.getFoodBycompany(this.employeeDetails.companyId);

    let date = new Date();
    this.today = date.toISOString();

    date.setDate(date.getDate() + 7);
    console.log(date)

    this.dateee = date;

  }

  getFoodBycompany(id: any) {
    this.employeeservice.getFoodDays(id).subscribe({
      next: (result) => {
        this.FoodDaysData = result;
        console.log(this.FoodDaysData);
      },
      error: () => {
        console.log('Invalid api request');
      },
    });
  }

  foodSelectionChanged(event: any) {
    console.log(event);
    if (event?.target?.checked==true) {
      this.bool=false;
      this.obj = {
        compID: this.employeeDetails.companyId,
        empID: this.employeeDetails.empId,
        requestDate: this.today,
        foodOptType: event?.target?.name,
        weekDayid: parseInt(event?.target?.value),
      };
      this.food.push(this.obj);

      console.log(this.food)
    }

    if (event?.target?.checked==false){
      this.obj = {
        compID: this.employeeDetails.companyId,
        empID: this.employeeDetails.empId,
        requestDate: this.today,
        foodOptType: event?.target?.name,
        weekDayid: parseInt(event?.target?.value),
      };
      console.log(this.obj);
      this.food.pop(this.obj);
      var size = this.food.length;
      console.log(size);
      console.log(this.food)
      if(size>0){
        this.bool = false;
      }
      else{
        this.bool= true;
      }
    }
  }

  // reloadCurrentPage() {
  //   window.location.reload();
  //  }

  submitFoodRequest() {
    console.log(this.food);


    this.employeeservice.submitEmployeeFoodRequest(this.food).subscribe({
      next: (result) => {
        console.log('success');

        this.toastr.success('Food Request Sent');
        while (this.food.length > 0) {
          this.food.pop();
        }
        // this.reloadCurrentPage();
      //   setTimeout(function(){
      //     window.location.reload();
      //  }, 2000);
      let url = 'employee/submit-request';
      this.router.navigateByUrl(url);
      },

      error: () => {
        console.log('Invalid api request');
        this.toastr.error('Failed');
        while (this.food.length > 0) {
          this.food.pop();
        }
      },
    });
  }
}
