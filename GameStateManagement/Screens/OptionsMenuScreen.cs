#region File Description

//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion File Description

namespace GameStateManagement
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    /// 
    internal class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        

        private MenuEntry maxFPSMenuEntry;
        private MenuEntry DebugModeMenuEntry;

        private static bool debugMode = false;

        public static int[] maxFPS = { 60, 100, 144 };
        public static int maxFPSIndex = 0;

        #endregion Fields

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            maxFPSMenuEntry = new MenuEntry(string.Empty);
            DebugModeMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            maxFPSMenuEntry.Selected += maxFPSSelected;
            DebugModeMenuEntry.Selected += debugModeSelected;
            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(maxFPSMenuEntry);
            MenuEntries.Add(DebugModeMenuEntry);
            MenuEntries.Add(back);
        }

        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        private void SetMenuEntryText()
        {
            maxFPSMenuEntry.Text = "Maximum FPS: " + maxFPS[maxFPSIndex];
            DebugModeMenuEntry.Text = "Debug Mode: " + debugMode;
        }

        #endregion Initialization

        #region Handle Input

        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        private void maxFPSSelected(object sender, PlayerIndexEventArgs e)
        {
            maxFPSIndex++;
            if(maxFPSIndex >= maxFPS.Length)
            {
                maxFPSIndex= 0;
            }

            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        private void debugModeSelected(object sender, PlayerIndexEventArgs e)
        {
            if (debugMode)
            {
                debugMode = false;
            }
            else
            {
                debugMode = true;
            }

            SetMenuEntryText();
        }
        #endregion Handle Input
    }
}