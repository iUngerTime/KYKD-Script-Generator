using GeneratorCore.Models.Enums;

namespace GeneratoreCore;

public interface IScriptDataProvider
{
	public ScriptContext Build(DateOnly date, TimeOfDay timeOfDay);
}
