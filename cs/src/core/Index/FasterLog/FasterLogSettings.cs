﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#pragma warning disable 0162

using System;

namespace FASTER.core
{
    /// <summary>
    /// Delegate for getting memory from user
    /// </summary>
    /// <param name="minLength">Minimum length of returned byte array</param>
    /// <returns></returns>
    public delegate byte[] GetMemory(int minLength);

    /// <summary>
    /// Type of checksum to add to log
    /// </summary>
    public enum LogChecksumType
    {
        /// <summary>
        /// No checksums
        /// </summary>
        None,
        /// <summary>
        /// Checksum per entry
        /// </summary>
        PerEntry
    }

    /// <summary>
    /// FASTER Log Settings
    /// </summary>
    public class FasterLogSettings
    {
        /// <summary>
        /// Device used for log
        /// </summary>
        public IDevice LogDevice = new NullDevice();

        /// <summary>
        /// Size of a page, in bits
        /// </summary>
        public int PageSizeBits = 22;

        /// <summary>
        /// Total size of in-memory part of log, in bits
        /// Should be at least one page long
        /// Num pages = 2^(MemorySizeBits-PageSizeBits)
        /// </summary>
        public int MemorySizeBits = 23;

        /// <summary>
        /// Size of a segment (group of pages), in bits
        /// This is the granularity of files on disk
        /// </summary>
        public int SegmentSizeBits = 30;

        /// <summary>
        /// Log commit manager - if you want to override the default implementation of commit.
        /// </summary>
        public ILogCommitManager LogCommitManager = null;

        /// <summary>
        /// Use specified directory for storing and retrieving checkpoints
        /// </summary>
        [Obsolete("This field is obsolete. The default log commit manager uses its own naming system. To use the old commit manager, set FasterLogSettings.LogCommitManager = new LocalLogCommitManager(LogCommitFile).", false)]
        public string LogCommitFile = null;

        /// <summary>
        /// User callback to allocate memory for read entries
        /// </summary>
        public GetMemory GetMemory = null;

        /// <summary>
        /// Type of checksum to add to log
        /// </summary>
        public LogChecksumType LogChecksum = LogChecksumType.None;

        /// <summary>
        /// Fraction of log marked as mutable (uncommitted)
        /// </summary>
        public double MutableFraction = 0;

        /// <summary>
        /// Use FasterLog as read-only iterator/viewer of log being committed by another instance
        /// </summary>
        public bool ReadOnlyMode = false;

        internal LogSettings GetLogSettings()
        {
            return new LogSettings
            {
                LogDevice = LogDevice,
                PageSizeBits = PageSizeBits,
                SegmentSizeBits = SegmentSizeBits,
                MemorySizeBits = ReadOnlyMode ? 0 : MemorySizeBits,
                CopyReadsToTail = CopyReadsToTail.None,
                MutableFraction = MutableFraction,
                ObjectLogDevice = null,
                ReadCacheSettings = null
            };
        }
    }
}
