﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Roommates.Models;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT rm.FirstName, r.Name, rm.RentPortion " +
                        "FROM Roommate rm " +
                        "LEFT JOIN Room r ON r.Id = rm.RoomId " +
                        "WHERE rm.Id = @id ";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;
                    

                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            //LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            //MovedInDate = reader.GetDateTime(reader.GetOrdinal("MovedInDate")),
                            Room = new Room
                            {
                               Name = reader.GetString(reader.GetOrdinal("Name"))
                            }
                        };
                    }
                    reader.Close();
                    return roommate;
                }
            }
        
        }
    }
}