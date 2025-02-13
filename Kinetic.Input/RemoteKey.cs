namespace Kinetic.Input;

public interface RemoteKey
{

	public static RemoteKeyBind Any = new RemoteKeyBind(RemoteKeyID.ANY);
	public static RemoteKeyBind MouseLeft = new RemoteKeyBind(RemoteKeyID.BUTTON_LEFT);
	public static RemoteKeyBind MouseRight = new RemoteKeyBind(RemoteKeyID.BUTTON_RIGHT);
	public static RemoteKeyBind MouseMid = new RemoteKeyBind(RemoteKeyID.BUTTON_MIDDLE);
	public static RemoteKeyBind KeySpace = new RemoteKeyBind(RemoteKeyID.KEY_SPACE);
	public static RemoteKeyBind KeyShift = new RemoteKeyBind(RemoteKeyID.KEY_LEFT_SHIFT);
	public static RemoteKeyBind KeyCtrl = new RemoteKeyBind(RemoteKeyID.KEY_LEFT_CONTROL);
	public static RemoteKeyBind KeyAlt = new RemoteKeyBind(RemoteKeyID.KEY_LEFT_ALT);
	public static RemoteKeyBind KeyEnter = new RemoteKeyBind(RemoteKeyID.KEY_ENTER);
	public static RemoteKeyBind KeyBackspace = new RemoteKeyBind(RemoteKeyID.KEY_BACKSPACE);

	void Consume();

	int HoldTime();

	bool Pressed();

	bool Holding();

	bool DoublePressed();

}