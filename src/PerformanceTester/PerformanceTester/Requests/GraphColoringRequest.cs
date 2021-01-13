namespace PerformanceTester.Requests
{
    public class GraphColoringRequest
    {
        public int[,] Graph { get; set; }
        public int NumberOfColors{ get; set; }
    }
}