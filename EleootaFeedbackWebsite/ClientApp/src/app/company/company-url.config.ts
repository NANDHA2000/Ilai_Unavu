import { environment } from "src/environments/environment";
export const company_url = {
  upload_employee:`${environment.apiBaseUrl}Employee/UploadEmployee`,
  view_food:`${environment.apiBaseUrl}Company/ViewFoodRequest`,
  get_data:`${environment.apiBaseUrl}Company/CompanyFoodCountByWeek`,
  get_Emp_data:`${environment.apiBaseUrl}Employee/GetAllEmployees`,
  modify_employee: `${environment.apiBaseUrl}Employee/ModifyEmployee`,
  getEmp_ById: `${environment.apiBaseUrl}Employee/GetEmployeeById`,
 delete_employee:`${environment.apiBaseUrl}Employee/DeleteEmployeeById`,
}
