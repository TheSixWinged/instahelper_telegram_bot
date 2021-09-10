using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace InstaHelper.DataBase
{
    public class MySqlWorker : IDataBaseWorker
    {
        private static MySqlWorker instance;
        protected MySqlWorker() { }
        public static MySqlWorker getInstance()
        {
            if (instance == null)
            {
                instance = new MySqlWorker();
            }
            return instance;
        }

        public string GetState(long id)
        {
            string state = null;

            using (MySqlConnection mySqlConnection = new MySqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                mySqlConnection.Open();

                MySqlDataReader readerDB = null;
                MySqlCommand searchUser_MySqlCommand = new MySqlCommand($"SELECT * FROM UserState WHERE UserID = {id}", mySqlConnection);
                readerDB = searchUser_MySqlCommand.ExecuteReader();

                if (readerDB.HasRows)
                {
                    while (readerDB.Read())
                    {
                        state = Convert.ToString(readerDB["State"]);
                    }
                    readerDB.Close();
                }
                else
                {
                    readerDB.Close();
                    state = "start";
                    MySqlCommand insertNewUser_MySqlCommand = new MySqlCommand($"INSERT INTO UserState (UserID) VALUES ('{id}')", mySqlConnection);
                    insertNewUser_MySqlCommand.ExecuteNonQuery();
                }
            }

            return state;
        }

        public void ChangeState(long id, string state)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                mySqlConnection.Open();

                MySqlCommand changeUserState_MySqlCommand = new MySqlCommand($"UPDATE UserState SET State='{state}' WHERE UserID = {id}", mySqlConnection);
                changeUserState_MySqlCommand.ExecuteNonQuery();
            }
        }

        public void InputParser(long id, string input)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                mySqlConnection.Open();

                MySqlDataReader readerDB = null;
                MySqlCommand searchParserInput_MySqlCommand = new MySqlCommand($"SELECT * FROM UserParserInput WHERE UserID = {id}", mySqlConnection);
                readerDB = searchParserInput_MySqlCommand.ExecuteReader();

                if (readerDB.HasRows)
                {
                    readerDB.Close();
                    MySqlCommand changeParserInput_MySqlCommand = new MySqlCommand($"UPDATE UserParserInput SET InputUsername='{input}' WHERE UserID = {id}", mySqlConnection);
                    changeParserInput_MySqlCommand.ExecuteNonQuery();
                }
                else
                {
                    readerDB.Close();
                    MySqlCommand insertNewParserInput_MySqlCommand = new MySqlCommand($"INSERT INTO UserParserInput (UserID, InputUsername) VALUES ('{id}', '{input}')", mySqlConnection);
                    insertNewParserInput_MySqlCommand.ExecuteNonQuery();
                }
            }
        }

        public string GetParser(long id)
        {
            string username = null;

            using (MySqlConnection mySqlConnection = new MySqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                mySqlConnection.Open();

                MySqlDataReader readerDB = null;
                MySqlCommand searchParserInput_MySqlCommand = new MySqlCommand($"SELECT * FROM UserParserInput WHERE UserID = {id}", mySqlConnection);
                readerDB = searchParserInput_MySqlCommand.ExecuteReader();

                if (readerDB.HasRows)
                {
                    while (readerDB.Read())
                    {
                        username = Convert.ToString(readerDB["InputUsername"]);
                    }
                    readerDB.Close();
                }
                else
                {
                    username = "";
                    readerDB.Close();
                }
            }

            return username;
        }

        public void SetCompetUnique(long id, bool set)
        {
            int setter = Convert.ToInt32(set);

            using (MySqlConnection mySqlConnection = new MySqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                mySqlConnection.Open();

                MySqlDataReader readerDB = null;
                MySqlCommand searchCompetUnique_MySqlCommand = new MySqlCommand($"SELECT * FROM UserCompetInput WHERE UserID = {id}", mySqlConnection);
                readerDB = searchCompetUnique_MySqlCommand.ExecuteReader();

                if (readerDB.HasRows)
                {
                    readerDB.Close();
                    MySqlCommand changeCompetUnique_MySqlCommand = new MySqlCommand($"UPDATE UserCompetInput SET UniqueSet='{setter}' WHERE UserID = {id}", mySqlConnection);
                    changeCompetUnique_MySqlCommand.ExecuteNonQuery();
                }
                else
                {
                    readerDB.Close();
                    MySqlCommand insertNewCompetUnique_MySqlCommand = new MySqlCommand($"INSERT INTO UserCompetInput (UserID, UniqueSet) VALUES ('{id}', '{setter}')", mySqlConnection);
                    insertNewCompetUnique_MySqlCommand.ExecuteNonQuery();
                }
            }
        }

        public bool GetCompetUnique(long id)
        {
            bool unique = false;

            using (MySqlConnection mySqlConnection = new MySqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                mySqlConnection.Open();

                MySqlDataReader readerDB = null;
                MySqlCommand searchCompetUnique_MySqlCommand = new MySqlCommand($"SELECT * FROM UserCompetInput WHERE UserID = {id}", mySqlConnection);
                readerDB = searchCompetUnique_MySqlCommand.ExecuteReader();

                if (readerDB.HasRows)
                {
                    while (readerDB.Read())
                    {
                        unique = Convert.ToBoolean(readerDB["UniqueSet"]);
                    }
                    readerDB.Close();
                }
                else
                {
                    readerDB.Close();
                }
            }

            return unique;
        }

        public void SetCompetAuth(long id, bool set)
        {
            int setter = Convert.ToInt32(set);

            using (MySqlConnection mySqlConnection = new MySqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                mySqlConnection.Open();

                MySqlDataReader readerDB = null;
                MySqlCommand searchCompetAuth_MySqlCommand = new MySqlCommand($"SELECT * FROM UserCompetInput WHERE UserID = {id}", mySqlConnection);
                readerDB = searchCompetAuth_MySqlCommand.ExecuteReader();

                if (readerDB.HasRows)
                {
                    readerDB.Close();
                    MySqlCommand changeCompetAuth_MySqlCommand = new MySqlCommand($"UPDATE UserCompetInput SET AuthorizationSet='{setter}' WHERE UserID = {id}", mySqlConnection);
                    changeCompetAuth_MySqlCommand.ExecuteNonQuery();
                }
                else
                {
                    readerDB.Close();
                    MySqlCommand insertNewCompetAuth_MySqlCommand = new MySqlCommand($"INSERT INTO UserCompetInput (UserID, AuthorizationSet) VALUES ('{id}', '{setter}')", mySqlConnection);
                    insertNewCompetAuth_MySqlCommand.ExecuteNonQuery();
                }
            }
        }

        public bool GetCompetAuth(long id)
        {
            bool auth = false;

            using (MySqlConnection mySqlConnection = new MySqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                mySqlConnection.Open();

                MySqlDataReader readerDB = null;
                MySqlCommand searchCompetAuth_MySqlCommand = new MySqlCommand($"SELECT * FROM UserCompetInput WHERE UserID = {id}", mySqlConnection);
                readerDB = searchCompetAuth_MySqlCommand.ExecuteReader();

                if (readerDB.HasRows)
                {
                    while (readerDB.Read())
                    {
                        auth = Convert.ToBoolean(readerDB["AuthorizationSet"]);
                    }
                    readerDB.Close();
                }
                else
                {
                    readerDB.Close();
                }
            }

            return auth;
        }

        public void InputCompetUserLogin(long id, string login)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                mySqlConnection.Open();

                MySqlDataReader readerDB = null;
                MySqlCommand searchCompetUserLogin_MySqlCommand = new MySqlCommand($"SELECT * FROM UserCompetInput WHERE UserID = {id}", mySqlConnection);
                readerDB = searchCompetUserLogin_MySqlCommand.ExecuteReader();

                if (readerDB.HasRows)
                {
                    readerDB.Close();
                    MySqlCommand changeCompetUserLogin_MySqlCommand = new MySqlCommand($"UPDATE UserCompetInput SET UserLogin='{login}' WHERE UserID = {id}", mySqlConnection);
                    changeCompetUserLogin_MySqlCommand.ExecuteNonQuery();
                }
                else
                {
                    readerDB.Close();
                    MySqlCommand insertNewCompetUserLogin_MySqlCommand = new MySqlCommand($"INSERT INTO UserCompetInput (UserID, UserLogin) VALUES ('{id}', '{login}')", mySqlConnection);
                    insertNewCompetUserLogin_MySqlCommand.ExecuteNonQuery();
                }
            }
        }

        public string GetCompetUserLogin(long id)
        {
            string login = null;

            using (MySqlConnection mySqlConnection = new MySqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                mySqlConnection.Open();

                MySqlDataReader readerDB = null;
                MySqlCommand searchCompetUserLogin_MySqlCommand = new MySqlCommand($"SELECT * FROM UserCompetInput WHERE UserID = {id}", mySqlConnection);
                readerDB = searchCompetUserLogin_MySqlCommand.ExecuteReader();

                if (readerDB.HasRows)
                {
                    while (readerDB.Read())
                    {
                        login = Convert.ToString(readerDB["UserLogin"]);
                    }
                    readerDB.Close();
                }
                else
                {
                    login = "";
                    readerDB.Close();
                }
            }

            return login;
        }

        public void InputCompetPostlink(long id, string postlink)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                mySqlConnection.Open();

                MySqlDataReader readerDB = null;
                MySqlCommand searchCompetPostlink_MySqlCommand = new MySqlCommand($"SELECT * FROM UserCompetInput WHERE UserID = {id}", mySqlConnection);
                readerDB = searchCompetPostlink_MySqlCommand.ExecuteReader();

                if (readerDB.HasRows)
                {
                    readerDB.Close();
                    MySqlCommand changeCompetPostlink_MySqlCommand = new MySqlCommand($"UPDATE UserCompetInput SET Postlink='{postlink}' WHERE UserID = {id}", mySqlConnection);
                    changeCompetPostlink_MySqlCommand.ExecuteNonQuery();
                }
                else
                {
                    readerDB.Close();
                    MySqlCommand insertNewCompetPostlink_MySqlCommand = new MySqlCommand($"INSERT INTO UserCompetInput (UserID, Postlink) VALUES ('{id}', '{postlink}')", mySqlConnection);
                    insertNewCompetPostlink_MySqlCommand.ExecuteNonQuery();
                }
            }
        }

        public string GetCompetPostlink(long id)
        {
            string postlink = null;

            using (MySqlConnection mySqlConnection = new MySqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                mySqlConnection.Open();

                MySqlDataReader readerDB = null;
                MySqlCommand searchCompetPostlink_MySqlCommand = new MySqlCommand($"SELECT * FROM UserCompetInput WHERE UserID = {id}", mySqlConnection);
                readerDB = searchCompetPostlink_MySqlCommand.ExecuteReader();

                if (readerDB.HasRows)
                {
                    while (readerDB.Read())
                    {
                        postlink = Convert.ToString(readerDB["Postlink"]);
                    }
                    readerDB.Close();
                }
                else
                {
                    postlink = "";
                    readerDB.Close();
                }
            }

            return postlink;
        }  
    }
}
