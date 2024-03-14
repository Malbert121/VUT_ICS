using ICS.DAL.Entities;

namespace ICS.DAL.Mappers
{
    public class RatingEntityMapper : IEntityMapper<RatingEntity>
    {
        public void MapToExistingEntity(RatingEntity existingEntity, RatingEntity newEntity)
        {
            existingEntity.points = newEntity.points;
            existingEntity.note = newEntity.note;
            existingEntity.studentId = newEntity.studentId;
             
        }
    }
}
