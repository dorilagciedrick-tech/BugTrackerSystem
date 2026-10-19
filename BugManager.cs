/*Naga Manage sang storage kag workflow sang bug reports 
sa backlog kag review board. Ang BugManager nagahatag sang mga methods 
para mag-submit sang bug reports, mag-send sa review, mag-return sa backlog, 
kag mag-close sang sprint.*/
using System.Collections.ObjectModel;

namespace BugTrackerApp
{
    public class BugReportCollection : Collection<BugReport>
    {
        public new void Insert(int index, BugReport item)
        {
            base.Insert(index, item);
        }

        public new bool Contains(BugReport item)
        {
            return base.Contains(item);
        }

        public new int IndexOf(BugReport item)
        {
            return base.IndexOf(item);
        }
    }

    public class BugManager
    {
        private readonly BugReportCollection backlog = new();
        private readonly BugReportCollection reviewBoard = new();

        public BugReportCollection Backlog => backlog;
        public BugReportCollection ReviewBoard => reviewBoard;

        public void SubmitBug(BugReport report)
        {
            backlog.Add(report);
        }

        public bool SendToReview(BugReport report)
        {
            if (backlog.Contains(report))
            {
                backlog.Remove(report);
                reviewBoard.Add(report);
                return true;
            }

            return false;
        }

        public bool ReturnToBacklog(BugReport report)
        {
            if (reviewBoard.Contains(report))
            {
                reviewBoard.Remove(report);
                backlog.Add(report);
                return true;
            }

            return false;
        }

        public void SprintClose()
        {
            while (reviewBoard.Count > 0)
            {
                var report = reviewBoard[0];
                reviewBoard.RemoveAt(0);
                backlog.Add(report);
            }
        }
    }
}
