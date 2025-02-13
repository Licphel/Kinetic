using Kinetic.Math;

namespace Kinetic.Input;

public interface RemoteKeyboard
{

	public static RemoteKeyboard Global;

	VaryingVector2 Cursor { get; }
	ScrollDirection ScrollDirection { get; }
	float Scroll { get; }
	string ClippedText { get; set; }
	string Text { get; }

	RemoteKey Observe(RemoteKeyID code);

	RemoteKey Observe(int code);

	void ConsumeTextInput();

	void ConsumeCursorScroll();

	void StartRoll();

	void EndRoll();

}

public enum ScrollDirection
{

	NONE,
	UP,
	DOWN

}
