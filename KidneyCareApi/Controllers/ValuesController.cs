using KidneyCareApi.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KidneyCareApi.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            Db dd = new Db();
            var ss1 = new Dal.User();
            ss1.UserName = "11";
            dd.Users.Add(ss1);
            dd.SaveChanges();
            //dd.Patients.Where(a => a.Id == 1).Select(a => new { aa = a.PatientsDatas. });
            //var ss = dd.table2.Select(a => new { a = a.dd, b = a.yy }).First();
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
