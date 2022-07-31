using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EconomyViewer.Utility
{
    /// <summary>
    /// Логика взаимодействия для MyMessageBox.xaml
    /// </summary>
    public partial class MyMessageBox : Window
    {
        internal string Caption {
            get => Title;
            set => Title = value;
        }

        internal string Message {
            get => TextBlock_Message.Text;
            set => TextBlock_Message.Text = value;
        }

        internal string OkButtonText {
            get => Label_Ok.Content.ToString()!;
            set => Label_Ok.Content = value.TryAddKeyboardAccellerator();
        }

        internal string CancelButtonText {
            get => Label_Cancel.Content.ToString()!;
            set => Label_Cancel.Content = value.TryAddKeyboardAccellerator();
        }

        internal string YesButtonText {
            get => Label_Yes.Content.ToString()!;
            set => Label_Yes.Content = value.TryAddKeyboardAccellerator();
        }

        internal string NoButtonText {
            get => Label_No.Content.ToString()!;
            set => Label_No.Content = value.TryAddKeyboardAccellerator();
        }
        public MessageBoxResult Result { get; set; }

        internal MyMessageBox(string message)
        {
            InitializeComponent();

            Message = message;
            Image_MessageBox.Visibility = Visibility.Collapsed;
            DisplayButtons(MessageBoxButton.OK);
        }
        internal MyMessageBox(string message, string caption)
        {
            InitializeComponent();

            Message = message;
            Caption = caption;
            Image_MessageBox.Visibility = Visibility.Collapsed;
            DisplayButtons(MessageBoxButton.OK);
        }

        internal MyMessageBox(string message, string caption, MessageBoxButton button)
        {
            InitializeComponent();

            Message = message;
            Caption = caption;
            Image_MessageBox.Visibility = Visibility.Collapsed;

            DisplayButtons(button);
        }

        internal MyMessageBox(string message, string caption, MessageBoxImage image)
        {
            InitializeComponent();

            Message = message;
            Caption = caption;
            DisplayImage(image);
            DisplayButtons(MessageBoxButton.OK);
        }

        internal MyMessageBox(string message, string caption, MessageBoxButton button, MessageBoxImage image)
        {
            InitializeComponent();

            Message = message;
            Caption = caption;
            Image_MessageBox.Visibility = Visibility.Collapsed;

            DisplayButtons(button);
            DisplayImage(image);
        }

        private void DisplayButtons(MessageBoxButton button)
        {
            List<System.Windows.Controls.Button>? resultButtons = new List<System.Windows.Controls.Button>()
            {
                Button_OK,
                Button_Cancel,
                Button_Yes,
                Button_No
            };
            Dictionary<MessageBoxButton, List<System.Windows.Controls.Button>>? dependencyButton = new Dictionary<MessageBoxButton, List<System.Windows.Controls.Button>>()
            {
                {MessageBoxButton.OK, new List<System.Windows.Controls.Button>() {Button_OK}},
                {MessageBoxButton.OKCancel, new List<System.Windows.Controls.Button>() {Button_OK, Button_Cancel}},
                {MessageBoxButton.YesNo, new List<System.Windows.Controls.Button>() {Button_Yes, Button_No}},
                {MessageBoxButton.YesNoCancel, new List<System.Windows.Controls.Button>() {Button_Yes, Button_No, Button_Cancel}}
            };
            void HideExept(List<System.Windows.Controls.Button> buttons)
            {
                resultButtons.ForEach(c => c.Visibility = Visibility.Collapsed);
                buttons.ToList().ForEach(c => c.Visibility = Visibility.Visible);
                buttons[0].Focus();
            }
            HideExept(dependencyButton[button]);
        }

        private void DisplayImage(MessageBoxImage image)
        {
            string path = image switch {
                MessageBoxImage.Exclamation => "Assets/warning.png",
                MessageBoxImage.Error => "Assets/error.png",
                MessageBoxImage.Information => "Assets/info.png",
                MessageBoxImage.Question => "Assets/help.png",
                _ => "Assets/warning.png",
            };
            Image_MessageBox.Source = new BitmapImage(new Uri(path, UriKind.Relative));
            Image_MessageBox.Visibility = Visibility.Visible;
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            Close();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }

        private void Button_Yes_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Yes;
            Close();
        }

        private void Button_No_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.No;
            Close();
        }
    }
    public partial class MyMessageBox
    {
        /// <summary>
        /// Displays a message box that has a message and returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult Show(string messageBoxText)
        {
            MyMessageBox? msg = new MyMessageBox(messageBoxText);
            msg.ShowDialog();

            return msg.Result;
        }

        /// <summary>
        /// Displays a message box that has a message and a title bar caption; and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult Show(string messageBoxText, string caption)
        {
            MyMessageBox? msg = new MyMessageBox(messageBoxText, caption);
            msg.ShowDialog();

            return msg.Result;
        }

        /// <summary>
        /// Displays a message box in front of the specified window. The message box displays a message and returns a result.
        /// </summary>
        /// <param name="owner">A System.Windows.Window that represents the owner window of the message box.</param>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult Show(Window owner, string messageBoxText)
        {
            MyMessageBox? msg = new MyMessageBox(messageBoxText) {
                Owner = owner
            };
            msg.ShowDialog();

            return msg.Result;
        }

        /// <summary>
        /// Displays a message box in front of the specified window. The message box displays a message and title bar caption; and it returns a result.
        /// </summary>
        /// <param name="owner">A System.Windows.Window that represents the owner window of the message box.</param>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption)
        {
            MyMessageBox? msg = new MyMessageBox(messageBoxText, caption) {
                Owner = owner
            };
            msg.ShowDialog();

            return msg.Result;
        }

        /// <summary>
        /// Displays a message box that has a message, title bar caption, and button; and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="button">A System.Windows.MessageBoxButton value that specifies which button or buttons to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            MyMessageBox? msg = new MyMessageBox(messageBoxText, caption, button);
            msg.ShowDialog();

            return msg.Result;
        }

        /// <summary>
        /// Displays a message box that has a message, title bar caption, button, and icon; and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="button">A System.Windows.MessageBoxButton value that specifies which button or buttons to display.</param>
        /// <param name="icon">A System.Windows.MessageBoxImage value that specifies the icon to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            MyMessageBox? msg = new MyMessageBox(messageBoxText, caption, button, icon);
            msg.ShowDialog();

            return msg.Result;
        }

        /// <summary>
        /// Displays a message box that has a message, title bar caption, and OK button with a custom System.String value for the button's text; and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="okButtonText">A System.String that specifies the text to display within the OK button.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult ShowOK(string messageBoxText, string caption, string okButtonText)
        {
            MyMessageBox? msg = new MyMessageBox(messageBoxText, caption, MessageBoxButton.OK) {
                OkButtonText = okButtonText
            };

            msg.ShowDialog();

            return msg.Result;
        }

        /// <summary>
        /// Displays a message box that has a message, title bar caption, OK button with a custom System.String value for the button's text, and icon; and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="okButtonText">A System.String that specifies the text to display within the OK button.</param>
        /// <param name="icon">A System.Windows.MessageBoxImage value that specifies the icon to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult ShowOK(string messageBoxText, string caption, string okButtonText, MessageBoxImage icon)
        {
            MyMessageBox? msg = new MyMessageBox(messageBoxText, caption, MessageBoxButton.OK, icon) {
                OkButtonText = okButtonText
            };

            msg.ShowDialog();

            return msg.Result;
        }

        /// <summary>
        /// Displays a message box that has a message, caption, and OK/Cancel buttons with custom System.String values for the buttons' text;
        /// and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="okButtonText">A System.String that specifies the text to display within the OK button.</param>
        /// <param name="cancelButtonText">A System.String that specifies the text to display within the Cancel button.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult ShowOKCancel(string messageBoxText, string caption, string okButtonText, string cancelButtonText)
        {
            MyMessageBox? msg = new MyMessageBox(messageBoxText, caption, MessageBoxButton.OKCancel) {
                OkButtonText = okButtonText,
                CancelButtonText = cancelButtonText
            };

            msg.ShowDialog();

            return msg.Result;
        }

        /// <summary>
        /// Displays a message box that has a message, caption, OK/Cancel buttons with custom System.String values for the buttons' text, and icon;
        /// and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="okButtonText">A System.String that specifies the text to display within the OK button.</param>
        /// <param name="cancelButtonText">A System.String that specifies the text to display within the Cancel button.</param>
        /// <param name="icon">A System.Windows.MessageBoxImage value that specifies the icon to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult ShowOKCancel(string messageBoxText, string caption, string okButtonText, string cancelButtonText, MessageBoxImage icon)
        {
            MyMessageBox? msg = new MyMessageBox(messageBoxText, caption, MessageBoxButton.OKCancel, icon) {
                OkButtonText = okButtonText,
                CancelButtonText = cancelButtonText
            };

            msg.ShowDialog();

            return msg.Result;
        }

        /// <summary>
        /// Displays a message box that has a message, caption, and Yes/No buttons with custom System.String values for the buttons' text;
        /// and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="yesButtonText">A System.String that specifies the text to display within the Yes button.</param>
        /// <param name="noButtonText">A System.String that specifies the text to display within the No button.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult ShowYesNo(string messageBoxText, string caption, string yesButtonText, string noButtonText)
        {
            MyMessageBox? msg = new MyMessageBox(messageBoxText, caption, MessageBoxButton.YesNo) {
                YesButtonText = yesButtonText,
                NoButtonText = noButtonText
            };

            msg.ShowDialog();

            return msg.Result;
        }

        /// <summary>
        /// Displays a message box that has a message, caption, Yes/No buttons with custom System.String values for the buttons' text, and icon;
        /// and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="yesButtonText">A System.String that specifies the text to display within the Yes button.</param>
        /// <param name="noButtonText">A System.String that specifies the text to display within the No button.</param>
        /// <param name="icon">A System.Windows.MessageBoxImage value that specifies the icon to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult ShowYesNo(string messageBoxText, string caption, string yesButtonText, string noButtonText, MessageBoxImage icon)
        {
            MyMessageBox? msg = new MyMessageBox(messageBoxText, caption, MessageBoxButton.YesNo, icon) {
                YesButtonText = yesButtonText,
                NoButtonText = noButtonText
            };

            msg.ShowDialog();

            return msg.Result;
        }

        /// <summary>
        /// Displays a message box that has a message, caption, and Yes/No/Cancel buttons with custom System.String values for the buttons' text;
        /// and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="yesButtonText">A System.String that specifies the text to display within the Yes button.</param>
        /// <param name="noButtonText">A System.String that specifies the text to display within the No button.</param>
        /// <param name="cancelButtonText">A System.String that specifies the text to display within the Cancel button.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult ShowYesNoCancel(string messageBoxText, string caption, string yesButtonText, string noButtonText, string cancelButtonText)
        {
            MyMessageBox? msg = new MyMessageBox(messageBoxText, caption, MessageBoxButton.YesNoCancel) {
                YesButtonText = yesButtonText,
                NoButtonText = noButtonText,
                CancelButtonText = cancelButtonText
            };

            msg.ShowDialog();

            return msg.Result;
        }

        /// <summary>
        /// Displays a message box that has a message, caption, Yes/No/Cancel buttons with custom System.String values for the buttons' text, and icon;
        /// and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="yesButtonText">A System.String that specifies the text to display within the Yes button.</param>
        /// <param name="noButtonText">A System.String that specifies the text to display within the No button.</param>
        /// <param name="cancelButtonText">A System.String that specifies the text to display within the Cancel button.</param>
        /// <param name="icon">A System.Windows.MessageBoxImage value that specifies the icon to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult ShowYesNoCancel(string messageBoxText, string caption, string yesButtonText, string noButtonText, string cancelButtonText, MessageBoxImage icon)
        {
            MyMessageBox? msg = new MyMessageBox(messageBoxText, caption, MessageBoxButton.YesNoCancel, icon) {
                YesButtonText = yesButtonText,
                NoButtonText = noButtonText,
                CancelButtonText = cancelButtonText
            };

            msg.ShowDialog();

            return msg.Result;
        }

    }
    internal static class Util
    {
        internal static ImageSource ToImageSource(this Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }

        /// <summary>
        /// Keyboard Accellerators are used in Windows to allow easy shortcuts to controls like Buttons and 
        /// MenuItems. These allow users to press the Alt key, and a shortcut key will be highlighted on the 
        /// control. If the user presses that key, that control will be activated.
        /// This method checks a string if it contains a keyboard accellerator. If it doesn't, it adds one to the
        /// beginning of the string. If there are two strings with the same accellerator, Windows handles it.
        /// The keyboard accellerator character for WPF is underscore (_). It will not be visible.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static string TryAddKeyboardAccellerator(this string input)
        {
            // This is the default WPF accellerator symbol - used to be & in WinForms
            // If it already contains an accellerator, do nothing
            return input.Contains('_') ? input : "_" + input;
        }
    }
}
