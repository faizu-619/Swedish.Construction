using Construction.API.Model;
using EventBus.Messages.Events;
using MassTransit;
using MassTransit.Transports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using System.Runtime.Caching;

namespace Construction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConstructionServiceController : ControllerBase
    {
        private readonly static ObjectCache _constructionCache = MemoryCache.Default;
        private readonly IPublishEndpoint _publishEndpoint;

        public ConstructionServiceController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        [HttpPost]
        public async Task<IActionResult> SubmitRequest([FromBody] ServiceRequest request)
        {
            string newId = GenerateId();
            _constructionCache.Add(newId, request, GetCachePolicy());
            return Ok(await Task.FromResult(newId));
        }

        [HttpGet]
        public async Task<IActionResult> GetQueue()
        {
            var queue = _constructionCache.ToImmutableSortedSet();
            return Ok(await Task.FromResult(queue));
        }

        [HttpPost("{id}/arrange-service")]
        public async Task<IActionResult> ArrangeService(string id)
        {
            var eventMessage = _constructionCache.Get(id);
            await _publishEndpoint.Publish<ContractorEvent>(eventMessage);
            return Ok();
        }

        private CacheItemPolicy GetCachePolicy()
        {
            return new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromDays(1) };
        }
        private string GenerateId()
        {
            var newId = Guid.NewGuid().ToString();
            if (_constructionCache.Contains(newId))
            {
                newId = Guid.NewGuid().ToString();
            }

            return newId;
        }
    }
}
