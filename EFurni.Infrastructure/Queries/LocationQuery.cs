using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.Data;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EFurni.Infrastructure.Queries
{
    public class LocationQuery : ILocationQuery
    {
        private readonly LocationsContext _locationDbContext;
        private readonly EFurniContext _efurniDbContext;

        public LocationQuery(LocationsContext locationDbContext, EFurniContext efurniDbContext)
        {
            _locationDbContext = locationDbContext;
            _efurniDbContext = efurniDbContext;
        }

        public async Task<IEnumerable<CountryDto>> GetCountriesAsync(PaginationParams paginationParams)
        {
            return await _locationDbContext.Countries.Select(x => new CountryDto {CountryId = x.Id, CountryName = x.CountryTag}).ToListAsync();
        }

        public async Task<IEnumerable<ProvinceDto>> GetProvincesAsync(
            string countyTag,
            PaginationParams paginationParams)
        {
            var query = from country in _locationDbContext.Countries
                join province in _locationDbContext.Province on country.Id equals province.CountryId
                where country.CountryTag == countyTag
                select new ProvinceDto()
                {
                    ProvinceId = province.ProvinceId,
                    ProvinceName = province.ProvinceName
                };

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<DistrictDto>> GetDistrictsAsync(
            string countryTag,
            string provinceName,
            PaginationParams paginationParams)
        {
            var query = from country in _locationDbContext.Countries
                join province in _locationDbContext.Province on country.Id equals province.CountryId
                join district in _locationDbContext.Districts on province.ProvinceId equals district.ProvinceId
                where country.CountryTag == countryTag && province.ProvinceName == provinceName
                select new DistrictDto {DistrictId = district.DistrictId, DistrictName = district.DistrictName};

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<NeighborhoodDto>> GetNeighborhoodsAsync(
            string countryTag,
            string provinceName,
            string districtName,
            PaginationParams paginationParams)
        {
            var query = from country in _locationDbContext.Countries
                join province in _locationDbContext.Province on country.Id equals province.CountryId
                join district in _locationDbContext.Districts on province.ProvinceId equals district.ProvinceId
                join neighborhood in _locationDbContext.Neighborhoods on district.DistrictId equals neighborhood.DistrictId
                where country.CountryTag == countryTag && province.ProvinceName == provinceName &&
                      district.DistrictName == districtName
                select new NeighborhoodDto
                {
                    NeighborhoodId = neighborhood.NeighborhoodId,
                    NeighborhoodName = neighborhood.NeighborhoodName,
                    ZipCode = neighborhood.PostalCode,
                    Longitude = (decimal) neighborhood.Longitude,
                    Latitude = (decimal) neighborhood.Latitude
                };

            return await query.ToListAsync();
        }

        public async Task<(decimal latitude, decimal longitude)> GetZipCodeCoordinateAsync(string zipCode)
        {
            var query = from neighborhood in _locationDbContext.Neighborhoods
                where neighborhood.PostalCode == zipCode
                select new
                {
                    lat = neighborhood.Latitude,
                    lon = neighborhood.Latitude
                };

            var result = await query.FirstOrDefaultAsync();

            if (result == null)
            {
                return (0, 0);
            }

            return ((decimal) result.lat, (decimal) result.lon);
        }

        public async Task<IEnumerable<ZipCodeLocationPair>> GetZipCodeDistancesAsync(
            string sourcePostalCode,
            IEnumerable<string> destinationPostalCodes)
        {
            string rawSql = $@"SELECT postal_code as PostalCode,st_distancesphere(
    (SELECT  ST_MakePoint(longitude,latitude) as distance1 FROM location.public.neighborhoods WHERE postal_code= '{sourcePostalCode}' GROUP BY distance1 LIMIT 1),
    st_makepoint(longitude,latitude)) as DistanceInMeter
    FROM location.public.neighborhoods WHERE {string.Join(" OR ", destinationPostalCodes.Select(x => "postal_code='" + x + "'"))} GROUP BY postal_code,DistanceInMeter";
            
            var response = await _locationDbContext.ZipCodeLocationPairs.FromSqlRaw(rawSql).AsNoTracking().ToArrayAsync();

            return response;
        }

        public async Task<GenericAddress> GetAddressFromZipAsync(string zipCode)
        {
            var query = from country in _locationDbContext.Countries
                join province in _locationDbContext.Province on country.Id equals province.CountryId
                join district in _locationDbContext.Districts on province.ProvinceId equals district.ProvinceId
                join neighborhood in _locationDbContext.Neighborhoods on district.DistrictId equals neighborhood.DistrictId
                where neighborhood.PostalCode == zipCode
                select new GenericAddress()
                {
                    CountryTag = country.CountryTag,
                    Province = province.ProvinceName,
                    District = district.DistrictName,
                    Neighborhood = neighborhood.NeighborhoodName
                };

            return await query.FirstOrDefaultAsync();
        }
    }
}