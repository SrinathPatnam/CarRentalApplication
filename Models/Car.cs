using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalApplication.Models
{

    public class Car
    {
        [Key]
        public int Carid { get; set; }
        public string Carname { get; set; }
        public string Cartype { get; set; }       
        public string Region { get; set; }
        public int Price { get; set; }
        public int Rating { get; set; }
        public int CategoryId { get; set; }

        [DisplayName("Upload Image")]
        public string Image { get; set; }
        public Category Category { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public List<Booking> Bookings { get; set; }
    }
}
