using System;
using System.Collections.Generic;
using System.Text;

namespace ZQTMS.Common
{
    /// <summary>
    /// 系统参数设置的实体类
    /// </summary>
    public class SysArgs
    {
        /// <summary>
        /// 交接方式
        /// </summary>
        public string TransferMode { set; get; }
        /// <summary>
        /// 运输方式
        /// </summary>
        public string TransitMode { set; get; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public string PaymentMode { set; get; }
        /// <summary>
        /// 品名
        /// </summary>
        public string Varieties { set; get; }
        /// <summary>
        /// 包装
        /// </summary>
        public string Packag { set; get; }
        /// <summary>
        /// 代收货款手续费率
        /// </summary>
        public string CommissionRate { set; get; }
        /// <summary>
        /// 是否外租
        /// </summary>
        public string IsRent { set; get; }
        /// <summary>
        /// 受理部门
        /// </summary>
        public string AcceptDepart { set; get; }
        /// <summary>
        /// 结算状态
        /// </summary>
        public string SettleState { set; get; }
        /// <summary>
        /// 车型
        /// </summary>
        public string VehicleType { set; get; }
        /// <summary>
        /// 车长
        /// </summary>
        public string VehicleLength { set; get; }
        /// <summary>
        /// 回单操作情况
        /// </summary>
        public string OperateState { set; get; }
        /// <summary>
        /// 回单返回情况
        /// </summary>
        public string ReceiptBackState { set; get; }
        /// <summary>
        /// 回单总账查询类型
        /// </summary>
        public string ReceiptSelectState { set; get; }
        /// <summary>
        /// 回单取消查询类型
        /// </summary>
        public string ReceiptCancelSelectType { set; get; }
        /// <summary>
        /// 派车状态
        /// </summary>
        public string DeliveryState { set; get; }
        /// <summary>
        /// 接货方式
        /// </summary>
        public string ReceivingWay { set; get; }
        /// <summary>
        /// 回单要求
        /// </summary>
        public string ReceiptRequir { set; get; }
        /// <summary>
        /// 折算重量
        /// </summary>
        public string ReducedWeight { set; get; }
        /// <summary>
        /// 货物类型
        /// </summary>
        public string GoodsType { set; get; }
        /// <summary>
        /// 客户类型
        /// </summary>
        public string CustomType { set; get; }
        /// <summary>
        /// 计价类型
        /// </summary>
        public string DenominatedType { set; get; }
        /// <summary>
        /// 常发品名
        /// </summary>
        public string OfenVarieties { set; get; }
        /// <summary>
        /// 配载要求
        /// </summary>
        public string StowagePlan { set; get; }
        /// <summary>
        /// 送货要求
        /// </summary>
        public string SendRequir { set; get; }
        /// <summary>
        /// 中转要求
        /// </summary>
        public string MiddleRequir { set; get; }
        /// <summary>
        /// 车辆合作形式
        /// </summary>
        public string CarCooperation { set; get; }
        /// <summary>
        /// 始发其他费项目
        /// </summary>
        public string ProjectOFHost { set; get; }
        /// <summary>
        /// 终端其他费项目
        /// </summary>
        public string ProjectOend { set; get; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayMode { set; get; }

        /// <summary>
        /// 客户标识
        /// </summary>
        public string CustomTag { set; get; }
        /// <summary>
        /// 收支类型
        /// </summary>
        public string InOutType { set; get; }
        /// <summary>
        /// 锁账日期
        /// </summary>
        public string LockDate { set; get; }

        /// <summary>
        /// 扣返其它费收入
        /// </summary>
        public string TotalOtherAcc { set; get; }

        /// <summary>
        /// 异动款项收入
        /// </summary>
        public string TotalTransaction { set; get; }

        /// <summary>
        /// 异动款项支出
        /// </summary>
        public string TotalOtherAccOut { set; get; }

        /// <summary>
        /// 异动款项支出
        /// </summary>
        public string TotalTransactionOut { set; get; }
        /// <summary>
        /// 终端分拨费返款最低一票
        /// </summary>
        public string BackFeeLessPay { set; get; }
        /// <summary>
        /// 终端分拨费返款费率
        /// </summary>
        public string BackFeeRate { set; get; }

        /// <summary>
        /// 不受账款管控
        /// </summary>
        public string ContractCheck { set; get; }
        /// <summary>
        /// 补扣费用类型
        /// </summary>
        public string BuKouFeeType { set; get; }
        /// <summary>
        /// 核销方向定义
        /// </summary>
        public string VerifyDirection { set; get; }

        /// <summary>
        /// 中心直送自提免费网点
        /// </summary>
        public string PickUpFreeWeb { get; set; }

        /// <summary>
        /// 汽运锁账
        /// </summary>
        public string qysj { get; set; }

        /// <summary>
        /// 改单锁定
        /// zxw 20170826
        /// </summary>
        public string ChangeBillControl { get; set; }

        /// <summary>
        /// 加油站
        /// </summary>
        public string gasStation { get; set; }



        /// <summary>
        /// 开单保价费最低一票
        /// zb20190828
        /// </summary>
        public string SupportValueLowest { get; set; }

        /// <summary>
        /// 开单保价费费率
        /// </summary>
        public string SupportValueRate { get; set; }

        /// <summary>
        /// 送货不用填街道网点
        /// </summary>
        public string NotStreet{ get; set; }



    }
}
