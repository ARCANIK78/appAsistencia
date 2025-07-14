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

        public async Task<(bool Exito, string Mensaje)> RegistrarAsistencia(string ci, string tipo)
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
                var respuesta = await _httpClient.PostAsync("api/Asistencia/registrar", contenido);
                if (respuesta.IsSuccessStatusCode)
                {

                    Console.WriteLine("Se Registro");
                    return (true, "✅ Asistencia registrada correctamente.");
                }
                else
                {
                    string errorMsg = await respuesta.Content.ReadAsStringAsync();

                    if (errorMsg.Contains("Ya se registró este tipo de asistencia"))
                    {
                        return (false, "❌ Ya se registró este tipo de asistencia para este empleado en esta fecha.");
                    }
                    return (false, "❌ Error al registrar la asistencia. Intente nuevamente.");

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, $"❌ Error al conectar con la API: {ex.Message}");
            }
        }
    }
}