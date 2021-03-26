using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class StudentModels
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonRequired]
        [StringLength(50, ErrorMessage = "Your last name is too long")]
        [MaxWords(10, ErrorMessage = "There are too many words in {0}")]
        public String LastName { get; set; }
        [BsonRequired]
        [StringLength(50, ErrorMessage = "Your name is too long")]
        [MaxWords(10, ErrorMessage = "There are too many words in {0}")]
        public String FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public virtual ICollection<EnrollmentModels> Enrollments { get; set; }
    }

    public class MaxWordsAttribute : ValidationAttribute
    {
        public MaxWordsAttribute(int maxWords)
        {
            _maxWords = maxWords;
        }
        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var valueAsString = value.ToString();
                if (valueAsString.Split(' ').Length > _maxWords)
                {
                    return new ValidationResult("Too many words!");
                }
            }
            return ValidationResult.Success;
        }
        private readonly int _maxWords;
    }
}