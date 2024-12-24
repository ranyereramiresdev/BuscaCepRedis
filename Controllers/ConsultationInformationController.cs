using System.Text.Json;
using apiRedis.Caching;
using ApiRedis.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiRedis.Controllers
{
    [ApiController]
    [Route("Consultation-Information")]
    public class ConsultationInformationController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ICachingService _cachingService;
        public ConsultationInformationController(HttpClient httpClient, ICachingService CachingService)
        {
            _httpClient = httpClient;
            _cachingService = CachingService;
        }

        /// <summary>
        /// Busca cep no cache, se não achar, busca no ViaCep
        /// </summary>
        /// <param name="cep"></param>
        /// <returns>Endereço referente ao cep informado</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpGet, Route("Buscar-Cep")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BuscarCep([FromQuery] string cep)
        {
            var viaCepUrl = $"https://viacep.com.br/ws/{cep}/json/";
            try
            {
                var enderecoCache = await _cachingService.GetAsync(cep);

                if (enderecoCache != null)
                {
                    var address = JsonSerializer.Deserialize<AddressModel>(enderecoCache);
                    return Ok(address);
                }
                else
                {
                    var response = await _httpClient.GetAsync(viaCepUrl);

                    if (!response.IsSuccessStatusCode)
                        return StatusCode(404, "Cep não encontrado.");

                    var endereco = await response.Content.ReadAsStringAsync();
                    await _cachingService.SetAsync(cep, endereco);

                    var address = JsonSerializer.Deserialize<AddressModel>(endereco);
                    return Ok(address);
                }
            }
            catch
            {
                return StatusCode(500, "Erro interno ao processar a requisição.");
            }
        }
    }
}
