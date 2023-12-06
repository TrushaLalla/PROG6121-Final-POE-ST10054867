using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Trial.Models;

namespace Trial.DatabaseContent
{
    public class PROG6212Database
    {
        private string conString = DBConString.ReturnConString();


        #region studentCommands
        public void UserRegistrationInsert(int id, string user1, string pass1)
        {
            using (SqlConnection mycon = new SqlConnection(conString))
            {
                mycon.Open();
                SqlCommand cmdInsert =
                    new SqlCommand($"INSERT INTO STUDENT " +
                    $"values('STID{id.ToString()}','{user1}','{pass1}')", mycon);

                cmdInsert.ExecuteNonQuery();
            }
        }


        //public void DeleteUser(string userId)
        //{
        //    using (SqlConnection mycon = new SqlConnection(conString))
        //    {
        //        SqlCommand cmdDelete = new SqlCommand($"DELETE FROM STUDENT where " +
        //            $"StudentID = '{userId}'", mycon);
        //        mycon.Open();

        //        cmdDelete.ExecuteNonQuery();
        //    }

        //}

        public string GetUser(string username, string password)
        {
            string checkAuth = "";
            SqlConnection mycon = new SqlConnection(conString);
            SqlCommand cmdGet = new SqlCommand($"SELECT * FROM STUDENT " +
                $"where UserName = '{username}'", mycon);

            mycon.Open();
            using (SqlDataReader reader = cmdGet.ExecuteReader())
            {
                while (reader.Read())
                {
                    if ((string)reader[1] == username && (string)reader[2] == getHashofPass(password))
                    {
                        checkAuth = (string)reader[0];
                    }
                    else
                    {
                        checkAuth = null;
                    }
                }
                mycon.Close();
                return checkAuth;
            }
        }

        public string getHashofPass(string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        }
        #endregion
        #region moduleCommands
        public void ModuleInsert(string code,string programming, int credit,int clshrs, int thrs,string userid)
        {
            using (SqlConnection mycon = new SqlConnection(conString))
            {
                mycon.Open();
                SqlCommand cmdInsert =
                    new SqlCommand($"INSERT INTO MODULES " +
                    $"values('{code}','{programming}',{credit},{clshrs},{thrs},{thrs},'{userid}')", mycon);

                cmdInsert.ExecuteNonQuery();
            }
        }
        public void DeleteModule(string code)
        {
            using (SqlConnection mycon = new SqlConnection(conString))
            {
                SqlCommand cmdDelete = new SqlCommand($"DELETE FROM MODULES where " +
                    $"Code = '{code}'", mycon);
                mycon.Open();

                cmdDelete.ExecuteNonQuery();
            }

        }
        public List<Modules> GetModule(string userid)
        {
            List<Modules> modlist = new List<Modules>();
            SqlConnection mycon = new SqlConnection(conString);
            SqlDataAdapter cmdGet = new SqlDataAdapter($"SELECT * FROM MODULES where StudentID = '{userid}'", mycon);
            DataTable mytable = new DataTable();
            DataRow myrow;
            string code, moduleName, studentID;
            int credit, classhrs, totalhrs, remainhrs;

            mycon.Open();
            cmdGet.Fill(mytable);

            if (mytable.Rows.Count> 0)
            {
                for (int i = 0; i < mytable.Rows.Count; i++)
                {
                    myrow = mytable.Rows[i];
                    code = (string)myrow[0];
                    moduleName = (string)myrow[1];
                    credit = (int)myrow[2];
                    classhrs= (int)myrow[3];
                    totalhrs= (int)myrow[4];
                    remainhrs= (int)myrow[5];
                    studentID= (string)myrow[6];

                    Modules ml = new Modules()
                    {
                        Code = code,
                        Name = moduleName,
                        NoOfCredits = credit,
                        ClassHrsPerWeek = classhrs,
                        TotalhoursOfStudy = totalhrs,
                        RemainingHoursOfStudy = remainhrs,
                        Studentid= studentID,
                    };

                    modlist.Add(ml);
                }
                
            }
            mycon.Close();
            return modlist;

        }
        public void UpdateModuleList(string sid, string codeid, int newremhrs)
        {
            using (SqlConnection mycon = new SqlConnection(conString))
            {
                mycon.Open();
                SqlCommand cmd = new SqlCommand($"update Modules set RemainingHours = {newremhrs}" +
                    $"where StudentID = '{sid}' AND Code ='{codeid}'", mycon);
                cmd.ExecuteNonQuery();
            }
        }

