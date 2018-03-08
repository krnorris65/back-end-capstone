using Microsoft.AspNetCore.Mvc.Rendering;
using PupPals.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PupPals.Models.PetViewModels
{
    public class PetCreateViewModel
    {
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Description { get; set; }


        public List<SelectListItem> HouseList { get; set; }
        public House House { get; set; }

        public PetCreateViewModel(ApplicationDbContext ctx, ApplicationUser usr)
        {

            this.HouseList = ctx.House
                                    .Where(h => h.User == usr)
                                    .AsEnumerable()
                                    .Select(li => new SelectListItem
                                    {
                                        Text = li.Address + ", " + li.City + ", " + li.State,
                                        Value = li.Id.ToString()
                                    }).ToList();

            this.HouseList.Insert(0, new SelectListItem
            {
                Text = "select address",
                Value = "0"
            });
        }


        [Required]
        public bool MyPet { get; set; }

        public bool BestFriend { get; set; }

        public string Notes { get; set; }

        public string Photo { get; set; }
    }
}
