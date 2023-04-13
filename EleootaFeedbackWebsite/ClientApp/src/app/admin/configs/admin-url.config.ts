import { environment } from "src/environments/environment";
export const admin_url = {
  get_company:`${environment.apiBaseUrl}Company/GetCompanyList`,
  post_food:`${environment.apiBaseUrl}Food/FoodTypeByWeekDaysOrder`,
  get_list:`${environment.apiBaseUrl}Company/GetCompanyList`,
  get_list_by_id:`${environment.apiBaseUrl}Company/GetCompanyList`,
  add_company:`${environment.apiBaseUrl}Company/AddCompany`,
  edit_company:`${environment.apiBaseUrl}Company/ModifyCompany`,
  upload_food:`${environment.apiBaseUrl}Food/UploadFood`,
  modify_company_status:`${environment.apiBaseUrl}Company/ModifyCompanyStatus`,
  get_companyDetails:`${environment.apiBaseUrl}Food/GetCompanyById`,
  get_food_menu:`${environment.apiBaseUrl}FeedBackMail/Getfoodlist`,
  View_Food_Request:`${environment.apiBaseUrl}Company/ViewFoodRequest`,
  get_download_list:`${environment.apiBaseUrl}Food/DownloadFoodMenu`,
  // https://eleootademofeedbackapi.azurewebsites.net/api/

 


}
