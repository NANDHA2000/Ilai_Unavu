import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../service/employee.service';

@Component({
  selector: 'app-submit-request',
  templateUrl: './submit-request.component.html',
  styleUrls: ['./submit-request.component.css']
})
export class SubmitRequestComponent implements OnInit {
  employeeDetails: any;

  constructor(private employeeservice: EmployeeService) { }

  ngOnInit(): void {
    this.employeeDetails = JSON.parse(sessionStorage.getItem('sessionData')!);
    console.log(this.employeeDetails);
  }

}
