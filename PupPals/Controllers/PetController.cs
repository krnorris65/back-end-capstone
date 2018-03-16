﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PupPals.Data;
using PupPals.Models;
using PupPals.Models.PetViewModels;

namespace PupPals.Controllers
{
    public class PetController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PetController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        // This task retrieves the currently authenticated user
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        // GET: Pet
        public async Task<IActionResult> Index()
        {
            ApplicationUser _user = await GetCurrentUserAsync();

            PetListViewModel petList = new PetListViewModel(_context, _user);
            return View(petList);
        }

        // GET: Pet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pet
                .Include(p => p.House)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // GET: Pet/Create
        public IActionResult Create(int houseId)
        {
            Pet pet = new Pet { HouseId = houseId };
            return View(pet);
        }

        // POST: Pet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pet pet, IFormFile file)
        {

            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                ApplicationUser user = await GetCurrentUserAsync();
                pet.User = user;

                //if photo was added, upload it and add it to the pet
                if(file != null)
                {

                    //specify the filepath
                    var upload = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                    //store the relative filepath in the database for use as the src of img in view
                    pet.Photo = Path.Combine(
                        "images/",
                        file.FileName
                    );

                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(upload, file.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                }


                _context.Add(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), "House", new { id = pet.HouseId});
            }

            return View(pet);
        }

        // GET: Pet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _pet = await _context.Pet.SingleOrDefaultAsync(m => m.Id == id);
            if (_pet == null)
            {
                return NotFound();
            }

            //gets the current user
            ApplicationUser _user = await GetCurrentUserAsync();

            //displays pet info and list of houses in a drop down
            PetEditViewModel petEditView = new PetEditViewModel(_context, _user, _pet);

            return View(petEditView);
        }

        // POST: Pet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pet pet, IFormFile file)
        {
            if (id != pet.Id)
            {
                return NotFound();
            }

            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser user = await GetCurrentUserAsync();
                    pet.User = user;

                    //if photo was added, upload it and add it to the pet
                    if (file != null)
                    {

                        //specify the filepath
                        var upload = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                        //store the relative filepath in the database for use as the src of img in view
                        pet.Photo = Path.Combine(
                            "images/",
                            file.FileName
                        );

                        if (file.Length > 0)
                        {
                            var filePath = Path.Combine(upload, file.FileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                        }
                    }
                    _context.Update(pet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetExists(pet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), "House", new { id = pet.HouseId });
            }
            //ViewData["HouseId"] = new SelectList(_context.House, "Id", "Address", pet.HouseId);
            return View(pet);
        }

        // GET: Pet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pet
                .Include(p => p.House)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // POST: Pet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pet = await _context.Pet.SingleOrDefaultAsync(m => m.Id == id);
            _context.Pet.Remove(pet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), "House", new { id = pet.HouseId });
        }

        private bool PetExists(int id)
        {
            return _context.Pet.Any(e => e.Id == id);
        }
    }
}
