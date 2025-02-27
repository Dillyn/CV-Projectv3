using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class UserHobbyEntity
    {
        public int Id { get; set; }
        [RegularExpression(@"^[^ ].*[^ ]$", ErrorMessage = "Content must not have leading or trailing spaces.")]
        public int user_id { get; set; }

        [RegularExpression(@"^[^ ].*[^ ]$", ErrorMessage = "Content must not have leading or trailing spaces.")]
        public int hobby_id { get; set; }
        public string description { get; set; }

        // Foreign key
        //Navigation property
    }
}
