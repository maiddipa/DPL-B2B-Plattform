namespace Dpl.B2b.Dal
{
    /// <summary>
    /// Used to be able to inject all contexts at once, NOT to be sued from BusinessLogic services
    /// </summary>    
    public class DbContextsWrapper
    {
        public OlmaDbContext Olma { get; }

        public DbContextsWrapper(OlmaDbContext olma)
        {
            Olma = olma;
        }
    }
}
