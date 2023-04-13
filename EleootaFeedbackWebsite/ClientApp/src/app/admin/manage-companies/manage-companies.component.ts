import { Component, OnInit } from '@angular/core';
import { ManageCompanyApiService } from './manage-company-api.service';
import {
  FormGroup,
  Validators,
  FormControl,
  Validator,
  FormBuilder,
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ValidateEmail } from 'src/app/validation.email';

@Component({
  selector: 'app-manage-companies',
  templateUrl: './manage-companies.component.html',
  styleUrls: ['./manage-companies.component.css'],
  providers: [ToastrService],
})
export class ManageCompaniesComponent implements OnInit {
  companybyid: any;
  companyData: any = [];
  tempCompData: any;
  companyId: any;
  companyName: any;
  companyMail: any;
  companyStatus: any;
  companyNameDelete: any;
  companyMailDelete: any;
  CompId: any;
  specialCharacter:boolean = false;
  specialCharacter2: boolean = false;

  constructor(
    private managecompapi: ManageCompanyApiService,
    private toastr: ToastrService,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit(): void {
    this.getCompanyList();
    this.initForm();
    this.initEditForm();
  }

  companyform!: FormGroup;
  initForm() {
    this.companyform = this.formBuilder.group({
      companyFormName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email, ValidateEmail]],
    });
  }

  get companyFormName() {
    return this.companyform.get('companyFormName');
  }
  get email() {
    this.specialCharacter =  this.companyform.get('email')?.errors?.['specialCharacter'];
    return this.companyform.get('email');
  }

  editCompanyInfo!: FormGroup;
  initEditForm() {
    this.editCompanyInfo = this.formBuilder.group({
      companyInfoName: [
        this.companyName,
        [Validators.required, Validators.maxLength(50)],
      ],
      companyInfoEmail: [
        this.companyMail,
        [Validators.required, Validators.email, ValidateEmail],
      ],
    });
  }

  get companyInfoName() {
    return this.editCompanyInfo.get('companyInfoName');
  }
  get companyInfoEmail() {
    this.specialCharacter2 =  this.editCompanyInfo.get('companyInfoEmail')?.errors?.['specialCharacter'];
    return this.editCompanyInfo.get('companyInfoEmail');
  }

  companylist: any = [];
  getCompanyList() {
    this.companyStatus = 1;
    this.managecompapi
      .getCompanyList(this.companyStatus)
      .subscribe((res: any) => {
        this.companylist = res;
        for (let i = 0; i < this.companylist.length; i++) {
          if (this.companylist[i].companyStatus) {
          }
        }
      });
  }

  getCompanyById(id: any, status: any) {
    this.managecompapi.getCompanyListbyId(id, status).subscribe((res: any) => {
      this.companylist = res;
      this.companyNameDelete = res.companyName;
      this.companyMailDelete = res.companyMail;
    });
  }

  getCompanyListData(data: any) {
    var obj = {
      CompanyId: 0,
      CompanyName: data.companyFormName,
      CompanyMail: data.email,
      Companystatus: 1,

    };

    this.managecompapi.addCompanyList(obj).subscribe({
      next: (result: any) => {
        if (result.isError == true) {
          this.toastr.error(result.message);
        } else {
          this.getCompanyList();
          this.toastr.success('Company Added');
        }
      },
      error: () => {
        console.log('Invalid api request');
        this.toastr.error('Failed');
      },
    });
  }

  editCompany(id: any, status: any) {
    this.managecompapi.getCompanyListbyId(id, status).subscribe((data) => {
      this.companyData = data;
      this.CompId = this.companyData[0].companyId;
      this.companyName = this.companyData[0].companyName;
      this.companyMail = this.companyData[0].companyMail;
      this.companyStatus = this.companyData[0].companyStatus;
      this.initEditForm();
    });
  }

  editCompanyData(data: any) {
    var objs = {
      companyId: this.CompId,
      companyName: data.companyInfoName,
      companyMail: data.companyInfoEmail,
      companyStatus: this.companyStatus,
    };

    this.managecompapi.editCompanyList(objs).subscribe({
      next: (result:any) => {
        if(result.isError == true)
        {
          this.toastr.error('Email address already existing');
        }else {
        this.getCompanyList();
        this.toastr.success('Details Edited');
      }},
      error: () => {
        console.log('Invalid api request');
        this.toastr.error('Failed');
      },
    });
  }
  
  confirmModal(companyId :any , companyName:any, companyMail:any){
    this.CompId=companyId,
    this.companyName=companyName,
    this.companyMail=companyMail
  }
  deleteCompany(id: any, name: any, mail: any) {
    let obj = {
      companyId: this.CompId,
      companyName: this.companyName,
      companyMail: this.companyMail,
      companyStatus: 0,
    };
    console.log(obj)
    // if(confirm('Are you sure, you want to delete the company ?'))
    this.managecompapi.deleteCompanyById(obj).subscribe({
      next: (result) => {
        this.getCompanyList();
        this.toastr.success('Company Deleted');
      },
      error: () => {
        console.log('Invalid api request');
        this.toastr.error('Failed');
      },
    });
  }

}
