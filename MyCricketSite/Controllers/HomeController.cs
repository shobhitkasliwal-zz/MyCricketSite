using HtmlAgilityPack;
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


            return Json(new { CrawlData = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CrawlTeams(string teams)
        {
            object[] obj = (object[])(new JavaScriptSerializer()).DeserializeObject(teams);
            TeamService tserv = new TeamService();
            Dictionary<string, string> groups = new Dictionary<string, string>();
            for (int i = 0; i < obj.Length; i++)
            {
                object[] tm = (object[])obj[i];
                Team team = new Team();

                if (tm.Length >= 5)
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(tm[1].ToString());
                    HtmlNode link = doc.DocumentNode.SelectNodes("//a[@href]")[0];

                    //   HtmlAttribute att = link["href"];
                    // att.Value = FixLink(att);
                    team.TeamName = link.InnerText;
                    string hrf = link.Attributes["href"].Value;
                    string qr = hrf.Split('?').Length > 0 ? hrf.Split('?')[1] : "";
                    qr = qr.Substring(qr.IndexOf("=") + 1, qr.IndexOf("&", qr.IndexOf("=")) - (qr.IndexOf("=") + 1));
                    team.RefId = qr;


                    team.ContactName = tm[2].ToString();
                    team.Email = tm[3].ToString();
                    team.Phone = tm[4].ToString();
                    team.HomeGround = tm[5].ToString();
                    tserv.Create(team);
                    //tserv.
                    if (groups.ContainsKey(tm[0].ToString()))
                    {
                        groups[tm[0].ToString()] = groups[tm[0].ToString()] + "," + team.Id.ToString();
                    }
                    else
                        groups.Add(tm[0].ToString(), team.Id.ToString());

                }
            }
            Dictionary<string, Dictionary<string, List<string>>> TournamentGroups = new Dictionary<string, Dictionary<string, List<string>>>();
            CookieContainer cookies = new CookieContainer();
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create("http://chicagotwenty20.com/Default.aspx?season=7");
            myWebRequest.CookieContainer = cookies;
            HttpWebResponse response = (HttpWebResponse)myWebRequest.GetResponse();
            WebResponse myWebResponse;
            foreach (KeyValuePair<string, string> grp in groups)
            {
                Dictionary<string, List<string>> gpteams = new Dictionary<string, List<string>>();
                foreach (string teamid in grp.Value.Split(','))
                {
                    gpteams.Add(teamid, new List<string>() { "Player1" });
                    TeamService ts = new TeamService();
                    Team tm = ts.GetById(teamid);

                    String URL = "http://chicagotwenty20.com/Team.aspx?team=" + tm.RefId + "&teamName=" + Url.Encode(tm.TeamName).Replace("+", "%20");

                    HttpWebRequest myWebRequest1 = (HttpWebRequest)WebRequest.Create(URL);
                    myWebRequest1.CookieContainer = cookies;
                    myWebResponse = myWebRequest1.GetResponse();//Returns a response from an Internet resource


                    Stream streamResponse = myWebResponse.GetResponseStream();//return the data stream from the internet
                    //and save it in the stream

                    StreamReader sreader = new StreamReader(streamResponse);//reads the data stream
                    string data = sreader.ReadToEnd();//reads it to the end
                    HtmlDocument doc1 = new HtmlDocument();
                    doc1.LoadHtml(data);
                    string div = doc1.DocumentNode.SelectSingleNode("//div[@id='ctl00_main_PlayersGrid']").InnerHtml;
                    doc1.LoadHtml(div);
                    string trs = doc1.DocumentNode.SelectSingleNode("//tbody").InnerText;
                    /// Make call to get all players insert into the Players Document and then add the player Ids here; 
                }

                TournamentGroups.Add(grp.Key, gpteams);

            }
            return Json(new { });
        }
    }
}