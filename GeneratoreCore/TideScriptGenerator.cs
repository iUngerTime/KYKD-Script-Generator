using GeneratorCore.Models.Enums;
using GeneratoreCore.Helpers;

namespace GeneratoreCore;

public class TideScriptGenerator
{
	private readonly IScriptDataProvider _provider;
	private readonly string _daylightExcelDataPath;
	private readonly string _tidesExcelDataPath;
	private readonly string _templatePath;

	public TideScriptGenerator(IScriptDataProvider provider, string daylightExcelDataPath,
		string tidesExcelDataPath, string templatePath)
	{
		_provider = provider;
		_daylightExcelDataPath = daylightExcelDataPath;
		_tidesExcelDataPath = tidesExcelDataPath;
		_templatePath = templatePath;
	}

	public string Generate(DateOnly date, TimeOfDay timeOfDay)
	{
		ScriptContext ctx = _provider.Build(date, timeOfDay);
		return TemplateRenderer.Render(_templatePath, ctx);
	}
}