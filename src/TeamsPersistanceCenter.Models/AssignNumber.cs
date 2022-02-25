using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsPersistanceCenter.Models
{
    [Table("User")]
    public class AssignNumber
    {
        public string Num { get; set; }
        public byte isUsed { get; set; }
    }
}
