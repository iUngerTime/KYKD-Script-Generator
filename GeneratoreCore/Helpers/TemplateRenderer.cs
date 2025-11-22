using System.Text.RegularExpressions;

namespace GeneratoreCore.Helpers;

public static class TemplateRenderer
{
	private static readonly Regex TokenRegex = new(@"\{\{([A-Za-z0-9_]+)\}\}", RegexOptions.Compiled);

	public static string Render(string templatePath, ScriptContext ctx)
	{
		string template = File.ReadAllText(templatePath);
		Dictionary<string, string> map = ctx.AsDictionary();

		return TokenRegex.Replace(template, m =>
		{
			string key = m.Groups[1].Value;
			return map.TryGetValue(key, out string? val) ? val : $"[[MISSING:{key}]]";
		});
	}
}