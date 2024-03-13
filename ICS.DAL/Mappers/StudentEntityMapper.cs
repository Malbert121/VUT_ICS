
namespace ICS.DAL.Mappers
{
    public class StudentEntityMapper : IEntityMapper<StudentEntity>
    {
        public void MapToExistingEntity(StudentEntity existingEntity, StudentEntity newEntity)
        {
            existingEntity.studentId = newEntity.studentId;
            existingEntity.firstName = newEntity.firstName;
            existingEntity.lastName = newEntity.lastName;
            existingEntity.fotoURL = newEntity.fotoURL;

            existingEntity.subjects.Clear();
            foreach (var subject in newEntity.subjects)
            {
                existingEntity.subjects.Add(subject);
            }
        }
    }
}