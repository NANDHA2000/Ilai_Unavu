import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { admin_url } from '../configs/admin-url.config';


@Injectable({
  providedIn: 'root'
})
export class ManageCompanyApiService {

  constructor(private http: HttpClient) { }

  ngOnInit(): void {

  }

  getCompanyList(data: any) {
    let url = admin_url.get_company
    let queryParams = new HttpParams();
    queryParams = queryParams.append("CompanyStatus", data);
    return this.http.get(url, { params: queryParams })
  }
  getCompanyListbyId(id: any, status: any) {
    let url = admin_url.get_list_by_id
    let queryParams = new HttpParams();
    queryParams = queryParams.append("CompanyId", id);
    queryParams = queryParams.append("CompanyStatus", status);
    return this.http.get(url, { params: queryParams })
  }

  // https://eleootademofeedbackapi.azurewebsites.net/api/Company/GetCompanyList?CompanyId=1


  addCompanyList(data: any) {
    let url = admin_url.add_company
    return this.http.post(url, data)

  }

  editCompanyList(data: any) {
    let url = admin_url.edit_company
    return this.http.put(url, data)
  }

  getCompanyDetailsbyId(id: any, status: any) {
    let url = admin_url.get_companyDetails
    let queryParams = new HttpParams();
    queryParams = queryParams.append("CompanyId", id);
    return this.http.get(url, { params: queryParams })
  }

  deleteCompanyById(data: any) {
    let url = admin_url.modify_company_status
    console.log(data);
    return this.http.patch(url, data)
  }
}
