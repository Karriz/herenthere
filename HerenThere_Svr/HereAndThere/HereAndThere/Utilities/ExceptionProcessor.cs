using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;

namespace HereAndThere.Utilities
{
    public class ExceptionProcessor
    {
        public static string Process(Exception exception, string source = null)
        {
            var validationException = exception as DbEntityValidationException;
            if (validationException != null)
            {
                var lines = validationException.EntityValidationErrors.Select(
                        x => new
                        {
                            name = x.Entry.Entity.GetType().Name.Split('_')[0],
                            errors = x.ValidationErrors.Select(y => y.PropertyName + ":" + y.ErrorMessage)
                        })
                    .Select(x => string.Format("{0} => {1}", x.name, string.Join(",", x.errors)));
                var text = string.Join("\r\n", lines);

                return text;
            }
            var updateException = exception as DbUpdateException;
            if (updateException != null)
            {
                Exception innerException = updateException;
                while (innerException.InnerException != null) innerException = innerException.InnerException;
                if (innerException != updateException)
                    if (innerException is SqlException)
                    {
                        var result = ProcessSqlExceptionMessage(innerException.Message);
                        return result;
                    }
                var entities = updateException.Entries.Select(x => x.Entity.GetType().Name.Split('_')[0])
                    .Distinct()
                    .Aggregate((a, b) => a + ", " + b);

                return string.Format("{0} => {1}", innerException.Message, entities);
            }
            //for any other exception, just get the full message
            var msg = GetErrorMessages(exception).Aggregate((a, b) => a + "\r\n" + b);

            return msg;
        }

        /// <summary>
        ///     Parses the specified exception and creates a new one.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static Exception Parse(Exception exception, string source = null)
        {
            var result = Process(exception, source);
            return new Exception(result);
        }

        /// <summary>
        ///     Processes the SQL exception message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        private static string ProcessSqlExceptionMessage(string message)
        {
            //todo: this is from sqlException oh. alter the above code to handle that particular exception type
            //todo: use locale instead
            if (message.Contains("unique index"))
                return "Data is constrained to be unique. This entry will violate that and has been rejected";
            if (message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                return "The record to be deleted has been referenced by other records and thus cannot be delete";
            if (message.Contains("Cannot insert duplicate key in object "))
                return
                    "A similar record already exists so cannot allow the creation of a new one. Consider using UPDATE instead.";
            return message;
        }

        /// <summary>
        ///     Gets the error messages.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        private static IEnumerable<string> GetErrorMessages(Exception exception)
        {
            if (exception.InnerException != null)
                foreach (var msg in GetErrorMessages(exception.InnerException))
                    yield return msg;
            yield return exception.Message;
        }
    }
}