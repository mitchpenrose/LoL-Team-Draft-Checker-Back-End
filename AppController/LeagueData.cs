using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Web;

namespace AppController
{
    public class LeagueData
    {
        WebClient wc;
        HtmlDocument htmlDoc;
        public LeagueData()
        {
            wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
        }
        public List<Data> getLeagueData(string url)
        {
            List<Data> teamInfo = new List<Data>();
            string html = wc.DownloadString(url);
            htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            //Get the summoner's champion information
            HtmlNodeCollection players = htmlDoc.DocumentNode.SelectNodes("//div[@class='most-champions-wrapper']");
            foreach(HtmlNode player in players)
            {
                Data summonerData = new Data();
                HtmlNodeCollection seasons = player.SelectNodes(".//ul");
                foreach(HtmlNode season in seasons)
                {
                    List<ChampData> seasonChampionInfo = new List<ChampData>();
                    HtmlNodeCollection champions = season.SelectNodes(".//li");
                    if (champions == null)
                        continue;
                    foreach(HtmlNode champion in champions)
                    {
                        ChampData seasonChampData = new ChampData();
                        string championName = HttpUtility.HtmlDecode(champion.SelectSingleNode(".//div[contains(@class, 'champion')]").GetAttributeValue("title", ""));
                        string kda = champion.SelectSingleNode(".//div[@class='kda']").InnerText.Trim();
                        string championGameNum = champion.SelectSingleNode(".//div[@class='game-count']").InnerText.Trim();
                        string winRatio = champion.SelectSingleNode(".//div[@class='win-ratio']").InnerText.Trim();
                        seasonChampData.ChampName = championName;
                        seasonChampData.Kda = kda;
                        seasonChampData.ChampionGameNum = championGameNum;
                        seasonChampData.WinRatio = winRatio;
                        seasonChampionInfo.Add(seasonChampData);
                    }
                    summonerData.SeasonsChampionInfo.Add(seasonChampionInfo);
                }
                teamInfo.Add(summonerData);
            }


            //Get the summoner information
            int index = 0;
            HtmlNodeCollection summoners = htmlDoc.DocumentNode.SelectNodes("//div[@class='summoner-summary']");
            foreach (HtmlNode summoner in summoners)
            {
                string summonerName = "", rank = "", wins = "", losses = "";
                try
                {
                    summonerName = summoner.SelectSingleNode(".//a[@class='name']").InnerText.Trim();
                    rank = summoner.SelectSingleNode(".//div[@class='lp']").InnerText.Trim();
                    wins = summoner.SelectSingleNode(".//div[@class='win']").InnerText.Trim().Replace("W", "");
                    losses = summoner.SelectSingleNode(".//div[@class='base']").InnerText.Trim().Split('\n')[1].Replace("L", "").Trim();
                }
                catch (NullReferenceException) { }
                teamInfo[index].SummonerName = summonerName;
                teamInfo[index].Rank = rank;
                teamInfo[index].Wins = wins;
                teamInfo[index].Losses = losses;
                index++;
            }

            return teamInfo;
        }
    }

    public class Data
    {
        private List<List<ChampData>> seasonsChampionInfo = new List<List<ChampData>>();
        private string summonerName;
        private string rank;
        private string wins;
        private string losses;

        public List<List<ChampData>> SeasonsChampionInfo { get => seasonsChampionInfo; set => seasonsChampionInfo = value; }
        public string SummonerName { get => summonerName; set => summonerName = value; }
        public string Rank { get => rank; set => rank = value; }
        public string Wins { get => wins; set => wins = value; }
        public string Losses { get => losses; set => losses = value; }
    }

    public class ChampData
    {
        private string champName;
        private string kda;
        private string championGameNum;
        private string winRatio;

        public string ChampName { get => champName; set => champName = value; }
        public string Kda { get => kda; set => kda = value; }
        public string ChampionGameNum { get => championGameNum; set => championGameNum = value; }
        public string WinRatio { get => winRatio; set => winRatio = value; }
    }
}
