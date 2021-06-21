using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
//using System.Data.SqlClient;

namespace AppController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {

        SQLiteConnection sqlite = new SQLiteConnection("Data Source=appDatabase.db");

        //api/person/byName?firstName=a&lastName=b
        //data/getData?topSum=a&jungleSum=b&middleSum=c&adcSum=d&supportSum=e
        [HttpGet("getData")]
        public IEnumerable<Data> Get(string topSum, string jungleSum, string middleSum, string adcSum, string supportSum)
        {
            LeagueData ld = new LeagueData();
            return ld.getLeagueData("https://na.op.gg/multi/query=" + topSum + "%2C" + jungleSum + "%2C" + middleSum + "%2C" + adcSum + "%2C" + supportSum);
        }

        [HttpGet("getTierData")]
        public string GetTierData()
        {
            sqlite.Open();
            SQLiteCommand cmd = sqlite.CreateCommand();
            cmd.CommandText = "select TierData from app_data where Id = 1";
            System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader();
            string totalJson = "";
            while (reader.Read())
            {
                totalJson += reader[0];
            }
            return totalJson;
        }

        [HttpGet("getCounterData")]
        public string GetCounterData()
        {
            sqlite.Open();
            SQLiteCommand cmd = sqlite.CreateCommand();
            cmd.CommandText = "select CounterData from app_data where Id = 1";
            System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader();
            string totalJson = "";
            while (reader.Read())
            {
                totalJson += reader[0];
            }
            return totalJson;
        }
    }
}
