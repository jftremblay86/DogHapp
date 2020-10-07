using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DogFetchApp.Models
{
    public class DogModel
    {

        [JsonProperty("message")] public string DogPicture { get; set; }

    }


}
