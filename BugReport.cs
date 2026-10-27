/*Data model ni sya, constructor ni sya para mag initialize sang fields
kag `ToString()` override nga gina gamit for list display*/
namespace BugTrackerApp
{
    public class BugReport
    {
        public string ReporterName { get; set; } = string.Empty;
        public string DeveloperName
        {
            get => ReporterName;
            set => ReporterName = value ?? string.Empty;
        }

        public string BugID { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string FeatureArea { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;

        public BugReport(
            string developerName,
            string bugID,
            string summary,
            string? courseName = null,
            string? featureArea = null,
            string? severity = null,
            string? environment = null)
        {
            ReporterName = string.IsNullOrWhiteSpace(developerName) ? "Unknown Reporter" : developerName.Trim();
            BugID = string.IsNullOrWhiteSpace(bugID) ? "Unknown" : bugID.Trim();
            Summary = summary?.Trim() ?? string.Empty;
            CourseName = string.IsNullOrWhiteSpace(courseName) ? "Not specified" : courseName.Trim();
            FeatureArea = string.IsNullOrWhiteSpace(featureArea) ? "General" : featureArea.Trim();
            Severity = string.IsNullOrWhiteSpace(severity) ? "Medium" : severity.Trim();
            Environment = string.IsNullOrWhiteSpace(environment) ? "Web" : environment.Trim();
        }

        public override string ToString()
        {
            return $"[{BugID}] {ReporterName} • {CourseName} • {FeatureArea} • {Severity}: {Summary}";
        }
    }
}