        public Modules getmodulebyid(string modulecode, string userid)
        {
            Modules mod = GetModule(userid).First(x => x.Code == modulecode);
            return mod;
        }

        #endregion
        #region StudySes Commands
        public void StudySessionInsert(int num, string code, int remhrs, DateTime date,string studid)
        {
            string formattedDate = date.ToString("yyyy-MM-dd HH:mm:ss");
            using (SqlConnection mycon = new SqlConnection(conString))
            {
                mycon.Open();
                SqlCommand cmdInsert =
                    new SqlCommand($"INSERT INTO STUDYSESSION " +
                    $"values('StudID{num.ToString()}','{code}',{remhrs},'{formattedDate}','{studid}')", mycon);

                cmdInsert.ExecuteNonQuery();
            }
        }


        public List<StudyLogs> GetStudySession(string studentID)
        {
            List<StudyLogs> studyLogs = new List<StudyLogs>();
            using (SqlConnection mycon = new SqlConnection(conString))
            {
                SqlCommand cmdGet = new SqlCommand($"SELECT * FROM STUDYSESSION " +
                    $"where StudentID = '{studentID}'", mycon);

                mycon.Open();
                using (SqlDataReader reader = cmdGet.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StudyLogs log = new StudyLogs
                        {
                            Code = reader["Code"].ToString(),
                            RemainingHoursOfStudy = Convert.ToInt32(reader["RemainingHours"]),
                            Date = Convert.ToDateTime(reader["Date"])
                        };
                        studyLogs.Add(log);
                    }
                }
            }
            return studyLogs;
        }

        public List<StudyLogs> GetStudyLogsByCode(string studentID, string code)
        {
            var studyLogs = GetStudySession(studentID);
            var filteredStudyLogs = studyLogs.Where(log => log.Code == code).ToList();
            return filteredStudyLogs;
        }
        public List<StudyLogs> GetGroupedStudyLogs(string studentID)
        {
            var studyLogs = GetStudySession(studentID);
            var groupedStudyLogs = studyLogs.GroupBy(log => log.Code)
                                            .Select(group => new StudyLogs
                                            {
                                                Code = group.Key,
                                                RemainingHoursOfStudy = group.Sum(log => log.RemainingHoursOfStudy),
                                                Date = group.Max(log => log.Date)
                                            })
                                            .ToList();
            return groupedStudyLogs;
        }
        #endregion
        #region SemDates Commands

        public void SemesterdatesInsert(int id, DateTime sdate, DateTime edate, int numweeks)
        {
            using (SqlConnection mycon = new SqlConnection(conString))
            {
                mycon.Open();
                SqlCommand cmdInsert =
            new SqlCommand($"INSERT INTO SemesterDate " +
            $"values('SemID{id.ToString()}','{sdate.ToString("yyyy/MM/dd")}','{edate.ToString("yyyy/MM/dd")}',{numweeks})", mycon);

                cmdInsert.ExecuteNonQuery();
            }
        }

        public string GetSemesterdates(int semid)
        {
            string wrd = "";
            SqlConnection mycon = new SqlConnection(conString);
            SqlCommand cmdGet = new SqlCommand($"SELECT * FROM SemesterDate " +
                $"where SemesterID = 'SemID{semid.ToString()}'", mycon);

            mycon.Open();
            using (SqlDataReader reader = cmdGet.ExecuteReader())
            {
                while (reader.Read())
                {
                    wrd = (string)reader[0];
                }
                mycon.Close();
                return wrd;
            }
        }
        #endregion


    }
}

