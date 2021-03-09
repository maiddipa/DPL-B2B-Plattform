using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dpl.B2b.BusinessLogic.State;
using Dpl.B2b.Contracts.Localizable;
using JetBrains.Annotations;

namespace Dpl.B2b.BusinessLogic.Rules
{
    [Serializable]
    public class RuleStateDictionary: IDictionary<string, RuleState>
    {
        public RuleStateDictionary()
        {
        }

        public RuleStateDictionary(RuleStateDictionary dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            foreach (var entry in dictionary)
            {
                _innerDictionary.Add(entry.Key, entry.Value);
            }
        }

        private Dictionary<string, RuleState> _innerDictionary = new Dictionary<string, RuleState>(StringComparer.OrdinalIgnoreCase);

        public int Count => _innerDictionary.Count;

        public ICollection<string> Keys => _innerDictionary.Keys;

        public ICollection<RuleState> Values => _innerDictionary.Values;

        public bool IsReadOnly => ((IDictionary<string, RuleState>)_innerDictionary).IsReadOnly;

        public RuleState this[string key]
        {
            get
            {
                _innerDictionary.TryGetValue(key, out var value);
                return value;
            }
            set => _innerDictionary[key] = value;
        }

        public bool Remove(string key)
        {
            return _innerDictionary.Remove(key);
        }

        public void Clear()
        {
            _innerDictionary.Clear();
        }

        public bool IsValid()
        {
            var isValidAll =_innerDictionary.Values.All(rule => rule.RuleStateItems.Count(ruleState => ruleState.Type == StateItemType.Error) == 0);
            
            return isValidAll;
        }

        public bool HasWarnings()
        {
            var hasWarningsAll = _innerDictionary.Values.All(rule => rule.RuleStateItems.Count(ruleState => ruleState.Type == StateItemType.Warning) > 0);

            return hasWarningsAll;
        }

        public void Merge(RuleStateDictionary dictionary)
        {
            if (dictionary == null)
            {
                return;
            }

            foreach (var entry in dictionary)
            {
                if (this.ContainsKey(entry.Key))
                {
                    foreach (var ruleStateItem in entry.Value.RuleStateItems)
                    {
                        this[entry.Key].RuleStateItems.Add(ruleStateItem);
                    }
                }
                else
                {
                    this[entry.Key] = entry.Value;
                }
                
            }
        }

        public bool ContainsKey(string key)
        {
            return _innerDictionary.ContainsKey(key);
        }

        public bool TryGetValue(string key, out RuleState value)
        {
            return _innerDictionary.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, RuleState> item)
        {
            ((IDictionary<string, RuleState>)_innerDictionary).Add(item);
        }

        public void Add(string key, RuleState value)
        {
            if (_innerDictionary.ContainsKey(key))
            {
                foreach (var ruleStateItem in value.RuleStateItems)
                {
                    var items= _innerDictionary[key].RuleStateItems;

                    // No duplicate
                    if (!items.Contains(ruleStateItem))
                    {
                        items.Add(ruleStateItem);    
                    }
                }
            }
            else
            {
                _innerDictionary.Add(key, value);
            }
            
        }

        public bool Contains(KeyValuePair<string, RuleState> item)
        {
            return ((IDictionary<string, RuleState>)_innerDictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, RuleState>[] array, int arrayIndex)
        {
            ((IDictionary<string, RuleState>)_innerDictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, RuleState> item)
        {
            return ((IDictionary<string, RuleState>)_innerDictionary).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, RuleState>> GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

        public bool IsValidField(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            // if the key is not found in the dictionary, we just say that it's valid (since there are no errors)
            return DictionaryHelpers.FindKeysWithPrefix(this, key).All(entry => entry.Value.RuleStateItems.Any(v=>v.Type==StateItemType.Error));
        }

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_innerDictionary).GetEnumerator();
        }

        #endregion

        public void AddMessage([NotNull] string key, [NotNull] ILocalizableMessage message)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (message == null) throw new ArgumentNullException(nameof(message));

            var ruleState = new RuleState {RuleStateItems = {message}};
            
            Add(key,ruleState);
        }
        
        public void AddMessage(bool constrain, [NotNull] string key, [NotNull] ILocalizableMessage message)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (message == null) throw new ArgumentNullException(nameof(message));
            
            if (constrain)
            {
                AddMessage(key, message);
            }
        }
    }
}