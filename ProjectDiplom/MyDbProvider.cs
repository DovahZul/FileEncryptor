using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using System.Windows.Forms;
using System.Data.SqlServerCe;

namespace diplom001
{
    public struct userDATA
    {
        public List<int> userID;// = new List<int>();
        public List<string> userPWD;//new List<string>();
        public List<string> userLGN;//= new List<string>();
        // string[] userLGN;
        public List<bool> canDelete;
        public List<int> popitki;
        public userDATA(List<int> l1, List<string> l2, List<string> l3, List<bool> l4, List<int> l5)
        {
            userID = l1;
            userLGN = l2;
            userPWD = l3;
            canDelete = l4;
            popitki = l5;

        }
    }
    class MyDbProvider
    {
        private SqlCeConnection cnn = new SqlCeConnection(Properties.Settings.Default.Database2ConnectionString);
        private SqlCeDataReader dr;
        public userDATA getAll()
        {

            List<int> l1 = new List<int>();
            List<string> l2 = new List<string>();
            List<string> l3 = new List<string>();
            List<bool> l4 = new List<bool>();
            List<int> l5 = new List<int>();
            SqlCeCommand cmd = new SqlCeCommand("SELECT id,login,password,candelete,popitki from users", cnn);

            cnn.Open();
            dr = cmd.ExecuteReader();


            // if (dr.HasRows)
            // {
            while (dr.Read())
            {
                l1.Add((int)dr[0]);
                l2.Add(dr[1].ToString());
                l3.Add(dr[2].ToString());
                l4.Add((bool)dr[3]);
                l5.Add((int)dr[4]);



            }
            // }

            dr.Close();
            cnn.Close();

            return new userDATA(l1, l2, l3, l4, l5);
        }
        public bool WriteUser(string login, string passwod, bool candelete, int popitki)
        {
            try
            {
                SqlCeCommand cmd = new SqlCeCommand("Insert INTO users(login,password,candelete,popitki) VALUES(@username,@userpwd,@userdel,@userpop)", cnn);
                //  cmd.Parameters.AddWithValue("@userid", id);
                cmd.Parameters.AddWithValue("@username", login);
                cmd.Parameters.AddWithValue("@userpwd", passwod);
                cmd.Parameters.AddWithValue("@userdel", candelete);
                cmd.Parameters.AddWithValue("@userpop", popitki);
                cnn.Open();
                cmd.ExecuteNonQuery();

                cnn.Close();
                return true;
            }
            catch (SqlCeException e)
            {

                MessageBox.Show(e.Message);
                return false;
            }

        }
        public void ExecuteQuery(string q)
        {
            try
            {
                SqlCeCommand mcd = new SqlCeCommand(q, cnn);
                if (mcd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Query Executed!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Query Not Executed " + ex.Message);
            }
        }
        public void UpdateDB()
        {
            string q = "update Database2.users";
            ExecuteQuery(q);
        }

    }

}



