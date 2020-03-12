using CommunityLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityLibrary.Repos
{
    public interface IReportRepo
    {
        IEnumerable<Report> Reports { get; }
        void AddReport(Report report);
    }
}
