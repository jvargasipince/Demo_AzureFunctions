using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace AF_TimeTrigger
{
    public static class Function1
    {
        //At second :00, every 5 minutes starting at minute :00, of every hour
        [FunctionName("DeleteCheckOutNotPaid")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            var str = ConfigurationManager.ConnectionStrings["sqldb_connection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                var text = "UPDATE Pre_CheckOut " +
                        "SET [Status] = 0  " +
                        "where Fecha_Pedido < GetDate() - 5;";

                using (SqlCommand cmd = new SqlCommand(text, conn))
                {
                    // Execute the command and log the # rows affected.
                    var rows = await cmd.ExecuteNonQueryAsync();
                    log.Info($"{rows} rows were updated");
                }
            }
        }


    }
}
