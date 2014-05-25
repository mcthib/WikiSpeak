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
    /// <summary>
    /// A user control that emulates a really long RichTextBox by stacking several RichTextBox controls together.
    /// This is needed because there is a vertical height limit to a RichTextBox in a ScrollViewer beyond which it will not render text.
    /// Also allows for highlighting.
    /// </summary>
    public partial class ScrollableRichTextBox : UserControl
    {
        /// <summary>
        /// The maximum number of allowable characters per block
        /// </summary>
		private const int MaxBlockCharacterCount = 3000;

		public ScrollableRichTextBox()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register(
				"Text",
				typeof(string),
                typeof(ScrollableRichTextBox),
                new PropertyMetadata("ScrollableRichTextBox", OnTextPropertyChanged));

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

		private static void OnTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
            ScrollableRichTextBox source = sender as ScrollableRichTextBox;
            if (source != null)
            {
                string value = (string)args.NewValue;
                source.ParseText(value);
            }
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

            int cursor = 0, blockStart = 0;
            while (blockStart < value.Length)
            {
                int nextLine = value.IndexOf("\n", blockStart);
                if (nextLine > 0)
                {
                    cursor = Math.Min(blockStart + MaxBlockCharacterCount, nextLine);
                }
                else
                {
                    cursor = Math.Min(value.Length, value.LastIndexOf(" ", blockStart, MaxBlockCharacterCount));
                }

                RichTextBoxEx block = new RichTextBoxEx();
                block.Text = value.Substring(blockStart, cursor - blockStart);
                this.StackPanel.Children.Add(block);

                blockStart = cursor + 1;
            }
		}
    }
}
