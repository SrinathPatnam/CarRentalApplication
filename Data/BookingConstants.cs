using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalApplication.Data
{
    public class BookingConstants
    {
        public const int STATUS_PENDING = 1;
        public const int STATUS_APPROVED = 2;
        public const int STATUS_REJECTED = 3;
        public const int STATUS_COMPLETED = 4;
        public const int STATUS_CANCELLED = 5;

        public static Dictionary<int, string> bookingStatus = new Dictionary<int, string> {
            { STATUS_PENDING, "Pending" },
            { STATUS_APPROVED, "Approved" },
            { STATUS_COMPLETED, "Completed"},
            {STATUS_REJECTED, "Rejected"},
            {STATUS_CANCELLED, "Cancelled"}
        };
    }
}
