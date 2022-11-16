using Function.Domain.Models.Settings;
using Function.Domain.Models.OL;
using Function.Domain.Models.Purview;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Function.Domain.Helpers
{
    public class SparkProcessParser: ISparkProcessParser
    {
        private ILogger _log;
        private ILoggerFactory _loggerfactory;
        private string _sparkAppGuid;
        private string _sparkAppQualifiedName;
        private IConfiguration _configuration;
        private IQnParser _qnParser;
        private Event _sparkEvent;
        const string SETTINGS = "OlToPurviewMappings";

        public SparkProcessParser(IConfiguration configuration,string sparkAppQualifiedName, string sparkAppGuid, Event SparkEvent, ILoggerFactory loggerfactory)
        {
            this._configuration = configuration;
            this._sparkAppQualifiedName = sparkAppQualifiedName;
            this._sparkAppGuid = sparkAppGuid;
            
            this._log = loggerfactory.CreateLogger<SparkProcessParser>();
            this._loggerfactory = loggerfactory;
            this._sparkEvent = SparkEvent;
            var parserConfig = new ParserSettings();
            try{
                var map = configuration[SETTINGS];
                parserConfig = JsonConvert.DeserializeObject<ParserSettings>(map) ?? throw new MissingCriticalDataException("critical config not found");
            } 
            catch (Exception ex) {
                _log.LogError(ex,"SparkProcessParser: Error retrieving ParserSettings.  Please make sure these are configured on your function.");
                throw;
            }


            _qnParser = new QnParser(parserConfig, _loggerfactory,
                                    null);
        }
        public SparkProcess GetSparkProcess()
        {
            var sparkProcess = new SparkProcess();

            var inputs = new List<InputOutput>();
            foreach (IInputsOutputs input in _sparkEvent.Inputs)
            {
                inputs.Add(GetInputOutputs(input));
            }

            var outputs = new List<InputOutput>();
            foreach (IInputsOutputs output in _sparkEvent.Outputs)
            {
                outputs.Add(GetInputOutputs(output));
            }
            sparkProcess.Attributes = GetProcAttributes(inputs,outputs,_sparkEvent);

            sparkProcess.RelationshipAttributes.Application.QualifiedName = _sparkAppQualifiedName;
            sparkProcess.RelationshipAttributes.Application.Guid = _sparkAppGuid;  

            return sparkProcess;
        }

        private ProcAttributes GetProcAttributes(List<InputOutput> inputs, List<InputOutput> outputs, Event sparkEvent)
        {
            var pa = new ProcAttributes();
            pa.Name = sparkEvent.Run.Facets.EnvironmentProperties?.EnvironmentProperties.SparkDatabricksNotebookPath + sparkEvent.Outputs[0].Name;
            pa.QualifiedName = $"sparkprocess://{inputs[0].UniqueAttributes.QualifiedName.Trim('/').ToLower()}:{outputs[0].UniqueAttributes.QualifiedName.Trim('/').ToLower()}";
            pa.CurrUser = sparkEvent.Run.Facets.EnvironmentProperties?.EnvironmentProperties?.User ?? "";
            pa.SparkPlanDescription = sparkEvent.Run.Facets.SparkLogicalPlan.ToString(Formatting.None)!;
            pa.Inputs = inputs;
            pa.Outputs = outputs;

            return pa;
        }

        private InputOutput GetInputOutputs(IInputsOutputs inOut)
        {
            var id = _qnParser.GetIdentifiers(inOut.NameSpace,inOut.Name);
            var inputOutputId = new InputOutput();
            inputOutputId.TypeName = id.PurviewType;
            inputOutputId.UniqueAttributes.QualifiedName = id.QualifiedName;

            return inputOutputId;
        }
    }
}