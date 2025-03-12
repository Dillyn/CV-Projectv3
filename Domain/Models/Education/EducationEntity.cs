using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Education
{
    public class EducationEntity
    {
        [Key]
        public int Id { get; set; }

        public string Institution { get; set; }

        public string Qualification { get; set; }

        // Navigation property for related UserEducation
        public ICollection<UserEducationEntity> UserEducations { get; set; }
    }
}
