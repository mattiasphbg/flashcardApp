using System;
using System.ComponentModel.DataAnnotations;

namespace flashcard.Shared.Models
{
    public class Flashcard
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Question { get; set; }

        [Required]
        [StringLength(1000)]
        public string Answer { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedDate { get; set; }
    }
}