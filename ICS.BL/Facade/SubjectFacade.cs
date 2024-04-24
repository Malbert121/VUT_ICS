using ICS.BL.Facade.Interface;
using ICS.BL.Mappers;
using ICS.BL.Models;
using ICS.DAL.Entities;
using ICS.DAL.Mappers;
using ICS.DAL.Repositories;
using ICS.DAL.UnitOfWork;
namespace ICS.BL.Facade;

public class SubjectFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    ISubjectModelMapper modelMapper)
    : FacadeBase<SubjectEntity, SubjectListModel, SubjectDetailModel, SubjectEntityMapper>(unitOfWorkFactory, modelMapper),
        ISubjectFacade
{

    protected override ICollection<string> IncludesStudentNavigationPathDetail =>
       new[] { $"{nameof(SubjectEntity.Students)}", $"{nameof(SubjectEntity.Activity)}" } ;


}
