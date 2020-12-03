using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reservas_de_Canchas.Models;

namespace Reservas_de_Canchas.Controllers
{
    public class ClientesController : Controller
    {
        public IActionResult Index()
        {
            using (var db = new EmpresaContext())
            {
                List<Cliente> lista = db.Cliente.ToList();
                return View(lista);
            }
               
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cliente cli)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var db = new EmpresaContext())
                {
                    if(!db.Cliente.Any(c => c.Email == cli.Email))
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
            catch (Exception )
            {
                return View();
            }

        }



        public IActionResult Details(string email)
        {
            try
            {
                using (var db = new EmpresaContext())
                {
                    return View(db.Cliente.Find(email));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }




        

    }
}
