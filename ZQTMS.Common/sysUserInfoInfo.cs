//------------------------------------------------
// File Name:sysUserInfoInfo.cs
// File Description:sysUserInfo Model Entity Class
// Author:蓝桥软件
// Create Time:2016-07-22 09:30:42
//------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using ZQTMS.Tool;

namespace ZQTMS.Common
{
    /// <summary>
    /// 描述： 用户表
    /// </summary>
    [Serializable, Table(TableName = "sysUserInfo", Description = "用户表", Identity = "", Primary = "UserId", Unique = "UserId")]
    public class sysUserInfoInfo
    {
        #region 私有属性
        private Guid _userId;
        private string _userAccount;
        private string _userPsw;
        private string _userName;
        private string _siteName;
        private string _webName;
        private string _causeName;
        private string _areaName;
        private string _departName;
        private string _gRCode;
        private string _gRName;
        private int _userState;
        private DateTime _lastLogTime;
        private string _lastLogLANIp;
        private string _lastLogWANIp;
        private string _userCupName;
        string _loginSiteCode;
        string _loginWebCode;
        string _WebRole;
        UserDB _UserDB;
        decimal _Discount;
        string _companyid;
        string _gsqc;
        //zaj 2017-11-23
        string _labelName;
        string _envelopeName;
        bool _isAutoBill;
        bool _isLimitMonthPay;
        string _transprotocol;
        string _departList;
        string _loadList;
        string _shortConList;
        string _bookNote;
        string _MiddleList;
        private string _token;
        #endregion

        #region 构造方法
        /// <summary>
        /// sysUserInfo 构造方法
        /// </summary>
        public sysUserInfoInfo()
        {
        }

        /// <summary>
        /// sysUserInfo 构造方法
        /// </summary>
        ///<param name="userId">UserId  唯一ID标识</param>
        ///<param name="userAccount">UserAccount  登录账号</param>
        ///<param name="userPsw">UserPsw  密码</param>
        ///<param name="userName">UserName  姓名</param>
        ///<param name="siteName">SiteName  隶属站点</param>
        ///<param name="webName">WebName  隶属网点</param>
        ///<param name="causeName">CauseName  隶属事业部</param>
        ///<param name="areaName">AreaName  隶属大区</param>
        ///<param name="departName">DepartName  隶属部门</param>
        ///<param name="gRName">GRCode  权限组编号（多选）</param>
        ///<param name="gRName">GRName  隶属权限组（多选）</param>
        ///<param name="userState">UserState  状态（0:启用、1停用）</param>
        ///<param name="lastLogTime">lastLogTime  最后登录时间</param>
        ///<param name="lastLogLANIp">LastLogLANIp  最后登录局域网IP</param>
        ///<param name="lastLogWANIp">LastLogWANIp  最后登录广域网IP</param>
        ///<param name="userCupName">UserCupName  电脑名称</param>
        public sysUserInfoInfo(Guid userId, string userAccount, string userPsw, string userName, string siteName, string webName, string causeName, string areaName, string departName, string gRCode, string gRName, int userState, DateTime lastLogTime, string lastLogLANIp, string lastLogWANIp, string userCupName, string loginSiteCode, string loginWebCode, string companyid, string token)
        {
            this._userId = userId;
            this._userAccount = userAccount;
            this._userPsw = userPsw;
            this._userName = userName;
            this._siteName = siteName;
            this._webName = webName;
            this._causeName = causeName;
            this._areaName = areaName;
            this._departName = departName;
            this._gRCode = gRCode;
            this._gRName = gRName;
            this._userState = userState;
            this._lastLogTime = lastLogTime;
            this._lastLogLANIp = lastLogLANIp;
            this._lastLogWANIp = lastLogWANIp;
            this._userCupName = userCupName;
            this._loginSiteCode = loginSiteCode;
            this._loginWebCode = loginWebCode;
            this._companyid = companyid;
            this._token = token;
        }
        #endregion

