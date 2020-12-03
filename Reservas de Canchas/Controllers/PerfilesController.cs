using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reservas_de_Canchas.Models;

namespace Reservas_de_Canchas.Controllers
{
    public class PerfilesController : Controller
    {

        
        public IActionResult Index(Cliente cli)
        {
            return View(cli);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            using (var db = new EmpresaContext())
            {
                var cliente = await db.Cliente.FindAsync(id);
                if (cliente == null)
                {
                    return NotFound();
                }
                return View(cliente);
            }
            
        }

        // POST: Perfiles1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Email,Contraseña,Nombre,Puntos")] Cliente cliente)
        {
            if (id != cliente.Email)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var db = new EmpresaContext())
                {

                    try
                    {
                        Cliente aux = db.Cliente.Find(id);
                        aux.Contraseña = cliente.Contraseña;
                        aux.Nombre = cliente.Nombre;
                        
                        //db.Update(cliente);
                        await db.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        
                    }
                }
                return RedirectToAction("Acceder", "Home");
            }
            return View(cliente);
        }



        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    using (var db = new EmpresaContext())
        //    {
        //        var cliente = await db.Cliente.FindAsync(id);
        //        if (cliente == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(cliente);
        //    }
           
        //}

        //// POST: Perfiles1/Delete/5
        ////[ValidateAntiForgeryToken]
        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeleteConfirmed(string id)
        //{
        //    using (var db = new EmpresaContext())
        //    {
        //        var cliente = db.Cliente.Find(id);
        //        db.Cliente.Remove(cliente);
        //        db.SaveChanges();
        //        return RedirectToAction("Acceder", "Home");
        //    }
            
        //}






        //public IActionResult Edit(string email)
        //{
        //    try
        //    {
        //        using (var db = new EmpresaContext())
        //        {
        //            Cliente c = db.Cliente.Find(email);
        //            return View(c);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //[HttpPost]
        //public IActionResult Edit(string email, Cliente cli)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            using (var db = new EmpresaContext())
        //            {
        //                db.Cliente.Update(cli);
        //                db.SaveChanges();
        //                return RedirectToAction("Index", "Perfiles", cli);
        //            }
        //        }
        //        catch (Exception)
        //        {

        //            throw;
        //        }
        //    }
        //    else
        //    {
        //        return View();
        //    }


        //}

    }
}
