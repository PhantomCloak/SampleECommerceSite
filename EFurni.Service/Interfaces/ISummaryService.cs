using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Shared.Models;

namespace EFurni.Services
{
    public interface ISummaryService
    {
        Task<IEnumerable<Info>> GetAllInformationAsync();
        Task<Info> GetInformationByNameAsync(string infoName);
        Task<Info[]> GenerateInfoAsync();
    }
}