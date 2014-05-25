using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WikiSpeak
{
    /// <summary>
    /// An extended RichTextBox that lets us do highlights and bind the text
    /// </summary>
    public class RichTextBoxEx : RichTextBox
    {
        public static DependencyProperty RichTextBoxExProperty = DependencyProperty.Register("Text", typeof(string), typeof(RichTextBoxEx), new PropertyMetadata(null, TextPropertyChanged));

        public string Text
        {
            get
            {
                return (string)GetValue(RichTextBoxExProperty);
            }
            set
            {
                SetValue(RichTextBoxExProperty, value);
            }
        }

        private static void TextPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            RichTextBoxEx richTextBoxEx = dependencyObject as RichTextBoxEx;
            if (richTextBoxEx != null)
            {
                string text = (dependencyPropertyChangedEventArgs.NewValue as string ?? string.Empty);

                richTextBoxEx.Blocks.Clear();

                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(text);
                richTextBoxEx.Blocks.Add(paragraph);
            }
        }

        /// <summary>
        /// Highlists the portion of the textbox in between 2 indices
        /// </summary>
        /// <param name="startIndex">start index</param>
        /// <param name="endIndex">end index</param>
        public void Highlight(int startIndex, int endIndex)
        {
            if ((startIndex >= 0) && (startIndex < this.Text.Length) && (startIndex <= endIndex))
            {
                endIndex = Math.Min(this.Text.Length - 1, endIndex);

                Paragraph paragraph = new Paragraph();
                
                // first section of the run (no format)
                if (startIndex > 0)
                {
                    paragraph.Inlines.Add(this.Text.Substring(0, startIndex));
                }
                
                // Middles section is where we format
                Run run = new Run() { Text = this.Text.Substring(startIndex, endIndex - startIndex + 1) };
                run.FontWeight = FontWeights.ExtraBold;
                run.Foreground = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
                paragraph.Inlines.Add(run);

                // end of the run (no format)
                if (endIndex != this.Text.Length - 1)
                {
                    paragraph.Inlines.Add(this.Text.Substring(endIndex + 1));
                }

                // Replace existing paragraph
                this.Blocks.Clear();
                this.Blocks.Add(paragraph);
            }
        }
    }
}
