using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vets.Models
{
    /// <summary>
    /// Descrição das consultas
    /// </summary>
    public class Consultas
    {
        [Key]
       public int ID { get; set; }

        public DateTime Data { get; set; }

        public string Observacoes { get; set; }


        //Fk para animais
        [ForeignKey(nameof(Animal))]
        public int AnimalFK { get; set; }  //Consulta -----> Animal
        public virtual  Animais Animal { get; set; }

        //FK para os Veterinários
        [ForeignKey(nameof(Veterinario))]
        public int VeterinarioFK { get; set; } //Consulta -----> Veterinario
        public virtual Veterinarios Veterinario { get; set; }


    }
}
