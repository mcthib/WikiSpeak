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
    public partial class RoundButton : Button
    {
        public RoundButton()
        {
            InitializeComponent();

            // Bind VisibilityMirror to Visibility so we can have a VisibilityChanged handler.
            // This in turn enables us to refresh the image button after the visual tree is populated.
            //SetBinding(FrameworkElement.VisibilityProperty, new System.Windows.Data.Binding("VisibilityMirror") { Source = this });
        }

        #region DependencyProperty: ImageSource
        public static DependencyProperty ImageSourceDependencyProperty = DependencyProperty.Register("ImageSource", typeof(Uri), typeof(RoundButton), new PropertyMetadata(null, ImageSourceDependencyPropertyChanged));

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
            RoundButton roundButton = dependencyObject as RoundButton;
            if (roundButton != null)
            {
                roundButton.RefreshButtonImage();
            }
        }
        #endregion DependencyProperty: ImageSource

        /// <summary>
        /// A recursive method that finds the control of type T and given name among the child controls
        /// </summary>
        /// <typeparam name="T">the type of the control to look for</typeparam>
        /// <param name="parent">parent control, i.e. this at the beginning of recursion</param>
        /// <param name="name">name of the control to look for</param>
        /// <remarks>will not work until the button is loaded</remarks>
        /// <returns>the control searched for, or null</returns>
        private static T GetChildRecursive<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            DependencyObject returnValue = null;

            for (int i = 0; (i < count) && (returnValue == null); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if ((child is T) && string.Equals(child.GetValue(FrameworkElement.NameProperty), name))
                {
                    returnValue = child;
                }
                else
                {
                    returnValue = GetChildRecursive<T>(child, name);
                }

            }

            return (T)returnValue;
        }

        /// <summary>
        /// Refreshes the image used as button icon
        /// </summary>
        private void RefreshButtonImage()
        {
            Image child = GetChildRecursive<Image>(this, "Image");
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
