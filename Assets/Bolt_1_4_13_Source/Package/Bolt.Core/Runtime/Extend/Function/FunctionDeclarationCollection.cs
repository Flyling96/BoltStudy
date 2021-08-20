using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ludiq;

namespace Bolt.Extend
{
    [SerializationVersion("A")]
    public class FunctionDeclarationCollection : KeyedCollection<string, FunctionDeclaration>, IKeyedCollection<string, FunctionDeclaration>
    {

        protected override string GetKeyForItem(FunctionDeclaration item)
        {
            return item.name;
        }

        public void EditorRename(FunctionDeclaration item,string newName)
        {
            ChangeItemKey(item, newName);
        }

        public bool TryGetValue(string key, out FunctionDeclaration value)
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
