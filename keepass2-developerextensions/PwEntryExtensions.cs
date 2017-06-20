﻿using KeePass.Plugins;
using KeePassLib;
using System;
using System.Drawing;
using System.Linq;

namespace KeePassExtensions
{
    public static class PwEntryExtensions
    {
        public static DateTime GetPasswordLastModified(this PwEntry entry)
        {
            if (entry.History != null && entry.History.Any())
            {
                var sortedEntries = entry.History.OrderBy(h => h.LastModificationTime).ToList();
                var lastModified = entry.LastModificationTime;

                for (var i = sortedEntries.Count() - 1; i >= 0; i--)
                {
                    var same = entry.Strings.GetSafe(PwDefs.PasswordField).ValueEquals(sortedEntries[i].Strings.GetSafe(PwDefs.PasswordField));
                    if (!same) break;
                    lastModified = sortedEntries[i].LastModificationTime;
                }

                // if the password is the same in the entry and all history items all we know is that it is at least as old as the earliest history item
                return lastModified;
            }

            // no history means either the password has just been set, or the history has been pruned in which case we have no other data
            return entry.LastModificationTime;
        }

        public static bool IsDeleted(this PwEntry entry, IPluginHost pluginHost)
        {
            return entry.ParentGroup.Uuid.CompareTo(pluginHost.Database.RecycleBinUuid) == 0;
        }

        public static string GetUrlDomain(this PwEntry entry)
        {
            var url = entry.Strings.ReadSafe(PwDefs.UrlField).ToLower();
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            if (!url.Contains("://"))
            {
                // add http as a fallback protocol
                url = string.Format("http://{0}", url);
            }

            try
            {
                var domain = new Uri(url).DnsSafeHost;
                if (domain.StartsWith("www."))
                {
                    domain = domain.Substring(4);
                }
                return domain;
            }
            catch (UriFormatException) { return string.Empty; }
        }

        public static Image GetIcon(this PwEntry entry, IPluginHost pluginHost)
        {
            var entryIcon = entry.IconId;
            var customIcon = entry.CustomIconUuid;

            if (customIcon.Equals(PwUuid.Zero)) return pluginHost.MainWindow.ClientIcons.Images[(int) entryIcon];
            var w = KeePass.UI.DpiUtil.ScaleIntX(16);
            var h = KeePass.UI.DpiUtil.ScaleIntY(16);

            var imgCustom = pluginHost.Database.GetCustomIcon(customIcon, w, h);
            return (imgCustom ?? pluginHost.MainWindow.ClientIcons.Images[(int)entryIcon]);
        }
    }
}