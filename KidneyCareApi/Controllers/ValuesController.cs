using KidneyCareApi.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KidneyCareApi.Common;
using ServiceStack.Redis;

namespace KidneyCareApi.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            //Db dd = new Db();
            //var ss1 = new Dal.User();
            //ss1.UserName = "11";
            //dd.Users.Add(ss1);
            //dd.SaveChanges();
            string ss = "";
            try
            {
                Util.AddLog(new LogInfo(){Describle = "ddd"});
            }
            catch (Exception e)
            {
                ss = e.ToString();
                throw;
            }

            ICacheHelper CacheClient = Util.GetCacheClient();
            CacheClient.Add(CacheKeyGroup.AuthenCode, "key", "asdf", TimeSpan.FromMinutes(100));

            var result = CacheClient.Get<string>(CacheKeyGroup.AuthenCode, "key");
            //dd.Patients.Where(a => a.Id == 1).Select(a => new { aa = a.PatientsDatas. });
            //var ss = dd.table2.Select(a => new { a = a.dd, b = a.yy }).First();


            //var host = "172.17.0.12";//实例访问host地址 
            //int port = 6379;// 端口信息 
            //string instanceId = "crs-77i2ghem";//实例id 
            //string pass = "11111111q";//密码 

            //RedisClient redisClient = new RedisClient(host, port, instanceId + ":" + pass);
            //string key = "name";
            //string value = "QcloudV5!";
            //redisClient.Set(key, value); //设置值 
            //System.Console.WriteLine("set key:[" + key + "]value:[" + value + "]");
            //string getValue = System.Text.Encoding.Default.GetString(redisClient.Get(key)); //读取值 
            //System.Console.WriteLine("value:" + getValue);
            //System.Console.Read();

            return new string[] { "value1", result };
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
