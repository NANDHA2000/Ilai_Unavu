import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { CompanyService } from '../service/company.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import swal from 'sweetalert2'

;
import Swal from 'sweetalert2';
import { ValidateEmail } from 'src/app/validation.email';
@Component({
  selector: 'app-manage-employees',
  templateUrl: './manage-employees.component.html',
  styleUrls: ['./manage-employees.component.css'],
  providers: [ToastrService],
})
export class ManageEmployeesComponent implements OnInit {
  companyDetails: any = [];
  empData: any = [];
  employeeList: any;
  empMail: any;
  empName: any;
  empId: any;
  empid: any;
  message: any;
  p=0;

  specialCharacter : boolean = false;

  constructor(
    private _http: HttpClient,
    private companyservice: CompanyService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.companyDetails = JSON.parse(sessionStorage.getItem('sessionData')!);
    console.log(this.companyDetails);
    this.getEmplist(this.companyDetails.companyId);
    console.log(this.companyDetails.companyId);
  }

  getEmplist(companyId: any) {
    this.companyservice
      .GetempData(this.companyDetails.companyId)
      .subscribe((result) => {
        console.log(result);

        this.empData = result;

        console.log(this.empData);
      });
  }

  shortLink: string = '';
  loading: boolean = false; // Flag variable
  file: any = null; // Variable to store file

  uploadExcelForm = new FormGroup({
    excelFile: new FormControl('', [Validators.required]),
  });

  get excelFile() {
    return this.uploadExcelForm.get('excelFile');
  }

  selectFile(event: any) {
    this.file = <File>event.target.files[0];

    if (
      this.file.type ===
      'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    ) {
      console.log(this.file.type);
    } else {
      this.toastr.error('Upload Excel File','',{ timeOut: 2000 });
      this.uploadExcelForm.reset();
    }
  }

  uploadEmpExcel() {
    this.loading = !this.loading;
    console.log(this.file);
    this.companyservice
      .employeeUpload(this.file, this.companyDetails.companyId)
      .subscribe((event: any) => {
        console.log(event);
        this.message = event;
        console.log(this.message);
        if (this.message.isError != true) {
          this.toastr.success(this.message.data.uploadError,'',{ timeOut: 2000 });
          this.getEmplist(this.companyDetails.companyId);
        }
        else{
          if(this.message.data==null){
          this.toastr.error(this.message.message,'',{ timeOut: 2000 });
          this.getEmplist(this.companyDetails.companyId);
          }
          else{
            this.toastr.error(this.message.data.uploadError,'',{ timeOut: 2000 });
          this.getEmplist(this.companyDetails.companyId);
          }
        }
      });
  }

  getEmployeeById(id: any) {
    this.companyservice.GetempById(id).subscribe((res: any) => {
      this.empData = res;
    });
  }

  editemployeeInfo = new FormGroup({
    employeeName: new FormControl('', [Validators.required]),
    employeeEmail: new FormControl('', [Validators.required, Validators.email , ValidateEmail]),
  });

  get employeeName() {
    return this.editemployeeInfo.get('employeeName');
  }

  get employeeEmail() {
    this.specialCharacter = this.editemployeeInfo.get('employeeEmail')?.errors?.['specialCharacter'];
    return this.editemployeeInfo.get('employeeEmail');
  }

  editEmployee(id: any) {
    console.log(id);
    this.companyservice.GetempById(id).subscribe((data) => {
      this.employeeList = data;
      this.empid = this.employeeList.empid;
      this.empName = this.employeeList.empName;
      this.empMail = this.employeeList.empMail;
      console.log(data);
    });
  }

  editEmployeedata(data: any) {
    var objs = {
      empid: this.empid,
      empName: data.employeeName,
      empMail: data.employeeEmail,
      companyId: this.companyDetails.companyId,
    };

    console.log(objs);

    this.companyservice.editEmployeedata(objs).subscribe({
      next: (result: any) => {
        if (result.isError == true) {
          this.toastr.error('Email address already existing','',{ timeOut: 2000 });
        } else {
          this.toastr.success('Details Edited','',{ timeOut: 2000 });
          this.getEmplist(this.companyDetails.companyId);
        }
      },
      error: () => {
        console.log('Invalid api request');
        this.toastr.error('Failed','',{ timeOut: 2000 });
      },
    });
  }

  deleteEmployeebyId(id:any){
    Swal.fire({

      title: 'Do you want to Delete the Employee?',

      showDenyButton: true,

      confirmButtonText: 'YES',

      denyButtonText: `NO`,

    }).then((result)=>{
      if (result.isConfirmed ){
        this.companyservice.deleteEmployeebyId(id).subscribe(()=>
        this.getEmplist(this.companyDetails.companyId)

        );
        this.toastr.success('Employee Delete Successfully','',{ timeOut: 2000 })
      }
      else if (result.isDenied){
       this.toastr.error('Employee Not Deleted','',{ timeOut: 2000 })
      }
    });
  }
   


  
}
