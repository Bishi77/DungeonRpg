using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonRpg.ViewModel.Helpers
{
    public interface ICanEnableField
    {
        bool CanEnableField(string key);
    }

    public sealed class CanEnableFieldHelper : DynamicObject
    {
        private readonly ICanEnableField _owner;

        public CanEnableFieldHelper(ICanEnableField owner)
        {
            _owner = owner;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _owner.CanEnableField(binder.Name);
            return true;
        }
    }
}
