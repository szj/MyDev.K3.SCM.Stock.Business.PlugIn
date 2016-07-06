using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;

namespace MyDev.K3.SCM.Stock.Business.PlugIn
{
    public class ReceiptEdit : Kingdee.BOS.Core.Bill.PlugIn.AbstractBillPlugIn

    {
        public override void BarItemClick(BarItemClickEventArgs e)
        {
            base.BarItemClick(e);

            if (e.BarItemKey == "btnTest")
            {
                this.View.ShowMessage("Hello world!", MessageBoxType.Notice);

            }
        }

    }




}
