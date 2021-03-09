using System;
using System.Collections.Generic;
using System.Text;

namespace ZQTMS.UpLoad
{
    [Serializable]
    public class ResponseModelClone<T>
    {        
        public string State { get; set; }       
        public T Result { get; set; }      
        public string Message { get; set; }
    }
}
