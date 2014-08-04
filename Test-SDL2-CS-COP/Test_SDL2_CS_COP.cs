using System;
using SDL2_CS_Bridge;
using SDL2_CS_COP;

namespace TestSDL2CSCOP
{
	/// <summary>
	/// The Pong Game
	/// 
    /// Ported from the Python paddle game example
	/// for PySDL
	///
	/// </summary>
    /// <remarks>
    /// This program simulates a simple paddle game
    /// using the COP system.
    /// 
    /// The objects consist of two paddles
    /// (one controlled by the keyboard
    /// and the other controlled by an AI)
    /// which bat a ball back and forth.
    /// 
    /// The implementation consists of three entities:
    /// - <see cref="PlayerEntity"/> player1
    /// - <see cref="PlayerEntity"/> player2
    /// - <see cref="BallEntity"/> ball
    /// 
    /// And four systems:
    /// - <see cref="TrackingAIController"/> Tracking AI Controller
    /// - <see cref="SDL2_CS_COP.StandardItems.Systems.MovementSystem"/> Movement System
    /// - <see cref="CollisionSystem"/> Collision System
    /// - <see cref="SDL2_CS_COP.StandardItems.Systems.SpriteRenderer"/> Sprite Renderer
    /// </remarks>
	class MainClass
	{
       
        private enum RenderingType {
            HARDWARE,
            SOFTWARE
        }

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">[-hardware] for hardware rendering</param>
		public static void Main (string[] args)
		{
            //TODO: parse the argument to look for -hardware
            // and set the rendering accordingly

			// Let Run() do all the work, as in Python
            run (RenderingType.HARDWARE);
		}

