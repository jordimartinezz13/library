using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

#nullable disable

namespace pt1_mvc.Models
{
    public partial class Client
    {
        public Client()
        {
            Comptes = new HashSet<Compte>();
        }

        public int Id { get; set; }

        [Required]
        public string Dni { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Nom { get; set; }

        [Required]
        public string Cognoms { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de naixement")]
        [Remote(action: "ValidarDataN", controller: "Client", 
            ErrorMessage = "La data de naixement ha de ser com a molt tard el dia d’avui")]
        public DateTime? DataN { get; set; }

        public virtual ICollection<Compte> Comptes { get; set; }

        public string FullName
        {
            get
            {
                return Nom + " " + Cognoms;
            }
        }

        public bool validarNom() 
        { 
            bool ok = true;
            ok = (Nom.Length >= 2) && (Nom.Length <= 20);
            
            return ok;
        }

        public bool validarDataN() 
        { 
            bool ok = true;
            DateTime dataReferencia = DateTime.Now;
            ok = DataN <= dataReferencia;
            
            return ok;
        }
        
        public validation validarClient()
        {
            validation validation = new validation(true, "");

            /*
            if (Dni == null) {
                validation.missatge = "El camp DNI és obligatori";
                validation.ok = false;
            }

            if (validation.ok && Nom == null) {
                validation.missatge = "El camp Nom és obligatori";
                validation.ok = false;
            }

            if (validation.ok && !validarNom()) {
                validation.missatge = "El nom ha de tenir una longitud mínima de 2 caracters i màxima de 20";
                validation.ok = false;                
            }

            if (validation.ok && Cognoms == null) {
                validation.missatge = "El camp Cognoms és obligatori";
                validation.ok = false;
            }

            if (validation.ok && !validarDataN())
            {
                validation.missatge = "La data de naixement no pot ser posterior al dia d'avui";
                validation.ok = false;
            }
            */

            return validation;
        }
    }
}
