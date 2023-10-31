using Entities;
using System.ComponentModel.DataAnnotations;

namespace WebFramework.DTOs
{
    public class UserDto:IValidatableObject
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public GenderType Gender { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserName.Equals("test", StringComparison.OrdinalIgnoreCase))
                yield return new ValidationResult("The User Name Should Not Be Test",new[] {nameof(UserName)});
            if (Password == "12345678")
                yield return new ValidationResult("Please Choose A Stronger Password", new[] {nameof(Password)});
        }
    }
}
