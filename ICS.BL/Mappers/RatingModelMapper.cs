using ICS.DAL;

namespace ICS.BL.Mappers
{
    public class RatingModelMapper : ModelMapperBase<RatingEntity, RatingListModel, RatingDetailModel>, IRatingModelMapper
    {
        public override RatingListModel MapToListModel(RatingEntity? entity)
             => entity is null
            ? RatingListModel.Empty
            : new RatingListModel
            {
                Id = entity.Id,
                Points = entity.points,
 
            };

        public override RatingDetailModel MapToDetailModel(RatingEntity? entity)
            => entity is null
            ? RatingDetailModel.Empty
            : new RatingDetailModel
            {
                Id = entity.Id,
                Points = entity.points,
                Note = entity.note,
                ActivityId = entity.activityId,
                Activity = entity.activity,
                StudentId = entity.studentId,
                Student = entity.student
                
            };

        public override RatingEntity MapToEntity(RatingDetailModel model)
        {
            return new RatingEntity
            {
                Id = model.Id,
                points = model.Points,
                note = model.Note,
                activityId = model.ActivityId,
                activity = model.Activity,
                studentId = model.StudentId,
                student = model.Student
            };
        }

    }
}