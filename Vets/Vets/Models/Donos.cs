using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vets.Models
{
    public class Donos
    {
        public Donos()
        {
            Animais = new HashSet<Animais>();
        }
        public int ID { get; set; }

        public string Nome { get; set; }

        public string NIF { get; set; }

        //Lista de Animais de um determinado dono
        public ICollection<Animais> Animais { get; set; }

    }
}
