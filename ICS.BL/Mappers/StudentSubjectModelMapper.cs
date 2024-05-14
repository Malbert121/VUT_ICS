// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICS.BL.Models;
using ICS.DAL.Entities;

namespace ICS.BL.Mappers;
public class StudentSubjectModelMapper : ModelMapperBase<StudentSubjectEntity, StudentSubjectListModel, StudentSubjectDetailModel>
{
    public override StudentSubjectListModel MapToListModel(StudentSubjectEntity? entity)
         => entity is null
        ? StudentSubjectListModel.Empty
        : new StudentSubjectListModel
        {
            Id = entity.Id,
            StudentId = entity.StudentId,
            SubjectId = entity.SubjectId,
            StudentFirstName = entity.Student.FirstName,
            StudentLastName = entity.Student.LastName,
            SubjectName = entity.Subject.Name,
            SubjectAbbreviation = entity.Subject.Abbreviation
        };

    public override StudentSubjectDetailModel MapToDetailModel(StudentSubjectEntity? entity)
        => entity is null
        ? StudentSubjectDetailModel.Empty
        : new StudentSubjectDetailModel
        {
            Id = entity.Id,
            StudentId = entity.StudentId,
            SubjectId = entity.SubjectId,
            StudentFirstName = entity.Student.FirstName,
            StudentLastName = entity.Student.LastName,
            SubjectName = entity.Subject.Name,
            SubjectAbbreviation = entity.Subject.Abbreviation
        };

    public override StudentSubjectEntity MapDetailModelToEntity(StudentSubjectDetailModel model)
    => throw new NotImplementedException("This method is unsupported. Use the other overload.");

    public StudentSubjectEntity MapDetailModelToEntity(StudentSubjectDetailModel model, Guid SubjectId)
    {
        return new StudentSubjectEntity
        {
            Id = model.Id,
            SubjectId = SubjectId,
            StudentId = model.StudentId,
            Subject = null!,
            Student = null!
        };
    }

    public StudentSubjectEntity MapDetailModelToEntity(StudentSubjectListModel model, Guid SubjectId)
    {
        return new StudentSubjectEntity
        {
            Id = model.Id,
            SubjectId = SubjectId,
            StudentId = model.StudentId,
            Subject = null!,
            Student = null!
        };
    }

    public StudentSubjectEntity MapDetailModelToEntity(Guid studentId, StudentSubjectDetailModel model)
    {
        return new StudentSubjectEntity
        {
            Id = model.Id,
            SubjectId = studentId,
            StudentId = model.StudentId,
            Subject = null!,
            Student = null!
        };
    }

    public StudentSubjectEntity MapDetailModelToEntity(Guid studentId, StudentSubjectListModel model)
    {
        return new StudentSubjectEntity
        {
            Id = model.Id,
            SubjectId = studentId,
            StudentId = model.StudentId,
            Subject = null!,
            Student = null!
        };
    }
}
