﻿using MoviesAPI.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class GenreCreationDTO
    {
        [Required]
        [StringLength(20)]
        [FirstLetterUppercaseAttribute]
        public string Name { get; set; }
    }
}