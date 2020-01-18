using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Locations.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using GoogleMaps.LocationServices;
using Dapper;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;

namespace Locations.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _configuration;
        private new readonly GoogleLocationService _locationService;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _locationService = new GoogleLocationService(_configuration["MapsAPIKey"].ToString());
        }

        public IActionResult Index()
        {
            return Map(new MapsModel());
        }

        public IActionResult Map(MapsModel model)
        {
            if (model.KeyString == null)
            {
                model = GetDefaultMapView();
            }
            return View("Map", model);
        }

        public MapsModel GetDefaultMapView()
        {
            MapsModel model = new MapsModel() { Locations = GetAllActiveLocations() };
            model.KeyString = _configuration["MapsAPIKey"].ToString();
            model.CenterLat = 43.0819M;
            model.CenterLong = -88.1712M;

            return model;
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [HttpPost]
        public IActionResult GetMap(MapsModel mapsModel)
        {
            try
            {
                string address = mapsModel.SearchString.Replace(" ", "+");
                MapPoint coords = _locationService.GetLatLongFromAddress(address);
                mapsModel = GetDefaultMapView();
                mapsModel.CenterLat = (decimal)coords.Latitude;
                mapsModel.CenterLong = (decimal)coords.Longitude;          
            }
            catch (Exception)
            { }

            return Map(mapsModel);
        }

        [HttpPost]
        public IActionResult UpdateLocation(LocationModel locationModel)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                db.Open();
                string sql = @"UPDATE [Locations].[dbo].[Locations] SET Name='" + locationModel.Name + "', Contact='" + locationModel.Contact + "', Email='" + locationModel.Email + "', Website='" + locationModel.Website + "', Phone='" + locationModel.Phone + "', StreetAddress1='" + locationModel.StreetAddress1 + "', StreetAddress2='" + locationModel.StreetAddress2 + "', City='" + locationModel.City + "'"
                    + ", [State]='" + locationModel.State + "', Zip='" + locationModel.Zip + "', LocationContact=-1, PrimaryContact=-1, Notes='" + locationModel.Notes + "', Petitions=" + locationModel.Petitions + ", Flyers=" + locationModel.Flyers + ", Posters=" + locationModel.Posters + ", LastPickUpDateTime='" + locationModel.LastPickUpDateTime + "', LastDropoffDateTime='" + locationModel.LastDropoffDateTime + "', LastOutOfStockDateTime='" + locationModel.LastOutOfStockDateTime + "', AllTimeOutofStock=" + locationModel.AllTimeOutOfStock + ", Unsupportive=" + Convert.ToInt16(locationModel.Unsupportive) + ", VolunteerInterest=" + Convert.ToInt16(locationModel.VolunteerInterest)
                    + " WHERE ID = " + locationModel.Id + " AND [Type] = 1;";
                db.Execute(sql);
            }
            var model = GetDefaultMapView();
            return Map(new MapsModel());
        }

        [HttpPost]
        public IActionResult AddLocation(LocationModel location)
        {
            string address = location.StreetAddress1.Replace(" ", "+") + "," + location.City.Replace(" ", "+") + "," + location.State.Replace(" ", "+");
            MapPoint coords = _locationService.GetLatLongFromAddress(address);
            location.Lat = (decimal)coords.Latitude;
            location.Long = (decimal)coords.Longitude;
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                db.Open();
                string sql = @"INSERT INTO [Locations].[dbo].[Locations] ([Name], [Contact], [Email], [Website], [Phone], [StreetAddress1], [StreetAddress2], [City]"
                    + ",[State], [Zip], [LocationContact], [PrimaryContact], [Notes], [Type], [Lat], [Long], [Petitions], [Flyers], [Posters], [LastPickUpDateTime], [LastOutOfStockDateTime], LastDropoffDateTime"
                    + ",[AllTimeOutofStock],[Unsupportive],[VolunteerInterest])"
                    + " VALUES ('" + location.Name + "','" + location.Contact + "','" + location.Email + "','" + location.Website + "','" + location.Phone + "','" + location.StreetAddress1 + "','" + location.StreetAddress1 + "','" + location.City + "'"
                    + ",'" + location.State + "','" + location.Zip + "', -1, -1,'" + location.Notes + "', 1, " + location.Lat + "," + location.Long + "," + location.Petitions + "," + location.Flyers + "," + location.Posters + ",'" + location.LastPickUpDateTime + "','" + location.LastOutOfStockDateTime + "','" + location.LastDropoffDateTime + "', 0, 0, 1) " + ";";
                db.Execute(sql);
            }
            var model = GetDefaultMapView();
            model.KeyString = _configuration["MapsAPIKey"].ToString();
            return View("Map", model);
        }

        public IActionResult GetAddNewModal()
        {
            return PartialView("_AddNewLocation", new LocationModel());
        }

        public List<LocationModel> GetAllActiveLocations() //TODO: refactor a bit for brevity
        {
            List<LocationModel> locations = new List<LocationModel>();
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                db.Open();
                string insertQuery = @"SELECT ID, Name, Contact, Email, Website, Phone, Contact, StreetAddress1, StreetAddress2,
                         City, State, Zip, LocationContact, PrimaryContact, Notes, Type, Lat, Long, Petitions, Flyers, Posters, LastDropoffDateTime, 
                         LastPickUpDateTime, LastOutOfStockDateTime, AllTimeOutofStock, Unsupportive, VolunteerInterest
                         FROM Locations WHERE (Type = 1)";
                var rdr = db.ExecuteReader(insertQuery);
                while (rdr.Read())
                    locations.Add(new LocationModel {
                        Id = (int)rdr["ID"],
                        Name = rdr["Name"].ToString(),
                        Contact = rdr["Contact"].ToString(),
                        Email = rdr["Email"].ToString(),
                        Website = rdr["Website"].ToString(),
                        Phone = rdr["Phone"].ToString(),
                        Flyers = (int)rdr["Flyers"],
                        Posters = (int)rdr["Posters"],
                        Petitions = (int)rdr["Petitions"],
                        StreetAddress1 = rdr["StreetAddress1"].ToString(),
                        StreetAddress2 = rdr["StreetAddress2"].ToString(),
                        City = rdr["City"].ToString(),
                        State = rdr["State"].ToString(),
                        Zip = rdr["Zip"].ToString(),
                        Notes = rdr["Notes"].ToString(),
                        Lat = (decimal)rdr["Lat"],
                        Long = (decimal)rdr["Long"],
                        LastDropoffDateTime = rdr["LastDropOffDateTime"] != DBNull.Value ? Convert.ToDateTime(rdr["LastDropOffDateTime"]) : DateTime.Now,
                        LastPickUpDateTime = rdr["LastPickUpDateTime"] != DBNull.Value ? Convert.ToDateTime(rdr["LastPickUpDateTime"]) : DateTime.Now,
                        LastOutOfStockDateTime = rdr["LastOutOfStockDateTime"] != DBNull.Value ? Convert.ToDateTime(rdr["LastOutOfStockDateTime"]) : DateTime.Now,
                        AllTimeOutOfStock = (int)rdr["AllTimeOutOfStock"]
                    });
            }
            return locations;
        }

        public List<string> GetLocationByLocationType(int type)
        {
            return null;
        }

        [HttpGet]
        public IActionResult GetLocationModalInfo(LocationModel location)
        {
            return PartialView("_LocationModalInfo", location);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
