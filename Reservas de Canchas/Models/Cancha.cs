using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reservas_de_Canchas.Models
{
    public partial class Cancha
    {
        public Cancha()
        {
            Turno = new HashSet<Turno>();
        }

        public int NroCancha { get; set; }

        [Required]
        [Display(Name = "Nombre de la cancha")]
        public string NombreCancha { get; set; }

        [Required]
        [Display (Name = "Estado")]
        public bool Habilitada { get; set; }

        [Required]
        public double Importe { get; set; }

        public virtual ICollection<Turno> Turno { get; set; }
    }
}
