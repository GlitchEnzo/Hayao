namespace Vapor
{
    using SharpDX;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// https://en.wikipedia.org/wiki/Wavefront_.obj_file
    /// </summary>
    public static class ObjLoader
    {
        public static InterleavedMesh<VertexPositionTextureNormal> LoadObj(string filepath)
        {
            var lines = File.ReadLines(filepath);

            List<Vector4> positions = new List<Vector4>();
            List<Vector3> texcoords = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();

            List<VertexPositionTextureNormal> vertexData = null;
            List<uint> indices = new List<uint>();

            var lineSeparator = new[] { ' ' };
            var faceSeparator = new[] { '/' };
            foreach (var line in lines)
            {
                if (line.StartsWith("#"))
                {
                    continue;
                }

                var items = line.Split(lineSeparator, System.StringSplitOptions.RemoveEmptyEntries);
                if (items.Length < 2)
                {
                    continue;
                }

                if (items[0] == "v")
                {
                    // read vertex positions
                    if (items.Length == 4)
                    {
                        Vector4 position = new Vector4(float.Parse(items[1]), float.Parse(items[2]), float.Parse(items[3]), 1.0f);
                        positions.Add(position);
                    }
                    else if (items.Length == 5)
                    {
                        Vector4 position = new Vector4(float.Parse(items[1]), float.Parse(items[2]), float.Parse(items[3]), float.Parse(items[4]));
                        positions.Add(position);
                    }
                }
                if (items[0] == "vt")
                {
                    // read texture coordinates
                    if (items.Length == 3)
                    {
                        Vector3 uv = new Vector3(float.Parse(items[1]), float.Parse(items[2]), 0.0f);
                        texcoords.Add(uv);
                    }
                    else if (items.Length == 4)
                    {
                        Vector3 uvw = new Vector3(float.Parse(items[1]), float.Parse(items[2]), float.Parse(items[3]));
                        texcoords.Add(uvw);
                    }
                }
                else if (items[0] == "vn")
                {
                    // read normals
                    Vector3 normal = new Vector3(float.Parse(items[1]), float.Parse(items[2]), float.Parse(items[3]));
                    normals.Add(normal);
                }
                else if (items[0] == "vp")
                {
                    // TODO: read parameter space vertices
                    Log.Warning("Parameter space vertices not yet supported!");
                }
                else if(items[0] == "f")
                {
                    // read faces
                    if (items.Length == 4)
                    {
                        // read triangles

                        // create the initial vertex list from the positions
                        if (vertexData == null)
                        {
                            vertexData = new List<VertexPositionTextureNormal>();

                            foreach (var position in positions)
                            {
                                VertexPositionTextureNormal vertex = new VertexPositionTextureNormal();
                                vertex.Position = position;
                                vertexData.Add(vertex);
                            }
                        }

                        // read the indices, tex coords, and normal
                        for (int i = 1; i < items.Length; i++)
                        {
                            var vertData = items[i].Split(faceSeparator);

                            if (vertData.Length == 1)
                            {
                                // index only
                                int vertexIndex = int.Parse(vertData[0]) - 1;
                                indices.Add((uint)vertexIndex);
                            }
                            else if (vertData.Length == 2)
                            {
                                // index & tex coords
                                int vertexIndex = int.Parse(vertData[0]) - 1;
                                indices.Add((uint)vertexIndex);

                                int uvIndex = int.Parse(vertData[1]) - 1;
                                var vertex = vertexData[vertexIndex];
                                vertex.TextureCoordinates = texcoords[uvIndex];
                                vertexData[vertexIndex] = vertex;
                            }
                            else if (vertData.Length == 3)
                            {
                                // index, tex coords, & normal
                                int vertexIndex = int.Parse(vertData[0]) - 1;
                                indices.Add((uint)vertexIndex);

                                // ensure uvs present
                                int uvIndex;
                                if (int.TryParse(vertData[1], out uvIndex))
                                {
                                    uvIndex--;
                                    var vertex = vertexData[vertexIndex];
                                    vertex.TextureCoordinates = texcoords[uvIndex];
                                    vertexData[vertexIndex] = vertex;
                                }

                                int normalIndex = int.Parse(vertData[2]) - 1;
                                var vertexElement = vertexData[vertexIndex];
                                vertexElement.Normal = normals[normalIndex];
                                vertexData[vertexIndex] = vertexElement;
                            }
                        }
                    }
                    else if (items.Length == 5)
                    {
                        // TODO: read quads
                        Log.Warning("Quads not yet supported!");
                    }
                }
            }

            InterleavedMesh<VertexPositionTextureNormal> mesh = new InterleavedMesh<VertexPositionTextureNormal>();
            mesh.VertexData = vertexData.ToArray();
            mesh.Indices = indices.ToArray();
            return mesh;
        }
    }
}
