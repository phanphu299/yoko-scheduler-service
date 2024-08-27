using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace Function.Timer
{
    public class QuartzJobCleaner
    {
        // private readonly IConfiguration _configuration;

        // public QuartzJobCleaner(IConfiguration configuration)
        // {
        //     _configuration = configuration;
        // }

        // TODO: To implement the cleaner later
        // [FunctionName("QuartzJobCleaner")]
        // public async Task RunAsync([TimerTrigger("0 */30 * * * *")] TimerInfo timer, ILogger log)
        // {
        // }
    }
}