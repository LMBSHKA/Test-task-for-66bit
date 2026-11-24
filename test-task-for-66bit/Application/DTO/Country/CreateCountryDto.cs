using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Country
{
    public class CreateCountryDto
    {
        /// <summary>Название страны.</summary>
        [Required]
        public string Name { get; set; }
    }
}
