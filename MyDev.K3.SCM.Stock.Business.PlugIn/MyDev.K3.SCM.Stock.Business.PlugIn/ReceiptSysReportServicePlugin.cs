using Kingdee.BOS.Contracts.Report;
using Kingdee.BOS.Core.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDev.K3.SCM.Stock.Business.PlugIn
{
    [Description("采购SysReportBaseService测试插件")]
    public  class ReceiptSysReportServicePlugin : SysReportBaseService
    {

        public override void Initialize()
        {

        }

        /// <summary>
        /// 动态构造列
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public override ReportHeader GetReportHeaders(IRptParams filter)
        {
            ReportHeader header = new ReportHeader();
            return header;
        }

        /// <summary>
        /// 设置报表头
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public override ReportTitles GetReportTitles(IRptParams filter)
        {
            ReportTitles titles = new ReportTitles();
            return titles;
        }

        /// <summary>
        /// 构造取数Sql，取数据填充到临时表：tableName
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="tableName"></param>
        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        {

        }

        /// <summary>
        /// 设置汇总行，只有显示财务信息时才需要汇总
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public override List<SummaryField> GetSummaryColumnInfo(IRptParams filter)
        {
            List<SummaryField> summaryList = new List<SummaryField>();
            return summaryList;
        }
    }
}
