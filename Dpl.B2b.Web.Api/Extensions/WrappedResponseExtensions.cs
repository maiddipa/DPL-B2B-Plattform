using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Rules;
using Dpl.B2b.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Dpl.B2b.Web.Api.Extensions
{
    public static class WrappedResponseExtensions
    {
        public static async Task<ActionResult<T>> Convert<T>(this Task<IWrappedResponse<T>> wrappedResponse,
            ControllerBase controller, string actionName = null)
        {
            switch (wrappedResponse.Result.ResultType)
            {
                case ResultType.Ok:
                {
                    return controller.Ok(wrappedResponse.Result.Data);
                }
                case ResultType.Created:
                {
                    // TODO implement ability to handle more than action name
                    return controller.CreatedAtAction(actionName, new {wrappedResponse.Id},
                        wrappedResponse.Result.Data);
                }
                case ResultType.Updated:
                {
                    return controller.Ok(wrappedResponse.Result.Data);
                }
                case ResultType.Deleted:
                {
                    return controller.Ok();
                }
                case ResultType.NotFound:
                {
                    return controller.NotFound();
                }
                case ResultType.Failed when wrappedResponse.Result.State is ServiceState serviceState:
                {
                    return controller.StatusCode(500, serviceState);
                }
                case ResultType.Failed:
                {
                    //return controller.Problem(String.Join(Environment.NewLine, wrappedResponse.Errors));
                    return controller.StatusCode(500);
                }
                case ResultType.BadRequest when wrappedResponse.Result.State is RuleStateDictionary ruleStates:
                {
                    var list = new List<RuleState>();

                    foreach (var ruleStatesKey in ruleStates.Keys) list.Add(ruleStates[ruleStatesKey]);

                    return controller.BadRequest(new DplProblemDetails {RuleStates = list.ToArray()});
                }
                case ResultType.BadRequest:
                {
                    return controller.BadRequest();
                }
                case ResultType.Forbidden:
                {
                    return controller.Forbid();
                }
                default:
                {
                    throw new NotSupportedException(
                        $"The provided wrapped response type is not supported: {wrappedResponse.Result.ResultType}");
                }
            }
        }

        public static async Task<ActionResult> Convert(this Task<IWrappedResponse> wrappedResponse,
            ControllerBase controller)
        {
            switch (wrappedResponse.Result.ResultType)
            {
                case ResultType.Ok:
                case ResultType.Deleted:
                {
                    return controller.Ok();
                }

                default:
                {
                    throw new NotSupportedException(
                        $"The provided wrapped response type is not supported: {wrappedResponse.Result.ResultType}");
                }
            }
        }

        public static async Task<ActionResult<T>> Convert<T>(this Task<IWrappedResponse> wrappedResponseTask,
            ControllerBase controller, string actionName = null) where T : class
        {
            switch (wrappedResponseTask.Result.ResultType)
            {
                case ResultType.Ok:
                {
                    var response = wrappedResponseTask.Result as IWrappedResponse<T>;
                    return controller.Ok(response?.Data);
                }
                case ResultType.Deleted:
                {
                    return controller.Ok();
                }
                case ResultType.Failed when wrappedResponseTask.Result.State is ServiceState serviceState:
                {
                    return controller.StatusCode(500, new DplProblemDetails {ServiceStates = new[] {serviceState}});
                }
                case ResultType.Failed:
                {
                    return controller.BadRequest();
                }
                case ResultType.BadRequest when wrappedResponseTask.Result.State is RuleStateDictionary ruleStates:
                {
                    var list = new List<RuleState>();

                    foreach (var ruleStatesKey in ruleStates.Keys) list.Add(ruleStates[ruleStatesKey]);

                    return controller.BadRequest(new DplProblemDetails {RuleStates = list.ToArray()});
                }
                case ResultType.BadRequest when wrappedResponseTask.Result.State is ServiceState serviceState:
                {
                    return controller.BadRequest(new DplProblemDetails {ServiceStates = new[] {serviceState}});
                }
                case ResultType.BadRequest:
                {
                    return controller.BadRequest();
                }
                case ResultType.Forbidden:
                {
                    return controller.Forbid();
                }
                case ResultType.Created:
                {
                    // TODO implement ability to handle more than action name
                    var response = wrappedResponseTask.Result as IWrappedResponse<T>;
                    return controller.CreatedAtAction(actionName, new {wrappedResponseTask.Id}, response?.Data);
                }
                case ResultType.Updated:
                {
                    var response = wrappedResponseTask.Result as IWrappedResponse<T>;
                    return controller.Ok(response?.Data);
                }
                case ResultType.NotFound:
                {
                    var response = wrappedResponseTask.Result as IWrappedResponse<T>;
                    return controller.NotFound(response);
                }
                default:
                {
                    throw new NotSupportedException(
                        $"The provided wrapped response type is not supported: {wrappedResponseTask.Result.ResultType}");
                }
            }
        }
    }
}