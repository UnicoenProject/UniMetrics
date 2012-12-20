using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Unicoen.Languages.C.ProgramGenerators;
using Unicoen.Languages.CSharp.ProgramGenerators;
using Unicoen.Languages.Java;
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
		public ElementNamespace ElementNamespace { get; set; }
		public List<ElementClass> ListElementClass { get; set; }

		public MeasurableElement(string filePath)
		{
			this.FilePath = filePath;
		}

		/// <summary>
		/// set the measurement value into the object
		/// </summary>
		public void SetMeasurableElement()
		{
			UnifiedProgram codeObject = null;
			var ext = Path.GetExtension(this.FilePath);
			switch (ext.ToLower())
			{
				case ".c":
					codeObject = new CProgramGenerator().GenerateFromFile(this.FilePath);
					break;
				case ".cs":
					codeObject = new CSharpProgramGenerator().GenerateFromFile(this.FilePath);
					break;
				case ".java":
					codeObject = new JavaProgramGenerator().GenerateFromFile(this.FilePath);
					break;
				case ".js":
					codeObject = new JavaScriptProgramGenerator().GenerateFromFile(this.FilePath);
					break;
				case ".py":
					codeObject = new Python2ProgramGenerator().GenerateFromFile(this.FilePath);
					break;
				case ".rb":
					codeObject = new Ruby18ProgramGenerator().GenerateFromFile(this.FilePath);
					break;
				default:
					Console.WriteLine("Incorrect input file");
					break;
			}
			
			var elementNamespace = new ElementNamespace();
			var _namespace = codeObject.Descendants<UnifiedNamespaceDefinition>();
			string str="";
			foreach (var n in _namespace)
			{
				str = n.Descendants<UnifiedVariableIdentifier>().ElementAt(0).Name;
			}
			elementNamespace.Name = str;
			this.ElementNamespace = elementNamespace;
			
			// One namespace has several classes
			var listClass = new List<ElementClass>();
			var _classes = codeObject.Descendants<UnifiedClassDefinition>();
			
			if (_classes.Count() == 0)
			{
				var aClass = codeObject as UnifiedElement;
				var elementClass = new ElementClass();
				GetMethodListFromClass(elementClass,aClass);
				GetAttributeListFromClass(elementClass, aClass);
				listClass.Add(elementClass);
			}

			foreach (var aClass in _classes){
				var elementClass = new ElementClass();
				elementClass.Name = GetIdentifierName(aClass);
				elementClass.Type = "class"; // class or abstract or interface
				try
				{
					elementClass.ClassParent = GetIdentifierName(aClass.Descendants<UnifiedExtendConstrain>().ElementAt(0));
				} catch (ArgumentOutOfRangeException e) { }
				GetMethodListFromClass(elementClass, aClass);
				GetAttributeListFromClass(elementClass, aClass);
				listClass.Add(elementClass);
			}
			this.ListElementClass = listClass;
		}

        /// <summary>
        /// get list of method in a class
        /// </summary>
		private void GetMethodListFromClass(ElementClass elementClass, UnifiedElement aClass)
		{
			// One class can have several methods
			var listMethod = new List<ElementMethod>();
			var _methods = aClass.Descendants<UnifiedConstructor, UnifiedFunctionDefinition>();
			foreach (var aMethod in _methods)
			{
				var elementMethod = new ElementMethod();
				if (aMethod is UnifiedConstructor)
				{
					elementMethod.Name = elementClass.Name;
					elementMethod.Type = "constructor";
				}
				else
				{
					elementMethod.Name = (aMethod as UnifiedFunctionDefinition).Name.Name;
					elementMethod.Type = "method"; // method or function or procedure
				}

				GetArgumentListFromMethod(elementMethod, aMethod);
				GetMethodCallListFromMethod(elementMethod, aMethod);
				listMethod.Add(elementMethod);
			}
			elementClass.ListMethod = listMethod;
		}

        /// <summary>
        /// get list of method argument in a method
        /// </summary>
		private void GetArgumentListFromMethod(ElementMethod elementMethod, UnifiedElement aMethod)
		{
			// One method can have several method arguments
			var listMethodArgument = new List<ElementMethodArgument>();
			var _methodArgs = aMethod.Descendants<UnifiedParameter>();
			foreach (var anArg in _methodArgs)
			{
				var elementMethodArgument = new ElementMethodArgument();
				try
				{
					elementMethodArgument.ArgName = GetIdentifierName(anArg.Names);
					elementMethodArgument.ArgType = GetIdentifierName(anArg.Type);
				} catch (NullReferenceException e) { }
				
				listMethodArgument.Add(elementMethodArgument);
			}
			elementMethod.ListMethofArgument = listMethodArgument;
		}

        /// <summary>
        /// get list of method call in a method
        /// </summary>
		private void GetMethodCallListFromMethod(ElementMethod elementMethod, UnifiedElement aMethod)
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
			elementMethod.ListMethodCall = listMethodCall;
		}

        /// <summary>
        /// get list of attribute in a class
        /// </summary>
		private void GetAttributeListFromClass(ElementClass elementClass, UnifiedElement aclass)
		{
            // One class can have several attributes
			var listAttribute = new List<ElementAttribute>();
			var attr = aclass.Descendants<UnifiedVariableDefinition>();
			foreach (var a in attr)
			{
				try
				{
				    if (a.Parent.Parent.Parent is UnifiedClassDefinition)
				    {
					    var m = new ElementAttribute();
						m.Name = a.Name.Name;
						m.Type = GetIdentifierName(a.Type);
					    listAttribute.Add(m);
				    }
				}
				catch (NullReferenceException e) { }
			}
			elementClass.ListAttribute = listAttribute;
		}

		private string GetIdentifierName (UnifiedElement ue)
		{
            // Get string name of a unified element
			return ue.Descendants<UnifiedVariableIdentifier>().ElementAt(0).Name;
		}
	}
}
