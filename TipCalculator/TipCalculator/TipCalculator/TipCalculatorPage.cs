using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace TipCalculator
{
    public class TipCalculatorPage : ContentPage
    {
        private Grid grid;
        
        private EntryCell billEntryCell;

        private Label tipPersentageLabel;
        private Slider tipPersentageSlider;

        private Label numberOfPeopleLabel;
        private Stepper numberOfPeopleStepper;
        private Label numberOfPeopleValueLabel;

        private Button calculateTipButton;

        private Label tipAmountTextLabel;
        private Label tipAmountValueLabel;

        private Label totalAmountTextLabel;
        private Label totalAmountValueLabel;

        private Label amountPerPersonTextLabel;
        private Label amountPerPersonValueLabel;

        public TipCalculatorPage()
        {
            InitializePage();
        }

        private void InitializePage()
        {
            grid = new Grid()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            InitializeGrid();
        }
        
        private void InitializeGrid()
        {
            InitializeElements();
            grid.Children.Add(new TableView()
            {
                Root = new TableRoot
                        {
                            new TableSection
                            {
                                billEntryCell
                            }
                        },
                Intent = TableIntent.Settings,

            }, 0, 2, 0, 2);

            grid.Children.Add(tipPersentageLabel,0,2);
            grid.Children.Add(tipPersentageSlider, 1, 2);

            grid.Children.Add(numberOfPeopleLabel, 0, 3);
            grid.Children.Add(numberOfPeopleValueLabel, 1, 3);
            grid.Children.Add(numberOfPeopleStepper, 1, 3);

            grid.Children.Add(calculateTipButton, 0, 4);
            Grid.SetColumnSpan(calculateTipButton, 2);

            grid.Children.Add(tipAmountTextLabel, 0, 5);
            grid.Children.Add(tipAmountValueLabel, 1, 5);

            grid.Children.Add(totalAmountTextLabel, 0, 6);
            grid.Children.Add(totalAmountValueLabel, 1, 6);

            grid.Children.Add(amountPerPersonTextLabel, 0, 7);
            grid.Children.Add(amountPerPersonValueLabel, 1, 7);
            // Accomodate iPhone status bar.
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            // Build the page.
            this.Content = grid;
        }

        private void InitializeElements()
        {
            billEntryCell = CreateEntryCell();
            tipPersentageLabel = CreateLabel("Your tip rate, 0 %:");
            tipPersentageLabel.BindingContext = tipPersentageSlider;
            
            tipPersentageSlider = CreateSlider();
            tipPersentageSlider.ValueChanged += SliderValueChanged;

            numberOfPeopleLabel = CreateLabel("Number of people:");
            numberOfPeopleLabel.SetBinding(Label.TextProperty, "NumberOfPeople");
            numberOfPeopleStepper = CreateStepper();
            numberOfPeopleStepper.ValueChanged += StepperValueChanged;
            numberOfPeopleValueLabel = CreateLabel(numberOfPeopleStepper.Value.ToString());

            tipAmountTextLabel = CreateLabel("Tip amount: ");
            tipAmountValueLabel = CreateLabel(string.Empty);

            totalAmountTextLabel = CreateLabel("Total amount: ");
            totalAmountValueLabel = CreateLabel(string.Empty);

            amountPerPersonTextLabel = CreateLabel("Tip per person:");
            amountPerPersonValueLabel = CreateLabel(string.Empty);

            calculateTipButton = CreateButton();
            calculateTipButton.Clicked += CalculateTipButton_Clicked;
        }

        private void StepperValueChanged(object sender, EventArgs e)
        {
            numberOfPeopleValueLabel.Text = ((Stepper) sender).Value.ToString();
        }

        private void SliderValueChanged(object sender, EventArgs e)
        {
            tipPersentageLabel.Text = string.Format("Your tip rate, {0} %:", ((Slider) sender).Value);
        }

        private void CalculateTipButton_Clicked(object sender, EventArgs e)
        {
            decimal bill;
            decimal tipRate;
            decimal numberOfPerson;

            decimal tipAmount;
            decimal totalAmount;
            decimal tipPerPerson;

            if (billEntryCell.Text == null || !Decimal.TryParse(billEntryCell.Text.Trim(), out bill))
            {
                DisplayAlert("Error", "Please, enter a valid count of your bill", "OK");
                return;
            }
            
            tipRate = (decimal)tipPersentageSlider.Value;
            numberOfPerson = (decimal)numberOfPeopleStepper.Value;

            tipAmount = bill * tipRate / 100;
            totalAmount = bill + tipAmount;
            tipPerPerson = totalAmount / numberOfPerson;

            tipAmountValueLabel.Text = string.Format("{0:0.00} $", tipAmount);
            totalAmountValueLabel.Text = string.Format("{0:0.00} $", totalAmount);
            amountPerPersonValueLabel.Text = string.Format("{0:0.00} $", tipPerPerson);
        }
        
        private Label CreateLabel(string labelText)
        {
            return new Label
            {
                Text = labelText,
                FontAttributes = FontAttributes.Bold,
                LineBreakMode = LineBreakMode.MiddleTruncation,
                TextColor = Color.Aqua,
                XAlign = TextAlignment.Center,
                YAlign = TextAlignment.Center
            };
        }

        private EntryCell CreateEntryCell()
        {
            return new EntryCell()
            {
                Label = "Bill Count: ",
                XAlign = TextAlignment.Center,
                Keyboard = Keyboard.Numeric,
                Placeholder = "0"
            };
        }

        private Slider CreateSlider()
        {
            return new Slider()
            {
                Minimum = 0,
                Maximum = 100,
                Value = 0
            };
        }

        private Stepper CreateStepper()
        {
            return new Stepper()
            {
                Minimum = 1,
                Value = 1,
                Increment = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
        }

        private Button CreateButton()
        {
            return new Button()
            {
                Text = "Calculate tip:",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Aqua
            };
        }
    }
}
