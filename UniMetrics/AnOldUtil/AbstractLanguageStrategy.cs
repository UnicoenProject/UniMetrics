using System;

namespace Unicoen.Apps.UniMetrics.OldUtil
{
    public class AbstractLanguageStrategy
    {
        public string FilePath;

        /*public abstract string GetLanguage();

        public abstract UnifiedProgram CreateCodeObject();

        public abstract string GetNamespaceType();

        public abstract string GetClassType(bool isAbstract, bool isInterface);

        public abstract string GetMethodType(bool isConstructor, bool isReturn);*/

        public virtual void Print()
        {
            Console.WriteLine("this is abstract language strategy");
        }
    }
}
