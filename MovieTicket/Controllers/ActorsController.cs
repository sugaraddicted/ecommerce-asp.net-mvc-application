﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MovieTicket.Data.Static;
using MovieTicket.Data.Services;
using MovieTicket.Models;

namespace MovieTicket.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class ActorsController : Controller
    {

        private readonly IActorsService _actorsService;

        public ActorsController(IActorsService actorsService)
        {

            _actorsService = actorsService;

        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allActors = await _actorsService.GetAllAsync();
            return View(allActors);
        }

        //Get: Actors/Create
        public async Task<IActionResult> Create()
        {
            return View();  
        }

        [HttpPost]

        public async Task<IActionResult> Create([Bind("FullName, ProfilePictureURL, Bio ")]Actor actor)
        {

            if (!ModelState.IsValid)
            {
                return View(actor);
            }

            await _actorsService.AddAsync(actor);

            return RedirectToAction(nameof(Index));

        }

        //Get: Actors/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var actorDetails = await _actorsService.GetByIdAsync(id);

            if (actorDetails == null) return View("NotFound");
            return View(actorDetails);
        }

        //Get: Actors/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var actorDetails = await _actorsService.GetByIdAsync(id);
            if (actorDetails == null) return View("NotFound");
            return View(actorDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ActorId,FullName,ProfilePictureURL,Bio")] Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return View(actor);
            }
            await _actorsService.UpdateAsync(id, actor);
            return RedirectToAction(nameof(Index));
        }


        //Get: Actors/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var actorDetails = await _actorsService.GetByIdAsync(id);
            if (actorDetails == null) return View("NotFound");
            return View(actorDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actorDetails = await _actorsService.GetByIdAsync(id);
            if (actorDetails == null) return View("NotFound");

            await _actorsService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
