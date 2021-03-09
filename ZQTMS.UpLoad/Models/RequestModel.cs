using System;
using System.Collections.Generic;
using System.Text;

namespace ZQTMS.UpLoad
{
    public class RequestModel<T>
    {
        public T Request { get; set; }
        public int OperType { get; set; }
    }

    public class RequestModelClone
    { 
         public string OperType
        {
            get;
            set;
        }
        public string Request
        {
            get;
            set;
        }
        public RequestModelClone()
        {
        }
        public RequestModelClone(string OperType, string Request)
        {
            this.OperType = OperType;
            this.Request = Request;
        }
    
    }
}
