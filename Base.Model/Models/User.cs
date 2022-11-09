using Base.Model.Entities;

namespace Base.Model.Models
{
    public partial class User : BaseEntity
    {
        public long UserID { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
