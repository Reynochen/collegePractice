using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace collegePractice.Models
{
    public class User
    {
        [Key]
        public string Name { get; set; }
        public string email { get; set; }
        public string time { get; set; }
    }
}
