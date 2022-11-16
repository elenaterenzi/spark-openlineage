using Function.Domain.Models.Purview;
using Function.Domain.Models.OL;
using Microsoft.Extensions.Logging;

namespace Function.Domain.Helpers
{
    public class SparkAppParser: ISparkAppParser
    {
        private const string APP_NOTEBOOK = "notebook";
        private const string APP_SHELL = "shell";
        private const string APP_JAR = "jar";
        private const string APP_EGG = "egg";

        private ILogger _log;
        public SparkAppParser(ILogger log)
        {
            this._log = log;
        }

        public SparkApplication GetSparkApplication(Event sparkEvent)
        {
            var sparkApp = new SparkApplication();
            string? notebookPath = sparkEvent.Run.Facets.EnvironmentProperties?.EnvironmentProperties?.SparkDatabricksNotebookPath?.Trim('/').ToLower();
            if (notebookPath != null)
            {
                // This is a notebook based application
                sparkApp.Attributes.Name = notebookPath.Substring(notebookPath.LastIndexOf('/')+1);
                sparkApp.Attributes.AppType = APP_NOTEBOOK;
                sparkApp.Attributes.QualifiedName = $"{APP_NOTEBOOK}://{notebookPath}";
            }
            else
            {
                //get information for the Spark Application from the Run
                string[] notebookPatharray = sparkEvent.Job.Name.Split(".",2)[0].Split("_");
                notebookPath = sparkEvent.Job.Namespace + "/" + string.Join("_",notebookPatharray[0..(notebookPatharray.Length-2)]);
                sparkApp.Attributes.Name = sparkEvent.Job.Name;
                sparkApp.Attributes.AppType = APP_NOTEBOOK;
                sparkApp.Attributes.QualifiedName = $"{APP_NOTEBOOK}://{notebookPath}";
            }
            sparkApp.Guid = sparkEvent.Run.RunId;
            return sparkApp;
        }
    }
}