using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class FilterMovieDTO
    {
        public int Page { get; set; } = 1;
        public int RecordPerPage { get; set; } = 10;

        public PaginationDTO Pagination
        {
            get { return new PaginationDTO() { Page = Page, RecordsPerPage = RecordPerPage }; }
        }

        public string Title { get; set; }
        public int GenreId { get; set; }
        public bool InTheaters { get; set; }
        public bool UpcomingReleases { get; set; }
        public string OrderingField { get; set; }
        public bool Ascending { get; set; } = true;
    }
}