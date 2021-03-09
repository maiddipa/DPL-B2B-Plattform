using Dpl.B2b.Contracts;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Dpl.B2b.Common;
using Olma = Dpl.B2b.Dal.Models;
using Ltms = Dpl.B2b.Dal.Ltms;
using Dpl.B2b.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dpl.B2b.BusinessLogic
{
    public class LtmsReferenceLookupService : ILtmsReferenceLookupService
    {

        private readonly IRepository<LoadCarrier> _olmaLoadCarrierRepo;
        private readonly IRepository<PostingAccount> _olmaPostingAccountRepo;

        private List<LoadCarrier> _loadCarriers;

        public LtmsReferenceLookupService(
            IRepository<Olma.LoadCarrier> olmaLoadCarrierRepo,
            IRepository<Olma.PostingAccount> olmaPostingAccountRepo
        )
        {
            _olmaLoadCarrierRepo = olmaLoadCarrierRepo;
            _olmaPostingAccountRepo = olmaPostingAccountRepo;
        }

        private List<LoadCarrier> GetLoadCarriers()
        {
            if (_loadCarriers == null)
            {
                _loadCarriers = _olmaLoadCarrierRepo
                     .FindAll()
                     .Include(i => i.Type)
                     .Include(i => i.Quality)
                     .AsNoTracking()
                     .ToList();
            }
            return _loadCarriers;
        }

        public int GetLtmsAccountId(int olmaPostingAccountId)
        {
            var postingAccount = _olmaPostingAccountRepo.FindAll()
                .IgnoreQueryFilters() //TODO Check if necessary and ok to ignoreQueryFilter
                .Where(i => i.Id == olmaPostingAccountId)
                .FirstOrDefault();
            return postingAccount.RefLtmsAccountId;
        }

        public IDictionary<int, int> GetLtmsAccountIds(int[] accountIds)
        {
            return _olmaPostingAccountRepo
                .FindAll()
                .AsNoTracking()
                .Where(i => accountIds.Contains(i.Id))
                .Select(i => new { i.Id, i.RefLtmsAccountId })
                .ToDictionary(i => i.Id, i => i.RefLtmsAccountId);
        }

        public IDictionary<int, int> GetOlmaPostingAccountIds(int[] accountIds)
        {
            return _olmaPostingAccountRepo
                .FindAll()
                .AsNoTracking()
                .Where(i => accountIds.Contains(i.RefLtmsAccountId))
                .Select(i => new { i.Id, i.RefLtmsAccountId })
                .ToDictionary(i => i.RefLtmsAccountId, i => i.Id);
        }

        public IDictionary<Tuple<short, short>, int> GetOlmaLoadCarrierIds(Tuple<short, short>[] articleAndQualityIds)
        {
            return this.GetLoadCarriers()
                .Join(
                    articleAndQualityIds,
                    lc => new { ArticleId = lc.Type.RefLtmsArticleId, QualityId = lc.Quality.RefLtmsQualityId },
                    aq => new { ArticleId = aq.Item1, QualityId = aq.Item2 },
                    (value, key) => (key, value.Id)
                ).ToDictionary(i => i.key, i => i.Id);
        }

        public IDictionary<int, Tuple<short, short, short>> GetLtmsPalletInfos(int[] loadCarrierIds = null)
        {
            var loadCarriers = loadCarrierIds == null
                ? this.GetLoadCarriers()
                : this.GetLoadCarriers().Where(i => loadCarrierIds.Contains(i.Id));

            return loadCarriers
                .ToDictionary(i => i.Id, i => Tuple.Create(i.RefLtmsPalletId, i.Type.RefLtmsArticleId, i.Quality.RefLtmsQualityId));
        }

        public IDictionary<int, short> GetLtmsArticleIds(int[] loadCarrierTypeIds)
        {
            return this.GetLoadCarriers()
                .Select(i => i.Type)
                .GroupBy(x => x.Id)
                .Select(g => g.First())
                .Where(i => loadCarrierTypeIds.Contains(i.Id))
                .ToDictionary(i => i.Id, i => i.RefLtmsArticleId);
        }
    }
}
