namespace Domain.Models
{
    public class ContactEntity
    {
        public int contact_id { get; set; }
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public int User_id { get; set; }
    }
}
