using Olma = Dpl.B2b.Dal.Models;
using Ltms = Dpl.B2b.Dal.Ltms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace Dpl.B2b.BusinessLogic.Sync
{

    public class SyncService : BaseService, ISyncService
    {
        private readonly IRepository<Ltms.Transactions> _ltmsTransactionsRepo;
        private readonly IRepository<Ltms.Accounts> _ltmsAccountsRepo;
        private readonly IRepository<Olma.LoadCarrier> _olmaLoadCarrierRepo;
        private readonly IRepository<Ltms.Pallet> _ltmsPalletRepo;
        private readonly List<int> _ltmsAccountIds;
        private readonly List<Olma.LoadCarrier> _loadCarrieres;
        private readonly List<Ltms.Pallet> _pallets;

        public SyncService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Ltms.Accounts> ltmsAccountsRepo, 
            IRepository<Olma.LoadCarrier> olmaLoadCarrierRepo, 
            IRepository<Ltms.Pallet> ltmsPalettRepo, 
            IRepository<Ltms.Transactions> ltmsTransactionsRepo) : base(authData,mapper)
        {
            _ltmsAccountsRepo = ltmsAccountsRepo;
            _olmaLoadCarrierRepo = olmaLoadCarrierRepo;
            _ltmsPalletRepo = ltmsPalettRepo;
            _ltmsTransactionsRepo = ltmsTransactionsRepo;
            _ltmsAccountIds = ltmsAccountsRepo.FindAll().AsNoTracking().Select(a => a.Id).ToList(); //TODO Discuss Do we need to filter locked or inactive accounts
            _loadCarrieres = _olmaLoadCarrierRepo.FindAll().AsNoTracking().ToList();
            _pallets = _ltmsPalletRepo.FindAll().ToList();
        }

        public Task<bool> CreateLtmsBookings(IEnumerable<PostingRequestsCreateRequest> postingRequests)
        {
            Console.WriteLine(string.Format("{0} requests to process",postingRequests.Count()));


            foreach (var request in postingRequests)
            {
                foreach (var requestPosition in request.Positions)
                {
                    var pallet = ConvertLoadCarrierIdToPallet(requestPosition.LoadCarrierId);
                    var sourceBooking = Mapper.Map<Ltms.Bookings>(request);
                    sourceBooking.ArticleId = pallet.ArticleId;
                    sourceBooking.QualityId = pallet.QualityId;
                    sourceBooking.AccountDirection = "Q";
                    sourceBooking.Quantity = -1 * requestPosition.LoadCarrierQuantity;
                    sourceBooking.BookingTypeId = ConvertReasonToLtmsBookingType(request.Reason, sourceBooking: true);
                    if (LtmsAccountExists(request.SourceRefLtmsAccountId))
                        sourceBooking.AccountId = request.SourceRefLtmsAccountId;

                    var destinationBooking = Mapper.Map<Ltms.Bookings>(request);
                    destinationBooking.ArticleId = pallet.ArticleId;
                    destinationBooking.QualityId = pallet.QualityId;
                    destinationBooking.AccountDirection = "Z";
                    if (LtmsAccountExists(request.DestinationRefLtmsAccountId))
                        destinationBooking.AccountId = request.DestinationRefLtmsAccountId;
                    destinationBooking.BookingTypeId = ConvertReasonToLtmsBookingType(request.Reason, sourceBooking: false);

                    var transaction = Mapper.Map<PostingRequestsCreateRequest, Ltms.Transactions>(request);
                    transaction.Bookings = new List<Ltms.Bookings>() { sourceBooking, destinationBooking };
                    _ltmsTransactionsRepo.Create(transaction);
                    //TODO Set correct TransactionType
                }
            }

            try
            {
                //TODO Do we need DB Transaction here ?
                _ltmsTransactionsRepo.Save();
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Task.FromResult(false);
            }
        }

        #region Helper
        private bool LtmsAccountExists(int ltmsAccountId)
        {
            return ltmsAccountId != 0 && _ltmsAccountIds.Contains(ltmsAccountId);
        }

        private Ltms.Pallet ConvertLoadCarrierIdToPallet(int loadCarrierId)
        {
            var pallet = new Ltms.Pallet() { ArticleId = 0, QualityId = 0 }; //HACK Hardcoded unknown Pallet
            var loadCarrier = _loadCarrieres.SingleOrDefault(l => l.Id == loadCarrierId);
            if (loadCarrier == null)
            {
                Console.WriteLine(string.Format("LoadCarrierId {0} does not exist", loadCarrierId));
                return pallet;
            }
                

            pallet = _pallets.SingleOrDefault(p => p.Id == loadCarrier.RefLtmsPalletId);
            if (pallet == null)
                Console.WriteLine(string.Format("PalletId {0} does not exist", loadCarrier.RefLtmsPalletId));
            return pallet;
        }

        private string ConvertReasonToLtmsBookingType(PostingRequestReason reason, bool sourceBooking)
        { //TODO Centralize Mapping (types are already mapped in AutoMapperProfile
            AccountingRecordType result = AccountingRecordType.U; //Default undefined
            switch (reason)
            {
                case PostingRequestReason.Transfer:
                    result = sourceBooking ? AccountingRecordType.KUA : AccountingRecordType.KUZ;
                    break;
                case PostingRequestReason.Voucher:
                    result = sourceBooking ? AccountingRecordType.GUTDPG : AccountingRecordType.AUSDPG;
                    break;
                case PostingRequestReason.LoadCarrierReceipt:
                    result = sourceBooking ? AccountingRecordType.ABH : AccountingRecordType.ANL;
                    break;
            }
            return Enum.GetName(typeof(AccountingRecordType),result);
        }
        #endregion
    }
}
