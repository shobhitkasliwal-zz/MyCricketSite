using HtmlAgilityPack;
using MongoDB.Bson;
using MongoDB.Driver;
using MyCricketSite.App_Start;
using MyCricketSite.Properties;
using MyCricketSiteData.Entities;
using MyCricketSiteData.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyCricketSite.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return null;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //public ActionResult GetTeamsForTournament(string tournamentID)
        //{
        //    TeamService teamService = new TeamService();
        //    //List<Team> teams = teamService.getTeamsForTournament(tournamentID).ToList<Team>();
        //   // string json = new JavaScriptSerializer().Serialize(teams);
        //    return Json(new { Teams = json }, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult Crawl()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CrawlTeams()
        {
            CookieContainer cookies = new CookieContainer();
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create("http://chicagotwenty20.com/Default.aspx?season=7");
            myWebRequest.CookieContainer = cookies;
            HttpWebResponse response = (HttpWebResponse)myWebRequest.GetResponse();

            WebResponse myWebResponse;

            String URL = "http://chicagotwenty20.com/Teams.aspx";

            HttpWebRequest myWebRequest1 = (HttpWebRequest)WebRequest.Create(URL);
            myWebRequest1.CookieContainer = cookies;
            myWebResponse = myWebRequest1.GetResponse();//Returns a response from an Internet resource


            Stream streamResponse = myWebResponse.GetResponseStream();//return the data stream from the internet
            //and save it in the stream

            StreamReader sreader = new StreamReader(streamResponse);//reads the data stream
            string data = sreader.ReadToEnd();//reads it to the end
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(data);
            TeamService tserv = new TeamService();
            HtmlNode mainNode = doc.DocumentNode.SelectSingleNode("//table[@id='ctl00_main_rdgTeams_ctl00']/tbody");

            // HtmlNode node = mainNode.SelectSingleNode("//tr");
            Dictionary<string, string> groups = new Dictionary<string, string>();
            foreach (HtmlNode trnode in mainNode.SelectNodes(".//tr"))
            {
                HtmlNodeCollection node = trnode.SelectNodes(".//td");
                if (node.Count < 5) continue;
                HtmlDocument innerDoc = new HtmlDocument();
                innerDoc.LoadHtml(node[1].InnerHtml);
                HtmlNode link = innerDoc.DocumentNode.SelectNodes("//a[@href]")[0];
                Team team = new Team();
                team.TeamName = link.InnerText;
                string hrf = link.Attributes["href"].Value;
                string qr = hrf.Split('?').Length > 0 ? hrf.Split('?')[1] : "";
                qr = qr.Substring(qr.IndexOf("=") + 1, qr.IndexOf("&", qr.IndexOf("=")) - (qr.IndexOf("=") + 1));
                team.RefId = qr;
                team.ContactName = node[2].InnerHtml.ToString();
                team.Email = node[3].InnerHtml.ToString();
                team.Phone = node[4].InnerHtml.ToString();
                team.HomeGround = node[5].InnerHtml.ToString();
                tserv.Create(team);
                if (groups.ContainsKey(node[0].InnerText.Trim()))
                {
                    groups[node[0].ToString()] = groups[node[0].InnerText.Trim()] + "," + team.Id.ToString();
                }
                else
                    groups.Add(node[0].InnerText.Trim(), team.Id.ToString());
            }

            Dictionary<string, Dictionary<string, List<string>>> TournamentGroups = new Dictionary<string, Dictionary<string, List<string>>>();


            foreach (KeyValuePair<string, string> grp in groups)
            {
                Dictionary<string, List<string>> gpteams = new Dictionary<string, List<string>>();
                foreach (string teamid in grp.Value.Split(','))
                {

                    List<string> PlayerList = new List<string>();
                    TeamService ts = new TeamService();
                    Team tm = ts.GetById(teamid);

                    String URL1 = "http://chicagotwenty20.com/Team.aspx?team=" + tm.RefId + "&teamName=" + Url.Encode(tm.TeamName).Replace("+", "%20");

                    myWebRequest1 = (HttpWebRequest)WebRequest.Create(URL1);
                    myWebRequest1.CookieContainer = cookies;
                    myWebResponse = myWebRequest1.GetResponse();//Returns a response from an Internet resource


                    streamResponse = myWebResponse.GetResponseStream();//return the data stream from the internet
                    //and save it in the stream

                    sreader = new StreamReader(streamResponse);//reads the data stream
                    string data1 = sreader.ReadToEnd();//reads it to the end
                    HtmlDocument doc1 = new HtmlDocument();
                    doc1.LoadHtml(data1);
                    string div = doc1.DocumentNode.SelectSingleNode("//div[@id='ctl00_main_PlayersGrid']").InnerHtml;
                    doc1.LoadHtml(div);
                    string trs_Player = doc1.DocumentNode.SelectSingleNode("//tbody").InnerHtml;
                    /// Make call to get all players insert into the Players Document and then add the player Ids here; 
                    HtmlDocument PlayerDoc = new HtmlDocument();
                    PlayerDoc.LoadHtml(trs_Player);
                    PlayerService playerService = new PlayerService();
                    foreach (HtmlNode trnode in PlayerDoc.DocumentNode.SelectNodes(".//tr"))
                    {
                        HtmlNodeCollection node = trnode.SelectNodes(".//td");
                        Player pl = new Player();
                        pl.RefId = node[0].InnerText;
                        HtmlDocument playerAnc = new HtmlDocument();
                        playerAnc.LoadHtml(node[2].InnerHtml);
                        HtmlNode player_link = playerAnc.DocumentNode.SelectNodes("//a[@href]")[0];
                        string pl_hrf = player_link.Attributes["href"].Value;
                        myWebRequest1 = (HttpWebRequest)WebRequest.Create("http://chicagotwenty20.com/" + pl_hrf);
                        myWebRequest1.CookieContainer = cookies;
                        myWebResponse = myWebRequest1.GetResponse();
                        streamResponse = myWebResponse.GetResponseStream();//return the data stream from the internet
                        //and save it in the stream

                        sreader = new StreamReader(streamResponse);//reads the data stream
                        string player_data = sreader.ReadToEnd();//reads it to the end
                        HtmlDocument pl_detail = new HtmlDocument();
                        pl_detail.LoadHtml(player_data);
                        pl_detail.LoadHtml(pl_detail.DocumentNode.SelectSingleNode("//table[@id='Table1']").InnerHtml);
                        pl_detail.LoadHtml(pl_detail.DocumentNode.SelectSingleNode("//div[@class='details']/table").InnerHtml);
                        pl_detail.LoadHtml(pl_detail.DocumentNode.SelectSingleNode("//div[@class='data-container']/ul").InnerHtml);
                        HtmlNodeCollection PlayerLInode = pl_detail.DocumentNode.SelectNodes(".//li");
                        string name = PlayerLInode[0].InnerText.Replace("Name:", "").Trim();
                        if (name.Contains("(C)")) pl.Role = "Captain";
                        if (name.Contains("(VC)")) pl.Role = "ViceCaptain";
                        name = name.Replace("(C)", "").Replace("(VC)", "").Trim();
                        string[] splitName = name.Split(' ');
                        pl.FirstName = splitName[0];
                        pl.LastName = splitName.Length > 1 ? splitName[1] : "";
                        pl.Email = PlayerLInode[1].InnerText.Replace("EmailAddress:", "").Trim();
                        pl.Phone = PlayerLInode[2].InnerText.Replace("Phone:", "").Trim();
                        pl.BattingStyle = PlayerLInode[3].InnerText.Replace("Batting Style:", "").Trim();
                        pl.BowlingStyle = PlayerLInode[4].InnerText.Replace("Bowling Style:", "").Trim();
                        pl.FieldingPosition = PlayerLInode[5].InnerText.Replace("Fielding Position:", "").Trim();
                        pl.DateAdded = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        playerService.Create(pl);
                        PlayerList.Add(pl.Id.ToString());
                    }
                    gpteams.Add(teamid, PlayerList);

                }

                TournamentGroups.Add(grp.Key, gpteams);

            }
            TournamentService tournamentService = new TournamentService();
            Tournament tournament = new Tournament();
            tournament.StartDate = new DateTime(2014, 05, 17);
            tournament.Status = "Closed";
            tournament.Name = "2014 Chicago Twenty 20";
            tournament.Groups = TournamentGroups;
            tournamentService.Create(tournament);

            return new EmptyResult();

        }



    }
}