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

			public int Width { get; private set; }
			public int Height { get; private set; }
			public uint PixelFormat { get; private set; }
			public TextureAccess Access { get; private set; }

			public bool Locked { get; private set; } = false;

			public Texture(Renderer renderer, uint pixelFormat, TextureAccess textureAccess, int width, int height)
			{
				SdlTexturePtr = SDL_CreateTexture(renderer.SdlRendererPtr, pixelFormat, (int)textureAccess, width, height);

				Query();
			}

			internal Texture(IntPtr sdlTexturePtr)
			{
				SdlTexturePtr = sdlTexturePtr;

				Query();
			}

			private void Query()
			{
				SDL_QueryTexture(SdlTexturePtr, out uint format, out var access, out var w, out var h);

				Width = w;
				Height = h;
				PixelFormat = format;
				Access = (TextureAccess)access;
			}

			public (IntPtr pixelsPtr, int pitch) Lock(Rectangle? rect = null)
			{
				if (Locked)
					throw new InvalidOperationException("Texture already locked");

				Locked = true;

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

			public void Unlock()
			{
				if (!Locked)
					throw new Exception("Texture was not locked");

				SDL_UnlockTexture(SdlTexturePtr);
				
				Locked = false;
			}

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
