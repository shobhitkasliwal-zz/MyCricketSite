using MyCricketSiteData.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace MyCricketSite
{
    public static class SessionUtils
    {
        public static Hashtable SiteSession
        {
            get
            {
                if (HttpContext.Current.Session["MyCricketSite_SiteSession"] == null)
                {
                    Hashtable tbl = new Hashtable();
                    HttpContext.Current.Session["MyCricketSite_SiteSession"] = tbl;
                    return tbl;
                }
                else
                {
                    return (Hashtable)HttpContext.Current.Session["MyCricketSite_SiteSession"];
                }
            }
        }

        public static User LoggedInUser
        {
            get { return SiteSession.ContainsKey("LoggedInUser") ? (User)SiteSession["LoggedInUser"] : null; }
            set { SiteSession["LoggedInUser"] = value; }
        }

        public static Tournament CurrentTournament
        {
            get { return SiteSession.ContainsKey("CurrentTournament") ? (Tournament)SiteSession["CurrentTournament"] : null; }
            set { SiteSession["CurrentTournament"] = value; }
        }
    }




}
