using Microsoft.AspNetCore.Mvc.Rendering;
using PupPals.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PupPals.Models.OwnerViewModels
{
    public class OwnerCreateViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a house")]
        public int HouseId { get; set; }

        public List<SelectListItem> HouseList { get; set; }
        public House House { get; set; }

        public OwnerCreateViewModel(ApplicationDbContext ctx, ApplicationUser usr)
        {
            //creates a dropdown list of the houses that the user has added
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
    }
}
