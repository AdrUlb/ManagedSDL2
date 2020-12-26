using System;
using System.Drawing;
using static Native.SDL;

namespace ManagedSDL2
{
	public static partial class SDL
	{
		public class Texture : IDisposable
		{
			internal readonly IntPtr SdlTexturePtr;

			public Texture(Renderer renderer, uint pixelFormat, TextureAccess textureAccess, int width, int height)
			{
				SdlTexturePtr = SDL_CreateTexture(renderer.SdlRendererPtr, pixelFormat, (int)textureAccess, width, height);
			}

			public (IntPtr pixelsPtr, int pitch) Lock(Rectangle? rect = null)
			{
				if (rect == null)
				{
					_ = SDL_LockTexture(SdlTexturePtr, IntPtr.Zero, out var pixels, out var pitch);

					return (pixels, pitch);
				}

				{
					var sdlRect = Util.CreateSDLRect(rect.Value);
					_ = SDL_LockTexture(SdlTexturePtr, ref sdlRect, out var pixels, out var pitch);
					return (pixels, pitch);
				}
			}

			public void Unlock() => SDL_UnlockTexture(SdlTexturePtr);

			bool disposed = false;

			public void Dispose()
			{
				if (disposed)
					return;

				GC.SuppressFinalize(this);
				SDL_DestroyTexture(SdlTexturePtr);

				disposed = true;
			}
		}
	}
}
