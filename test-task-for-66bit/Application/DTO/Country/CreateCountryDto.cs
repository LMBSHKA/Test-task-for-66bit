using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Country
{
    public class CreateCountryDto
    {
        [Required]
        public string Name { get; set; }
    }
}
