using ICS.BL.Facade.Interface;
using ICS.BL.Models;
using ICS.DAL.Entities;
using ICS.DAL.Mappers;
using ICS.BL.Mappers;
using ICS.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace ICS.BL.Facade;

public class StudentFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    IStudentModelMapper modelMapper)
    : FacadeBase<StudentEntity, StudentListModel, StudentDetailModel, StudentEntityMapper>(unitOfWorkFactory, modelMapper),
        IStudentFacade
{
    protected override ICollection<string> IncludesSubjectNavigationPathDetail =>
       new[] { $"{nameof(StudentEntity.Subjects)}.{nameof(StudentSubjectEntity.Subject)}" };

    public async Task<IEnumerable<StudentListModel>> GetSearchAsync(string search)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<StudentEntity> entities = await uow
            .GetRepository<StudentEntity, StudentEntityMapper>()
            .Get()
            .Where(e => (e.FirstName + " " + e.LastName).ToLower().Contains(search.ToLower()))
            .ToListAsync();

        return ModelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<StudentListModel>> GetSortedAsync(string sortOptions)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<StudentEntity> entities = sortOptions switch
        {
            "byDescendingId" => await uow
                            .GetRepository<StudentEntity, StudentEntityMapper>()
                            .Get()
                            .OrderByDescending(entity => entity.Id)
                            .ToListAsync(),
            "byId" => await uow
                            .GetRepository<StudentEntity, StudentEntityMapper>()
                            .Get()
                            .OrderBy(entity => entity.Id)
                            .ToListAsync(),
            "byDescendingLastName" => await uow
                            .GetRepository<StudentEntity, StudentEntityMapper>()
                            .Get()
                            .OrderByDescending(entity => entity.LastName)  
                            .ToListAsync(),
            "byLastName" => await uow
                            .GetRepository<StudentEntity, StudentEntityMapper>()
                            .Get()
                            .OrderBy(entity => entity.LastName)
                            .ToListAsync(),
            _ => null!,
        };

        return ModelMapper.MapToListModel(entities);
    }


}
