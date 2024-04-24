using ICS.BL.Models;
using ICS.DAL.Entities;

namespace ICS.BL.Mappers;

public class RatingModelMapper(ActivityModelMapper? activityModelMapper, StudentModelMapper? studentModelMapper) : ModelMapperBase<RatingEntity, RatingListModel, RatingDetailModel>, IRatingModelMapper
{
    public override RatingListModel MapToListModel(RatingEntity? entity)
         => entity is null
        ? RatingListModel.Empty
        : new RatingListModel
        {
            Id = entity.Id,
            points = entity.Points,
            studentId = entity.StudentId,
            activityId = entity.ActivityId,
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
            activity = activityModelMapper!.MapToListModel(entity.Activity),
            studentId = entity.StudentId,
            student = studentModelMapper!.MapToListModel(entity.Student)
            
        };

    public override RatingEntity MapDetailModelToEntity(RatingDetailModel model)
    {
        return new RatingEntity
        {
            Id = model.Id,
            Points = model.points,
            Note = model.note,
            ActivityId = model.activityId,
            Activity = activityModelMapper!.MapListModelToEntity(model.activity!),
            StudentId = model.studentId,
            Student = studentModelMapper!.MapListModelToEntity(model.student!)
        };
    }
    public override RatingEntity MapListModelToEntity(RatingListModel? model)
    {
        return new RatingEntity
        {
            Id = model!.Id,
            Points = model.points,
            ActivityId = model.activityId,
            StudentId = model.studentId,
            //Student = studentModelMapper!.MapListModelToEntity(model.student!)
        };
    }
}
