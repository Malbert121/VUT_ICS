
namespace ICS.DAL.Mappers
{
    public class SubjectEntityMapper : IEntityMapper<SubjectEntity>
    {
        public void MapToExistingEntity(SubjectEntity existingEntity, SubjectEntity newEntity)
        {
            existingEntity.name = newEntity.name;
            existingEntity.abbreviation = newEntity.abbreviation;

            existingEntity.activity.Clear();
            foreach (var activity in newEntity.activity)
            {
                existingEntity.activity.Add(activity);
            }

            existingEntity.students.Clear();
            foreach (var student in newEntity.students)
            {
                existingEntity.students.Add(student);
            }
        }
    }
}