using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unidad2.Models
{
    public class Libro
    {
        //POCO: Plain Old C# Object
        public string Titulo { get; set; } = null!;
        public string Autor { get; set; } = null!;
        public string Portada { get; set; } = null!;
    }
}
