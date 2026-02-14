using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace InfrastructureCheckers.Collections
{
    public class DisposableMinifiedScheme : IDisposable
    {
        private bool _disposed = false;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        private void Dispose(bool isDisposing)
        {
            if(this._disposed)
                return;

            if (isDisposing)
            {
                // free managed resource
                
            }
            
            // free unmanaged
            
            this._disposed = true;
        }
        
        ~DisposableMinifiedScheme()
        {
            this.Dispose(false);
        }
    }
    
    public class DisposableFullScheme : IDisposable
    {

        // pointer to an external unmanaged resource
        private IntPtr handle;
        
        // managed resource this class uses
        private Component component = new Component();

        public DisposableFullScheme(IntPtr _ptr)
        {
            this.handle = _ptr;
        }

        private bool _disposed = false;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        private void Dispose(bool isDisposing)
        {
            if(this._disposed)
                return;

            if (isDisposing)
            {
                // free managed resource
                component.Dispose();
            }
            
            // free unmanaged
            
            this._disposed = true;
        }
        
        ~DisposableFullScheme()
        {
            this.Dispose(false);
        }

        [System.Runtime.InteropServices.DllImport("Kernel32", SetLastError = true)]
        private extern static bool CloseHandle(IntPtr handle);
    }
    
    
    
    
    
    public class ComplexResourceHolder : IDisposable
    {
        // unmanaged memory
        private IntPtr buffer;

        // managed wrapper over unmanaged handle
        private SafeWaitHandle waitHandle;

        // managed wrapper over unmanaged handle
        private FileStream logFile;

        // PURE managed resource (logical cleanup)
        private CancellationTokenSource cancellation;

        private bool disposed;

        public ComplexResourceHolder()
        {
            buffer = Marshal.AllocHGlobal(256);
            waitHandle = CreateEventSafe();

            logFile = new FileStream(
                "log.txt",
                FileMode.Create,
                FileAccess.Write,
                FileShare.Read);

            cancellation = new CancellationTokenSource();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            // always
            if (buffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(buffer);
                buffer = IntPtr.Zero;
            }

            if (disposing)
            {
                // managed cleanup
                cancellation?.Dispose();
                cancellation = null;

                logFile?.Dispose();
                logFile = null;

                waitHandle?.Dispose();
                waitHandle = null;
            }

            disposed = true;
        }

        ~ComplexResourceHolder()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // WinAPI
        private static SafeWaitHandle CreateEventSafe()
        {
            IntPtr handle = CreateEvent(
                IntPtr.Zero,
                false,
                false,
                null);

            if (handle == IntPtr.Zero)
                throw new InvalidOperationException();

            return new SafeWaitHandle(handle, true);
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateEvent(
            IntPtr lpEventAttributes,
            bool bManualReset,
            bool bInitialState,
            string lpName);
    }
    
    
    
    // extended example with real managed and unmanaged 
    public class ExtendedDispose : IDisposable
    {
        // ===== PURE UNMANAGED =====
        private IntPtr buffer;          // native heap
        private IntPtr nativeLibrary;   // HMODULE
        private IntPtr hwnd;            // HWND

        // ===== MANAGED =====
        private SafeWaitHandle waitHandle;
        private FileStream logFile;
        private CancellationTokenSource cancellation;

        private bool disposed;

        public ExtendedDispose()
        {
            // --- unmanaged allocations ---
            buffer = Marshal.AllocHGlobal(256);

            nativeLibrary = LoadLibrary("user32.dll");
            if (nativeLibrary == IntPtr.Zero)
                throw new InvalidOperationException("LoadLibrary failed");

            hwnd = CreateWindowEx(
                0,
                "STATIC",
                "HiddenWindow",
                0,
                0, 0, 100, 100,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero);

            if (hwnd == IntPtr.Zero)
                throw new InvalidOperationException("CreateWindowEx failed");

            // --- managed ---
            waitHandle = CreateEventSafe();

            logFile = new FileStream(
                "log.txt",
                FileMode.Create,
                FileAccess.Write,
                FileShare.Read);

            cancellation = new CancellationTokenSource();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            // =====================================================
            // PURE UNMANAGED CLEANUP (ALWAYS)
            // =====================================================

            if (hwnd != IntPtr.Zero)
            {
                DestroyWindow(hwnd);
                hwnd = IntPtr.Zero;
            }

            if (nativeLibrary != IntPtr.Zero)
            {
                FreeLibrary(nativeLibrary);
                nativeLibrary = IntPtr.Zero;
            }

            if (buffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(buffer);
                buffer = IntPtr.Zero;
            }

            // =====================================================
            // MANAGED CLEANUP (Dispose() only)
            // =====================================================
            if (disposing)
            {
                cancellation?.Dispose();
                cancellation = null;

                logFile?.Dispose();
                logFile = null;

                waitHandle?.Dispose();
                waitHandle = null;
            }

            disposed = true;
        }

        ~ExtendedDispose()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // ===== WinAPI =====

        private static SafeWaitHandle CreateEventSafe()
        {
            IntPtr handle = CreateEvent(IntPtr.Zero, false, false, null);
            if (handle == IntPtr.Zero)
                throw new InvalidOperationException();

            return new SafeWaitHandle(handle, true);
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateEvent(
            IntPtr lpEventAttributes,
            bool bManualReset,
            bool bInitialState,
            string lpName);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr CreateWindowEx(
            int dwExStyle,
            string lpClassName,
            string lpWindowName,
            int dwStyle,
            int x, int y, int nWidth, int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam);

        [DllImport("user32.dll")]
        private static extern bool DestroyWindow(IntPtr hWnd);
    }
    
}
