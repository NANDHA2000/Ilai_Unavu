import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  employeeDetails: any=[];

  constructor() { }
  IsLoggedIn(){
    this.employeeDetails= JSON.parse(sessionStorage.getItem('sessionData')!)

    return this.employeeDetails?.userType == 'Company'
  }
}
