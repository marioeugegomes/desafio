
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Poc.Cliente.Mensageria.Entities
{
    public class MensagemTopico
    {
        [Required]
        [JsonProperty(PropertyName = "document")]
        public integer document { get; set; }

        [Required]
        [JsonProperty(PropertyName = "type_transaction")]
        public string type { get; set; }

        [Required]
        [JsonProperty(PropertyName = "type_payment")]
        public string payment { get; set; }

        [Required]
        [JsonProperty(PropertyName = "value")]
        public float value { get; set; }

        [Required]
        [JsonProperty(PropertyName = "date")]
        public datetime date { get; set; }

    }
}
