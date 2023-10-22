using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Web;

namespace OmniScanMRI.WebApp.Models
{
    public class AppointmentBooking
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(255)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Appointment Reason is required.")]
        [StringLength(500)]
        public string AppointmentReason { get; set; }

        [Required(ErrorMessage = "Doctor is required.")]
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public virtual Doctors Doctor { get; set; }

        [Required(ErrorMessage = "Appointment Date and Time is required.")]
        public DateTime AppointmentDateTime { get; set; }
    }
}