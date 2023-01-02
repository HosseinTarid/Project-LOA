using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Services.API_Service.Login;

namespace Assets.Script.Services.API_Service.Profile
{
    public class EditProfile: IApiCommands
    {
        private Dictionary<string, string> _parameters;
        private Dictionary<string, string> _headers;
        public EditProfile(string name, string email, string password, string password_confirmation)
        {
            _parameters = new Dictionary<string, string>();
            _parameters.Add("name", name);
            _parameters.Add("email", email);
            _parameters.Add("password", password);
            _parameters.Add("password_confirmation", password_confirmation);
            _parameters.Add("two_factor_authentication", "1");


        }
        public void Send<T>(HttpRequestAction<T> actions)
        {
            
        }
    }
}
