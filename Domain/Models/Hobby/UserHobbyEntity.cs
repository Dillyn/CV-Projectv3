using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Hobby
{
    public class UserHobbyEntity
    {
        [Key]
        public int Id { get; set; }  // Unique primary key for the join table

        public int UserId { get; set; }  // Foreign key to User
        public UserEntity User { get; set; }

        public int HobbyId { get; set; }  // Foreign key to Hobby
        public HobbyEntity Hobby { get; set; }

        public string Description { get; set; }
    }
}
