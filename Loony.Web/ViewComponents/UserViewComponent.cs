using Loony.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Loony.Web.ViewComponents
{
    public class FullNameViewComponent : ViewComponent
    {
        private readonly DataContext db;

        public FullNameViewComponent(DataContext dbContext)
        {
            db = dbContext;
        }

        public string Invoke(Guid guid)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == guid);
            return user.FullName;
        }
    }


    public class EmailViewComponent : ViewComponent
    {
        private readonly DataContext db;

        public EmailViewComponent(DataContext dbContext)
        {
            db = dbContext;
        }

        public string Invoke(Guid guid)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == guid);
            return user.Email;
        }
    }
}
