using ICS.BL.Facade.Interface;
using ICS.BL.Mappers;
using ICS.DAL.Entities;
using ICS.DAL.Mappers;
using ICS.DAL.Repositories;
using ICS.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ICS.BL.Models;

namespace ICS.BL.Facade
{
    public abstract class
        FacadeBase<TEntity, TListModel, TDetailModel, TEntityMapper>(
            IUnitOfWorkFactory unitOfWorkFactory,
            IModelMapper<TEntity, TListModel, TDetailModel> modelMapper)
        : IFacade<TEntity, TListModel, TDetailModel>
        where TEntity : class, IEntity
        where TListModel : IModel
        where TDetailModel : class, IModel
        where TEntityMapper : IEntityMapper<TEntity>, new()
    {
        protected readonly IModelMapper<TEntity, TListModel, TDetailModel> ModelMapper = modelMapper;
        protected readonly IUnitOfWorkFactory UnitOfWorkFactory = unitOfWorkFactory;

        protected virtual string IncludesStudentNavigationPathDetail => string.Empty;
        protected virtual string IncludesActivityNavigationPathDetail => string.Empty;
        protected virtual string IncludesSubjectNavigationPathDetail => string.Empty;

        public async Task DeleteAsync(Guid id)
        {
            await using IUnitOfWork uow = UnitOfWorkFactory.Create();
            try
            {
                await uow.GetRepository<TEntity, TEntityMapper>().DeleteAsync(id);
                await uow.CommitAsync().ConfigureAwait(false);
            }
            catch (DbUpdateException e)
            {
                throw new InvalidOperationException("Entity deletion failed.", e);
            }
        }

        public virtual async Task<TDetailModel?> GetAsync(Guid id)
        {
            await using IUnitOfWork uow = UnitOfWorkFactory.Create();

            IQueryable<TEntity> query = uow.GetRepository<TEntity, TEntityMapper>().Get();

            if (string.IsNullOrWhiteSpace(IncludesStudentNavigationPathDetail) is false)
            {
                query = query.Include(IncludesStudentNavigationPathDetail);
            }
            else if (string.IsNullOrWhiteSpace(IncludesActivityNavigationPathDetail) is false)
            {
                query = query.Include(IncludesActivityNavigationPathDetail);
            }
            else if (string.IsNullOrWhiteSpace(IncludesSubjectNavigationPathDetail) is false)
            {
                query = query.Include(IncludesSubjectNavigationPathDetail);
            }

            TEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == id);

            return entity is null
                ? null
                : ModelMapper.MapToDetailModel(entity);
        }

        public virtual async Task<IEnumerable<TListModel>> GetAsync()
        {
            await using IUnitOfWork uow = UnitOfWorkFactory.Create();
            List<TEntity> entities = await uow
                .GetRepository<TEntity, TEntityMapper>()
                .Get()
                .ToListAsync();

            return ModelMapper.MapToListModel(entities);
        }

        public virtual async Task<TDetailModel> SaveAsync(TDetailModel model)
        {
            TDetailModel result;

            GuardCollectionsAreNotSet(model);

            TEntity entity = ModelMapper.MapToEntity(model);

            IUnitOfWork uow = UnitOfWorkFactory.Create();
            IRepository<TEntity> repository = uow.GetRepository<TEntity, TEntityMapper>();

            if (await repository.ExistsAsync(entity))
            {
                TEntity updatedEntity = await repository.UpdateAsync(entity);
                result = ModelMapper.MapToDetailModel(updatedEntity);
            }
            else
            {
                entity.Id = Guid.NewGuid();
                TEntity insertedEntity =  repository.Insert(entity);
                result = ModelMapper.MapToDetailModel(insertedEntity);
            }

            await uow.CommitAsync();

            return result;
        }

        /// <summary>
        /// This Guard ensures that there is a clear understanding of current infrastructure limitations.
        /// This version of BL/DAL infrastructure does not support insertion or update of adjacent entities.
        /// WARN: Does not guard navigation properties.
        /// </summary>
        /// <param name="model">Model to be inserted or updated</param>
        /// <exception cref="InvalidOperationException"></exception>
        private static void GuardCollectionsAreNotSet(TDetailModel model)
        {
            IEnumerable<PropertyInfo> collectionProperties = model
                .GetType()
                .GetProperties()
                .Where(i => typeof(ICollection).IsAssignableFrom(i.PropertyType));

            foreach (PropertyInfo collectionProperty in collectionProperties)
            {
                if (collectionProperty.GetValue(model) is ICollection { Count: > 0 })
                {
                    throw new InvalidOperationException(
                        "Current BL and DAL infrastructure disallows insert or update of models with adjacent collections.");
                }
            }
        }
    }

}
