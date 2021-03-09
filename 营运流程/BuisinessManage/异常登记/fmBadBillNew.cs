using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class fmBadBillNew : BaseForm
    {
        public fmBadBillNew()
        {
            InitializeComponent();
        }

        private void getdata()
        {
            if (bdate.DateTime > edate.DateTime)
            {
                XtraMessageBox.Show("��ʼ���ڲ��ܴ��ڽ�������", "����ѡ�����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                //��Ӳ���
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("webid", edwebid.Text.Trim() == "ȫ��" ? "%%" : edwebid.Text.Trim()));
                list.Add(new SqlPara("abnormalityState", StateEdit.Text.Trim() == "ȫ��" ? "%%" : StateEdit.Text.Trim()));
                list.Add(new SqlPara("zerenbumen", zerenbumen.Text.Trim() == "ȫ��" ? "%%" : zerenbumen.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BAD_TYD_CL_New", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                //������Դ
                gridshow.DataSource = ds;
                gridshow.DataMember = ds.Tables[0].ToString();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void fmBadBill_Load(object sender, EventArgs e)
        {
            //����ͨ���趨
            CommonClass.FormSet(this);
            bdate.EditValue = CommonClass.gbdate;
            edate.EditValue = CommonClass.gedate;
            BarMagagerOper.SetBarPropertity(bar3); //����о���Ĺ���������������ʵ��
            //edwebid.Text = CommonClass.UserInfo.SiteName;
            GridOper.RestoreGridLayout(gridView2, "��������¼");


            CommonClass.SetWeb(edwebid, edwebid.Text);
            CommonClass.SetWeb(zerenbumen, zerenbumen.Text);
            //CommonClass.SetWeb(DepartEidt, edwebid.Text);
            GridOper.RestoreGridLayout(gridView2, "��������¼");
            FixColumn fix = new FixColumn(gridView2, barSubItem2);

            zerenbumen.Text = CommonClass.UserInfo.WebName;
            edwebid.Text = CommonClass.UserInfo.WebName;
            //DepartEidt.Text = CommonClass.UserInfo.WebName;
            barButtonItem14.Visibility = BarItemVisibility.Never;//�ύ��OA����ʾ�ˡ��ڷ��������Զ�����

            //�����쳣����״̬����Ⱦ��Ӧ��GridViewRow��ɫ    2017-09-14  yd
            //δ����
            GridOper.CreateStyleFormatCondition(gridView2, "abnormalityState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "������", Color.Red);
            //�Ѵ���
            GridOper.CreateStyleFormatCondition(gridView2, "abnormalityState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "�Ѵ���", Color.FromArgb(0, 192, 0));
            //δ�ٲ�
            GridOper.CreateStyleFormatCondition(gridView2, "abnormalityState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "���ٲ�", Color.FromArgb(192, 0, 0));
            //���ٲ�
            GridOper.CreateStyleFormatCondition(gridView2, "abnormalityState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "���ٲ�", Color.Cyan);
            //�����
            GridOper.CreateStyleFormatCondition(gridView2, "abnormalityState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "�����", Color.FromArgb(204, 255, 0));
            
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            fmBadBillDealClone wb = new fmBadBillDealClone();
            string billno = GridOper.GetRowCellValueString(gridView2, rows, "BillNo");
            wb.crrBillNO = billno;
            wb.gv = gridView2;
            wb.look = 10;
            wb.ShowDialog();
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            //int rows = gridView2.FocusedRowHandle;
            //if (rows < 0) return;

            ////int id = Convert.ToInt32(gridView2.GetRowCellValue(rows, "id"));
            //string id = gridView2.GetRowCellValue(rows, "ID").ToString();
            //string badchuliman = gridView2.GetRowCellValue(rows, "badchuliman").ToString();//������
            //string badzerenchuliman = gridView2.GetRowCellValue(rows, "badzerenchuliman").ToString();//���λ�����
            //string badshman = gridView2.GetRowCellValue(rows, "badshman").ToString();//�����
            //string billno = gridView2.GetRowCellValue(rows, "BillNo").ToString();

            //int shstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "shstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "shstate").ToString());
            //int hfstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "hfstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "hfstate").ToString());



            //if (shstate > 0)
            //{
            //    MsgBox.ShowOK("�������������޸ģ�");
            //    return;
            //}

            //fmBadBillDealClone wb = new fmBadBillDealClone();
            //wb.gv = gridView2;
            //wb.look = 4;
            //wb.crrBillNO = billno;
            //wb.ShowDialog();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            //int id = Convert.ToInt32(gridView2.GetRowCellValue(rows, "id"));
            string id = gridView2.GetRowCellValue(rows, "ID").ToString();
            string billNo = gridView2.GetRowCellValue(rows, "BillNo").ToString();
            //string badchuliman = gridView2.GetRowCellValue(rows, "badchuliman").ToString();//������
            //string badzerenchuliman = gridView2.GetRowCellValue(rows, "badzerenchuliman").ToString();//���λ�����
            //string badshman = gridView2.GetRowCellValue(rows, "badshman").ToString();//�����
            string badcreateby = gridView2.GetRowCellValue(rows, "badcreateby").ToString();//�����

            //int shstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "shstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "shstate").ToString());
            //int hfstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "hfstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "hfstate").ToString());

            //string cancelman = gridView2.GetRowCellValue(rows, "cancelman").ToString();//ȡ����
            //string badchuliyijian = gridView2.GetRowCellValue(rows, "badchuliyijian").ToString();//ȡ����


            if (badcreateby != CommonClass.UserInfo.UserName)
            {
                MsgBox.ShowOK("ֻ�ܳ������˵Ǽǵ��쳣��");
                return;
            }

            //����״̬
            string state = gridView2.GetRowCellValue(rows, "abnormalityState").ToString();
            if (state != "������")
            {
                MsgBox.ShowOK("���봦�����̺���쳣�޷����г�����");
                return;
            }

            //���볷��ԭ��
            new frmRevocationCause(id, billNo).ShowDialog();//hj20180514
            //ˢ������
            getdata();

            //else if (badchuliyijian != "")
            //{
            //    MsgBox.ShowOK("�Ѹ��٣����ܳ�����");
            //    return;
            //}
            //else if (shstate > 0)
            //{
            //    MsgBox.ShowOK("����ᣬ���ܳ�����");
            //    return;
            //}
            //else if (hfstate > 0)
            //{
            //    MsgBox.ShowOK("�����λ��֣����ܳ�����");
            //    return;
            //}
            //else if (cancelman != "")
            //{
            //    MsgBox.ShowOK("�ѱ�ȡ�����λ��֣������ܳ�����");
            //    return;
            //}

        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            //int rows = gridView2.FocusedRowHandle;
            //if (rows < 0) return;

            ////int id = Convert.ToInt32(gridView2.GetRowCellValue(rows, "id"));
            //string id = gridView2.GetRowCellValue(rows, "ID").ToString();
            //string badchuliman = gridView2.GetRowCellValue(rows, "badchuliman").ToString();//������
            //string badzerenchuliman = gridView2.GetRowCellValue(rows, "badzerenchuliman").ToString();//���λ�����
            //string badshman = gridView2.GetRowCellValue(rows, "badshman").ToString();//�����
            //int shstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "shstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "shstate").ToString());
            //int hfstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "hfstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "hfstate").ToString());
            //fmBadBillDeal wb = new fmBadBillDeal();
            //wb.gv = gridView2;
            //wb.look = 1;
            //wb.ShowDialog();

            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            fmBadBillDealClone wb = new fmBadBillDealClone();
            string billno = GridOper.GetRowCellValueString(gridView2, rows, "BillNo");
            wb.crrBillNO = billno;
            wb.gv = gridView2;
            wb.look = 11;
            wb.ShowDialog();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            //int rows = gridView2.FocusedRowHandle;
            //if (rows < 0) return;

            ////int id = Convert.ToInt32(gridView2.GetRowCellValue(rows, "id"));
            //string id = gridView2.GetRowCellValue(rows, "ID").ToString();
            //string badchuliman = gridView2.GetRowCellValue(rows, "badchuliman").ToString();//������
            //string badzerenchuliman = gridView2.GetRowCellValue(rows, "badzerenchuliman").ToString();//���λ�����
            //int shstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "shstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "shstate").ToString());

            ////if (badchuliman == "")
            ////{
            ////    commonclass.MsgBox.ShowOK("δ�������ܽ������λ��֣�");
            ////    return;
            ////}
            ////else if (badzerenchuliman != "")
            ////{
            ////    commonclass.MsgBox.ShowOK("�ѻ������Σ�����Ҫ�ٴ���");
            ////    return;
            ////}
            ////else
            //if (shstate > 0)
            //{
            //    MsgBox.ShowOK("������������Ҫ�ٴ���");
            //    return;
            //}
            //fmBadBillDeal wb = new fmBadBillDeal();
            //wb.gv = gridView2;
            //wb.look = 2;
            //wb.ShowDialog();
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            //int id = Convert.ToInt32(gridView2.GetRowCellValue(rows, "id"));
            string id = gridView2.GetRowCellValue(rows, "ID").ToString();
            string badchuliman = gridView2.GetRowCellValue(rows, "badchuliman").ToString();//������
            string badzerenchuliman = gridView2.GetRowCellValue(rows, "badzerenchuliman").ToString();//���λ�����
            string badshman = gridView2.GetRowCellValue(rows, "badshman").ToString();//�����

            int shstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "shstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "shstate").ToString());
            int hfstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "hfstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "hfstate").ToString());

            if (hfstate == 0)
            {
                MsgBox.ShowOK("δ�������λ��֣��������");
                return;
            }

            if (hfstate == 2)
            {
                MsgBox.ShowOK("���λ����ѽ��з�������������");
                return;
            }
            fmBadBillDeal wb = new fmBadBillDeal();
            wb.gv = gridView2;
            wb.look = 3;
            wb.ShowDialog();
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView2);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView2);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView2, "��������¼");
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout("��������¼");
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView2);
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            //int rows = gridView2.FocusedRowHandle;
            //if (rows < 0) return;

            //int shstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "shstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "shstate").ToString());

            ////if (shstate > 0)
            ////{
            ////    MsgBox.ShowOK("������������Ҫ�ٴ���");
            ////    return;
            ////}
            //fmBadBillDeal wb = new fmBadBillDeal();
            //wb.gv = gridView2;
            //wb.look = 5;
            //wb.ShowDialog();
        }

        private void gridView2_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e == null || e.Column.FieldName != "rowid") return;
            e.Value = e.RowHandle + 1;
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            frmBillSearch.ShowBillSearch(GridOper.GetRowCellValueString(gridView2, "BillNo"));
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            //GridView gv = gridView2;
            //if (gv.FocusedRowHandle < 0)
            //    return;

            //string billno = gv.GetRowCellValue(gv.FocusedRowHandle, "BillNo").ToString();

            //#region ���ر���ͼƬ
            //string file = "";
            //try
            //{
            //    List<SqlPara> list = new List<SqlPara>();
            //    list.Add(new SqlPara("BillNo", billno));
            //    list.Add(new SqlPara("BillType", 1));
            //    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_TBFILEINFO_BadBill", list);
            //    DataSet ds = SqlHelper.GetDataSet(sps);

            //    if (ds != null && ds.Tables.Count > 0)
            //    {
            //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //        {
            //            string filename = ConvertType.ToString(ds.Tables[0].Rows[i]["FilePath"]);
            //            string[] sArray = filename.Split('/');

            //            string bdPath = System.Windows.Forms.Application.StartupPath + "/TempFile";
            //            if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
            //            string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));
            //            WebClient wc = new WebClient();
            //            if (!File.Exists(bdFileName))
            //            {
            //                wc.DownloadFile("http://ZQTMS.dekuncn.com:7020" + filename, bdFileName);
            //            }

            //            //�ϴ�ͼƬ��OA������
            //            byte[] bt = wc.UploadFile(HttpHelper.OAUrlImage, "POST", bdFileName);
            //            string json = Encoding.UTF8.GetString(bt);
            //            OAFileUpResult result = JsonConvert.DeserializeObject<OAFileUpResult>(json);
            //            if (result.Success == true)
            //            {
            //                file += string.Format("<File>{0}</File>", result.FileName);
            //            }
            //            else
            //            {
            //                continue;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowException(ex);
            //    return;
            //}
            //#endregion

            //string ConsignorCellPhone = gv.GetRowCellValue(gv.FocusedRowHandle, "ConsignorCellPhone").ToString();
            //string ConsignorPhone = gv.GetRowCellValue(gv.FocusedRowHandle, "ConsignorPhone").ToString();
            //string string19 = ConsignorCellPhone + "/" + ConsignorPhone;
            //string19 = string19.Trim('/');

            //string ConsigneeCellPhone = gv.GetRowCellValue(gv.FocusedRowHandle, "ConsigneeCellPhone").ToString();
            //string ConsigneePhone = gv.GetRowCellValue(gv.FocusedRowHandle, "ConsigneePhone").ToString();
            //string string26 = ConsigneeCellPhone + "/" + ConsigneePhone;
            //string26 = string26.Trim('/');

            //string zerenwebid1 = gv.GetRowCellValue(gv.FocusedRowHandle, "zerenwebid1").ToString();
            //string zerenwebid2 = gv.GetRowCellValue(gv.FocusedRowHandle, "zerenwebid2").ToString();
            //string zerenwebid3 = gv.GetRowCellValue(gv.FocusedRowHandle, "zerenwebid3").ToString();
            //string string32 = zerenwebid1 + "/" + zerenwebid2 + "/" + zerenwebid3;
            //string32 = string32.Trim('/');


            //StringBuilder sb = new StringBuilder();
            //sb.AppendFormat("<date1>{0}</date1>", gv.GetRowCellValue(gv.FocusedRowHandle, "BillDate"));//ZQTMS��������
            //sb.AppendFormat("<number2>{0}</number2>", gv.GetRowCellValue(gv.FocusedRowHandle, "accspe"));//������
            //sb.AppendFormat("<string24>{0}</string24>", gv.GetRowCellValue(gv.FocusedRowHandle, "webid"));//��������
            //sb.AppendFormat("<string10>{0}</string10>", gv.GetRowCellValue(gv.FocusedRowHandle, "Varieties"));//Ʒ��
            //sb.AppendFormat("<string1>{0}</string1>", gv.GetRowCellValue(gv.FocusedRowHandle, "Package"));//��װ
            //sb.AppendFormat("<number7>{0}</number7>", gv.GetRowCellValue(gv.FocusedRowHandle, "Num"));//����
            ////sb.AppendFormat("<string28>{0}</string28>", gv.GetRowCellValue(gv.FocusedRowHandle, "qsstate"));//ǩ�����
            //sb.AppendFormat("<string4>{0}</string4>", gv.GetRowCellValue(gv.FocusedRowHandle, "ConsignorCompany"));//������λ
            //sb.AppendFormat("<string5>{0}</string5>", gv.GetRowCellValue(gv.FocusedRowHandle, "ConsignorName"));//������
            //sb.AppendFormat("<string19>{0}</string19>", string19);//��ϵ��ʽ�������ˣ�
            //sb.AppendFormat("<string2>{0}</string2>", gv.GetRowCellValue(gv.FocusedRowHandle, "PaymentMode"));//���㷽ʽ
            //sb.AppendFormat("<string6>{0}</string6>", gv.GetRowCellValue(gv.FocusedRowHandle, "ConsigneeCompany"));//�ջ���λ
            //sb.AppendFormat("<string11>{0}</string11>", gv.GetRowCellValue(gv.FocusedRowHandle, "ConsigneeName"));//�ջ���
            //sb.AppendFormat("<string26>{0}</string26>", string26);//��ϵ��ʽ���ջ��ˣ�
            //sb.AppendFormat("<string20>{0}</string20>", gv.GetRowCellValue(gv.FocusedRowHandle, "badtype"));//�쳣����
            //sb.AppendFormat("<string22>{0}</string22>", ConvertType.ToString(gv.GetRowCellValue(gv.FocusedRowHandle, "badtype")) == "" ? "����" : ConvertType.ToString(gv.GetRowCellValue(gv.FocusedRowHandle, "badtype")));//���ⷽʽ
            //sb.AppendFormat("<string32>{0}</string32>", string32);//���β���
            //sb.AppendFormat("<number1>{0}</number1>", gv.GetRowCellValue(gv.FocusedRowHandle, "accpfe"));//ʵ���⸶���
            //sb.AppendFormat("<string29>{0}</string29>", billno);//�˵���
            //sb.AppendFormat("<string33>{0}</string33>", gv.GetRowCellValue(gv.FocusedRowHandle, "DestinationSite"));//��վ

            //sb.AppendFormat("<string21>{0}</string21>", gv.GetRowCellValue(gv.FocusedRowHandle, "SuopeiMan"));//������
            //sb.AppendFormat("<string27>{0}</string27>", gv.GetRowCellValue(gv.FocusedRowHandle, "SuopeiPhone"));//�����˵���ϵ��ʽ
            //sb.AppendFormat("<string7>{0}</string7>", gv.GetRowCellValue(gv.FocusedRowHandle, "SpBankName"));//���л���
            //sb.AppendFormat("<string8>{0}</string8>", gv.GetRowCellValue(gv.FocusedRowHandle, "SpBankAdd"));//������
            //sb.AppendFormat("<string9>{0}</string9>", gv.GetRowCellValue(gv.FocusedRowHandle, "SpBankAcount"));//�˺�
            //sb.AppendFormat("<string30>{0}</string30>", "�����ͻ�");//�ͻ�����
            //sb.AppendFormat("<string28>{0}</string28>", "����ǩ��");//ǩ�����
            ////MsgBox.ShowOK(sb.ToString());

            //string msg = SubmitToOA.PostBadBill(billno, sb.ToString(), file);
            //if (msg != "")//˵���ύʧ��
            //{
            //    MsgBox.ShowError("�쳣������Ϣ�ύ��OA����ʧ�ܣ��ɳ��������ύ!" + msg);
            //    return;
            //}
            //MsgBox.ShowOK();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            //frmBillSearch frm = new frmBillSearch();

        }
    }
}