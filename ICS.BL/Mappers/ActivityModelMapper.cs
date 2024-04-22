using ICS.BL;
using ICS.BL.Models;
using ICS.DAL.Entities;

namespace ICS.BL.Mappers;

public class ActivityModelMapper(RatingModelMapper? ratingModelMapper, SubjectModelMapper? subjectModelMapper) : ModelMapperBase<ActivityEntity, ActivityListModel, ActivityDetailModel>, IActivityModelMapper
{
    public override ActivityListModel MapToListModel(ActivityEntity? entity)
         => entity is null
        ? ActivityListModel.Empty
        : new ActivityListModel
        {
            Id = entity.Id,
            name = entity.Name,
            start = entity.Start,
            end = entity.End,
            room = entity.Room
        };

    public override ActivityDetailModel MapToDetailModel(ActivityEntity? entity)
        => entity is null
        ? ActivityDetailModel.Empty
        : new ActivityDetailModel
        {
            Id = entity.Id,
            name = entity.Name,
            start = entity.Start,
            end = entity.End,
            room = entity.Room,
            activityTypeTag = entity.ActivityTypeTag,
            description = entity.Description,
            subjectId = entity.SubjectId,
            subject = subjectModelMapper!.MapToListModel(entity.Subject),
            ratings = ratingModelMapper!.MapToListModel(entity.Ratings).ToObservableCollection()
        };

    public override ActivityEntity MapToEntity(ActivityDetailModel model)
    {
        return new ActivityEntity
        {
            Id = model.Id,
            Name = model.name,
            Start = model.start,
            End = model.end,
            Room = model.room,
            ActivityTypeTag = model.activityTypeTag,
            Description = model.description,
            SubjectId = model.subjectId,
            Subject = subjectModelMapper!.MapToEntityList(model.subject)

        };
    }

    public override ActivityEntity MapToEntityList(ActivityListModel model)
    {
        return new ActivityEntity
        {
            Id = model.Id,
            Name = model.name,
            Start = model.start,
            End = model.end,
            Room = model.room
        };
    }

}
