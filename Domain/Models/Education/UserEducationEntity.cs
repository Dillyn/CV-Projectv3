namespace Domain.Models.Education
{
    public class UserEducationEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // Foreign key to User
        public UserEntity User { get; set; }

        public int EducationId { get; set; }  // Foreign key to Hobby
        public EducationEntity Education { get; set; }
        public string Start_date { get; set; }
        public string End_date { get; set; }
    }
}
