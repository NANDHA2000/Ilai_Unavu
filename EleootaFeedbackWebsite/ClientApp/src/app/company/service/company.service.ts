import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { company_url } from '../company-url.config';

@Injectable({
  providedIn: 'root',
})
export class CompanyService {
  constructor(private http: HttpClient) {}

  viewFoodRequest(id: any) {
    let url = company_url.view_food;
    let queryParams = new HttpParams();
    queryParams = queryParams.append('id', id);
    return this.http.get(url, { params: queryParams });
  }
  GetData(name:any,week:any) {
    let url = company_url.get_data;
    let queryParams = new HttpParams();
    queryParams = queryParams.append('companyName', name);
    queryParams = queryParams.append('isNextWeek', week);
    return this.http.get(url, { params: queryParams });
  }

  employeeUpload(file: any,id:any) {
    const formData = new FormData();

    // Store form name as "file" with file data
    formData.append('file', file, file.name);

    let queryParams = new HttpParams();
    queryParams = queryParams.append('companyID', id);
    let url = company_url.upload_employee;
    console.log(queryParams);
    return this.http.post(url, formData, { params: queryParams });
  }

  GetempData(companyId: any) {
    let url = company_url.get_Emp_data;
    let queryParams = new HttpParams();
    queryParams = queryParams.append('companyId',companyId);
    return this.http.get(url, { params: queryParams });
  }

  GetempById(id:any){
    let url=company_url.getEmp_ById
    let queryParams = new HttpParams();
    queryParams = queryParams.append('id',id);
    console.log(id)
    return this.http.get(url,{params: queryParams})
  }

  editEmployeedata(data:any){
    let url =company_url.modify_employee
    return this.http.put(url,data)
  }

  
  deleteEmployeebyId(id:any){
    let url=company_url.delete_employee
    let queryParams = new HttpParams();
    queryParams = queryParams.append('employeeId',id);
    return this.http.delete(url,{params: queryParams})
  }
 

}
// https://eleootademofeedbackapi.azurewebsites.net/api/Employee/DeleteEmployeeById?employeeId=6