using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Reservas_de_Canchas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Session;

namespace Reservas_de_Canchas.Controllers
{
    public class UsuariosController : Controller
    {
        //private readonly EmpresaContext _context;

        //public UsuariosController(EmpresaContext context)
        //{
        //    _context = context;
        //}

        public ActionResult Index(Cliente cli)
        {
            try
            {
                
                using (var db = new EmpresaContext())
                {
                    TempData["EmailUsuario"] = cli.Email;
                    var lista = db.Turno.Include(t => t.EmailClienteNavigation).Include(t => t.NroCanchaNavigation);
                    return View(lista.Where(a => a.EmailCliente == cli.Email).ToList());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult Reservar()
        {
            using (var db = new EmpresaContext())
            {
                //ViewData["NroCancha"] = new SelectList(db.Cancha, "NroCancha", "NombreCancha");
                return View();
            }
        }


        [HttpPost]
        public ActionResult Reservar(Turno t)
        {
            
            try
            {
                using (var db = new EmpresaContext())
                {
                    if (db.Cliente.Any(c => c.Email == t.EmailCliente))
                    {
                        Cancha cancha = db.Cancha.Find(t.NroCancha);
                        //validar fecha no pasada
                        if (cancha.Habilitada)
                        {


                            if (!this.TurnoNoDisponible(t))
                            {

                                t.NroCanchaNavigation = cancha;
                                Cliente cli = db.Cliente.Find(t.EmailCliente);
                                t.EmailClienteNavigation = cli;
                                db.Turno.Add(t);
                                cli.Puntos += 100;
                                cli.Turno.Add(t);
                                db.SaveChanges();
                                return RedirectToAction("Index", cli);
                            }
                            else
                            {
                                ModelState.AddModelError("", "Turno no disponible");
                                return View();
                            }

                        }
                        else
                        {
                            ModelState.AddModelError("", "La cancha no esta disponible");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email inexistente");
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
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                using (var db = new EmpresaContext())
                {
                    var turno = await db.Turno.FindAsync(id);
                    //Turno t = db.Turno.Find(id);
                    return View(turno);
                }
            }
            catch (Exception)
            {
                throw;
            }

            //if (id == null)
            //{
            //    return NotFound();
            //}
            //using (var db = new EmpresaContext())
            //{
            //    var turno = await db.Turno.FindAsync(id);
            //    if (turno == null)
            //    {
            //        return NotFound();
            //    }
            //    ViewData["EmailCliente"] = new SelectList(db.Cliente, "Email", "Email", turno.EmailCliente);
            //    ViewData["NroCancha"] = new SelectList(db.Cancha, "NroCancha", "NombreCancha", turno.NroCancha);
            //    return View(turno);
            //}

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("NroTurno,EmailCliente,NroCancha,FechaHora")] Turno t)
        {
            try
            {
                using (var db = new EmpresaContext())
                {

                    Cancha cancha = db.Cancha.Find(t.NroCancha);
                    //validar fecha no pasada
                    if (cancha.Habilitada)
                    {

                         
                        if (!this.TurnoNoDisponible(t))
                        {

                            Turno turno = db.Turno.Find(id);
                            turno.FechaHora = t.FechaHora;
                            turno.EmailCliente = t.EmailCliente;
                            turno.NroCancha = t.NroCancha;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Turno no disponible");
                            return View();

                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "La cancha no esta disponible");
                        return View();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

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
        //        {   using (var db = new EmpresaContext())
        //            {
        //                db.Update(turno);
        //                await db.SaveChangesAsync();
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
        //    using (var db = new EmpresaContext())
        //    {
        //        ViewData["EmailCliente"] = new SelectList(db.Cliente, "Email", "Email", turno.EmailCliente);
        //        ViewData["NroCancha"] = new SelectList(db.Cancha, "NroCancha", "NombreCancha", turno.NroCancha);
        //        return View(turno);
        //    }
        //}


        public ActionResult Detalles(int NroTurno)
        {
            try
            {
                using (var db = new EmpresaContext())
                {
                    return View(db.Turno.Find(NroTurno));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult ListarCanchas()
        {
            try
            {
                using (var db = new EmpresaContext())
                {
                    return PartialView(db.Cancha.ToList());
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool TurnoNoDisponible(Turno t)
        {
            bool TurnoDips = false;
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
                                TurnoDips = true;
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

        //private bool TurnoExists(int id)
        //{
        //    using (var db = new EmpresaContext())
        //    {
        //        return db.Turno.Any(e => e.NroTurno == id);
                
        //    }
        //}
    }



    //    @{
    //    ViewData["Title"] = "Inicio de Seción";
    //}

    //@*<div class="text-center">
    //    <h1 class="display-4">Welcome</h1>
    //    <p>Learn about<a href="https://docs.microsoft.com/aspnet/core">building Web apps with ASP.NET Core</a>.</p>
    //</div>*@



//    @model Reservas_de_Canchas.Models.Turno

//@{
//        ViewData["Title"] = "Reservar";

//    }

//<h1>Reservar</h1>

//<h4>Turno</h4>
//<hr />
//<div class="row">
//    <div class="col-md-4">
//        <form asp-action="Reservar">
//            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
//            <div class="form-group">
//                <label asp-for="EmailCliente" class="control-label"></label>
//                @*<select asp-for="EmailCliente" class ="form-control" asp-items="ViewBag.EmailCliente"></select>*@
//                <input asp-for="EmailCliente" class="form-control" type="email" value=""/>
//                <span asp-validation-for="EmailCliente" class="text-danger"></span>
//            </div>
//            <div class="form-group">
//                <label asp-for="NroCancha" class="control-label"></label>
//                @*<select asp-for="NroCancha" class ="form-control" asp-items="ViewBag.NroCanchaNavigation"></select>*@
               
//            <select asp-for="NroCancha" class="form-control">
//                @using(var db = new EmpresaContext())
//    {
//        @foreach(var item in db.Cancha.ToList())
//                    {
//                        < option value = "@item.NroCancha" > "@item.NombreCancha" </ option >
//                    }
//    }
//            </select>
//                <span asp-validation-for="NroCancha" class="text-danger"></span>
//            </div>
//            <div class="form-group">
//                <label asp-for="FechaHora" class="control-label"></label>
//                <input asp-for="FechaHora" class="form-control" />
//                <span asp-validation-for="FechaHora" class="text-danger"></span>
//            </div>
//            <div class="form-group">
//                <input type = "submit" value="Reservar" class="btn btn-primary" />
//            </div>
//        </form>
//    </div>
//</div>

//<div>
//    <a asp-action="Index">Back to List</a>
//</div>

//@section Scripts
//    {
//    @{ await Html.RenderPartialAsync("_ValidationScriptsPartial"); }
//    }


}
