

using Newtonsoft.Json;

namespace TCCMarketPlace.Model
{
    public class SlideShowImage
    {
        [JsonProperty("SERVICE_ID")]
        public int ServiceId { get; set; }
        public int ImageId { get; set; }

        [JsonProperty("SLIDER_IMAGE")]
        public string ImageBlobUrl { get; set; }
    }
}
