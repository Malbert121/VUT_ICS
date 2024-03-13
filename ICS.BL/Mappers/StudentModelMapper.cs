using ICS.DAL;


namespace ICS.BL.Mappers
{
    public class StudentModelMapper : ModelMapperBase<StudentEntity, StudentListModel, StudentDetailModel>, IStudentModelMapper
    {
        public override StudentListModel MapToListModel(StudentEntity? entity)
             => entity is null
            ? StudentListModel.Empty
            : new StudentListModel 
            {
                Id = entity.Id,
                studentId = entity.studentId,
                firstName = entity.firstName, 
                lastName = entity.lastName
            };

        public override StudentDetailModel MapToDetailModel(StudentEntity? entity)
            => entity is null
            ? StudentDetailModel.Empty
            : new StudentDetailModel
            {
                Id = entity.Id,
                studentId = entity.studentId,
                firstName = entity.firstName,
                lastName = entity.lastName,
                fotoURL = entity.fotoURL,
                subjects = new SubjectModelMapper().MapToListModel(entity.subjects)
            };

        public override StudentEntity MapToEntity(StudentDetailModel model)
        {
            return new StudentEntity
            {
                Id = model.Id,
                studentId = model.studentId,
                firstName = model.firstName,
                lastName = model.lastName,
                fotoURL = model.fotoURL,
                subjects = SubjectModelMapper.MapToListModel(model.subjects)
                    .Select(SubjectModelMapper.MapToEntity)
                    .ToList()
            };
        }

    }
}