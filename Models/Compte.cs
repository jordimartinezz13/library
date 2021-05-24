using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace pt1_mvc.Models
{
    public partial class Compte
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Codi { get; set; }
        public int Saldo { get; set; }
        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        public bool validarCodi() 
        { 
            bool ok = true;
            ok = Codi.Length <= 10;
            
            return ok;
        }

        public validation validarCompte()
        {
            validation validation = new validation(true, "");

            /*
            if (Codi == null) {
                validation.missatge = "El camp codi és obligatori";
                validation.ok = false;
            }

            if (validation.ok && !validarCodi()) {
                validation.missatge = "El codi de compte ha de tenir una longitud màxima de 10 caracters";
                validation.ok = false;                
            }
            */

            return validation;
        }
    }
}
