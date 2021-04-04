using System.Dynamic;

namespace DungeonRpg.ViewModels.Helpers
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
