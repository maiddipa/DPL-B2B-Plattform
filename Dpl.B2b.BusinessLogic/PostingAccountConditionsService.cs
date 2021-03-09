using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Ltms = Dpl.B2b.Dal.Ltms;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    public class PostingAccountConditionsService : BaseService, IPostingAccountConditionsService
    {
        private readonly IRepository<Ltms.Conditions> _ltmsConditionsRepo;
        private readonly IRepository<Olma.LoadCarrier> _olmaLoadCarrierRepo;

        public PostingAccountConditionsService(IAuthorizationDataService authData, IMapper mapper,
            IRepository<Ltms.Conditions> ltmsConditionsRepo,
            IRepository<Olma.LoadCarrier> olmaLoadCarrierRepo) : base(authData, mapper)
        {
            _ltmsConditionsRepo = ltmsConditionsRepo;
            _olmaLoadCarrierRepo = olmaLoadCarrierRepo;
        }

        public async Task<IWrappedResponse<IEnumerable<PostingAccountCondition>>> Search(
            PostingAccountConditionsSearchRequest request)
        {
            var allLoadCarriers = _olmaLoadCarrierRepo.FindAll().Include(lc => lc.Quality).Include(lc => lc.Type)
                .ToList();
            var conditions = _ltmsConditionsRepo.FindByCondition(c =>
                    c.AccountId == request.RefLtmsAccountId && c.ValidFrom.Date <= DateTime.Today &&
                    (!c.ValidUntil.HasValue || c.ValidUntil.Value.Date >= DateTime.Today))
                .Include(t => t.Term)
                .Include(d => d.BookingDependent).ToList();

            var postingAccountConditions = new List<PostingAccountCondition>();

            foreach (var condition in conditions)
            {
                var loadCarriers =
                    allLoadCarriers.Where(lc => condition.BookingDependent != null && lc.Type.RefLtmsArticleId == condition.BookingDependent.ArticleId);

                if (condition.BookingDependent?.QualityId != null)
                {
                    loadCarriers = loadCarriers.Where(flc =>
                        flc.Quality.RefLtmsQualityId == condition.BookingDependent.QualityId);
                }

                postingAccountConditions.AddRange(loadCarriers.Select(loadCarrier => new PostingAccountCondition
                {
                    Type = condition.TermId.GetConditionType(),
                    Amount = condition.BookingDependent?.Amount,
                    MaxQuantity = condition.Term.UpperLimit,
                    MinQuantity = condition.Term.LowerLimit,
                    LoadCarrierId = loadCarrier.Id
                }));
            }

            return new WrappedResponse<IEnumerable<PostingAccountCondition>>
            {
                Data = postingAccountConditions,
                ResultType = ResultType.Ok
            };
        }
    }
}