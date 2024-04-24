using ICS.BL.Facade.Interface;
using ICS.BL.Mappers;
using ICS.BL.Models;
using ICS.DAL.Entities;
using ICS.DAL.Mappers;
using ICS.DAL.UnitOfWork;

namespace ICS.BL.Facade;

public class RatingFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    IRatingModelMapper modelMapper)
    : FacadeBase<RatingEntity, RatingListModel, RatingDetailModel, RatingEntityMapper>(
        unitOfWorkFactory, modelMapper), IRatingFacade
{
        protected override ICollection<string> IncludesActivityNavigationPathDetail =>
                new[] { $"{nameof(RatingEntity.Activity)}", $"{nameof(RatingEntity.Student)}" };
}
