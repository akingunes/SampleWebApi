namespace SampleWebApi.Results
{
    public class SmsApiResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }   
        public object Result { get; set; }
    }
}
