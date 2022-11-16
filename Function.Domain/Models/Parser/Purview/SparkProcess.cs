using Newtonsoft.Json;
using System.Collections.Generic;
namespace Function.Domain.Models.Purview
{
    public class SparkProcess
    {
        [JsonProperty("typeName")]
        public string TypeName = "spark_process";
        [JsonProperty("guid")]
        public string Guid = "-2";
        [JsonProperty("attributes")]
        public ProcAttributes Attributes = new ProcAttributes();
        [JsonProperty("relationshipAttributes")]
        public RelationshipAttributes RelationshipAttributes = new RelationshipAttributes();
    }
    public class ProcAttributes
    {
         [JsonProperty("name")]
        public string Name = "";
        [JsonProperty("qualifiedName")]
        public string QualifiedName = ""; 
        [JsonProperty("columnMapping")]
        public string ColumnMapping = ""; 
         [JsonProperty("executionId")]
        public string ExecutionId = "0"; 
         [JsonProperty("currUser")]
        public string CurrUser = ""; 
         [JsonProperty("sparkPlanDescription")]
        public string SparkPlanDescription = ""; 
         [JsonProperty("inputs")]
        public List<InputOutput>? Inputs = new List<InputOutput>();
         [JsonProperty("outputs")]
        public List<InputOutput>? Outputs = new List<InputOutput>();
    }
    public class RelationshipAttributes
    {
        [JsonProperty("application")]
        public Application Application = new Application();
    }

    public class Application
    {
        [JsonProperty("qualifiedName")]
        public string QualifiedName = ""; 
        [JsonProperty("guid")]
        public string Guid = "-1";
    }
}