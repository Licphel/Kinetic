namespace Kinetic.Visual;

public interface TextureBuffer
{

	public Texture Src { get; }

	void Begin(SpriteBatch batch);

	void End(SpriteBatch batch);

	void Free();

}
