﻿#region BSD License
/*
 * 
 *  SaveAs BSD 3-Clause License (https://github.com/Krypton-Suite/Standard-Toolkit/blob/master/LICENSE)
 *  Modifications by Peter Wagner(aka Wagnerp) & Simon Coghlan(aka Smurf-IV), et al. 2023 - 2023. All rights reserved. 
 *  
 */
#endregion

namespace Krypton.Toolkit
{
    /// <summary>A <see cref="KryptonCommand"/> created specifically for the <see cref="PaletteButtonSpecStyle.SaveAs"/> button spec.</summary>
    [Category(@"code")]
    [ToolboxItem(false)]
    //[ToolboxBitmap(typeof(KryptonHelpCommand), @"ToolboxBitmaps.KryptonHelp.bmp")]
    [Description(@"For use with the 'SaveAs' ButtonSpec style.")]
    [DesignerCategory(@"code")]
    public class KryptonIntegratedToolbarSaveAsCommand : KryptonCommand
    {
        #region Instance Fields

        private ButtonSpecAny? _saveAsButtonSpec;

        private ButtonImageStates? _imageStates;

        private Image? _activeImage;

        private Image? _disabledImage;

        private Image? _normalImage;

        private Image? _pressedImage;

        #endregion

        #region Public

        /// <summary>Gets or sets the save as button.</summary>
        /// <value>The save as button.</value>
        [DefaultValue(null), Description(@"Access to the save as button spec.")]
        [AllowNull]
        public ButtonSpecAny? ToolBarSaveAsButton
        {
            get => _saveAsButtonSpec ?? new ButtonSpecAny();
            set { _saveAsButtonSpec = value; UpdateImage(KryptonManager.InternalGlobalPaletteMode); }
        }

        /// <summary>Gets the active image.</summary>
        /// <value>The active image.</value>
        public Image? ActiveImage { get => _activeImage; private set => _activeImage = value; }

        /// <summary>Gets the disabled image.</summary>
        /// <value>The disabled image.</value>
        public Image? DisabledImage { get => _disabledImage; private set => _disabledImage = value; }

        /// <summary>Gets the normal image.</summary>
        /// <value>The normal image.</value>
        public Image? NormalImage { get => _normalImage; private set => _normalImage = value; }

        /// <summary>Gets the pressed image.</summary>
        /// <value>The pressed image.</value>
        public Image? PressedImage { get => _pressedImage; private set => _pressedImage = value; }

        #endregion

        #region Identity

        /// <summary>Initializes a new instance of the <see cref="KryptonIntegratedToolbarSaveAsCommand" /> class.</summary>
        public KryptonIntegratedToolbarSaveAsCommand()
        {
            _imageStates = new ButtonImageStates();

            Text = KryptonLanguageManager.ToolBarStrings.SaveAs;
        }

        #endregion

        #region Implementation

        /// <summary>Updates the image.</summary>
        /// <param name="helpImage">The help image.</param>
        private void UpdateImage(Image helpImage) => ImageSmall = helpImage;

        /// <summary>Adds the image states.</summary>
        /// <param name="activeImage">The active image.</param>
        /// <param name="disabledImage">The disabled image.</param>
        /// <param name="normalImage">The normal image.</param>
        /// <param name="pressedImage">The pressed image.</param>
        private void AddImageStates(Image? activeImage, Image disabledImage, Image normalImage, Image? pressedImage)
        {
            if (_saveAsButtonSpec != null)
            {
                _saveAsButtonSpec.ImageStates.ImageDisabled = disabledImage;

                _saveAsButtonSpec.ImageStates.ImageTracking = activeImage ?? null;

                _saveAsButtonSpec.ImageStates.ImageNormal = normalImage;

                _saveAsButtonSpec.ImageStates.ImagePressed = pressedImage ?? null;
            }
        }

        /// <summary>Updates the active image.</summary>
        /// <param name="activeImage">The active image.</param>
        private void UpdateActiveImage(Image activeImage)
        {
            _activeImage = activeImage;

            if (_saveAsButtonSpec != null)
            {
                _saveAsButtonSpec.ImageStates.ImageTracking = _activeImage;
            }
        }

