using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using HereAndThere.Models;
using HereAndThere.Utilities;

namespace HereAndThere.Controllers
{


    /// <summary>
    /// </summary>
    public class HereAndThereController : ApiController
    {

        public IHttpActionResult GetMatchInfo(int matchId)
        {
            try
            {
                return Ok("Comming Soon");
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionProcessor.Process(ex));
            }
        }
        // GET: api/HereAndThere
        // [ResponseType(typeof(IEnumerable<Player>))]
        /// <summary>
        ///     Gets all Players
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(List<Player>))]
        public IHttpActionResult GetPlayers()
        {
            using (var db = new HereAndThereDbContext())
            {
                try
                {
                    var players = db.players.Select(x => new {x.id, x.name}).ToList();
                    return Ok(players);
                }
                catch (Exception ex)
                {
                    return BadRequest(ExceptionProcessor.Process(ex));
                }
            }
        }

        /// <summary>
        ///     Get All Players of a match and their scores
        /// </summary>
        /// <param name="matchId"> Match Id</param>
        /// <returns> List of Players of the Match</returns>
       
        public IHttpActionResult GetMatchScores(int matchId)
        {
            using (var db = new HereAndThereDbContext())
            {
                try
                {
                    // var players = db.matches.Where(x=>x.id==matchId).Select(x => new { x., x.name }).ToList();
                    var match = db.matches.FirstOrDefault(x => x.id == matchId);

                    if (match != null)
                    {
                        var players =match.players. Select(x=> new  { x.name, score = x.scores.Sum(t => t.point), x.id}).ToList();

                        return Ok(players);
                    }
                   return BadRequest(string.Format("No Such match with id: {0}", matchId));
                }
                catch (Exception ex)
                {
                    return BadRequest(ExceptionProcessor.Process(ex));
                }
            }
        }


        /// <summary>
        ///     Add Player to a match
        /// </summary>
        /// <param name="name">the name of the player</param>
        /// <param name="matchId">the id  of the match the player plays</param>
        /// <returns>the PlayerId</returns>
        [HttpPost]
        [ResponseType(typeof(int))]
        public IHttpActionResult AddPlayer(string name, int matchId = 1)
        {
            using (var db = new HereAndThereDbContext())
            {
                try
                {
                    var player = new Player {name = name, playerTypeId = Utils.DefaultPlayerType.id};
                    player.SetAuditable();

                    var existingPlayer =
                        db.players.FirstOrDefault(x => x.name.ToLower() == player.name.ToLower());
                    if (existingPlayer != null) player = existingPlayer;
                    else
                        db.players.Add(player);


                    //Todo: check whether the match has ended
                    var match = db.matches.FirstOrDefault(x => x.id == matchId);
                    if (match == null) return BadRequest(string.Format("No Such match with id: {0}", matchId));

                    if (!match.players.Contains(player)) match.players.Add(player);


                    db.SaveChanges();
                    return Ok(new {playerId = player.id});
                }
                catch (Exception ex)
                {
                    return BadRequest(ExceptionProcessor.Process(ex));
                }
            }
        }


        /// <summary>
        ///     Add a new Match
        /// </summary>
        /// <param name="lowerLeft"> Lower left bound of match venue</param>
        /// <param name="upperRight">upper left bound of match venue boundary</param>
        /// <param name="startTime">DateTime the match start</param>
        /// <param name="endTime">DateTime the match ends</param>
        /// <returns></returns>
        [ResponseType(typeof(Match))]
        public IHttpActionResult AddMatch(Boundary lowerLeft, Boundary upperRight, DateTime startTime,
            DateTime endTime)
        {
            try
            {
                using (var db = new HereAndThereDbContext())
                {
                    var match = new Match();
                    if (lowerLeft != null) lowerLeft.SetAuditable();
                    if (upperRight != null) upperRight.SetAuditable();

                    match.boundaries.Add(lowerLeft);
                    match.boundaries.Add(upperRight);
                    match.startTime = startTime;
                    match.endTime = endTime;
                    match.matchTypeId = Utils.DefaultMatchType.id;
                    match.SetAuditable();
                    db.matches.Add(match);

                    db.SaveChanges();


                    return Json(match);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionProcessor.Process(ex));
            }
        }

        /// <summary>
        ///     Adds a player Score
        /// </summary>
        /// <param name="playerId"> Player Id</param>
        /// <param name="matchId"> Match Id of the match the player is playing</param>
        /// <param name="point">score or point obtained </param>
        /// <param name="timeStamp">the DateTime the score was obtained</param>
        /// <param name="spottedPlayerId">the Id of the Player who was spotted</param>
        /// <param name="description">Description of the reason for the score</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddPlayeScore(int playerId, int matchId, decimal point, DateTime timeStamp,
            int spottedPlayerId,
            string description = "")
        {
            try
            {
                using (var db = new HereAndThereDbContext())
                {
                    var player = db.players.FirstOrDefault(x => x.id == playerId);
                    if (player == null) return BadRequest(string.Format("No Such Player with id: {0}", playerId));
                    var match = db.matches.FirstOrDefault(x => x.id == matchId);
                    if (match == null) return BadRequest(string.Format("No Such Match with id: {0}", matchId));
                    var spottedPlayer = db.players.FirstOrDefault(x => x.id == spottedPlayerId);
                    if (spottedPlayer == null)
                        return BadRequest(string.Format("No Such spotted Player with id: {0}", spottedPlayer));

                    if (playerId == spottedPlayerId) return BadRequest("Player Cannot Spot self for a score ");

                    var score = new Score
                    {
                        matchId = matchId,
                        description = description,
                        timeStamp = timeStamp,
                        playerId = playerId,
                        point = point,
                        spottedPlayerId = spottedPlayerId
                    };

                    score.SetAuditable();

                    db.scores.Add(score);
                    db.SaveChanges();
                    return Ok("Score added sucessfully");
                }
            }
            catch (Exception exception)
            {
                return BadRequest(ExceptionProcessor.Process(exception));
            }
        }

        //        Player player = dbx.players.Find(id);
        //    public IHttpActionResult GetPlayer(long id)
        //    [ResponseType(typeof(Player))]

        //    // GET: api/HereAndThere/5

        //    {
        //        if (player == null)
        //        {
        //            return NotFound();
        //        }

        //        return Ok(player);
        //    }

        //    // PUT: api/HereAndThere/5
        //    [ResponseType(typeof(void))]
        //    public IHttpActionResult PutPlayer(long id, Player player)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        if (id != player.id)
        //        {
        //            return BadRequest();
        //        }

        //        dbx.Entry(player).State = EntityState.Modified;

        //        try
        //        {
        //            dbx.SaveChanges();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PlayerExists(id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return StatusCode(HttpStatusCode.NoContent);
        //    }

        //    // POST: api/HereAndThere
        //    [ResponseType(typeof(Player))]
        //    public IHttpActionResult PostPlayer(Player player)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        dbx.players.Add(player);
        //        dbx.SaveChanges();

        //        return CreatedAtRoute("DefaultApi", new { id = player.id }, player);
        //    }

        //    // DELETE: api/HereAndThere/5
        //    [ResponseType(typeof(Player))]
        //    public IHttpActionResult DeletePlayer(long id)
        //    {
        //        Player player = dbx.players.Find(id);
        //        if (player == null)
        //        {
        //            return NotFound();
        //        }

        //        dbx.players.Remove(player);
        //        dbx.SaveChanges();

        //        return Ok(player);
        //    }

        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing)
        //        {
        //            dbx.Dispose();
        //        }
        //        base.Dispose(disposing);
        //    }

        //    private bool PlayerExists(long id)
        //    {
        //        return dbx.players.Count(e => e.id == id) > 0;
        //    }
    }
}