using ICS.BL.Models;
using ICS.DAL.Entities;

namespace ICS.BL.Mappers
{
    public class RatingModelMapper : ModelMapperBase<RatingEntity, RatingListModel, RatingDetailModel>, IRatingModelMapper
    {
        public override RatingListModel MapToListModel(RatingEntity? entity)
             => entity?.student is null
            ? RatingListModel.Empty
            : new RatingListModel
            {
                Id = entity.Id,
                points = entity.points,
                studentId = entity.studentId,
                activityId = entity.activityId,
                student = entity.student
            };

        public override RatingDetailModel MapToDetailModel(RatingEntity? entity)
            => entity is null
            ? RatingDetailModel.Empty
            : new RatingDetailModel
            {
                Id = entity.Id,
                points = entity.points,
                note = entity.note,
                activityId = entity.activityId,
                activity = entity.activity,
                studentId = entity.studentId,
                student = entity.student
                
            };

        public override RatingEntity MapToEntity(RatingDetailModel model)
        {
            return new RatingEntity
            {
                Id = model.Id,
                points = model.points,
                note = model.note,
                activityId = model.activityId,
                activity = model.activity,
                studentId = model.studentId,
                student = model.student
            };
        }

    }
}