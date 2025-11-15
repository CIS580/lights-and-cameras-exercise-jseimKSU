using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightingAndCamerasExample
{
    public class FPSCamera : ICamera
    {
        // The angle of rotation about the Y-axis
        float horizontalAngle;

        // The angle of rotation about the X-axis
        float verticalAngle;
        // The camera's position in the world 
        Vector3 position;
        MouseState oldMouseState;
        Game game;

        public Matrix View { get; protected set; }
        public Matrix Projection { get; protected set; }

        public float Sensitivity { get; set; } = 0.0018f;

        public float Speed { get; set; } = .5f;

        /// <summary>
        /// Constructs a new FPS Camera
        /// </summary>
        /// <param name="game">The game this camera belongs to</param>
        /// <param name="position">The player's initial position</param>
        public FPSCamera(Game game, Vector3 position)
        {
            this.game = game;
            this.position = position; 
            this.horizontalAngle = 0;
            this.verticalAngle = 0;
            this.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.GraphicsDevice.Viewport.AspectRatio, 1, 1000);
            Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
            oldMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Updates the camera
        /// </summary>
        /// <param name="gameTime">The current GameTime</param>
        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            var newMouseState = Mouse.GetState();
            var facing = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(horizontalAngle));
            // Forward and backward movement
            if (keyboard.IsKeyDown(Keys.W)) position += facing * Speed;
            if (keyboard.IsKeyDown(Keys.S)) position -= facing * Speed;
            // Strifing movement
            if (keyboard.IsKeyDown(Keys.A)) position += Vector3.Cross(Vector3.Up, facing) * Speed;
            if (keyboard.IsKeyDown(Keys.D)) position -= Vector3.Cross(Vector3.Up, facing) * Speed;
            // Adjust horizontal angle
            horizontalAngle += Sensitivity * (oldMouseState.X - newMouseState.X);

            // Adjust vertical angle 
            verticalAngle += Sensitivity * (oldMouseState.Y - newMouseState.Y);
            var direction = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationX(verticalAngle) * Matrix.CreateRotationY(horizontalAngle));
            // create the veiw matrix
            View = Matrix.CreateLookAt(position, position + direction, Vector3.Up);
            // Reset mouse state 
            Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
            oldMouseState = Mouse.GetState();
        }
    }
}
