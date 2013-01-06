﻿using Unicoen.Apps.UniMetrics.UcoAnalyzer;

namespace Unicoen.Apps.UniMetrics.FrameworkUser
{
    public class CStrategy : MeasurableElementGenerator
    {
        public override string GetNamespaceType()
        {
            return "";
        }

        public override string GetClassType(bool isAbstract, bool isInterface)
        {
            return "module";
        }

        public override string GetMethodType(bool isConstructor, bool isReturn)
        {
            return isReturn ? "function" : "procedure";
        }
    }
}
