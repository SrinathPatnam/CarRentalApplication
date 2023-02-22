using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalApplication.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Card Number")]
        [MinLength(16,ErrorMessage = "Please enter valid Card Number")]
        [StringLength(16,ErrorMessage = "Please enter valid Card Number")]
        public string CardNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }
        [Required]
        public int CvvNumber { get; set; }
        public int BookingId { get; set; } 
        public Booking Booking { get; set; }
        public int Payment_Price { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; }

        public string UserId { get; set; }

        public Payment()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
        }

    }
}
