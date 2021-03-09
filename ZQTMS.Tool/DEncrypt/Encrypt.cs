
using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace ZQTMS.Tool
{
	/// <summary>
	/// ���ܽ���ʵ���ࡣ
	/// </summary>
	public static class Encrypt
	{
		//��Կ
		private static byte[] arrDESKey = new byte[] {42, 16, 93, 156, 78, 4, 218, 32};
		private static byte[] arrDESIV = new byte[] {55, 103, 246, 79, 36, 99, 167, 3};

		/// <summary>
		/// ���ܡ�
		/// </summary>
		/// <param name="m_Need_Encode_String"></param>
		/// <returns></returns>
		public static string Encode(string m_Need_Encode_String)
		{
			if (m_Need_Encode_String == null)
			{
				throw new Exception("Error: \nԴ�ַ���Ϊ�գ���");
			}
			DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
			MemoryStream objMemoryStream = new MemoryStream();
			CryptoStream objCryptoStream = new CryptoStream(objMemoryStream,objDES.CreateEncryptor(arrDESKey,arrDESIV),CryptoStreamMode.Write);
			StreamWriter objStreamWriter = new StreamWriter(objCryptoStream);
			objStreamWriter.Write(m_Need_Encode_String);
			objStreamWriter.Flush();
			objCryptoStream.FlushFinalBlock();
			objMemoryStream.Flush();
			return Convert.ToBase64String(objMemoryStream.GetBuffer(), 0, (int)objMemoryStream.Length);
		}

		/// <summary>
		/// ���ܡ�
		/// </summary>
		/// <param name="m_Need_Encode_String"></param>
		/// <returns></returns>
		public static string Decode(string m_Need_Encode_String)
		{
			if (m_Need_Encode_String == null)
			{
				throw new Exception("Error: \nԴ�ַ���Ϊ�գ���");
			}
			DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
			byte[] arrInput = Convert.FromBase64String(m_Need_Encode_String);
			MemoryStream objMemoryStream = new MemoryStream(arrInput);
			CryptoStream objCryptoStream = new CryptoStream(objMemoryStream,objDES.CreateDecryptor(arrDESKey,arrDESIV),CryptoStreamMode.Read);
			StreamReader  objStreamReader  = new StreamReader(objCryptoStream);
			return objStreamReader.ReadToEnd();
		}

        /// <summary>
        /// md5
        /// </summary>
        /// <param name="encypStr"></param>
        /// <returns></returns>
        public static string Md5ForString(string encypStr)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();
            byte[] inputBye;
            byte[] outputBye;
            inputBye = System.Text.Encoding.ASCII.GetBytes(encypStr);
            outputBye = m5.ComputeHash(inputBye);
            retStr = Convert.ToBase64String(outputBye);
            return (retStr);
        }


        /// <summary>
        /// ��ȡ�ļ���MD5��
        /// </summary>
        /// <param name="filePath">�ļ�������·������·�����ļ�������չ��</param>
        /// <returns></returns>
        public static string MD5ForFile(string filePath)
        {
            try
            {
                string temp = filePath + ".md5temp";
                if (File.Exists(temp))
                {
                    File.Delete(temp);
                }
                File.Copy(filePath, temp, true);

                byte[] retVal;
                using (FileStream file = new FileStream(temp, FileMode.Open))
                {
                    
                    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    retVal = md5.ComputeHash(file);
                    file.Close();
                    file.Dispose();
                }
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                File.Delete(temp);

                return sb.ToString().ToUpper();
            }
            catch (Exception ex)
            {
                throw new Exception("��ȡ�ļ�MD5ʧ�ܣ�" + ex.Message);
            }
        }
	}
}
