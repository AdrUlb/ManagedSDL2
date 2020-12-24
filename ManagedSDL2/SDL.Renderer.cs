using System;
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
