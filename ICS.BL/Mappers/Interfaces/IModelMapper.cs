

namespace ICS.BL.Mappers;

public interface IModelMapper<TEntity, TListModel, TDetailModel>
{
    TListModel MapToListModel(TEntity? entity);
    TDetailModel MapToDetailModel(TEntity? entity);
    TEntity MapDetailModelToEntity(TDetailModel model);
    IEnumerable<TListModel> MapToListModel(IEnumerable<TEntity> entities)
        => entities.Select(MapToListModel);

}
