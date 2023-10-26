using Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Role : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }
    }
}
