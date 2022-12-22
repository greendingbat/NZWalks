using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionsRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionsRepository, IMapper mapper)
        {
            this.regionsRepository = regionsRepository;
            this.mapper = mapper;
        }

        [HttpGet]   
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionsRepository.GetAllAsync();

            // return DTO regions
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(domainRegion =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = domainRegion.Id,
            //        Code = domainRegion.Code,
            //        Name = domainRegion.Name,
            //        Area = domainRegion.Area,
            //        Lat = domainRegion.Lat,
            //        Long = domainRegion.Long,
            //        Population = domainRegion.Population,
            //    };
            //    regionsDTO.Add(regionDTO);
            //});

            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionsRepository.GetAsync(id);

            if(region == null)
            {
                return NotFound();
            }

            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            // Request(DTO) to Domain model
            var regionDomain = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population,
            };

            // Pass details to Repository
            var response = await regionsRepository.AddAsync(regionDomain);

            // Convert back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Area = regionDomain.Area,
                Lat = regionDomain.Lat,
                Long = regionDomain.Long,
                Name = regionDomain.Name,
                Population = regionDomain.Population,
            };

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            // Get region from database
            var deletedRegionDomain = await regionsRepository.DeleteAsync(id);

            // If null NotFound
            if (deletedRegionDomain == null)
            {
                return NotFound();
            }

            // Convert response back to DTO
            var deletedRegionDTO = new Models.DTO.Region()
            {
                Id = deletedRegionDomain.Id,
                Code = deletedRegionDomain.Code,
                Area = deletedRegionDomain.Area,
                Lat = deletedRegionDomain.Lat,
                Long = deletedRegionDomain.Long,
                Name = deletedRegionDomain.Name,
                Population = deletedRegionDomain.Population,
            };

            // return Ok response
            return Ok(deletedRegionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            // Convert DTO to domain model
            var regionDomain = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population,
            };


            // update region using repository
            regionDomain = await regionsRepository.UpdateAsync(id, regionDomain);

            // if null then NotFound
            if (regionDomain == null) { return NotFound(); }

            // convert domain back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Area = regionDomain.Area,
                Lat = regionDomain.Lat,
                Long = regionDomain.Long,
                Name = regionDomain.Name,
                Population = regionDomain.Population,
            };

            // return ok response
            return Ok(regionDTO);
        }
    }
}
