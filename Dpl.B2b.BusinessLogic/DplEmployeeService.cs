using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic.Extensions;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class DplEmployeeService : BaseService, IDplEmployeeService
    {
        private readonly IRepository<Olma.Customer> _olmaCustomer;
        private readonly IRepository<Olma.PostingAccount> _olmaPostingAccountRepo;

        public DplEmployeeService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.Customer> olmaCustomerRepo,
            IRepository<Olma.PostingAccount> olmaPostingAccountRepo
        ) : base(authData, mapper)
        {
            _olmaCustomer = olmaCustomerRepo;
            _olmaPostingAccountRepo = olmaPostingAccountRepo;
        }

        public async Task<IWrappedResponse> SearchCustomers(string searchTerm)
        {
            if (searchTerm == null)
            {
                return BadRequest<IEnumerable<DplEmployeeCustomer>>();
            }

            var query = _olmaCustomer.FindAll()
                .AsNoTracking();

            //var customersQuery = query.Where(i => i.Name.Contains(searchTerm) || i.CustomerNumberString.Contains(searchTerm))
            //    .ProjectTo<DplEmployeeCustomer>(Mapper.ConfigurationProvider)
            //    .ToList();

            //var divisionsQuery = query.SelectMany(i => i.Divisions)
            //    .Where(d => d.Name.Contains(searchTerm) || d.ShortName.Contains(searchTerm))
            //    .ProjectTo<DplEmployeeCustomer>(Mapper.ConfigurationProvider)
            //    .ToList();

            //var postingAccountsQuery = _olmaPostingAccountRepo.FindAll()                
            //    .Where(i => i.DisplayName.Contains(searchTerm) || i.RefLtmsAccountIdString.Contains(searchTerm))
            //    .SelectMany(i => i.CustomerDivisions.Select(d => new DplEmployeeCustomer()
            //    {
            //        Type = DplEmployeeCustomerSelectionEntryType.PostingAccount,
            //        CustomerId = d.CustomerId,
            //        PostingAccountId = i.Id,
            //        DisplayName = d.Customer.Name + " (" + d.Customer.RefErpCustomerNumber + ")",
            //        DisplayNameLong = d.Customer.Name + " (" + d.Customer.RefErpCustomerNumber + ")" + " | " + i.DisplayName + " (" + i.RefLtmsAccountId + ")"
            //    }))
            //    // this is neccessary as multiple divisions will map to the same customer
            //    .Distinct()
            //    .ToList();

            //var combinedProjectedQuery = customersQuery
            //    .Union(divisionsQuery)
            //    .Union(postingAccountsQuery)
            //    .OrderBy(i => i.DisplayName);

            var searchTerms = searchTerm.Split(",").Select(i => i.Trim()).Where(i => i.Length > 0).ToList();

            var combinedProjectedQuery = query
                    .SelectMany(i => i.Divisions)
                    .WhereAll(searchTerms, searchTerm => d => d.Name.Contains(searchTerm) || d.ShortName.Contains(searchTerm)
                        || d.Customer.Name.Contains(searchTerm) || d.Customer.RefErpCustomerNumberString.Contains(searchTerm)
                        || d.PostingAccount.DisplayName.Contains(searchTerm) || d.PostingAccount.RefLtmsAccountIdString.Contains(searchTerm)
                        || d.Customer.Address.Street1.Contains(searchTerm) || d.Customer.Address.City.Contains(searchTerm) || d.Customer.Address.PostalCode.Contains(searchTerm)
                    )
                .ProjectTo<DplEmployeeCustomer>(Mapper.ConfigurationProvider)
                .ToList();

            var mappedResult = combinedProjectedQuery
                .ToList()
                .AsEnumerable();

            return Ok(mappedResult);
        }
    }
}
