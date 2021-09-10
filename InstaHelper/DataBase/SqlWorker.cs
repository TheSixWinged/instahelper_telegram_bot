using System;
using System.Configuration;
using System.Data.SqlClient;

namespace InstaHelper.DataBase
{
    public class SqlWorker : IDataBaseWorker
    {
        private static SqlWorker instance;
        protected SqlWorker() { }
        public static SqlWorker getInstance()
        {
            if(instance == null)
            {
                instance = new SqlWorker();
            }
            return instance;
        }

        public string GetState(long id)
        {
            string state = null;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                sqlConnection.Open();

                SqlDataReader readerDB = null;
                SqlCommand searchUser_SqlCommand = new SqlCommand($"SELECT * FROM [UserState] WHERE UserID = {id}", sqlConnection);
                readerDB = searchUser_SqlCommand.ExecuteReader();

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
                    SqlCommand insertNewUser_SqlCommand = new SqlCommand($"INSERT INTO [UserState] (UserID) VALUES ('{id}')", sqlConnection);
                    insertNewUser_SqlCommand.ExecuteNonQuery();
                }
            }

            return state;
        }

        public void ChangeState(long id, string state)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                sqlConnection.Open();

                SqlCommand changeUserState_SqlCommand = new SqlCommand($"UPDATE [UserState] SET State='{state}' WHERE UserID = {id}", sqlConnection);
                changeUserState_SqlCommand.ExecuteNonQuery();
            }
        }

        public void InputParser(long id, string input)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                sqlConnection.Open();

                SqlDataReader readerDB = null;
                SqlCommand searchParserInput_SqlCommand = new SqlCommand($"SELECT * FROM [UserParserInput] WHERE UserID = {id}", sqlConnection);
                readerDB = searchParserInput_SqlCommand.ExecuteReader();

                if (readerDB.HasRows)
                {
                    readerDB.Close();
                    SqlCommand changeParserInput_SqlCommand = new SqlCommand($"UPDATE [UserParserInput] SET InputUsername='{input}' WHERE UserID = {id}", sqlConnection);
                    changeParserInput_SqlCommand.ExecuteNonQuery();
                }
                else
                {
                    readerDB.Close();
                    SqlCommand insertNewParserInput_SqlCommand = new SqlCommand($"INSERT INTO [UserParserInput] (UserID, InputUsername) VALUES ('{id}', '{input}')", sqlConnection);
                    insertNewParserInput_SqlCommand.ExecuteNonQuery();
                }
            }
        }

        public string GetParser(long id)
        {
            string username = null;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                sqlConnection.Open();

                SqlDataReader readerDB = null;
                SqlCommand searchParserInput_SqlCommand = new SqlCommand($"SELECT * FROM [UserParserInput] WHERE UserID = {id}", sqlConnection);
                readerDB = searchParserInput_SqlCommand.ExecuteReader();

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
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                sqlConnection.Open();

                SqlDataReader readerDB = null;
                SqlCommand searchCompetUnique_SqlCommand = new SqlCommand($"SELECT * FROM [UserCompetInput] WHERE UserID = {id}", sqlConnection);
                readerDB = searchCompetUnique_SqlCommand.ExecuteReader();

                if (readerDB.HasRows)
                {
                    readerDB.Close();
                    SqlCommand changeCompetUnique_SqlCommand = new SqlCommand($"UPDATE [UserCompetInput] SET UniqueSet='{set}' WHERE UserID = {id}", sqlConnection);
                    changeCompetUnique_SqlCommand.ExecuteNonQuery();
                }
                else
                {
                    readerDB.Close();
                    SqlCommand insertNewCompetUnique_SqlCommand = new SqlCommand($"INSERT INTO [UserCompetInput] (UserID, UniqueSet) VALUES ('{id}', '{set}')", sqlConnection);
                    insertNewCompetUnique_SqlCommand.ExecuteNonQuery();
                }
            }
        }

        public bool GetCompetUnique(long id)
        {
            bool unique = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                sqlConnection.Open();

                SqlDataReader readerDB = null;
                SqlCommand searchCompetUnique_SqlCommand = new SqlCommand($"SELECT * FROM [UserCompetInput] WHERE UserID = {id}", sqlConnection);
                readerDB = searchCompetUnique_SqlCommand.ExecuteReader();

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
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                sqlConnection.Open();

                SqlDataReader readerDB = null;
                SqlCommand searchCompetAuth_SqlCommand = new SqlCommand($"SELECT * FROM [UserCompetInput] WHERE UserID = {id}", sqlConnection);
                readerDB = searchCompetAuth_SqlCommand.ExecuteReader();

                if (readerDB.HasRows)
                {
                    readerDB.Close();
                    SqlCommand changeCompetAuth_SqlCommand = new SqlCommand($"UPDATE [UserCompetInput] SET AuthorizationSet='{set}' WHERE UserID = {id}", sqlConnection);
                    changeCompetAuth_SqlCommand.ExecuteNonQuery();
                }
                else
                {
                    readerDB.Close();
                    SqlCommand insertNewCompetAuth_SqlCommand = new SqlCommand($"INSERT INTO [UserCompetInput] (UserID, AuthorizationSet) VALUES ('{id}', '{set}')", sqlConnection);
                    insertNewCompetAuth_SqlCommand.ExecuteNonQuery();
                }
            }
        }

        public bool GetCompetAuth(long id)
        {
            bool auth = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                sqlConnection.Open();

                SqlDataReader readerDB = null;
                SqlCommand searchCompetAuth_SqlCommand = new SqlCommand($"SELECT * FROM [UserCompetInput] WHERE UserID = {id}", sqlConnection);
                readerDB = searchCompetAuth_SqlCommand.ExecuteReader();

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
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                sqlConnection.Open();

                SqlDataReader readerDB = null;
                SqlCommand searchCompetUserLogin_SqlCommand = new SqlCommand($"SELECT * FROM [UserCompetInput] WHERE UserID = {id}", sqlConnection);
                readerDB = searchCompetUserLogin_SqlCommand.ExecuteReader();

                if (readerDB.HasRows)
                {
                    readerDB.Close();
                    SqlCommand changeCompetUserLogin_SqlCommand = new SqlCommand($"UPDATE [UserCompetInput] SET UserLogin='{login}' WHERE UserID = {id}", sqlConnection);
                    changeCompetUserLogin_SqlCommand.ExecuteNonQuery();
                }
                else
                {
                    readerDB.Close();
                    SqlCommand insertNewCompetUserLogin_SqlCommand = new SqlCommand($"INSERT INTO [UserCompetInput] (UserID, UserLogin) VALUES ('{id}', '{login}')", sqlConnection);
                    insertNewCompetUserLogin_SqlCommand.ExecuteNonQuery();
                }
            }
        }

        public string GetCompetUserLogin(long id)
        {
            string login = null;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                sqlConnection.Open();

                SqlDataReader readerDB = null;
                SqlCommand searchCompetUserLogin_SqlCommand = new SqlCommand($"SELECT * FROM [UserCompetInput] WHERE UserID = {id}", sqlConnection);
                readerDB = searchCompetUserLogin_SqlCommand.ExecuteReader();

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
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                sqlConnection.Open();

                SqlDataReader readerDB = null;
                SqlCommand searchCompetPostlink_SqlCommand = new SqlCommand($"SELECT * FROM [UserCompetInput] WHERE UserID = {id}", sqlConnection);
                readerDB = searchCompetPostlink_SqlCommand.ExecuteReader();

                if (readerDB.HasRows)
                {
                    readerDB.Close();
                    SqlCommand changeCompetPostlink_SqlCommand = new SqlCommand($"UPDATE [UserCompetInput] SET Postlink='{postlink}' WHERE UserID = {id}", sqlConnection);
                    changeCompetPostlink_SqlCommand.ExecuteNonQuery();
                }
                else
                {
                    readerDB.Close();
                    SqlCommand insertNewCompetPostlink_SqlCommand = new SqlCommand($"INSERT INTO [UserCompetInput] (UserID, Postlink) VALUES ('{id}', '{postlink}')", sqlConnection);
                    insertNewCompetPostlink_SqlCommand.ExecuteNonQuery();
                }
            }
        }

        public string GetCompetPostlink(long id)
        {
            string postlink = null;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString))
            {
                sqlConnection.Open();

                SqlDataReader readerDB = null;
                SqlCommand searchCompetPostlink_SqlCommand = new SqlCommand($"SELECT * FROM [UserCompetInput] WHERE UserID = {id}", sqlConnection);
                readerDB = searchCompetPostlink_SqlCommand.ExecuteReader();

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
