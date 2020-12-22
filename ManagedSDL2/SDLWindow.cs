using System;
using static Native.SDL;
using static Native.SDL.SDL_WindowFlags;

namespace ManagedSDL2
{
	public class SDLWindow : IDisposable
	{
		internal IntPtr SdlWindowPtr;

		public string Title
		{
			get => SDL_GetWindowTitle(SdlWindowPtr);
			set => SDL_SetWindowTitle(SdlWindowPtr, value);
		}

		public (int Width, int Height) Size
		{
			get
			{
				SDL_GetWindowSize(SdlWindowPtr, out var w, out var h);
				return (w, h);				
			}

			set => SDL_SetWindowSize(SdlWindowPtr, value.Width, value.Height);
		}

		public SDLWindow(string title, int x, int y, int width, int height, bool shown = true)
		{
			SdlWindowPtr = SDL_CreateWindow(title, x, y, width, height, shown ? SDL_WINDOW_SHOWN : SDL_WINDOW_HIDDEN); 
		}

		~SDLWindow() => Dispose();

		bool disposed = false;

		public void Dispose()
		{
			if (disposed)
				return;

			GC.SuppressFinalize(this);
			SDL_DestroyWindow(SdlWindowPtr);

			disposed = true;
		}
	}
}
