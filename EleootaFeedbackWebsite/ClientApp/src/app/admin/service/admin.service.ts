import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { admin_url } from '../configs/admin-url.config';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient) { }

  getCompanyNames(data: any) {
    let url = admin_url.get_company
    return this.http.get(url)
  }

  postFoodData(data: any) {
    let url = admin_url.post_food
    return this.http.post(url, data)
  }

  foodDataUpload(data: any) {
    const formData = new FormData()
    formData.append('file', data)
    console.log(formData);
    console.log(data)
    let url = admin_url.upload_food
    return this.http.post(url, formData)
  }
  // getMenu(){
  //   let url=admin_url.get_food_menu
  //   return this.http.get(url)
  // }

  download_menu() {
    let url = admin_url.get_download_list
    return this.http.get(url)


  }
  check(idd: any) {
    let queryParams = new HttpParams();
    let url = admin_url.View_Food_Request;
    queryParams = queryParams.append("id", idd);
    return this.http.get(url, { params: queryParams })
  }


}

