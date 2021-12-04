namespace SoftJail.Common
{
    public static class GlobalConstants
    {
        //Prisoner

        public const int Prisoner_FullName_MinLenght = 3;

        public const int Prisoner_FullName_MaxLenght = 20;

        public const int Prisoner_Age_MinValue = 18;

        public const int Prisoner_Age_MaxValue = 65;

        public const int Prisoner_Bail_MinValue = 0;

        public const string Prisoner_NickName_Regex = @"^The\s[A-Z][a-z]+$";

        //Officer

        public const int Officer_FullName_MinLenght = 3;

        public const int Officer_FullName_MaxLenght = 30;

        public const int Officer_Salary_MinValue = 0;

        //Cell

        public const int Cell_Number_MinLenght = 1;

        public const int Cell_Number_MaxLenght = 1000;

        //Department

        public const int Department_Name_MinLenght = 3;

        public const int Department_Name_MaxLenght = 25;

        //Mail

        public const string Mail_Address_Regex = @"^[A-Za-z0-9\s]+\sstr.$";

        //Error

        public const string Error_Message = "Invalid Data";
    }
}
