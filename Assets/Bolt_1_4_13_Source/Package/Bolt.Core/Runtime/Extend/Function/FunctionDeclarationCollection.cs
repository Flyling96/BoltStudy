using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ludiq;

namespace Bolt.Extend
{

    [SerializationVersion("A")]
    public class FunctionDeclarationCollection<TFunctionElement> : KeyedCollection<string, TFunctionElement>, IKeyedCollection<string, TFunctionElement>
        where TFunctionElement: IFunctionElement
    {

        protected override string GetKeyForItem(TFunctionElement item)
        {
            return item.name;
        }

        public void EditorRename(TFunctionElement item,string newName)
        {
            ChangeItemKey(item, newName);
        }

        public bool TryGetValue(string key, out TFunctionElement value)
        {
            if(Dictionary == null)
            {
                value = default;
                return false;
            }

            return Dictionary.TryGetValue(key, out value);
        }

    }
}
