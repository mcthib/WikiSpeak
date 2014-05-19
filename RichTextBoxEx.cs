using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

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
    }
}
