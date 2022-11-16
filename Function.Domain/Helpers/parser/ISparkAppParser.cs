using Function.Domain.Models.OL;
using Function.Domain.Models.Purview;

namespace Function.Domain.Helpers
{
    public interface ISparkAppParser
    {
        public SparkApplication GetSparkApplication(Event sparkEvent);
    }
}