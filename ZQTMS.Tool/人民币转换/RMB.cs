using System;

namespace ZQTMS.Tool
{
    /// <summary> 
    /// Rmb ��ժҪ˵���� 
    /// </summary> 
    public class Rmb
    {
        /// <summary> 
        /// ת������Ҵ�С��� 
        /// </summary> 
        /// <param name="num">���</param> 
        /// <returns>���ش�д��ʽ</returns> 
        public static string GetChinaMoney(decimal num)
        {
            string str1 = "��Ҽ��������½��ƾ�";            //0-9����Ӧ�ĺ��� 
            string str2 = "��Ǫ��ʰ��Ǫ��ʰ��Ǫ��ʰԪ�Ƿ�"; //����λ����Ӧ�ĺ��� 
            string str3 = "";    //��ԭnumֵ��ȡ����ֵ 
            string str4 = "";    //���ֵ��ַ�����ʽ 
            string str5 = "";  //����Ҵ�д�����ʽ 
            int i;    //ѭ������ 
            int j;    //num��ֵ����100���ַ������� 
            string ch1 = "";    //���ֵĺ������ 
            string ch2 = "";    //����λ�ĺ��ֶ��� 
            int nzero = 0;  //����������������ֵ�Ǽ��� 
            int temp;            //��ԭnumֵ��ȡ����ֵ 

            num = Math.Round(Math.Abs(num), 2);    //��numȡ����ֵ����������ȡ2λС�� 
            str4 = ((long)(num * 100)).ToString();        //��num��100��ת�����ַ�����ʽ 
            j = str4.Length;      //�ҳ����λ 
            if (j > 15) { return "������"; }
            str2 = str2.Substring(15 - j);   //ȡ����Ӧλ����str2��ֵ���磺200.55,jΪ5����str2=��ʰԪ�Ƿ� 

            //ѭ��ȡ��ÿһλ��Ҫת����ֵ 
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //ȡ����ת����ĳһλ��ֵ 
                temp = Convert.ToInt32(str3);      //ת��Ϊ���� 
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //����ȡλ����ΪԪ�����ڡ������ϵ�����ʱ 
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "��" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //��λ�����ڣ��ڣ���Ԫλ�ȹؼ�λ 
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "��" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //�����λ����λ��Ԫλ�������д�� 
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //���һλ���֣�Ϊ0ʱ�����ϡ����� 
                    str5 = str5 + '��';
                }
            }
            if (num == 0)
            {
                str5 = "��Ԫ��";
            }
            return str5;
        }

        /// <summary> 
        /// һ�����أ����ַ�����ת���������ڵ���CmycurD(decimal num) 
        /// </summary> 
        /// <param name="num">�û�����Ľ��ַ�����ʽδת��decimal</param> 
        /// <returns></returns> 
        public static string GetChinaMoney(string numstr)
        {
            try
            {
                decimal num = Convert.ToDecimal(numstr);
                return GetChinaMoney(num);
            }
            catch
            {
                return "��������ʽ��";
            }
        }
    }


    #region MyRegion
    //public class RMB1
    //{
    //    public static string ToRMB(string x)
    //    {
    //        string ret = "";
    //        int nnum;
    //        x = x.Replace("-", "");
    //        if (x.IndexOf(".") > -1)
    //        {
    //            if (x.Length == (x.IndexOf(".") + 2))
    //                x = x + "0";
    //            nnum = int.Parse(x.Substring(0, x.IndexOf(".")));
    //            if (x.Substring(x.IndexOf(".") + 1, 2) == "00")
    //                ret = ToInt(nnum.ToString()) + "Ԫ��";
    //            else
    //                ret = ToInt(nnum.ToString()) + "Ԫ" + ToDot(x.Substring(x.IndexOf(".") + 1, 2));
    //        }
    //        else
    //        {
    //            ret = ToInt(x) + "Ԫ��";
    //        }
    //        return ret;
    //    }
    //    private static char ToNum(char x)
    //    {
    //        string strChnNames = "��Ҽ��������½��ƾ�";
    //        string strNumNames = "0123456789";
    //        return strChnNames[strNumNames.IndexOf(x)];
    //    }
    //    private static string ChangeInt(string x)
    //    {
    //        string[] strArrayLevelNames = new string[4] { "", "ʰ", "��", "Ǫ" };
    //        string ret = "";
    //        int i;
    //        for (i = x.Length - 1; i >= 0; i--)
    //            if (x[i] == '0')
    //                ret = ToNum(x[i]) + ret;
    //            else
    //                ret = ToNum(x[i]) + strArrayLevelNames[x.Length - 1 - i] + ret;
    //        while ((i = ret.IndexOf("����")) != -1)
    //            ret = ret.Remove(i, 1);
    //        if (ret[ret.Length - 1] == '��' && ret.Length > 1)
    //            ret = ret.Remove(ret.Length - 1, 1);
    //        if (ret.Length >= 2 && ret.Substring(0, 2) == "Ҽʰ")
    //            ret = ret.Remove(0, 1);
    //        return ret;
    //    }
    //    private static string ToInt(string x)
    //    {
    //        int len = x.Length;
    //        string ret, temp;
    //        if (len <= 4)
    //            ret = ChangeInt(x);
    //        else if (len <= 8)
    //        {
    //            ret = ChangeInt(x.Substring(0, len - 4)) + "��";
    //            temp = ChangeInt(x.Substring(len - 4, 4));
    //            if (temp.IndexOf("Ǫ") == -1 && temp != "")
    //                ret += "��" + temp;
    //            else
    //                ret += temp;
    //        }
    //        else
    //        {
    //            ret = ChangeInt(x.Substring(0, len - 8)) + "��";
    //            temp = ChangeInt(x.Substring(len - 8, 4));
    //            if (temp.IndexOf("Ǫ") == -1 && temp != "")
    //                ret += "��" + temp;
    //            else
    //                ret += temp;
    //            ret += "��";
    //            temp = ChangeInt(x.Substring(len - 4, 4));
    //            if (temp.IndexOf("Ǫ") == -1 && temp != "")
    //                ret += "��" + temp;
    //            else
    //                ret += temp;
    //        }
    //        int i;
    //        if ((i = ret.IndexOf("����")) != -1)
    //            ret = ret.Remove(i + 1, 1);
    //        while ((i = ret.IndexOf("����")) != -1)
    //            ret = ret.Remove(i, 1);
    //        if (ret[ret.Length - 1] == '��' && ret.Length > 1)
    //            ret = ret.Remove(ret.Length - 1, 1);
    //        return ret;
    //    }
    //    private static string ToDot(string x)
    //    {
    //        string ret = "";
    //        string sdot = "�Ƿ�";
    //        if (x.Length > 2)
    //            x = x.Substring(0, 2);
    //        for (int i = 0; i < x.Length; i++)
    //        {
    //            ret += ToNum(x[i]) + sdot.Substring(i, 1);
    //        }
    //        if (ret.IndexOf("���") > -1)
    //            ret = ret.Substring(2, 2);
    //        if (ret.IndexOf("���") > -1)
    //            ret = ret.Substring(0, 2);
    //        return ret;
    //    }

    //    public static string GetChinaMoney(decimal money)
    //    {
    //        return ToRMB(money.ToString());
    //    }
    //} 
    #endregion

}
