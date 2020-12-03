using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reservas_de_Canchas.Models;

namespace Reservas_de_Canchas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Index(string email, string contraseña)
        {
            try
            {
                using (var db = new EmpresaContext())
                {
                    Cliente cli = db.Cliente.Find(email);
                    if(cli != null)
                    {
                        if(cli.Contraseña == contraseña)
                        {
                            return RedirectToAction("Index", "Usuarios1", new { cli.Email } , "");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Contraseña incorrecta");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se encontro el usuario");
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
        public IActionResult Acceder()
        {
            return View();
        }




        [HttpPost]
        public IActionResult Acceder(string email, string contraseña)
        {
            try
            {
                using (var db = new EmpresaContext())
                {
                    Cliente cli = db.Cliente.Find(email);
                    if (cli != null)
                    {
                        if (cli.Contraseña == contraseña)
                        {
                            return RedirectToAction("Index", "Perfiles", cli);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Contraseña incorrecta");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se encontro el usuario");
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
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registrar(Cliente cli)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                using (var db = new EmpresaContext())
                {
                    if (!db.Cliente.Any(c => c.Email == cli.Email))
                    {
                        cli.Puntos = 0;
                        db.Cliente.Add(cli);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ya existe ese usuario");
                        return View();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
