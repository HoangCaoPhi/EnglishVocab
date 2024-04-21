using Newtonsoft.Json;

namespace EnglishVocab.Application.Models
{
    public class ConvertConfig
    {
        public static readonly DateFormatHandling JSONDateFormatHandling = DateFormatHandling.IsoDateFormat;
        public static readonly DateTimeZoneHandling JSONDateTimeZoneHandling = DateTimeZoneHandling.Local;
        public static readonly string JSONDateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffFFFFK";
        public static readonly NullValueHandling JSONNullValueHandling = NullValueHandling.Ignore;
        public static readonly ReferenceLoopHandling JSONReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    }
}
