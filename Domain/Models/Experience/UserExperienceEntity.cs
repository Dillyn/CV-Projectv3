namespace Domain.Models.Experience
{
    public class UserExperienceEntity
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public int Experience_id { get; set; }
        public string Start_date { get; set; }
        public string End_date { get; set; }


    }
}
