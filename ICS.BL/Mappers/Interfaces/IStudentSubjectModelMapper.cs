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

public interface IStudentSubjectModelMapper : IModelMapper<StudentSubjectEntity, StudentSubjectListModel, StudentSubjectDetailModel>
{
}
