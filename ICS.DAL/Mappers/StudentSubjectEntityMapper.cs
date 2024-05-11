using ICS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICS.DAL.Mappers
{
    public class StudentSubjectEntityMapper : IEntityMapper<StudentSubjectEntity>
    {
        public void MapToExistingEntity(StudentSubjectEntity existingEntity, StudentSubjectEntity newEntity)
        {
            existingEntity.StudentId = newEntity.StudentId;
            existingEntity.SubjectId = newEntity.SubjectId;
        }
    }
}
