using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class HobbyEntity
    {
        public int Hobby_Id { get; set; }
        [RegularExpression(@"^[^ ].*[^ ]$", ErrorMessage = "Content must not have leading or trailing spaces.")]
        public string Title { get; set; } = "";

        [RegularExpression(@"^[^ ].*[^ ]$", ErrorMessage = "Content must not have leading or trailing spaces.")]
        public string Image { get; set; } = "";

        // Foreign key
        //Navigation property
    }
}
