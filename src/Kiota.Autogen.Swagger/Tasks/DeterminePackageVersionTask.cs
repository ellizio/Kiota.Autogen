using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kiota.Autogen.Swagger.Tasks
{
    public class DeterminePackageVersionTask : Task
    {
        [Required] public string ProjectPath { get; set; } = default!;
        [Required] public string PackageId { get; set; } = default!;
        [Required] public string FrameworkVersion { get; set; } = default!;
        
        [Output] public string? Version { get; set; }
        [Output] public bool? Transitive { get; set; }
        
        public override bool Execute()
        {
            var success = CommandExecutor.Execute($"dotnet list {ProjectPath} package --include-transitive --format json", out var output);
            if (!success)
            {
                Log.LogError(output);
                return true;
            }
            
            var result = JsonSerializer.Deserialize<DotnetListResult>(output, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

            var projectFileName = Path.GetFileName(ProjectPath);
            var project = result.Projects.FirstOrDefault(p => Path.GetFileName(p.Path) == projectFileName);
            if (project is null)
            {
                Log.LogWarning($"Project {projectFileName} not found");
                return true;
            }

            var framework = project.Frameworks.FirstOrDefault(f => f.Version == FrameworkVersion);
            if (framework is null)
            {
                Log.LogWarning($"Framework {FrameworkVersion} not found");
                return true;
            }

            var package = framework.TopLevelPackages?.FirstOrDefault(p => p.Id == PackageId);
            if (package is not null)
            {
                Version = package.ResolvedVersion;
                Transitive = false;
                return true;
            }

            package = framework.TransitivePackages?.FirstOrDefault(p => p.Id == PackageId);
            if (package is not null)
            {
                Version = package.ResolvedVersion;
                Transitive = true;
                return true;
            }

            Log.LogWarning($"Package {PackageId} not found");
            return true;
        }
        
        private class DotnetListResult
        {
            public IEnumerable<Project> Projects { get; set; } = default!;
        }
        
        private class Project
        {
            public string Path { get; set; } = default!;
            public IEnumerable<Framework> Frameworks { get; set; } = default!;
        }
        
        private class Framework
        {
            [JsonPropertyName("framework")]
            public string Version { get; set; } = default!;

            public IEnumerable<Package>? TopLevelPackages { get; set; }

            public IEnumerable<Package>? TransitivePackages { get; set; }
        }
        
        private class Package
        {
            public string Id { get; set; } = default!;
            public string ResolvedVersion { get; set; } = default!;
        }
    }
}