using ICS.BL.Facade.Interface;
using ICS.BL.Mappers;
using ICS.BL.Models;
using ICS.DAL.Entities;
using ICS.DAL.Mappers;
using ICS.DAL.Repositories;
using ICS.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace ICS.BL.Facade;

public class SubjectFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    ISubjectModelMapper modelMapper)
    : FacadeBase<SubjectEntity, SubjectListModel, SubjectDetailModel, SubjectEntityMapper>(unitOfWorkFactory, modelMapper),
        ISubjectFacade
{

    protected override ICollection<string> IncludesStudentNavigationPathDetail =>
       new[] { $"{nameof(SubjectEntity.Students)}.{nameof(StudentSubjectEntity.Student)}", $"{nameof(SubjectEntity.Activity)}" };

    public async Task<IEnumerable<SubjectListModel>> GetSearchAsync(string search)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<SubjectEntity> entities = await uow
            .GetRepository<SubjectEntity, SubjectEntityMapper>()
            .Get()
            .Where(e => e.Name.ToLower().Contains(search.ToLower()))
            .ToListAsync();

        return ModelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<SubjectListModel>> GetSortedAsync(string sortOptions)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<SubjectEntity> entities = sortOptions switch
        {
            "byDescendingId" => await uow
                            .GetRepository<SubjectEntity, SubjectEntityMapper>()
                            .Get()
                            .OrderByDescending(entity => entity.Id)
                            .ToListAsync(),
            "byId" => await uow
                            .GetRepository<SubjectEntity, SubjectEntityMapper>()
                            .Get()
                            .OrderBy(entity => entity.Id)
                            .ToListAsync(),
            "byDescendingName" => await uow
                            .GetRepository<SubjectEntity, SubjectEntityMapper>()
                            .Get()
                            .OrderByDescending(entity => entity.Name)
                            .ToListAsync(),
            "byName" => await uow
                            .GetRepository<SubjectEntity, SubjectEntityMapper>()
                            .Get()
                            .OrderBy(entity => entity.Name)
                            .ToListAsync(),
            _ => null!,
        };
        return ModelMapper.MapToListModel(entities);
    }


}
