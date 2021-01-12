using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Queries.Filter;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Contract.V1.Responses;
using EFurni.Services;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;
using EFurni.Shared.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFurni.Core.Controllers.V1
{
    [ApiController]
    public class StoreAliasController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly IZipDistanceService _distanceService;

        public StoreAliasController(
            IStoreService storeService,
            IZipDistanceService distanceService)
        {
            _storeService = storeService;
            _distanceService = distanceService;
        }

        [HttpPost(ApiRoutes.StoreAlias.StoreMatch)]
        public async Task<IActionResult> MatchStores([FromBody] StoreMatchQuery matchQuery)
        {
            var stores = await _storeService.GetAllStoresAsync(new StoreFilterParams
            {
                ProductsInStock = matchQuery.ProductsInStock
            });
            var stocks = await _storeService.GetStocksAsync(matchQuery.ProductsInStock);

            var nearestZipPairs = await _distanceService.GetNearestZipCodes(matchQuery.NearestStoreFromZip, stores.Select(x => x.ZipCode).ToArray());

            //merge stores
            var nearestStore = (from d in stores
                join p in nearestZipPairs on d.ZipCode equals p.PostalCode
                orderby p.DistanceInMeter
                select new EntityDistanceInfo<StoreDto>
                {
                    Entity = d,
                    DistanceFromSourceInMeter = p.DistanceInMeter
                }).ToArray();

            var aa = (from d in nearestStore
                join p in stocks on d.Entity.StoreId equals p.StoreId
                where matchQuery.ProductsInStock.Contains(p.ProductId) && p.Quantity > 0
                select new MatchTuple<EntityDistanceInfo<StoreDto>, int>
                {
                    Entity = d,
                    Association = p.ProductId
                }).ToArray().OrderBy(x => x.Association);

            return Ok(new Response<IEnumerable<MatchTuple<EntityDistanceInfo<StoreDto>, int>>>(aa));
        }
    }
}