using BookShop.Common;
using BookShop.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ImportDto
{
    [XmlType("Book")]
    public class BookImportDto
    {
        [Required]
        [StringLength(GlobalConstants.Book_Name_MaxLenght, MinimumLength = GlobalConstants.Book_Name_MinLenght)]
        public string Name { get; set; }

        [Required]
        [Range(GlobalConstants.Book_Genre_MinValue, GlobalConstants.Book_Genre_MaxValue)]
        public int Genre { get; set; }

        [Range(GlobalConstants.Book_Price_MinValue, (double)decimal.MaxValue)]
        public decimal Price { get; set; }

        [Range(GlobalConstants.Book_Pages_MinValue, GlobalConstants.Book_Pages_MaxValue)]
        public int Pages { get; set; }

        [Required]
        public string PublishedOn { get; set; }
    }
}
