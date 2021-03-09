using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace ZQTMS.Update
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length == 0) return;
            CheckInstance("ZQTMS系统更新");//检查程序是否运行多实例

            string msg = args[0];
            string[] arr = msg.Split(new string[] { "@##*@" }, StringSplitOptions.None);
            if (arr.Length < 6)
            {
                MessageBox.Show("参数指定错误!");
                return;
            }

            string Json = arr[0];
            string Httpuser = arr[1];//Http下载账号
            string Httppwd = arr[2]; //Http下载密码
            string user = arr[3];    //系统登录账号
            string pwd = arr[4];     //系统登录账号
            string MainName = arr[5];//主程序名字
            string Mac = arr[6];//mac
            Application.Run(new FrmDownLoad(Json, Httpuser, Httppwd, user, pwd, MainName,Mac));

            //else
            //{
            //    Application.Run(new FrmDownLoad(null, null, null, null, null, null));
            //}
            //Application.Run(new FrmDownLoad("", "", "", "", "", ""));
        }


        /// <summary>
        ///检查程序是否运行多实例
        /// </summary>
        private static void CheckInstance(string name)
        {
            Boolean createdNew; //返回是否赋予了使用线程的互斥体初始所属权
            Mutex instance = new Mutex(true, name, out createdNew); //同步基元变量
            if (createdNew) //首次使用互斥体
            {
                instance.ReleaseMutex();
            }
            else
            {
                //"已经启动了一个程序，请先退出！"
                Application.Exit();
            }
        }

    }
}
