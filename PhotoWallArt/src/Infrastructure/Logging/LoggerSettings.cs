namespace PhotoWallArt.Infrastructure.Logging;

public class LoggerSettings
{
    public string AppName { get; set; } = "PhotaWallArt";
    public string ElasticSearchUrl { get; set; } = string.Empty;
    public bool WriteToFile { get; set; } = false;
    public bool StructuredConsoleLogging { get; set; } = false;
    public string MinimumLogLevel { get; set; } = "Information";

}

public class WriteTo
{
    public string ConnectionString { get; set; }
    public SinkOptionsSection SinkOptionsSection { get; set; }
}

public class SinkOptionsSection
{
    public string TableName { get; set; }
    public bool AutoCreateSqlTable { get; set; }
}

public class ColumnOptionsSection
{
    public List<string> AddStandardColumns { get; set; }
    public List<string> RemoveStandardColumns { get; set; }
    public List<AdditionalColumn> AdditionalColumns { get; set; }
}

public class AdditionalColumn
{
    public string ColumnName { get; set; }
    public string DataType { get; set; }
    public bool AllowNull { get; set; }
    public int? DataLength { get; set; }
}