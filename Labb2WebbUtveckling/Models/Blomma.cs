﻿using System.ComponentModel.DataAnnotations;

namespace Labb2WebbUtveckling.Models
{
    public class Blomma
    {
          
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }
    }
}

