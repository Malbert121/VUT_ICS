using System.Reflection;
using ICS.BL.Facade.Interface;
using ICS.BL.Mappers;
using ICS.BL.Models;
using ICS.DAL.Entities;
using ICS.DAL.Mappers;
using ICS.DAL.Repositories;
using ICS.DAL.UnitOfWork;
using System.Collections;
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

    public async Task<IEnumerable<RatingListModel>> GetFromActivityAsync(Guid activityId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<RatingEntity> entities = await uow
            .GetRepository<RatingEntity, RatingEntityMapper>()
            .Get()
            .Where(e => e.ActivityId == activityId)
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

    public async Task<RatingDetailModel> SaveAsync(RatingDetailModel model, Guid activityId)
    {
        RatingDetailModel result;

        GuardCollectionsAreNotSet(model);

        RatingEntity entity = ModelMapper.MapDetailModelToEntity(model);

        IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<RatingEntity> repository = uow.GetRepository<RatingEntity, RatingEntityMapper>();

        if (await repository.ExistsAsync(entity))
        {
            RatingEntity updatedEntity = await repository.UpdateAsync(entity);
            result = ModelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            entity.Id = Guid.NewGuid();
            entity.ActivityId = activityId;
            RatingEntity insertedEntity = repository.Insert(entity);
            result = ModelMapper.MapToDetailModel(insertedEntity);
        }

        await uow.CommitAsync().ConfigureAwait(false);

        return result;
    }

    private static void GuardCollectionsAreNotSet(RatingDetailModel model)
    {
        IEnumerable<PropertyInfo> collectionProperties = model
            .GetType()
            .GetProperties()
            .Where(i => typeof(ICollection).IsAssignableFrom(i.PropertyType));

        foreach (PropertyInfo collectionProperty in collectionProperties)
        {
            if (collectionProperty.GetValue(model) is ICollection { Count: > 0 })
            {
                throw new InvalidOperationException(
                    "Current BL and DAL infrastructure disallows insert or update of models with adjacent collections.");
            }
        }
    }
}
