namespace EnglishVocab.Shared.Wrappers;
public class ServiceResponse<T>
{
    public ServiceResponse()
    {
    }
    public ServiceResponse(T data, string message = null)
    {
        Succeeded = true;
        UserMessage = message;
        Data = data;
    }
    public ServiceResponse(string message)
    {
        Succeeded = false;
        UserMessage = message;
    }
    public bool Succeeded { get; set; }
    public string UserMessage { get; set; }

    public string SystemMessage { get; set; }
    public List<string> Errors { get; set; }
    public T Data { get; set; }
}