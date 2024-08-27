using System;
using System.Collections.Generic;
using Scheduler.Application.Constant;

namespace Scheduler.Application.Model
{
    public class AdditionalParamsDto : Dictionary<string, object>
    {
        public AdditionalParamsDto() : base(StringComparer.InvariantCultureIgnoreCase)
        { }

        public string TenantId
        {
            get
            {
                if (this.ContainsKey(FieldName.TENANT_ID) && this[FieldName.TENANT_ID] != null)
                    return this[FieldName.TENANT_ID].ToString();
                return string.Empty;
            }
        }

        public string SubscriptionId
        {
            get
            {
                if (this.ContainsKey(FieldName.SUBSCRIPTION_ID) && this[FieldName.SUBSCRIPTION_ID] != null)
                    return this[FieldName.SUBSCRIPTION_ID].ToString();
                return string.Empty;
            }
        }

        public string ProjectId
        {
            get
            {
                if (this.ContainsKey(FieldName.PROJECT_ID) && this[FieldName.PROJECT_ID] != null)
                    return this[FieldName.PROJECT_ID].ToString();
                return string.Empty;
            }
        }
    }
}