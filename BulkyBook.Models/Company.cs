using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        [DataType(DataType.PostalCode)]
        public double? PostalCode { get; set; }
        [DataType(DataType.PhoneNumber)]
        public double? PhoneNumber { get; set; }
    }
}
