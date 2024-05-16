using ICS.BL;
using ICS.BL.Models;
using ICS.DAL.Entities;

namespace ICS.BL.Mappers;

public class ActivityModelMapper(RatingModelMapper ratingModelMapper) : ModelMapperBase<ActivityEntity, ActivityListModel, ActivityDetailModel>, IActivityModelMapper
{

    public override ActivityListModel MapToListModel(ActivityEntity? entity)
         => entity is null
        ? ActivityListModel.Empty
        : new ActivityListModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Start = entity.Start,
            End = entity.End,
            Room = entity.Room,
            SubjectId = entity.SubjectId
        };

    public override ActivityDetailModel MapToDetailModel(ActivityEntity? entity)
        => entity is null
        ? ActivityDetailModel.Empty
        : new ActivityDetailModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Start = entity.Start,
            End = entity.End,
            Room = entity.Room,
            ActivityTypeTag = entity.ActivityTypeTag,
            Description = entity.Description,
            SubjectId = entity.SubjectId,
            Ratings = ratingModelMapper.MapToListModel(entity.Ratings).ToObservableCollection()
        };

    public override ActivityEntity MapDetailModelToEntity(ActivityDetailModel model)
    {
        return new ActivityEntity
        {
            Id = model.Id,
            Name = model.Name,
            Start = model.Start,
            End = model.End,
            Room = model.Room,
            ActivityTypeTag = model.ActivityTypeTag,
            Description = model.Description,
            SubjectId = model.SubjectId

        };
    }


}
