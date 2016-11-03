using System;
using System.Linq;
using HereAndThere.Models;

namespace HereAndThere.Utilities
{
    public class Utils
    {
        public static PlayerType DefaultPlayerType { get; set; }
        public static MatchType DefaultMatchType { get; set; }


        public static void LoadSystemDefaultData()
        {
            using (var db = new HereAndThereDbContext())
            {
                DefaultPlayerType = db.playerTypes.FirstOrDefault(x => x.name == "DEFAULT");
                DefaultMatchType = db.matchTypes.FirstOrDefault(x => x.name == "DEFAULT");
            }
        }
    }

    public static class Auxiliary
    {
        public static void SetAuditable(this Auditable o)
        {
            o.createdBy = "system";
            o.timeStamp = DateTime.Now;
        }
    }
}