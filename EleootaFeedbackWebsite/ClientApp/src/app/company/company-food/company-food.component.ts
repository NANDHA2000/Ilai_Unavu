import { Component, OnInit } from '@angular/core';
import { CompanyService } from '../service/company.service';

@Component({
  selector: 'app-company-food',
  templateUrl: './company-food.component.html',
  styleUrls: ['./company-food.component.css']
})
export class CompanyFoodComponent implements OnInit {

  foodRequestData:any=[];
  companyFoodTypesOrder:any=[];
  companyFoodWeekDaysOrder:any=[];
  companyDetails:any=[];

  constructor(private companyservice:CompanyService) { }

  ngOnInit(): void {
    this.companyDetails = JSON.parse(sessionStorage.getItem('sessionData')!);

    console.log(this.companyDetails);



    console.log(this.companyDetails.companyId)

    this.getFoodRequest(this.companyDetails.companyId);;
  }

  getFoodRequest(id:any){

    this.companyservice.viewFoodRequest(id).subscribe({
      next: (result) => {
        console.log(result)
      this.foodRequestData=result
      this.companyFoodTypesOrder=this.foodRequestData.companyFoodTypesOrder;
      console.log(this.companyFoodTypesOrder)
      this.companyFoodWeekDaysOrder=this.foodRequestData.companyFoodWeekDaysOrder
      console.log(this.companyFoodWeekDaysOrder)
      },
      error: () => {
        console.log('Invalid api');
      },
    })
  }

}
