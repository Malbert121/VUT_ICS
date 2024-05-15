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
    protected override ICollection<string> IncludesStudentSubjectNavigationPathDetail =>
    new[] { $"{nameof(StudentSubjectEntity.Subject)}", $"{nameof(StudentSubjectEntity.Student)}" };
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

    public async Task SaveAsync(StudentSubjectDetailModel model, Guid subjectId)
    {
        StudentSubjectEntity entity = modelMapper.MapDetailModelToEntity(model, subjectId);

        IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<StudentSubjectEntity> repository =
            uow.GetRepository<StudentSubjectEntity, StudentSubjectEntityMapper>();

        if (await repository.ExistsAsync(entity))
        {
            await repository.UpdateAsync(entity);
            await uow.CommitAsync();
        }
        else
        {
            repository.Insert(entity);
            await uow.CommitAsync();
        }
    }

    public async Task SaveAsync(Guid studentId, StudentSubjectListModel model)
    {
        StudentSubjectEntity entity = modelMapper.MapDetailModelToEntity(studentId, model);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<StudentSubjectEntity> repository =
            uow.GetRepository<StudentSubjectEntity, StudentSubjectEntityMapper>();

        if (await repository.ExistsAsync(entity))
        {
            await repository.UpdateAsync(entity);
            await uow.CommitAsync();
        }
    }

    public async Task SaveAsync(Guid studentId, StudentSubjectDetailModel model)
    {
        StudentSubjectEntity entity = modelMapper.MapDetailModelToEntity(studentId, model);

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

    public async Task<IEnumerable<StudentSubjectListModel>> GetSubjectsAsync(Guid studentId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<StudentSubjectEntity> query = uow
            .GetRepository<StudentSubjectEntity, StudentSubjectEntityMapper>()
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
        List<StudentSubjectEntity> entities = await query.Where(e => e.StudentId == studentId).ToListAsync();

        return ModelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<StudentSubjectListModel>> GetStudentsAsync(Guid subjectId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<StudentSubjectEntity> query = uow
            .GetRepository<StudentSubjectEntity, StudentSubjectEntityMapper>()
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
        List<StudentSubjectEntity> entities = await query.Where(e => e.SubjectId == subjectId).ToListAsync();

        return ModelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<StudentSubjectListModel>> GetSortedAsync(string sortOptions, Guid subjectId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<StudentSubjectEntity> query = uow
            .GetRepository<StudentSubjectEntity, StudentSubjectEntityMapper>()
            .Get()
            .Where(e => e.Subject.Id == subjectId);

        foreach (string pathDetail in IncludesActivityNavigationPathDetail
            .Concat(IncludesRatingNavigationPathDetail)
            .Concat(IncludesStudentNavigationPathDetail)
            .Concat(IncludesSubjectNavigationPathDetail)
            .Concat(IncludesStudentSubjectNavigationPathDetail))
        {
            query = query.Include(pathDetail);
        }

        List<StudentSubjectEntity> entities = sortOptions switch
        {
            "byDescendingId" => await query
                .OrderByDescending(entity => entity.Student.Id)
                .ToListAsync(),

            "byId" => await query
                .OrderBy(entity => entity.Student.Id)
                .ToListAsync(),

            "byDescendingLastName" => await query
                .OrderByDescending(entity => entity.Student.LastName)
                .ToListAsync(),

            "byLastName" => await query
                .OrderBy(entity => entity.Student.LastName)
                .ToListAsync(),

            _ => throw new ArgumentException("Invalid sort option", nameof(sortOptions))
        };

        return ModelMapper.MapToListModel(entities);
    }


}
