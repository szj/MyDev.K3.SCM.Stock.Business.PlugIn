using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.Validation;
using Kingdee.BOS.Orm.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDev.K3.SCM.Stock.Business.PlugIn
{

    [Description("采购OperationServicePlugIn测试插件")]
    public  class RecepiptOperationPlugin: AbstractOperationServicePlugIn
    {

        public override void OnPreparePropertys(PreparePropertysEventArgs e)
        {
            //e.FieldKeys.Add("");将需要应用的字段Key加入
        }

        /// <summary>
        /// 添加校验器
        /// </summary>
        /// <param name="e"></param>
        public override void OnAddValidators(AddValidatorsEventArgs e)
        {
            var operValidator = new OperValidator();
            operValidator.AlwaysValidate = true;
            operValidator.EntityKey = "FBillHead";
            e.Validators.Add(operValidator);
        }

        /// <summary>
        /// 操作开始前功能处理
        /// </summary>
        /// <param name="e"></param>
        public override void BeginOperationTransaction(BeginOperationTransactionArgs e)
        {
            foreach (DynamicObject o in e.DataEntitys)
            {
            }
        }

        /// <summary>
        /// 操作结束后功能处理
        /// </summary>
        /// <param name="e"></param>
        public override void EndOperationTransaction(EndOperationTransactionArgs e)
        {
            foreach (DynamicObject o in e.DataEntitys)
            {
            }
        }


        /// <summary>
        /// 当前操作的校验器
        /// </summary>
        private class OperValidator : AbstractValidator
        {
            public override void Validate(Kingdee.BOS.Core.ExtendedDataEntity[] dataEntities, ValidateContext validateContext, Kingdee.BOS.Context ctx)
            {
                //foreach (var dataEntity in dataEntities)
                //{
                //判断到数据有错误
                //    if()
                //    {
                //        ValidationErrorInfo ValidationErrorInfo = new ValidationErrorInfo(
                //            string.Empty,
                //            dataEntity["Id"].ToString(),
                //            dataEntity.DataEntityIndex,
                //            dataEntity.RowIndex,
                //            dataEntity["Id"].ToString(),
                //            "errMessage",
                //             string.Empty);
                //        validateContext.AddError(null, ValidationErrorInfo);
                //        continue;
                //    }

                //}
            }
        }

    }
}
