using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vets.Models
{
    public class Donos
    {
        /// <summary>
        /// representa os dados de um dono
        /// </summary>
        public Donos()
        {
            Animais = new HashSet<Animais>();
        }
        /// <summary>
        /// Identificador do Dono. Será PK na tabela da BD
        /// </summary>
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage ="O Nome é de preenchimento obrigatório")]
        [StringLength(40,ErrorMessage ="O {0} não pode ter mais de {1} carateres")]
        public string Nome { get; set; }

        /// <summary>
        /// Identifica o sexo do 'Dono'
        /// </summary>
        [RegularExpression("[FMfm]",ErrorMessage ="Deve escrever F ou M no campo {0}")]
        [StringLength(1,MinimumLength =1)]
        public string Sexo { get; set; }



        /// <summary>
        /// Numero de Identificação Fiscal
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")] //o parametro {0} representa o nome do atributo
        [StringLength(9,MinimumLength = 9 ,ErrorMessage = "Deve escrever exatamente {1} algarismos")]
        [RegularExpression("[12567][0-9]{8}",ErrorMessage ="Deve escrever um nº, com 9 algarismos, começando por 1,2,5,6 ou 7")]//[]- valores possiveis {}- numero de ocorrencias
        public string NIF { get; set; }




        /// <summary>
        /// Lista de Animais de um determinado dono
        /// </summary>
        public ICollection<Animais> Animais { get; set; }

    }
}
