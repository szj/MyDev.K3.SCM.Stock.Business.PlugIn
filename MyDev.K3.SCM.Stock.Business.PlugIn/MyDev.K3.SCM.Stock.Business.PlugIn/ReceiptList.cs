using Kingdee.BOS.Core.Bill;
using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Core.DynamicForm.Operation;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.List;
using Kingdee.BOS.Core.List.PlugIn;
using Kingdee.BOS.Core.List.PlugIn.Args;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Core.Metadata.ConvertElement;
using Kingdee.BOS.Core.Metadata.ConvertElement.ServiceArgs;
using Kingdee.BOS.Core.SqlBuilder;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Resource;
using Kingdee.BOS.ServiceHelper;
using Kingdee.K3.SCM.ServiceHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDev.K3.SCM.Stock.Business.PlugIn
{
    [Description("采购List测试插件")]
    class ReceiptList : AbstractListPlugIn
    {

        private string callSys = "";
        private bool isEnableReserve;
        public override void OnInitialize(InitializeEventArgs e)
        {
            object systemProfile = CommonServiceHelper.GetSystemProfile(base.Context, 0L, "MFG_PLNParam", "IsEnableReserve", false);
            this.isEnableReserve = Convert.ToBoolean(systemProfile);
        }

        public override void AfterBindData(EventArgs e)
        {
            this.ListView.GetMainBarItem("tbReserveLinkQuery").Visible = this.isEnableReserve;
        }
        public override void BarItemClick(BarItemClickEventArgs e)
        {
            string text2 = "PUR_ReceiveBill";
            string text3 = "PUR_MRB";
            ListSelectedRow[] array2 = new ListSelectedRow[1];
            List<ConvertRuleElement> convertRules = ConvertServiceHelper.GetConvertRules(this.View.Context, text2, text3);
            ConvertRuleElement convertRuleElement = convertRules.FirstOrDefault((ConvertRuleElement t) => t.IsDefault);
            ListViewPlugInProxy listViewPlugInProxy = this.View.GetService<DynamicFormViewPlugInProxy>() as ListViewPlugInProxy;
            if (listViewPlugInProxy != null)
            {
                GetConvertRuleEventArgs getConvertRuleEventArgs = new GetConvertRuleEventArgs(text2, text3, FormOperationEnum.Push, convertRules, convertRuleElement);
                listViewPlugInProxy.FireOnGetConvertRule(getConvertRuleEventArgs);
                convertRuleElement = (getConvertRuleEventArgs.Rule as ConvertRuleElement);
            }
            if (convertRuleElement == null)
            {
                this.View.ShowMessage(ResManager.LoadKDString("规则不存在或者没有指定“默认”转换规则！", "004015030000058", SubSystemType.SCM, new object[0]), MessageBoxType.Notice);
                e.Cancel = true;
                return;
            }
            ConvertOperationResult convertOperationResult = ConvertServiceHelper.Push(base.Context, new PushArgs(convertRuleElement, array2), null);
            DynamicObject[] objs = (
                from p in convertOperationResult.TargetDataEntities
                select p.DataEntity).ToArray<DynamicObject>();
            this.ShowResultInTabPage(objs, text3);
        }


        private void ShowResultInTabPage(DynamicObject[] objs, string targetFormId)
        {
            BillShowParameter billShowParameter = new BillShowParameter();
            billShowParameter.Status = OperationStatus.ADDNEW;
            if (objs.Count<DynamicObject>() == 1)
            {
                string key = "_ConvertSessionKey";
                string text = "ConverOneResult";
                billShowParameter.CustomParams.Add(key, text);
                this.View.Session[text] = objs[0];
                billShowParameter.FormId = targetFormId;
            }
            else
            {
                if (objs.Count<DynamicObject>() > 1)
                {
                    string key2 = "_ConvertResultsSessionKey";
                    string text2 = "ConvertResults";
                    billShowParameter.CustomParams.Add(key2, text2);
                    billShowParameter.CustomParams.Add("_ConvertResultFormId", targetFormId);
                    billShowParameter.FormId = "BOS_TargetBillsForm";
                    this.View.Session[text2] = objs;
                }
            }
            this.View.ShowForm(billShowParameter);
        }


        /// <summary>
        /// 对列表数据追加过滤或是排序，推荐通过过滤方案进行处理，如果是特殊的强制过滤，可以在这个位置进行处理
        /// </summary>
        /// <param name="e"></param>
        public override void PrepareFilterParameter(FilterArgs e)
        {
            e.AppendQueryFilter("");
            e.AppendQueryOrderby("");
        }

        /// <summary>
        /// queryservice取数方案，通过业务对象来获取数据，推荐使用
        /// </summary>
        /// <returns></returns>
        public DynamicObjectCollection GetQueryDatas()
        {
            QueryBuilderParemeter paramCatalog = new QueryBuilderParemeter()
            {
                FormId = "",//取数的业务对象
                FilterClauseWihtKey = "",//过滤条件，通过业务对象的字段Key拼装过滤条件
                SelectItems = SelectorItemInfo.CreateItems("", "", ""),//要筛选的字段【业务对象的字段Key】，可以多个，如果要取主键，使用主键名
            };

            DynamicObjectCollection dyDatas = Kingdee.BOS.ServiceHelper.QueryServiceHelper.GetDynamicObjectCollection(this.Context, paramCatalog);
            return dyDatas;
        }

    }
}
