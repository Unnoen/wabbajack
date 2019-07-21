﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wabbajack.Common
{
    public class RawSourceFile
    {
        public string AbsolutePath;
        public string Path;

        private string _hash;
        public string Hash
        {
            get
            {
                if (_hash != null) return _hash;
                _hash = AbsolutePath.FileSHA256();
                return _hash;
            }
        }

        public T EvolveTo<T>() where T : Directive, new()
        {
            var v = new T();
            v.To = Path;
            return v;
        }
    }

    public class ModList
    {
        /// <summary>
        /// Name of the ModList
        /// </summary>
        public string Name;
        /// <summary>
        /// Author of the Mod List
        /// </summary>
        public string Author;
        /// <summary>
        /// Version of this Mod List
        /// </summary>
        public string Version;

        /// <summary>
        /// Install directives
        /// </summary>
        public List<Directive> Directives;

        /// <summary>
        /// Archives required by this modlist
        /// </summary>
        public List<Archive> Archives;
    }

    public class Directive
    {
        /// <summary>
        /// location the file will be copied to, relative to the install path.
        /// </summary>
        public string To;
    }

    public class IgnoredDirectly : Directive
    {
        public string Reason;
    }

    public class NoMatch : IgnoredDirectly
    {

    }

    public class InlineFile : Directive
    {
        /// <summary>
        /// Data that will be written as-is to the destination location;
        /// </summary>
        public string SourceData;
    }

    public class FromArchive : Directive
    {
        /// <summary>
        /// MurMur3 hash of the archive this file comes from
        /// </summary>
        public string ArchiveHash;
        /// <summary>
        /// The relative path of the file in the archive
        /// </summary>
        public string From;
    }

    public class PatchedArchive : FromArchive
    {
        /// <summary>
        /// The file to apply to the source file to patch it
        /// </summary>
        public string Patch;
    }

    public class Archive
    {
        /// <summary>
        /// MurMur3 Hash of the archive
        /// </summary>
        public string Hash;
        /// <summary>
        /// Human friendly name of this archive
        /// </summary>
        public string Name;
    }

    /// <summary>
    /// URL that can be downloaded directly without any additional options
    /// </summary>
    public class DirectURLArchive : Archive
    {
        public string URL;
    }

    /// <summary>
    /// An archive that requires additional HTTP headers.
    /// </summary>
    public class DirectURLArchiveEx : DirectURLArchive
    {
        public Dictionary<string, string> Headers;
    }

    /// <summary>
    /// Archive that comes from MEGA
    /// </summary>
    public class MEGAArchive : DirectURLArchive
    {
    }

    /// <summary>
    /// Archive that comes from MODDB
    /// </summary>
    public class MODDBArchive : DirectURLArchive
    {
    }

    /// <summary>
    /// The indexed contents of an archive
    /// </summary>
    public class IndexedArchiveCache
    {
        public string Hash;
        public List<IndexedEntry> Entries;
    }

    public class IndexedArchive : IndexedArchiveCache
    {
        public dynamic IniData;
        public string Name;
    }

    /// <summary>
    /// A archive entry
    /// </summary>
    public class IndexedEntry
    {
        /// <summary>
        /// Path in the archive to this file
        /// </summary>
        public string Path;
        /// <summary>
        /// MurMur3 hash of this file
        /// </summary>
        public string Hash;
        /// <summary>
        /// Size of the file (uncompressed)
        /// </summary>
        public long Size;
    }

    /// <summary>
    /// Data found inside a BSA file in an archive
    /// </summary>
    public class BSAIndexedEntry : IndexedEntry
    {
        /// <summary>
        /// MurMur3 hash of the BSA this file comes from
        /// </summary>
        public string BSAHash;
    }
}