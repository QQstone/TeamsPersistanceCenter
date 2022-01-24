using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamsPersistanceCenter.Models
{

    public class User
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string Num { get; set; }
    }
}
