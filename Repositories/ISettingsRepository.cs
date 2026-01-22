using System.Threading.Tasks;

namespace IsTakipWpf.Repositories
{
    public interface ISettingsRepository
    {
        Task<string> GetValueAsync(string key);
        Task<bool> SetValueAsync(string key, string value);
    }
}
