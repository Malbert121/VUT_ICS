using ICS.BL.Mappers;
using ICS.BL.Models;
using ICS.DAL.Entities;

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
                name = entity.name,
                start = entity.start
            };

        public override ActivityDetailModel MapToDetailModel(ActivityEntity? entity)
            => entity is null
            ? ActivityDetailModel.Empty
            : new ActivityDetailModel
            {
                Id = entity.Id,
                name = entity.name,
                start = entity.start,
                end = entity.end,
                room = entity.room,
                activityTypeTag = entity.activityTypeTag,
                description = entity.description,
                subjectId = entity.subjectId,
                subject = entity.subject,
                rating = entity.rating
            };

        public override ActivityEntity MapToEntity(ActivityDetailModel model)
        {
            return new ActivityEntity
            {
                Id = model.Id,
                name = model.name,
                start = model.start,
                end = model.end,
                room = model.room,
                activityTypeTag = model.activityTypeTag,
                description = model.description,
                subjectId = model.subjectId,
                subject = model.subject,
                rating = model.rating
            };
        }

    }
}