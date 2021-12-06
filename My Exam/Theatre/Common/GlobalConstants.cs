using System;

namespace Theatre.Common
{
    public static class GlobalConstants
    {
        //Theatre

        public const int Thetre_Name_MinLenght = 4;

        public const int Thetre_Name_MaxLenght = 30;

        public const double Thetre_NumberOfHalls_MinValue = 1;

        public const double Thetre_NumberOfHalls_MaxValue = 10;

        public const int Thetre_Director_MinLenght = 4;

        public const int Thetre_Director_MaxLenght = 30;

        //Play

        public const int Play_Title_MinLenght = 4;

        public const int Play_Title_MaxLenght = 50;

        public const int Play_Rating_MinValue = 0;

        public const int Play_Rating_MaxValue = 10;

        public const int Play_Description_MaxLemght = 700;

        public const int Play_ScreenWriter_MinLenght = 4;

        public const int Play_ScreenWriter_MaxLenght = 30;

        public const string Play_Duration_MinValue = "01:00:00";

        //Cast

        public const int Cast_FullName_MinLenght = 4;

        public const int Cast_FullName_MaxLenght = 30;

        public const string Cast_PhoneNumber_Regex = @"^\+44-\d{2}-\d{3}-\d{4}$";

        //Ticket

        public const int Ticket_Price_MinValue = 1;

        public const int Ticket_Price_MaxValue = 100;

        public const int Ticket_RowNumber_MinValue = 1;

        public const int Ticket_RowNumber_MaxValue = 10;

        //Error Messages

        public const string Error_Message = "Invalid data!";
    }
}
