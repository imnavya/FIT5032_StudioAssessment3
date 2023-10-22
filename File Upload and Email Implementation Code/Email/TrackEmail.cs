using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OmniScanMRI.WebApp.Models
{
    public class TrackEmail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public string SentByUserId { get; set; }

        [Required]
        public DateTime SentDate { get; set; }
        public string CcEmail { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}