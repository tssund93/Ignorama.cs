using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class NewThreadViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Subject")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Body")]
        public string Text { get; set; }

        [Required]
        [Display(Name = "Board")]
        public int TagID { get; set; }

        public bool Anonymous { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
    }
}