namespace Iridium.WPFClient
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;
    using ICSharpCode.AvalonEdit;
    using ICSharpCode.AvalonEdit.Highlighting;

    public class CodeEditor : TextEditor, INotifyPropertyChanged
    {
        public CodeEditor()
        {
            this.FontFamily = new FontFamily("Consolas");
            this.FontSize = 12;
            this.ShowLineNumbers = true;
            this.Options = new TextEditorOptions
            {
                HighlightCurrentLine = true,
            };
            this.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
        }

        /// <summary>
        /// A bindable Text property
        /// </summary>
        public new string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        /// The bindable text property dependency property
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(CodeEditor), new PropertyMetadata((obj, args) =>
            {
                var target = (CodeEditor)obj;
                target.Text = (string)args.NewValue;
            }));

        protected override void OnTextChanged(EventArgs e)
        {
            this.RaisePropertyChanged("Text");
            base.OnTextChanged(e);
        }

        /// <summary>
        /// Raises a property changed event
        /// </summary>
        /// <param name="property">The name of the property that updates</param>
        public void RaisePropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}