import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { HomeService } from '../service/home.service';
import { ToastrService } from 'ngx-toastr';
import { ValidateEmail } from 'src/app/validation.email';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  providers: [ToastrService],
})
export class HomeComponent implements OnInit {
  foodMenu: any = []
  day: any
  foodDetails: any
  foodMenuList: any = []
  obj: any = []
  feedBackObj: any = []
  feedBackData: any = []
  now: any;
  isFeedbackSelected: boolean = true;
  feedBackForm: boolean = true;
  today: any
  show: boolean = false;
  val: any;
  emailNotValid: boolean = false;
  isSubmit: boolean = false;
  specialCharacter: any = false;


  constructor(
    private httpservice: HomeService,
    private datepipe: DatePipe,
    private toastr: ToastrService
  ) { }

  dayName: any

  ngOnInit(): void {
    this.httpservice.getMenu().subscribe(data => {
      this.foodMenu = data;
      console.log(data)
      this.now = this.datepipe.transform((new Date), 'MM/dd/yyyy h:mm:ss');
      this.today = new Date()
      this.day = this.datepipe.transform(this.today, 'EEEE')
      this.dayName = this.day.toLowerCase()
      console.log(this.dayName)
      this.foodMenuList = this.foodMenu.menu.days[0][this.dayName]['breakfast']

    });

  }


  feedbackForm = new FormGroup({

    // dishes : new FormControl('',[Validators.required]),
    name: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required, Validators.email,ValidateEmail]),
    mobile: new FormControl('', [Validators.required, Validators.maxLength(10), Validators.pattern('[0-9]*')]),
    company: new FormControl('', [Validators.required]),
    feedback: new FormControl('', [Validators.required]),
  });



  get name() {
    return this.feedbackForm.get('name');
  }
  get email() {
    this.specialCharacter =  this.feedbackForm.get('email')?.errors?.['specialCharacter'];
    return this.feedbackForm.get('email');
  }
  get mobile() {
    return this.feedbackForm.get('mobile');
  }
  get company() {
    return this.feedbackForm.get('company');
  }
  get feedback() {
    return this.feedbackForm.get('feedback');
  }

  // getWeekdayName(event:any){

  //   this.day=event.target.value

  // }

  reloadPage() {
    window.location.reload();
  }

  getFoodDetails(event: any) {

    if (event == "uptate") {
      this.feedBackForm = true;
    }
    else {
      this.foodDetails = event.target.value
      console.log(this.foodDetails);
      this.foodMenuList = this.foodMenu.menu.days[0][this.dayName][this.foodDetails];

      if (event.target.checked) {
        this.feedBackForm = false;
        this.isFeedbackSelected = true;
      } else {
        this.feedBackForm = true;
      }
    }

      this.foodMenuList.forEach((ele : any) => {
        console.log("ele",Object.keys(ele).length );
        Object.keys(ele).map(res=>{
          if(res == 'selectedRating'){
            this.isFeedbackSelected = false;
          }
        });
      });
   
    this.submitForm();

  }

  selectFeedBack(item: any, rating: any) {
    this.getFoodDetails('uptate')
    item.selectedRating = rating;
    this.isFeedbackSelected = false;
    console.log(this.isFeedbackSelected);

    this.feedBackObj = {
      dish: item.dish,
      rating: rating,
    };
    this.feedBackData.push(this.feedBackObj);
    this.submitForm();
  }

  emailValidation() {
    console.log("  this.email",  this.email ,this.specialCharacter);
    let email = this.feedbackForm.get('email')?.value;
    if (email) {
      this.httpservice.emailValidLogin(email).subscribe((res: any) => {
        if (!res?.['isError']) {
          this.emailNotValid = true;
          this.submitForm();
        } else
          this.emailNotValid = false;
          this.submitForm();
      });
    };

    this.submitForm();
  }

  validation(data?:any){
    if(data == "Email"){
      this.emailValidation();
    }
    this.submitForm();
  }

  submitForm() {
    if (this.feedbackForm.valid && this.emailNotValid && !this.isFeedbackSelected)
      this.isSubmit = true;
    else
      this.isSubmit = false;
  }



  sendFeedBackMail(data: any) {

    console.log("  this.email",  this.email);
    
    if (this.isSubmit) {
      console.log("this.feedbackForm.valid", this.feedbackForm.valid);
      this.isSubmit = true;
      this.obj = {
        "name": data.name,
        "feedbackDate": this.now,
        "day": this.day,
        "mobile": data.mobile,
        "email": data.email,
        "company": data.company,
        "type": this.foodDetails,
        "dishes": this.feedBackData,
        "feedback": data.feedback
      }

      console.log(this.obj);
      this.httpservice.postFeedBack(this.obj).subscribe({
        next: (result) => {
          this.feedbackForm.reset();
          // this.toastr.success('Feedback Sent Successfully')
        },
        error: () => {
          console.log('Invalid api request');
          this.toastr.error('Failed');
        },
      });
    } else {
      this.toastr.error('please enter the required fields');
    }

  }
}
