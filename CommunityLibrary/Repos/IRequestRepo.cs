using CommunityLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityLibrary.Repos
{
    public interface IRequestRepo
    {
        IEnumerable<Request> Requests { get; }
        void AddRequest(Request request);
        bool CheckForOwnerName(string userName);
        bool CheckForRequesterName(string userName);
    }
}
