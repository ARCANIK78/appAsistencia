using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using appAsistencia.model;
using Newtonsoft.Json;

namespace appAsistencia.api
{
    class ApiHorarioTrabajo
    {
        private readonly HttpClient _httpClient;

        public ApiHorarioTrabajo()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44332/"); // Cambia si tu API usa otro puerto
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<Horario> ObtenerHorarioAsync()
        {
            try
            {
                HttpResponseMessage respuesta = await _httpClient.GetAsync("api/HorarioTrabajo");
                if (!respuesta.IsSuccessStatusCode)
                    return null;
                string json = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Horario>(json);
            }
            catch (Exception ex)
            {
                {
                    Console.WriteLine("Error al obtener el horario: " + ex.Message);
                    return null;
                }
            }
        }
    }
}
