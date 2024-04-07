using ICS.DAL.Entities;

namespace ICS.DAL.Mappers
{
    public class SubjectEntityMapper : IEntityMapper<SubjectEntity>
    {
        public void MapToExistingEntity(SubjectEntity existingEntity, SubjectEntity newEntity)
        {
            existingEntity.Name = newEntity.Name;
            existingEntity.Abbreviation = newEntity.Abbreviation;

            existingEntity.Activity.Clear();
            foreach (var activity in newEntity.Activity)
            {
                existingEntity.Activity.Add(activity);
            }

            existingEntity.Students.Clear();
            foreach (var student in newEntity.Students)
            {
                existingEntity.Students.Add(student);
            }
        }
    }
}