namespace Vapor
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using SharpDX.Windows;
    using D3D11 = SharpDX.Direct3D11;
    using SharpDX.DXGI;
    using SharpDX;
    using SharpDX.Direct3D;

    public class Application : IDisposable
    {
        public static D3D11.Device Device { get; set; }

        public static List<VaporObject> Objects { get; set; } = new List<VaporObject>();

        private RenderForm renderForm;
        public const int Width = 1280;
        public const int Height = 720;

        private SwapChain swapChain;
        private Viewport viewport;
        private D3D11.RenderTargetView renderTargetView;

        public Scene CurrentScene { get; set; }

        public string WindowTitle
        {
            get
            {
                return renderForm.Text;
            }
            set
            {
                renderForm.Text = value;
            }
        }

        private static Application instance;
        public static Application Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Application();
                }

                return instance;
            }
        }

        protected Application()
        {
            // Set window properties
            renderForm = new RenderForm("Vapor");
            renderForm.ClientSize = new Size(Width, Height);
            renderForm.AllowUserResizing = false;

            InitializeDeviceResources();
        }

        private void InitializeDeviceResources()
        {
            ModeDescription backBufferDesc = new ModeDescription(Width, Height, new Rational(60, 1), Format.R8G8B8A8_UNorm);

            // Descriptor for the swap chain
            SwapChainDescription swapChainDesc = new SwapChainDescription()
            {
                ModeDescription = backBufferDesc,
                SampleDescription = new SampleDescription(1, 0),
                Usage = Usage.RenderTargetOutput,
                BufferCount = 1,
                OutputHandle = renderForm.Handle,
                IsWindowed = true
            };

            // Create device and swap chain
            D3D11.Device device;
            D3D11.Device.CreateWithSwapChain(DriverType.Hardware, D3D11.DeviceCreationFlags.None, swapChainDesc, out device, out swapChain);
            Device = device;

            viewport = new Viewport(0, 0, Width, Height);
            Device.ImmediateContext.Rasterizer.SetViewport(viewport);

            // this disables all culling based on winding, to debug why certain things are not drawing
            //var rasterizerDesc = new D3D11.RasterizerStateDescription()
            //{
            //    FillMode = D3D11.FillMode.Solid,
            //    CullMode = D3D11.CullMode.None,
            //    IsFrontCounterClockwise = true,
            //    DepthBias = 0,
            //    DepthBiasClamp = 0,
            //    SlopeScaledDepthBias = 0,
            //    IsDepthClipEnabled = true,
            //    IsScissorEnabled = false,
            //    IsMultisampleEnabled = true,
            //    IsAntialiasedLineEnabled = true
            //};
            //Device.ImmediateContext.Rasterizer.State = new D3D11.RasterizerState(Device, rasterizerDesc);

            // Create render target view for back buffer
            using (D3D11.Texture2D backBuffer = swapChain.GetBackBuffer<D3D11.Texture2D>(0))
            {
                renderTargetView = new D3D11.RenderTargetView(Device, backBuffer);
            }

            // Set back buffer as current render target view
            Device.ImmediateContext.OutputMerger.SetRenderTargets(renderTargetView);
        }

        public virtual void Clear(SharpDX.Color clearColor)
        {
            Device.ImmediateContext.ClearRenderTargetView(renderTargetView, clearColor);
        }

        public virtual void Run()
        {
            // Start the render loop
            RenderLoop.Run(renderForm, Loop);
        }

        protected virtual void Loop()
        {
            Time.Update();

            if (CurrentScene != null)
            {
                CurrentScene.Update();
            }

            if (CurrentScene != null)
            {
                CurrentScene.Draw();
            }

            // Swap front and back buffer
            swapChain.Present(1, PresentFlags.None);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    renderForm.Dispose();
                    swapChain.Dispose();
                    renderTargetView.Dispose();
                    Device.Dispose();

                    //if (CurrentScene != null)
                    //{
                    //    CurrentScene.Dispose();
                    //}

                    for (int i = Objects.Count - 1; i >= 0; i--)
                    {
                        if (Objects[i] != null)
                        {
                            Objects[i].Dispose();
                        }
                    }
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
