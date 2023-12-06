using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClassLibrary2;

namespace Trial.DatabaseContent
{
    public class SingletonStudentIDHolder
    {
        private static readonly SingletonStudentIDHolder _instance= new SingletonStudentIDHolder();
        string id;
        public string code;
        public SingletonStudentIDHolder()
        {

        }

        public static SingletonStudentIDHolder Instance()
        {
            return _instance;
        }

        public void setid(string stuid)
        {
            id= stuid;
        }

        public string getid()
        {
            return id;
        }

    }
}
