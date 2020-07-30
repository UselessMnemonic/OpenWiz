using System.Text.Json;
using System.Text.Json.Serialization;
using System;

namespace OpenWiz
{
    /// <summary>
    /// Models the state communicated by a Wiz light.
    /// </summary>
    ///
    public class WizState
    {
        /// <summary>
        /// The method name, required for all state objects.
        /// </summary>
        /// <value>A OpenWiz.WizMethod.</value>
        /// 
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WizMethod Method { get; set; }

        /// <summary>
        /// Describes the parameters of a method call.
        /// </summary>
        /// <value>A OpenWiz.Params, or null if no params are given.</value>
        /// 
        public WizParams Params { get; set; }

        /// <summary>
        /// Describes the result of a method call.
        /// </summary>
        /// <value>A OpenWiz.Result, or null if no result is given.</value>
        /// 
        public WizResult Result { get; set; }

        /// <summary>
        /// Describes an error returned by a Wiz light.
        /// </summary>
        /// <value>A OpenWiz.WizError, or null if no error is given..</value>
        /// 
        public WizError Error { get; set; }

        /// <summary>
        /// The ID of the state.
        /// </summary>
        /// <value>The int value of the ID, or null if no ID is given.</value>
        /// 
        public int? Id { get; set; }

        /// <summary>
        /// Generates an object that can be used to register the host with a Wiz light.
        /// </summary>
        /// <param name="homeId">The Home ID of the Wiz light</param>
        /// <param name="hostIp">The IP of the host machine</param>
        /// <param name="hostMac">The MAC of the host machine's interface card</param>
        /// <returns>An object containing registration information.</returns>
        ///
        public static WizState MakeRegistration(int homeId, string hostIp, byte[] hostMac)
        {
            return new WizState {
                Method = WizMethod.registration,
                Params = new WizParams {
                    HomeId = homeId,
                    PhoneIp = hostIp,
                    PhoneMac = BitConverter.ToString(hostMac).Replace("-","").ToLower(),
                    Register = true
                }
            };
        }

        /// <summary>
        /// Generates an object that can be used to request the current state of a light.
        /// </summary>
        /// <returns>An object containing the request.</returns>
        ///
        public static WizState MakeGetPilot()
        {
            return new WizState {
                Method = WizMethod.getPilot
            };
        }

        /// <summary>
        /// Generates an object that can be used to request the user's configuration of a light.
        /// </summary>
        /// <returns>An object containing the request.</returns>
        /// 
        public static WizState MakeGetUserConfig()
        {
            return new WizState {
                Method = WizMethod.getUserConfig
            };
        }

        /// <summary>
        /// Generates an object that can be used to request the internal configuration of a light.
        /// </summary>
        /// <returns>An object containing the request.</returns>
        /// 
        public static WizState MakeGetSystemConfig()
        {
            return new WizState {
                Method = WizMethod.getSystemConfig
            };
        }

        /// <summary>
        /// Parses a json string into its object representation.
        /// </summary>
        /// <param name="jsonString">A JSON String</param>
        /// <returns>A state object, or null if one cannot be constructed<./returns>
        ///
        public static WizState Parse(string jsonString)
        {
            if (jsonString == null) return null;
            WizState state = null;
            try
            {
                state = JsonSerializer.Deserialize<WizState>(jsonString, new JsonSerializerOptions {
                    IgnoreNullValues = true,
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException e)
            {
                Console.WriteLine($"[INFO] WizLightState::Parse encountered an exception -- {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
            return state;
        }

        /// <summary>
        /// Parses an object from a byte array.
        /// </summary>
        /// <param name="bytes">A byte array holding UTF8 encoded json</param>
        /// <returns>A state object, or null if one cannot be constructed.</returns>
        ///
        public static WizState Parse(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 1) return null;
            WizState state = null;
            try
            {
                state = JsonSerializer.Deserialize<WizState>(bytes, new JsonSerializerOptions {
                    IgnoreNullValues = true,
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException e)
            {
                Console.WriteLine($"[INFO] WizLightState::Parse encountered an exception -- {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
            return state;
        }

        /// <summary>
        /// Parses an object from an ArraySegment.
        /// </summary>
        /// <param name="bytes">A byte array holding UTF8 encoded json</param>
        /// <returns>A state object, or null if one cannot be constructed.</returns>
        ///
        public static WizState Parse(ArraySegment<byte> bytes)
        {
            if (bytes == null || bytes.Count < 1) return null;
            WizState state = null;
            try
            {
                state = JsonSerializer.Deserialize<WizState>(bytes, new JsonSerializerOptions {
                    IgnoreNullValues = true,
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException e)
            {
                Console.WriteLine($"[INFO] WizLightState::Parse encountered an exception -- {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
            return state;
        }

        /// <summary>
        /// Serializes the state object into raw UTF8 JSON.
        /// </summary>
        /// <returns>A UTF8 encoding of this state, in JSON.</returns>
        ///
        public byte[] ToUTF8()
        {
            return JsonSerializer.SerializeToUtf8Bytes(this, new JsonSerializerOptions {
                IgnoreNullValues = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        /// <summary>
        /// Serializes the state object into a JSON string.
        /// </summary>
        /// <returns>A JSON encoding of this state.</returns>
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