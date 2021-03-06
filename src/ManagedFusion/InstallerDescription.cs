using System.IO;
using System.Management.Fusion.WrappedFusion;

namespace System.Management.Fusion
{
    /// <summary>
    /// Descriptor class for applications manipulating the GAC using an instance of <see cref="AssemblyCache"/>.
    /// </summary>
    public class InstallerDescription
    {



        /// <summary>
        /// Gets the type of the installer described by the current <see cref="InstallerDescription"/>.
        /// </summary>
        public InstallerType Type { get; }

        /// <summary>
        /// Gets the unique identifier of the installer described by the current <see cref="InstallerDescription"/>.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the description of the installer described by the current <see cref="InstallerDescription"/>.
        /// </summary>
        public string Description { get; }



        /// <summary>
        /// Initializes a new instance of <see cref="InstallerDescription"/> from the values specified.
        /// </summary>
        /// <param name="installerType"></param>
        /// <param name="applicationDescription"></param>
        /// <param name="uniqueId"></param>
        private InstallerDescription(InstallerType installerType, string uniqueId, string applicationDescription)
        {
            Type = installerType;
            Id = uniqueId;
            Description = applicationDescription;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="InstallerDescription"/> using the data specified in the given <see cref="FusionInstallReference"/>.
        /// </summary>
        /// <param name="fusionInstallReference"></param>
        internal InstallerDescription(FusionInstallReference fusionInstallReference)
        {
            Type = InstallerTypeExt.FromGuid(fusionInstallReference.GuidScheme);
            Description = fusionInstallReference.NonCannonicalData;
            Id = fusionInstallReference.Identifier;
        }



        public static InstallerDescription CreateForInstaller(string installerName, string installerIdentifier)
        {
            return new InstallerDescription(InstallerType.Installer, installerIdentifier, installerName);
        }

        public static InstallerDescription CreateForFile(string description, string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("An instance of InstallReference can only be created for an existing file.", fileName);
            return new InstallerDescription(InstallerType.File, fileName, description);
        }

        public static InstallerDescription CreateForOpaqueString(string description, string opaqueString)
        {
            return new InstallerDescription(InstallerType.OpaqueString, opaqueString, description);
        }



        public override string ToString()
        {
            return "[" + Type + "] " + Id;
        }



        internal FusionInstallReference ToFusionStruct()
        {
            var result = new FusionInstallReference(
              Type.AsGuid(),
              Id,
              Description);
            return result;
        }


    }
}
