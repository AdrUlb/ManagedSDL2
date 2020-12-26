using System;
using System.Drawing;
using static Native.SDL;

namespace ManagedSDL2
{
	public static partial class SDL
	{
		public class Surface : IDisposable
		{
			internal readonly IntPtr SdlSurfacePtr;

			internal Surface (IntPtr sdlSurfacePtr)
			{
				this.SdlSurfacePtr = sdlSurfacePtr;
			}

			public Surface(int width, int height)
			{
				SdlSurfacePtr = SDL_CreateRGBSurface(0, width, height, 32, 0, 0, 0, 0);
			}

			public void BlitOnto(Rectangle srcRect, Surface dst, Rectangle dstRect)
			{
				var sdlSrcRect = Util.CreateSDLRect(srcRect);
				var sdlDstRect = Util.CreateSDLRect(dstRect);
				_ = SDL_BlitSurface(SdlSurfacePtr, ref sdlSrcRect, dst.SdlSurfacePtr, ref sdlDstRect);
			}

			bool disposed = false;

			public void Dispose()
			{
				if (disposed)
					return;

				GC.SuppressFinalize(this);
				SDL_FreeSurface(SdlSurfacePtr);

				disposed = true;
			}
		}
	}
}
