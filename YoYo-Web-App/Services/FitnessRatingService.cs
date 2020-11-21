using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using YoYo_Web_App.Models;

namespace YoYo_Web_App.Services
{
    public partial class FitnessRatingService : IFitnessRatingService
    {
        #region Fields

        #endregion

        #region Ctor
        public FitnessRatingService()
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Geting fitness ratings from json
        /// </summary>
        /// <returns></returns>
        public List<FitnessRating> GetFitnessRatingsFromJson()
        {
            var jsonFile = "DATA/fitnessrating_beeptest.json";
            try
            {
                List<FitnessRating> List = null;

                string jsonString = File.ReadAllText(jsonFile);
                JArray array = JArray.Parse(jsonString);

                List = new List<FitnessRating>();

                foreach (JObject obj in array.Children<JObject>())
                {
                    string name = string.Empty;
                    string value = string.Empty;

                    FitnessRating fitnessRating = new FitnessRating();

                    foreach (JProperty singleProp in obj.Properties())
                    {
                        name = singleProp.Name;
                        value = singleProp.Value.ToString();

                        if (name.Equals("AccumulatedShuttleDistance"))
                            fitnessRating.AccumulatedShuttleDistance = Convert.ToInt32(value);
                        else if (name.Equals("CommulativeTime"))
                            fitnessRating.CommulativeTime = value;
                        else if (name.Equals("SpeedLevel"))
                            fitnessRating.SpeedLevel = Convert.ToInt32(value);
                        else if (name.Equals("ShuttleNo"))
                            fitnessRating.ShuttleNo = Convert.ToInt32(value);
                        else if (name.Equals("Speed"))
                            fitnessRating.Speed = Convert.ToDouble(value);
                        else if (name.Equals("LevelTime"))
                            fitnessRating.LevelTime = Convert.ToDouble(value);
                        else if (name.Equals("CommulativeTime"))
                            fitnessRating.CommulativeTime = value;
                        else if (name.Equals("StartTime"))
                            fitnessRating.StartTime = value;
                        else if (name.Equals("ApproxVo2Max"))
                            fitnessRating.ApproxVo2Max = Convert.ToDouble(value);
                    }

                    List.Add(fitnessRating);
                }

                return List != null ? List : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get players from json
        /// </summary>
        /// <returns></returns>
        public List<Players> GetPlayersFromJson()
        {
            List<Players> List = null;
            var jsonFile = "DATA/Playes.json";
            try
            {
                string jsonString = File.ReadAllText(jsonFile);
                JArray array = JArray.Parse(jsonString);

                List = new List<Players>();

                foreach (JObject obj in array.Children<JObject>())
                {
                    string name = string.Empty;
                    string value = string.Empty;
                    Players player = new Players();

                    foreach (JProperty singleProp in obj.Properties())
                    {
                        name = singleProp.Name;
                        value = singleProp.Value.ToString();

                        if (name.Equals("Id"))
                            player.Id = Convert.ToInt32(value);
                        if (name.Equals("Name"))
                            player.Name = value;
                    }

                    List.Add(player);

                }

                return List != null ? List : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// spliting list colum wise
        /// </summary>
        /// <param name="fitnessRatings"></param>
        /// <param name="Column"></param>
        /// <returns></returns>
        public List<string> GetList(List<FitnessRating> fitnessRatings, string Column)
        {
            try
            {
                if (fitnessRatings == null)
                    return null;

                List<string> List = new List<string>();

                foreach (var Item in fitnessRatings)
                {
                    switch (Column)
                    {
                        case "StartTime":
                            List.Add(Item.StartTime);
                            break;
                        case "CommulativeTime":
                            List.Add(Item.CommulativeTime);
                            break;
                        case "Speed":
                            List.Add(Convert.ToString(Item.Speed));
                            break;
                        case "SpeedLevel":
                            List.Add(Convert.ToString(Item.SpeedLevel));
                            break;
                        case "ShuttleNo":
                            List.Add(Convert.ToString(Item.ShuttleNo));
                            break;
                        case "AccumulatedShuttleDistance":
                            List.Add(Convert.ToString(Item.AccumulatedShuttleDistance));
                            break;
                        case "LevelTime":
                            List.Add(Convert.ToString(Item.LevelTime));
                            break;
                        default:
                            break;
                    }
                }

                return List;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
