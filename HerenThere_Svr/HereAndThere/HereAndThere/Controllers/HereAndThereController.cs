using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using HereAndThere.Models;
using HereAndThere.Utilities;
using Newtonsoft.Json;

namespace HereAndThere.Controllers
{
    /// <summary>
    /// </summary>
    public class HereAndThereController : ApiController
    {
        /// <summary>
        ///     Gets an ongoing Match. you can use GetMatchInfo, if you have the matchId
        /// </summary>
        /// <returns>
        ///     Ongoing Match Info example
        ///     {"id":1,"startTime":"2016-11-07T21:00:29","endTime":"2016-11-10T09:51:28.78","playerCount":3,"players":[{"name":"karri","score":0.0,"id":1},{"name":"test2","score":0.0,"id":4},{"name":"k","score":0.0,"id":5}],"boundaries":[{"name":"lowerLeft","latitude":65.0519270000,"longitude":25.4474250000},{"name":"upperRight","latitude":65.0640070000,"longitude":25.4756460000}]}
        /// </returns>
        public IHttpActionResult GetOnGoingMatch()
        {
            try
            {
                using (var db = new HereAndThereDbContext())
                {
                    var match = db.matches.Where(x => (x.endTime != null) && (x.endTime.Value > DateTime.Now))
                        .OrderByDescending(x => x.startTime)
                        .FirstOrDefault();
                    if (match == null) return BadRequest("No Ongoing Match");
                    return GetMatchInfo((int) match.id);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionProcessor.Process(ex));
            }
        }

