import { environment } from "src/environments/environment";
export const home_url = {
  send_otp:`${environment.apiBaseUrl}Login/SendOtp`,
  verify_otp:`${environment.apiBaseUrl}Login/VerifyOtp`,
  get_food_menu:`${environment.apiBaseUrl}FeedBackMail/Getfoodlist`,
  send_feedback:`${environment.apiBaseUrl}FeedBackMail/SendFeedBackMail`,
  Verify_Feedback_User:`${environment.apiBaseUrl}FeedBackMail/VerifyFeedbackUser`,
  data_exists :`${environment.apiBaseUrl}Login/DataExists`
}