        /// <summary>Updates the disabled image.</summary>
        /// <param name="disabledImage">The disabled image.</param>
        private void UpdateDisabledImage(Image disabledImage)
        {
            _disabledImage = disabledImage;

            if (_saveAsButtonSpec != null)
            {
                _saveAsButtonSpec.ImageStates.ImageDisabled = disabledImage;
            }
        }

        /// <summary>Updates the normal image.</summary>
        /// <param name="normalImage">The normal image.</param>
        private void UpdateNormalImage(Image normalImage)
        {
            _normalImage = normalImage;

            if (_saveAsButtonSpec != null)
            {
                _saveAsButtonSpec.ImageStates.ImageNormal = normalImage;
            }
        }

        /// <summary>Updates the pressed image.</summary>
        /// <param name="pressedImage">The pressed image.</param>
        private void UpdatePressedImage(Image pressedImage)
        {
            _pressedImage = pressedImage;

            if (_saveAsButtonSpec != null)
            {
                _saveAsButtonSpec.ImageStates.ImagePressed = pressedImage;
            }
        }

        /// <summary>Updates the image.</summary>
        /// <param name="mode">The mode.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">mode - null</exception>
        private void UpdateImage(PaletteMode mode)
        {

            switch (mode)
            {
                case PaletteMode.Global:
                    break;
                case PaletteMode.ProfessionalSystem:
                    UpdateImage(SystemToolbarImageResources.SystemToolbarSaveNormal);
                    break;
                case PaletteMode.ProfessionalOffice2003:
                    UpdateImage(Office2003ToolbarImageResources.Office2003ToolbarSaveNormal);
                    break;
                case PaletteMode.Office2007DarkGray:
                    UpdateImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007Blue:
                    UpdateImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007BlueDarkMode:
                    UpdateImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007BlueLightMode:
                    UpdateImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007Silver:
                    UpdateImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007SilverDarkMode:
                    UpdateImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007SilverLightMode:
                    UpdateImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007White:
                    UpdateImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007Black:
                    UpdateImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007BlackDarkMode:
                    UpdateImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010DarkGray:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010Blue:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010BlueDarkMode:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010BlueLightMode:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010Silver:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010SilverDarkMode:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010SilverLightMode:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010White:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010Black:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010BlackDarkMode:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2013DarkGray:
                case PaletteMode.Office2013LightGray:
                case PaletteMode.Office2013White:
                    UpdateImage(Office2013ToolbarImageResources.Office2013ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365DarkGray:
                    UpdateImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365Black:
                    UpdateImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365BlackDarkMode:
                    UpdateImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365Blue:
                    UpdateImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365BlueDarkMode:
                    UpdateImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365BlueLightMode:
                    UpdateImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365Silver:
                    UpdateImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365SilverDarkMode:
                    UpdateImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365SilverLightMode:
                    UpdateImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365White:
                    UpdateImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleBlue:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleBlueDarkMode:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleBlueLightMode:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleOrange:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleOrangeDarkMode:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleOrangeLightMode:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparklePurple:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparklePurpleDarkMode:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparklePurpleLightMode:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Custom:
                    UpdateImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }

            UpdateActiveImage(mode);

            UpdateDisabledImage(mode);

            UpdateNormalImage(mode);

            UpdatePressedImage(mode);
        }

        /// <summary>Updates the active image.</summary>
        /// <param name="mode">The mode.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">mode - null</exception>
        private void UpdateActiveImage(PaletteMode mode)
        {
            switch (mode)
            {
                case PaletteMode.Global:
                    break;
                case PaletteMode.ProfessionalSystem:
                    UpdateActiveImage(GenericToolbarImageResources.GenericSaveAs);
                    break;
                case PaletteMode.ProfessionalOffice2003:
                    UpdateActiveImage(Office2003ToolbarImageResources.Office2003ToolbarSaveNormal);
                    break;
                case PaletteMode.Office2007DarkGray:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007Blue:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007BlueDarkMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007BlueLightMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007Silver:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007SilverDarkMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007SilverLightMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007White:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007Black:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007BlackDarkMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010DarkGray:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010Blue:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010BlueDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010BlueLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010Silver:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010SilverDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010SilverLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010White:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010Black:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010BlackDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2013DarkGray:
                case PaletteMode.Office2013LightGray:
                case PaletteMode.Office2013White:
                    UpdateActiveImage(Office2013ToolbarImageResources.Office2013ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365DarkGray:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365Black:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365BlackDarkMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365Blue:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365BlueDarkMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365BlueLightMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365Silver:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365SilverDarkMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365SilverLightMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365White:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleBlue:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleBlueDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleBlueLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleOrange:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleOrangeDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleOrangeLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparklePurple:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparklePurpleDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparklePurpleLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Custom:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        /// <summary>Updates the disabled image.</summary>
        /// <param name="mode">The mode.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">mode - null</exception>
        private void UpdateDisabledImage(PaletteMode mode)
        {
            switch (mode)
            {
                case PaletteMode.Global:
                    break;
                case PaletteMode.ProfessionalSystem:
                    UpdateActiveImage(GenericToolbarImageResources.GenericSaveAs);
                    break;
                case PaletteMode.ProfessionalOffice2003:
                    UpdateActiveImage(Office2003ToolbarImageResources.Office2003ToolbarSaveDisabled);
                    break;
                case PaletteMode.Office2007DarkGray:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2007Blue:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2007BlueDarkMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2007BlueLightMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2007Silver:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2007SilverDarkMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2007SilverLightMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2007White:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2007Black:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2007BlackDarkMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2010DarkGray:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2010Blue:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2010BlueDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2010BlueLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2010Silver:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2010SilverDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2010SilverLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2010White:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2010Black:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2010BlackDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Office2013DarkGray:
                case PaletteMode.Office2013LightGray:
                case PaletteMode.Office2013White:
                    UpdateActiveImage(Office2013ToolbarImageResources.Office2013ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Microsoft365DarkGray:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Microsoft365Black:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Microsoft365BlackDarkMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Microsoft365Blue:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Microsoft365BlueDarkMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Microsoft365BlueLightMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Microsoft365Silver:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Microsoft365SilverDarkMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Microsoft365SilverLightMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Microsoft365White:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.SparkleBlue:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.SparkleBlueDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.SparkleBlueLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.SparkleOrange:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.SparkleOrangeDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.SparkleOrangeLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.SparklePurple:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.SparklePurpleDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.SparklePurpleLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled);
                    break;
                case PaletteMode.Custom:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        /// <summary>Updates the normal image.</summary>
        /// <param name="mode">The mode.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">mode - null</exception>
        private void UpdateNormalImage(PaletteMode mode)
        {
            switch (mode)
            {
                case PaletteMode.Global:
                    break;
                case PaletteMode.ProfessionalSystem:
                    UpdateActiveImage(GenericToolbarImageResources.GenericSaveAs);
                    break;
                case PaletteMode.ProfessionalOffice2003:
                    UpdateActiveImage(Office2003ToolbarImageResources.Office2003ToolbarSaveNormal);
                    break;
                case PaletteMode.Office2007DarkGray:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007Blue:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007BlueDarkMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007BlueLightMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007Silver:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007SilverDarkMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007SilverLightMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007White:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007Black:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007BlackDarkMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010DarkGray:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010Blue:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010BlueDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010BlueLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010Silver:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010SilverDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010SilverLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010White:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010Black:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010BlackDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2013DarkGray:
                case PaletteMode.Office2013LightGray:
                case PaletteMode.Office2013White:
                    UpdateActiveImage(Office2013ToolbarImageResources.Office2013ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365DarkGray:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365Black:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365BlackDarkMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365Blue:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365BlueDarkMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365BlueLightMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365Silver:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365SilverDarkMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365SilverLightMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365White:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleBlue:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleBlueDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleBlueLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleOrange:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleOrangeDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleOrangeLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparklePurple:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparklePurpleDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparklePurpleLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Custom:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        /// <summary>Updates the pressed image.</summary>
        /// <param name="mode">The mode.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">mode - null</exception>
        private void UpdatePressedImage(PaletteMode mode)
        {
            switch (mode)
            {
                case PaletteMode.Global:
                    break;
                case PaletteMode.ProfessionalSystem:
                    UpdateActiveImage(GenericToolbarImageResources.GenericSaveAs);
                    break;
                case PaletteMode.ProfessionalOffice2003:
                    UpdateActiveImage(Office2003ToolbarImageResources.Office2003ToolbarSaveNormal);
                    break;
                case PaletteMode.Office2007DarkGray:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007Blue:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007BlueDarkMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007BlueLightMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007Silver:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007SilverDarkMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007SilverLightMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007White:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007Black:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2007BlackDarkMode:
                    UpdateActiveImage(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010DarkGray:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010Blue:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010BlueDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010BlueLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010Silver:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010SilverDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010SilverLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010White:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010Black:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2010BlackDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Office2013DarkGray:
                case PaletteMode.Office2013LightGray:
                case PaletteMode.Office2013White:
                    UpdateActiveImage(Office2013ToolbarImageResources.Office2013ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365DarkGray:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365Black:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365BlackDarkMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365Blue:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365BlueDarkMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365BlueLightMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365Silver:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365SilverDarkMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365SilverLightMode:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Microsoft365White:
                    UpdateActiveImage(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleBlue:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleBlueDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleBlueLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleOrange:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleOrangeDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparkleOrangeLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparklePurple:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparklePurpleDarkMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.SparklePurpleLightMode:
                    UpdateActiveImage(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                    break;
                case PaletteMode.Custom:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        /// <summary>Updates the image states.</summary>
        /// <param name="mode">The mode.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">mode - null</exception>
        private void UpdateImageStates(PaletteMode mode)
        {
            if (_saveAsButtonSpec != null)
            {
                switch (mode)
                {
                    case PaletteMode.Global:
                        break;
                    case PaletteMode.ProfessionalSystem:
                        AddImageStates(null, SystemToolbarImageResources.SystemToolbarSaveDisabled, SystemToolbarImageResources.SystemToolbarSaveNormal, null);
                        break;
                    case PaletteMode.ProfessionalOffice2003:
                        AddImageStates(null, Office2003ToolbarImageResources.Office2003ToolbarSaveDisabled, Office2003ToolbarImageResources.Office2003ToolbarSaveNormal, null);
                        break;
                    case PaletteMode.Office2007DarkGray:
                        AddImageStates(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2007Blue:
                        AddImageStates(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2007BlueDarkMode:
                        AddImageStates(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2007BlueLightMode:
                        AddImageStates(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2007Silver:
                        AddImageStates(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2007SilverDarkMode:
                        AddImageStates(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2007SilverLightMode:
                        AddImageStates(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2007White:
                        AddImageStates(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2007Black:
                        AddImageStates(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2007BlackDarkMode:
                        AddImageStates(Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsDisabled, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal, Office2007ToolbarImageResources.Office2007ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2010DarkGray:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2010Blue:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2010BlueDarkMode:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2010BlueLightMode:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2010Silver:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2010SilverDarkMode:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2010SilverLightMode:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2010White:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2010Black:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2010BlackDarkMode:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Office2013DarkGray:
                    case PaletteMode.Office2013LightGray:
                    case PaletteMode.Office2013White:
                        AddImageStates(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Microsoft365DarkGray:
                        AddImageStates(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Microsoft365Black:
                        AddImageStates(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Microsoft365BlackDarkMode:
                        AddImageStates(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Microsoft365Blue:
                        AddImageStates(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Microsoft365BlueDarkMode:
                        AddImageStates(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Microsoft365BlueLightMode:
                        AddImageStates(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Microsoft365Silver:
                        AddImageStates(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Microsoft365SilverDarkMode:
                        AddImageStates(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Microsoft365SilverLightMode:
                        AddImageStates(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Microsoft365White:
                        AddImageStates(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.SparkleBlue:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.SparkleBlueDarkMode:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.SparkleBlueLightMode:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.SparkleOrange:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.SparkleOrangeDarkMode:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.SparkleOrangeLightMode:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.SparklePurple:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.SparklePurpleDarkMode:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.SparklePurpleLightMode:
                        AddImageStates(Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsDisabled, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal, Office2010ToolbarImageResources.Office2010ToolbarSaveAsNormal);
                        break;
                    case PaletteMode.Custom:
                        AddImageStates(Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsDisabled, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal, Office2019ToolbarImageResources.Office2019ToolbarSaveAsNormal);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                }
            }
        }

        #endregion
    }
}