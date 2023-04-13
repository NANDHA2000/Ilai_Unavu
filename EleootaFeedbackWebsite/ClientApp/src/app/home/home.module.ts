import { NgModule } from '@angular/core';
import { CommonModule} from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home/home.component';
import { HomeNavComponent } from './home-nav/home-nav.component';
import { LoginComponent } from './login/login.component';
import { FooterComponent } from './footer/footer.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgOtpInputModule } from  'ng-otp-input';
import { NumberDirective } from './home/number-directive.service';





@NgModule({
  declarations: [
    HomeComponent,
    HomeNavComponent,
    LoginComponent,
    FooterComponent,
    NumberDirective

  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    ReactiveFormsModule,
    NgOtpInputModule,
    FormsModule

  ]
})
export class HomeModule { }
