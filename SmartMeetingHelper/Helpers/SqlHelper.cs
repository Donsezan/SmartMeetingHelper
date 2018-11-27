using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using SmartMeetingHelper.Models;

namespace SmartMeetingHelper.Helpers
{
    public class SqlHelper
    {
       

        public void CreateBase()
        {
            SQLiteConnection.CreateFile(Settings.DbName);
            using (SQLiteConnection connect = new SQLiteConnection("Data Source=" + Settings.DbName))
            {
                connect.Open();

                using (SQLiteCommand command = new SQLiteCommand(connect))
                {
                    command.CommandText = $@"CREATE TABLE [{Settings.TableName}] (
                    [Id] char(100) PRIMARY KEY NOT NULL,
                    [Name] char(100) NOT NULL,
                    [PhotoId] char(100) NOT NULL,
                    [Email] char(100) NOT NULL,
                    [LastVisit] char(100) NOT NULL
                    );";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connect.Close();
            }
        }

        public bool AddUserToDb(UserModel userModel)
        {
            using (SQLiteConnection connect = new SQLiteConnection("Data Source=" + Settings.DbName))
            {
                connect.Open();
                var sql =
                    $"insert into Users (Id, Name, PhotoId, Email, lastVisit) values ('{userModel.Id}', '{userModel.Name}', '{userModel.PhotoId}', '{userModel.Email}', '{userModel.LastVisit}')";
                var command = new SQLiteCommand(sql, connect);
                command.ExecuteNonQuery();
                connect.Close();
            }
            return true;
        }

        public UserModel FoundInDbModel(string id)
        {
            var userModel = new UserModel();
            using (SQLiteConnection connect = new SQLiteConnection("Data Source=" + Settings.DbName))
            {
                connect.Open();
                using (SQLiteCommand fmd = connect.CreateCommand())
                {
                    fmd.CommandText = $@"SELECT * FROM Users WHERE Id = '{id}';";
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        userModel.Id = Convert.ToString(r["Id"]);
                        userModel.Name = Convert.ToString(r["Name"]);
                        userModel.PhotoId = Convert.ToString(r["PhotoId"]);
                        userModel.Email = Convert.ToString(r["Email"]);
                        userModel.LastVisit = Convert.ToString(r["LastVisit"]);
                    }
                }
                connect.Close();
                return userModel;
            }
        }

        public List<UserModel> GetAllDataFromDb()
        {
            var usersList = new List<UserModel>();
            
            using (SQLiteConnection connect = new SQLiteConnection("Data Source=" + Settings.DbName))
            {
                connect.Open();
                using (SQLiteCommand fmd = connect.CreateCommand())
                {
                    fmd.CommandText = $@"select * from Users;";
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataReader r = fmd.ExecuteReader();

                    while (r.Read())
                    {
                        var userModel = new UserModel
                        {
                            Id = Convert.ToString(r["Id"]),
                            Name = Convert.ToString(r["Name"]),
                            PhotoId = Convert.ToString(r["PhotoId"]),
                            Email = Convert.ToString(r["Email"]),
                            LastVisit = Convert.ToString(r["LastVisit"])
                        };
                        usersList.Add(userModel);
                    }
                }
                connect.Close();
                return usersList;
            }
        }
    }
}