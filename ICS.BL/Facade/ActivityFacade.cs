using ICS.BL.Facade.Interface;
using ICS.BL.Models;
using ICS.DAL.Entities;
using ICS.DAL.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICS.BL.Mappers;
using ICS.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using ICS.DAL.Repositories;
using System.Collections;
using System.Reflection;

namespace ICS.BL.Facade;

public class ActivityFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    IActivityModelMapper modelMapper)
    : FacadeBase<ActivityEntity, ActivityListModel, ActivityDetailModel, ActivityEntityMapper>(
        unitOfWorkFactory, modelMapper), IActivityFacade
{
    protected override ICollection<string> IncludesActivityNavigationPathDetail =>
        new[] {$"{nameof(ActivityEntity.Subject)}", $"{nameof(ActivityEntity.Ratings)}", $"{nameof(ActivityEntity.Ratings)}.{nameof(RatingEntity.Student)}" };

    public async Task<IEnumerable<ActivityListModel>> GetSearchAsync(string search, Guid subjectId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<ActivityEntity> entities = await uow
            .GetRepository<ActivityEntity, ActivityEntityMapper>()
            .Get()
            .Where(e => e.Name.ToLower().Contains(search.ToLower()) && e.SubjectId == subjectId)
            .ToListAsync();

        return ModelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<ActivityListModel>> GetFromSubjectAsync(Guid subjectId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<ActivityEntity> entities = await uow
            .GetRepository<ActivityEntity, ActivityEntityMapper>()
            .Get()
            .Where(e => e.SubjectId == subjectId)
            .ToListAsync();

        return ModelMapper.MapToListModel(entities);
    }
    public async Task<IEnumerable<ActivityListModel>> GetSortedAsync(string sortOptions, Guid subjectId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<ActivityEntity> entities = sortOptions switch
        {
            "byDescendingId" => await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.SubjectId == subjectId)
                            .OrderByDescending(entity => entity.Id)
                            .ToListAsync(),
            "byId" => await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.SubjectId == subjectId)
                            .OrderBy(entity => entity.Id)
                            .ToListAsync(),
            "byDescendingDate" => await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.SubjectId == subjectId)
                            .OrderByDescending(entity => entity.Start)
                            .ToListAsync(),
            "byDate" => await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.SubjectId == subjectId)
                            .OrderBy(entity => entity.Start)
                            .ToListAsync(),
            "byDescendingName" => await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.SubjectId == subjectId)
                            .OrderByDescending(entity => entity.Name)
                            .ToListAsync(),
            "byName" => await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.SubjectId == subjectId)
                            .OrderBy(entity => entity.Name)
                            .ToListAsync(),
            "byDescendingRoom" => await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.SubjectId == subjectId)
                            .OrderByDescending(entity => entity.Start)
                            .ToListAsync(),
            "byRoom" => await uow
                            .GetRepository<ActivityEntity, ActivityEntityMapper>()
                            .Get()
                            .Where(e => e.SubjectId == subjectId)
                            .OrderBy(entity => entity.Start)
                            .ToListAsync(),
            _ => null!,
        };
        return ModelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<ActivityListModel>> GetFilteredAsync(DateTime date)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<ActivityEntity> entities = await uow
            .GetRepository<ActivityEntity, ActivityEntityMapper>()
            .Get()
            .Where(e => e.Start >= date)
            .OrderBy(e => e.Start)
            .ToListAsync();

        return ModelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<ActivityListModel>> GetFilteredAsync(DateTime date, DateTime endDate)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<ActivityEntity> entities = await uow
            .GetRepository<ActivityEntity, ActivityEntityMapper>()
            .Get()
            .Where(e => e.Start >= date && e.End <= endDate)
            .OrderBy(e => e.Start)
            .ToListAsync();

        return ModelMapper.MapToListModel(entities);
    }

}


