using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq.Expressions;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;

namespace Dpl.B2b.BusinessLogic.Rules.Common
{
    /// <summary>
    /// Universelle Resource überprüfung,
    /// Wenn ein Property auf dem Context null ist dann ist der Wert als required zu verstehen
    /// Wenn ein Property auf dem Context eine IRule ist dann wird dieser ausgewertet
    /// </summary>
    public class ResourceValidRule : BaseValidationRule, IParentChildRule, IWithLocalizableMessage
    {
        public ILocalizableMessage Message { get; } = new ResourceValid();

        private dynamic _context;

        public dynamic Context
        {
            get => _context ??= new ExpandoObject();
            set => _context = value;
        }

        public IDictionary<string, dynamic> ResourcesDictionary => Context;
        
        public ResourceValidRule()
        {
            ResourceName = nameof(ResourceValidRule);
            ((INotifyPropertyChanged)Context).PropertyChanged += HandlePropertyChanges;
        }
        
        public ResourceValidRule(IRule parentRule): this()
        {
            ParentRule = parentRule;
        }

        public IRule ParentRule { get; set; }

        private static (object Value, string ParamName) GetParameterInfo
            (Expression<Func<object>> expr)
        {
            //First try to get a member expression directly.
            //If it fails we know there is a type conversion: parameter is not an object 
            //(or we have an invalid lambda that will make us crash)
            //Get the "to object" conversion unary expression operand and then 
            //get the member expression of that and we're set.
            var m = (expr.Body as MemberExpression) ??
                    (expr.Body as UnaryExpression).Operand as MemberExpression;

            return (expr.Compile().Invoke(), m.Member.Name);
        }
        
        public override void Evaluate()
        {
            foreach (var property in (IDictionary<string, dynamic>)Context)
            {
                if (property.Value is IValidationRule validationRule)
                {
                    var valid= validationRule.IsValid();
                    // There is a valid function -> No error
                    if (valid) continue;
                    
                    // Falls ParentRule eine BaseValidationRule nimm den RessourceName,
                    // ansonsten den Key des Ressource property  
                    var key = ParentRule switch
                    {
                        BaseValidationRule p => p.ResourceName,
                        _ => property.Key
                    };

                    var message = property.Value is IWithLocalizableMessage item 
                        ? item.Message 
                        : Message;
                    
                    
                    RuleState.AddMessage($"{key}#{property.Key}", message);
                }
                else
                {
                    // There is a resource value -> No error
                    if (property.Value != null) continue;
                
                    // There is no ressource -> Not valid
                    // Falls ParentRule eine BaseValidationRule nimm den RessourceName,
                    // ansonsten den Key des Ressource property  
                    var key = ParentRule switch
                    {
                        BaseValidationRule p => p.ResourceName,
                        _ => property.Key
                    };
                
                    RuleState.AddMessage(key, Message);
                }
            }
        }

        public ResourceValidRule AddResource(dynamic resource, string name)
        {
            ResourcesDictionary.Add(name, resource);

            return this;
        }
        
        public ResourceValidRule AddResource<T>(ResourceMeta<T> resourceMeta)
        {
            ResourcesDictionary.Add(resourceMeta.Name, resourceMeta.Resource);

            return this;
        }
        
        public ResourceValidRule AddResource<T>(T resource, string name)
        {
            ResourcesDictionary.Add(name, resource);
            
            return this;
        }

        public class ResourceMeta<T>
        {
            public string Name { get; set; } 
            public T Resource { get; set; } 
        }
        
        private static void HandlePropertyChanges(object sender, PropertyChangedEventArgs e)
        {
            //Debug.WriteLine("{0} has changed.", e.PropertyName);
        }

       

       
    }
}