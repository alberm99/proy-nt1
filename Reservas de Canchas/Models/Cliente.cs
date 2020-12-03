using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reservas_de_Canchas.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Turno = new HashSet<Turno>();
        }

        [Required]
        public string Email { get; set; }
        
        public string Contraseña { get; set; }
        [Required]
        public string Nombre { get; set; }
        public int Puntos { get; set; }

        public virtual ICollection<Turno> Turno { get; set; }
    }
}
