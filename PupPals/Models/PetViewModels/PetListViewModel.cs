using Microsoft.EntityFrameworkCore;
using PupPals.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PupPals.Models.PetViewModels
{
    public class PetListViewModel
    {
        public List<Pet> MyPets { get; set; }

        public List<Pet> BestFriends { get; set; }

        public PetListViewModel(ApplicationDbContext ctx, ApplicationUser usr) {

            this.MyPets = ctx.Pet
                    .Include(p => p.House)
                    .Where(p => p.User == usr && p.MyPet == true)
                    .ToList();

            this.BestFriends = ctx.Pet
                    .Include(p => p.House)
                    .Where(p => p.User == usr && p.BestFriend == true)
                    .ToList();

        }
    }
}
