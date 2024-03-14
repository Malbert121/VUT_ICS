using ICS.DAL;

namespace ICS.BL.Mappers
{
    public class ActivityModelMapper : ModelMapperBase<ActivityEntity, ActivityListModel, ActivityDetailModel>, IActivityModelMapper
    {
        public override ActivityListModel MapToListModel(ActivityEntity? entity)
             => entity is null
            ? ActivityListModel.Empty
            : new ActivityListModel
            {
                Id = entity.Id,
                Name = entity.name,
                Start = entity.start
            };

        public override ActivityDetailModel MapToDetailModel(ActivityEntity? entity)
            => entity is null
            ? ActivityDetailModel.Empty
            : new ActivityDetailModel
            {
                Id = entity.Id,
                Name = entity.name,
                Start = entity.start,
                End = entity.end,
                Room = entity.room,
                ActivityTypeTag = entity.activityTypeTag,
                Description = entity.description,
                SubjectId = entity.subjectId,
                Subject = entity.subject,
                Rating = entity.rating
            };

        public override ActivityEntity MapToEntity(ActivityDetailModel model)
        {
            return new ActivityEntity
            {
                Id = model.Id,
                name = model.Name,
                start = model.Start,
                end = model.End,
                room = model.Room,
                activityTypeTag = model.ActivityTypeTag,
                description = model.Description,
                subjectId = model.SubjectId,
                subject = model.Subject,
                rating = model.Rating
            };
        }

    }
}