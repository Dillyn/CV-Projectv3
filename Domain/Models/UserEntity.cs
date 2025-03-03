using System.ComponentModel.DataAnnotations;
using Domain.Models.Hobby;

namespace Domain.Models
{
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        // Navigation property for related Hobbies
        public ICollection<UserHobbyEntity> UserHobbies { get; set; }
    }
}
