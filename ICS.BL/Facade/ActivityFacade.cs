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

namespace ICS.BL.Facade;

public class ActivityFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    IActivityModelMapper modelMapper)
    : FacadeBase<ActivityEntity, ActivityListModel, ActivityDetailModel, ActivityEntityMapper>(
        unitOfWorkFactory, modelMapper), IActivityFacade
{
    protected override ICollection<string> IncludesRatingNavigationPathDetail =>
        new[] {$"{nameof(ActivityEntity.Subject)}", $"{nameof(ActivityEntity.Ratings)}" };
}


