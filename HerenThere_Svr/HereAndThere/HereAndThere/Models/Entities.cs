using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using HereAndThere.TypeConvertors;

namespace HereAndThere.Models
{
   

    public class Lookup:Auditable
    {
       
        [Required,MaxLength(32)]
        public string name { get; set; }
        public string description { get; set; }
       
    }

    public class Player:Auditable
    {
       [Required,MaxLength(32),Index(IsUnique = true)]
        public string name { get; set; }
        public long playerTypeId { get; set; }


        public  virtual PlayerType playerType{ get; set; }

        public virtual  ICollection<Score> scores { get; set; }
        public virtual ICollection<Score> scoresAgainst { get; set; }
        public  virtual  ICollection<Movement> movements { get; set; }
        public virtual ICollection<Match> matches { get; set; }

        public Player()
        {
            scores=new HashSet<Score>();
            movements = new HashSet<Movement>();
            scoresAgainst=new HashSet<Score>();
            matches=new HashSet<Match>();
        }
    }

    public class Score:Auditable
    {
       
        public long matchId { get; set; }
        public long playerId { get; set; }
       
        public long spottedPlayerId { get; set; }
        [Required]
        public decimal point { get; set; }

        public string description { get; set; }
        public virtual Match match { get; set; }

        public virtual Player player { get; set; }

        public virtual Player spottedPlayer { get; set; }
       
    }

    
    public class PlayerType : Lookup
    {
        public virtual ICollection<Player> players { get; set; }

        public PlayerType()
        {
            players = new HashSet<Player>();
        }
    }

    public class MatchType : Lookup
    {
        public virtual ICollection<Match> matches { get; set; }

        public MatchType()
        {
            matches=new HashSet<Match>();
        }
       
    }
    public class Match : Auditable
    {
        
        /// <summary>
        /// Match Type Id
        /// </summary>
        public long matchTypeId { get; set; }
        /// <summary>
        /// Start Time of the Match
        /// </summary>
        public DateTime startTime { get; set; }
        /// <summary>
        /// End Time of the Match (Optional)
        /// </summary>
        public DateTime? endTime { get; set; }
        public virtual MatchType matchType { get; set; }
        /// <summary>
        /// Boundaries of the Match 
        /// </summary>
        public virtual ICollection<Boundary> boundaries { get; set; }
        public virtual  ICollection<Score> scores { get; set; }
        public virtual ICollection<Player> players{ get; set; }
        public  virtual  ICollection<Movement> movements { get; set; }

        
        public Match()
        {
            boundaries=new HashSet<Boundary>();
            scores=new HashSet<Score>();
            players=new HashSet<Player>();
            movements=new HashSet<Movement>();
        }
    }
    //[TypeConverter(typeof(BoundaryTypeConvertor))]
    public class Boundary : Auditable
    {
       
        public long matchId { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }

    }

    public class Movement:Auditable
    {
       
        public long playerId { get; set; }
        public long matchId { get; set; }
        public bool isMoving { get; set; }
        public long locationId { get; set; }
        public virtual  Player player { get; set; }
        public virtual Match match { get; set; }
        public virtual ICollection<Location> locations { get; set; }

        public Movement()
        {
            locations=new HashSet<Location>();
        }
       
    }
    public class Location : Auditable
    {
       
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public bool isActual { get; set; }
        public bool isVisible { get; set; }
        public long movementId { get; set; }

        public virtual Movement movement { get; set; }
      
    }

    public class Auditable 
     {
        [Key]
        public long id { get; set; }
        ///<summary>When the record was created</summary>
        public  DateTime timeStamp { get; set; }
        ///<summary>Who made the entry</summary>
        [Required, MaxLength(32)]
       public string createdBy { get; set; }
        ///<summary>When the record was last modified</summary>
       
        public  DateTime? modifiedTimeStamp { get; set; }
        ///<summary>Who last modified the record</summary>
        [MaxLength(32)]
      public  string modifiedBy { get; set; }


     }

}