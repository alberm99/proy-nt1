using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reservas_de_Canchas.Models
{
    public partial class Turno
    {

       
        public int NroTurno { get; set; }

        [Required]
        [Display(Name = "Email del cliente")]
        public string EmailCliente { get; set; }


        [Required]
        [Display (Name ="Cancha")]
        public int NroCancha { get; set; }


        [Required]
        [Display(Name ="Fecha y Hora")]
        public DateTime FechaHora { get; set; }



        [Display(Name = "Email del cliente")]
        public virtual Cliente EmailClienteNavigation { get; set; }


        [Display(Name = "Cancha")]
        public virtual Cancha NroCanchaNavigation { get; set; }
    }
}
