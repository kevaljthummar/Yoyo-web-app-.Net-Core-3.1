using Microsoft.EntityFrameworkCore;
using YoYo_Web_App.Models;

namespace YoYo_Web_App.DATA
{
    public class YoyoContext : DbContext
    {
        public YoyoContext(DbContextOptions<YoyoContext> options) : base(options)
        {

        }

        public DbSet<TrackPlayers> TrackPlayers { get; set; }
    }
}
