using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;


namespace DatabaseOpgave
{
    class DBClient
    {
        //Database connection string - replace it with the connnection string to your own database 
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DBOPGAVE;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        #region CRUD Methods Hotel
        private int GetMaxHotelNo(SqlConnection connection)
        {
            Console.WriteLine("Calling -> GetMaxHotelNo");

            //This SQL command will fetch one row from the DemoHotel table: The one with the max HotelID
            string queryStringMaxHotelNo = "SELECT  MAX(HotelID)  FROM Hotel";
            Console.WriteLine($"SQL applied: {queryStringMaxHotelNo}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(queryStringMaxHotelNo, connection);
            SqlDataReader reader = command.ExecuteReader();

            //Assume undefined value 0 for max HotelID
            int MaxHotelID = 0;

            //Is there any rows in the query
            if (reader.Read())
            {
                //Yes, get max HotelID
                MaxHotelID = reader.GetInt32(0); //Reading int fro 1st column
            }

            //Close reader
            reader.Close();

            Console.WriteLine($"Max hotel#: {MaxHotelID}");
            Console.WriteLine();

            //Return max HotelID
            return MaxHotelID;
        }

        private int DeleteHotel(SqlConnection connection, int hotelId)
        {
            Console.WriteLine("Calling -> DeleteHotel");

            //This SQL command will delete one row from the DemoHotel table: The one with primary key HotelID
            string deleteCommandString = $"DELETE FROM Hotel  WHERE HotelID = {hotelId}";
            Console.WriteLine($"SQL applied: {deleteCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(deleteCommandString, connection);
            Console.WriteLine($"Deleting hotel #{hotelId}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            string resetCounter = $"DBCC CHECKIDENT ('Hotel', RESEED, {GetMaxHotelNo(connection)})";
            SqlCommand command2 = new SqlCommand(resetCounter, connection);
            command2.ExecuteNonQuery();
            //Return number of rows affected
            return numberOfRowsAffected;
        }

        private int UpdateHotel(SqlConnection connection, Hotel hotel)
        {
            Console.WriteLine("Calling -> UpdateHotel");

            //This SQL command will update one row from the DemoHotel table: The one with primary key HotelID
            string updateCommandString = $"UPDATE Hotel SET Name='{hotel.Name}', Address='{hotel.Address}' WHERE HotelID = {hotel.HotelID}";
            Console.WriteLine($"SQL applied: {updateCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(updateCommandString, connection);
            Console.WriteLine($"Updating hotel #{hotel.HotelID}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected
            return numberOfRowsAffected;
        }

        private int InsertHotel(SqlConnection connection, Hotel hotel)
        {
            Console.WriteLine("Calling -> InsertHotel");

            //This SQL command will insert one row into the DemoHotel table with primary key HotelID
            string insertCommandString = $"INSERT INTO Hotel VALUES('{hotel.Name}', '{hotel.Address}')";
            Console.WriteLine($"SQL applied: {insertCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(insertCommandString, connection);

            Console.WriteLine($"Creating hotel #{hotel.HotelID}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected 
            return numberOfRowsAffected;
        }

        private List<Hotel> ListAllHotels(SqlConnection connection)
        {
            Console.WriteLine("Calling -> ListAllHotels");

            //This SQL command will fetch all rows and columns from the DemoHotel table
            string queryStringAllHotels = "SELECT * FROM Hotel";
            Console.WriteLine($"SQL applied: {queryStringAllHotels}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(queryStringAllHotels, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Listing all hotels:");

            //NO rows in the query 
            if (!reader.HasRows)
            {
                //End here
                Console.WriteLine("No hotels in database");
                reader.Close();

                //Return null for 'no hotels found'
                return null;
            }

            //Create list of hotels found
            List<Hotel> hotels = new List<Hotel>();
            while (reader.Read())
            {
                //If we reached here, there is still one hotel to be put into the list 
                Hotel nextHotel = new Hotel()
                {
                    HotelID = reader.GetInt32(0), //Reading int from 1st column
                    Name = reader.GetString(1),    //Reading string from 2nd column
                    Address = reader.GetString(2)  //Reading string from 3rd column
                };

                //Add hotel to list
                hotels.Add(nextHotel);

                Console.WriteLine(nextHotel);
            }

            //Close reader
            reader.Close();
            Console.WriteLine();

            //Return list of hotels
            return hotels;
        }

        private Hotel GetHotel(SqlConnection connection, int HotelID)
        {
            Console.WriteLine("Calling -> GetHotel");

            //This SQL command will fetch the row with primary key HotelID from the DemoHotel table
            string queryStringOneHotel = $"SELECT * FROM Hotel WHERE HotelID = {HotelID}";
            Console.WriteLine($"SQL applied: {queryStringOneHotel}");

            //Prepare SQK command
            SqlCommand command = new SqlCommand(queryStringOneHotel, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine($"Finding hotel#: {HotelID}");

            //NO rows in the query? 
            if (!reader.HasRows)
            {
                //End here
                Console.WriteLine("No hotels in database");
                reader.Close();

                //Return null for 'no hotel found'
                return null;
            }

            //Fetch hotel object from teh database
            Hotel hotel = null;
            if (reader.Read())
            {
                hotel = new Hotel()
                {
                    HotelID = reader.GetInt32(0), //Reading int fro 1st column
                    Name = reader.GetString(1),    //Reading string from 2nd column
                    Address = reader.GetString(2)  //Reading string from 3rd column
                };

                Console.WriteLine(hotel);
            }

            //Close reader
            reader.Close();
            Console.WriteLine();

            //Return found hotel
            return hotel;
        }
        #endregion

        #region CRUD Methods Facility
        private int GetMaxFacilityNo(SqlConnection connection)
        {
            Console.WriteLine("Calling -> GetMaxFacilityNo");

            //This SQL command will fetch one row from the DemoFacility table: The one with the max FacilityID
            string queryStringMaxFacilityNo = "SELECT  MAX(FacilityID)  FROM Facility";
            Console.WriteLine($"SQL applied: {queryStringMaxFacilityNo}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(queryStringMaxFacilityNo, connection);
            SqlDataReader reader = command.ExecuteReader();

            //Assume undefined value 0 for max FacilityID
            int MaxFacilityID = 0;

            //Is there any rows in the query
            if (reader.Read())
            {
                //Yes, get max FacilityID
                MaxFacilityID = reader.GetInt32(0); //Reading int fro 1st column
            }

            //Close reader
            reader.Close();

            Console.WriteLine($"Max Facility#: {MaxFacilityID}");
            Console.WriteLine();

            //Return max FacilityID
            return MaxFacilityID;
        }

        private int DeleteFacility(SqlConnection connection, int FacilityId)
        {
            Console.WriteLine("Calling -> DeleteFacility");

            //This SQL command will delete one row from the DemoFacility table: The one with primary key FacilityID
            string deleteCommandString = $"DELETE FROM Facility  WHERE FacilityID = {FacilityId}";
            Console.WriteLine($"SQL applied: {deleteCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(deleteCommandString, connection);
            Console.WriteLine($"Deleting Facility #{FacilityId}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            string resetCounter = $"DBCC CHECKIDENT ('Facility', RESEED, {GetMaxFacilityNo(connection)})";
            SqlCommand command2 = new SqlCommand(resetCounter, connection);
            command2.ExecuteNonQuery();
            //Return number of rows affected
            return numberOfRowsAffected;
        }

        private int UpdateFacility(SqlConnection connection, Facility Facility)
        {
            Console.WriteLine("Calling -> UpdateFacility");

            //This SQL command will update one row from the DemoFacility table: The one with primary key FacilityID
            string updateCommandString = $"UPDATE Facility SET Name='{Facility.Name}', Type='{Facility.Type}' WHERE FacilityID = {Facility.FacilityID}";
            Console.WriteLine($"SQL applied: {updateCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(updateCommandString, connection);
            Console.WriteLine($"Updating Facility #{Facility.FacilityID}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected
            return numberOfRowsAffected;
        }

        private int InsertFacility(SqlConnection connection, Facility Facility)
        {
            Console.WriteLine("Calling -> InsertFacility");

            //This SQL command will insert one row into the DemoFacility table with primary key FacilityID
            string insertCommandString = $"INSERT INTO Facility VALUES('{Facility.Name}', '{Facility.Type}')";
            Console.WriteLine($"SQL applied: {insertCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(insertCommandString, connection);

            Console.WriteLine($"Creating Facility #{Facility.FacilityID}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected 
            return numberOfRowsAffected;
        }

        private List<Facility> ListAllFacilitys(SqlConnection connection)
        {
            Console.WriteLine("Calling -> ListAllFacilitys");

            //This SQL command will fetch all rows and columns from the DemoFacility table
            string queryStringAllFacilitys = "SELECT * FROM Facility";
            Console.WriteLine($"SQL applied: {queryStringAllFacilitys}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(queryStringAllFacilitys, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Listing all Facilitys:");

            //NO rows in the query 
            if (!reader.HasRows)
            {
                //End here
                Console.WriteLine("No Facilitys in database");
                reader.Close();

                //Return null for 'no Facilitys found'
                return null;
            }

            //Create list of Facilitys found
            List<Facility> Facilitys = new List<Facility>();
            while (reader.Read())
            {
                //If we reached here, there is still one Facility to be put into the list 
                Facility nextFacility = new Facility()
                {
                    FacilityID = reader.GetInt32(0), //Reading int from 1st column
                    Name = reader.GetString(1),    //Reading string from 2nd column
                    Type = reader.GetString(2)  //Reading string from 3rd column
                };

                //Add Facility to list
                Facilitys.Add(nextFacility);

                Console.WriteLine(nextFacility);
            }

            //Close reader
            reader.Close();
            Console.WriteLine();

            //Return list of Facilitys
            return Facilitys;
        }

        private Facility GetFacility(SqlConnection connection, int FacilityID)
        {
            Console.WriteLine("Calling -> GetFacility");

            //This SQL command will fetch the row with primary key FacilityID from the DemoFacility table
            string queryStringOneFacility = $"SELECT * FROM Facility WHERE FacilityID = {FacilityID}";
            Console.WriteLine($"SQL applied: {queryStringOneFacility}");

            //Prepare SQK command
            SqlCommand command = new SqlCommand(queryStringOneFacility, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine($"Finding Facility#: {FacilityID}");

            //NO rows in the query? 
            if (!reader.HasRows)
            {
                //End here
                Console.WriteLine("No Facilitys in database");
                reader.Close();

                //Return null for 'no Facility found'
                return null;
            }

            //Fetch Facility object from teh database
            Facility Facility = null;
            if (reader.Read())
            {
                Facility = new Facility()
                {
                    FacilityID = reader.GetInt32(0), //Reading int fro 1st column
                    Name = reader.GetString(1),    //Reading string from 2nd column
                    Type = reader.GetString(2)  //Reading string from 3rd column
                };

                Console.WriteLine(Facility);
            }

            //Close reader
            reader.Close();
            Console.WriteLine();

            //Return found Facility
            return Facility;
        }
        #endregion

        public void Start()
        {
            //Apply 'using' to connection (SqlConnection) in order to call Dispose (interface IDisposable) 
            //whenever the 'using' block exits
            #region Hotel CRUD
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Open connection
                connection.Open();

                //List all hotels in the database
                ListAllHotels(connection);

                //Create a new hotel with primary key equal to current max primary key plus 1
                Hotel newHotel = new Hotel()
                {
                    //HotelID = GetMaxHotelNo(connection) + 1,
                    Name = "New Hotel",
                    Address = "Maglegaardsvej 2, 4000 Roskilde"
                };

                //Insert the hotel into the database
                InsertHotel(connection, newHotel);

                //List all hotels including the the newly inserted one
                ListAllHotels(connection);

                //Get the newly inserted hotel from the database in order to update it 
                Hotel hotelToBeUpdated = GetHotel(connection, GetMaxHotelNo(connection));

                //Alter Name and Addess properties
                hotelToBeUpdated.Name += "(updated)";
                hotelToBeUpdated.Address += "(updated)";

                //Update the hotel in the database 
                UpdateHotel(connection, hotelToBeUpdated);

                //List all hotels including the updated one
                ListAllHotels(connection);

                //Get the updated hotel in order to delete it
                Hotel hotelToBeDeleted = GetHotel(connection, hotelToBeUpdated.HotelID);

                //Delete the hotel
                DeleteHotel(connection, hotelToBeDeleted.HotelID);

                //List all hotels - now without the deleted one
                ListAllHotels(connection);

            }
            #endregion

            #region Facility CRUD
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Open connection
                connection.Open();

                //List all Facilitys in the database
                ListAllFacilitys(connection);

                //Create a new Facility with primary key equal to current max primary key plus 1
                Facility newFacility = new Facility()
                {
                    //FacilityID = GetMaxFacilityNo(connection) + 1,
                    Name = "Bowling",
                    Type = "Plessure"
                };

                //Insert the Facility into the database
                InsertFacility(connection, newFacility);

                //List all Facilitys including the the newly inserted one
                ListAllFacilitys(connection);

                //Get the newly inserted Facility from the database in order to update it 
                Facility FacilityToBeUpdated = GetFacility(connection, GetMaxFacilityNo(connection));

                //Alter Name and Addess properties
                FacilityToBeUpdated.Name += "(updated)";
                FacilityToBeUpdated.Type += "(updated)";

                //Update the Facility in the database 
                UpdateFacility(connection, FacilityToBeUpdated);

                //List all Facilitys including the updated one
                ListAllFacilitys(connection);

                //Get the updated Facility in order to delete it
                Facility FacilityToBeDeleted = GetFacility(connection, FacilityToBeUpdated.FacilityID);

                //Delete the Facility
                DeleteFacility(connection, FacilityToBeDeleted.FacilityID);

                //List all Facilitys - now without the deleted one
                ListAllFacilitys(connection);

            }
            #endregion
        }
    }
}
