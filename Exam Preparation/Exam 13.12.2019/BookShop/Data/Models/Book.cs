using BookShop.Common;
using BookShop.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Data.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.Book_Name_MaxLenght)]
        public string Name { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Range(GlobalConstants.Book_Price_MinValue, (double)decimal.MaxValue)]
        public decimal Price { get; set; }

        [Range(GlobalConstants.Book_Pages_MinValue, GlobalConstants.Book_Pages_MaxValue)]
        public int Pages { get; set; }

        [Required]
        public DateTime PublishedOn { get; set; }

        public ICollection<AuthorBook> AuthorsBooks { get; set; }

        public Book()
        {
            AuthorsBooks = new HashSet<AuthorBook>();
        }
    }
}
