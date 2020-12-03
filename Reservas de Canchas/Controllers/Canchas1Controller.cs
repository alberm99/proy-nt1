using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Reservas_de_Canchas.Models;

namespace Reservas_de_Canchas.Controllers
{
    public class Canchas1Controller : Controller
    {
        private readonly EmpresaContext _context;

        public Canchas1Controller(EmpresaContext context)
        {
            _context = context;
        }

        // GET: Canchas1
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cancha.ToListAsync());
        }

        // GET: Canchas1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cancha = await _context.Cancha
                .FirstOrDefaultAsync(m => m.NroCancha == id);
            if (cancha == null)
            {
                return NotFound();
            }

            return View(cancha);
        }

        // GET: Canchas1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Canchas1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NroCancha,NombreCancha,Habilitada,Importe")] Cancha cancha)
        {
            if (ModelState.IsValid)
            {
                if (!_context.Cancha.Any(a => a.NombreCancha == cancha.NombreCancha))
                {
                    if(cancha.Importe > 0)
                    {
                        _context.Add(cancha);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "El importe debe ser positivo");
                        return View();
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "Ya existe una cancha con ese nombre");
                    return View();
                }
                    
            }
            return View(cancha);
        }

        // GET: Canchas1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cancha = await _context.Cancha.FindAsync(id);
            if (cancha == null)
            {
                return NotFound();
            }
            return View(cancha);
        }

        // POST: Canchas1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NroCancha,NombreCancha,Habilitada,Importe")] Cancha cancha)
        {
            if (id != cancha.NroCancha)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (cancha.Importe > 0)
                    {

                        if (_context.Cancha.Any(c => c.NombreCancha == cancha.NombreCancha))
                        {
                            Cancha aux = _context.Cancha.Find(id);
                            if(aux.NombreCancha == cancha.NombreCancha)
                            {
                                //_context.Update(cancha);
                                aux.NombreCancha = cancha.NombreCancha;
                                aux.Habilitada = cancha.Habilitada;
                                aux.Importe = cancha.Importe;
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                ModelState.AddModelError("", "Ya existe otra cancha con ese nombre");
                                return View();
                            }
                        }
                        else
                        {
                            _context.Update(cancha);
                            await _context.SaveChangesAsync();
                        }
                        
                    }
                    else
                    {
                        ModelState.AddModelError("", "El importe debe ser positivo");
                        return View();
                    }
                       
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CanchaExists(cancha.NroCancha))
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
            return View(cancha);
        }

        // GET: Canchas1/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var cancha = await _context.Cancha
        //        .FirstOrDefaultAsync(m => m.NroCancha == id);
        //    if (cancha == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(cancha);
        //}

        //// POST: Canchas1/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var cancha = await _context.Cancha.FindAsync(id);
        //    _context.Cancha.Remove(cancha);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool CanchaExists(int id)
        {
            return _context.Cancha.Any(e => e.NroCancha == id);
        }
    }
}
