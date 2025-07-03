using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace appAsistencia.api
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44332/"); 
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> RegistrarAsistencia(string ci, string tipo)
        {
            var asistencia = new
            {
                CI = ci,
                Falta = false,  
                Tipo = tipo,
            };

            var json = JsonConvert.SerializeObject(asistencia);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var respuesta = await _httpClient.PostAsync("api/Asistencia", contenido);
                return respuesta.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Puedes registrar el error en un log o mostrar un mensaje
                Console.WriteLine("Error al conectar con la API: " + ex.Message);
                return false;
            }
        }
    }
}
