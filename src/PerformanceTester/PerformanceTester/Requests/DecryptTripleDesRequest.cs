namespace PerformanceTester.Requests
{
    public class DecryptTripleDesRequest
    {
        public string EncryptedText { get; set; }
        public string Key { get; set; }
    }
}