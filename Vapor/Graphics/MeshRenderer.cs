﻿namespace Vapor
{
    public class MeshRenderer : Renderer
    {
        public Mesh Mesh { get; set; }

        public override void Draw(Camera camera)
        {
            base.Draw(camera);

            Mesh.Draw(Material);
        }
    }
}
