using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class CreateRoleVM
    {
        [Required]
        public string RoleName { get; set; }        

    }
}
