using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lab06.Models;

namespace lab06.Controllers
{
    public class LearnerController : Controller
    {
        private readonly SchoolContext db;

        // Constructor để inject SchoolContext
        public LearnerController(SchoolContext context)
        {
            db = context;
        }

        // Khai báo biến toàn cục pageSize - số phần tử dữ liệu trên 1 trang
        private int pageSize = 3;

        // Action Index - hiển thị danh sách học viên với phân trang
        // mid: ID của chuyên ngành để lọc (optional)
        public IActionResult Index(int? mid)
        {
            // Lấy toàn bộ learners trong DbSet, chuyển về IQueryable<Learner> để query
            var learners = (IQueryable<Learner>)db.Learners.Include(m => m.Major);

            // Nếu có mid thì lọc learner theo mid (chuyên ngành)
            if (mid != null)
            {
                learners = (IQueryable<Learner>)db.Learners
                    .Where(l => l.MajorID == mid)
                    .Include(m => m.Major);
            }

            // Tính số trang
            int pageNum = (int)Math.Ceiling(learners.Count() / (float)pageSize);

            // Trả số trang về view để hiển thị nav-trang
            ViewBag.pageNum = pageNum;

            // Lấy dữ liệu trang đầu
            var result = learners.Take(pageSize).ToList();

            return View(result);
        }

        // Action LearnerFilter - xử lý lọc, tìm kiếm và phân trang bằng AJAX
        // mid: ID chuyên ngành để lọc (optional)
        // keyword: Từ khóa để tìm kiếm theo tên (optional)
        // pageIndex: Chỉ số trang hiện tại (optional)
        public IActionResult LearnerFilter(int? mid, string? keyword, int? pageIndex)
        {
            // Lấy toàn bộ learners trong DbSet chuyển về IQueryable<Learner> để query
            var learners = (IQueryable<Learner>)db.Learners;

            // Lấy chỉ số trang, nếu chỉ số trang null thì gán mặc định bằng 1
            int page = (int)(pageIndex == null || pageIndex <= 0 ? 1 : pageIndex);

            // Nếu có mid thì lọc learner theo mid (chuyên ngành)
            if (mid != null)
            {
                // Lọc
                learners = learners.Where(l => l.MajorID == mid);
                // Gửi mid về view để ghi lại trên nav-phân trang
                ViewBag.mid = mid;
            }

            // Nếu có keyword thì tìm kiếm theo tên
            if (keyword != null)
            {
                // Tìm kiếm (không phân biệt hoa thường)
                learners = learners.Where(l => l.FirstMidName.ToLower().Contains(keyword.ToLower()));
                // Gửi keyword về view để ghi trên nav-phân trang
                ViewBag.keyword = keyword;
            }

            // Tính số trang
            int pageNum = (int)Math.Ceiling(learners.Count() / (float)pageSize);

            // Gửi số trang về view để hiển thị nav-trang
            ViewBag.pageNum = pageNum;

            // Chọn dữ liệu trong trang hiện tại
            var result = learners
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .Include(m => m.Major);

            // Trả về PartialView với dữ liệu đã lọc và phân trang
            return PartialView("LearnerTable", result);
        }
    }
}

