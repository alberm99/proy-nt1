using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Reservas_de_Canchas.Models;

namespace Reservas_de_Canchas.Controllers
{
    public class CanchasController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                using (var db = new EmpresaContext())
                {
                    return View(db.Cancha.ToList());
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Cancha c)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var db = new EmpresaContext())
                {
                    if (!db.Cancha.Any(a => a.NombreCancha == c.NombreCancha))
                    {
                        db.Cancha.Add(c);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ya existe una cancha con ese nombre");
                        return View();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        public IActionResult Edit(int nroCancha)
        {
            try
            {
                using (var db = new EmpresaContext())
                {
                    return View(db.Cancha.Find(nroCancha));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit (int id, Cancha c)
        {
            if (!ModelState.IsValid)
            {
                return View(c);
            }

            try
            {
                using (var db = new EmpresaContext())
                {                    
                    if(c.Importe > 0)
                    {
                        db.Update(c);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "El importe debe ser positivo");
                        return View();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
