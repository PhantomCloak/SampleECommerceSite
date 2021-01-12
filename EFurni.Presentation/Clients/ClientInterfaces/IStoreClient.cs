using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;
using EFurni.Shared.Types;

namespace EFurni.Presentation.Clients.ClientInterfaces
{
    public interface IStoreClient
    {
        public Task<IEnumerable<MatchTuple<EntityDistanceInfo<StoreDto>, int>>> GetNearestStores(string zipCode,int[] productIds);
    }
}