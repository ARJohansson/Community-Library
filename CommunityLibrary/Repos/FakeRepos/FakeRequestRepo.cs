using CommunityLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityLibrary.Repos.FakeRepos
{
    public class FakeRequestRepo : IRequestRepo
    {
        private static List<Request> requests = new List<Request>();
        public IEnumerable<Request> Requests => requests;

        public void AddRequest(Request request)
        {
            requests.Add(request);
        }

        public bool CheckForOwnerName(string userName)
        {
            for(int i = 0; i < requests.Count(); i++)
            {
                if(requests[i].Owner == userName)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckForRequesterName(string userName)
        {
            for (int i = 0; i < requests.Count(); i++)
            {
                if (requests[i].Requester == userName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
