using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Unicoen.Apps.UniMetrics.Strategy;
using Unicoen.Model;

namespace Unicoen.Apps.UniMetrics.UcoAnalyzer
{
	class MeasurableElement
	{
		public string FilePath { get; set; }
		public AbstractLanguageStrategy Strategy;
		public List<MsElementNamespace> ListElementNamespace { get; set; }

		public MeasurableElement(string filePath)
		{
			FilePath = filePath;
		}

		/// <summary>
		/// set the measurement value into the object
		/// </summary>
		public void SetMeasurableElement()
		{
			var ext = Path.GetExtension(FilePath);
			switch (ext.ToLower())
			{
				case ".c":
					Strategy = new CStrategy(FilePath);
					break;
				case ".cs":
					Strategy = new CSharpStrategy(FilePath);
					break;
				case ".java":
					Strategy = new JavaStrategy(FilePath);
					break;
				case ".js":
					Strategy = new JavaScriptStrategy(FilePath);
					break;
				case ".py":
					Strategy = new PythonStrategy(FilePath);
					break;
				case ".rb":
					Strategy = new RubyStrategy(FilePath);
					break;
				default:
					Console.WriteLine("Incorrect input file");
					break;
			}
			File.WriteAllText(@"_uco\" + Path.GetFileName(FilePath) + DateTime.Now.ToFileTime() + ".txt", Strategy.CreateCodeObject().ToString());
			ListElementNamespace = GetNamespaceListFromFile(Strategy.CreateCodeObject());
		}

		/// <summary>
		/// get name of namespace/package
		/// </summary>
		private string GetNamespaceName(UnifiedNamespaceDefinition aNamespace)
		{
			var str = "";
			var del = "";
			if (aNamespace.FirstDescendant<UnifiedVariableIdentifier>().Parent is UnifiedNamespaceDefinition)
			{
				str = aNamespace.FirstDescendant<UnifiedVariableIdentifier>().Name;
			}
			else
			{
				var ns = aNamespace.FirstDescendant<UnifiedProperty>().Descendants<UnifiedVariableIdentifier>().Count();
				del = aNamespace.FirstDescendant<UnifiedProperty>().Delimiter;
				for (var i = 0; i < ns; i++)
				{
					str += aNamespace.Descendants<UnifiedVariableIdentifier>().ElementAt(i).Name + del;
				}
			}
			return str.Remove(str.Length-del.Length, del.Length);
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
			var namespaces = codeObject.Descendants<UnifiedNamespaceDefinition>();
			if (!namespaces.Any())
			{
				var elementNamespace = new MsElementNamespace();
				elementNamespace.ListClass = GetClassListFromNamespace(codeObject);
				listNamespace.Add(elementNamespace);
			}
			foreach (var aNamespace in namespaces)
			{
				var elementNamespace = 
					new MsElementNamespace
						{
							Name = GetNamespaceName(aNamespace),
							// namespace or package
							Type = Strategy.GetNamespaceType(),
							ListClass = GetClassListFromNamespace(aNamespace)
						};

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
			var classes = aNamespace.Descendants<UnifiedClassDefinition, UnifiedInterfaceDefinition>();
			if (!classes.Any())
			{
				var elementClass = new MsElementClass();
				elementClass.ListMethod = GetMethodListFromClass(aNamespace);
				elementClass.ListAttribute = GetAttributeListFromClass(aNamespace);
				listClass.Add(elementClass);
			}
			foreach (var aClass in classes)
			{
				var isAbstract = false;
				var isInterface = aClass is UnifiedInterfaceDefinition;
				for (var i = 0; i < aClass.FirstDescendant<UnifiedModifierCollection>().Count; i++)
				{
					isAbstract = aClass.Descendants<UnifiedModifier>().ElementAt(i).Name.Equals("abstract");
				}
				var elementClass = 
					new MsElementClass
						{
							Name = GetIdentifierName(aClass),
							// class or interface or abstract or module
							Type = Strategy.GetClassType(isAbstract, isInterface), 
							ClassParent = GetIdentifierName(aClass.FirstDescendant<UnifiedExtendConstrain>()),
							ListMethod = GetMethodListFromClass(aClass),
							ListAttribute = GetAttributeListFromClass(aClass)
						};
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
			var methods = aClass.Descendants<UnifiedConstructor, UnifiedFunctionDefinition>();
			foreach (var aMethod in methods)
			{
				var returnValue = GetIdentifierName(aMethod.FirstDescendant<UnifiedReturn>());
				var isReturn = (returnValue != null);
				var isConstructor = aMethod is UnifiedConstructor;
				var elementMethod = 
					new MsElementMethod
						{
							Name = isConstructor
								   ? GetIdentifierName(aMethod.GrandParent())
								   : ((UnifiedFunctionDefinition) aMethod).Name.Name,
							// constructor or method or function or procedure
							Type = Strategy.GetMethodType(isConstructor, isReturn),
							ListMethofArgument = GetArgumentListFromMethod(aMethod),
							ListMethodCall = GetMethodCallListFromMethod(aMethod)
						};
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
			var methodArgs = aMethod.Descendants<UnifiedParameter>();
			foreach (var anArg in methodArgs)
			{
				var elementMethodArgument = 
					new MsElementMethodArgument
						{
							ArgName = GetIdentifierName(anArg.Names),
							ArgType = GetIdentifierName(anArg.Type)
						};
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
			var methodCall = aMethod.Descendants<UnifiedCall>();
			foreach (var aCall in methodCall)
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
			var attributes = aclass.Descendants<UnifiedVariableDefinition>();
			foreach (var anAttr in attributes)
			{
				if (!(anAttr.GrandGrandParent() is UnifiedClassDefinition)) continue;
				var elementAttribute = 
					new MsElementAttribute
						{
							Name = anAttr.Name.Name, 
							Type = GetIdentifierName(anAttr.Type)
						};
				listAttribute.Add(elementAttribute);
			}
			return listAttribute;
		}

		private string GetIdentifierName (UnifiedElement ue)
		{
			// Get string name of a unified element
			try 
			{
				return ue.Descendants<UnifiedVariableIdentifier>().ElementAt(0).Name;
			}
			catch (Exception e)
			{
				return null;
			}
		}
	}
}
