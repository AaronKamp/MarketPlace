namespace TCCMarketPlace.Model
{
    public class YLMService
    {
        public int Service_Id { get; set; }
        public int Service_Type_Id { get; set; }
        public string Icon_Image { get; set; }
        public string Service_Title { get; set; }
        public string Service_Short_Desc { get; set; }
        public string Signup_Status { get; set; }
        public string Url { get; set; }
        public string Partner_Promo_Code { get; set; }
        public string Custom_Field_1 { get; set; }
        public string Custom_Field_2 { get; set; }
        public string Custom_Field_3 { get; set; }
        public string  Report_Url { get; set; }
        public string Inapp_Purchase_Id { get; set; }
        public decimal Purchase_Price {get;set; }
        public string  Serv_Enable { get; set; }
    }
}
