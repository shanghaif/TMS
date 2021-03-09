using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using NPOI;
using NPOI.HSSF;
using NPOI.XSSF.UserModel;
using System.Data;

namespace ZQTMS.Common
{
   public static class NpoiOperExcel
    {

        /// <summary>  
        /// 将excel导入到datatable  
        /// </summary>  
        /// <param name="filePath">excel路径</param>  
        /// <param name="isColumnName">是否有表头，是则ture不是则false</param>  
        /// <returns>返回datatable</returns>  
        public static DataTable ExcelToDataTable(string filePath, bool isTableName)
        {
            DataTable dataTable = null;
            FileStream fs = null;
            DataColumn column = null;
            DataRow dataRow = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow row = null;
            IRow firstRow=null;
            ICell cell = null;
            int startRow = 0;
            string qFileName = filePath.Substring(0, filePath.LastIndexOf('.'));//获取文件前缀
            string ext = Path.GetExtension(filePath);//获取文件后缀
            string name = DateTime.Now.ToString().Replace('/', '0').Replace(' ', '0').Replace(':', '0');
            // string ex=filePath.
            string newFileName = name + ext;//组成新文件名
            try
            {  //fs = File.OpenRead(filePath)
                File.Copy(filePath, newFileName);
                using (fs = File.OpenRead(newFileName))
                {

                    // 2007版本  
                    if (filePath.IndexOf(".xlsx") > 0)
                        workbook = new XSSFWorkbook(fs);
                    // 2003版本  
                    else if (filePath.IndexOf(".xls") > 0)
                        workbook = new HSSFWorkbook(fs);

                    if (workbook != null)
                    {
                        sheet = workbook.GetSheetAt(0);//读取第一个sheet，当然也可以循环读取每个sheet  
                        dataTable = new DataTable();
                        if (sheet != null)
                        {
                            int rowCount = sheet.LastRowNum;//总行数  
                            List<IRow> rows = new List<IRow>();
                            for (int i = 0; i <= rowCount;i++ )
                            {
                                IRow row1 = sheet.GetRow(i);
                                if(row1==null)
                                { continue; }
                               
                                int cellsCount = row1.LastCellNum;
                                for(int j=0;j<cellsCount;j++)
                                {
                                    if (row1.GetCell(j) != null)
                                    {
                                        bool flag = false;
                                        switch (row1.GetCell(j).CellType)
                                        {
                                            case CellType.Numeric:
                                                if (row1.GetCell(j) != null && row1.GetCell(j).NumericCellValue != null)
                                                {
                                                    rows.Add(row1);
                                                    flag = true;
                                                }
                                                break;
                                            case CellType.String:
                                                if (row1.GetCell(j) != null && row1.GetCell(j).StringCellValue != "")
                                                {
                                                    rows.Add(row1);
                                                    flag = true;
                                                }
                                                break;
                                        }
                                        if (flag)
                                        {
                                            break;
                                        }
                                    }
                                   
                                }
                                
                            }
                                if (rows.Count > 0)
                                {                                  
                                    int cellCount = 0;
                                    //构建datatable的列  
                                    if (isTableName)
                                    {
                                         firstRow = rows[1];
                                        cellCount = firstRow.LastCellNum;                                        
                                        startRow = 2;//如果第一行是标题，则从第三行开始读取  
                                        for (int i = rows[1].FirstCellNum; i <= cellCount; ++i)
                                        {
                                            cell = firstRow.GetCell(i);
                                            if (cell != null)
                                            {
                                                if (cell.StringCellValue != "")
                                                {
                                                    column = new DataColumn(cell.StringCellValue);
                                                    dataTable.Columns.Add(column);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                         firstRow = rows[0];
                                        cellCount = firstRow.LastCellNum;
                                        startRow = 1;                                     
                                        for (int i = rows[0].FirstCellNum; i < cellCount; ++i)
                                        {
                                            cell = firstRow.GetCell(i);
                                            if (cell != null)
                                            {
                                                if (cell.StringCellValue != "")
                                                {
                                                    column = new DataColumn(cell.StringCellValue);
                                                    dataTable.Columns.Add(column);
                                                }
                                            }
                                        }
                                    }

                                    //填充行  
                                    for (int i = startRow; i < rows.Count; ++i)
                                    {
                                        int k = 0;
                                        //int cellCount = firstRow.LastCellNum;//列数 
                                        row = rows[i];
                                        if (row == null) continue;

                                        dataRow = dataTable.NewRow();
                                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                                        {
                                             cell = row.GetCell(j);
                                            ICell cell1 = firstRow.GetCell(j);
                                             bool flag = false;
                                           //type=  cell.GetType() ;
                                             switch (cell.CellType)
                                             { 
                                                 case CellType.Numeric:
                                                     if (cell != null && cell.NumericCellValue != null)
                                                     {
                                                         flag = true;
                                                     }
                                                     break;
                                                 case CellType.String:
                                                     if (cell != null && cell.StringCellValue != "")
                                                     {
                                                         flag = true;
                                                     }
                                                     break;
                                             }
                                            //if(cell!=null &&cell.StringCellValue!="")
                                            //{
                                            //    flag = true;
                                            //}
                                            if(flag)
                                            {
                                                
                                                for(int l=j;l<dataTable.Columns.Count+j;l++)
                                                {
                                                    cell = row.GetCell(l);
                                                    if (cell == null)
                                                    { 
                                                        dataRow[k]="";
                                                        k++;
                                                        continue;
                                                    }
                                                    switch (cell.CellType)
                                                    {
                                                        case CellType.Blank:
                                                            dataRow[k] = "";
                                                            k++;
                                                            break;
                                                        case CellType.Boolean:
                                                            dataRow[k] = cell.BooleanCellValue;
                                                            k++;
                                                            break;
                                                        case CellType.Error:
                                                            dataRow[k] ="";
                                                            k++;
                                                            break;
                                                        case CellType.Formula:
                                                            dataRow[k] = "";
                                                            k++;
                                                            break;
                                                       
                                                        case CellType.Unknown:
                                                            dataRow[k] = "";
                                                            k++;
                                                            break;
                                                        case CellType.Numeric:
                                                            short format = cell.CellStyle.DataFormat;
                                                            //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理  
                                                            if (format == 14 || format == 31 || format == 57 || format == 58 || format == 176)
                                                            {
                                                                dataRow[k] = cell.DateCellValue;
                                                                k++;
                                                            }

                                                            else
                                                            {
                                                                dataRow[k] = cell.NumericCellValue;
                                                                k++;
                                                            }
                                                            break;
                                                        case CellType.String:
                                                            dataRow[k] = cell.StringCellValue;
                                                            k++;
                                                            break;
                                                            
                                                    }
                                                }
                                                flag = false;
                                                break;
                                            }
                                           
                                           
                                        }
                                        dataTable.Rows.Add(dataRow);
                                    }
                                }
                        }
                    }
                }
                File.Delete(newFileName);
                return dataTable;

            }
            catch (Exception ex)
            {
                File.Delete(newFileName);
                if (fs != null)
                {
                    fs.Close();
                }
                return null;
                 
            }
        }

               /// <summary>  
        /// 将excel导入到datatable  
        /// </summary>  
        /// <param name="filePath">excel路径</param>  
        /// <param name="isColumnName">第一行是否是列名</param>  
        /// <returns>返回datatable</returns>  
        public static DataTable ExcelToDataTable2(string filePath, bool isColumnName)  
        {  
            DataTable dataTable = null;  
            FileStream fs = null;  
            DataColumn column = null;  
            DataRow dataRow = null;  
            IWorkbook workbook = null;  
            ISheet sheet = null;  
            IRow row = null;  
            ICell cell = null;  
            int startRow = 0;  
            try  
            {  
                using (fs = File.OpenRead(filePath))  
                {  
                    // 2007版本  
                    if (filePath.IndexOf(".xlsx") > 0)  
                        workbook = new XSSFWorkbook(fs);  
                    // 2003版本  
                    else if (filePath.IndexOf(".xls") > 0)  
                        workbook = new HSSFWorkbook(fs);  
  
                    if (workbook != null)  
                    {  
                        sheet = workbook.GetSheetAt(0);//读取第一个sheet，当然也可以循环读取每个sheet  
                        dataTable = new DataTable();  
                        if (sheet != null)  
                        {  
                            int rowCount = sheet.LastRowNum;//总行数  
                            if (rowCount > 0)  
                            {  
                                IRow firstRow = sheet.GetRow(0);//第一行  
                                int cellCount = firstRow.LastCellNum;//列数  
  
                                //构建datatable的列  
                                if (isColumnName)  
                                {  
                                    startRow = 1;//如果第一行是列名，则从第二行开始读取  
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)  
                                    {  
                                        cell = firstRow.GetCell(i);  
                                        if (cell != null)  
                                        {  
                                            if (cell.StringCellValue != null)  
                                            {  
                                                column = new DataColumn(cell.StringCellValue);  
                                                dataTable.Columns.Add(column);  
                                            }  
                                        }  
                                    }  
                                }  
                                else  
                                {  
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)  
                                    {  
                                        column = new DataColumn("column" + (i + 1));  
                                        dataTable.Columns.Add(column);  
                                    }  
                                }  
  
                                //填充行  
                                for (int i = startRow; i <= rowCount; ++i)  
                                {  
                                    row = sheet.GetRow(i);  
                                    if (row == null) continue;  
  
                                    dataRow = dataTable.NewRow();  
                                    for (int j = row.FirstCellNum; j < cellCount; ++j)  
                                    {  
                                        cell = row.GetCell(j);                                          
                                        if (cell == null)  
                                        {  
                                            dataRow[j] = "";  
                                        }  
                                        else  
                                        {  
                                            //CellType(Unknown = -1,Numeric = 0,String = 1,Formula = 2,Blank = 3,Boolean = 4,Error = 5,)  
                                            switch (cell.CellType)  
                                            {  
                                                case CellType.Blank:  
                                                    dataRow[j] = "";  
                                                    break;  
                                                case CellType.Numeric:  
                                                    short format = cell.CellStyle.DataFormat;  
                                                    //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理  
                                                    if (format == 14 || format == 31 || format == 57 || format == 58)  
                                                        dataRow[j] = cell.DateCellValue;  
                                                    else  
                                                        dataRow[j] = cell.NumericCellValue;  
                                                    break;  
                                                case CellType.String:  
                                                    dataRow[j] = cell.StringCellValue;  
                                                    break;  
                                            }  
                                        }  
                                    }  
                                    dataTable.Rows.Add(dataRow);  
                                }  
                            }  
                        }  
                    }  
                }  
                return dataTable;  
            }  
            catch (Exception)  
            {  
                if (fs != null)  
                {  
                    fs.Close();  
                }  
                return null;  
            }  
        }  
    }
}
