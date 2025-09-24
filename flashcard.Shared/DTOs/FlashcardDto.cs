using System;
using System.ComponentModel.DataAnnotations;

namespace flashcard.Shared.DTOs
{

    public class FlashcardDto
    {
        public string Id { get; set; } // Använd string för Guid i frontend
        public string Question { get; set; }
        public string Answer { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }

    public class CreateFlashcardDto
    {
        [Required(ErrorMessage = "Question is required")]
        [StringLength(250, ErrorMessage = "Question cannot exceed 250 characters")]
        public string Question { get; set; }

        [Required(ErrorMessage = "Answer is required")]
        [StringLength(1000, ErrorMessage = "Answer cannot exceed 1000 characters")]
        public string Answer { get; set; }
    }

    public class UpdateFlashcardDto
    {
        [StringLength(250, ErrorMessage = "Question cannot exceed 250 characters")]
        public string Question { get; set; }

        [StringLength(1000, ErrorMessage = "Answer cannot exceed 1000 characters")]
        public string Answer { get; set; }
    }
}