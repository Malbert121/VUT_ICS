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

namespace ICS.BL.Facade
{
    public class StudentFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        IStudentModelMapper modelMapper)
        : FacadeBase<StudentEntity, StudentListModel, StudentDetailModel, StudentEntityMapper>(unitOfWorkFactory, modelMapper),
            IStudentFacade
    {
        protected override string IncludesSubjectNavigationPathDetail =>
            $"{nameof(StudentEntity.subjects)}.{nameof(SubjectEntity.students)}";


    }
}
