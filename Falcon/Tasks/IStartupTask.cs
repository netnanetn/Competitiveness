namespace Falcon.Tasks 
{
    public interface IStartupTask 
    {
        void Execute();

        int Order { get; }
    }
}
