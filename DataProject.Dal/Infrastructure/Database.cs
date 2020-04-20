using System;
using System.Data;
using System.Data.Common;
using System.Data.Objects;
using DataProject.Domain;

namespace IKYS.Dal.Infrastructure
{
    public class Database : IDisposable
    {
        //TEST COMMENT
        bool _isDisposed = false;
        DbConnection _connection;
        public ObjectContext _context;
        public ObjectContext ObjectContext
        {
            get { return _context; }
        }
        public LogoDataModel Context
        {
            get { return (LogoDataModel)_context; }
            set { _context = value; }
        }
        public DbConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        public Database()
        {
            _context = new LogoDataModel(Cryptographer.LogoConnection().ConnectionString);
            _connection = _context.Connection;
            _connection.Open();       
        }

        #region methods

        public void Commit()
        {
           
        }
        public void Rollback()
        {
           
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_connection != null && _connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                    _connection.Dispose();
                }

                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }

                _isDisposed = true;
            }
        }

        #endregion
    }
}
