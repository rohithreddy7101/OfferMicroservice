using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfferMicroservice.Models;
using OfferMicroservice.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OfferMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OfferController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(OfferController));
        private readonly IOfferService ser;
        public OfferController(IOfferService _ser)
        {
            _log4net.Info("offer controller initiated");
            ser = _ser;
        }

        [HttpGet]
        [Route("GetOffersList")]
        public IList<Offer> GetOffersList()
        {
            _log4net.Info("In offers controller HttpGet GetOffersList method is initiated");
            
            return ser.GetOffersList();
        }

        [HttpGet]
        [Route("GetOfferById/{id}")]
        public ActionResult<Offer> GetOfferById(int id)
        {
            _log4net.Info("In offers controller HttpGet GetOfferById and" + id + "is searched");
            return ser.GetOfferById(id);
        }

        [HttpGet]
        [Route("GetOfferByCategory/{category}")]
        public IList<Offer> GetOfferByCategory(string category)
        {
            _log4net.Info("In offers controller HttpGet GetOfferByCategory and" + category + "is searched");
            return ser.GetOfferByCategory(category);
        }

        [HttpGet]
        [Route("GetOfferByOpenedDate/{openedDate}")]
        public IList<Offer> GetOfferByOpenedDate(DateTime openedDate)
        {
            _log4net.Info("In offers controller HttpGet GetOfferByOpenedDate and" + openedDate + "is searched");
            return ser.GetOfferByOpenedDate(openedDate);

        }

        [HttpGet]
        [Route("GetOfferByTopThreeLikes")]
        public IList<Offer> GetOfferByTopThreeLikes()
        {
            _log4net.Info("In offers controller HttpGet GetOfferByTopThreeLikes is initiated");
            return ser.GetOfferByTopThreeLikes();
        }

        [HttpPost]
        [Route("PostOffer")]
        public ActionResult<Offer> PostOffer(Offer newOffer)
        {
            try
            {


                var post = ser.PostOffer(newOffer);

                _log4net.Info("In offers controller HttpPost PostOffer is initiated");
                if (post.EmployeeId == 0 || post.Category == null || post.Details == null)
                {
                    return NotFound();
                    _log4net.Info("offer not found");
                }
                else
                {
                    return Ok();
                }

            }
            catch (Exception exception)
            {
                _log4net.Error("Exception Found=" + exception.Message);
                return new StatusCodeResult(500);
            }
         }
        [HttpPut]
        [Route("EditOffer")]

        public ActionResult<Offer> EditOffer(Offer updatedOffer)
        {
            try
            {
                var offer = ser.EditOffer(updatedOffer);
                if (offer == null)
                {
                    return NotFound("Offer not found");
                    _log4net.Info("offer not found");
                }

                if (offer.ClosedDate > offer.EngagedDate && offer.Status != "Closed")
                {
                    return BadRequest("Please update status to Closed");
                    _log4net.Info("Please update status to Closed");
                }

                return Ok();
            }
            catch (Exception exception)
            {
                _log4net.Error("Exception Found=" + exception.Message);
                return new StatusCodeResult(500);
            }
        }
    
        [HttpPost]
        [Route("EngageOffer")]
        public ActionResult<IEnumerable<Offer>> EngageOffer(Offer offerDetails)
        {
            try
            {
                var offer = ser.EngageOffer(offerDetails);
                if (offer == null)
                {
                    return NotFound("Offer not found");
                    _log4net.Info("offer not found");
                }
                else if (offer.Status == "Engaged" || offer.Status == "Closed")
                {
                    return BadRequest("Offer is either Engaged or Closed");
                    _log4net.Info("Offer is either Engaged or Closed");
                }
                else
                {
                    return Ok("Offer status updated to Engaged");
                    _log4net.Info("Offer status updated to Engaged");
                }
            }
            catch (Exception exception)
            {
                _log4net.Error("Exception Found=" + exception.Message);
                return new StatusCodeResult(500);
            }
        }
        [HttpPost]
        [Route("LikeOffer/{offerid}")]
        public ActionResult LikeOffer(int offerid)
        {
            try
            {
                var offer = ser.LikeOffer(offerid);
                if (offer == null)
                {
                    return NotFound("Offer not found");
                    _log4net.Info("offer not found");
                }
                else
                {
                    Console.WriteLine(offer.Likes);
                    return Ok();
                }
            }
            catch (Exception exception)
            {
                _log4net.Error("Exception Found=" + exception.Message);
                return new StatusCodeResult(500);
            }
        } 
        [HttpGet]
        [Route("GetLikeData")]
        public ActionResult<List<LikeData>> GetLikeData()
        {
           return  ser.LikeData();
           
        }

    }
}