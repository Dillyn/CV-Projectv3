namespace Domain.Models.Contact

{
    public class ContactEntity
    {
        public int Contact_id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int User_id { get; set; }
    }
}
