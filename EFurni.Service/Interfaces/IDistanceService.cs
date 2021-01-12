using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Shared.Models;

namespace EFurni.Services
{
    public interface IZipDistanceService
    {
        Task<IEnumerable<ZipCodeLocationPair>> GetNearestZipCodes(string originZipCode,string[] destZipCodes);
    }
}