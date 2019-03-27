using Microsoft.AspNetCore.Mvc.Rendering;
using PupPals.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PupPals.Models.PetViewModels
{
    public class PetEditViewModel
    {
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Description { get; set; }



        public List<SelectListItem> HouseList { get; set; }
        public House House { get; set; }

        [Required]
        public bool MyPet { get; set; }

        public bool BestFriend { get; set; }

        public string Notes { get; set; }

        public string Photo { get; set; }

        public PetEditViewModel(ApplicationDbContext ctx, ApplicationUser usr, Pet _pet)
        {
            //creates a dropdown list of the houses that the user has added and preselects the house that is currently set for that pet
            this.HouseList = ctx.House
                                    .Where(h => h.User == usr)
                                    .AsEnumerable()
                                    .Select(li => new SelectListItem
                                    {
                                        Text = li.Address + ", " + li.City + ", " + li.State,
                                        Value = li.Id.ToString(),
                                        Selected = li.Id == _pet.HouseId ? true : false
                                    }).ToList();


            this.Name = _pet.Name;
            this.Type = _pet.Type;
            this.Description = _pet.Description;
            this.MyPet = _pet.MyPet;
            this.BestFriend = _pet.BestFriend;
            this.Notes = _pet.Notes;
            this.Photo = _pet.Photo;
            this.House = _pet.House;

    }
    }
}
