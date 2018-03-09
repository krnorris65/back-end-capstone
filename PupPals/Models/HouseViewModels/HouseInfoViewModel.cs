using PupPals.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PupPals.Models.HouseViewModels
{
    public class HouseInfoViewModel
    {
        public int Id { get; set; }

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

        public List<Pet> PetList { get; set; }

        public List<Owner> OwnerList { get; set; }

        public HouseInfoViewModel(ApplicationDbContext ctx, ApplicationUser usr, House _house)
        {
            this.Id = _house.Id;
            this.Address = _house.Address;
            this.City = _house.City;
            this.State = _house.State;
            this.ZipCode = _house.ZipCode;
            this.IsResidence = _house.IsResidence;
            this.Avoid = _house.Avoid;
            this.Notes = _house.Notes;

            this.PetList = ctx.Pet.Where(p => p.HouseId == _house.Id).ToList();

        }
    }
}
