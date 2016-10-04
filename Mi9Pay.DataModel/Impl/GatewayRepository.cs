using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;

namespace Mi9Pay.DataModel
{
    public class GatewayRepository : IGatewayRepository
    {
        #region Private member variables...

        private GatewayPayEntities _context = null;
        private GenericRepository<GatewayPaymentApp> _appRepository;
        private GenericRepository<GatewayPaymentMethod> _methodRepository;
        private GenericRepository<GatewayPaymentAccount> _accountRepository;
        #endregion

        public GatewayRepository()
        {
            _context = new GatewayPayEntities();
        }

        #region Public Repository Creation properties...

        /// <summary>
        /// Get/Set Property for product repository.
        /// </summary>
        public GenericRepository<GatewayPaymentApp> AppRepository
        {
            get
            {
                if (this._appRepository == null)
                    this._appRepository = new GenericRepository<GatewayPaymentApp>(_context);
                return _appRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<GatewayPaymentMethod> MethodRepository
        {
            get
            {
                if (this._methodRepository == null)
                    this._methodRepository = new GenericRepository<GatewayPaymentMethod>(_context);
                return _methodRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for token repository.
        /// </summary>
        public GenericRepository<GatewayPaymentAccount> AccountRepository
        {
            get
            {
                if (this._accountRepository == null)
                    this._accountRepository = new GenericRepository<GatewayPaymentAccount>(_context);
                return _accountRepository;
            }
        }
        #endregion

        #region Public member methods...
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
