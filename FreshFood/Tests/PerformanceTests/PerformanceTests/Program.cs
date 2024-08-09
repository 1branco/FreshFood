namespace PerformanceTests;

class Program
{
    static void Main(string[] args)
    {   
        var summary = BenchmarkRunner.Run<Services.LoginPerformanceTest>();
    }
}
