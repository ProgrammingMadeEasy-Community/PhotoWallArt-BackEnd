public class LoggerSettings
{
    public string AppName { get; set; }
    public string ElasticSearchUrl { get; set; }
    public bool WriteToFile { get; set; }
    public bool StructuredConsoleLogging { get; set; }

    public WriteToSettings WriteTo { get; set; }
}

public class WriteToSettings
{
    public string ConnectionString { get; set; }
    public SinkOptionsSection SinkOptionsSection { get; set; }
}

public class SinkOptionsSection
{
    public string TableName { get; set; }
    public string SchemaName { get; set; }
    public bool AutoCreateSqlTable { get; set; }
    public string RestrictedToMinimumLevel { get; set; }
}
