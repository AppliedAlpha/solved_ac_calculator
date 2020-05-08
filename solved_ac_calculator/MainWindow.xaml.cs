using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace solved_ac_calculator
{
    public partial class MainWindow : Window
    {
        public ProblemClient problemClient = null;
        public BrushConverter converter = new BrushConverter();

        public MainWindow()
        {
            InitializeComponent();
            execute.Click += _execute;
        }

        private void _execute(object sender, RoutedEventArgs eventArgs)
        {
            try
            {
                String color_hex = null;
                if (!String.IsNullOrWhiteSpace(text_input.Text))
                {
                    problemClient = new ProblemClient(text_input);
                    problemClient.request(request_bar);
                    problemClient.calculate();

                    Int64 exp = problemClient.exp;
                    Int64[] curr = { exp - problemClient.tier.low, problemClient.tier.high - problemClient.tier.low };
                    String ti = problemClient.tier.name;
                    int exp_rate = (int)(curr[0] * 10000 / curr[1]);

                    if (ti.Contains("Bronze")) color_hex = "#ad5600";
                    else if (ti.Contains("Silver")) color_hex = "#435f7a";
                    else if (ti.Contains("Gold")) color_hex = "#ec9a00";
                    else if (ti.Contains("Platinum")) color_hex = "#27e2a4";
                    else if (ti.Contains("Diamond")) color_hex = "#00b4fc";
                    else if (ti.Contains("Ruby")) color_hex = "#ff0062";

                    Brush brush = (Brush)converter.ConvertFromString(color_hex);
                    Brush brush2 = (Brush)converter.ConvertFromString("#7F" + color_hex.Substring(1, 6));
                    total_exp.Foreground = brush;
                    exp_bar.Foreground = brush;
                    tier.Foreground = brush;
                    exp_bar_bright.Foreground = brush2;

                    total_exp.Text = exp.ToString("N0");
                    current_exp.Text = $"{exp_rate / 100}.{exp_rate % 100}% ({curr[0].ToString("N0")} / {curr[1].ToString("N0")})";
                    tier.Text = ti;
                    exp_bar.Value = (double)exp_rate / 100;
                    exp_bar_bright.Value = (double)exp_rate / 100;
                    // "https://static.solved.ac/tier_small/1.svg";
                    MessageBox.Show("Successfully Calculated!");
                }
                else
                {
                    throw new ArgumentException("TextBox is Empty!");
                }
}
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Occured");
            }
        }
    }
}
