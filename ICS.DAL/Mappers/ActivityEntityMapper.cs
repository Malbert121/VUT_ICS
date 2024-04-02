using ICS.DAL.Entities;

namespace ICS.DAL.Mappers
{
    public class ActivityEntityMapper : IEntityMapper<ActivityEntity>
    {
         public void MapToExistingEntity(ActivityEntity existingEntity, ActivityEntity newEntity)
        {
            existingEntity.name = newEntity.name;
            existingEntity.start = newEntity.start;
            existingEntity.end = newEntity.end;
            existingEntity.room = newEntity.room;
            existingEntity.activityTypeTag = newEntity.activityTypeTag;
            existingEntity.description = newEntity.description;
            existingEntity.subjectId = newEntity.subjectId;
            existingEntity.ratings.Clear();
            foreach (var rating in newEntity.ratings)
            {
                existingEntity.ratings.Add(rating);
            }
        }
    }
}
