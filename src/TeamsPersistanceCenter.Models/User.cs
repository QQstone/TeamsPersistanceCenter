using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamsPersistanceCenter.Models
{

    [Table("User")]
    public class User
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string Num { get; set; }
    }
}
