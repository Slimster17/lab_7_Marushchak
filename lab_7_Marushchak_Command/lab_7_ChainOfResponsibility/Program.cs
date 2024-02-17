using System;

namespace lab_7_ChainOfResponsibility
{
    abstract class PaymentHandler // common handler
    {
        protected PaymentHandler _nextHandler;

        public void SetNextHandler(PaymentHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public abstract void CompletePayment(double amount);
    }
    
    class BankPaymentHandler : PaymentHandler // concreate handler
    {
        public override void CompletePayment(double amount)
        {
            if (amount <= 100)
            {
                Console.WriteLine($"{amount} is complete by Bank payment");
            }
            else if (_nextHandler != null)
            {
                _nextHandler.CompletePayment(amount);
            }
        }
    }
    
    class WesternUnionPaymentHandler : PaymentHandler
    {
        public override void CompletePayment(double amount)
        {
            if (amount > 100 && amount <= 200)
            {
                Console.WriteLine($"{amount} is complete by WesternUnion payment");
            }
            else if (_nextHandler != null)
            {
                _nextHandler.CompletePayment(amount);
            }
        }
    }
    
    class UnistreamPaymentHandler : PaymentHandler
    {
        public override void CompletePayment(double amount)
        {
            if (amount > 200 && amount <= 300)
            {
                Console.WriteLine($"{amount} is complete by Unistream payment");
            }
            else if (_nextHandler != null)
            {
                _nextHandler.CompletePayment(amount);
            }
        }
    }
    
    class PaypalPaymentHandler : PaymentHandler
    {
        public override void CompletePayment(double amount)
        {
            if (amount > 300 && amount <= 400)
            {
                Console.WriteLine($"{amount} is complete by Paypal payment");
            }
            else if (_nextHandler != null)
            {
                _nextHandler.CompletePayment(amount);
            }
        }
    }

    class Client // user
    {
        private PaymentHandler _handlerChain;

        public Client()
        {
            _handlerChain = new BankPaymentHandler();
            PaymentHandler westernUnionHandler = new WesternUnionPaymentHandler();
            PaymentHandler unistreamHandler = new UnistreamPaymentHandler();
            PaymentHandler payPalHandler = new PaypalPaymentHandler();
            
            _handlerChain.SetNextHandler(westernUnionHandler);
            westernUnionHandler.SetNextHandler(unistreamHandler);
            unistreamHandler.SetNextHandler(payPalHandler);
        }

        public void Pay(double amount)
        {
            _handlerChain.CompletePayment(amount);
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            Client client = new Client();
            
            client.Pay(50);
            Console.WriteLine(new string('*',50));
            
            client.Pay(150);
            Console.WriteLine(new string('*',50));
            
            client.Pay(250);
            Console.WriteLine(new string('*',50));
            
            client.Pay(350);
            Console.WriteLine(new string('*',50));
            
            client.Pay(450);

        }
    }
}