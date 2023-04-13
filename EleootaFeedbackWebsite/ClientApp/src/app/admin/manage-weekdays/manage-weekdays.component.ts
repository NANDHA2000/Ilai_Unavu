import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
// import { timeLog } from 'console';
import { ToastrService } from 'ngx-toastr';
// import { runInThisContext } from 'vm';
import { FOOD_PAYLOAD_CONSTANT } from '../configs/food.config';
import { ManageCompanyApiService } from '../manage-companies/manage-company-api.service';
import { AdminService } from '../service/admin.service';

@Component({
  selector: 'app-manage-weekdays',
  templateUrl: './manage-weekdays.component.html',
  styleUrls: ['./manage-weekdays.component.css'],
  providers: [ToastrService],
})
export class ManageWeekdaysComponent implements OnInit {
  item: any;

  FoodTypeByWeekDaysOrder: FormGroup | any;
  companyIdINfo: any;
  weekDaysOrder: any;
  companyList: any;
  foodweekdays: boolean = false;
  foodtype: boolean = false;
  companyname: boolean = false;


  comapanyId: any;
  companyStatus = 1;
  food = FOOD_PAYLOAD_CONSTANT;
  payload: any = {};
  FoodTypesOrder: any[] = [];
  FoodWeekDaysOrder: any[] = [];

  foodOrder: any;


  // weekDayselectForm: any;

  constructor(
    private adminservice: AdminService,
    private toastr: ToastrService,
    private managecompapi: ManageCompanyApiService,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.getCompanyList();
    this.initialForm(null);
  }

  get companyFoodTypesOrder() {
    return this.FoodTypeByWeekDaysOrder.get('companyFoodTypesOrder')
  }
  get companyFoodWeekDaysOrder() {
    return this.FoodTypeByWeekDaysOrder.get('companyFoodWeekDaysOrder')
  }

  get companyId() {
    return this.FoodTypeByWeekDaysOrder.get('companyId')
  }

  initialForm(data: any) {
    this.FoodTypeByWeekDaysOrder = this.fb.group({
      companyId: [this.companyIdINfo != undefined ? this.companyIdINfo : "", [Validators.required]],
      companyFoodTypesOrder: this.fb.group({
        'breakfast': [this.foodOrder?.Breakfast ? this.foodOrder.Breakfast : false, [Validators.required]],
        'lunch': [this.foodOrder?.Lunch ? this.foodOrder.Lunch : false, [Validators.required]],
        'potluck': [this.foodOrder?.Potluck ? this.foodOrder.Potluck : false, [Validators.required]],
        'dinner': [this.foodOrder?.Dinner ? this.foodOrder.Dinner : false, [Validators.required]],
      }),
      companyFoodWeekDaysOrder: this.fb.group({
        'monday': [this.weekDaysOrder?.Monday ? this.weekDaysOrder.Monday : false, [Validators.required]],
        'tuesday': [this.weekDaysOrder?.Tuesday ? this.weekDaysOrder.Tuesday : false, [Validators.required]],
        'wednesday': [this.weekDaysOrder?.Wednesday ? this.weekDaysOrder.Wednesday : false, [Validators.required]],
        'thursday': [this.weekDaysOrder?.Thursday ? this.weekDaysOrder.Thursday : false, [Validators.required]],
        'friday': [this.weekDaysOrder?.Friday ? this.weekDaysOrder.Friday : false, [Validators.required]],
        'saturday': [this.weekDaysOrder?.Saturday ? this.weekDaysOrder.Saturday : false, [Validators.required]],
        'sunday': [this.weekDaysOrder?.Sunday ? this.weekDaysOrder.Sunday : false, [Validators.required]],
      }),
    });
  }


  getCompanyList() {
    this.companyStatus = 1;
    this.managecompapi.getCompanyList(this.companyStatus).subscribe({
      next: (result) => {
        this.companyList = result;
      },
      error: () => {
        console.log('Error');
      },
    });
  }

  getCompanyName(data: any) {
    this.companyname = true;
    let companyId = Number(this.FoodTypeByWeekDaysOrder.get('companyId').value);
    this.adminservice.check(companyId).subscribe((res: any) => {
      console.log("res", res);
      this.payload = res;
      if (this.companyId.value) {
        this.companyIdINfo = this.companyId.value;
      } else {
        this.companyIdINfo = res.companyId;
      }


      if (res.companyFoodTypesOrder.length != 0) {

        this.FoodTypesOrder = res.companyFoodTypesOrder.map((element: any) => {
          element['status'] = true;
          return element;
        });
        console.log("value1", this.FoodTypesOrder);

        this.foodtype = true;
      } else {
        this.foodtype = false;
      }

      if (res.companyFoodWeekDaysOrder.length != 0) {
        this.FoodWeekDaysOrder = res.companyFoodWeekDaysOrder.map((element: any) => {
          element['status'] = true;
          return element;
        });
        console.log("value2", this.FoodWeekDaysOrder);

        this.foodweekdays = true;
      } else {
        this.foodweekdays = false;
      }


      this.foodOrder = {};
      res.companyFoodTypesOrder.forEach((element: any) => {
        Object.values(element).map(res => {
          if (res == 'Breakfast' || res == 'Lunch' || res == 'Potluck' || res == 'Dinner') {
            console.log("res", res);
            var name: any = res;
            this.foodOrder[name] = true;
          }
        });
      });
      this.weekDaysOrder = {};
      res.companyFoodWeekDaysOrder.forEach((element: any) => {
        Object.values(element).map(res => {
          if (res == 'Monday' || res == 'Tuesday' || res == 'Wednesday' || res == 'Thursday' || res == 'Friday' || res == 'Saturday' || res == 'Sunday') {
            console.log("res", res);
            var name: any = res;
            this.weekDaysOrder[name] = true;
          }
        });
      });
      console.log("res ******", res);
      console.log("foodOrder", this.foodOrder);
      console.log("weekDaysOrder", this.weekDaysOrder);

      this.initialForm(res);
    });

  }

