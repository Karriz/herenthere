using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HereAndThere.Models;
using HereAndThere.Utilities;

namespace HereAndThere.Controllers
{
    public class MatchesController : Controller
    {
        private HereAndThereDbContext db = new HereAndThereDbContext();

        // GET: Matches
        public ActionResult Index()
        {
            var matches = db.matches.Include(m => m.matchType);
            return View(matches.ToList());
        }

        // GET: Matches/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Match match = db.matches.Find(id);
            if (match == null)
            {
                return HttpNotFound();
            }
            return View(match);
        }

        // GET: Matches/Create
        public ActionResult Create()
        {
            ViewBag.matchTypeId = new SelectList(db.matchTypes, "id", "name");
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public ActionResult Create(decimal lowerLeftLatitude, decimal lowerLeftLongitude,
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
                        ViewBag.StatusMessage=string.Format("Ongoing match with Id: {0}",onGoingMatch.id);

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
                        longitude = upperRightLongitude
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

                    ViewBag.StatusMessage = "Match Created Successfully";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.StatusMessage = ExceptionProcessor.Process(ex);
                return View();
                
            }
           
           
        }

        // GET: Matches/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Match match = db.matches.Find(id);
            if (match == null)
            {
                return HttpNotFound();
            }
            ViewBag.matchTypeId = new SelectList(db.matchTypes, "id", "name", match.matchTypeId);
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Edit(int id, DateTime endTime)
        {
            try
            {
                using (var db = new HereAndThereDbContext())
                {
                    var match = db.matches.FirstOrDefault(x => x.id == id);
                    if (match == null)
                    {
                        ViewBag.StatusMessage = string.Format("No Such Match with id: {0}", id);
                        return View(match);
                    }

                    if (match.endTime.HasValue & (match.endTime.Value > endTime))
                    {
                        ViewBag.StatusMessage=string.Format("Request ignored, match ends after {0}", endTime);
                        return View(match);
                    }
                        
                    match.endTime = endTime;
                    db.SaveChanges();
                    ViewBag.StatusMessage=
                    "Match End Time Extended successfully";
                    return View(match );
                }
            }
            catch (Exception ex)
            {
                ViewBag.StatusMessage = ExceptionProcessor.Process(ex);
                
                return View();
            }
           
        }

        // GET: Matches/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Match match = db.matches.Find(id);
            if (match == null)
            {
                return HttpNotFound();
            }
            return View(match);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
           
            
            try
            {
                using (var db = new HereAndThereDbContext())
                {
                    var match = db.matches.FirstOrDefault(x => x.id == id);
                    if (match == null) {   ViewBag.StatusMessage=  string.Format("No Such Match with id: {0}", id) ;
                        return View();
                    }

                    var endTime = DateTime.Now;
                    match.endTime = endTime;
                    db.SaveChanges();
                    
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.StatusMessage = ExceptionProcessor.Process(ex);
                return View();
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
