using LogoGo3Data;
using SharpSvn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Go3Interration.Models
{
    public class SvnHelper
    {
        public static SvnClient clientSvn()
        {

            SvnClient client = new SvnClient();
            client.Authentication.ForceCredentials(getSvnConf().Uname, getSvnConf().Pass);
            client.Authentication.SslServerTrustHandlers += new EventHandler<SharpSvn.Security.SvnSslServerTrustEventArgs>(Authentication_SslServerTrustHandlers);
            void Authentication_SslServerTrustHandlers(object sender, SharpSvn.Security.SvnSslServerTrustEventArgs e)
            {
                // Look at the rest of the arguments of E, whether you wish to accept

                // If accept:
                e.AcceptedFailures = e.Failures;
                e.Save = true; // Save acceptance to authentication store
            }


            return client;
        }


        public static SvnUpdateResult getRepoRevision()
        {

            using (SvnClient client = clientSvn())
            {
                SvnInfoEventArgs info;
                Uri repos = new Uri(getSvnConf().RepoUrl);

                client.GetInfo(repos, out info);
                SvnUpdateResult result;
                client.Update(getSvnConf().LocalRepo, out result);
              // client.Update(@"D:\SVN\go3ent", out result);
                return result;


            }
        }


        public static SvnConf getSvnConf() {
          return  GenerateProcess.readSvnConf().deserializeJson<SvnConf>();
        }


       

    }

    public class SvnConf
    {

        public string RepoUrl { get; set; }
        public string LocalRepo { get; set; }
        public string Uname { get; set; }
        public string Pass { get; set; }
    }
}