        #region 公有字段
        ///<Summary>
        /// UserId
        /// 唯一ID标识  默认值：(newid())
        ///</Summary>
        [TableColumn(IsIdentity = false, IsPrimary = true, IsUnique = true, Description = "唯一ID标识")]
        public Guid UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
            }
        }

        ///<Summary>
        /// UserAccount
        /// 登录账号 
        ///</Summary>
        [TableColumn(IsIdentity = false, IsPrimary = false, IsUnique = false, Description = "登录账号")]
        public string UserAccount
        {
            get
            {
                return _userAccount;
            }
            set
            {
                _userAccount = value;
            }
        }

        ///<Summary>
        /// UserPsw
        /// 密码 
        ///</Summary>
        [TableColumn(IsIdentity = false, IsPrimary = false, IsUnique = false, Description = "密码")]
        public string UserPsw
        {
            get
            {
                return _userPsw;
            }
            set
            {
                _userPsw = value;
            }
        }

        ///<Summary>
        /// UserName
        /// 姓名 
        ///</Summary>
        [TableColumn(IsIdentity = false, IsPrimary = false, IsUnique = false, Description = "姓名")]
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        ///<Summary>
        /// SiteName
        /// 隶属站点 
        ///</Summary>
        [TableColumn(IsIdentity = false, IsPrimary = false, IsUnique = false, Description = "隶属站点")]
        public string SiteName
        {
            get
            {
                return _siteName;
            }
            set
            {
                _siteName = value;
            }
        }

        ///<Summary>
        /// WebName
        /// 隶属网点 
        ///</Summary>
        [TableColumn(IsIdentity = false, IsPrimary = false, IsUnique = false, Description = "隶属网点")]
        public string WebName
        {
            get
            {
                return _webName;
            }
            set
            {
                _webName = value;
            }
        }

        ///<Summary>
        /// CauseName
        /// 隶属事业部 
        ///</Summary>
        [TableColumn(IsIdentity = false, IsPrimary = false, IsUnique = false, Description = "隶属事业部")]
        public string CauseName
        {
            get
            {
                return _causeName;
            }
            set
            {
                _causeName = value;
            }
        }

        ///<Summary>
        /// AreaName
        /// 隶属大区 
        ///</Summary>
        [TableColumn(IsIdentity = false, IsPrimary = false, IsUnique = false, Description = "隶属大区")]
        public string AreaName
        {
            get
            {
                return _areaName;
            }
            set
            {
                _areaName = value;
            }
        }

        ///<Summary>
        /// DepartName
        /// 隶属部门 
        ///</Summary>
        [TableColumn(IsIdentity = false, IsPrimary = false, IsUnique = false, Description = "隶属部门")]
        public string DepartName
        {
            get
            {
                return _departName;
            }
            set
            {
                _departName = value;
            }
        }

        ///<Summary>
        /// GRCode
        /// 权限组编号（多选） 
        ///</Summary>
        [TableColumn(IsIdentity = false, IsPrimary = false, IsUnique = false, Description = "权限组编号")]
        public string GRCode
        {
            get
            {
                return _gRCode;
            }
            set
            {
                _gRCode = value;
            }
        }

        ///<Summary>
        /// GRName
        /// 隶属权限组（多选） 
        ///</Summary>
        [TableColumn(IsIdentity = false, IsPrimary = false, IsUnique = false, Description = "隶属权限组")]
        public string GRName
        {
            get
            {
                return _gRName;
            }
            set
            {
                _gRName = value;
            }
        }

        ///<Summary>
        /// UserState
        /// 状态（0:启用、1停用） 
        ///</Summary>
        [TableColumn(IsIdentity = false, IsPrimary = false, IsUnique = false, Description = "状态")]
        public int UserState
        {
            get
            {
                return _userState;
            }
            set
            {
                _userState = value;
            }
        }

        ///<Summary>
        /// lastLogTime
        /// 最后登录时间 
        ///</Summary>
        [TableColumn(IsIdentity = false, IsPrimary = false, IsUnique = false, Description = "最后登录时间")]
        public DateTime lastLogTime
        {
            get
            {
                return _lastLogTime;
            }
            set
            {
                _lastLogTime = value;
            }
        }

        ///<Summary>
        /// LastLogLANIp
        /// 最后登录局域网IP 
        ///</Summary>
        [TableColumn(IsIdentity = false, IsPrimary = false, IsUnique = false, Description = "最后登录局域网IP")]
        public string LastLogLANIp
        {
            get
            {
                return _lastLogLANIp;
            }
            set
            {
                _lastLogLANIp = value;
            }
        }

        ///<Summary>
        /// LastLogWANIp
        /// 最后登录广域网IP 
        ///</Summary>
        [TableColumn(IsIdentity = false, IsPrimary = false, IsUnique = false, Description = "最后登录广域网IP")]
        public string LastLogWANIp
        {
            get
            {
                return _lastLogWANIp;
            }
            set
            {
                _lastLogWANIp = value;
            }
        }

        ///<Summary>
        /// UserCupName
        /// 电脑名称 
        ///</Summary>
        [TableColumn(IsIdentity = false, IsPrimary = false, IsUnique = false, Description = "电脑名称")]
        public string UserCupName
        {
            get
            {
                return _userCupName;
            }
            set
            {
                _userCupName = value;
            }
        }

        /// <summary>
        /// 获取或设置登录站点代码
        /// </summary>
        public string LoginSiteCode
        {
            get { return _loginSiteCode; }
            set { _loginSiteCode = value; }
        }

        /// <summary>
        /// 获取或设置登录网点代码
        /// </summary>
        public string LoginWebCode
        {
            get { return _loginWebCode; }
            set { _loginWebCode = value; }
        }

        /// <summary>
        /// 获取用户角色
        /// </summary>
        public string WebRole
        {
            get { return _WebRole; }
            set { _WebRole = value; }
        }

        /// <summary>
        /// 登录的目标数据库
        /// </summary>
        public UserDB UserDB
        {
            get { return _UserDB; }
            set { _UserDB = value; }
        }

        /// <summary>
        /// 用户折扣
        /// </summary>
        public decimal Discount
        {
            get { return _Discount; }
            set { _Discount = value; }
        }

        public string companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        public string gsqc
        {
            get { return _gsqc; }
            set { _gsqc = value; }
        }
        //zaj
        /// <summary>
        /// 标签名
        /// </summary>
        public string LabelName
        {
            get { return _labelName; }
            set { _labelName = value; }
        }
        /// <summary>
        /// 信封名
        /// </summary>
        public string EnvelopeName
        {
            get { return _envelopeName; }
            set { _envelopeName = value; }
        }
        /// <summary>
        /// 是否自动运单
        /// </summary>
        public bool IsAutoBill
        {
            get { return _isAutoBill; }
            set { _isAutoBill = value; }
        }
 
        public bool IsLimitMonthPay
        {
            get { return _isLimitMonthPay; }
            set { _isLimitMonthPay = value; }
        }
        /// <summary>
        /// 运输协议
        /// </summary>
        public string Transprotocol
        {
            get { return _transprotocol; }
            set { _transprotocol = value; }
        }

        /// <summary>
        /// 配载清单
        /// </summary>
        public string DepartList
        {
            get { return _departList; }
            set { _departList = value; }
        }

        /// <summary>
        /// 装车清单
        /// </summary>
        public string LoadList
        {
            get { return _loadList; }
            set { _loadList = value; }
        }

        /// <summary>
        /// 短驳清单
        /// </summary>
        public string ShortConList
        {
            get { return _shortConList; }
            set { _shortConList = value; }
        }

        /// <summary>
        /// 托运单
        /// </summary>
        public string BookNote
        {
            get { return _bookNote; }
            set { _bookNote = value; }
        }


        /// <summary>
        /// 中转清单
        /// </summary>
        public string MiddleList  //maohui20180324
        {
            get { return _MiddleList; }
            set { _MiddleList = value; }
        }

        /// <summary>
        /// 公司对应E3PLtoken
        /// </summary>
        public string token
        {
            get { return _token; }
            set { _token = value; }
        }
        #endregion
    }

    /// <summary>
    /// 枚举值来自 CommonClass.GetDatabaseInfo的db字段
    /// </summary>
    public enum UserDB
    {
        ZQTMS20160713 = 1,
        ZQTMS3PL = 2,
        ZQDB20201111 = 3
    }
}
