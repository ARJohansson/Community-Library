using CommunityLibrary.Data;
using CommunityLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityLibrary.Repos
{
    public class ReportRepo : IReportRepo
    {
        private ApplicationDbContext context;

        public ReportRepo(ApplicationDbContext c) => context = c;
        public IEnumerable<Report> Reports => context.Reports;

        public void AddReport(Report report)
        {
            context.Add(report);
            context.SaveChanges();
        }
    }
}
