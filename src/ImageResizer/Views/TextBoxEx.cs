using System.Windows;
using System.Windows.Controls;

namespace ImageResizer.Views
{
    public class TextBoxEx : TextBox
    {
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
            nameof(Placeholder),
            typeof(string),
            typeof(TextBoxEx),
            new PropertyMetadata(string.Empty));

        static readonly DependencyPropertyKey HasTextPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(HasText),
            typeof(bool),
            typeof(TextBoxEx),
            new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;

        static TextBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(TextBoxEx),
                new FrameworkPropertyMetadata(typeof(TextBoxEx)));
            TextProperty.OverrideMetadata(
                typeof(TextBoxEx),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(TextPropertyChanged)));
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public bool HasText
            => (bool)GetValue(HasTextProperty);

        static void TextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var textBox = (TextBoxEx)sender;
            var hasText = textBox.Text.Length > 0;

            if (textBox.HasText != hasText)
                textBox.SetValue(HasTextPropertyKey, hasText);
        }
    }
}
