using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PupPals.Models
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int HouseId { get; set; }
        public House House { get; set; }

        [Required]
        public bool MyPet { get; set; }

        public bool BestFriend { get; set; }

        public string Notes { get; set; }

        public string Photo { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

    }
}
