namespace EnglishVocab.Application.Models
{
    public class ServiceResponse
    {
        public object Data { get; set; }

        public bool Success { get; set; } = true;

        public object[] ValidateInfo { get; set; }

        public string SystemMessage { get; set; }

        public string UserMessage { get; set; }

        public int Code { get; set; }

        public ServiceResponse(object data, bool success = true, int code = 200)
        {
            Data = data;
            Success = success;
            Code = code;
        }

        public ServiceResponse()
        {

        }
    }
}
