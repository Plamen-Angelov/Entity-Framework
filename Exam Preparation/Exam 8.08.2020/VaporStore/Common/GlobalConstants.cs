namespace VaporStore.Common
{
    public static class GlobalConstants
    {
        public const int User_UserName_MaxLenght = 20;

        public const int User_UserName_MinLenght = 3;

        public const string User_FullName_Regex = @"^[A-Z][a-z]+ [A-Z][a-z]+$";

        public const int User_Age_Min = 3;

        public const int User_Age_Max = 103;

        public const int Card_Number_MaxLenght = 19;

        public const int Card_Cvc_MaxLenght = 3;

        public const string Card_Number_Regex = @"\d{4}\s\d{4}\s\d{4}\s\d{4}";

        public const string Card_Cvc_Regex = @"\d{3}";

        public const double Game_Min_PriceValue = 0;

        public const string Purchase_Key_Regex = @"^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$";

        public const string Error_Message = "Invalid Data";
    }
}
