using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.Contracts;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Dpl.B2b.BusinessLogic.Extensions
{

    public static class DataExtensions
    {
        const int MaxLimit = 50;

        private static IMapper _mapper;        
        public static IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    _mapper = ServiceLocator.Current.GetInstance<IMapper>();
                }

                return _mapper;
            }
        }

        public static IOrderedQueryable<TEntity> OrderBy<TEntity, TKey>(this IQueryable<TEntity> query, Expression<Func<TEntity, TKey>> keySelector, System.ComponentModel.ListSortDirection? sortOrder)
        {
            var sortOrderSafe = sortOrder.HasValue ? sortOrder.Value : System.ComponentModel.ListSortDirection.Ascending;
            return sortOrderSafe == System.ComponentModel.ListSortDirection.Ascending ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);
        }

        public static IOrderedQueryable<TEntity> ThenBy<TEntity, TKey>(this IOrderedQueryable<TEntity> query, Expression<Func<TEntity, TKey>> keySelector, System.ComponentModel.ListSortDirection? sortOrder)
        {
            var sortOrderSafe = sortOrder.HasValue ? sortOrder.Value : System.ComponentModel.ListSortDirection.Ascending;
            return sortOrderSafe == System.ComponentModel.ListSortDirection.Ascending ? query.ThenBy(keySelector) : query.ThenByDescending(keySelector);
        }

        public static IOrderedEnumerable<TEntity> OrderBy<TEntity, TKey>(this IEnumerable<TEntity> query, Func<TEntity, TKey> keySelector, System.ComponentModel.ListSortDirection? sortOrder)
        {
            var sortOrderSafe = sortOrder.HasValue ? sortOrder.Value : System.ComponentModel.ListSortDirection.Ascending;
            return sortOrderSafe == System.ComponentModel.ListSortDirection.Ascending ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);
        }

        public static IQueryable<TEntity> WhereAny<TEntity, TElement>(this IQueryable<TEntity> query, IEnumerable<TElement> elements,
            Func<TElement, Expression<Func<TEntity, bool>>> getOption)
            where TEntity : class
        {
            var elementsSafe = elements != null ? elements.ToList() : new List<TElement>();
            if (elementsSafe.Count == 0)
            {
                return query;
            }

            var options = elements.Select(i => getOption(i)).ToArray();

            var predicate = PredicateBuilder.New<TEntity>(options[0]);
            for (int i = 1; i < options.Length; i++)
            {
                predicate.Or(options[i]);
            }

            return query.Where(predicate);
        }

        public static IQueryable<TEntity> WhereAll<TEntity, TElement>(this IQueryable<TEntity> query, IEnumerable<TElement> elements,
            Func<TElement, Expression<Func<TEntity, bool>>> getOption)
            where TEntity : class
        {
            var elementsSafe = elements != null ? elements.ToList() : new List<TElement>();
            if (elementsSafe.Count == 0)
            {
                return query;
            }

            var options = elements.Select(i => getOption(i)).ToArray();

            var predicate = PredicateBuilder.New<TEntity>(options[0]);
            for (int i = 1; i < options.Length; i++)
            {
                predicate.And(options[i]);
            }

            return query.Where(predicate);
        }

        public static IPaginationResult<TEntity> ToPaginationResult<TEntity>(this IQueryable<TEntity> query, IPagination pagination, int maxLimit = MaxLimit)
            where TEntity : class
        {
            if (pagination == null)
            {
                throw new ArgumentNullException("The pagination cannot be null when trying to convert to PaginationResult");
            }

            var countTotal = query.Count();

            // by default limit to max 50 results
            var limit = pagination.Limit == 0 ? maxLimit : Math.Min(pagination.Limit, maxLimit);
            var page = Math.Max(pagination.Page, 1);
            var skip = (page - 1) * limit;

            var queryWithPagination = query;
            if (skip != 0)
            {
                queryWithPagination = queryWithPagination.Skip(skip);
            }

            if (limit != 0)
            {
                queryWithPagination = queryWithPagination.Take(limit);
            }

            int remainder;
            var lastPage = Math.DivRem(countTotal, limit, out remainder) + Math.Min(1, remainder);

            var result = new PaginationResult<TEntity>()
            {
                PerPage = limit,
                LastPage = lastPage,
                CurrentPage = page,
                Total = countTotal,
                // its important to execute the query here
                Data = queryWithPagination.ToList()
            };

            return result;
        }

        public static IPaginationResult<TModel> ToMappedPaginationResult<TEntity, TModel>(this IQueryable<TEntity> query,
            IPagination pagination, int maxLimit = MaxLimit)
            where TEntity : class
            where TModel : class
        {
            var paginationResult = query.ToPaginationResult(pagination, maxLimit);
            return Mapper.Map<IPaginationResult<TEntity>, PaginationResult<TModel>>(paginationResult);
        }

        public static IWrappedResponse<TModel> GetBy<TEntity, TModel>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> predicate, bool ignoreQueryFilters = false)
                    where TEntity : class
                    where TModel : class
        {

            var query = repository.FindAll()
                .Where(predicate);

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            var entity = query
                .ProjectTo<TModel>(Mapper.ConfigurationProvider)
                .SingleOrDefault();

            if (entity == null)
            {
                return new WrappedResponse<TModel>()
                {
                    ResultType = ResultType.NotFound,
                    Data = null
                };
            }

            return new WrappedResponse<TModel>()
            {
                ResultType = ResultType.Ok,
                Data = entity
            };
        }

        public static IWrappedResponse<TModel> GetById<TEntity, TModel>(this IRepository<TEntity> repository, int id, bool ignoreQueryFilters = false)
            where TEntity : class
            where TModel : class
        {

            var query = repository.FindAll()
                .Where(i => Microsoft.EntityFrameworkCore.EF.Property<int>(i, "Id") == id);

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            var entity = query
                .ProjectTo<TModel>(Mapper.ConfigurationProvider)
                .SingleOrDefault();

            if (entity == null)
            {
                return new WrappedResponse<TModel>()
                {
                    ResultType = ResultType.NotFound,
                    Data = null
                };
            }

            return new WrappedResponse<TModel>()
            {
                ResultType = ResultType.Ok,
                Data = entity
            };
        }

        public static bool ExistsById<TEntity>(this IRepository<TEntity> repository, int id, bool ignoreQueryFilters = false)
            where TEntity : class
        {

            var query = repository.FindAll()
                .Where(i => Microsoft.EntityFrameworkCore.EF.Property<int>(i, "Id") == id);

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            return query.Any();
        }
        
        public static bool Exists<TEntity>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> predicate, bool ignoreQueryFilters = false)
            where TEntity : class
        {
            var query = repository
                .FindAll()
                .Where(predicate);
            
            if(ignoreQueryFilters)
                    query=query.IgnoreQueryFilters();
            
            return query.Any();
        }

        public static IWrappedResponse<TModel> Create<TEntity, TCreateRequest, TModel>(this IRepository<TEntity> repository, TCreateRequest createRequest)
            where TEntity : class
            where TCreateRequest : class
            where TModel : class
        {
            if (createRequest == null)
            {
                throw new ArgumentNullException("The createRequest cannot be null when trying to create entity from request");
            }

            TEntity entity = Mapper.Map<TEntity>(createRequest);

            repository.Create(entity);

            var count = repository.Save();

            if (count == 0)
            {
                return new WrappedResponse<TModel>()
                {
                    ResultType = ResultType.Failed,
                    Data = null
                };
            }

            var result = Mapper.Map<TEntity, TModel>(entity);

            // TODO dirty hack, should be done via id interface on Olma.Models
            dynamic dynamicResult = result;

            return new WrappedResponse<TModel>()
            {
                ResultType = ResultType.Created,
                Id = dynamicResult.Id,
                Data = result
            };
        }

        /// <summary>
        /// Helper method for updates.
        /// The method first checks if an entity with the given id exists,
        /// then updates the entity based on the update request.
        /// Afterwards it maps the updated entity to the provided business model type,
        /// which is then returned inside a wrapped response.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to be updated.</typeparam>
        /// <typeparam name="TUpdateRequest">An update request that is mapped to the entity type.</typeparam>
        /// <typeparam name="TModel">The business model that corresponds to the entity type.</typeparam>
        /// <param name="repository"></param>
        /// <param name="id"></param>
        /// <param name="updateRequest"></param>
        /// <returns>Test</returns>
        public static IWrappedResponse<TModel> Update<TEntity, TUpdateRequest, TModel>(this IRepository<TEntity> repository, int id, TUpdateRequest updateRequest)
            where TEntity : class
            where TUpdateRequest : class
            where TModel : class
        {
            if (updateRequest == null)
            {
                throw new ArgumentNullException("The createRequest cannot be null when trying to create entity from request");
            }

            var entity = repository.GetByKey(id);

            if (entity == null)
            {
                return new WrappedResponse<TModel>()
                {
                    ResultType = ResultType.NotFound,
                    Data = null,
                    // TODO think about whether the error should be generate here or not
                    // as we provide all the information to create a more context specific error message
                    // from where this extension method is called via wrapped response
                    Errors = new[] { $"There was no {typeof(TEntity).Name} with the id: {id}" }
                };
            }

            Mapper.Map<TUpdateRequest, TEntity>(updateRequest, entity);

            var count = repository.Save();

            // on updates if save succeeds but no row was updated something went wrong
            if (count == 0)
            {
                return new WrappedResponse<TModel>()
                {
                    ResultType = ResultType.Failed,
                    Data = null
                };
            }

            var result = Mapper.Map<TEntity, TModel>(entity);

            return new WrappedResponse<TModel>()
            {
                ResultType = ResultType.Updated,
                Data = result
            };
        }

        public static IWrappedResponse Delete<TEntity>(this IRepository<TEntity> repository, int id)
            where TEntity : class
        {
            var entity = repository.GetByKey(id);

            if (entity == null)
            {
                return new WrappedResponse()
                {
                    ResultType = ResultType.NotFound,
                };
            }

            repository.Delete(entity);
            var count = repository.Save();

            // when count is zero but entity was found prior, return delete was successful
            if (count == 0)
            {
                return new WrappedResponse()
                {
                    ResultType = ResultType.Deleted,
                };
            }

            return new WrappedResponse()
            {
                ResultType = ResultType.Deleted,
            };
        }        
    }
}
