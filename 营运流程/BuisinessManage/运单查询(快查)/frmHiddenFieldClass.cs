using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
namespace ZQTMS.UI
{
    public enum MenuID
    { 
        
        快找界面,
        [Description("9ae5cabc-9cf0-43d9-86d0-1ddc64bca6f1")]
        快找运单接货信息,
        [Description("7929b1b8-2f35-4b81-8631-a0a944d46363")]
        快找运单短驳信息,
        [Description("1b8d2c18-73b3-44a4-8f64-6d30a3c1f843")]
        快找运单配载信息,
        [Description("de54f341-5a31-4cfb-8a00-3fbbacb8cb83")]
        快找运单送货信息,
        [Description("a0e02030-7f5b-4a8c-9e81-048fa1dfdaab")]
        快找运单费用信息,
        [Description("4d41d99d-e8ba-40cf-9b9a-eee98369bfd9")]
        快找运单改单记录,
        [Description("5bdcb703-6916-4b37-8476-909ee52e8c36")]
        快找运单结算账户明细,
        [Description("3a1be613-0612-4183-9965-53a681e417fa")]
        快找运单增补调整,
        [Description("62bcbad9-cf77-4219-b1c0-bdb8f90ed426")]
        快找运单开单结算信息,
        [Description("40664fc9-25be-4de2-a3da-59959af2af40")]
        快找运单预约送货,
        [Description("dcd62ba4-a228-4be4-913f-81b4d16eaca0")]
        短驳出库新增短驳,
        [Description("069253ec-88be-4524-aa03-d9e6a3718a63")]
        短驳到货短驳明细,
        [Description("bc94548d-a75d-4ee2-a778-ae06bbb414fb")]
        在途与到货车辆,
        [Description("a682cdc8-abe1-4dca-ba46-3b208683eccc")]
        到货接驳明细,
        [Description("bdbcd5ff-190c-41da-84d4-7b6ce57680ab")]
        到货库存,
        [Description("93536a1a-0638-4f90-9672-e95fb6895227")]
        到货记录,
        [Description("4691638a-ad02-4d13-b9cc-2b88b52ce7be")]
        到货通知记录,
        [Description("e030164a-52ee-4229-a8ec-00bb37d7be22")]
        终端中转,
        [Description("53f58e31-4ef7-4450-a4a6-ba442902c289")]
        终端中转单票库存,
        [Description("1c5d4c79-465f-4ad1-8e62-f1a215301eb6")]
        终端中转批量库存,
        [Description("f615e37b-8547-49c5-861d-ed19001c12d9")]
        送货上门按票,
        [Description("f0e24587-720e-471a-b996-8f1350992d1b")]
        送货上门按车,
        [Description("ca0a9f29-67f7-43eb-aa12-a48c8a35cdb4")]
        安排送货提取库存,
        [Description("48ca081c-bfd8-432f-a38d-b0b2e8474a26")]
        安排送货送货清单,
        [Description("3c66e455-f22b-4daa-bd68-5a7b93102abf")]
        签收记录,
        [Description("a772a70d-6c7b-4d68-8bd2-79ac34733c3c")]
        签收录入提取库存,
        [Description("a325b68b-dc9b-4565-8e84-c6ea21f95765")]
        回单总账,
        [Description("8692a9b5-1cc5-4e47-a8cc-059da520f37c")]
        回单寄出,
        [Description("14ba9bbf-d585-4e6c-bc69-a71d546de090")]
        回单返回,
        [Description("1e4f9df0-15c8-421d-beea-59155f62d80e")]
        回单返厂,
        [Description("14ba9bbf-d585-4e6c-bc69-a71d546de090")]
        客户取单
    }

    class frmHiddenFieldClass
    {
        private Dictionary<string, string> dic = new Dictionary<string, string>();
        public frmHiddenFieldClass()
        {
             
           
            

        }
    }



}
