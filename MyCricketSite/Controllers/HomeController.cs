using HtmlAgilityPack;
using MongoDB.Bson;
using MongoDB.Driver;
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

            HttpCookie cookie = HttpContext.Request.Cookies["User"];
            if (cookie != null)
            {
                string issuer = cookie.Value.Split('|')[0];
                string key = cookie.Value.Split('|').Length > 1 ? cookie.Value.Split('|')[1] : "";
                UserService userv = new UserService();
                User user = userv.FindByProvider(key, issuer);
                if (user != null)
                {
                    SessionUtils.LoggedInUser = user;
                }
            }
            HttpCookie tournament_cookie = HttpContext.Request.Cookies["DefaultTournament"];
            if (tournament_cookie != null)
            {
                TournamentService tsv = new TournamentService();
                Tournament tournament = tsv.GetById(tournament_cookie.Value);
                if (tournament != null)
                {
                    SessionUtils.CurrentTournament = tournament;
                }
            }
            return View();
        }


        public ActionResult GetAllTournamentDisplay()
        {
            TournamentService tservice = new TournamentService();

            var serializer = new JavaScriptSerializer();
            List<Tournament> tournaments = tservice.getAllTournaments();

            string tournament = serializer.Serialize(tournaments.Select(t => new { t.EntityId, t.Name, t.StartDate, t.Status }));
            return Json(new { Tournaments = tournament }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SelectTournament(string EntityId, bool saveondevice)
        {
            TournamentService tservice = new TournamentService();
            Tournament t = tservice.GetById(EntityId);
            SessionUtils.CurrentTournament = t;
            if (saveondevice)
            {
                HttpCookie cookie = HttpContext.Request.Cookies["DefaultTournament"];
                if (cookie == null)
                    cookie = new HttpCookie("DefaultTournament");
                cookie.Value = EntityId;
                cookie.Expires = DateTime.Now.AddYears(5);
                HttpContext.Response.Cookies.Add(cookie);
            }
            return Json(new { Result = "SUCCESS", TournamentName = t.Name });
        }


        public ActionResult SaveUserProfileOnDevice()
        {
            return View();
        }

        public ActionResult AddUserProfileCookie()
        {
            //SessionUtils.LoggedInUser
            HttpCookie cookie = HttpContext.Request.Cookies["User"];
            if (cookie == null)
                cookie = new HttpCookie("User");
            cookie.Value = SessionUtils.LoggedInUser.Issuer + "|" + SessionUtils.LoggedInUser.ProviderKey;
            cookie.Expires = DateTime.Now.AddYears(5);
            HttpContext.Response.Cookies.Add(cookie);
            return Json(new
            {
                HtmlValue = "SUCCESS"
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetAllGamesForCurrentTournament()
        {
            if (SessionUtils.CurrentTournament != null)
            {
                GameService gs = new GameService();
                List<Game> games = gs.GetGamesForTournament(SessionUtils.CurrentTournament.EntityId);
                var serializer = new JavaScriptSerializer();
                return Json(new { Games = serializer.Serialize(games) }, JsonRequestBehavior.AllowGet);
            }
            return new EmptyResult();
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
            string mainUrl = "http://chicagotwenty20.com";
            string season = "7";
            CookieContainer cookies = new CookieContainer();
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(mainUrl + "/Default.aspx?season=" + season);
            myWebRequest.CookieContainer = cookies;
            HttpWebResponse response = (HttpWebResponse)myWebRequest.GetResponse();

            WebResponse myWebResponse;

            String URL = mainUrl + "/Teams.aspx";

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
            List<Team> TournamentTeamList = new List<Team>();
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
                TournamentTeamList.Add(team);
                if (groups.ContainsKey(node[0].InnerText.Trim()))
                {
                    groups[node[0].InnerText.ToString()] = groups[node[0].InnerText.Trim()] + "," + team.Id.ToString();
                }
                else
                    groups.Add(node[0].InnerText.Trim(), team.Id.ToString());
            }

            Dictionary<string, Dictionary<string, List<string>>> TournamentGroups = new Dictionary<string, Dictionary<string, List<string>>>();

            List<Game> GameList = new List<Game>();
            foreach (KeyValuePair<string, string> grp in groups)
            {
                Dictionary<string, List<string>> gpteams = new Dictionary<string, List<string>>();
                foreach (string teamid in grp.Value.Split(','))
                {

                    List<string> PlayerList = new List<string>();
                    TeamService ts = new TeamService();
                    Team tm = ts.GetById(teamid);

                    String URL1 = mainUrl + "/Team.aspx?team=" + tm.RefId + "&teamName=" + Url.Encode(tm.TeamName).Replace("+", "%20");

                    myWebRequest1 = (HttpWebRequest)WebRequest.Create(URL1);
                    myWebRequest1.CookieContainer = cookies;
                    myWebResponse = myWebRequest1.GetResponse();//Returns a response from an Internet resource


                    streamResponse = myWebResponse.GetResponseStream();//return the data stream from the internet
                    //and save it in the stream

                    sreader = new StreamReader(streamResponse);//reads the data stream
                    string data1 = sreader.ReadToEnd();//reads it to the end
                    HtmlDocument doc1 = new HtmlDocument();
                    doc1.LoadHtml(data1);
                    string divGamesBody = doc1.DocumentNode.SelectSingleNode("//table[@id='ctl00_main_Teamfixturegrid_ctl00']/tbody").InnerHtml;
                    HtmlDocument GameDoc = new HtmlDocument();
                    GameDoc.LoadHtml(divGamesBody);

                    foreach (HtmlNode trGamenode in GameDoc.DocumentNode.SelectNodes(".//tr"))
                    {
                        HtmlNodeCollection GameTd = trGamenode.SelectNodes(".//td");
                        if (GameTd.Count <= 8) continue;
                        Game gm = new Game();
                        gm.RefId = GameTd[0].InnerText.ToString();
                        gm.GroupName = GameTd[1].InnerText.ToString();
                        DateTime gmDate;
                        DateTime.TryParse(GameTd[2].InnerText.ToString(), out gmDate);
                        gm.GameDate = gmDate;
                        Dictionary<string, string> dictTeam = new Dictionary<string, string>();
                        string teams = GameTd[3].InnerText.ToString();
                        dictTeam.Add("Home", teams.Split(new string[] { " v " }, StringSplitOptions.None)[0]);
                        dictTeam.Add("Away", teams.Split(new string[] { " v " }, StringSplitOptions.None)[1]);
                        gm.PlayingTeams = dictTeam;
                        Dictionary<string, string> dictUmpiringTeam = new Dictionary<string, string>();
                        dictUmpiringTeam.Add("Umpire1", GameTd[6].InnerText.ToString());
                        dictUmpiringTeam.Add("Umpire2", GameTd[7].InnerText.ToString());
                        gm.UmpiringTeams = dictUmpiringTeam;
                        GameList.Add(gm);
                    }

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
                        myWebRequest1 = (HttpWebRequest)WebRequest.Create(mainUrl + "/" + pl_hrf);
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

            GameService gameServ = new GameService();
            foreach (var game in (GameList.GroupBy(z => z.RefId).Select(group => group.First()).ToList<Game>()))
            {
                Game dbGame = new Game();
                dbGame.TournamentID = tournament.Id.ToString();
                dbGame.RefId = game.RefId;
                dbGame.GameDate = game.GameDate;
                dbGame.GroupName = game.GroupName;
                Dictionary<string, string> playingTeams = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> tm in game.PlayingTeams)
                {
                    var team = TournamentTeamList.Where(t => t.TeamName == tm.Value).FirstOrDefault();
                    playingTeams.Add(tm.Key, team.Id.ToString());
                }
                dbGame.PlayingTeams = playingTeams;
                Dictionary<string, string> umpiringTeams = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> tm in game.UmpiringTeams)
                {
                    var team = TournamentTeamList.Where(t => t.TeamName == tm.Value).FirstOrDefault();
                    if (team != null)
                        umpiringTeams.Add(tm.Key, team.Id.ToString());
                }
                dbGame.UmpiringTeams = umpiringTeams;
                gameServ.Create(dbGame);
            }


            return new EmptyResult();

        }



    }
}