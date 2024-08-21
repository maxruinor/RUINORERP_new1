﻿//--------------------------------------------------------------------------------
// Copyright (C) 2013-2021 JDH Software - <support@jdhsoftware.com>
//
// This program is provided to you under the terms of the Microsoft Public
// License (Ms-PL) as published at https://github.com/Cocotteseb/Krypton-OutlookGrid/blob/master/LICENSE.md
//
// Visit https://www.jdhsoftware.com and follow @jdhsoftware on Twitter
//
//--------------------------------------------------------------------------------
using System;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid
{
    /// <summary>
    /// Class for events of the group image of a group row.
    /// </summary>
    public class OutlookGridGroupImageEventArgs : EventArgs
    {
        private OutlookGridRow row;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="row">The OutlookGridRow.</param>
        public OutlookGridGroupImageEventArgs(OutlookGridRow row)
        {
            this.row = row;
        }

        /// <summary>
        /// Gets or sets the row.
        /// </summary>
        public OutlookGridRow Row
        {
            get
            {
                return this.row;
            }
            set
            {
                this.row = value;
            }
        }
    }
}
