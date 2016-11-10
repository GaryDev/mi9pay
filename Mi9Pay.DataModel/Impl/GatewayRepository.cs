using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Mi9Pay.DataModel
{
    public class GatewayRepository : IGatewayRepository
    {
        #region Private member variables...

        private GatewayPayEntities _context = null;
        private GenericRepository<GatewayPaymentApp> _appRepository;

        private GenericRepository<GatewayPaymentAccount> _accountRepository;
        private GenericRepository<GatewayPaymentMerchant> _merchantRepository;
        private GenericRepository<GatewayPaymentStore> _storeRepository;

        private GenericRepository<GatewayPaymentMethod> _methodRepository;
        private GenericRepository<GatewayPaymentStorePaymentMethod> _storePaymentMethodRepository;

        private GenericRepository<GatewayPaymentCustomer> _customerRepository;
        private GenericRepository<GatewayPaymentOrder> _orderRepository;        
        private GenericRepository<GatewayPaymentOrderDetail> _orderDetailRepository;
        private GenericRepository<GatewayPaymentOrderType> _orderTypeRepository;
        private GenericRepository<GatewayPaymentOrderStatus> _orderStatusRepository;

        private GenericRepository<GatewayPaymentBillWechat> _billWechatRepository;
        private GenericRepository<GatewayPaymentBillAlipay> _billAlipayRepository;

        #endregion

        public GatewayRepository()
        {
            _context = new GatewayPayEntities();
        }

        #region Public Repository Creation properties...

        /// <summary>
        /// Get/Set Property for app repository.
        /// </summary>
        public GenericRepository<GatewayPaymentApp> App
        {
            get
            {
                if (this._appRepository == null)
                    this._appRepository = new GenericRepository<GatewayPaymentApp>(_context);
                return _appRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for method repository.
        /// </summary>
        public GenericRepository<GatewayPaymentMethod> Method
        {
            get
            {
                if (this._methodRepository == null)
                    this._methodRepository = new GenericRepository<GatewayPaymentMethod>(_context);
                return _methodRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for account repository.
        /// </summary>
        public GenericRepository<GatewayPaymentAccount> Account
        {
            get
            {
                if (this._accountRepository == null)
                    this._accountRepository = new GenericRepository<GatewayPaymentAccount>(_context);
                return _accountRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for merchant repository.
        /// </summary>
        public GenericRepository<GatewayPaymentMerchant> Merchant
        {
            get
            {
                if (this._merchantRepository == null)
                    this._merchantRepository = new GenericRepository<GatewayPaymentMerchant>(_context);
                return _merchantRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for store repository.
        /// </summary>
        public GenericRepository<GatewayPaymentStore> Store
        {
            get
            {
                if (this._storeRepository == null)
                    this._storeRepository = new GenericRepository<GatewayPaymentStore>(_context);
                return _storeRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for store payment method repository.
        /// </summary>
        public GenericRepository<GatewayPaymentStorePaymentMethod> StorePaymentMethod
        {
            get
            {
                if (this._storePaymentMethodRepository == null)
                    this._storePaymentMethodRepository = new GenericRepository<GatewayPaymentStorePaymentMethod>(_context);
                return _storePaymentMethodRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for customer repository.
        /// </summary>
        public GenericRepository<GatewayPaymentCustomer> Customer
        {
            get
            {
                if (this._customerRepository == null)
                    this._customerRepository = new GenericRepository<GatewayPaymentCustomer>(_context);
                return _customerRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for order repository.
        /// </summary>
        public GenericRepository<GatewayPaymentOrder> Order
        {
            get
            {
                if (this._orderRepository == null)
                    this._orderRepository = new GenericRepository<GatewayPaymentOrder>(_context);
                return _orderRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for order detail repository.
        /// </summary>
        public GenericRepository<GatewayPaymentOrderDetail> OrderDetail
        {
            get
            {
                if (this._orderDetailRepository == null)
                    this._orderDetailRepository = new GenericRepository<GatewayPaymentOrderDetail>(_context);
                return _orderDetailRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for order type repository.
        /// </summary>
        public GenericRepository<GatewayPaymentOrderType> OrderType
        {
            get
            {
                if (this._orderTypeRepository == null)
                    this._orderTypeRepository = new GenericRepository<GatewayPaymentOrderType>(_context);
                return _orderTypeRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for order status repository.
        /// </summary>
        public GenericRepository<GatewayPaymentOrderStatus> OrderStatus
        {
            get
            {
                if (this._orderStatusRepository == null)
                    this._orderStatusRepository = new GenericRepository<GatewayPaymentOrderStatus>(_context);
                return _orderStatusRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for wechat bill repository.
        /// </summary>
        public GenericRepository<GatewayPaymentBillWechat> BillWechat
        {
            get
            {
                if (this._billWechatRepository == null)
                    this._billWechatRepository = new GenericRepository<GatewayPaymentBillWechat>(_context);
                return _billWechatRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for alipay bill repository.
        /// </summary>
        public GenericRepository<GatewayPaymentBillAlipay> BillAlipay
        {
            get
            {
                if (this._billAlipayRepository == null)
                    this._billAlipayRepository = new GenericRepository<GatewayPaymentBillAlipay>(_context);
                return _billAlipayRepository;
            }
        }

        #endregion

        #region Public member methods...

        public IEnumerable<GatewayPaymentMethodTypeJoinResult> GetPaymentMethodCombinationByStore(int storeId)
        {
            var queryResult = from gpmd in _context.GatewayPaymentMethodTypeJoin
                              join gpspm in _context.GatewayPaymentStorePaymentMethod on gpmd.UniqueId equals gpspm.GatewayPaymentMethodTypeJoin
                              join gps in _context.GatewayPaymentStore on gpspm.GatewayPaymentStore equals gps.UniqueId
                              where gps.StoreId == storeId
                              select new GatewayPaymentMethodTypeJoinResult
                              {
                                  PaymentCombine = gpmd,
                                  StorePaymentMethod = gpspm.UniqueId,
                                  IsDefault = gpspm.PaymentMethodDefault.HasValue ? gpspm.PaymentMethodDefault.Value : false
                              };
            return queryResult;
        }
        
        public GatewayPaymentAccount GetGatewayPaymentAccount(int storeId, string payMethodCode)
        {
            GatewayPaymentAccount account = (from gpa in _context.GatewayPaymentAccount
                                            join gpm in _context.GatewayPaymentMerchant on gpa.GatewayPaymentMerchant equals gpm.UniqueId
                                            join gps in _context.GatewayPaymentStore on gpm.UniqueId equals gps.GatewayPaymentMerchant
                                            join gpmd in _context.GatewayPaymentMethod on gpa.GatewayPaymentMethod equals gpmd.UniqueId
                                            where gps.StoreId == storeId && string.Compare(gpmd.Code, payMethodCode, true) == 0
                                            select gpa).SingleOrDefault();
            return account;
        }

        public GatewayPaymentOrder GetGatewayPaymentOrder(string invoiceNumber, int storeId, Guid payMethodJoin)
        {
            GatewayPaymentOrder order = (from gpo in _context.GatewayPaymentOrder
                                         join gpmd in _context.GatewayPaymentStorePaymentMethod on gpo.GatewayPaymentStorePaymentMethod equals gpmd.UniqueId
                                         where gpo.OrderNumber == invoiceNumber && gpo.StoreID == storeId && gpmd.GatewayPaymentMethodTypeJoin == payMethodJoin
                                         select gpo).SingleOrDefault();
            return order;
        }

        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now,
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }

        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false;
        #endregion

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
