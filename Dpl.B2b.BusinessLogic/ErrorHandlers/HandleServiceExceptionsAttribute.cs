using Dpl.B2b.Contracts;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace Dpl.B2b.BusinessLogic.ErrorHandlers
{
    [PSerializable]
    public class HandleServiceExceptionsAttribute : OnExceptionAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            var errorHandler = ErrorHandler.Create();
            errorHandler.LogException(args.Exception);
            
            var methodReturnType = ((System.Reflection.MethodInfo)args.Method).ReturnType;

            if (methodReturnType.IsConstructedGenericType
                && methodReturnType.GenericTypeArguments.Length > 0
                && methodReturnType.GenericTypeArguments[0] == typeof(IWrappedResponse))
            {
                var response = new WrappedResponse
                {
                    ResultType = ResultType.Failed,
                    State = errorHandler
                        .TranslateException(args.Exception)
                        .GetServiceState()
                };

                args.FlowBehavior = FlowBehavior.Return;
                args.ReturnValue = response;
            }
            else
            {
                args.FlowBehavior = FlowBehavior.RethrowException;
            }
        }
    }
}