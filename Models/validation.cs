using System;

namespace pt1_mvc.Models
{
    public class validation
    {
        public validation(bool ok, string missatge) 
        { 
            this.ok = ok;
            this.missatge = missatge;
        }

        public bool ok
        {
            get;
            set;
        }

        public string missatge
        {
            get;
            set;
        }
    }
}
