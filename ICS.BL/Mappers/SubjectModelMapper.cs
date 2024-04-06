using ICS.BL.Models;
using ICS.DAL;
using ICS.DAL.Entities;

namespace ICS.BL.Mappers
{
    public class SubjectModelMapper (IActivityModelMapper activityModelMapper, IStudentModelMapper studentModelMapper) : ModelMapperBase<SubjectEntity, SubjectListModel, SubjectDetailModel>, ISubjectModelMapper
    {
        public override SubjectListModel MapToListModel(SubjectEntity? entity)
             => entity is null
            ? SubjectListModel.Empty
            : new SubjectListModel
            {
                Id = entity.Id,
                name = entity.name,
                abbreviation = entity.abbreviation
            };

        public override SubjectDetailModel MapToDetailModel(SubjectEntity? entity)
            => entity is null
            ? SubjectDetailModel.Empty
            : new SubjectDetailModel
            {
                Id = entity.Id,
                name = entity.name,
                abbreviation = entity.abbreviation,
                activity = activityModelMapper.MapToListModel(entity.activity).ToObservableCollection(),
                students = studentModelMapper.MapToListModel(entity.students).ToObservableCollection()
            };

        public override SubjectEntity MapToEntity(SubjectDetailModel model)
        {
            return new SubjectEntity
            {
                Id = model.Id,
                name = model.name,
                abbreviation = model.abbreviation,
            };
        }


    }
}