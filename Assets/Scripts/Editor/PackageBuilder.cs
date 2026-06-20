using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RO_Flex_UI.Editor
{
    public static class ROFlexUIPackageBuilder
    {
        private const string PackageRoot = "RO Flex UI";
        private const string SamplesOutput = PackageRoot + "/Samples~";

        /// <summary>
        /// Each entry copies one visible authoring folder into the generated
        /// Samples~ directory.
        ///
        /// Keep the source assets under Assets/ so they remain visible and
        /// editable inside the playground project.
        /// </summary>
        private static readonly IReadOnlyList<CopyEntry> Copies =
            new List<CopyEntry>
            {
                new(
                    source: "Assets/Samples/Demos",
                    destination: SamplesOutput + "/Demos"
                ),
                new(
                    source: "Assets/Samples/Fonts",
                    destination: SamplesOutput + "/Fonts"
                ),

                // Add more samples later:
                //
                // new(
                //     source: "Assets/RO Flex UI Playground/Package Samples/Localization Example",
                //     destination: SamplesOutput + "/Localization Example"
                // ),
            };

        [MenuItem("Tools/ROFlexUI/Rebuild Samples")]
        public static void RebuildSamples()
        {
            ValidatePackageFolder();

            var outputAbsolutePath = ToAbsolutePath(SamplesOutput);

            if (Directory.Exists(outputAbsolutePath))
            {
                Directory.Delete(outputAbsolutePath, recursive: true);
            }

            Directory.CreateDirectory(outputAbsolutePath);

            foreach (var copy in Copies)
            {
                CopyDirectory(
                    sourceDirectory: ToAbsolutePath(copy.Source),
                    destinationDirectory: ToAbsolutePath(copy.Destination)
                );
            }

            Debug.Log(
                $"RO Flex UI samples rebuilt successfully:\n{outputAbsolutePath}"
            );
        }

        private static void ValidatePackageFolder()
        {
            var packageAbsolutePath = ToAbsolutePath(PackageRoot);
            var manifestAbsolutePath = Path.Combine(
                packageAbsolutePath,
                "package.json"
            );

            if (!Directory.Exists(packageAbsolutePath))
            {
                throw new DirectoryNotFoundException(
                    $"Package directory was not found:\n{packageAbsolutePath}"
                );
            }

            if (!File.Exists(manifestAbsolutePath))
            {
                throw new FileNotFoundException(
                    $"Package manifest was not found:\n{manifestAbsolutePath}"
                );
            }
        }

        private static void CopyDirectory(
            string sourceDirectory,
            string destinationDirectory
        )
        {
            if (!Directory.Exists(sourceDirectory))
            {
                throw new DirectoryNotFoundException(
                    $"Sample source directory was not found:\n{sourceDirectory}"
                );
            }

            Directory.CreateDirectory(destinationDirectory);

            foreach (var sourceFile in Directory.EnumerateFiles(sourceDirectory))
            {
                var fileName = Path.GetFileName(sourceFile);

                if (ShouldIgnore(fileName))
                {
                    continue;
                }

                var destinationFile = Path.Combine(
                    destinationDirectory,
                    fileName
                );

                File.Copy(
                    sourceFile,
                    destinationFile,
                    overwrite: true
                );
            }

            foreach (var sourceSubdirectory in
                     Directory.EnumerateDirectories(sourceDirectory))
            {
                var directoryName = Path.GetFileName(sourceSubdirectory);

                if (ShouldIgnore(directoryName))
                {
                    continue;
                }

                var destinationSubdirectory = Path.Combine(
                    destinationDirectory,
                    directoryName
                );

                CopyDirectory(
                    sourceSubdirectory,
                    destinationSubdirectory
                );
            }
        }

        private static bool ShouldIgnore(string fileOrDirectoryName)
        {
            return fileOrDirectoryName.Equals(
                       ".DS_Store",
                       StringComparison.OrdinalIgnoreCase
                   )
                   || fileOrDirectoryName.EndsWith(
                       ".tmp",
                       StringComparison.OrdinalIgnoreCase
                   );
        }

        private static string ToAbsolutePath(string projectRelativePath)
        {
            return Path.GetFullPath(
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    projectRelativePath
                )
            );
        }

        private readonly struct CopyEntry
        {
            public CopyEntry(string source, string destination)
            {
                Source = source;
                Destination = destination;
            }

            public string Source { get; }
            public string Destination { get; }
        }
    }
}