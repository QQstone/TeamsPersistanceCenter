using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsPersistanceCenter.Models
{
    [Table("Administrator")]
    public class Administrator
    {
        [Key]
        public string Id { get; set; }
        public string Email { get; set; }
        public int IsValid { get; set; }
    }
}
