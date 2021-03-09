using Microsoft.Extensions.Configuration;

namespace Dpl.B2b.BusinessLogic
{
    public abstract class BaseFactory
    {
        protected readonly IConfiguration _configuration;

        protected BaseFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}