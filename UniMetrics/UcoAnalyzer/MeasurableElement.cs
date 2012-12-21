using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Unicoen.Languages.C.ProgramGenerators;
using Unicoen.Languages.CSharp.ProgramGenerators;
using Unicoen.Languages.Java.ProgramGenerators;
using Unicoen.Languages.JavaScript.ProgramGenerators;
using Unicoen.Languages.Python2.ProgramGenerators;
using Unicoen.Languages.Ruby18.Model;
using Unicoen.Model;

namespace Unicoen.Apps.Loc.UcoAnalyzer
{
	class MeasurableElement
	{
		public string FilePath { get; set; }
		public List<MsElementNamespace> ListElementNamespace { get; set; }

		public MeasurableElement(string filePath)
		{
			this.FilePath = filePath;
		}

		/// <summary>
		/// set the measurement value into the object
		/// </summary>
		public void SetMeasurableElement()
		{
			var codeObject = CreateCodeObject(this.FilePath);
            this.ListElementNamespace = GetNamespaceListFromFile(codeObject);
		}

	    /// <summary>
		/// generate unified code object from input source code
		/// </summary>
		public UnifiedProgram CreateCodeObject(string filePath)
		{
			UnifiedProgram codeObject = null;
			var ext = Path.GetExtension(filePath);
			switch (ext.ToLower())
			{
				case ".c":
					codeObject = new CProgramGenerator().GenerateFromFile(filePath);
					break;
				case ".cs":
					codeObject = new CSharpProgramGenerator().GenerateFromFile(filePath);
					break;
				case ".java":
					codeObject = new JavaProgramGenerator().GenerateFromFile(filePath);
					break;
				case ".js":
					codeObject = new JavaScriptProgramGenerator().GenerateFromFile(filePath);
					break;
				case ".py":
					codeObject = new Python2ProgramGenerator().GenerateFromFile(filePath);
					break;
				case ".rb":
					codeObject = new Ruby18ProgramGenerator().GenerateFromFile(filePath);
					break;
				default:
					Console.WriteLine("Incorrect input file");
					break;
			}
			File.WriteAllText(@"_uco\" + Path.GetFileName(filePath) + DateTime.Now.ToFileTime() + ".txt", codeObject.ToString());
			return codeObject;
		}

        /// <summary>
        /// get name of namespace/package
        /// </summary>
        private string GetNamespaceName(UnifiedNamespaceDefinition aNamespace)
        {
            var str = "";
            if (aNamespace.FirstDescendant<UnifiedVariableIdentifier>().Parent is UnifiedNamespaceDefinition)
            {
                str = aNamespace.FirstDescendant<UnifiedVariableIdentifier>().Name;
            }
            else
            {
                var ns = aNamespace.FirstDescendant<UnifiedProperty>().Descendants<UnifiedVariableIdentifier>().Count();
                var del = aNamespace.FirstDescendant<UnifiedProperty>().Delimiter;
                for (var i = 0; i < ns; i++)
                {
                    str += aNamespace.Descendants<UnifiedVariableIdentifier>().ElementAt(i).Name + del;
                }
            }
            return str.TrimEnd('.');
        }

        /// <summary>
        /// get list of namespace/package in a file
        /// Java does not allow multiple packages in the same source file 
        /// C# does allow multiple namespaces in a single .cs file
        /// </summary>
        private List<MsElementNamespace> GetNamespaceListFromFile(UnifiedElement codeObject)
        {
            // One file has one or several namespace/package(s)
            var listNamespace = new List<MsElementNamespace>();
            var _namespace = codeObject.Descendants<UnifiedNamespaceDefinition>();
            if (!_namespace.Any())
            {
                var elementNamespace = new MsElementNamespace();
                elementNamespace.ListClass = GetClassListFromNamespace(codeObject);
                listNamespace.Add(elementNamespace);
            }
            foreach (var aNamespace in _namespace)
            {
                var elementNamespace = new MsElementNamespace();
                elementNamespace.Name = GetNamespaceName(aNamespace);
                elementNamespace.Type = "package"; // namespace or package
                elementNamespace.ListClass = GetClassListFromNamespace(aNamespace);
                listNamespace.Add(elementNamespace);
            }
            return listNamespace;
        }

        /// <summary>
        /// get list of class/interface in a namespace/package
        /// </summary>
        private List<MsElementClass> GetClassListFromNamespace(UnifiedElement aNamespace)
        {
            // One namespace has several classes
            var listClass = new List<MsElementClass>();
            var _classes = aNamespace.Descendants<UnifiedClassDefinition, UnifiedInterfaceDefinition>();
            if (!_classes.Any())
            {
                var elementClass = new MsElementClass();
                elementClass.ListMethod = GetMethodListFromClass(aNamespace);
                elementClass.ListAttribute = GetAttributeListFromClass(aNamespace);
                listClass.Add(elementClass);
            }
            foreach (var aClass in _classes)
            {
                var elementClass = new MsElementClass();
                elementClass.Name = GetIdentifierName(aClass);

                if (aClass is UnifiedInterfaceDefinition)
                {
                    elementClass.Type = "interface";
                }
                else
                {
                    elementClass.Type = "class"; // class or abstract or interface
                }
                try
                {
                    elementClass.ClassParent = GetIdentifierName(aClass.Descendants<UnifiedExtendConstrain>().ElementAt(0));
                }
                catch (ArgumentOutOfRangeException e)
                {
                }
                elementClass.ListMethod = GetMethodListFromClass(aClass);
                elementClass.ListAttribute = GetAttributeListFromClass(aClass);
                listClass.Add(elementClass);
            }
            return listClass;
        }

		/// <summary>
		/// get list of method/function/procedure in a class
		/// </summary>
		private List<MsElementMethod> GetMethodListFromClass(UnifiedElement aClass)
		{
			// One class can have several methods
			var listMethod = new List<MsElementMethod>();
			var _methods = aClass.Descendants<UnifiedConstructor, UnifiedFunctionDefinition>();
			foreach (var aMethod in _methods)
			{
				var elementMethod = new MsElementMethod();
				if (aMethod is UnifiedConstructor)
				{
                    elementMethod.Name = GetIdentifierName(aMethod.GrandParent());
					elementMethod.Type = "constructor";
				}
				else
				{
					elementMethod.Name = ((UnifiedFunctionDefinition) aMethod).Name.Name;
					elementMethod.Type = "method"; // method or function or procedure
				}

				elementMethod.ListMethofArgument = GetArgumentListFromMethod(aMethod);
				elementMethod.ListMethodCall = GetMethodCallListFromMethod(aMethod);
				listMethod.Add(elementMethod);
			}
			return listMethod;
		}

		/// <summary>
		/// get list of method argument in a method/function/procedure
		/// </summary>
		private List<MsElementMethodArgument> GetArgumentListFromMethod(UnifiedElement aMethod)
		{
			// One method can have several method arguments
			var listMethodArgument = new List<MsElementMethodArgument>();
			var _methodArgs = aMethod.Descendants<UnifiedParameter>();
			foreach (var anArg in _methodArgs)
			{
				var elementMethodArgument = new MsElementMethodArgument();
				try
				{
					elementMethodArgument.ArgName = GetIdentifierName(anArg.Names);
					elementMethodArgument.ArgType = GetIdentifierName(anArg.Type);
				} catch (NullReferenceException e) { }
				
				listMethodArgument.Add(elementMethodArgument);
			}
			return listMethodArgument;
		}

		/// <summary>
		/// get list of method call in a method/function/procedure
		/// </summary>
		private List<string> GetMethodCallListFromMethod(UnifiedElement aMethod)
		{
			// One method can call other methods
			var listMethodCall = new List<string>();
			var _methodCall = aMethod.Descendants<UnifiedCall>();
			foreach (var aCall in _methodCall)
			{
				if (aCall.Descendants().ElementAt(0) is UnifiedVariableIdentifier)
				{
					listMethodCall.Add(GetIdentifierName(aCall));
				}
			}
			return listMethodCall;
		}

		/// <summary>
		/// get list of attribute in a class
		/// </summary>
		private List<MsElementAttribute> GetAttributeListFromClass(UnifiedElement aclass)
		{
			// One class can have several attributes
			var listAttribute = new List<MsElementAttribute>();
			var attr = aclass.Descendants<UnifiedVariableDefinition>();
			foreach (var a in attr)
			{
				try
				{
					if (a.GrandGrandParent() is UnifiedClassDefinition)
					{
						var m = new MsElementAttribute();
						m.Name = a.Name.Name;
						m.Type = GetIdentifierName(a.Type);
						listAttribute.Add(m);
					}
				}
				catch (NullReferenceException e) { }
			}
			return listAttribute;
		}

		private string GetIdentifierName (UnifiedElement ue)
		{
			// Get string name of a unified element
			return ue.Descendants<UnifiedVariableIdentifier>().ElementAt(0).Name;
		}
	}
}
