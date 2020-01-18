using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Locations.Models
{
    public class MapsModel
    {
        public string KeyString { get; set; }
        public string SearchString { get; set; }
        public decimal CenterLat { get; set; }
        public decimal CenterLong { get; set; }
        public List<LocationModel> Locations { get; set; }
    }

    public class LocationModel
    {
        public string KeyString { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string Contact { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Notes { get; set; }
        public int Type { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public int Petitions { get; set; }
        public int Flyers { get; set; }
        public int Posters { get; set; }
        public DateTime LastDropoffDateTime { get; set; }
        public DateTime LastOutOfStockDateTime { get; set; }
        public DateTime LastPickUpDateTime { get; set; }
        public int AllTimeOutOfStock { get; set; }
        public bool Unsupportive { get; set; }
        public bool VolunteerInterest { get; set; }
    }

    public class LocationsDA
    {
        //INSERT**
        //        using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["myDbConnection"].ConnectionString))
        //{
        //    string insertQuery = @"INSERT INTO [dbo].[Customer]([FirstName], [LastName], [State], [City], [IsActive], [CreatedOn]) VALUES (@FirstName, @LastName, @State, @City, @IsActive, @CreatedOn)";

        //    var result = db.Execute(insertQuery, new
        //    {
        //        customerModel.FirstName,
        //        customerModel.LastName,
        //        StateModel.State,
        //        CityModel.City,
        //        isActive,
        //        CreatedOn = DateTime.Now
        //    });
        //}

        //(if modekl has same field names???)
        //string insertQuery = @"INSERT INTO [dbo].[Customer]([FirstName], [LastName], [State], [City], [IsActive], [CreatedOn]) VALUES (@FirstName, @LastName, @State, @City, @IsActive, @CreatedOn)";
        //var result = db.Execute(insertQuery, customerViewModel);


        //SELECT**
        //   using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["myDbConnection"].ConnectionString))
        //{
        //    string selectQuery = @"SELECT * FROM [dbo].[Customer] WHERE FirstName = @FirstName";

        //    var result = db.Query(selectQuery, new
        //    {
        //        customerModel.FirstName
        //    });
        //}


        //SELECT and convert to obj or dynamic object**
        //var data = sql.Query<MyClass>(SELECT * FROM MyClassTable");


        //UPDATE**
        //using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["myDbConnection"].ConnectionString))
        //{
        //    string updateQuery = @"UPDATE [dbo].[Customer] SET IsActive = @IsActive WHERE FirstName = @FirstName AND LastName = @LastName";

        //    var result = db.Execute(updateQuery, new
        //    {
        //        isActive,
        //        customerModel.FirstName,
        //        customerModel.LastName
        //    });
        //}

        ////DELETE**
        //Code for CRUD
        //using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["myDbConnection"].ConnectionString))
        //{
        //string deleteQuery = @"DELETE FROM [dbo].[Customer] WHERE FirstName = @FirstName AND LastName = @LastName";

        //    var result = db.Execute(deleteQuery, new
        //    {
        //        customerModel.FirstName,
        //        customerModel.LastName
        //    });
        //}

        ///...................SSRS Report View/Delivery Dest & Sched.++;

    }
}
