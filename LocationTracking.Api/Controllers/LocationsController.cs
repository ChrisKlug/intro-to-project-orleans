using LocationTracking.Api.Models;
using LocationTracking.Grains;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace LocationTracking.Api.Controllers
{
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly IGrainFactory grainFactory;

        public LocationsController(IGrainFactory grainFactory)
        {
            this.grainFactory = grainFactory;
        }

        [HttpPost("api/client/{clientId}/locations")]
        public async Task<IActionResult> AddLocation(string clientId, LocationModel model)
        {
            var grain = grainFactory.GetGrain<ILocationTrackingGrain>(clientId);
            await grain.UpdateLocation(model.ToLocation());
            return Ok();
        }
    }
}