  weekdayChanged(data: any) {
    this.FoodWeekDaysOrder = [];
    this.payload['companyId'] = this.companyId.value;
    if (this.companyFoodWeekDaysOrder.value.monday) {
      let obj = {
        weekDayName: 'Monday',
        weekDayId: 1,
        status: true,
      }

      this.FoodWeekDaysOrder.push(obj);
    }
    if (this.companyFoodWeekDaysOrder.value.tuesday) {
      let obj = {
        weekDayName: 'Tuesday',
        weekDayId: 2,
        status: true,
      }

      this.FoodWeekDaysOrder.push(obj);
    }
    if (this.companyFoodWeekDaysOrder.value.wednesday) {

      let obj = {
        weekDayName: 'Wednesday',
        weekDayId: 3,
        status: true,
      }

      this.FoodWeekDaysOrder.push(obj);
    }
    if (this.companyFoodWeekDaysOrder.value.thursday) {
      let obj = {
        weekDayName: 'Thursday',
        weekDayId: 4,
        status: true,
      }

      this.FoodWeekDaysOrder.push(obj);
    }
    if (this.companyFoodWeekDaysOrder.value.friday) {

      let obj = {
        weekDayName: 'Friday',
        weekDayId: 5,
        status: true,
      }

      this.FoodWeekDaysOrder.push(obj);
    }
    if (this.companyFoodWeekDaysOrder.value.saturday) {
      let obj = {
        weekDayName: 'Saturday',
        weekDayId: 6,
        status: true,
      }

      this.FoodWeekDaysOrder.push(obj);
    }

    if (this.companyFoodWeekDaysOrder.value.sunday) {
      let obj = {
        weekDayName: 'Sunday',
        weekDayId: 7,
        status: true,
      }

      this.FoodWeekDaysOrder.push(obj);
    }

    if (this.FoodWeekDaysOrder.length != 0) {
      this.foodweekdays = true;
    } else {
      this.foodweekdays = false;
    }

    console.log("FoodWeekDaysOrder", this.FoodWeekDaysOrder);


    this.FoodWeekDaysOrder.forEach((res: any) => {
      res['compId'] = Number(this.companyId?.value);
      if (this.payload?.companyFoodWeekDaysOrder?.length != 0 && this.payload?.companyFoodWeekDaysOrder != undefined) {
        res['companyName'] = this.payload?.companyFoodWeekDaysOrder[0]['companyName']
      }
      res['employeeCount'] = 0;
      res['foodTypeCount'] = 0;
      res['foodTypeName'] = null;
      res['weekDaysCount'] = 0;
    })

  }

  foodTypeChanged(data: any) {
    console.log("data", this.FoodTypesOrder, this.companyFoodTypesOrder.value);
    this.FoodTypesOrder = [];
    this.payload['companyId'] = this.companyId.value;
    if (this.companyFoodTypesOrder.value.breakfast) {
      let obj = {
        foodTypes: 'Breakfast',
        foodtypeID: 1,
        status: true,
      }
      this.FoodTypesOrder.push(obj);
    }
 

    if (this.companyFoodTypesOrder.value.lunch) {
      let obj = {
        foodTypes: 'Lunch',
        foodtypeID: 2,
        status: true,
      }
      this.FoodTypesOrder.push(obj);
    }
 
    if (this.companyFoodTypesOrder.value.potluck) {

      let obj = {
        foodTypes: 'Potluck',
        foodtypeID: 3,
        status: true,
      }
      this.FoodTypesOrder.push(obj);
    }
 
    if (this.companyFoodTypesOrder.value.dinner) {
      let obj = {
        foodTypes: 'Dinner',
        foodtypeID: 4,
        status: true,
      }
      this.FoodTypesOrder.push(obj);
    }

    if (this.FoodTypesOrder.length != 0) {
      this.foodtype = true;
    } else {
      this.foodtype = false;
    }

    console.log("this.payload.companyFoodTypesOrder", this.payload?.companyFoodTypesOrder);

    this.FoodTypesOrder.forEach((res: any) => {
      res['companyID'] = Number(this.companyId.value);
      if (this.payload?.companyFoodTypesOrder?.length != 0 && this.payload.companyFoodTypesOrder != undefined) {
        res['companyName'] = this.payload?.companyFoodTypesOrder[0]['companyName']
      }
    })

    console.log("this.FoodTypesOrder", this.FoodTypesOrder);

  }

  submitFood() {
    if (!this.companyname) {
      this.toastr.error("Please select Company Name");
      return;
    }
    console.log("foodweekdays", this.foodweekdays, " foodtype", this.foodtype);

    if (!this.foodweekdays || !this.foodtype) {
      this.toastr.error("Please fill the required fields");
      return;
    }

    var payload = {
      companyId: Number(this.companyId.value),
      companyFoodTypesOrder: this.FoodTypesOrder,
      companyFoodWeekDaysOrder: this.FoodWeekDaysOrder,
    };



    this.adminservice.postFoodData(payload).subscribe({
      next: (result: any) => {
        console.log(result);
        if (!result['isError']) {
          this.toastr.success(result.message);
          window.setTimeout(function () { location.reload() }, 1500)
        } else {
          this.toastr.error(result.message);
          // this.toastr.error("Please change Food Types and Weekdays");
        }
      },
      error: () => {
        console.log('Error');
      },
    });

  }
}
