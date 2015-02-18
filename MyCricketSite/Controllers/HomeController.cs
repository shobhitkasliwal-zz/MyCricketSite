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
            for (int i = 0; i < obj.Length; i++)
            {
                object[] tm = (object[])obj[i];
                Team team = new Team();
                if (tm.Length >= 5)
                {
                    team.TeamName = tm[1].ToString();
                    team.ContactName = tm[2].ToString();
                    team.Email = tm[3].ToString();
                    team.Phone = tm[4].ToString();
                    team.HomeGround = tm[5].ToString();
                    tserv.Create(team);
                }
            }
            return Json(new { });
        }
    }
}