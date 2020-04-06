﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vets.Controllers.Data;
using Vets.Models;

namespace Vets.Controllers
{
    public class DonosController : Controller
    {
        private readonly VetsDB db;

        public DonosController(VetsDB context)
        {
            db = context;
        }

        // GET: Donos
        public async Task<IActionResult> Index()
        {
            //em sql, db.Donos.ToListAsync() significa SELECT * FROM Donos; 
            return View(await db.Donos.ToListAsync());
        }

        // GET: Donos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donos = await db.Donos
                .FirstOrDefaultAsync(m => m.ID == id);
            if (donos == null)
            {
                return NotFound();
            }

            return View(donos);
        }

        // GET: Donos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Donos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome,NIF")] Donos dono)
        {
            if (ModelState.IsValid)
            {
                db.Add(dono);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dono);
        }

        // GET: Donos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dono = await db.Donos.FindAsync(id);
            if (dono == null)
            {
                return NotFound();
            }
            return View(dono);
        }

        // POST: Donos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nome,NIF")] Donos donos)
        {
            if (id != donos.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(donos);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonosExists(donos.ID))
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
            return View(donos);
        }

        // GET: Donos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donos = await db.Donos
                .FirstOrDefaultAsync(m => m.ID == id);
            if (donos == null)
            {
                return NotFound();
            }

            return View(donos);
        }

        // POST: Donos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donos = await db.Donos.FindAsync(id);
            db.Donos.Remove(donos);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonosExists(int id)
        {
            return db.Donos.Any(e => e.ID == id);
        }
    }
}
