using System;
using System.Collections.Generic;
using System.Globalization;

namespace Calculator
{
    public partial class MainPage : ContentPage
    {
        private List<string> _expressionTokens = new List<string>();
        private bool _isNewNumber = true; // true — очистить экран при вводе цифры

        public MainPage()
        {
            InitializeComponent();
        }

        // Ввод цифр
        private void OnNumberClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string pressed = button.Text;

            if (Display.Text == "0" || _isNewNumber)
            {
                Display.Text = string.Empty;
            }

            _isNewNumber = false;
            Display.Text += pressed;
        }

        // Нажатие на операнд (+, -, ×, ÷)
        private void OnOperationClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            // Если уже есть число и знак — считаем промежуточный итог
            if (_expressionTokens.Count == 2)
            {
                _expressionTokens.Add(Display.Text); // Добавляем второе число

                double intermediateResult = ExecuteMath(_expressionTokens[0], _expressionTokens[1], _expressionTokens[2]);

                _expressionTokens.Clear();
                _expressionTokens.Add(intermediateResult.ToString(CultureInfo.InvariantCulture));

                Display.Text = intermediateResult.ToString(CultureInfo.InvariantCulture).Replace(".", ",");
            }
            else
            {
                _expressionTokens.Add(Display.Text); // Добавляем первое число
            }

            _expressionTokens.Add(button.Text); // Добавляем новый знак

            System.Diagnostics.Debug.WriteLine($"Стек: {string.Join(" ", _expressionTokens)}");

            Display.Text = "0"; // Очистка экрана по твоей логике
            _isNewNumber = true;
        }

        // Нажатие на "Равно" (=)
        private void OnCalculateClicked(object sender, EventArgs e)
        {
            if (_expressionTokens.Count == 2)
            {
                _expressionTokens.Add(Display.Text); // Добавляем второе число

                double finalResult = ExecuteMath(_expressionTokens[0], _expressionTokens[1], _expressionTokens[2]);

                Display.Text = finalResult.ToString(CultureInfo.InvariantCulture).Replace(".", ",");

                _expressionTokens.Clear();
                _isNewNumber = true;
            }
        }

        // Вычисление из 3-х элементов
        private double ExecuteMath(string num1, string op, string num2)
        {
            double left = double.Parse(num1.Replace(",", "."), CultureInfo.InvariantCulture);
            double right = double.Parse(num2.Replace(",", "."), CultureInfo.InvariantCulture);

            return op switch
            {
                "+" => left + right,
                "-" => left - right,
                "×" => left * right,
                "÷" => right == 0 ? 0 : left / right,
                _ => 0
            };
        }

        // Сброс (C)
        private void OnClearClicked(object sender, EventArgs e)
        {
            Display.Text = "0";
            _expressionTokens.Clear();
            _isNewNumber = true;
        }

        // Ввод запятой
        private void OnDecimalClicked(object sender, EventArgs e)
        {
            if (_isNewNumber)
            {
                Display.Text = "0,";
                _isNewNumber = false;
                return;
            }

            if (!Display.Text.Contains(","))
            {
                Display.Text += ",";
            }
        }
    }
}
