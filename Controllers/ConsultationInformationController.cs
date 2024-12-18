using System.Text.Json;
using apiRedis.Caching;
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

        public class AddressModel
        {
            public string cep { get; set; }
            public string logradouro { get; set; }
            public string complemento { get; set; }
            public string unidade { get; set; }
            public string bairro { get; set; }
            public string localidade { get; set; }
            public string uf { get; set; }
            public string estado { get; set; }
            public string regiao { get; set; }
            public string ibge { get; set; }
            public string gia { get; set; }
            public string ddd { get; set; }
            public string siafi { get; set; }
        }

        [HttpGet, Route("Buscar-Cep")]
        public async Task<IActionResult> GetEndereco([FromQuery] string cep)
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
                        return StatusCode((int)response.StatusCode, "Erro ao consultar o ViaCEP.");

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
