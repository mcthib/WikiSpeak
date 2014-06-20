using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WikiSpeak
{
    /// <summary>
    /// A round button that contains an image and gives visual feedback upon being pressed
    /// </summary>
    public partial class BigRoundButton : Button
    {
        public BigRoundButton()
        {
            InitializeComponent();

            // Bind VisibilityMirror to Visibility so we can have a VisibilityChanged handler.
            // This in turn enables us to refresh the image button after the visual tree is populated.
            //SetBinding(FrameworkElement.VisibilityProperty, new System.Windows.Data.Binding("VisibilityMirror") { Source = this });
        }

        #region DependencyProperty: ImageSource
        public static DependencyProperty ImageSourceDependencyProperty = DependencyProperty.Register("ImageSource", typeof(Uri), typeof(BigRoundButton), new PropertyMetadata(null, ImageSourceDependencyPropertyChanged));

        public Uri ImageSource
        {
            get
            {
                return GetValue(ImageSourceDependencyProperty) as Uri;
            }
            set
            {
                SetValue(ImageSourceDependencyProperty, value);
            }
        }

        public static void ImageSourceDependencyPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            BigRoundButton roundButton = dependencyObject as BigRoundButton;
            if (roundButton != null)
            {
                roundButton.RefreshButtonImage();
            }
        }
        #endregion DependencyProperty: ImageSource

        /// <summary>
        /// Refreshes the image used as button icon
        /// </summary>
        private void RefreshButtonImage()
        {
            Image child = RoundButton.GetChildRecursive<Image>(this, "Image");
            if (child != null)
            {
                BitmapImage bitmap = new BitmapImage(this.ImageSource);
                child.Source = bitmap;
            }
        }

        /// <summary>
        /// Handler for the button image being loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            this.RefreshButtonImage();
        }
    }
}
