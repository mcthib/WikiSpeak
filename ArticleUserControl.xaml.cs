using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WikiSpeak
{
    public partial class ArticleUserControl : UserControl
    {
        public ArticleUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// event signalling that the item got selected
        /// </summary>
        public event EventHandler ButtonClicked;

        /// <summary>
        /// Fires the Tapped event
        /// </summary>
        private void FireButtonClickedEvent()
        {
            if (ButtonClicked != null)
            {
                ButtonClicked(this, new EventArgs());
            }
        }

        private void Button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            FireButtonClickedEvent();
        }

        public static DependencyProperty ExcerptVisibilityDependencyProperty = DependencyProperty.Register("ExcerptVisibility", typeof(Visibility), typeof(ArticleUserControl), new PropertyMetadata(Visibility.Collapsed, ExcerptVisibilityChanged));

        public static void ExcerptVisibilityChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ArticleUserControl control = dependencyObject as ArticleUserControl;
            if (control != null)
            {
                control.Excerpt.Visibility = (Visibility)(dependencyPropertyChangedEventArgs.NewValue);
            }
        }

        public Visibility ExcerptVisibility
        {
            get
            {
                return (Visibility)GetValue(ExcerptVisibilityDependencyProperty);
            }
            set
            {
                SetValue(ExcerptVisibilityDependencyProperty, value);
            }
        }

        public static DependencyProperty ButtonImageSourceDependencyProperty = DependencyProperty.Register("ButtonImageSource", typeof(Uri), typeof(ArticleUserControl), new PropertyMetadata(null, ButtonImageSourceChanged));

        public Uri ButtonImageSource
        {
            get
            {
                return (Uri)GetValue(ButtonImageSourceDependencyProperty);
            }
            set
            {
                SetValue(ButtonImageSourceDependencyProperty, value);
            }
        }

        public static void ButtonImageSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ArticleUserControl control = dependencyObject as ArticleUserControl;
            if (control != null)
            {
                control.Button.SetValue(RoundButton.ImageSourceDependencyProperty, (Uri)(dependencyPropertyChangedEventArgs.NewValue));
            }
        }

    }
}
