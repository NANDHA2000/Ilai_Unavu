import { Subscription, timer } from "rxjs";
import { map, share } from "rxjs/operators";
import { Component, OnInit,OnDestroy } from '@angular/core';

@Component({
  selector: 'app-company-nav',
  templateUrl: './company-nav.component.html',
  styleUrls: ['./company-nav.component.css']
})
export class CompanyNavComponent implements OnInit,OnDestroy {
  companyDetails:any=[]
  time = new Date();
  rxTime = new Date();
  intervalId:any;
  subscription:any;


  constructor() { }

  ngOnInit(): void {
    this.companyDetails= JSON.parse(sessionStorage.getItem('sessionData')!)

    this.intervalId = setInterval(() => {
      this.time = new Date();
    }, 1000);


  this.subscription = timer(0, 1000)
      .pipe(
        map(() => new Date()),
        share()
      )
      .subscribe(time => {
        let hour = this.rxTime.getHours();
        let minuts = this.rxTime.getMinutes();
        let seconds = this.rxTime.getSeconds();
        //let a = time.toLocaleString('en-US', { hour: 'numeric', hour12: true });
        let NewTime = hour + ":" + minuts + ":" + seconds
        //console.log(NewTime);
        this.rxTime = time;
      });
  }

  ngOnDestroy() {
    clearInterval(this.intervalId);
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
  

  logoutSession(){
    sessionStorage.clear();
  }
}


