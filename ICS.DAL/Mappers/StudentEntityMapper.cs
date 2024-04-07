using ICS.DAL.Entities;

namespace ICS.DAL.Mappers
{
    public class StudentEntityMapper : IEntityMapper<StudentEntity>
    {
        public void MapToExistingEntity(StudentEntity existingEntity, StudentEntity newEntity)
        {
            existingEntity.FirstName = newEntity.FirstName;
            existingEntity.LastName = newEntity.LastName;
            existingEntity.FotoUrl = newEntity.FotoUrl;

            existingEntity.Subjects.Clear();
            foreach (var subject in newEntity.Subjects)
            {
                existingEntity.Subjects.Add(subject);
            }
        }
    }
}