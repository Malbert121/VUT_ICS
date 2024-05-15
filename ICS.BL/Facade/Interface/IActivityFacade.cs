using ICS.BL.Models;
using ICS.DAL.Entities;

namespace ICS.BL.Facade.Interface;

public interface IActivityFacade : IFacade<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
    Task<IEnumerable<ActivityListModel>> GetFromSubjectAsync(Guid subjectId);
    Task<IEnumerable<ActivityListModel>> GetSearchAsync(string search, Guid subjectId);
    Task<IEnumerable<ActivityListModel>> GetSortedAsync(string sortOptions, Guid subjectId);
    Task<IEnumerable<ActivityListModel>> GetFilteredAsync(Guid subjectId, DateTime date);
    Task<IEnumerable<ActivityListModel>> GetFilteredAsync(Guid subjectId, DateTime date, DateTime endDate);
}
