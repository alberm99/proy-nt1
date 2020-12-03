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
    public class TurnosController : Controller
    {
        private readonly EmpresaContext _context;

        public TurnosController(EmpresaContext context)
        {
            _context = context;
        }

        // GET: Turnos
        public async Task<IActionResult> Index()
        {
            var lista = _context.Turno.Include(t => t.EmailClienteNavigation).Include(t => t.NroCanchaNavigation);
            return View(await lista.ToListAsync());

        }
        // GET: Turnos/Details/5



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turno
                .Include(t => t.EmailClienteNavigation)
                .Include(t => t.NroCanchaNavigation)
                .FirstOrDefaultAsync(m => m.NroTurno == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }




        // GET: Turnos/Create
        public IActionResult Create()
        {
            ViewData["EmailCliente"] = new SelectList(_context.Cliente, "Email", "Email");
            ViewData["NroCancha"] = new SelectList(_context.Cancha, "NroCancha", "NombreCancha");
            return View();
        }


        // POST: Turnos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NroTurno,EmailCliente,NroCancha,FechaHora")] Turno turno)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Cancha c = _context.Cancha.Find(turno.NroCancha);
                    if (c.Habilitada)
                    {
                        if (TurnoDisponible(turno))
                        {
                            _context.Add(turno);
                            Cliente cli = _context.Cliente.Find(turno.EmailCliente);
                            cli.Puntos += 100;
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError("", "El turno no está disponible");
                            return View();
                        }                     
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cancha Inhabilitada");
                        return View();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
               
            }
            ViewData["EmailCliente"] = new SelectList(_context.Cliente, "Email", "Email", turno.EmailCliente);
            ViewData["NroCancha"] = new SelectList(_context.Cancha, "NroCancha", "NombreCancha", turno.NroCancha);
            return View(turno);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turno
                .Include(t => t.EmailClienteNavigation)
                .Include(t => t.NroCanchaNavigation)
                .FirstOrDefaultAsync(m => m.NroTurno == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }


        // POST: Turnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var turno = await _context.Turno.FindAsync(id);
            _context.Turno.Remove(turno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Turnos/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var turno = await _context.Turno.FindAsync(id);
        //    if (turno == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["EmailCliente"] = new SelectList(_context.Cliente, "Email", "Email", turno.EmailCliente);
        //    ViewData["NroCancha"] = new SelectList(_context.Cancha, "NroCancha", "NombreCancha", turno.NroCancha);
        //    return View(turno);
        //}

        //// POST: Turnos/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("NroTurno,EmailCliente,NroCancha,FechaHora")] Turno turno)
        //{
        //    if (id != turno.NroTurno)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            Cancha c = _context.Cancha.Find(turno.NroCancha);
        //            if(c.Habilitada)
        //            {
        //                if (TurnoDisponible(turno))
        //                {    
        //                    _context.Update(turno);
        //                    await _context.SaveChangesAsync();
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("", "Turno no disponible");
        //                    return View();
        //                }
        //            }
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TurnoExists(turno.NroTurno))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["EmailCliente"] = new SelectList(_context.Cliente, "Email", "Email", turno.EmailCliente);
        //    ViewData["NroCancha"] = new SelectList(_context.Cancha, "NroCancha", "NombreCancha", turno.NroCancha);
        //    return View(turno);
        //}

        // GET: Turnos/Delete/5

        private bool TurnoExists(int id)
        {
            return _context.Turno.Any(e => e.NroTurno == id);
        }

        private bool TurnoDisponible(Turno t)
        {
            bool TurnoDips = true;
            try
            {
                using (var db = new EmpresaContext())
                {
                    foreach (Turno item in db.Turno)
                    {
                        if (item.FechaHora == t.FechaHora)
                        {
                            if (item.NroCancha == t.NroCancha)
                            {
                                TurnoDips = false;
                            }
                        }
                    }

                    return TurnoDips;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
