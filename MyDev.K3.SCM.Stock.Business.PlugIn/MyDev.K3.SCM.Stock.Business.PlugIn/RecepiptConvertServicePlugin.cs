using Kingdee.BOS.Core.Metadata.ConvertElement.PlugIn;
using Kingdee.BOS.Core.Metadata.ConvertElement.PlugIn.Args;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDev.K3.SCM.Stock.Business.PlugIn
{
    [Description("采购ConvertPlugIn测试插件")]
    public class RecepiptConvertServicePlugin : AbstractConvertPlugIn
    {

        /// <summary>
        /// 解析字段映射关系，并构建查询参数。
        /// 这里可以加入你想要的字段
        /// </summary>
        /// <param name="e">事件参数包</param>
        public override void OnQueryBuilderParemeter(QueryBuilderParemeterEventArgs e)
        {
            //e.SelectItems.Add(new SelectorItemInfo("")); 添加你需要添加的字段的Key
        }

        /// <summary>
        /// 创建关联关系后事件
        /// </summary>
        /// <param name="e">事件参数包</param>
        public override void OnAfterCreateLink(CreateLinkEventArgs e)
        {

        }

        /// <summary>
        /// 下推/选单时，根据字段映射,向目标字段填充值
        /// </summary>
        /// <param name="e"></param>
        public override void OnFieldMapping(FieldMappingEventArgs e)
        {
            switch (e.TargetField.Key.ToUpperInvariant())
            {
                //case "FXXX": 选单时的目标字段Key【大写】，可以根据条件设置值e.MapValue
                //    e.MapValue = "";
                //   break;
                case "":
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 下推/选单，自动填充目标字段值完毕后，这里可以对填充值进行修复，处理
        /// </summary>
        /// <param name="e"></param>
        public override void OnAfterFieldMapping(AfterFieldMappingEventArgs e)
        {

        }

        /// <summary>
        /// 最后触发：单据转换后事件
        /// </summary>
        /// <param name="e"></param>
        public override void AfterConvert(AfterConvertEventArgs e)
        {

        }

    }
}
