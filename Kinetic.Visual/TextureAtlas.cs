namespace Kinetic.Visual;

public interface TextureAtlas
{

	public void Begin();

	public TexturePart Accept(Texture tex);

	public void End();

}
