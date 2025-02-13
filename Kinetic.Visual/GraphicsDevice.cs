using Kinetic.Math;

namespace Kinetic.App;

public interface GraphicsDevice
{

	public static GraphicsDevice Global;

	string Title { set; }

	Vector2 Size { get; set; }

	Vector2 DeviceSize { get; }

	Vector2 Pos { get; set; }

	bool Decorated { set; }

	void Maximize();

	void SwapBuffer();

}
