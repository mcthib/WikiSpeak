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
using System.Text;

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
            ScrollableRichTextBox richTextBoxEx = sender as ScrollableRichTextBox;
            if (richTextBoxEx != null)
            {
                string text = (string)args.NewValue;
                richTextBoxEx.ParseText(text);

                richTextBoxEx.Highlight(1, 15);
            }
		}

        /// <summary>
        /// Highlights the text between indices
        /// <remarks>cannot cross RichTextBoxEx boundaries (but can be a subset)</remarks>
        /// </summary>
        /// <param name="startIndex">first character to highlight</param>
        /// <param name="endIndex">last character to highlight</param>
        public void Highlight(int startIndex, int endIndex)
        {
            int runningCount = 0;

            foreach (object child in this.StackPanel.Children)
            {
                RichTextBoxEx richTextBoxEx = child as RichTextBoxEx;
                if (richTextBoxEx != null)
                {
                    if (runningCount + richTextBoxEx.Text.Length > startIndex)
                    {
                        richTextBoxEx.Highlight(startIndex - runningCount, endIndex - runningCount);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Parses the text into stacked RichTextBlock's, as big as it can make them.
        /// <remarks>I've seen the 512 MB emulator crash (OOM) with ~100K text.</remarks>
        /// </summary>
        /// <param name="text"></param>
		private void ParseText(string text)
		{
			if (this.StackPanel == null)
			{
				return;
			}
			
			if (text == null)
			{
				text = string.Empty;
			}

			// Clear previous TextBlocks
			this.StackPanel.Children.Clear();

            int cursor = 0, blockStart = 0;
            StringBuilder blockText = new StringBuilder();
            while (blockStart < text.Length)
            {
                int nextLine = text.IndexOf("\n", blockStart);
                if (nextLine > 0)
                {
                    cursor = Math.Min(blockStart + MaxBlockCharacterCount, nextLine);
                }
                else
                {
                    cursor = Math.Min(text.Length, text.LastIndexOf(" ", blockStart, MaxBlockCharacterCount));
                }

                if (blockText.Length + cursor - blockStart > MaxBlockCharacterCount)
                {
                    RichTextBoxEx block = new RichTextBoxEx() { Text = blockText.ToString() };
                    this.StackPanel.Children.Add(block);
                    blockText.Clear();
                }

                blockText.AppendLine(text.Substring(blockStart, cursor - blockStart));

                blockStart = cursor + 1;
            }

            if (blockText.Length > 0)
            {
                RichTextBoxEx block = new RichTextBoxEx() { Text = blockText.ToString() };
                this.StackPanel.Children.Add(block);
            }
		}
    }
}
