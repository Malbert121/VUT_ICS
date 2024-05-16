using ICS.BL.Models;
using ICS.DAL.Entities;

namespace ICS.BL.Mappers;

public class RatingModelMapper() : ModelMapperBase<RatingEntity, RatingListModel, RatingDetailModel>, IRatingModelMapper
{
    public override RatingListModel MapToListModel(RatingEntity? entity)
         => entity is null
        ? RatingListModel.Empty
        : new RatingListModel
        {
            Id = entity.Id,
            Points = entity.Points,
            StudentId = entity.StudentId,
            StudentName = entity.Student is not null ? entity.Student.FirstName + " " + entity.Student.LastName : string.Empty,
            ActivityId = entity.ActivityId,
        };

    public override RatingDetailModel MapToDetailModel(RatingEntity? entity)
        => entity is null
        ? RatingDetailModel.Empty
        : new RatingDetailModel
        {
            Id = entity.Id,
            Points = entity.Points,
            Note = entity.Note,
            StudentId = entity.StudentId,
            Student = entity.Student is not null ? entity.Student.FirstName + " " + entity.Student.LastName : string.Empty,
            ActivityId = entity.ActivityId
        };

    public override RatingEntity MapDetailModelToEntity(RatingDetailModel model)
    {
        return new RatingEntity
        {
            Id = model.Id,
            Points = model.Points,
            Note = model.Note,
            StudentId = model.StudentId,
            ActivityId = model.ActivityId
        };
    }
}
