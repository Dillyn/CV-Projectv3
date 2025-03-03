namespace Domain.Models.Education
{
    public class UserEducationEntity
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public int Education_id { get; set; }
        public string Start_date { get; set; }
        public string End_date { get; set; }
    }
}
