using GeneratorCore.Models.Enums;

namespace GeneratoreCore;

public class TideScriptGenerator
{
	private readonly IScriptDataProvider _provider;
	private readonly string _templatePath;

	public TideScriptGenerator(IScriptDataProvider provider, string templatePath)
	{
		_provider = provider;
		_templatePath = templatePath;
	}

	public string Generate(DateOnly date, TimeOfDay timeOfDay)
	{
		var ctx = _provider.Build(date, timeOfDay);
		return TemplateRenderer.Render(_templatePath, ctx);
	}
}