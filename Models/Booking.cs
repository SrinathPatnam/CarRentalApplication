using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CarRentalApplication.Models
{     
    public enum ExtraFeatures
    {
        None,
        ChildSeat,
        FourWheelDrive,
        HeatedSeats,
        NavigationSystem
    }

    public enum Payment_Status
    {
        Paid,
        TobePaid
    }

    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        public string UserId { get; set; }
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }  

        public ExtraFeatures? ExtraFeatures { get; set; }
        public int CarId { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; }

        public Payment_Status? Payment_Status { get; set; }

        public Car Car { get; set; }
        public IList<Booking> BookingList { get; set; }
        public Booking()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
            this.Payment_Status = Models.Payment_Status.TobePaid;
        }        
    }
}
