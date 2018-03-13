using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PupPals.Data;
using PupPals.Models;
using PupPals.Models.HouseViewModels;

namespace PupPals.Controllers
{
    [Authorize]
    public class HouseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HouseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // This task retrieves the currently authenticated user
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        // GET: House
        public async Task<IActionResult> Index()
        {
            //only lists houses that the current user has added to the system
            ApplicationUser user = await GetCurrentUserAsync();
            var userHouses = _context.House.Include(h => h.PetList).Include(h => h.OwnerList).Where(h => h.User == user);
            return View(await userHouses.ToListAsync());
        }

        // GET: House/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var house = await _context.House.Include(h => h.PetList).Include(h => h.OwnerList)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (house == null)
            {
                return NotFound();
            }

            ApplicationUser user = await GetCurrentUserAsync();

            return View(house);
        }

        // GET: House/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: House/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(House house)
        {
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                ApplicationUser user = await GetCurrentUserAsync();
                house.User = user;
                _context.Add(house);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(house);
        }

        // GET: House/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var house = await _context.House.SingleOrDefaultAsync(m => m.Id == id);
            if (house == null)
            {
                return NotFound();
            }
            return View(house);
        }

        // POST: House/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, House house)
        {
            if (id != house.Id)
            {
                return NotFound();
            }

            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser user = await GetCurrentUserAsync();
                    house.User = user;
                    _context.Update(house);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HouseExists(house.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(house);
        }

        // GET: House/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var house = await _context.House
                .SingleOrDefaultAsync(m => m.Id == id);
            if (house == null)
            {
                return NotFound();
            }

            return View(house);
        }

        // POST: House/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var house = await _context.House.SingleOrDefaultAsync(m => m.Id == id);
            _context.House.Remove(house);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HouseExists(int id)
        {
            return _context.House.Any(e => e.Id == id);
        }

        // GET: House
        public async Task<IActionResult> HouseList()
        {
            //only lists houses that the current user has added to the system
            ApplicationUser user = await GetCurrentUserAsync();
            var userHouses = _context.House.Where(h => h.User == user);
            return Json(userHouses);
        }
    }
}
