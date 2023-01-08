using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Models.Admin
{
    public class ListUserModel
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public bool IsAdmin { get; set; }
    }
}
