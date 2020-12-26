using System;
using System.Collections.Generic;
using static Native.SDL;
using static Native.SDL.SDL_WindowFlags;

namespace ManagedSDL2
{
	public static partial class SDL
	{
		static readonly List<Window> existingWindows = new List<Window>();

		public class Window : IDisposable
		{
			internal IntPtr SdlWindowPtr;

			public string Title
			{
				get => SDL_GetWindowTitle(SdlWindowPtr);
				set => SDL_SetWindowTitle(SdlWindowPtr, value);
			}

			public (int X, int Y) Position
			{
				get
				{
					SDL_GetWindowPosition(SdlWindowPtr, out var x, out var y);
					return (x, y);
				}

				set => SDL_SetWindowPosition(SdlWindowPtr, value.X, value.Y);
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

			public delegate void CloseRequestedEventHandler();

			public event CloseRequestedEventHandler? CloseRequested;

			protected virtual void OnCloseRequested() { }

			internal void HandleCloseRequested()
			{
				CloseRequested?.Invoke();
				OnCloseRequested();
			}

			public Window(string title, int x, int y, int width, int height, bool shown = true)
			{
				SdlWindowPtr = SDL_CreateWindow(title, x, y, width, height, shown ? SDL_WINDOW_SHOWN : SDL_WINDOW_HIDDEN);

				if (SdlWindowPtr == IntPtr.Zero)
					throw new ErrorException(SDL_GetError());

				existingWindows.Add(this);
			}

			public void Show() { SDL_ShowWindow(SdlWindowPtr); }

			public void Hide() => SDL_HideWindow(SdlWindowPtr);

			public Surface GetSurface() => new Surface(SDL_GetWindowSurface(SdlWindowPtr));

			bool disposed = false;

			public void Dispose()
			{
				if (disposed)
					return;

				GC.SuppressFinalize(this);
				existingWindows.Remove(this);
				SDL_DestroyWindow(SdlWindowPtr);

				disposed = true;
			}
		}
	}
}
