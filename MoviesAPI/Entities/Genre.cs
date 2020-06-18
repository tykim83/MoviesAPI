using MoviesAPI.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Entities
{
    public class Genre //: IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [FirstLetterUppercaseAttribute]
        public string Name { get; set; }

        public List<MoviesGeneres> MoviesGeneres { get; set; }
    }
}