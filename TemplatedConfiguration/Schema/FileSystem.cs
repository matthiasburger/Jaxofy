using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RulesEngine.Models;

namespace TemplatedConfiguration.Schema;

public partial class FileSystemHandler
{
    public static readonly RuleParameter[] rparams = 
    {
        new("Configuration", new { BasePath = "bla" }),
        new("Track", new
        {
            Artists = new[]
            {
                new { Name = "Jaxon" },
                new { Name = "Jaxon 2" }
            },
            Album = new
            {
                Title = "Hallo Welt"
            },
            Id = 1,
            Content = "lalalala",
            FileExtension = ".mp3"
        })
    };
    
    [JsonProperty("FileSystem")] 
    public Directory FileSystem { get; set; }
}

public class Directory : IRuleCompilable
{
    [JsonProperty("Path")] public string Path { get; set; }

    [JsonProperty("Directories")] public Directory[]? Directories { get; set; }

    [JsonProperty("Files")] public File[]? Files { get; set; }

    public async Task Compile()
    {
        string ruleName = Guid.NewGuid().ToString();
        Rule r = new()
        {
            RuleName = ruleName,
            Expression = "true",
            Actions = new ()
            {
                OnSuccess = new()
                {
                    Name = "OutputExpression",
                    Context = new()
                    {
                        { "Expression", Path.Trim('{','}') }
                    }
                }
            }
        };
        string workflowName = Guid.NewGuid().ToString();
        RulesEngine.RulesEngine re = new();
        re.AddWorkflow(new Workflow
        {
            WorkflowName = workflowName,
            Rules = new[] { r }
        });
        ActionRuleResult? ruleResult = await re.ExecuteActionWorkflowAsync(workflowName, ruleName, FileSystemHandler.rparams);
        Path = ruleResult?.Output?.ToString() ?? "-";

        if (Directories is not null)
        {
            foreach (Directory subDirectory in Directories)
                await subDirectory.Compile();
        }

        if (Files is not null)
        {
            foreach (File f in Files)
                await f.Compile();
        }
    }
}

public class File : IRuleCompilable
{
    [JsonProperty("Name")] public string Name { get; set; }

    [JsonProperty("Content")] public string Content { get; set; }
    
    
    public async Task Compile()
    {
        string ruleName = Guid.NewGuid().ToString();
        string ruleName2 = Guid.NewGuid().ToString();
        Rule r = new()
        {
            RuleName = ruleName,
            Expression = "true",
            Actions = new ()
            {
                OnSuccess = new()
                {
                    Name = "OutputExpression",
                    Context =
                        new()
                        {
                            { "Expression", Content.Trim('{','}') }
                        }
                }
            }
        };
        
        Rule r2 = new()
        {
            RuleName = ruleName2,
            Expression = "true",
            Actions = new ()
            {
                OnSuccess = new()
                {
                    Name = "OutputExpression",
                    Context =
                        new()
                        {
                            { "Expression", Name.Trim('{','}') }
                        }
                }
            }
        };
        string workflowName = Guid.NewGuid().ToString();
        RulesEngine.RulesEngine re = new();
        re.AddWorkflow(new Workflow
        {
            WorkflowName = workflowName,
            Rules = new[] { r, r2 }
        });
        
        List<RuleResultTree>? ruleResult = await re.ExecuteAllRulesAsync(workflowName, FileSystemHandler.rparams);
        
        Content = ruleResult?.FirstOrDefault(x => x.Rule.RuleName == ruleName)?
            .ActionResult.Output?.ToString() ?? "-";
        
        Name = ruleResult?.FirstOrDefault(x => x.Rule.RuleName == ruleName2)?
            .ActionResult.Output?.ToString() ?? "-";
    }
}

public partial class FileSystemHandler
{
    public static FileSystemHandler? FromJson(string json) =>
        JsonConvert.DeserializeObject<FileSystemHandler>(json, Converter.Settings);

    public async Task Compile()
    {
        // do something

        await FileSystem.Compile();
    }
}

public static class Serialize
{
    public static string ToJson(this FileSystemHandler self) => JsonConvert.SerializeObject(self, Converter.Settings);
}

internal static class Converter
{
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters =
        {
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
        },
    };
}

interface IRuleCompilable
{
    Task Compile();
}