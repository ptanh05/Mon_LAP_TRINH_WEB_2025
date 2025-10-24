using System.ComponentModel.DataAnnotations;
namespace Pta_231220711_DE01.Models
{
    public class PtaComputer
    {
        public int PtaComId { get; set; }
        [Required(ErrorMessage ="ten may tinh khong duoc de trong")]
        public string PtaComName { get; set; } = string.Empty;

        [Required(ErrorMessage ="gia may tinh khong duoc de trong")]
        [Range(100, 5000, ErrorMessage ="gia phai tu 100 den 5000") ]
        public decimal PtaComPrice { get; set; }

        [Required(ErrorMessage ="hinh anh khong duoc de trong")]
        [RegularExpression(@"^.*\.(jpg|png|gif|tiff)$", ErrorMessage ="File phai co dinh dang jpg png gif tiff")]
        public string PtaComImage { get; set; } = string.Empty;

        public bool PtaStatus { get; set; }
    }
}
