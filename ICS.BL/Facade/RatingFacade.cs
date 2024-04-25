using ICS.BL.Facade.Interface;
using ICS.BL.Mappers;
using ICS.BL.Models;
using ICS.DAL.Entities;
using ICS.DAL.Mappers;
using ICS.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace ICS.BL.Facade;

public class RatingFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    IRatingModelMapper modelMapper)
    : FacadeBase<RatingEntity, RatingListModel, RatingDetailModel, RatingEntityMapper>(
        unitOfWorkFactory, modelMapper), IRatingFacade
{
    protected override ICollection<string> IncludesRatingNavigationPathDetail =>
        new[] { $"{nameof(RatingEntity.Activity)}", $"{nameof(RatingEntity.Student)}" };

    public async Task<IEnumerable<RatingListModel>> GetSearchAsync(string search)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<RatingEntity> entities = await uow
            .GetRepository<RatingEntity, RatingEntityMapper>()
            .Get()
            .Where(e => e.Note.Contains(search))
            .ToListAsync();

        return ModelMapper.MapToListModel(entities);
    }


    public async Task<IEnumerable<RatingListModel>> GetSortedAsync(string sortOptions)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<RatingEntity> entities = sortOptions switch
        {
            "byDescendingId" => await uow
                            .GetRepository<RatingEntity, RatingEntityMapper>()
                            .Get()
                            .OrderByDescending(entity => entity.Id)
                            .ToListAsync(),
            "byId" => await uow
                            .GetRepository<RatingEntity, RatingEntityMapper>()
                            .Get()
                            .OrderBy(entity => entity.Id)
                            .ToListAsync(),
            "byDescendingPoints" => await uow
                            .GetRepository<RatingEntity, RatingEntityMapper>()
                            .Get()
                            .OrderByDescending(entity => entity.Points)
                            .ToListAsync(),
            "byPoints" => await uow
                            .GetRepository<RatingEntity, RatingEntityMapper>()
                            .Get()
                            .OrderBy(entity => entity.Points)
                            .ToListAsync(),
            _ => null!,
        };
        return ModelMapper.MapToListModel(entities);
    }
}
