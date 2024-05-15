using ICS.BL.Models;
using ICS.DAL.Entities;

namespace ICS.BL.Facade.Interface;

public interface IStudentFacade : IFacade<StudentEntity, StudentListModel, StudentDetailModel>
{
    Task<IEnumerable<StudentListModel>> GetSearchAsync(string search);
    Task<IEnumerable<StudentListModel>> GetSortedAsync(string sortOptions);

}
