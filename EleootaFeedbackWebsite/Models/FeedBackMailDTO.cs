using System.Collections.Generic;

namespace FeedBackSendMail.Models
{
    public class FeedBackMailDTO
    {
        public string Name { get; set; }
        public string FeedbackDate { get; set; }
        public string Day { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }        
        public string Company { get; set; }
        public string Type { get; set; }  
        public List<Items> Dishes { get; set; }
        public string Feedback { get; set; }
    }
    public class Items
    {
        public string Dish { get; set; }
        public string Rating { get; set; }        
    }

    public class FileFeedBackMailDTO
    {
        public string Name { get; set; }
        public string FeedbackDate { get; set; }
        public string Day { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string Dishes { get; set; }
        public string Rating  { get; set; }
        public string Feedback { get; set; }
    }
}
