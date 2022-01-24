using System;
using System.Data;

namespace TeamsPersistanceCenter.Models.BaseModels
{
    /// <summary>
    /// SQL Server Unique Constraint Violation Error
    /// </summary>
    public sealed class SqlUniqueConstraintViolationError : DataException
    {
        /// <inheritdoc cref="ApplicationException"/>
        public SqlUniqueConstraintViolationError(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
