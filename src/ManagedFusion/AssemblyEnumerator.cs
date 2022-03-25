﻿
using System.Collections.Generic;
using System.Management.Fusion.Helpers;
using System.Management.Fusion.WrappedFusion;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Management.Fusion
{
    internal class AssemblyEnumerator : IEnumerator<AssemblyName>
    {
        /// <summary>
        /// To obtain an instance of the CreateAssemblyEnum API, call the CreateAssemblyNameObject API.
        /// </summary>
        /// <param name="pEnum">Pointer to a memory location that contains the IAssemblyEnum pointer.</param>
        /// <param name="pUnkReserved">Must be null.</param>
        /// <param name="pName">An assembly name that is used to filter the enumeration. Can be null to enumerate all assemblies in the GAC.</param>
        /// <param name="dwFlags">Exactly one bit from the ASM_CACHE_FLAGS enumeration.</param>
        /// <param name="pvReserved">Must be NULL.</param>
        [DllImport("fusion.dll", SetLastError = true, PreserveSig = false)]
        static extern void CreateAssemblyEnum(out IAssemblyEnum pEnum, IntPtr pUnkReserved, IAssemblyName pName,
          AssemblyCacheFlags dwFlags, IntPtr pvReserved);

        private readonly IAssemblyEnum _enum;
        private AssemblyName _current;

        public AssemblyEnumerator()
        {
            CreateAssemblyEnum(out _enum, (IntPtr)0, null, AssemblyCacheFlags.GAC, (IntPtr)0);
        }

        public AssemblyName Current
        {
            get { return _current; }
        }

        object System.Collections.IEnumerator.Current
        {
            get { return _current; }
        }

        public bool MoveNext()
        {
            IAssemblyName ae;
            IApplicationContext context;
            var hResult = _enum.GetNextAssembly(out context, out ae, 0);
            _current = ae == null || !HResult.IsSuccess(hResult) ? null : ae.ToAssemblyName();
            return _current != null;
        }

        public void Reset()
        {
            _enum.Reset();
        }

        public void Dispose()
        {
        }

    }
}
