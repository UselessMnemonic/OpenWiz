using System.Text.Json;
using System.Collections.Generic;

namespace OpenWIZ
{
    /// <summary>Class <c>WizLightState</c> models the state communicated by a Wiz
    ///   light.</summary>
    ///
    public class WizLightState
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public IDictionary<string, object> Params { get; set; }

        /// <summary>Method <c>MakeRegistration</c> generates a state that can be
        ///   used to register the host with a Wiz light.</summary>
        /// <param name="homeId">The Home ID of the Wiz light</param>
        /// <param name="hostIp">The IP of the host machine</param>
        /// <param name="hostMac">The MAC of the host machine's interface card,
        ///   on which lights are reachable</param>
        /// <returns>A <c>WizLightState</c> containing registration information.</returns>
        ///
        public static WizLightState MakeRegistration(int homeId, string hostIp, string hostMac)
        {
            WizLightState state = new WizLightState
            {
                Id = 0,
                Method = "registration",
                Params = new Dictionary<string, object>()
            };
            state.Params.Add("homeId", homeId);
            state.Params.Add("phoneIp", hostIp);
            state.Params.Add("phoneMac", hostMac.ToLower());
            state.Params.Add("register", true);
            return state;
        }

        /// <summary>Method <c>ToString</c> serializes the <c>WizLightState</c>
        ///   into JSON.</summary>
        /// <returns>A JSON string</returns>
        ///
        override public string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions {
                IgnoreNullValues = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}