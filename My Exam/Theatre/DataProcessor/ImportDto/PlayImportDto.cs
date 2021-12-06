using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Theatre.Common;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class PlayImportDto
    {
        [Required]
        [XmlElement("Title")]
        [StringLength(GlobalConstants.Play_Title_MaxLenght, MinimumLength = GlobalConstants.Play_Title_MinLenght)]
        public string Title { get; set; }

        [Required]
        [XmlElement("Duration")]
        public string Duration { get; set; }

        [Required]
        [XmlElement("Rating")]
        [Range(GlobalConstants.Play_Rating_MinValue, GlobalConstants.Play_Rating_MaxValue)]
        public float Rating { get; set; }

        [Required]
        [XmlElement("Genre")]
        public string Genre { get; set; }

        [Required]
        [XmlElement("Description")]
        [StringLength(GlobalConstants.Play_Description_MaxLemght)]
        public string Description { get; set; }

        [Required]
        [XmlElement("Screenwriter")]
        [StringLength(GlobalConstants.Play_ScreenWriter_MaxLenght, MinimumLength = GlobalConstants.Play_ScreenWriter_MinLenght)]
        public string Screenwriter { get; set; }
    }
}
