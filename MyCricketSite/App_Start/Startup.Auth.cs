using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.Owin;

using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using System.Security.Claims;
using System.Threading.Tasks;
namespace MyCricketSite
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            // clientId: "",
            // clientSecret: "");

            //app.UseTwitterAuthentication(
            // consumerKey: "",
            // consumerSecret: "");

            //app.UseFacebookAuthentication(
            // appId: "",
            // appSecret: "");

            var googleOAuth2AuthenticationOptions = new GoogleOAuth2AuthenticationOptions
            {
                ClientId = "663626932144-khkhi77m34qgkr83jc2vonv4senhpiji.apps.googleusercontent.com",
                ClientSecret = "Y00ksJWhKSz44a9dNwj8uJQr",
                CallbackPath = new PathString("/Account/ExternalLoginCallback/"),
                Provider = new GoogleOAuth2AuthenticationProvider()
                {
                    OnAuthenticated = context =>
                    {
                        var userDetail = context.User;
                        context.Identity.AddClaim(new Claim(ClaimTypes.Name, context.Identity.FindFirstValue(ClaimTypes.Name)));
                        context.Identity.AddClaim(new Claim(ClaimTypes.Email, context.Identity.FindFirstValue(ClaimTypes.Email)));
                        var img = context.User.GetValue("image");
                        if (img != null)
                            context.Identity.AddClaim(new Claim("Image", img.ToString()));



                        var gender = userDetail.Value<string>("gender");
                        context.Identity.AddClaim(new Claim(ClaimTypes.Gender, gender));

                        var picture = userDetail.Value<string>("picture");
                        if (picture != null)
                            context.Identity.AddClaim(new Claim("picture", picture));

                        return Task.FromResult(0);
                    }
                }
            };

            //googleOAuth2AuthenticationOptions.Scope.Add("email");
            googleOAuth2AuthenticationOptions.Scope.Add("https://www.googleapis.com/auth/plus.login");
            googleOAuth2AuthenticationOptions.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
            googleOAuth2AuthenticationOptions.Scope.Add("https://www.googleapis.com/auth/userinfo.email");

            app.UseGoogleAuthentication(googleOAuth2AuthenticationOptions);

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "663626932144-khkhi77m34qgkr83jc2vonv4senhpiji.apps.googleusercontent.com",
            //    ClientSecret = "Y00ksJWhKSz44a9dNwj8uJQr",
            //    CallbackPath = new PathString("/Account/ExternalLoginCallback/")
            //});
        }
    }
}