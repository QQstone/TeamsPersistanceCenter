using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsPersistanceCenter.Models
{
    [Table("AssignNumber")]
    public class AssignNumber
    {
        [Key]
        public string Num { get; set; }
        public byte IsUsed { get; set; }
    }
}
