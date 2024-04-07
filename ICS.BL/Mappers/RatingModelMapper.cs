using ICS.BL.Models;
using ICS.DAL.Entities;

namespace ICS.BL.Mappers
{
    public class RatingModelMapper : ModelMapperBase<RatingEntity, RatingListModel, RatingDetailModel>, IRatingModelMapper
    {
        public override RatingListModel MapToListModel(RatingEntity? entity)
             => entity?.Student is null
            ? RatingListModel.Empty
            : new RatingListModel
            {
                Id = entity.Id,
                points = entity.Points,
                studentId = entity.StudentId,
                activityId = entity.ActivityId,
                student = entity.Student
            };

        public override RatingDetailModel MapToDetailModel(RatingEntity? entity)
            => entity is null
            ? RatingDetailModel.Empty
            : new RatingDetailModel
            {
                Id = entity.Id,
                points = entity.Points,
                note = entity.Note,
                activityId = entity.ActivityId,
                activity = entity.Activity,
                studentId = entity.StudentId,
                student = entity.Student
                
            };

        public override RatingEntity MapToEntity(RatingDetailModel model)
        {
            return new RatingEntity
            {
                Id = model.Id,
                Points = model.points,
                Note = model.note,
                ActivityId = model.activityId,
                Activity = model.activity,
                StudentId = model.studentId,
                Student = model.student
            };
        }

    }
}