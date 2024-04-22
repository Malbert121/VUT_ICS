using ICS.BL.Models;
using ICS.DAL;
using ICS.DAL.Entities;

namespace ICS.BL.Mappers;

public class SubjectModelMapper (ActivityModelMapper? activityModelMapper, StudentModelMapper? studentModelMapper) : ModelMapperBase<SubjectEntity, SubjectListModel, SubjectDetailModel>, ISubjectModelMapper
{
    public override SubjectListModel MapToListModel(SubjectEntity? entity)
         => entity is null
        ? SubjectListModel.Empty
        : new SubjectListModel
        {
            Id = entity.Id,
            name = entity.Name,
            abbreviation = entity.Abbreviation
        };

    public override SubjectDetailModel MapToDetailModel(SubjectEntity? entity)
        => entity is null
        ? SubjectDetailModel.Empty
        : new SubjectDetailModel
        {
            Id = entity.Id,
            name = entity.Name,
            abbreviation = entity.Abbreviation,
            activity = activityModelMapper!.MapToListModel(entity.Activity).ToObservableCollection(),
            students = studentModelMapper!.MapToListModel(entity.Students).ToObservableCollection()
        };

    public override SubjectEntity MapDetailModelToEntity(SubjectDetailModel model)
    {
        return new SubjectEntity
        {
            Id = model.Id,
            Name = model.name,
            Abbreviation = model.abbreviation,
        };
    }

    public override SubjectEntity MapListModelToEntity(SubjectListModel? model)
    {
        return new SubjectEntity
        {
            Id = model!.Id,
            Name = model.name,
            Abbreviation = model.abbreviation,
        };
    }

}
