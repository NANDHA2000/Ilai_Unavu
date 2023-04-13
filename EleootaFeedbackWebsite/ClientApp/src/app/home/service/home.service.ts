import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { home_url } from '../home-url.config';

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  constructor(private http: HttpClient) { }

  sendEmailOtp(data: any) {
    let url = home_url.send_otp
    return this.http.post(url, data)
  }
  verifyEmailOtp(data: any) {
    let url = home_url.verify_otp
    return this.http.post(url, data)
  }

  getMenu() {
    let url = home_url.get_food_menu
    return this.http.get(url)
  }

  postFeedBack(data: any) {
    console.log(data)
    let url = home_url.send_feedback
    return this.http.post(url, data, { responseType: 'text' })
  }
  
  submitReq(data: any) {
    let url = home_url.data_exists
    return this.http.post(url, data);
  }

  emailValidLogin(email: any) {
    let queryParams = new HttpParams();
    queryParams = queryParams.append("Email", email);
    return this.http.get(home_url.Verify_Feedback_User, { params: queryParams })
  }

}
