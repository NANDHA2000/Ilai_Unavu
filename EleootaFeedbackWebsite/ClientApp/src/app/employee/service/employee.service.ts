import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { employee_url } from '../employee-url.config';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private http:HttpClient) { }

  getFoodDays(id:any){
    let url=employee_url.view_food_request
    let queryParams = new HttpParams();
    queryParams = queryParams.append("id",id);

    return this.http.get(url,{params:queryParams});
  }

  submitEmployeeFoodRequest(data:any){
    let url=employee_url.submit_food_request;
    return this.http.post(url,data)
  }

}
