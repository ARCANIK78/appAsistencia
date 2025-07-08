using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appAsistencia.model
{
    class Horario
    {
        [JsonProperty("HoraEntradaAM")]
        public string EntradaAM { get; set; }

        [JsonProperty("HoraSalidaAM")]
        public string SalidaAM { get; set; }

        [JsonProperty("HoraEntradaPM")]
        public string EntradaPM { get; set; }

        [JsonProperty("HoraSalidaPM")]
        public string SalidaPM { get; set; }
    }
}
