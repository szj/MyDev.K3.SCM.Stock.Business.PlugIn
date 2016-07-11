using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core.Bill;
using Kingdee.BOS.Core.CommonFilter;
using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Core.Metadata.EntityElement;
using Kingdee.BOS.Core.Metadata.FieldElement;
using Kingdee.BOS.Core.Permission;
using Kingdee.BOS.Core.SqlBuilder;
using Kingdee.BOS.Core.SystemParameter;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Resource;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Util;
using Kingdee.K3.SCM.ServiceHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MyDev.K3.SCM.Stock.Business.PlugIn
{
   [Description("采购Bill测试插件")]
    public class ReceiptEdit : Kingdee.BOS.Core.Bill.PlugIn.AbstractBillPlugIn
    {


        public static void OpenPurSystemParam(IDynamicFormView view)
        {
            SystemParameterShowParameter systemParameterShowParameter = new SystemParameterShowParameter();
            systemParameterShowParameter.PermissionItemId = "f323992d896745fbaab4a2717c79ce2e";
            systemParameterShowParameter.Status = OperationStatus.EDIT;
            systemParameterShowParameter.ShowMaxButton = true;
            systemParameterShowParameter.Resizable = true;
            systemParameterShowParameter.SubSystemId = "20";
            systemParameterShowParameter.OpenStyle.ShowType = ShowType.Floating;
            systemParameterShowParameter.FormId = "BOS_ParameterSetBase";
            systemParameterShowParameter.ObjectTypeId = "PUR_SystemParameter";
            systemParameterShowParameter.CreateWebParams();
            view.ShowForm(systemParameterShowParameter);
        }

        //当前操作是否修改
        public bool IsModificationOperator
        {
            get
            {
                return Convert.ToBoolean(base.View.Model.GetValue("FIsModificationOperator"));
            }
        }


        public static void CodePoints(String title, String s)
        {
            Console.Write("{0}The code points in {1} are: {0}", Environment.NewLine, title);
            foreach (ushort u in s)
                Console.Write("{0:x4} ", u);
            Console.WriteLine();
        }

        /// <summary>
        /// 初始化，对其他界面传来的参数进行处理，对控件某些属性进行处理
        /// 这里不宜对数据DataModel进行处理
        /// </summary>
        /// <param name="e"></param>
        public override void BarItemClick(BarItemClickEventArgs e)
        {
            base.BarItemClick(e);
            //弹窗
            OpenPurSystemParam(View);
            //供应商
            String supplierName = "供应商名称";
            View.ShowForm(new BillShowParameter
            {
                PermissionItemId = "fce8b1aca2144beeb3c6655eaf78bc34",
                FormId = "BD_Supplier",
                CustomComplexParams =
                {

                    {
                        "SupplierName",
                        supplierName
                    }
                }
            });

            //设置按钮
            base.View.GetMainBarItem("tbIssueQuery").Visible = base.Context.IsMultiOrg;

            Convert.ToBoolean(base.View.Model.GetValue("FIsModificationOperator"));

            //系统参数
            object systemProfile = CommonServiceHelper.GetSystemProfile(base.Context, 0L, "MFG_PLNParam", "IsEnableReserve", false);
            //动态窗体
            FormMetadata formMetadata = MetaDataServiceHelper.Load(base.View.Context, "PUR_USERORDERPARAM", true) as FormMetadata;

            DynamicObject dynamicObject = UserParamterServiceHelper.Load(base.View.Context, formMetadata.BusinessInfo, base.View.Context.UserId, "PUR_PurchaseOrder", "UserParameter");
            if (dynamicObject != null && dynamicObject.DynamicObjectType.Properties.ContainsKey("FiltBySupplyList") && dynamicObject["FiltBySupplyList"] != null && !string.IsNullOrWhiteSpace(dynamicObject["FiltBySupplyList"].ToString()))
            {
                Boolean IsFiltByCatalogList = Convert.ToBoolean(dynamicObject["FiltBySupplyList"]);
            }

            this.Model.DeleteEntryData("FIinstallment");
            //分录删除
            base.View.Model.DeleteEntryRow("FPOOrderEntry", 1);
            // New GUID
            SequentialGuid.NewGuid().ToString();
            //==================提示信息处理========================
            base.View.ShowWarnningMessage(ResManager.LoadKDString("反审核采购订单会清除预付信息，是否继续？", "004011000014814", SubSystemType.SCM, new object[0]), "", MessageBoxOptions.OKCancel, delegate (MessageBoxResult result)
            {
                if (MessageBoxResult.OK == result)
                {
                    //this.isShowMsgForCheckPrePay = true;
                    base.View.InvokeFormOperation(FormOperationEnum.UnAudit);
                }
            }, MessageBoxType.Advise);

            base.View.ShowErrMessage("YOU ERRo", "", MessageBoxType.Notice);
            e.Cancel = true;

            //===============================
            if (base.View.Model.GetEntryRowCount("FPOOrderEntry") <= 0) {
            }
            //===
            if (this.IsModificationOperator) {

            }
            //=============权限================
            PermissionAuthResult permissionAuthResult = PermissionServiceHelper.FuncPermissionAuth(base.Context, new BusinessObject
            {
                Id = "AP_PAYBILL"
            }, "6e44119a58cb4a8e86f6c385e14a17ad");
            if (!permissionAuthResult.Passed)
            {
                base.View.ShowMessage(ResManager.LoadKDString("没有付款单的查看权限，不能查询！", "004015030001552", SubSystemType.SCM, new object[0]), MessageBoxType.Notice);
                e.Cancel = true;
                return;
            }
            //刷新
            base.View.ParentFormView.Refresh();
            //====分录数据 物料处理===========
            Entity entity = base.View.Model.BillBusinessInfo.GetEntity("FPOOrderEntry");
            DynamicObjectCollection entityDataObject = View.Model.GetEntityDataObject(entity);


            //====demo1================
            String str1 = "indigo";
            String str2, str3;

            str2 = str1.ToUpper(new CultureInfo("en-US", false));
            str3 = str1.ToUpper(new CultureInfo("tr-TR", false));

            CodePoints("str1", str1);
            CodePoints("str2", str2);
            CodePoints("str3", str3);

            Console.WriteLine(str1+";"+ str2+";"+ str3);
            //======demo2========================
            if (e.BarItemKey == "btnTest")
            {
                View.ShowMessage("in the rain!", MessageBoxType.Notice);
            }
        }


        /// <summary>
        /// 初始化，对其他界面传来的参数进行处理，对控件某些属性进行处理
        /// 这里不宜对数据DataModel进行处理
        /// </summary>
        /// <param name="e"></param>
        public override void OnInitialize(InitializeEventArgs e)
        {
            Console.Write("hello ");
        }

        /// <summary>
        /// 新建单据加载数据完成之后，需要处理的功能
        /// </summary>
        /// <param name="e"></param>
        public override void AfterCreateNewData(EventArgs e)
        {
            base.AfterCreateNewData(e);
        }

        /// <summary>
        /// 修改，查看单据加载已有数据之后，需要处理的功能
        /// </summary>
        /// <param name="e"></param>
        public override void AfterLoadData(EventArgs e)
        {
            base.AfterLoadData(e);
        }

        /// <summary>
        /// 数据加载之后，需要处理的功能，这里主要对界面样式进行处理，尽量不要对Datamodel进行处理
        /// </summary>
        /// <param name="e"></param>
        public override void AfterBindData(EventArgs e)
        {
            base.AfterBindData(e);
        }


        /// <summary>
        /// 在根据编码检索数据之前调用；
        /// 通过重载本事件，可以设置必要的过滤条件，以限定检索范围；
        /// 还可以控制当前过滤是否启用组织隔离，数据状态隔离
        /// </summary>
        /// <param name="e"></param>
        public override void BeforeSetItemValueByNumber(BeforeSetItemValueByNumberArgs e)
        {
            base.BeforeSetItemValueByNumber(e);
            switch (e.BaseDataField.Key.ToUpperInvariant())
            {
                //case "FXXX":通过字段的Key[大写]来区分不同的基础资料
                //e.Filter = "FXXX= AND fxxy=";过滤的字段使用对应基础资料的字段的Key，支持ksql语法
                //break;
                case "":
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 显示基础资料列表之前调用
        /// 通过重载本事件，可以设置必要的过滤条件，以限定检索范围；
        /// </summary>
        /// <param name="e"></param>
        public override void BeforeF7Select(BeforeF7SelectEventArgs e)
        {
            switch (e.FieldKey.ToUpperInvariant())
            {
                //case "FXXX":通过字段的Key[大写]来区分不同的基础资料
                //    e.ListFilterParameter.Filter = "FXXX= AND fxxy=";过滤的字段使用对应基础资料的字段的Key，支持ksql语法
                //break;
                case "":
                    break;

                case "FFOCUSSETTLEORGID":
                    {
                        DynamicObject dynamicObject = base.View.Model.GetValue("FPurchaseOrgId") as DynamicObject;
                        if (dynamicObject == null)
                        {
                            e.Cancel = true;
                            base.View.ShowErrMessage(ResManager.LoadKDString("请先选择采购组织", "004011000019435", SubSystemType.SCM, new object[0]), "", MessageBoxType.Notice);
                            return;
                        }
                        long sourceOrgId = Convert.ToInt64(dynamicObject["id"]);
                        object[] orgByBizRelationship = OrganizationServiceHelper.GetOrgByBizRelationship(Context, sourceOrgId, 105L, true, false);
                        if (orgByBizRelationship != null && orgByBizRelationship.Length > 0)
                        {
                            string text = string.Format(" forgid in ({0}) ", string.Join(",", orgByBizRelationship));
                            if (string.IsNullOrEmpty(e.ListFilterParameter.Filter))
                            {
                                e.ListFilterParameter.Filter = text;
                            }
                            else
                            {
                                IRegularFilterParameter expr_3F5 = e.ListFilterParameter;
                                expr_3F5.Filter = expr_3F5.Filter + " AND " + text;
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// 界面数据发生变化之前，需要处理的功能
        /// </summary>
        /// <param name="e"></param>
        public override void BeforeUpdateValue(BeforeUpdateValueEventArgs e)
        {
            base.BeforeUpdateValue(e);
        }

        /// <summary>
        /// 界面数据发生变化之后，需要处理的功能
        /// </summary>
        /// <param name="e"></param>
        public override void DataChanged(DataChangedEventArgs e)
        {
            switch (e.Field.Key.ToUpperInvariant())
            {
                case "":
                    break;
                default:
                    break;
            }
        }


        private bool IsSrcFormId(string formID)
        {
            DynamicObjectCollection source = this.Model.DataObject["POOrderEntry"] as DynamicObjectCollection;
            return source.Any((DynamicObject p) => Convert.ToString(p["SrcBillTypeId"]).EqualsIgnoreCase(formID));
        }

        /// <summary>
        /// 单据持有事件发生前需要完成的功能
        /// </summary>
        /// <param name="e"></param>
        public override void BeforeDoOperation(BeforeDoOperationEventArgs e)
        {
            string key;
            switch (key=e.Operation.FormOperation.Operation.ToUpperInvariant())
            {
                case "SAVE": //表单定义的事件都可以在这里执行，需要通过事件的代码[大写]区分不同事件
                    
                    if (!ValidteQQNumber())
                    {
                        e.Cancel = true;//验证不通过
                    }
                    break;
                case "xx":
                    //源单类型比较
                    if (this.IsSrcFormId("SUB_SUBREQORDER"))
                    {
                        base.View.ShowMessage(ResManager.LoadKDString("委外订单生成的采购订单不允许新增分录。", "004011030006477", SubSystemType.SCM, new object[0]), MessageBoxType.Notice);
                        e.Cancel = true;
                    }
                   
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 验证QQ号的全法性
        /// </summary>
        /// <returns></returns>
        bool ValidteQQNumber()
        {  
            var qqNum = Model.GetValue("F_BAD_QQ") as string;
            if (string.IsNullOrWhiteSpace(qqNum))
            {
                this.View.ShowErrMessage("QQ号不能忘了填写，不然怎么约！");
                return false;
            }
            if (!IsNumber(qqNum))
            {
                this.View.ShowErrMessage("目前为止，还没见为不是数字的QQ号，如果小马哥确定要推出非数据的QQ号请第一时间告诉我！");
                return false;
            }
            return true;
        }
        public static bool IsNumber(string strNumber)
        {
            Regex regex = new Regex("[^0-9]");
            return !regex.IsMatch(strNumber);
        }

        /// <summary>
        /// 单据持有事件发生后需要完成的功能
        /// </summary>
        /// <param name="e"></param>
        public override void AfterDoOperation(AfterDoOperationEventArgs e)
        {
            switch (e.Operation.Operation.ToUpperInvariant())
            {
                //case "SAVE": 表单定义的事件都可以在这里执行，需要通过事件的代码[大写]区分不同事件
                //break;
                case "":
                    break;
                default:
                    break;
            }
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



        /// <summary>
        /// 物料检验
        /// </summary>
        /// <param name="e"></param>
        /// <param name="lPurchaseCatelog"></param>
        private void CheckMaterialExists(BarItemClickEventArgs e, long lPurchaseCatelog)
        {
            base.View.Model.GetEntryRowCount("FPOOrderEntry");
            Entity entity = base.View.Model.BillBusinessInfo.GetEntity("FPOOrderEntry");
            DynamicObjectCollection entityDataObject = base.View.Model.GetEntityDataObject(entity);
            StringBuilder stringBuilder = new StringBuilder();
            List<long> list = new List<long>();

            //查物料目录
            List<SelectorItemInfo> list2 = new List<SelectorItemInfo>();
            list2.Add(new SelectorItemInfo("FName"));
            list2.Add(new SelectorItemInfo("FMaterialId"));
            QueryBuilderParemeter para = new QueryBuilderParemeter
            {
                FormId = "PUR_CATALOG",
                FilterClauseWihtKey = string.Format("FID={0} ", lPurchaseCatelog),
                SelectItems = list2
            };
            DynamicObjectCollection dynamicObjectCollection = QueryServiceHelper.GetDynamicObjectCollection(base.Context, para, null);
            foreach (DynamicObject current in dynamicObjectCollection)
            {
                list.Add(Convert.ToInt64(current["FMaterialId"]));
            }
            //===========================


            foreach (DynamicObject current2 in entityDataObject)
            {
                if (current2 != null && !list.Contains(Convert.ToInt64(current2["MaterialId_Id"])))
                {
                    DynamicObject dynamicObject = current2["MaterialId"] as DynamicObject;
                    if (dynamicObject != null)
                    {
                        stringBuilder.AppendLine(string.Format(ResManager.LoadKDString("物料{0}不存在于采购目录的物料中！", "004011030000973", SubSystemType.SCM, new object[0]), dynamicObject["Name"]));
                    }
                }
            }
            if (stringBuilder.Length > 0)
            {
                base.View.ShowErrMessage(stringBuilder.ToString(), ResManager.LoadKDString("表体中录入的物料不存在于采购目录的物料中！", "004011030000976", SubSystemType.SCM, new object[0]), MessageBoxType.Notice);
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="lPurchaseCatelog"></param>
        private void CheckSupplierExists(BarItemClickEventArgs e, long lPurchaseCatelog)
        {
            long num = 0L;
            DynamicObject dynamicObject = base.View.Model.GetValue("FSupplierId") as DynamicObject;
            if (dynamicObject != null)
            {
                num = Convert.ToInt64(dynamicObject["Id"]);
            }
            List<SelectorItemInfo> list = new List<SelectorItemInfo>();
            list.Add(new SelectorItemInfo("FName"));
            QueryBuilderParemeter para = new QueryBuilderParemeter
            {
                FormId = "PUR_CATALOG",
                FilterClauseWihtKey = string.Format("FID={0} AND FSupplierId={1}", lPurchaseCatelog, num),
                SelectItems = list
            };
            DynamicObjectCollection dynamicObjectCollection = QueryServiceHelper.GetDynamicObjectCollection(base.Context, para, null);
            if (dynamicObjectCollection.Count <= 0)
            {
                base.View.ShowErrMessage(ResManager.LoadKDString("供应商不存在于采购目录的供应商中！", "004011030000970", SubSystemType.SCM, new object[0]), ResManager.LoadKDString("供应商不存在于采购目录的供应商中！", "004011030000970", SubSystemType.SCM, new object[0]), MessageBoxType.Notice);
                e.Cancel = true;
            }
        }



        private DynamicObjectCollection GetBaseDataDefaultValue(Field sfield)
        {
            LookUpObject lookUpObject = ((BaseDataField)sfield).LookUpObject;
            BaseDataField baseDataField = (BaseDataField)sfield;
            QueryBuilderParemeter param = new QueryBuilderParemeter
            {
                FormId = lookUpObject.FormId,
                SelectItems = SelectorItemInfo.CreateItems(new string[]
                {
                    baseDataField.LookUpObject.PkFieldName,
                    baseDataField.LookUpObject.NumberFieldName,
                    baseDataField.LookUpObject.NameFieldName
                }),
                OrderByClauseWihtKey = baseDataField.LookUpObject.NumberFieldName,
                IsShowApproved = true,
                IsShowUsed = true
            };
            return CommonServiceHelper.GetDynamicObjectCollection(base.Context, param, null);
        }


        private Field GetField(string keyName)
        {
            foreach (Entity current in base.View.Model.BillBusinessInfo.Entrys)
            {
                foreach (Field current2 in current.Fields)
                {
                    if (current2.Key.ToUpper() == keyName.ToUpper())
                    {
                        return current2;
                    }
                }
            }
            return null;
        }

        private void LockFields()
        {
            /*
            List<string> fieldKeys = PurchaseOrderEdit.GetFieldKeys();
            foreach (string current in fieldKeys)
            {
                base.View.LockField(current, false);
            }
            */
        }

        private void LockInvalidEntryRow()
        {
            DynamicObjectCollection dynamicObjectCollection = base.View.Model.DataObject["POOrderEntry"] as DynamicObjectCollection;
            if (dynamicObjectCollection != null && dynamicObjectCollection.Count > 0)
            {
                foreach (DynamicObject current in dynamicObjectCollection)
                {
                    if (!"A".EqualsIgnoreCase(Convert.ToString(current["MRPTerminateStatus"])) || !"A".EqualsIgnoreCase(Convert.ToString(current["MRPCloseStatus"])) || !"A".EqualsIgnoreCase(Convert.ToString(current["MRPFreezeStatus"])))
                    {
                        base.View.StyleManager.SetEnabled("FMaterialId", current, null, false);
                        base.View.StyleManager.SetEnabled("FQty", current, null, false);
                        base.View.StyleManager.SetEnabled("FPrice", current, null, false);
                        base.View.StyleManager.SetEnabled("FTaxPrice", current, null, false);
                        base.View.StyleManager.SetEnabled("FEntryTaxRate", current, null, false);
                        base.View.StyleManager.SetEnabled("FTaxPrice", current, null, false);
                        base.View.StyleManager.SetEnabled("FDeliveryDate", current, null, false);
                    }
                }
                    
                //base.View.GetBarItem("FPOOrderEntry", "tbReserveLinkQuery").Visible = false;
            }
        }


        private bool GetFilterPayBill(string fieldKey, out string filter)
        {
            filter = "";
            if (string.IsNullOrWhiteSpace(fieldKey))
            {
                return false;
            }
            DynamicObject dynamicObject = base.View.Model.GetValue("FSupplierId") as DynamicObject;
            DynamicObject dynamicObject2 = base.View.Model.GetValue("FSettleCurrId") as DynamicObject;
            long num = (dynamicObject == null) ? 0L : Convert.ToInt64(dynamicObject["Id"]);
            long num2 = (dynamicObject2 == null) ? 0L : Convert.ToInt64(dynamicObject2["Id"]);
            filter = string.Format(" FPAYITEMTYPE='1' AND FSUPPLIERID={0} AND FCURRENCYID={1}", num, num2);
            return !string.IsNullOrWhiteSpace(filter);
        }

        private void SetValueId(string key, DynamicObject obj, int row)
        {
            if (obj == null)
            {
                base.View.Model.SetValue(key, null, row);
                return;
            }
            base.View.Model.SetValue(key, Convert.ToInt64(obj["Id"]), row);
        }

        protected bool IsCanView(string fromId)
        {
            IPermissionService permissionService = ServiceFactory.GetPermissionService(base.Context);
            PermissionAuthResult permissionAuthResult = permissionService.FuncPermissionAuth(base.Context, new BusinessObject
            {
                Id = fromId
            }, "d716c2ed10744cd4bfad5a1a4cdebeb6");
            ServiceFactory.CloseService(permissionService);
            return permissionAuthResult.Passed;
        }


        private void ShowPayList()
        {
            string value = Convert.ToString(base.View.Model.GetPKValue());
            DynamicFormShowParameter dynamicFormShowParameter = new DynamicFormShowParameter();
            dynamicFormShowParameter.Height = 600;
            dynamicFormShowParameter.Width = 800;
            dynamicFormShowParameter.FormId = "Pur_PayDetailList";
            dynamicFormShowParameter.SyncCallBackAction = true;
            dynamicFormShowParameter.ParentPageId = base.View.PageId;
            dynamicFormShowParameter.PageId = SequentialGuid.NewGuid().ToString();
            string key = "SessionPayKey";
            dynamicFormShowParameter.CustomParams.Add(key, value);
            base.View.ShowForm(dynamicFormShowParameter);
        }
    }
}
