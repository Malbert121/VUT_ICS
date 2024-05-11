using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICS.DAL.Entities
{
    public record StudentSubjectEntity : IEntity
    {
        public required Guid StudentId { get; set; }
        public required Guid SubjectId { get; set; }
        public required StudentEntity Student { get; init; }
        public  required SubjectEntity Subject { get; init; } 
        public required Guid Id { get; set; }

    }
}
