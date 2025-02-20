// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using MonoTorrent.Client;

namespace PRDownloader.Services;

public static class EngineSettingsProvider
{
    public static EngineSettings CreateDefaultSettings()
    {
        var appRoot = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var cacheDirectory = Path.Combine(appRoot, "ProjectReality", "PRDownloader");

        var settingBuilder = new EngineSettingsBuilder
        {
            // Allow the engine to automatically forward ports using upnp/nat-pmp (if a compatible router is available)
            AllowPortForwarding = true,

            // Automatically save a cache of the DHT table when all torrents are stopped.
            AutoSaveLoadDhtCache = true,

            // Automatically save 'FastResume' data when TorrentManager.StopAsync is invoked, automatically load it
            // before hash checking the torrent. Fast Resume data will be loaded as part of 'engine.AddAsync' if
            // torrent metadata is available. Otherwise, if a magnetlink is used to download a torrent, fast resume
            // data will be loaded after the metadata has been downloaded. 
            AutoSaveLoadFastResume = true,

            // If a MagnetLink is used to download a torrent, the engine will try to load a copy of the metadata
            // it's cache directory. Otherwise the metadata will be downloaded and stored in the cache directory
            // so it can be reloaded later.
            AutoSaveLoadMagnetLinkMetadata = true,

            CacheDirectory = cacheDirectory,
        };

        return settingBuilder.ToSettings();
    }
}
