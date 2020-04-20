using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage ="O Nome é de preenchimento obrigatório")]
        [StringLength(40,ErrorMessage ="O {0} não pode ter mais de {1} carateres")]
        public string Nome { get; set; }



        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")] //o parametro {0} representa o nome do atributo
        [StringLength(9,MinimumLength = 9 ,ErrorMessage = "Deve escrever exatamente {1} algarismos")]
        [RegularExpression("[12567][0-9]{8}",ErrorMessage ="Deve escrever um nº, com 9 algarismos, começando por 1,2,5,6 ou 7")]//[]- valores possiveis {}- numero de ocorrencias
        public string NIF { get; set; }

        //Lista de Animais de um determinado dono
        public ICollection<Animais> Animais { get; set; }

    }
}
