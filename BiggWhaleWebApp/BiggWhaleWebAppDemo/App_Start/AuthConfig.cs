using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Membership.OpenAuth;

namespace BiggWhaleWebAppDemo
{
    internal static class AuthConfig
    {
        public static void RegisterOpenAuth()
        {
            // See http://go.microsoft.com/fwlink/?LinkId=252803 for details on setting up this ASP.NET
            // application to support logging in via external services.

            //OpenAuth.AuthenticationClients.AddTwitter(
            //    consumerKey: "your Twitter consumer key",
            //    consumerSecret: "your Twitter consumer secret");

            OpenAuth.AuthenticationClients.AddFacebook(
                //appId: "1847537955479968",
                appId: "1847537955470000",
                appSecret: "456148e370d05b74854f0603190ef070");

            OpenAuth.AuthenticationClients.AddMicrosoft(
                clientId: "7d5a6869-9d38-46f4-aba3-cf7bcb0ba9c5",
                clientSecret: "ohiVbQu73Kz1JGZ8QVQQ4mu");

            //OpenAuth.AuthenticationClients.AddGoogle();
        }
    }
}