using System.Text;
using Kinetic.App;
using Kinetic.Math;

namespace Kinetic.Visual;

public struct Lore
{

	public List<(LoreStyle, Func<string>)> Content = new();
	public LoreStyle _LatestSP = null;

	public Lore() {}

	public string Summary
	{
		get
		{
			var sb = new StringBuilder();
			foreach(var s in Content)
				sb.Append(s.Item2());
			return sb.ToString();
		}
	}

	public static Lore Dynamic(Func<string> text)
	{
		Lore lr = new Lore();
		lr.Content.Add((lr._LatestSP = new LoreStyle(), text));
		return lr;
	}

	public static Lore Literal(string text = "")
	{
		return Dynamic(() => text);
	}

	public static Lore Translate(string text, params string[] repmt)
	{
		return Dynamic(() => I18N.GetText(text, repmt));
	}

	public Lore Style(LoreStyle sp)
	{
		int i = Content.Count - 1;
		Content[i] = (sp, Content[i].Item2);
		return this;
	}

	public Lore Paint(Vector4 c)
	{
		_LatestSP.Color = c;
		return this;
	}

	public Lore Paint(float r, float g, float b, float a = 1)
	{
		_LatestSP.Color = new Vector4(r, g, b, a);
		return this;
	}

	public Lore Combine(Lore l1)
	{
		foreach(var v in l1.Content)
		{
			Content.Add(v);
		}
		return this;
	}

}
