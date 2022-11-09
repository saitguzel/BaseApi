namespace Base.Model.Entities
{
    public class BaseEntity
    {

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime? LastUpdatedOn { get; set; }
        public long? LastUpdatedByUserID { get; set; }
    }
}
