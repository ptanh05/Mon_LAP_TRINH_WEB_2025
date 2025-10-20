using System.Collections.Generic;

namespace StudentManagement.Models
{
    public class StudentListViewModel
    {
        public IReadOnlyList<Student> Students { get; set; } = new List<Student>();

        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public string? SortDir { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }

        public int TotalPages => PageSize == 0 ? 1 : (int)System.Math.Ceiling((double)TotalItems / PageSize);
    }
}


