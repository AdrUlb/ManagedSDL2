using static Native.SDL;
using static Native.SDL.SDL_BlendMode;
using static Native.SDL.SDL_TextureAccess;

namespace ManagedSDL2
{
	public static partial class SDL
	{
		public enum InitFlags : uint
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

		public enum BlendMode
		{
			None = SDL_BLENDMODE_NONE,
			Blend = SDL_BLENDMODE_BLEND,
			Add = SDL_BLENDMODE_ADD,
			Mod = SDL_BLENDMODE_MOD,
			Mul = SDL_BLENDMODE_MUL,
			Invalid = SDL_BLENDMODE_INVALID
		}

		public static class PixelFormat
		{
			public static uint Unknown => SDL_PIXELFORMAT_UNKNOWN;
			public static uint Index1LSB => SDL_PIXELFORMAT_INDEX1LSB;
			public static uint Index1MSB => SDL_PIXELFORMAT_INDEX1MSB;
			public static uint Index4LSB => SDL_PIXELFORMAT_INDEX4LSB;
			public static uint Index4MSB => SDL_PIXELFORMAT_INDEX4MSB;
			public static uint Index8 => SDL_PIXELFORMAT_INDEX8;
			public static uint RGB332 => SDL_PIXELFORMAT_RGB332;
			public static uint XRGB444 => SDL_PIXELFORMAT_XRGB444;
			public static uint RGB444 => SDL_PIXELFORMAT_RGB444;
			public static uint XBGR444 => SDL_PIXELFORMAT_XBGR444;
			public static uint BGR444 => SDL_PIXELFORMAT_BGR444;
			public static uint XRGB1555 => SDL_PIXELFORMAT_XRGB1555;
			public static uint RGB555 => SDL_PIXELFORMAT_RGB555;
			public static uint XBGR1555 => SDL_PIXELFORMAT_XBGR1555;
			public static uint BGR555 => SDL_PIXELFORMAT_BGR555;
			public static uint ARGB4444 => SDL_PIXELFORMAT_ARGB4444;
			public static uint RGBA4444 => SDL_PIXELFORMAT_RGBA4444;
			public static uint ABGR4444 => SDL_PIXELFORMAT_ABGR4444;
			public static uint BGRA4444 => SDL_PIXELFORMAT_BGRA4444;
			public static uint ARGB1555 => SDL_PIXELFORMAT_ARGB1555;
			public static uint RGBA5551 => SDL_PIXELFORMAT_RGBA5551;
			public static uint ABGR1555 => SDL_PIXELFORMAT_ABGR1555;
			public static uint BGRA5551 => SDL_PIXELFORMAT_BGRA5551;
			public static uint RGB565 => SDL_PIXELFORMAT_RGB565;
			public static uint BGR565 => SDL_PIXELFORMAT_BGR565;
			public static uint RGB24 => SDL_PIXELFORMAT_RGB24;
			public static uint BGR24 => SDL_PIXELFORMAT_BGR24;
			public static uint XRGB888 => SDL_PIXELFORMAT_XRGB888;
			public static uint RGB888 => SDL_PIXELFORMAT_RGB888;
			public static uint RGBX8888 => SDL_PIXELFORMAT_RGBX8888;
			public static uint XBGR888 => SDL_PIXELFORMAT_XBGR888;
			public static uint BGR888 => SDL_PIXELFORMAT_BGR888;
			public static uint BGRX8888 => SDL_PIXELFORMAT_BGRX8888;
			public static uint ARGB8888 => SDL_PIXELFORMAT_ARGB8888;
			public static uint RGBA8888 => SDL_PIXELFORMAT_RGBA8888;
			public static uint ABGR8888 => SDL_PIXELFORMAT_ABGR8888;
			public static uint BGRA8888 => SDL_PIXELFORMAT_BGRA8888;
			public static uint ARGB2101010 => SDL_PIXELFORMAT_ARGB2101010;
			public static uint YV12 => SDL_PIXELFORMAT_YV12;
			public static uint IYUV => SDL_PIXELFORMAT_IYUV;
			public static uint YUY2 => SDL_PIXELFORMAT_YUY2;
			public static uint UYVY => SDL_PIXELFORMAT_UYVY;
			public static uint YVYU => SDL_PIXELFORMAT_YVYU;
		}

		public enum TextureAccess
		{
			Static = SDL_TEXTUREACCESS_STATIC,
			Streaming = SDL_TEXTUREACCESS_STREAMING,
			Target = SDL_TEXTUREACCESS_TARGET
		}

		public delegate void QuitEventHandler();

		public static class WindowPos
		{
			public const int Undefined = SDL_WINDOWPOS_UNDEFINED;
			public const int Centered = SDL_WINDOWPOS_CENTERED;
		}

		public static event QuitEventHandler? Quit;

		public static void Init(InitFlags flags)
		{
			if (SDL_Init((uint)flags) != 0)
				throw new ErrorException(SDL_GetError());
		}

		public static void ProcessEvents()
		{
			while (SDL_PollEvent(out var _event) != 0)
			{
				switch (_event.type)
				{
					case SDL_EventType.SDL_WINDOWEVENT:
						{
							var sdlWindowPtr = SDL_GetWindowFromID(_event.window.windowID);
							Window? window = null;

							foreach (var win in existingWindows)
							{
								if (win.SdlWindowPtr == sdlWindowPtr)
								{
									window = win;
								}
							}

							if (window == null)
								break;

							switch (_event.window.windowEvent)
							{
								case SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE:
									window.HandleCloseRequested();
									break;
							}
						}
						break;
					case SDL_EventType.SDL_QUIT:
						Quit?.Invoke();
						break;
				}
			}
		}
	}
}
