using System.Text.Json;
using System.Text.Json.Serialization;
using System;

namespace OpenWiz
{
    /// <summary>Class <c>WizLightState</c> models the state communicated by a Wiz
    ///   light.</summary>
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

        /// <summary>Method <c>MakeRegistration</c> generates an object that can be
        ///   used to register the host with a Wiz light.</summary>
        /// <param name="homeId">The Home ID of the Wiz light</param>
        /// <param name="hostIp">The IP of the host machine</param>
        /// <param name="hostMac">The MAC of the host machine's interface card</param>
        /// <returns>A <c>WizLightState</c> containing registration information.</returns>
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

        /// <summary>Method <c>MakeGetState</c> generates an object that can be
        ///   used to request the current state of a light.</summary>
        /// <param name="homeId">The Home ID of the Wiz light</param>
        ///   on which lights are reachable</param>
        /// <returns>A <c>WizLightState</c> containing the request.</returns>
        ///
        public static WizState MakeGetState(int homeId)
        {
            return new WizState {
                Method = WizMethod.getPilot,
                Id = 0,
                Params = new WizParams {
                }
            };
        }

        /// <summary>Parses a <c>WizLightState</c> from a json string</summary>
        /// <param name="jsonString">A JSON String</param>
        /// <returns>A <c>WizLightState</c> representation of the string,
        ///   or <c>null</c> if one cannot be constructed</returns>
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

        /// <summary>Parses a <c>WizLightState</c> from a byte array</summary>
        /// <param name="bytes">A byte array holding UTF8 encoded json</param>
        /// <returns>A <c>WizLightState</c> representation of the encoded state,
        ///   or <c>null</c> if one cannot be constructed</returns>
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

        /// <summary>Parses a <c>WizLightState</c> from an <c>ArraySegment</c></summary>
        /// <param name="bytes">A byte array holding UTF8 encoded json</param>
        /// <returns>A <c>WizLightState</c> representation of the encoded state,
        ///   or <c>null</c> if one cannot be constructed</returns>
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

        /// <summary>Method <c>ToUTF8</c> serializes the <c>WizLightState</c>
        ///   into raw UTF8.</summary>
        /// <returns>A UTF8 encoding of this state</returns>
        ///
        public byte[] ToUTF8()
        {
            return JsonSerializer.SerializeToUtf8Bytes(this, new JsonSerializerOptions {
                IgnoreNullValues = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
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