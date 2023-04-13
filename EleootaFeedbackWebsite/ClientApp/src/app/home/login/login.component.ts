import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HomeService } from '../service/home.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgOtpInputComponent } from 'ng-otp-input';
import { timeout, timer } from 'rxjs';
import { DatePipe, getLocaleDateFormat } from '@angular/common';
import { ValidateEmail } from 'src/app/validation.email';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [ToastrService]
})
export class LoginComponent implements OnInit {
  loginData: any = [];
  userData: any = [];
  emailID: any;
  otp: any;
  ngOtpInputRef: any = [];
  timeleft: any;
  m: any;
  s: any;
  bool = false;
  boolotp = true;
  datee: any;
  sdate: any;
  specialCharacter : boolean = false;

  @ViewChild(NgOtpInputComponent, { static: false })
  ngOtpInput: any;
  timeLeft: any;
  interval: any;
  displayTimer: any;
  currentDate: any;

  onOtpChange(event: any) { }

  constructor(private homeservice: HomeService,
    private router: Router,
    private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  loginForm = new FormGroup({
    email: new FormControl('', [Validators.required , Validators.email , ValidateEmail]),
  });

  get email() {
    this.specialCharacter =  this.loginForm.get('email')?.errors?.['specialCharacter'];
    return this.loginForm.get('email');
  }

  displayStyle = "none";

  openPopup() {
    this.displayStyle = "block";
  }
  closePopup() {
    this.displayStyle = "none";
    window.location.reload()
  }


  sendOtp(data: any) {
    this.bool = false;
    this.emailID = data.email;
    // console.log(data)
    this.homeservice.sendEmailOtp(data).subscribe({
      next: (result) => {
        this.loginData = result;
        console.log(result);
        if (this.loginData.isError) {
          this.toastr.error('Invalid Email', '', { timeOut: 2000 });

        }
        else {
          this.toastr.success('OTP Sent', '', { timeOut: 2000 })
          this.openPopup()
        }

      },
      error: () => {
        console.log('Invalid login details');
        this.toastr.error('Error', '', { timeOut: 2000 });
      },
    });
  }
  verifyForm = new FormGroup({
    firstdigit: new FormControl('', [
      Validators.required,
      Validators.pattern('[0-9]*'),
    ]),
    seconddigit: new FormControl('', [
      Validators.required,
      Validators.pattern('[0-9]*'),
    ]),
    thirddigit: new FormControl('', [
      Validators.required,
      Validators.pattern('[0-9]*'),
    ]),
    fourthdigit: new FormControl('', [
      Validators.required,
      Validators.pattern('[0-9]*'),
    ]),
    fifthdigit: new FormControl('', [
      Validators.required,
      Validators.pattern('[0-9]*'),
    ]),
    sixthdigit: new FormControl('', [
      Validators.required,
      Validators.pattern('[0-9]*'),
    ]),
  });

  get firstdigit() {
    return this.verifyForm.get('firstdigit');
  }
  get seconddigit() {
    return this.verifyForm.get('seconddigit');
  }
  get thirddigit() {
    return this.verifyForm.get('thirddigit');
  }
  get fourthdigit() {
    return this.verifyForm.get('fourthdigit');
  }
  get fifthdigit() {
    return this.verifyForm.get('fifthdigit');
  }
  get sixthdigit() {
    return this.verifyForm.get('sixthdigit');
  }

  verifyOtp(data: any) {
    // console.log(data)
    this.otp =
      data.firstdigit +
      data.seconddigit +
      data.thirddigit +
      data.fourthdigit +
      data.fifthdigit +
      data.sixthdigit;
    const obj: any = {
      email: this.emailID,
      otp: this.ngOtpInput.currentVal,
    };


    this.homeservice.verifyEmailOtp(obj).subscribe({
      next: (result) => {
        this.userData = result;
        sessionStorage.setItem('sessionData', JSON.stringify(this.userData));
        console.log(this.userData);

        if (this.userData.isSuccess == true) {
          if (this.userData.userType == 'Employee') {
            var today = new Date();

            const obj = {
              "empid": this.userData.empId,
              "currentDate": today

            }
            this.homeservice.submitReq(obj).subscribe((data: any) => {
              let date = new Date();

              this.currentDate = date.getDay();

              this.sdate = this.currentDate.toString();

              if (data.message == "Data Already exists") {
                let url = 'employee/submit-request';
                this.router.navigateByUrl(url);
              }
              else {
                let url = this.userData.redireURL;
                this.router.navigateByUrl(url);
              }
            })

          } else if (this.userData.userType == 'Company') {
            let url = this.userData.redireURL;
            this.router.navigateByUrl(url);
          } else if (this.userData.userType == 'Admin') {
            let url = this.userData.redireURL;
            this.router.navigateByUrl(url);
          } else {
            let url = `login`;
            this.router.navigateByUrl(url);
          }
        } else {
          console.log('invalid otp');
          this.toastr.error('Login Failed');
          let url = `login`;
          this.router.navigateByUrl(url);

        }
      },
      error: () => {
        console.log('Invalid login details');
        this.toastr.error('Login Failed');
      },
    });
  }

  keytab(event: any) {

    let element = event.srcElement.nextElementSibling; // get the sibling element
    console.log(element);
    if (element == null)  // check if its null
      return;
    else
      element.focus();
    // focus if not null

  }

  startTimer() {
    this.displayTimer = "block";

    this.interval = setInterval(() => {
      if (this.timeLeft > 0) {
        this.boolotp = true;
        this.timeLeft--;

        if (this.timeLeft == 0) {

          this.pauseTimer();
          this.displayTimer = "none";
          this.bool = true;
          this.boolotp = false;
        }
      }
      else {
        this.timeLeft = 60;
      }
    }, 1000)
  }
  pauseTimer() {
    clearInterval(this.interval);
  }


}

