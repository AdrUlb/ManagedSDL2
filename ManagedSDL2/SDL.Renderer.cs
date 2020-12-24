using System;
using System.Drawing;
using static Native.SDL;
using static Native.SDL.SDL_RendererFlags;

namespace ManagedSDL2
{
	public static partial class SDL
	{
		public class Renderer : IDisposable
		{
			IntPtr sdlRendererPtr;

			public Renderer(Window window, int deviceIndex = -1, bool accelerated = true)
			{
				sdlRendererPtr = SDL_CreateRenderer(window.SdlWindowPtr, deviceIndex, accelerated ? SDL_RENDERER_ACCELERATED : SDL_RENDERER_SOFTWARE);
				SDL_SetRenderDrawBlendMode(sdlRendererPtr, SDL_BlendMode.SDL_BLENDMODE_BLEND);
			}

			private void SetColor(Color color) => SDL_SetRenderDrawColor(sdlRendererPtr, color.R, color.G, color.B, color.A);

			public void DrawPoint(Color color, int x, int y)
			{
				SetColor(color);
				SDL_RenderDrawPoint(sdlRendererPtr, x, y);
			}

			public void Clear(Color color)
			{
				SetColor(color);
				SDL_RenderClear(sdlRendererPtr);
			}

			public void Clear() => Clear(Color.Black);

			public void Present() => SDL_RenderPresent(sdlRendererPtr);

			bool disposed = false;

			public void Dispose()
			{
				if (disposed)
					return;

				SDL_DestroyRenderer(sdlRendererPtr);

				disposed = true;
			}
		}
	}
}
