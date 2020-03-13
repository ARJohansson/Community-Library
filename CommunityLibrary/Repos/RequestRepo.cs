using CommunityLibrary.Data;
using CommunityLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityLibrary.Repos
{
    public class RequestRepo : IRequestRepo
    {
        private ApplicationDbContext context;

        public RequestRepo(ApplicationDbContext c) => context = c;
        public IEnumerable<Request> Requests => context.Requests;

        public void AddRequest(Request request)
        {
            context.Add(request);
            context.SaveChanges();
        }

        public bool CheckForOwnerName(string userName)
        {
            for(int i = 0; i < context.Requests.Count(); i++)
            {
                List<Request> requests = Requests.ToList();
                if(requests[i].Owner == userName)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckForRequesterName(string userName)
        {
            for(int i = 0; i < context.Requests.Count(); i++)
            {
                List<Request> requests = Requests.ToList();
                if(requests[i].Requester == userName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
