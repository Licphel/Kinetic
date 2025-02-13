namespace Kinetic.Visual;

public class IconMock : Icon
{

	public static IconMock Instance = new IconMock();

	private IconMock() {}

	public void Draw(SpriteBatch batch, float x, float y, float w, float h)
	{
	}

}