namespace BookShop.Common
{
    public static class GlobalConstants
    {
        //Author

        public const int Author_FirstName_MinLenght = 3;

        public const int Author_FirstName_MaxLenght = 30;

        public const int Author_LastName_MinLenght = 3;

        public const int Author_LastName_MaxLenght = 30;

        public const int Author_Phone_MaxLenght = 12;

        public const string Author_PhoneNumber_Regex = @"^\d{3}-\d{3}-\d{4}$";

        //Book

        public const int Book_Name_MinLenght = 3;

        public const int Book_Name_MaxLenght = 30;

        public const double Book_Price_MinValue = 0.01;

        public const int Book_Pages_MinValue = 50;

        public const int Book_Pages_MaxValue = 5000;

        public const int Book_Genre_MinValue = 1;

        public const int Book_Genre_MaxValue = 3;

        //Error Message

        public const string Error_Message = "Invalid data!";
    }
}