        /// <summary>
        ///     Add a Player activity: Motion and Location
        /// </summary>
        /// <param name="activity">
        ///     the player activity eg.
        ///     {
        ///     "playerId":1,
        ///     "matchId":1,
        ///     "timeStamp":"2016-11-07T23:28:40.406Z",
        ///     "isMoving":true,
        ///     "locations":[
        ///     {"latitude":0.1,"longitude":0.1,"isActual":true,"isVisible":true,"timeStamp":"2016-11-07T23:28:40.406Z"}
        ///     ]
        ///     }
        /// </param>
        /// <returns>list of player locations example:  [
        ///     {"id":1,"latitude":0.1,"longitude":0.1,"isActual":true,"isVisible":true,"timeStamp":"2016-11-07T23:28:40.406Z","playerId":1,"matchId":1}
        ///     ]</returns>
        [
            HttpPost]
        public IHttpActionResult AddPlayerActivity(string activity)
        {
            try
            {
                using (var db = new HereAndThereDbContext())
                {
                    var activityType = new
                    {
                        playerId = 1,
                        matchId = 1,
                        timeStamp = new DateTime(),
                        isMoving = true,
                        locations = new[]
                        {
                            new
                            {
                                latitude = 0.1,
                                longitude = 0.1,
                                isActual = true,
                                isVisible = true,
                                timeStamp = new DateTime()
                            }
                        }
                    };

                    var data = JsonConvert.DeserializeAnonymousType(activity, activityType);

                    var match = db.matches.FirstOrDefault(x => x.id == data.matchId);
                    if (match == null) return BadRequest(string.Format("No Such match with id: {0}", data.matchId));
                    var player = db.players.FirstOrDefault(x => x.id == data.playerId);
                    if (player == null) return BadRequest(string.Format("No Such Player with id: {0}", data.matchId));

                    var movement = new Movement
                    {
                        matchId = data.matchId,
                        playerId = data.playerId,
                        timeStamp = data.timeStamp,
                        isMoving = data.isMoving
                    };
                    movement.SetAuditable(movement.timeStamp);
                    db.movements.Add(movement);

                    var locations = new List<Location>();
                    foreach (var location in data.locations)
                    {
                        var loc = new Location
                        {
                            latitude = (decimal) location.latitude,
                            longitude = (decimal) location.longitude,
                            isActual = location.isActual,
                            isVisible = location.isVisible,
                            timeStamp = location.timeStamp,
                            movement = movement
                        };
                        loc.SetAuditable(loc.timeStamp);
                        locations.Add(loc);
                    }
                    db.locations.AddRange(locations);
                    db.SaveChanges();

                    var returnedLocations =
                        locations.Select(
                            x =>
                                new
                                {
                                    x.id,
                                    x.latitude,
                                    x.longitude,
                                  
                                    x.isActual,
                                    x.isVisible,
                                    x.timeStamp,
                                    x.movement.playerId,
                                    x.movement.matchId
                                }).ToList();

                    return Ok(returnedLocations);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(ExceptionProcessor.Process(exception));
            }
        }

        /// <summary>
        ///     Make one or more Locations invisible
        /// </summary>
        /// <param name="locationIds">JSON list of location ids eg [1,2,3,4]</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult MakeLocationInvisible(string locationIds)
        {
            try
            {
                using (var db = new HereAndThereDbContext())
                {
                    var ids = JsonConvert.DeserializeObject<List<int>>(locationIds);
                    var locations = db.locations.Where(x => ids.Contains((int) x.id)).ToList();
                    foreach (var location in locations)
                    {
                        location.isVisible = false;
                        location.modifiedBy = "system";
                        location.modifiedTimeStamp = DateTime.Now;
                    }
                }

                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(ExceptionProcessor.Process(exception));
            }
        }

        /// <summary>
        ///     Gets all  currently visible Player locations of a Match
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns></returns>
        public IHttpActionResult GetAllPlayerLocations(int matchId)
        {
            try
            {
                using (var db = new HereAndThereDbContext())
                {
                    var match = db.matches.FirstOrDefault(x => x.id == matchId);
                    if (match == null) return BadRequest(string.Format("No Such match with id: {0}", matchId));
                    var movements =
                        db.locations.Where(x => (x.movement.matchId == matchId) && x.isVisible)
                            .Select(
                                x =>
                                    new
                                    {
                                        locationId = x.id,
                                        x.movement.playerId,
                                        playerName = x.movement.player.name,
                                        x.latitude,
                                        x.longitude,
                                        x.isActual,
                                        x.isVisible,
                                        x.movement.isMoving,
                                        x.timeStamp
                                    })
                            .ToList();
                    return Ok(movements);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionProcessor.Process(ex));
            }
        }

        /// <summary>
        ///     Gets All Players of a Match
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns>List of Players</returns>
        [ResponseType(typeof(IEnumerable<Player>))]
        public IHttpActionResult GetMatchPlayers(int matchId)
        {
            try
            {
                using (var db = new HereAndThereDbContext())
                {
                    var match = db.matches.FirstOrDefault(x => x.id == matchId);
                    if (match == null) return BadRequest(string.Format("No Such match with id: {0}", matchId));

                    return Ok(match.players.Select(x => new {x.id, x.name}).ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionProcessor.Process(ex));
            }
        }

        /// <summary>
        ///     Get the General Information about a match , all Players and their scores
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns>
        ///     The Match Info example :
        ///     {"id":1,"startTime":"2016-11-07T21:00:29","endTime":"2016-11-10T09:51:28.78","playerCount":3,"players":[{"name":"karri","score":0.0,"id":1},{"name":"test2","score":0.0,"id":4},{"name":"k","score":0.0,"id":5}],"boundaries":[{"name":"lowerLeft","latitude":65.0519270000,"longitude":25.4474250000},{"name":"upperRight","latitude":65.0640070000,"longitude":25.4756460000}]}
        /// </returns>
        public IHttpActionResult GetMatchInfo(int matchId)
        {
            try
            {
                using (var db = new HereAndThereDbContext())
                {
                    var match = db.matches.FirstOrDefault(x => x.id == matchId);
                    if (match == null) return BadRequest(string.Format("No Such match with id: {0}", matchId));

                    var players =
                        match.players.Select(x => new {x.name, score = x.scores.Sum(t => t.point), x.id}).ToList();
                    var boundaries = match.boundaries.Select(x => new {x.name, x.latitude, x.longitude}).ToList();

                    var result = new
                    {
                        match.id,
                        match.startTime,
                        match.endTime,
                        playerCount = match.players.ToList().Count,
                        players,
                        boundaries
                    };
                    return Ok(result);
                }
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
        ///     Gets all Players of a Match and their scores
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
                        var players =
                            match.players.Select(x => new {x.name, score = x.scores.Sum(t => t.point), x.id}).ToList();

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
        ///     Add Player to a Match
        /// </summary>
        /// <param name="name">the name of the player</param>
        /// <param name="matchId">the id  of the match the player plays</param>
        /// <returns>the PlayerId</returns>
        [HttpPost]
        [ResponseType(typeof(object))]
        public IHttpActionResult AddPlayerToMatch(string name, int matchId = 1)
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
        ///     Add a Player, also automatically add player to ongoing match (for now)
        /// </summary>
        /// <param name="name">the name of the player</param>
        /// <returns>the PlayerId and Match Id of ongoing match</returns>
        [HttpPost]
        public IHttpActionResult AddPlayer(string name)
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


                    var onGoingMatch =
                        player.matches.Where(x => (x.endTime != null) && (x.endTime.Value > DateTime.Now))
                            .OrderByDescending(x => x.startTime)
                            .FirstOrDefault();

                    if (onGoingMatch != null)
                    {
                        db.SaveChanges();
                        return Ok(new {playerId = player.id, onGoingMatchId = onGoingMatch.id});
                    }

                    //todo: for now, just keeping one match at a time, and adding players to that ongoing match
                    //todo: player creates a match ands sends request to others to accept and play 

                    var match = db.matches.Where(x => (x.endTime != null) && (x.endTime.Value > DateTime.Now))
                        .OrderByDescending(x => x.startTime)
                        .FirstOrDefault();

                    if (match == null)
                        return BadRequest("No Ongoing match, contact game administrator");

                    player.matches.Add(match);
                    db.SaveChanges();
                    return Ok(new {playerId = player.id, onGoingMatchId = match.id});
                }
                catch (Exception ex)
                {
                    return BadRequest(ExceptionProcessor.Process(ex));
                }
            }
        }

        /// <summary>
        ///     Adds a new Match. For now, if there is an ongoing Match, request is ignored and ongoing Match is returned. To
        ///     extend the duration of a Match please use ExtendMatchTime API
        /// </summary>
        /// <param name="startTime">DateTime the match start</param>
        /// <param name="endTime">DateTime the match ends</param>
        /// <param name="lowerLeftLatitude">Lower Left Latitude bound of match venue</param>
        /// <param name="lowerLeftLongitude">Lower Left Longitude bound of match venue</param>
        /// <param name="upperRightLatitude">Upper Right Latitude bound of match venue</param>
        /// <param name="upperRightLongitude">Upper Right Longitude bound of match venue</param>
        /// <returns>The Match </returns>
        [ResponseType(typeof(Match))]
        public IHttpActionResult AddMatch(decimal lowerLeftLatitude, decimal lowerLeftLongitude,
            decimal upperRightLatitude, decimal upperRightLongitude, DateTime startTime,
            DateTime endTime)
        {
            try
            {
                using (var db = new HereAndThereDbContext())
                {
                    //Keeping only one match at a time
                    var onGoingMatch =
                        db.matches.Where(x => (x.endTime != null) && (x.endTime.Value > DateTime.Now))
                            .OrderByDescending(x => x.startTime)
                            .FirstOrDefault();
                    if (onGoingMatch != null)
                        return Ok(onGoingMatch);

                    var match = new Match();
                    var lowerLeft = new Boundary
                    {
                        name = "lowerLeft",
                        latitude = lowerLeftLatitude,
                        longitude = lowerLeftLongitude
                    };
                    lowerLeft.SetAuditable();
                    var upperRight = new Boundary
                    {
                        name = "upperRight",
                        latitude = upperRightLatitude,
                        longitude = upperRightLatitude
                    };
                    upperRight.SetAuditable();


                    match.boundaries.Add(lowerLeft);
                    match.boundaries.Add(upperRight);
                    match.startTime = startTime;
                    match.endTime = endTime;
                    match.matchTypeId = Utils.DefaultMatchType.id;
                    match.SetAuditable();
                    db.matches.Add(match);

                    db.SaveChanges();


                    return Ok(match);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionProcessor.Process(ex));
            }
        }

        /// <summary>
        ///     Extends the end time of a Match. Request is ignored when the match is still running
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="newEndTime"></param>
        /// <returns></returns>
        public IHttpActionResult ExtendMatchTime(int matchId, DateTime newEndTime)
        {
            try
            {
                using (var db = new HereAndThereDbContext())
                {
                    var match = db.matches.FirstOrDefault(x => x.id == matchId);
                    if (match == null) return BadRequest(string.Format("No Such Match with id: {0}", matchId));
                    if (match.endTime.HasValue & (match.endTime.Value > newEndTime))
                        return Ok(string.Format("Request ignored, match ends after {0}", newEndTime));
                    match.endTime = newEndTime;
                    db.SaveChanges();
                    return Ok("Match End time extended");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionProcessor.Process(ex));
            }
        }

        /// <summary>
        ///     Adds a Player's game score or points. Points could be negetive or positive.
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
                        return BadRequest(string.Format("No Such spotted Player with id: {0}", spottedPlayerId));

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


        //    public IHttpActionResult PutPlayer(long id, Player player)
        //    [ResponseType(typeof(void))]

        //    // PUT: api/HereAndThere/5

        //        }

        //        Player player = dbx.players.Find(id);
        //    public IHttpActionResult GetPlayer(long id)
        //    [ResponseType(typeof(Player))]

        //    // GET: api/HereAndThere/5

        //    {
        //        if (player == null)
        //        {

        //            return NotFound();

        //        return Ok(player);

        //    }
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