/*Data model ni sya, constructor ni sya para mag initialize sang fields
kag `ToString()` override nga gina gamit for list display*/
namespace BugTrackerApp
{
    public class BugReport
    {
        public string DeveloperName { get; set; }
        public string BugID { get; set; }
        public string Summary { get; set; }

        public BugReport(string developerName, string bugID, string summary)
        {
            DeveloperName = developerName;
            BugID = bugID;
            Summary = summary;
        }

        public override string ToString()
        {
            return $"{BugID} - {DeveloperName}: {Summary}";
        }
    }
}