		/// <summary>
		/// Run this instance.
		/// </summary>
        /// <param name="renderingType">Whether software or hardware rendering is used (currently ignored)</param>
		private static void run(RenderingType renderingType)
		{
            SDL2_CS_Bridge.Color WHITE = new SDL2_CS_Bridge.Color (255, 255, 255);
            SDL2_CS_Bridge.Color BG_COLOR = new SDL2_CS_Bridge.Color (127, 127, 127);
            /*
             * sdl2.ext.init()
             */
			// in SDL2-CS-COP, the world is the base instance and includes all windows
			// (since it is derived from SDL2_Bridge)
			SDL2_CS_COP.World world = new SDL2_CS_COP.World ();

            /*
             * window = sdl2.ext.Window("The Pong Game", size=(800, 600))
             * window.show()
             * 
             * if "-hardware" in sys.argv:
             *     print("Using hardware acceleration")
             *     renderer = sdl2.ext.Renderer(window)
             *     factory = sdl2.ext.SpriteFactory(sdl2.ext.TEXTURE, renderer=renderer)
             * else:
             *     print("Using software rendering")
             *     factory = sdl2.ext.SpriteFactory(sdl2.ext.SOFTWARE)
             */
            SDL2_CS_Bridge.IWindow window;
            SDL2_CS_Bridge.SpriteFactory spriteFactory;

            if (renderingType == RenderingType.SOFTWARE) {
                System.Console.WriteLine ("Using software rendering");
                window = new SDL2_CS_Bridge.SurfaceWindow (world,
                    "The Pong Game", 
                    SDL2.SDL.SDL_WINDOWPOS_CENTERED, 
                    SDL2.SDL.SDL_WINDOWPOS_CENTERED,
                    800, 600, SDL2.SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
                spriteFactory = new SDL2_CS_Bridge.SpriteFactory (SDL2_CS_Bridge.SpriteType.SOFTWARE);
            } else {
                System.Console.WriteLine ("Using hardware rendering");
                window = new SDL2_CS_Bridge.RenderedWindow (world,
                    "The Pong Game", 
                    SDL2.SDL.SDL_WINDOWPOS_CENTERED, 
                    SDL2.SDL.SDL_WINDOWPOS_CENTERED,
                    800, 600, SDL2.SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN,
                    SDL2.SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
                System.Collections.Generic.Dictionary<String,Object> keywordArguments = new System.Collections.Generic.Dictionary<string, object> (1);
                keywordArguments ["renderer"] = window; // Window is derived from Renderer
                spriteFactory = new SDL2_CS_Bridge.SpriteFactory (SDL2_CS_Bridge.SpriteType.TEXTURE, keywordArguments);
            }
			window.Show ();


			/* # Create the paddles - we want white ones. To keep it easy enough for us,
	         * # we create a set of surfaces that can be used for Texture- and
	         * # Software-based sprites.
	         */
			SDL2_CS_Bridge.Sprite sp_paddle_sprite = spriteFactory.NewSpriteFromColor (WHITE, new SDL2_CS_Bridge.Size(20, 200));
			SDL2_CS_COP.StandardItems.Components.Sprite sp_paddle1 = new SDL2_CS_COP.StandardItems.Components.Sprite (sp_paddle_sprite);
			SDL2_CS_COP.StandardItems.Components.Sprite sp_paddle2 = new SDL2_CS_COP.StandardItems.Components.Sprite (sp_paddle_sprite);
			SDL2_CS_Bridge.Sprite sp_ball_sprite = spriteFactory.NewSpriteFromColor (WHITE, new SDL2_CS_Bridge.Size (20, 20));
			SDL2_CS_COP.StandardItems.Components.Sprite sp_ball = new SDL2_CS_COP.StandardItems.Components.Sprite (sp_ball_sprite);

            // Alternative mechanism of making sprites, directly from the Sprite class:
            //if (renderingType == RenderingType.SOFTWARE) {
            //    Sprite.SpriteType = SDL2_CS_Bridge.SpriteType.SOFTWARE;
            //    Sprite.Renderer = null;
            //} else {
            //    Sprite.SpriteType = SDL2_CS_Bridge.SpriteType.TEXTURE;
            //    Sprite.Renderer = (SDL2_CS_Bridge.RenderedWindow)window;
            //}
            //SDL2_CS_Bridge.Sprite sp_paddle_sprite2 = Sprite.NewSprite (WHITE, new SDL2_CS_Bridge.Size (20, 200));
            //SDL2_CS_Bridge.Sprite sp_ball_sprite2 = Sprite.NewSprite (WHITE, new SDL2_CS_Bridge.Size (20, 20));

            // Another mechanism (and the simplest one): tell the World your type and let it do all the managing:
            //SDL2_CS_COP.World world2 = new SDL2_CS_COP.World (renderingType == RenderingType.HARDWARE);
            //SDL2_CS_Bridge.IWindow window2 = world2.AddWindow(
            //    "The Pong Game", 
            //    SDL2.SDL.SDL_WINDOWPOS_CENTERED, 
            //    SDL2.SDL.SDL_WINDOWPOS_CENTERED,
            //    800, 600, SDL2.SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN,
            //    SDL2.SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED); // render flags ignore if not using SDL2 Renderer
            //SDL2_CS_Bridge.Sprite sp_paddle_sprite1 = Sprite.NewSprite (WHITE, new SDL2_CS_Bridge.Size(20, 200));
            //SDL2_CS_Bridge.Sprite sp_ball_sprite1 = Sprite.NewSprite (WHITE, new SDL2_CS_Bridge.Size (20, 20));

			/*
			 * Create the systems that will manipulate and render the objects
             * 
             * movement = MovementSystem(0, 0, 800, 600)
             * collision = CollisionSystem(0, 0, 800, 600)
             * aicontroller = TrackingAIController(0, 600)
             */
            SDL2_CS_COP.StandardItems.Systems.MovementSystem movementSystem = new SDL2_CS_COP.StandardItems.Systems.MovementSystem (0, 0, 799, 599);
			CollisionSystem collisionSystem = new CollisionSystem (0, 0, 799, 599);
			TrackingAIController trackingAISystem = new TrackingAIController (0, 599);
            /* 
             * if factory.sprite_type == sdl2.ext.SOFTWARE:
             *     spriterenderer = SoftwareRenderSystem(window)
             * else:
             *     spriterenderer = TextureRenderSystem(renderer)
             */
            // (renderer no longer needs to know about HOW the sprite will be renderered--the window determines that.)
            SDL2_CS_COP.StandardItems.Systems.SpriteRenderer spriteRenderer = new SDL2_CS_COP.StandardItems.Systems.SpriteRenderer (window);

			/*
			 * Add the systems to the world in the order in which they will be called
	         */
			world.AddSystem (trackingAISystem);
			world.AddSystem (movementSystem);
			world.AddSystem (collisionSystem);
            world.AddSystem (spriteRenderer);

			/*
	         * Create the player and the ball entities
	         */
			PlayerEntity player1 = new PlayerEntity (world, sp_paddle1, 0, 250);
			PlayerEntity player2 = new PlayerEntity (world, sp_paddle2, 780, 250, true);
			System.Console.WriteLine ("Created AI Player {0}", player2); // we'll never ref this directly
			BallEntity ball = new BallEntity (world, sp_ball, 390, 290);
			ball.VelocityComponent.Vx = 0 - BallEntity.BALL_SPEED;
			collisionSystem.BallEntity = ball;
			trackingAISystem.Ball = ball;

			/*
	         * Run until the user quits
	         */
			Boolean isRunning = true;
			while (isRunning) {
				/*
		         * For each event
		         */
                foreach (SDL2_CS_Bridge.Event sdlEvent in world.GetAndProcessEvents()) {
					/*
			         * end the loop if the user selected QUIT
			         */
					if (sdlEvent.EventType == SDL2.SDL.SDL_EventType.SDL_QUIT) {
						isRunning = false;
						break;
					}
					/* 
			         * Handle the up and down cursor events
                     * (note: in future will be handled by a COP_System in GetAndProcessEvents)
			         */
					if (sdlEvent.EventType == SDL2.SDL.SDL_EventType.SDL_KEYDOWN) {
						if (sdlEvent.KeyCode == SDL2.SDL.SDL_Keycode.SDLK_UP) {
							player1.VelocityComponent.Vy = 0 - PlayerEntity.PADDLE_SPEED;
						} else if (sdlEvent.KeyCode == SDL2.SDL.SDL_Keycode.SDLK_DOWN) {
							player1.VelocityComponent.Vy = PlayerEntity.PADDLE_SPEED;
						}
					} else if (sdlEvent.EventType == SDL2.SDL.SDL_EventType.SDL_KEYUP) {
						if (sdlEvent.KeyCode == SDL2.SDL.SDL_Keycode.SDLK_DOWN || sdlEvent.KeyCode == SDL2.SDL.SDL_Keycode.SDLK_UP) {
							player1.VelocityComponent.Vy = 0;
						}
					}
				}
				/*
		         * Process with a 10ms delay
				 */
				SDL2.SDL.SDL_Delay (10);
                window.Fill (BG_COLOR);
				world.Process ();
				window.UpdateWindowSurface ();
			}
		}
	}
}
