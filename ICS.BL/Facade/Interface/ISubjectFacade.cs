using ICS.BL.Models;
using ICS.DAL.Entities;

namespace ICS.BL.Facade.Interface;

public interface ISubjectFacade : IFacade<SubjectEntity, SubjectListModel, SubjectDetailModel>
{
    Task<IEnumerable<SubjectListModel>> GetSearchAsync(string search);
    Task<IEnumerable<SubjectListModel>> GetSortedAsync(string sortOptions);
}
