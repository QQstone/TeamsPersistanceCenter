using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TeamsPersistanceCenter.Api.Infrastructure.ActionFilters
{
    /// <summary>
    /// Checks whether <see cref="ControllerBase.ModelState">ModelState</see> is valid before executing the action.
    /// If the <c>ModelState</c> is not valid, returns an HTTP 400 response containing a <see cref="ValidationProblemDetails" /> body.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public bool IsEnabled { get; }

        public ValidateModelStateAttribute(bool isEnabled = true)
        {
            IsEnabled = isEnabled;
        }

        /// <summary>
        /// Gets a dictionary of error messages from the supplied <see cref="ModelStateDictionary"/>.
        /// Note: This logic is similar to <see cref="SerializableError"/> class. The <see cref="SerializableError"/> class
        /// ignores <see cref="ModelError.Exception"/> properties and only returns error messages from
        /// <see cref="ModelError.ErrorMessage"/> properties.  However, OData controller model binding errors are contained
        /// in <see cref="ModelError.Exception"/> properties. This method returns <see cref="ModelError.Exception"/> error message,
        /// if <see cref="ModelError.ErrorMessage"/> is empty.
        /// </summary>
        /// <param name="modelState">The <see cref="ModelStateDictionary"/> to collect errors from.</param>
        /// <returns>A dictionary of error messages</returns>
        public static IDictionary<string, string[]> GetErrorMessages(ModelStateDictionary modelState)
        {
            static string GetModelErrorMessage(ModelError error) =>
                string.IsNullOrEmpty(error.ErrorMessage)
                    ? error.Exception.Message
                    : error.ErrorMessage;

            if (modelState == null)
                throw new ArgumentNullException(nameof(modelState));

            var errors = modelState
                .Select(kvp => new KeyValuePair<string, string[]>(
                    kvp.Key,
                    kvp.Value.Errors.Select(GetModelErrorMessage)
                        .ToArray())
            );

            var dict = new Dictionary<string, string[]>(errors, StringComparer.OrdinalIgnoreCase);
            return dict;
        }



        /// <summary>
        /// When model binding fails, such as because of property name mismatch, the model state
        /// contains a <see cref="ModelError"/> entry with an empty <c>ErrorMessage</c> property.
        /// In this case, the actual error message is in the <c>Exception</c> property instead.
        /// However, default serialization does not include the exception messages in the response.
        /// This method replaces all such entries so that the <c>Exception.ErrorMessage</c> is copied over to
        /// the <c>ErrorMessage</c> property.
        /// </summary>
        /// <param name="modelState">A new model state with the error message fix applied.</param>
        /// <returns></returns>
        public static ModelStateDictionary FixupModelStateErrors(ModelStateDictionary modelState)
        {
            // Make a clone and modify the clone, instead of the original
            modelState = new ModelStateDictionary(modelState);
            foreach (var entry in modelState.Values)
            {
                var errorList = entry.Errors.ToImmutableList();
                foreach (var error in errorList)
                {
                    if (string.IsNullOrEmpty(error.ErrorMessage) && error.Exception != null)
                    {
                        var index = entry.Errors.IndexOf(error);
                        entry.Errors.RemoveAt(index);
                        entry.Errors.Insert(index, new ModelError(error.Exception.Message));
                    }
                }
            }

            return modelState;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (IsEnabled && !context.ModelState.IsValid)
            {
                var modelState = FixupModelStateErrors(context.ModelState);
                context.Result = ((ControllerBase)context.Controller).ValidationProblem(modelState);
                return Task.CompletedTask;
            }

            return base.OnActionExecutionAsync(context, next);
        }
    }
}
