using ICS.DAL.Entities;

namespace ICS.DAL.Mappers
{
    public class RatingEntityMapper : IEntityMapper<RatingEntity>
    {
        public void MapToExistingEntity(RatingEntity existingEntity, RatingEntity newEntity)
        {
            existingEntity.Points = newEntity.Points;
            existingEntity.Note = newEntity.Note;
            existingEntity.StudentId = newEntity.StudentId;
            existingEntity.ActivityId = newEntity.ActivityId;
        }
    }
}
