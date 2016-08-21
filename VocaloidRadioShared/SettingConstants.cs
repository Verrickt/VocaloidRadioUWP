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

namespace VocaloidRadioShared
{
    /// <summary>
    /// Collection of string constants used in the entire solution. This file is shared for all projects
    /// </summary>
    public static class SettingConstants
    {
        // Data keys
        public const string StreamUri = "streamuri";
        public const string BackgroundTaskState = "backgroundtaskstate"; // Started, Running, Cancelled
        public const string AppState = "appstate"; // Suspended, Resumed
        public const string AppSuspendedTimestamp = "appsuspendedtimestamp";
        public const string AppResumedTimestamp = "appresumedtimestamp";
        public const string AppTheme = "apptheme";
        public const string WLANPrefer = "wlanprefer";
        public const string CellularPrefer = "cellularprefer";
        public const string Initialized = "initialized";
        public const string MediaPlaybackStatus = "mediaplaybackstatus";
    }
}
