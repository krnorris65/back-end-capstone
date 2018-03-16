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
using PupPals.Models.OwnerViewModels;

namespace PupPals.Controllers
{
    [Authorize]
    public class OwnerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public OwnerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // This task retrieves the currently authenticated user
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        // GET: Owner
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await GetCurrentUserAsync();
            //only return owners that are of houses that the user added
            var applicationDbContext = _context.Owner.Include(o => o.House).Where(o => o.House.User == user);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Owner/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplicationUser user = await GetCurrentUserAsync();

            var owner = await _context.Owner
                .Include(o => o.House)
                .Where(o => o.House.User == user)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // GET: Owner/Create
        public IActionResult Create(int houseId)
        {
            Owner owner = new Owner { HouseId = houseId };
            return View(owner);
        }

        // POST: Owner/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Owner owner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(owner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), "House", new { id = owner.HouseId });
            }

            return View(owner);
        }


        // GET: Owner/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplicationUser _user = await GetCurrentUserAsync();

            var owner = await _context.Owner
                .Include(o => o.House)
                .Where(o => o.House.User == _user)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (owner == null)
            {
                return NotFound();
            }

            OwnerEditViewModel editOwner = new OwnerEditViewModel(_context, _user, owner);
            return View(editOwner);
        }

        // POST: Owner/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Owner owner)
        {
            if (id != owner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(owner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnerExists(owner.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), "House", new { id = owner.HouseId }); ;
            }
            //ViewData["HouseId"] = new SelectList(_context.House, "Id", "Address", owner.HouseId);
            return View(owner);
        }

        // GET: Owner/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser _user = await GetCurrentUserAsync();

            var owner = await _context.Owner
                .Include(o => o.House)
                .Where(o => o.House.User == _user)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // POST: Owner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var owner = await _context.Owner.SingleOrDefaultAsync(m => m.Id == id);
            _context.Owner.Remove(owner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), "House", new { id = owner.HouseId });
        }

        private bool OwnerExists(int id)
        {
            return _context.Owner.Any(e => e.Id == id);
        }
    }
}
