//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Diagnostics;
using Windows.Media;
using Windows.Storage;

namespace VocaloidRadioShared
{
    /// <summary>
    /// Collection of string constants used in the entire solution. This file is shared for all projects
    /// </summary>
    public static class SettingHelper
    {
        /// <summary>
        /// Function to read a setting value and clear it after reading it
        /// </summary>
        public static object ReadResetSettingsValue(string key)
        {
            Debug.WriteLine(key);
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                Debug.WriteLine("null returned");
                return null;
            }
            else
            {
                var value = ApplicationData.Current.LocalSettings.Values[key];
                ApplicationData.Current.LocalSettings.Values.Remove(key);
                Debug.WriteLine("value found " + value.ToString());
                return value;
            }
        }


        /// <summary>
        /// Function to read a setting value and clear it after reading it
        /// </summary>
        public static object ReadSettingsValue(string key)
        {
            Debug.WriteLine(key);
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                Debug.WriteLine("null returned");
                return null;
            }
            else
            {
                var value = ApplicationData.Current.LocalSettings.Values[key];
                Debug.WriteLine("value found " + value.ToString());
                return value;
            }
        }


        /// <summary>
        /// Save a key value pair in settings. Create if it doesn't exist
        /// </summary>
        public static void SaveSettingsValue(string key, object value)
        {
            Debug.WriteLine(key + ":" + (value == null ? "null" : value.ToString()));

            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                ApplicationData.Current.LocalSettings.Values.Add(key, value);
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values[key] = value;
            }
        }

        static SettingHelper()
        {
            RestoreDefault();
        }

        private static void RestoreDefault()
        {
            bool? inited = ReadSettingsValue(SettingConstants.Initialized) as bool?;
            if (inited != false)
            {
                string[] _streamSource = new[]
                {
                    "http://curiosity.shoutca.st:8019/128",
                    "http://curiosity.shoutca.st:8019/stream"
                };
                var wlan = SettingHelper.ReadSettingsValue(SettingConstants.WLANPrefer) as string;
                var cellular = SettingHelper.ReadSettingsValue(SettingConstants.CellularPrefer) as string;
                if (String.IsNullOrWhiteSpace(wlan))
                {
                    SettingHelper.SaveSettingsValue(SettingConstants.WLANPrefer, _streamSource[1]);
                }
                if (String.IsNullOrWhiteSpace(cellular))
                {
                    SettingHelper.SaveSettingsValue(SettingConstants.CellularPrefer, _streamSource[0]);
                }
                SaveSettingsValue(SettingConstants.Initialized,true);
            }
            SaveSettingsValue(SettingConstants.MediaPlaybackStatus, MediaPlaybackStatus.Stopped.ToString());
        }
    }
}
