using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exercise_M_Tech.Models
{
    public class Employee
    {
        [Key]
        public int ID { get; set; }

        [Column(TypeName = "varchar(75)")]
        [Display(Name = "Name")]
        [Required(ErrorMessage = "The field name is required")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(75)")]
        [Display(Name = "LastName")]
        [Required(ErrorMessage = "The field lastname is required")]
        public string LastName { get; set; }

        [Column(TypeName = "varchar(13)")]
        [Display(Name = "RFC")]
        [Required(ErrorMessage = "The field RFC is required")]
        public string RFC { get; set; }

        [Column(TypeName = "datetime")]
        [Display(Name = "BornDate")]
        public DateTime BornDate { get; set; }

        [Column(TypeName = "tinyint")]
        [Display(Name = "Status")]
        public EmployeeStatus Status { get; set; }
    }
    public enum EmployeeStatus
    {
        [DescriptionAttribute("NoSet")]
        NotSet = 1,
        [DescriptionAttribute("Active")]
        Active = 2,
        [DescriptionAttribute("Inactive")]
        Inactive = 3,
    }


}
