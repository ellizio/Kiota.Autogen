using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Kiota.Autogen.Swagger.Tasks
{
    public class ParseSettingsTask : Task
    {
        private const string SettingsJsonFile = "settings.json";

        [Output] public ITaskItem[] Settings { get; set; } = default!;
        
        public override bool Execute()
        {
            IReadOnlyCollection<Setting> settings;

            if (File.Exists(SettingsJsonFile))
                settings = DeserializeJson();
            else
                return false;

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

        private static IReadOnlyCollection<Setting> DeserializeJson()
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