namespace ICS.BL.Facades;

public class SubjectFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    SubjectModelMapper modelMapper)
    :
        FacadeBase<SubjectEntity, SubjectListModel, SubjectDetailModel, SubjectEntityMapper>(
            unitOfWorkFactory, modelMapper), ISubjectFacade

{
    public async Task<SubjectDetailModel> SaveSubjectWithActivitiesAsync(SubjectDetailModel model)
    {
        GuardCollectionsAreNotSet(model);

        using IUnitOfWork uow = UnitOfWorkFactory.Create();
        var subjectEntity = ModelMapper.MapToEntity(model);
        

        if (await uow.GetRepository<SubjectEntity, SubjectEntityMapper>().ExistsAsync(subjectEntity))
        {
            subjectEntity = await uow.GetRepository<SubjectEntity, SubjectEntityMapper>().UpdateAsync(subjectEntity);
        }
        else
        {
            subjectEntity = uow.GetRepository<SubjectEntity, SubjectEntityMapper>().Insert(subjectEntity);
        }
        
        await uow.CommitAsync();

        return ModelMapper.MapToDetailModel(subjectEntity);
    }
}