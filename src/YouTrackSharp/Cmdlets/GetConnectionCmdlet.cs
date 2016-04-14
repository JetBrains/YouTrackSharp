// *************************************************
// YouTrackSharp.GetConnectionCmdlet.cs
// Last Modified: 04/04/2016 1:52 PM
// Modified By: Green, Brett (greenb1)
// *************************************************

using System.Management.Automation;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "connection")]
    public class GetConnectionCmdlet : PSCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Username")]
        [ValidateNotNull]
        public string Username { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Password")]
        [ValidateNotNull]
        public string Password { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Host")]
        [ValidateNotNull]
        public string YouTrackHost { get; set; }

        private bool _useSSL = false;

        [Parameter(Mandatory = true, HelpMessage = "UseSSL")]
        [ValidateNotNull]
        public bool UseSSL
        {
            get { return _useSSL; }
            set { _useSSL = value; }
        }

        private int _port = 80;

        [Parameter(Mandatory = true, HelpMessage = "Port")]
        [ValidateNotNull]
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        protected override void ProcessRecord()
        {
            var connection = new Connection(YouTrackHost, Port, UseSSL);
            connection.Authenticate(Username, Password);
            WriteObject(connection);
        }
    }
}