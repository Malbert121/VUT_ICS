using ICS.BL.Models;
using ICS.DAL.Entities;


namespace ICS.BL.Facade.Interface;

public interface IRatingFacade : IFacade<RatingEntity, RatingListModel, RatingDetailModel>
{
    Task<IEnumerable<RatingListModel>> GetFromActivityAsync(Guid activityId);

}
