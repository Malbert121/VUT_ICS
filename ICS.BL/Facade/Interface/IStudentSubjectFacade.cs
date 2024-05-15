using ICS.BL.Models;
using ICS.DAL.Entities;

namespace ICS.BL.Facade.Interface;

public interface IStudentSubjectFacade : IFacade<StudentSubjectEntity, StudentSubjectListModel, StudentSubjectDetailModel>
{
    Task<IEnumerable<StudentSubjectListModel>> GetSubjectsAsync(Guid studentId);

    Task<IEnumerable<StudentSubjectListModel>> GetStudentsAsync(Guid subjectId);

    Task SaveAsync(StudentSubjectDetailModel model, Guid subjectId);
}
