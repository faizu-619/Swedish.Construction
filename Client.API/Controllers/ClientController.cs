using Client.Repository.Entities;
using Client.Repository.Repositories.Interfaces;
using DnsClient.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Client.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _repository;
        private readonly ILogger<ClientController> _logger;

        public ClientController(IClientRepository repository, ILogger<ClientController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ClientEntity>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ClientEntity>>> Get()
        {
            var products = await _repository.GetClients();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ClientEntity), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ClientEntity>> Get(string id)
        {
            var product = await _repository.GetClient(id);

            if (product == null)
            {
                _logger.LogError($"ClientEntity with id: {id}, not found.");
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ClientEntity), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ClientEntity>> Post([FromBody] ClientEntity product)
        {
            await _repository.CreateClient(product);

            return CreatedAtRoute("Get", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ClientEntity), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Put([FromBody] ClientEntity product)
        {
            return Ok(await _repository.UpdateClient(product));
        }

        [HttpDelete("{id:length(24)}")]        
        [ProducesResponseType(typeof(ClientEntity), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _repository.DeleteClient(id));
        }
    }
}
