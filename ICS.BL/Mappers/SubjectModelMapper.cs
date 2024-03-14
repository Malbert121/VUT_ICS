using ICS.DAL;

namespace ICS.BL.Mappers
{
    public class SubjectModelMapper : ModelMapperBase<SubjectEntity, SubjectListModel, SubjectDetailModel>, ISubjectModelMapper
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
                activity = new ActivityModelMapper().MapToListModel(entity.activity).ToObservableCollection(),
                students = new StudentModelMapper().MapToListModel(entity.students).ToObservableCollection()
            };

        public override SubjectEntity MapToEntity(SubjectDetailModel model)
        {
            return new SubjectEntity
            {
                Id = model.Id,
                name = model.name,
                abbreviation = model.abbreviation,
                activity = model.activity
                    .Select(ActivityModelMapper.MapToEntity)
                    .ToList(), 
                students = model.students
                    .Select(StudentModelMapper.MapToEntity)
                    .ToList()
            };
        }

    }
}