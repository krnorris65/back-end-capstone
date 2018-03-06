using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PupPals.Models.HouseViewModels
{
    public class HouseListViewModel
    {
        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [StringLength(2)]
        public string State { get; set; }

        public string ZipCode { get; set; }

        [Required]
        public bool IsResidence { get; set; }

        [Required]
        public bool Avoid { get; set; }

        public string Notes { get; set; }

        List<Pet> PetList { get; set; }

        List<Owner> OwnerList { get; set; }
    }
}
