using Newtonsoft.Json;
namespace Function.Domain.Models.Purview
{
    public class SparkApplication
    {
        [JsonProperty("typeName")]
        public string TypeName = "spark_application";
        [JsonProperty("guid")]
        public string Guid = "-1";
        [JsonProperty("attributes")]
        public AppAttributes Attributes = new AppAttributes();
    }

        public class AppAttributes
    {
        [JsonProperty("name")]
        public string Name = "";
        [JsonProperty("appType")]
        public string AppType = "";
        [JsonProperty("qualifiedName")]
        public string QualifiedName = "";
    }
}