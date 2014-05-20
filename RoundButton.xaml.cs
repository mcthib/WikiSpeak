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
        }

        public static DependencyProperty ImageDependencyProperty = DependencyProperty.Register("ImageSource", typeof(Uri), typeof(RoundButton), new PropertyMetadata(null, ImageDependencyPropertyChanged));

        public Uri ImageSource
        {
            get
            {
                return GetValue(ImageDependencyProperty) as Uri;
            }
            set
            {
                SetValue(ImageDependencyProperty, value);
            }
        }

        private static T GetChildRecursive<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            DependencyObject returnValue = null;

            for (int i = 0; (i < count) && (returnValue != null); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
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

        public static void ImageDependencyPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            RoundButton roundButton = dependencyObject as RoundButton;
            if (roundButton != null)
            {
                Image child = GetChildRecursive<Image>(dependencyObject, "blah");
                if (child != null)
                {
                    BitmapImage bitmap = new BitmapImage(dependencyPropertyChangedEventArgs.NewValue as Uri);
                    child.Source = bitmap;
                }

                //roundButton.ButtonImage.Source = (Uri)(dependencyPropertyChangedEventArgs.NewValue);
            }
        }
    }
}
