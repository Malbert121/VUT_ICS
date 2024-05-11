// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using ICS.BL.Facade.Interface;
using ICS.BL.Mappers;
using ICS.BL.Models;
using ICS.DAL.Entities;
using ICS.DAL.Mappers;
using ICS.DAL.Repositories;
using ICS.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace ICS.BL.Facade;
public class StudentSubjectFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    StudentSubjectModelMapper modelMapper)
    : FacadeBase<StudentSubjectEntity, StudentSubjectListModel, StudentSubjectDetailModel, StudentSubjectEntityMapper>(unitOfWorkFactory, modelMapper),
        IStudentSubjectFacade
{
    public async Task SaveAsync(StudentSubjectListModel model, Guid subjectId)
    {
        StudentSubjectEntity entity = modelMapper.MapDetailModelToEntity(model, subjectId);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<StudentSubjectEntity> repository =
            uow.GetRepository<StudentSubjectEntity, StudentSubjectEntityMapper>();

        if (await repository.ExistsAsync(entity))
        {
            await repository.UpdateAsync(entity);
            await uow.CommitAsync();
        }
    }

    public async Task SaveAsync(StudentSubjectDetailModel model, Guid SubjectId)
    {
        StudentSubjectEntity entity = modelMapper.MapDetailModelToEntity(model, SubjectId);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<StudentSubjectEntity> repository =
            uow.GetRepository<StudentSubjectEntity, StudentSubjectEntityMapper>();

        repository.Insert(entity);
        await uow.CommitAsync();
    }

    public async Task<IEnumerable<StudentSubjectListModel>> GetSearchAsync(string search)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<StudentSubjectEntity> entities = await uow
            .GetRepository<StudentSubjectEntity, StudentSubjectEntityMapper>()
        .Get()
            .Where(e => (e.Student.FirstName + " " + e.Student.LastName).Contains(search))
            .ToListAsync();

        return ModelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<StudentSubjectListModel>> GetSortedAsync(string sortOptions)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<StudentSubjectEntity> entities = sortOptions switch
        {
            "byDescendingId" => await uow
                            .GetRepository<StudentSubjectEntity, StudentSubjectEntityMapper>()
                            .Get()
                            .OrderByDescending(entity => entity.Id)
                            .ToListAsync(),
            "byId" => await uow
                            .GetRepository<StudentSubjectEntity, StudentSubjectEntityMapper>()
                            .Get()
                            .OrderBy(entity => entity.Id)
                            .ToListAsync(),
            "byDescendingLastName" => await uow
                            .GetRepository<StudentSubjectEntity, StudentSubjectEntityMapper>()
                            .Get()
                            .OrderByDescending(entity => entity.Student.LastName)
                            .ToListAsync(),
            "byLastName" => await uow
                            .GetRepository<StudentSubjectEntity, StudentSubjectEntityMapper>()
                            .Get()
                            .OrderBy(entity => entity.Student.LastName)
                            .ToListAsync(),
            _ => null!,
        };

        return ModelMapper.MapToListModel(entities);
    }


}
