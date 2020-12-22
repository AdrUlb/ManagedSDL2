using static Native.SDL;

namespace ManagedSDL2
{
	public enum SDLInitFlags : uint
	{
		Timer = SDL_INIT_TIMER,
		Audio = SDL_INIT_AUDIO,
		Video = SDL_INIT_VIDEO,
		Joystick = SDL_INIT_JOYSTICK,
		Haptic = SDL_INIT_HAPTIC,
		GameController = SDL_INIT_GAMECONTROLLER,
		Events = SDL_INIT_EVENTS,
		Sensors = SDL_INIT_SENSOR,
		Everything = SDL_INIT_EVERYTHING
	}

	public static class SDL
	{
		public delegate void QuitEventHandler();

		public static int WindowPosUndefined => SDL_WINDOWPOS_UNDEFINED;
		public static int WindowPosCentered => SDL_WINDOWPOS_CENTERED;

		public static event QuitEventHandler? Quit;

		public static void Init(SDLInitFlags flags)
		{
			SDL_Init((uint)flags);
		}

		public static void ProcessEvents()
		{
			while (SDL_PollEvent(out var _event) != 0)
			{
				switch (_event.type)
				{
					case SDL_EventType.SDL_QUIT:
						Quit?.Invoke();
						break;
				}
			}
		}
	}
}
