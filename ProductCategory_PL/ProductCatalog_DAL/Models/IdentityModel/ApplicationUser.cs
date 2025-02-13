using Microsoft.AspNetCore.Identity;


namespace ProductCatalog_DAL.Models.IdentityModel
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string country { get; set; } = null!;
        public string? street { get; set; }
        public string? city { get; set; }
        public string? PersonalImage { get; set; }
    }
}
