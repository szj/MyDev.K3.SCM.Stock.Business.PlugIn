using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Core.Report.PlugIn;
using Kingdee.BOS.Core.SqlBuilder;
using Kingdee.BOS.Orm.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDev.K3.SCM.Stock.Business.PlugIn
{
    [Description("采购SysReportPlugIn测试插件")]
    public class ReceiptReportPlugin : AbstractSysReportPlugIn
    {
        /// <summary>
        /// 列表数据加载之后，需要处理的功能，包括列的样式，格式，颜色，背景之类的样式设计
        /// </summary>
        /// <param name="e"></param>
        public override void AfterBindData(EventArgs e)
        {

        }

        /// <summary>
        /// 菜单点击事件，表单插件同样适用
        /// </summary>
        /// <param name="e"></param>
        public override void BarItemClick(BarItemClickEventArgs e)
        {
            switch (e.BarItemKey.ToUpperInvariant())
            {
                //case "TBDELETE": 列表工具栏按钮事件，通过按钮Key[大写]来区分那个按钮事件
                //break;
                case "":
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 菜单点击后处理事件，表单插件同样适用
        /// </summary>
        /// <param name="e"></param>
        public override void AfterBarItemClick(AfterBarItemClickEventArgs e)
        {
            switch (e.BarItemKey.ToUpperInvariant())
            {
                case "":
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// 行双击事件
        /// </summary>
        /// <param name="e"></param>
        public override void EntityRowDoubleClick(EntityRowClickEventArgs e)
        {

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
