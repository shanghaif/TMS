using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ZQTMS.Common
{
    public class OAFileUpResult
    {
        public string FileName;

        public bool Success;

        public string ErrMsg;
    }

    public static class SubmitToOA
    {
        static string flowmodelid = "60483";

        /// <summary>
        /// XML原始文档
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        private static XmlDocument GetXmlDoc(string subject)
        {
            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"
                    + "<Form>"
                    + "  <sys>"
                    + "    <starterid>{0}</starterid>"
                    + "    <flowmodelid>{1}</flowmodelid>"
                    + "    <subject>{2}</subject>"
                    + "    <autosend>0</autosend>"
                    + "  </sys>"
                    + "  <header></header>"
                    + "  <body></body>"
                    + "</Form>";

            xml = string.Format(xml, CommonClass.OAUserID, flowmodelid, subject);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }


        private static XmlDocument GetXmlADoc(string subject)
        {
            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"
                    + "<Form>"
                    + "  <sys>"
                    + "    <starterid>{0}</starterid>"
                    + "    <flowmodelid>{1}</flowmodelid>"
                    + "    <subject>{2}</subject>"
                    + "    <autosend>0</autosend>"
                    + "  </sys>"
                    + "  <header></header>"
                    + "  <Attments></Attments>"
                    + "  <body></body>"
                    + "</Form>";

            xml = string.Format(xml, CommonClass.OAUserID, flowmodelid, subject);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }


        /// <summary>
        /// 提交异常理赔信息到OA
        /// </summary>
        /// <param name="billno">运单号</param>
        /// <param name="header">明细内容(xml)</param>
        public static string PostBadBill(string billno, string header, string attments)
        {
            string subject = string.Format("关于运单【{0}】的异常理赔", billno);
            XmlDocument doc = GetXmlADoc(subject);

            XmlNode nodeheader = doc.SelectSingleNode("Form/header");
            nodeheader.InnerXml = header;

            XmlNode nodeattments = doc.SelectSingleNode("Form/Attments");
            nodeattments.InnerXml = attments;

            string data = doc.InnerXml;
            string result = HttpHelper.HttpPostOA(data);
            return result;
        }
    }
}
