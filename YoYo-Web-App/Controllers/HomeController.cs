using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YoYo_Web_App.DATA;
using YoYo_Web_App.Models;
using YoYo_Web_App.Services;

namespace YoYo_Web_App.Controllers
{
    public class HomeController : Controller
    {
        #region Fields
        private readonly ILogger<HomeController> _logger;
        private readonly IFitnessRatingService _fitnessRatingService;
        private readonly YoyoContext _yoyoContext;
        #endregion

        #region Ctor
        public HomeController(ILogger<HomeController> logger,
            IFitnessRatingService fitnessRatingService,
            YoyoContext yoyoContext)
        {
            _logger = logger;
            _fitnessRatingService = fitnessRatingService;
            _yoyoContext = yoyoContext;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initial metho
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var Players = _fitnessRatingService.GetPlayersFromJson();

            var model = new YoyoViewModel();

            if (Players != null)
                model.Players = Players;

            return View(model);
        }

        /// <summary>
        /// geting timer data from json
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TimerListAjax()
        {
            var FitnessRating = _fitnessRatingService.GetFitnessRatingsFromJson();
            var Players = _fitnessRatingService.GetPlayersFromJson();
            var existingPlayer = _yoyoContext.TrackPlayers.ToArray();

            if (FitnessRating != null)
            {
                var StartTimer = _fitnessRatingService.GetList(FitnessRating, "StartTime");
                var EndTimer = _fitnessRatingService.GetList(FitnessRating, "CommulativeTime");
                var Speed = _fitnessRatingService.GetList(FitnessRating, "Speed");
                var SpeedLevel = _fitnessRatingService.GetList(FitnessRating, "SpeedLevel");
                var ShuttleNo = _fitnessRatingService.GetList(FitnessRating, "ShuttleNo");
                var Distance = _fitnessRatingService.GetList(FitnessRating, "AccumulatedShuttleDistance");
                var LevelTime = _fitnessRatingService.GetList(FitnessRating, "LevelTime");
                var playerCount = Players != null ? Players.Count : 0;
                return Json(new[]
                { new
                    {
                        startTimer = StartTimer.ToArray(),
                        endTimer = EndTimer.ToArray(),
                        speed = Speed.ToArray(),
                        speedLevel = SpeedLevel.ToArray(),
                        shuttleNo = ShuttleNo.ToArray(),
                        distance = Distance.ToArray(),
                        levelTime = LevelTime.ToArray(),
                        playerCount = playerCount,
                        existingPlayer= existingPlayer
                    }
                });
            }

            return null;
        }

        /// <summary>
        /// Add track temp data to entity
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="warnTime"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> WarnPlayer(int Id, string warnTime)
        {
            if (Id <= 0 || string.IsNullOrEmpty(warnTime))
                return Json(new[]
                { new{
                    error="values are null"
                    }
                });

            var Players = _fitnessRatingService.GetPlayersFromJson();
            var player = Players.Where(x => x.Id.Equals(Id)).FirstOrDefault();
            TrackPlayers trackPlayers = null;

            if (player != null)
            {
                trackPlayers = new TrackPlayers()
                {
                    Id = Id,
                    name = player.Name,
                    warned = warnTime,
                    stoped = string.Empty,
                    shuttleNo = string.Empty,
                    speedLevel = string.Empty
                };

                if (trackPlayers != null)
                {
                    var IsExist = _yoyoContext.TrackPlayers.Where(x => x.Id.Equals(Id)).FirstOrDefault();
                    if (IsExist == null)
                    {
                        _yoyoContext.TrackPlayers.Add(trackPlayers);
                        await _yoyoContext.SaveChangesAsync();
                    }
                    else
                    {
                        return Json(new { error = "" });
                    }
                }
            }

            return Json(new {success = trackPlayers});
        }

        /// <summary>
        /// Update track temp data to entity
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="stopTime"></param>
        /// <param name="shuttle"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> StopPlayer(int Id, string stopTime, string shuttle, string level)
        {
            if (Id <= 0 || string.IsNullOrEmpty(stopTime) || string.IsNullOrEmpty(shuttle) || string.IsNullOrEmpty(level))
                return Json(new { error = "values are null" });

            var existingPlayer = _yoyoContext.TrackPlayers.Where(x => x.Id.Equals(Id)).FirstOrDefault();

            if (existingPlayer != null)
            {
                existingPlayer.stoped = stopTime;
                existingPlayer.speedLevel = level;
                existingPlayer.shuttleNo = shuttle;

                await _yoyoContext.SaveChangesAsync();

                return Json(new { success = existingPlayer });
            }

            return Json(new { error = "" });
        }

        /// <summary>
        /// default core method
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// default core method
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
