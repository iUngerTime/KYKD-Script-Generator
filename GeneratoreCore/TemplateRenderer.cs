using System.Text.RegularExpressions;

namespace GeneratoreCore;

public static class TemplateRenderer
{
	private static readonly Regex TokenRegex = new(@"\{\{([A-Za-z0-9_]+)\}\}", RegexOptions.Compiled);

	public static string Render(string templatePath, ScriptContext ctx)
	{
		string template = File.ReadAllText(templatePath);
		var map = ctx.AsDictionary();

		return TokenRegex.Replace(template, m =>
		{
			var key = m.Groups[1].Value;
			return map.TryGetValue(key, out var val) ? val : $"[[MISSING:{key}]]";
		});
	}
}