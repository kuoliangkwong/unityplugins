using System;
using System.Runtime.InteropServices;
using AOT;
using Apple.Core.Runtime;
using UnityEngine;

namespace Apple.GameKit
{
    public static class DefaultNSExceptionHandler
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Init()
        {
#if (UNITY_EDITOR_OSX || !UNITY_EDITOR) && (UNITY_IOS || UNITY_TVOS || UNITY_STANDALONE_OSX || UNITY_VISIONOS)
            Interop.DefaultNSExceptionHandler_Set(ThrowNSException);
#endif
        }

        [MonoPInvokeCallback(typeof(NSExceptionCallback))]
        private static void ThrowNSException(IntPtr nsExceptionPtr)
        {
            InteropReference.PointerCast<NSException>(nsExceptionPtr).Throw();
        }

        private static class Interop
        {
            [DllImport(InteropUtility.DLLName)]
            public static extern void DefaultNSExceptionHandler_Set(NSExceptionCallback callback);
        }
    }

    public static class DefaultNSErrorHandler
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Init()
        {
#if (UNITY_EDITOR_OSX || !UNITY_EDITOR) && (UNITY_IOS || UNITY_TVOS || UNITY_STANDALONE_OSX || UNITY_VISIONOS)
            Interop.DefaultNSErrorHandler_Set(ThrowNSError);
#endif
        }

        [MonoPInvokeCallback(typeof(NSErrorCallback))]
        private static void ThrowNSError(IntPtr nsErrorPtr)
        {
            throw new GameKitException(nsErrorPtr);
        }

        private static class Interop
        {
            [DllImport(InteropUtility.DLLName)]
            public static extern void DefaultNSErrorHandler_Set(NSExceptionCallback callback);
        }
    }
}
