using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Models
{
    public class PaginationModel<T>
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "TotalCount must be at least 1.")]
        public int TotalCount { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be greater than 0.")]
        public int PageNumber { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100.")]
        public int PageSize { get; set; }

        [Required(ErrorMessage = "Data list must not be null.")]
        [MinLength(0, ErrorMessage = "Data must have at least 0 items.")]
        public List<T> Data { get; set; } = new List<T>();
    }
}
