using Microsoft.AspNetCore.Mvc.Rendering;
using PupPals.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PupPals.Models.OwnerViewModels
{
    public class OwnerEditViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }


        public List<SelectListItem> HouseList { get; set; }
        public House House { get; set; }

        public OwnerEditViewModel(ApplicationDbContext ctx, ApplicationUser usr, Owner _owner)
        {
            //creates a dropdown list of the houses that the user has added and preselects the house that is currently set for that owner
            this.HouseList = ctx.House
                                    .Where(h => h.User == usr)
                                    .AsEnumerable()
                                    .Select(li => new SelectListItem
                                    {
                                        Text = li.Address + ", " + li.City + ", " + li.State,
                                        Value = li.Id.ToString(),
                                        Selected = li.Id == _owner.HouseId ? true : false
                                    }).ToList();

            this.FirstName = _owner.FirstName;
            this.LastName = _owner.LastName;
            this.Phone = _owner.Phone;
            this.Email = _owner.Email;
            this.House = _owner.House;
        }
    }
}
