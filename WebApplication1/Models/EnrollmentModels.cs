using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public enum Grade
    {
        A, B, C, D, E, F
    }
    public class EnrollmentModels
    {
        [BsonId]
        public ObjectId EnrollmentId { get; set; }
        [BsonId]
        public ObjectId CourseId { get; set; }
        [BsonId]
        public ObjectId StudentId { get; set; }
        public Grade? Grade { get; set; }

        public virtual CourseModels Course { get; set; }
        public virtual StudentModels Student { get; set; }
    }
}