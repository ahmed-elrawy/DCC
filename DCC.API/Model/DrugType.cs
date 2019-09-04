using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DCC.API.Model
{
    public class DrugType
    {
        [Key]
        public int DrugTypeId { get; set; }
        public string DrugTypeName { get; set; }
        public ICollection<Drug> Drug { get; set; }
     }
}