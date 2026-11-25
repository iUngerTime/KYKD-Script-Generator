// Reads the KYKD tides and daylight spreadsheets into raw cell values with ExcelDataReader,
// plus the small conversions the parsers need to hand those values to the model constructors.
using System.Globalization;
using ExcelDataReader;

namespace GeneratorCore.Helpers;

static class ExcelSheetReader
{
	/// <summary>
	/// Reads every row of the first worksheet as raw cell values.
	/// </summary>
	/// <param name="excelDataPath">Path to the xlsx file.</param>
	/// <returns>One array per row, null for empty cells; index 0 is spreadsheet row 1.</returns>
	public static List<object?[]> ReadFirstSheet(string excelDataPath)
	{
		// ExcelDataReader needs the legacy code pages registered, even for xlsx.
		System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

		List<object?[]> rows = new();
		using (FileStream stream = File.OpenRead(excelDataPath))
		using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
		{
			while (reader.Read())
			{
				object[] values = new object[reader.FieldCount];
				reader.GetValues(values);
				rows.Add(values.Select(v => v is DBNull ? null : v).ToArray());
			}
		}

		return rows;
	}

	/// <summary>
	/// Gets a cell by zero-based column index.
	/// </summary>
	/// <param name="row">The row values.</param>
	/// <param name="column">Zero-based column index.</param>
	/// <returns>The cell value, or null when the row is shorter than the column.</returns>
	public static object? Cell(object?[] row, int column)
	{
		return column < row.Length ? row[column] : null;
	}

	/// <summary>
	/// Whether a row is blank or a "====" separator row, judged by its first data cell.
	/// </summary>
	/// <param name="firstCell">The row's first data cell.</param>
	/// <returns>True when the row holds no data.</returns>
	public static bool IsSkippable(object? firstCell)
	{
		string text = ToInvariantString(firstCell);
		return string.IsNullOrWhiteSpace(text) || text.Contains('=');
	}

	/// <summary>
	/// Formats a time cell as "HH:mm", which is what Excel shows for the sheets' "h:mm" formats.
	/// </summary>
	/// <param name="value">The raw cell value.</param>
	/// <returns>The time as "HH:mm", or the value's text when it is not a time.</returns>
	public static string ToTimeString(object? value)
	{
		switch (value)
		{
			case DateTime dateTime:
				return dateTime.ToString("HH:mm", CultureInfo.InvariantCulture);
			case TimeSpan span:
				return $"{(int)span.TotalHours:00}:{span.Minutes:00}";
			case double serial:
				return DateTime.FromOADate(serial).ToString("HH:mm", CultureInfo.InvariantCulture);
			default:
				return ToInvariantString(value);
		}
	}

	/// <summary>
	/// Converts a cell value to text using the invariant culture.
	/// </summary>
	/// <param name="value">The raw cell value.</param>
	/// <returns>The text, or an empty string for an empty cell.</returns>
	public static string ToInvariantString(object? value)
	{
		return Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
	}
}
