using System;
using static Native.SDL;
using static Native.SDL.SDL_WindowFlags;

namespace ManagedSDL2
{
	public class SDLWindow : IDisposable
	{
		IntPtr sdlWindowPtr;

		public SDLWindow(string title, int x, int y, int w, int h, bool shown = true)
		{
			sdlWindowPtr = SDL_CreateWindow(title, x, y, w, h, shown ? SDL_WINDOW_SHOWN : SDL_WINDOW_HIDDEN);
		}

		~SDLWindow() => Dispose();

		bool disposed = false;

		public void Dispose()
		{
			if (disposed)
				return;

			GC.SuppressFinalize(this);
			SDL_DestroyWindow(sdlWindowPtr);

			disposed = true;
		}
	}
}
