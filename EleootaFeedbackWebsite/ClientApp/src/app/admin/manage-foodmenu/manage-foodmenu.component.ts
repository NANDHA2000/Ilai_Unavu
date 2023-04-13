import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from '../service/admin.service';

@Component({
  selector: 'app-manage-foodmenu',
  templateUrl: './manage-foodmenu.component.html',
  styleUrls: ['./manage-foodmenu.component.css'],
  providers: [ToastrService],
})
export class ManageFoodmenuComponent implements OnInit {
  fileName: any;
  formData: any = [];
  resJsonResponse: any=[];
  theJSON:any=[];
  downloadJsonHref: any;
  foodlist: any=[];
  download_link: any;

  constructor(
    private adminservice: AdminService,
    private toastr: ToastrService,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {this.getFoodMenu()}

  shortLink: string = '';
  loading: boolean = false;

  file!: File;

  uploadFoodDataForm = new FormGroup({
    jsonfile: new FormControl('', [Validators.required]),
  });

  get jsonfile() {
    return this.uploadFoodDataForm.get('jsonfile');
  }

  selectFile(event: any) {
    this.file = event.target.files[0];
    if (this.file.type === 'application/json') {
      console.log(this.file.type);
    } else {
      this.toastr.error('Upload JSON File');
      this.uploadFoodDataForm.reset();
    }
  }

  uploadFoodData() {
    this.loading = !this.loading;

    this.adminservice.foodDataUpload(this.file).subscribe({
      next: (event: any) => {
        if (typeof event === 'object') {
          this.shortLink = event.link;
          console.log(event);
          this.getFoodMenu();

          this.loading = false;
          
        }
        this.toastr.success('Food Menu Uploaded');
      },
      error: () => {
        this.toastr.error('Failed');
        console.log('Invalid api');
      },
    });
    
  }
  getFoodMenu(){
    this.adminservice.download_menu().subscribe(data => { 
     
      this.resJsonResponse = data; 
      console.log(this.resJsonResponse)
      this.generateDownloadJsonUri()  
  })}
  
    generateDownloadJsonUri() { 
      this.theJSON = JSON.stringify(this.resJsonResponse,null,4); 
      let uri = this.sanitizer.bypassSecurityTrustUrl("data:text/json;charset=UTF-8," + encodeURIComponent(this.theJSON)); 
      this.downloadJsonHref = uri;
  
  }


  }


