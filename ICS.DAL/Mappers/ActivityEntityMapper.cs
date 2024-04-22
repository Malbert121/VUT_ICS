using ICS.DAL.Entities;

namespace ICS.DAL.Mappers
{
    public class ActivityEntityMapper : IEntityMapper<ActivityEntity>
    {
         public void MapToExistingEntity(ActivityEntity existingEntity, ActivityEntity newEntity)
        {
            existingEntity.Name = newEntity.Name;
            existingEntity.Start = newEntity.Start;
            existingEntity.End = newEntity.End;
            existingEntity.Room = newEntity.Room;
            existingEntity.ActivityTypeTag = newEntity.ActivityTypeTag;
            existingEntity.Description = newEntity.Description;
            existingEntity.SubjectId = newEntity.SubjectId;
        }
    }
}
