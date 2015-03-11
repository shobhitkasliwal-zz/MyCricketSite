using MyCricketSiteData.Entities;
using MyCricketSiteData.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyCricketSite.Controllers
{
    public class SuperAdminController : Controller
    {

        [HttpGet]
        public ActionResult Tournament()
        {

            Tournament tournament = new Tournament();
            return View(tournament);
        }

        [HttpPost]
        public ActionResult Tournament(Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                var tournamentService = new TournamentService();

                tournamentService.Create(tournament);
                return Json(new { output = "Tournament Created Successfully" });
            }
            return View();
        }


        [HttpGet]
        public ActionResult Team()
        {

            Team team = new Team();
            TournamentService tournamentService = new TournamentService();
            // List<Tournament> tournaments = tournamentService.getActiveTornaments().ToList<Tournament>();
            List<SelectListItem> obj = new List<SelectListItem>();


            //ViewBag.Tournaments = new SelectList(tournaments, "Id", "Name"); ;
            return View(team);
        }

        [HttpPost]
        public ActionResult Team(Team team)
        {
            if (ModelState.IsValid)
            {
                var tournamentService = new TeamService();

                tournamentService.Create(team);
                return Json(new { output = "Team Created Successfully" });
            }
            return View();
        }

        [HttpGet]
        public ActionResult Game()
        {

            Game game = new Game();
            TournamentService tournamentService = new TournamentService();
           // List<Tournament> tournaments = tournamentService.getActiveTornaments().ToList<Tournament>();
            List<SelectListItem> obj = new List<SelectListItem>();
         //   ViewBag.Tournaments = new SelectList(tournaments, "Id", "Name"); ;



            return View(game);
        }

        [HttpPost]
        public ActionResult Game(Game game)
        {
            if (ModelState.IsValid)
            {
                var gameService = new GameService();

                gameService.Create(game);
                return Json(new { output = "Game Created Successfully" });
            }
            return View();
        }

    }
}