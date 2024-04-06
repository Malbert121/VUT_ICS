using ICS.BL;
using ICS.BL.Models;
using ICS.DAL.Entities;
using System.Collections.ObjectModel;


namespace ICS.BL.Mappers
{
    public class StudentModelMapper(SubjectModelMapper subjectModelMapper) : ModelMapperBase<StudentEntity, StudentListModel, StudentDetailModel>, IStudentModelMapper
    {
        public override StudentListModel MapToListModel(StudentEntity? entity)
             => entity is null
            ? StudentListModel.Empty
            : new StudentListModel 
            {
                Id = entity.Id,
                firstName = entity.firstName, 
                lastName = entity.lastName
            };

        public override StudentDetailModel MapToDetailModel(StudentEntity? entity)
        {
            if (entity is null)
                return StudentDetailModel.Empty;

            var detailModel = new StudentDetailModel
            {
                Id = entity.Id,
                firstName = entity.firstName,
                lastName = entity.lastName,
                fotoURL = entity.fotoURL
            };

            if (subjectModelMapper != null)
            {              
                detailModel.subjects = subjectModelMapper.MapToListModel(entity.subjects).ToObservableCollection();
            }
            else
            {               
                detailModel.subjects = new ObservableCollection<SubjectListModel>();
            }

            return detailModel;
        }




        public override StudentEntity MapToEntity(StudentDetailModel model)
        {
            return new StudentEntity
            {
                Id = model.Id,
                firstName = model.firstName,
                lastName = model.lastName,
                fotoURL = model.fotoURL
            };
        }



    }
}