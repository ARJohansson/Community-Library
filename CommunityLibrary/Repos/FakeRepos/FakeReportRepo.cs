using CommunityLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityLibrary.Repos.FakeRepos
{
    public class FakeReportRepo : IReportRepo
    {
        private static List<Report> reports = new List<Report>();
        public IEnumerable<Report> Reports => reports;

        public void AddReport(Report report)
        {
            reports.Add(report);
        }
    }
}
