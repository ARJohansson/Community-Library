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
    }
}
