using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExportApp.Utility
{
    /// <summary>
    /// Represents information about Time out exception of sql server.
    /// </summary>
    public class SqlTimeoutException : System.Exception
    {


        #region Constructors and destructor


        public SqlTimeoutException() : base() { }

        public SqlTimeoutException(string messageText) : base(messageText) { }

        public SqlTimeoutException(Exception ex)
        {
            this.Data.Add("SourceExceptionMessage", ex.Message);
            this.Data.Add("SourceExceptionSource", ex.Source);
            this.Data.Add("SourceExceptionStackTrace", ex.StackTrace);
            foreach (object key in ex.Data)
                this.Data.Add(key, ex.Data[key]);
        }


        ~SqlTimeoutException() { }


        #endregion


        /// <summary>
        /// Gets or sets flag, indicating whether the connection has timed out or the sql query or
        /// sql command has timed out.
        /// </summary>
        public bool IsConnectionTimedOut { get; set; }

        /// <summary>
        /// Parameters of the query which may have caused this exception to occurr.
        /// </summary>
        public Dictionary<string, string> QueryParameters { get; set; }
    }
}

