using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Hobby
{
    public class HobbyEntity
    {
        [Key]
        public int Hobby_Id { get; set; }

        public string Title { get; set; }


        public string Image { get; set; }

        // Navigation property for related UserHobbies
        public ICollection<UserHobbyEntity> UserHobbies { get; set; }
    }
}
