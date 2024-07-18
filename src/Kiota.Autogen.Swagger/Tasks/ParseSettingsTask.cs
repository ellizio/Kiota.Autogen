using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Kiota.Autogen.Swagger.Tasks
{
    public class ParseSettingsTask : Task
    {
        private const string SettingsJsonFile = "gensettings.json";

        [Output] public ITaskItem[] Settings { get; set; } = default!;
        
        public override bool Execute()
        {
            var settings = Deserialize();

            var output = new List<ITaskItem>(settings.Count);
            foreach (var setting in settings)
            {
                var item = new TaskItem(setting.Version);
                item.SetMetadata(nameof(setting.Name), setting.Name);
                item.SetMetadata(nameof(setting.Namespace), setting.Namespace);
                item.SetMetadata(nameof(setting.Version), setting.Version);
                
                output.Add(item);
            }

            Settings = output.ToArray();
            return true;
        }

        private static IReadOnlyCollection<Setting> Deserialize()
        {
            using var sr = new FileStream(SettingsJsonFile, FileMode.Open);
            return JsonSerializer.Deserialize<List<Setting>>(sr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        private class Setting
        {
            public string Name { get; set; } = default!;
            public string Namespace { get; set; } = default!;
            public string Version { get; set; } = default!;
        }
    }
}