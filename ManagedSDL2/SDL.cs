﻿using static Native.SDL;
using static Native.SDL.SDL_BlendMode;

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
