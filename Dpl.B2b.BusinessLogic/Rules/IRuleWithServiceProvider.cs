using System;
using Microsoft.Extensions.DependencyInjection;

namespace Dpl.B2b.BusinessLogic.Rules
{
    public interface IRuleWithServiceProvider<TRule>:IRuleWithServiceProvider
    {
        /// <summary>
        /// Changes the ServiceScope. Use responsibly
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public new TRule AssignServiceProvider(IServiceProvider serviceProvider);
    }
    
    public interface IRuleWithServiceProvider:IRule
    {
        /// <summary>
        /// The defined ServiceProvider 
        /// </summary>
        public IServiceProvider ServiceProvider { get; }
        
        /// <summary>
        /// Changes the ServiceScope. Use responsibly
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public IRule AssignServiceProvider(IServiceProvider serviceProvider);
    }
}