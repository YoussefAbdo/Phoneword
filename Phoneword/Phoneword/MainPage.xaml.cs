using System;
using Xamarin.Forms;

namespace Phoneword
{
    public partial class MainPage : ContentPage
    {
        string translatedNumber;
        Label enterPhoneWord;
        Entry phoneNumberText;
        Button translateButton;
        Button callButton;
        public MainPage()
        {

            this.Padding = new Thickness(20, 20, 20, 20);

            StackLayout panel = new StackLayout
            {
                Spacing = 15
                
            };

            enterPhoneWord = new Label
            {
                Text = "Enter a Phoneword",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            };
            phoneNumberText = new Entry {
                Text = "1-855-XAMARIN"
            };
            translateButton = new Button {
                Text = "Translate"
            };
            callButton = new Button
            {
                Text = "Call",
                IsEnabled = false,
            };

            panel.Children.Add(enterPhoneWord);
            panel.Children.Add(phoneNumberText);
            panel.Children.Add(translateButton);
            panel.Children.Add(callButton);

            translateButton.Clicked += OnTranslate;
            callButton.Clicked += OnCall;
            this.Content = panel;
        }

        private void OnTranslate(object sender, EventArgs e)
        {
            string enteredNumber = phoneNumberText.Text;
            translatedNumber = Core.PhonewordTranslator.ToNumber(enteredNumber);

            if (!string.IsNullOrEmpty(translatedNumber))
            {
                callButton.IsEnabled = true;
                callButton.Text = "Call " + translatedNumber;
            }
            else
            {
                callButton.IsEnabled = false;
                callButton.Text = "Call";
            }
        }
        async void OnCall (Object sender, EventArgs e)
        {
            if (await this.DisplayAlert(
                "Dial a Number",
                "Would you like to call " + translatedNumber + "?",
                "Yes",
                "No"))
            {
                try
                {
                    var dialer = DependencyService.Get<IDialer>();
                    if (dialer != null)
                    {
                        await dialer.DialAsync(translatedNumber);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}