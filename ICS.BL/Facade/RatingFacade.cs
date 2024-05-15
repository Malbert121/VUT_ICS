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

    public async Task<IEnumerable<RatingListModel>> GetSearchAsync(string search, Guid activityId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        // Split the search input into separate terms
        string[] searchTerms = search.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        List<RatingEntity> entities = await uow
            .GetRepository<RatingEntity, RatingEntityMapper>()
            .Get()
            .Where(e => searchTerms.All(term =>
                    e.Student.FirstName.ToLower().Contains(term) ||
                    e.Student.LastName.ToLower().Contains(term)) &&
                e.ActivityId == activityId)
            .ToListAsync();

        var studentIds = entities.Select(e => e.StudentId).ToList();
        var studentNames = await uow.GetRepository<StudentEntity, StudentEntityMapper>()
                                    .Get()
                                    .Where(s => studentIds.Contains(s.Id))
                                    .ToDictionaryAsync(s => s.Id, s => s.FirstName);

        var models = ModelMapper.MapToListModel(entities);
        foreach (var model in models)
        {
            if (studentNames.TryGetValue(model.studentId, out var name))
                model.StudentName = name;
        }

        return models;
    }


    public async Task<IEnumerable<RatingListModel>> GetFromActivityAsync(Guid activityId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<RatingEntity> query = uow
            .GetRepository<RatingEntity, RatingEntityMapper>()
            .Get();

        foreach (string pathDetail in IncludesActivityNavigationPathDetail)
        {
            query = query.Include(pathDetail);
        }
        foreach (string pathDetail in IncludesRatingNavigationPathDetail)
        {
            query = query.Include(pathDetail);
        }
        foreach (string pathDetail in IncludesStudentNavigationPathDetail)
        {
            query = query.Include(pathDetail);
        }
        foreach (string pathDetail in IncludesSubjectNavigationPathDetail)
        {
            query = query.Include(pathDetail);
        }
        foreach (string pathDetail in IncludesStudentSubjectNavigationPathDetail)
        {
            query = query.Include(pathDetail);
        }
        List<RatingEntity> entities = await query.Where(e => e.ActivityId == activityId).ToListAsync();

        return ModelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<RatingListModel>> GetSortedAsync(string sortOptions, Guid activityId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        List<RatingEntity> entities = sortOptions switch
        {
            "byDescendingId" => await uow
                            .GetRepository<RatingEntity, RatingEntityMapper>()
                            .Get()
                            .Where(e => e.ActivityId == activityId)
                            .OrderByDescending(entity => entity.Id)
                            .ToListAsync(),
            "byId" => await uow
                            .GetRepository<RatingEntity, RatingEntityMapper>()
                            .Get()
                            .Where(e => e.ActivityId == activityId)
                            .OrderBy(entity => entity.Id)
                            .ToListAsync(),
            "byDescendingPoints" => await uow
                            .GetRepository<RatingEntity, RatingEntityMapper>()
                            .Get()
                            .Where(e => e.ActivityId == activityId)
                            .OrderByDescending(entity => entity.Points)
                            .ToListAsync(),
            "byPoints" => await uow
                            .GetRepository<RatingEntity, RatingEntityMapper>()
                            .Get()
                            .Where(e => e.ActivityId == activityId)
                            .OrderBy(entity => entity.Points)
                            .ToListAsync(),
            _ => null!,
        };
        var studentIds = entities.Select(e => e.StudentId).ToList();
        var studentNames = await uow.GetRepository<StudentEntity, StudentEntityMapper>()
                                    .Get()
                                    .Where(s => studentIds.Contains(s.Id))
                                    .ToDictionaryAsync(s => s.Id, s => s.FirstName);

        var models = ModelMapper.MapToListModel(entities);
        foreach (var model in models)
        {
            if (studentNames.TryGetValue(model.studentId, out var name))
                model.StudentName = name;
        }

        return models;
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
