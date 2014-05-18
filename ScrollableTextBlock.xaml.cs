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

namespace WikiSpeak
{
	public partial class ScrollableTextBlock : UserControl
	{
		private const int MaxTextCount = 3000;

		public ScrollableTextBlock()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register(
				"Text",
				typeof(string),
				typeof(ScrollableTextBlock),
				new PropertyMetadata("ScrollableTextBlock", OnTextPropertyChanged));

		public string Text
		{
			get
			{
				return (string)GetValue(TextProperty);
			}
			set
			{
				SetValue(TextProperty, value);
			}
		}

		private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ScrollableTextBlock source = (ScrollableTextBlock)d;
			string value = (string)e.NewValue;
			source.ParseText(value);
		}

		private static TextBlock GetTextBlock()
		{
			return new TextBlock()
			{
				// All properties set in the Style
			};
		}

		private void ParseText(string value)
		{
			if (this.StackPanel == null)
			{
				return;
			}
			
			if (value == null)
			{
				value = string.Empty;
			}

			// Clear previous TextBlocks
			this.StackPanel.Children.Clear();

			Brush whiteBrush = new SolidColorBrush(Colors.White);

			for (int index = 0; index < value.Length; index += MaxTextCount)
			{
				TextBlock textBlock = GetTextBlock();
				textBlock.Text = value.Substring(index, Math.Min(MaxTextCount, value.Length - index));
				textBlock.Foreground = whiteBrush;
				this.StackPanel.Children.Add(textBlock);
			}
		}
	}
}
