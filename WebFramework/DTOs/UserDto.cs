using Entities;
using System.ComponentModel.DataAnnotations;

namespace WebFramework.DTOs
{
    public class UserDto:IValidatableObject
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public GenderType Gender { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserName.Equals("test", StringComparison.OrdinalIgnoreCase))
                yield return new ValidationResult("The User Name Should Not Be Test");
            if (Password == "12345678")
                yield return new ValidationResult("Please Choose A Stronger Password");
        }
    }
}
