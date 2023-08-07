// See https://aka.ms/new-console-template for more information

using TemplatedConfiguration.Schema;

const string configTemplate = @"
{
  ""FileSystem"": {
    ""Path"": ""Configuration.BasePath"",
    ""Directories"": [
      {
        ""Path"": ""string.Join(\"", \"", Track.Artists.Select(x => x.Name).ToArray())"",
        ""Directories"": [
          {
            ""Path"": ""Track.Album.Title"",
            ""Files"": [
              {
                ""Name"": ""string.Concat(Track.Id, Track.FileExtension)"",
                ""Content"": ""Track.Content""
              }
            ]
          }
        ]
      }
    ]
  }
}
";


FileSystemHandler? handler = FileSystemHandler.FromJson(configTemplate);
if (handler is not null)
{
  await handler.Compile();
  var fs = handler?.FileSystem;
}