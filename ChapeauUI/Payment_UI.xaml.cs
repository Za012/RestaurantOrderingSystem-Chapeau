﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media.Animation;
using System.Data;
using ChapeauLogic;
using ChapeauModel;
using System;

namespace ChapeauUI
{
    /// <summary>
    /// Interaction logic for Payment_UI.xaml
    /// </summary>
    public partial class Payment_UI : Page
    {
        private Payment payment;
        private Payment_Service payment_Logic = new Payment_Service();

        public Payment_UI(int order_id, int customer_count)
        {
            InitializeComponent();
            payment = new Payment(customer_count, order_id);
            FillReceipt();
            lbl_order.Content = $"Order ID: {order_id}";
        }

        private void FillReceipt()
        {

            //Get listview from UserControls
            OrderList orderList = new OrderList(payment);
            order_list.Children.Add(orderList);
            //// Process the data and fill the model + calc price
            payment = payment_Logic.GetTotalPrice(payment_Logic.GetReceipt(payment.Order_id), payment);

            // Display price on the labels
            total_price.Content = $"Total Price: {payment.Price.ToString("0.00 €")}";
            vat_price.Content = $"Vat Price: {payment.Vat.ToString("0.00 €")}";
            btn_Payment_Finish.IsEnabled = false;
            tip_Box.IsEnabled = false;
        }

        private void Btn_Payment_Finish_Click(object sender, RoutedEventArgs e)
        {
            // Radio button animation if user click finish when r button is not clicked.
            //if (payment.Method == null)
            //{
            //    DoubleAnimation animation = new DoubleAnimation(2, new Duration(TimeSpan.FromSeconds(1.5)));
            //    Pin_rBtn.BeginAnimation(WidthProperty, animation);
            //    return;
            //}
            //Another time...

            float tip = 0;

            // Get information from textboxes
            if (tip_Box.Text != "")
            {
                tip = float.Parse(tip_Box.Text);
            }

            string comment = comment_Box.Text;

            // Fill the payment with information then send information to db
            payment_Logic.SetPayment(payment, tip, comment);
            payment_Logic.InsertPayment(payment);

            // Direct to tableview when order is finalized.
            NavigationService.Navigate(new Tableview_UI());
        }

        private void Radio_Btn_Checked(object sender, RoutedEventArgs e)
        {

            PayMethodCheck(payment_Logic.GetPayMethod((sender as RadioButton).Content.ToString()));
            btn_Payment_Finish.IsEnabled = true;
        }

        private void PayMethodCheck(PayMethod method)
        {
            payment.Method = method;
            if (payment.Method == PayMethod.Cash)
            {
                tip_Box.IsEnabled = true;
            }
            else
            {
                tip_Box.IsEnabled = false;
            }
        }

        //What exactly does this need to do...? Ask Nymp/Erwin/Gerwin
        private void Btn_Split_Click(object sender, RoutedEventArgs e)
        {
            SplitPanel();
        }
        private void SplitPanel()
        {
            Payment_Split split = new Payment_Split(payment);
            test_panel.Children.Add(split);
            Btn_Split.Visibility = Visibility.Hidden;
            Btn_Undo_Split.Visibility = Visibility.Visible;
            btn_even_split.Visibility = Visibility.Visible;
        }


        private void Btn_Undo_Split_Click(object sender, RoutedEventArgs e)
        {
            test_panel.Children.Clear();
            Btn_Undo_Split.Visibility = Visibility.Hidden;
            btn_even_split.Visibility = Visibility.Hidden;
            Btn_Split.Visibility = Visibility.Visible;
        }

        private void Btn_even_split_Click(object sender, RoutedEventArgs e)
        {
            test_panel.Children.Clear();
            SplitPanel();
        }
    }
}
