/*
 * Copyright 2007-2011 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable CheckNamespace
// ReSharper disable IntroduceOptionalParameters.Global
namespace JetBrains.Annotations
{
    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = true)]
    public sealed class AspDataFieldAttribute : Attribute
    {
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Suppression is OK here.")]
    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public sealed class AspMethodPropertyAttribute : Attribute
    {
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
    public sealed class AspMvcActionAttribute : Attribute
    {
        [UsedImplicitly]
        public string AnonymousProperty { get; private set; }

        public AspMvcActionAttribute()
        {
        }

        public AspMvcActionAttribute(string anonymousProperty)
        {
            AnonymousProperty = anonymousProperty;
        }
    }

    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public sealed class AspMvcActionSelectorAttribute : Attribute
    {
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class AspMvcAreaAttribute : PathReferenceAttribute
    {
        [UsedImplicitly]
        public string AnonymousProperty { get; private set; }

        [UsedImplicitly]
        public AspMvcAreaAttribute()
        {
        }

        public AspMvcAreaAttribute(string anonymousProperty)
        {
            AnonymousProperty = anonymousProperty;
        }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
    public sealed class AspMvcControllerAttribute : Attribute
    {
        [UsedImplicitly]
        public string AnonymousProperty { get; private set; }

        public AspMvcControllerAttribute()
        {
        }

        public AspMvcControllerAttribute(string anonymousProperty)
        {
            AnonymousProperty = anonymousProperty;
        }
    }

    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class AspMvcDisplayTemplateAttribute : Attribute
    {
    }

    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class AspMvcEditorTemplateAttribute : Attribute
    {
    }

    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class AspMvcMasterAttribute : Attribute
    {
    }

    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class AspMvcModelTypeAttribute : Attribute
    {
    }

    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
    public sealed class AspMvcPartialViewAttribute : PathReferenceAttribute
    {
    }

    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class AspMvcSupressViewErrorAttribute : Attribute
    {
    }

    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
    public sealed class AspMvcViewAttribute : PathReferenceAttribute
    {
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public sealed class AspTypePropertyAttribute : Attribute
    {
        [PublicAPI]
        public bool CreateConstructorReferences { get; private set; }

        public AspTypePropertyAttribute(bool createConstructorReferences)
        {
            CreateConstructorReferences = createConstructorReferences;
        }
    }

    /// <summary>
    /// Indicates the condition parameter of the assertion method.
    /// The method itself should be marked by <see cref="T:JetBrains.Annotations.AssertionMethodAttribute"/> attribute.
    /// The mandatory argument of the attribute is the assertion type.
    /// </summary>
    /// <seealso cref="T:JetBrains.Annotations.AssertionConditionType"/>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [Obsolete("Use ContractAnnotationAttribute instead")]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class AssertionConditionAttribute : Attribute
    {
        /// <summary>
        /// Gets condition type
        /// </summary>
        public AssertionConditionType ConditionType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertionConditionAttribute"/> class.
        /// </summary>
        /// <param name="conditionType">Specifies condition type</param>
        public AssertionConditionAttribute(AssertionConditionType conditionType)
        {
            ConditionType = conditionType;
        }
    }

    /// <summary>
    /// Specifies assertion type. If the assertion method argument satisfies the condition, then the execution continues.
    /// Otherwise, execution is assumed to be halted
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    public enum AssertionConditionType
    {
// ReSharper disable InconsistentNaming
        IS_TRUE,
        IS_FALSE,
        IS_NULL,
        IS_NOT_NULL,
// ReSharper restore InconsistentNaming
    }

// ReSharper disable CSharpWarnings::CS0612

    /// <summary>
    /// Indicates that the marked method is assertion method, i.e. it halts control flow if one of the conditions is satisfied.
    /// To set the condition, mark one of the parameters with <see cref="T:JetBrains.Annotations.AssertionConditionAttribute"/> attribute
    /// </summary>
    /// <seealso cref="T:JetBrains.Annotations.AssertionConditionAttribute"/>
    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AssertionMethodAttribute : Attribute
    {
    }

// ReSharper restore CSharpWarnings::CS0612

    /// <summary>
    /// When applied to target attribute, specifies a requirement for any type which is marked with target attribute to implement or inherit specific type or types
    /// </summary>
    /// <example>
    /// <code>
    /// [BaseTypeRequired(typeof(IComponent)] // Specify requirement
    ///             public class ComponentAttribute : Attribute
    ///             {}
    /// 
    ///             [Component] // ComponentAttribute requires implementing IComponent interface
    ///             public class MyComponent : IComponent
    ///             {}
    /// </code>
    /// </example>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    [BaseTypeRequired(typeof(Attribute))]
    public sealed class BaseTypeRequiredAttribute : Attribute
    {
        /// <summary>
        /// Gets enumerations of specified base types
        /// </summary>
        public Type[] BaseTypes { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTypeRequiredAttribute"/> class.
        /// </summary>
        /// <param name="baseType">Specifies which types are required</param>
        public BaseTypeRequiredAttribute(Type baseType)
        {
            BaseTypes = new[]
                {
                    baseType
                };
        }
    }

    /// <summary>
    /// Indicates that the value of marked element could be <c>null</c> sometimes, so the check for <c>null</c> is necessary before its usage
    /// </summary>
    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Delegate,
        AllowMultiple = false, Inherited = true)]
    public sealed class CanBeNullAttribute : Attribute
    {
    }

    /// <summary>
    /// Indicates that the value of marked type (or its derivatives) cannot be compared using '==' or '!=' operators.
    /// There is only exception to compare with <c>null</c>, it is permitted
    /// </summary>
    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public sealed class CannotApplyEqualityOperatorAttribute : Attribute
    {
    }

    /// <summary>
    /// Describes dependency between method input and output
    /// </summary>
    /// <syntax><p>Function definition table syntax:</p>
    /// <list>
    /// <item>
    /// FDT      ::= FDTRow [;FDTRow]*
    /// </item>
    /// <item>
    /// FDTRow   ::= Input =&gt; Output | Output &lt;= Input
    /// </item>
    /// <item>
    /// Input    ::= ParameterName: Value [, Input]*
    /// </item>
    /// <item>
    /// Output   ::= [ParameterName: Value]* {halt|stop|void|nothing|Value}
    /// </item>
    /// <item>
    /// Value    ::= true | false | null | notnull | canbenull
    /// </item>
    /// </list>
    /// If method has single input parameter, it's name could be omitted. <br/>
    /// Using "halt" (or "void"/"nothing", which is the same) for method output means that methos doesn't return normally. <br/>
    /// "canbenull" annotation is only applicable for output parameters. <br/>
    /// You can use multiple [ContractAnnotation] for each FDT row, or use single attribute with rows separated by semicolon. <br/></syntax><examples>
    /// <list>
    /// <item>
    /// [ContractAnnotation("=&gt; halt")] public void TerminationMethod()
    /// </item>
    /// <item>
    /// [ContractAnnotation("halt &lt;= condition: false")] public void Assert(bool condition, string text) // Regular Assertion method
    /// </item>
    /// <item>
    /// [ContractAnnotation("s:null =&gt; true")] public bool IsNullOrEmpty(string s) // String.IsNullOrEmpty
    /// </item>
    /// <item>
    /// [ContractAnnotation("null =&gt; null; notnull =&gt; notnull")] public object Transform(object data) // Method which returns null if parameter is null, and not null if parameter is not null
    /// </item>
    /// <item>
    /// [ContractAnnotation("s:null=&gt;false; =&gt;true,result:notnull; =&gt;false, result:null")] public bool TryParse(string s, out Person result)
    /// </item>
    /// </list>
    /// </examples>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class ContractAnnotationAttribute : Attribute
    {
        public string FDT { get; private set; }

        public bool ForceFullStates { get; private set; }

        public ContractAnnotationAttribute([NotNull] string fdt) : this(fdt, false)
        {
        }

        public ContractAnnotationAttribute([NotNull] string fdt, bool forceFullStates)
        {
            FDT = fdt;
            ForceFullStates = forceFullStates;
        }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [Flags]
    public enum ImplicitUseKindFlags
    {
        Default = 7,
        Access = 1,
        Assign = 2,
        InstantiatedWithFixedConstructorSignature = 4,
        InstantiatedNoFixedConstructorSignature = 8,
    }

    /// <summary>
    /// Specify what is considered used implicitly when marked with <see cref="T:JetBrains.Annotations.MeansImplicitUseAttribute"/> or <see cref="T:JetBrains.Annotations.UsedImplicitlyAttribute"/>
    /// </summary>
    [UsedImplicitly(WithMembers)]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [Flags]
    public enum ImplicitUseTargetFlags
    {
        Default = 1,
        Itself = Default,
        Members = 2,
        WithMembers = Members | Itself,
    }

    /// <summary>
    /// Tells code analysis engine if the parameter is completely handled when the invoked method is on stack.
    /// If the parameter is delegate, indicates that delegate is executed while the method is executed.
    /// If the parameter is enumerable, indicates that it is enumerated while the method is executed.
    /// </summary>
    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public sealed class InstantHandleAttribute : Attribute
    {
    }

    /// <summary>
    /// Indicates that the function argument should be string literal and match one of the parameters of the caller function.
    /// For example, <see cref="T:System.ArgumentNullException"/> has such parameter.
    /// </summary>
    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class InvokerParameterNameAttribute : Attribute
    {
    }

    /// <summary>
    /// Indicates that method is *pure* linq method, with postponed enumeration.
    /// C# iterator methods (yield ...) are always LinqTunnel
    /// </summary>
    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class LinqTunnelAttribute : Attribute
    {
    }

    /// <summary>
    /// Indicates that marked element should be localized or not.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class LocalizationRequiredAttribute : Attribute
    {
        /// <summary>
        /// Gets a value indicating whether a element should be localized.
        /// <value>
        /// <c>true</c> if a element should be localized; otherwise, <c>false</c>.
        /// </value>
        /// </summary>
        [UsedImplicitly]
        public bool Required { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JetBrains.Annotations.LocalizationRequiredAttribute"/> class with
        ///             <see cref="P:JetBrains.Annotations.LocalizationRequiredAttribute.Required"/> set to <see langword="true"/>.
        /// </summary>
        public LocalizationRequiredAttribute()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JetBrains.Annotations.LocalizationRequiredAttribute"/> class.
        /// </summary>
        /// <param name="required"><c>true</c> if a element should be localized; otherwise, <c>false</c>.</param>
        public LocalizationRequiredAttribute(bool required)
        {
            Required = required;
        }

        /// <summary>
        /// Returns whether the value of the given object is equal to the current <see cref="T:JetBrains.Annotations.LocalizationRequiredAttribute"/>.
        /// </summary>
        /// <param name="obj">The object to test the value equality of. </param>
        /// <returns>
        /// <c>true</c> if the value of the given object is equal to that of the current; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var requiredAttribute = obj as LocalizationRequiredAttribute;
            if (requiredAttribute != null)
            {
                return requiredAttribute.Required == Required;
            }
            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:JetBrains.Annotations.LocalizationRequiredAttribute"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Should be used on attributes and causes ReSharper to not mark symbols marked with such attributes as unused (as well as by other usage inspections)
    /// </summary>
    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class MeansImplicitUseAttribute : Attribute
    {
        [UsedImplicitly]
        public ImplicitUseKindFlags UseKindFlags { get; private set; }

        /// <summary>
        /// Gets value indicating what is meant to be used
        /// </summary>
        [UsedImplicitly]
        public ImplicitUseTargetFlags TargetFlags { get; private set; }

        [UsedImplicitly]
        public MeansImplicitUseAttribute()
            : this(ImplicitUseKindFlags.Default)
        {
        }

        [UsedImplicitly]
        public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
        {
            UseKindFlags = useKindFlags;
            TargetFlags = targetFlags;
        }

        [UsedImplicitly]
        public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags)
            : this(useKindFlags, ImplicitUseTargetFlags.Default)
        {
        }

        [UsedImplicitly]
        public MeansImplicitUseAttribute(ImplicitUseTargetFlags targetFlags)
            : this(ImplicitUseKindFlags.Default, targetFlags)
        {
        }
    }

    /// <summary>
    /// Indicates that IEnumarable, passed as parameter, is not enumerated.
    /// </summary>
    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class NoEnumerationAttribute : Attribute
    {
    }

    /// <summary>
    /// Indicates that the function is used to notify class type property value is changed.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class NotifyPropertyChangedInvocatorAttribute : Attribute
    {
        [UsedImplicitly]
        public string ParameterName { get; private set; }

        public NotifyPropertyChangedInvocatorAttribute()
        {
        }

        public NotifyPropertyChangedInvocatorAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }
    }

    /// <summary>
    /// Indicates that the value of marked element could never be <c>null</c>
    /// </summary>
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Delegate,
        AllowMultiple = false, Inherited = true)]
    public sealed class NotNullAttribute : Attribute
    {
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class PathReferenceAttribute : Attribute
    {
        [UsedImplicitly]
        public string BasePath { get; private set; }

        public PathReferenceAttribute()
        {
        }

        [UsedImplicitly]
        public PathReferenceAttribute([PathReference] string basePath)
        {
            BasePath = basePath;
        }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    public enum PointKinds
    {
        This,
        Ret,
        Par,
        LamPar,
        LamRet,
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    public enum PointPlurality
    {
        El,
        Col,
    }

    /// <summary>
    /// This attribute is intended to mark publicly available API which should not be removed and so is treated as used.
    /// </summary>
    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [MeansImplicitUse]
    public sealed class PublicAPIAttribute : Attribute
    {
        public PublicAPIAttribute()
        {
        }

        public PublicAPIAttribute(string comment)
        {
        }
    }

    /// <summary>
    /// Indicates that method doesn't contain observable side effects.
    /// The same as <see cref="T:System.Diagnostics.Contracts.PureAttribute"/>
    /// </summary>
    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public sealed class PureAttribute : Attribute
    {
    }

    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public sealed class RazorHelperCommonAttribute : Attribute
    {
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class RazorImportNamespaceAttribute : Attribute
    {
        [PublicAPI]
        public string Name { get; private set; }

        public RazorImportNamespaceAttribute(string name)
        {
            Name = name;
        }
    }

    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public sealed class RazorLayoutAttribute : Attribute
    {
    }

    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter, Inherited = true)]
    public sealed class RazorSectionAttribute : Attribute
    {
    }

    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public sealed class RazorWriteLiteralMethodAttribute : Attribute
    {
    }

    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public sealed class RazorWriteMethodAttribute : Attribute
    {
    }

    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public sealed class RazorWriteMethodParameterAttribute : Attribute
    {
    }

    /// <summary>
    /// Indicates that marked method builds string by format pattern and (optional) arguments.
    /// Parameter, which contains format string, should be given in constructor.
    /// The format string should be in <see cref="M:System.String.Format(System.IFormatProvider,System.String,System.Object[])"/> -like form
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class StringFormatMethodAttribute : Attribute
    {
        /// <summary>
        /// Gets format parameter name
        /// </summary>
        [UsedImplicitly]
        public string FormatParameterName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringFormatMethodAttribute"/> class.
        /// </summary>
        /// <param name="formatParameterName">Specifies which parameter of an annotated method should be treated as format-string</param>
        public StringFormatMethodAttribute(string formatParameterName)
        {
            FormatParameterName = formatParameterName;
        }
    }

    /// <summary>
    /// Indicates that the marked method unconditionally terminates control flow execution.
    /// For example, it could unconditionally throw exception
    /// </summary>
    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [Obsolete("Use ContractAnnotationAttribute instead")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class TerminatesProgramAttribute : Attribute
    {
    }

    /// <summary>
    /// Indicates that the marked symbol is used implicitly (e.g. via reflection, in external library),
    /// so this symbol will not be marked as unused (as well as by other usage inspections)
    /// </summary>
    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class UsedImplicitlyAttribute : Attribute
    {
        [UsedImplicitly]
        public ImplicitUseKindFlags UseKindFlags { get; private set; }

        /// <summary>
        /// Gets value indicating what is meant to be used
        /// </summary>
        [UsedImplicitly]
        public ImplicitUseTargetFlags TargetFlags { get; private set; }

        [UsedImplicitly]
        public UsedImplicitlyAttribute()
            : this(ImplicitUseKindFlags.Default)
        {
        }

        [UsedImplicitly]
        public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
        {
            UseKindFlags = useKindFlags;
            TargetFlags = targetFlags;
        }

        [UsedImplicitly]
        public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags) : this(useKindFlags, ImplicitUseTargetFlags.Default)
        {
        }

        [UsedImplicitly]
        public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags)
            : this(ImplicitUseKindFlags.Default, targetFlags)
        {
        }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    public sealed class ValueFlowAttribute
    {
        public PointPlurality FromPlurality { get; private set; }

        public PointKinds FromPointKind { get; private set; }

        public byte FromParameterIndex { get; private set; }

        public byte FromLambdaIndex { get; private set; }

        public PointPlurality ToPlurality { get; private set; }

        public PointKinds ToPointKind { get; private set; }

        public byte ToParameterIndex { get; private set; }

        public byte ToLambdaParameterIndex { get; private set; }

        public ValueFlowAttribute(PointPlurality fromPlurality, PointKinds fromPointKinds, byte fromParameterIndex, byte fromLambdaIndex,
                                  PointPlurality toPlurality, PointKinds toPointKinds, byte toParameterIndex, byte toLambdaParameterIndex)
        {
            FromPlurality = fromPlurality;
            FromPointKind = fromPointKinds;
            FromParameterIndex = fromParameterIndex;
            FromLambdaIndex = fromLambdaIndex;
            ToPlurality = toPlurality;
            ToPointKind = toPointKinds;
            ToParameterIndex = toParameterIndex;
            ToLambdaParameterIndex = toLambdaParameterIndex;
        }
    }

    /// <summary>
    /// Indicates the property of some <c>BindingBase</c>-derived type, that is used
    /// to bind some item of <c>ItemsControl</c>-derived type. This annotation will
    /// enable the <c>DataContext</c> type resolve for XAML bindings for such properties.
    /// </summary>
    /// <remarks>
    /// Property should have the tree ancestor of the <c>ItemsControl</c> type or
    /// marked with the <see cref="T:JetBrains.Annotations.XamlItemsControlAttribute"/> attribute.
    /// </remarks>
    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public sealed class XamlItemBindingOfItemsControlAttribute : Attribute
    {
    }

    /// <summary>
    /// Indicates the type that has <c>ItemsSource</c> property and should be treated
    /// as <c>ItemsControl</c>-derived type, to enable inner items <c>DataContext</c> type resolve.
    /// </summary>
    [UsedImplicitly]
    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    [GeneratedCode("ReSharper", "7.0.97.60")]
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class XamlItemsControlAttribute : Attribute
    {
    }
}
// ReSharper restore IntroduceOptionalParameters.Global
// ReSharper restore CheckNamespace