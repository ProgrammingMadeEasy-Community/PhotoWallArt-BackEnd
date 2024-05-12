using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PhotoWallArt.Host.BackGroundServices;

public class LogCleanupService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<LogCleanupService> _logger;

    public LogCleanupService(IConfiguration configuration, ILogger<LogCleanupService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public void DeleteOldLogs()
    {
        try
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                var command = new SqlCommand("DELETE FROM Logs WHERE TimeStamp < DATEADD(day, -30, GETDATE())", connection);
                connection.Open();
                command.ExecuteNonQuery();
                int affectedRows = command.ExecuteNonQuery();
                _logger.LogInformation($"{affectedRows} log entries deleted.");
            }

            _logger.LogInformation("Logs cleaned up successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Cleaning Logs.");
        }
    }
}