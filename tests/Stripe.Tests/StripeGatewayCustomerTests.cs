using NUnit.Framework;
using ServiceStack.Stripe;
using ServiceStack.Stripe.Types;
using ServiceStack.Text;

namespace Stripe.Tests
{
    /*
     * Customers 
     * https://stripe.com/docs/api/curl#customers
     */
    [TestFixture]
    public class StripeGatewayCustomerTests : TestsBase
    {
        [Test]
        public void Can_Create_Customer()
        {
            var customer = CreateCustomer();

            customer.PrintDump();

            Assert.That(customer.Id, Is.Not.Null);
            Assert.That(customer.Email, Is.EqualTo("test@email.com"));
            Assert.That(customer.Cards.Count, Is.EqualTo(1));
            Assert.That(customer.Cards.Data[0].Name, Is.EqualTo("Test Card"));
            Assert.That(customer.Cards.Data[0].ExpMonth, Is.EqualTo(1));
            Assert.That(customer.Cards.Data[0].ExpYear, Is.EqualTo(2015));
        }

        [Test]
        public void Can_Create_Customer_with_Card_Token()
        {
            var cardToken = gateway.Post(new CreateStripeToken {
                Card = new StripeCard
                {
                    Name = "Test Card",
                    Number = "4242424242424242",
                    Cvc = "123",
                    ExpMonth = 1,
                    ExpYear = 2015,
                    AddressLine1 = "1 Address Road",
                    AddressLine2 = "12345",
                    AddressZip = "City",
                    AddressState = "NY",
                    AddressCountry = "US",
                },
            });

            var customer = gateway.Post(new CreateStripeCustomerWithToken
            {
                AccountBalance = 10000,
                Card = cardToken.Id,
                Description = "Description",
                Email = "test@email.com",
            });

            customer.PrintDump();

            Assert.That(customer.Id, Is.Not.Null);
            Assert.That(customer.Email, Is.EqualTo("test@email.com"));
            Assert.That(customer.Cards.Count, Is.EqualTo(1));
            Assert.That(customer.Cards.Data[0].Name, Is.EqualTo("Test Card"));
            Assert.That(customer.Cards.Data[0].ExpMonth, Is.EqualTo(1));
            Assert.That(customer.Cards.Data[0].ExpYear, Is.EqualTo(2015));
        }

        [Test]
        public void Can_Create_Customer_with_conflicting_JsConfig()
        {
            JsConfig.EmitCamelCaseNames = true;

            var customer = CreateCustomer();

            customer.PrintDump();

            Assert.That(customer.Id, Is.Not.Null);
            Assert.That(customer.Email, Is.EqualTo("test@email.com"));
            Assert.That(customer.Cards.Count, Is.EqualTo(1));
            Assert.That(customer.Cards.Data[0].Name, Is.EqualTo("Test Card"));
            Assert.That(customer.Cards.Data[0].ExpMonth, Is.EqualTo(1));
            Assert.That(customer.Cards.Data[0].ExpYear, Is.EqualTo(2015));
        }

        [Test]
        public void Can_Get_Customer()
        {
            var customer = CreateCustomer();

            customer.PrintDump();

            var newCustomer = gateway.Get(new GetStripeCustomer { Id = customer.Id });

            newCustomer.PrintDump();

            Assert.That(newCustomer.Id, Is.EqualTo(customer.Id));
            Assert.That(newCustomer.Email, Is.EqualTo("test@email.com"));
            Assert.That(newCustomer.Cards.Count, Is.EqualTo(1));
            Assert.That(newCustomer.Cards.Data[0].Name, Is.EqualTo("Test Card"));
        }

        [Test]
        public void Can_Update_Customer()
        {
            var customer = CreateCustomer();

            var updatedCustomer = gateway.Post(new UpdateStripeCustomer
            {
                Id = customer.Id,
                Card = new StripeCard
                    {
                        Id = customer.Cards.Data[0].Id,
                        Name = "Updated Test Card",
                        Number = "4242424242424242",
                        Cvc = "123",
                        ExpMonth = 1,
                        ExpYear = 2015,
                        AddressLine1 = "1 Address Road",
                        AddressLine2 = "12345",
                        AddressZip = "City",
                        AddressState = "NY",
                        AddressCountry = "US",
                    },
                AccountBalance = 20000,
                Description = "Updated Description",
                Email = "updated@email.com",
            });

            updatedCustomer.PrintDump();

            Assert.That(updatedCustomer.Id, Is.EqualTo(customer.Id));
            Assert.That(updatedCustomer.Email, Is.EqualTo("updated@email.com"));
            Assert.That(updatedCustomer.Cards.Count, Is.EqualTo(1));
            Assert.That(updatedCustomer.Cards.Data[0].Name, Is.EqualTo("Updated Test Card"));
        }

        [Test]
        public void Can_Delete_Customer()
        {
            var customer = CreateCustomer();

            var deletedRef = gateway.Delete(new DeleteStripeCustomer { Id = customer.Id });

            deletedRef.PrintDump();

            Assert.That(deletedRef.Id, Is.EqualTo(customer.Id));
            Assert.That(deletedRef.Deleted);
        }

        [Test]
        public void Can_List_all_Customers()
        {
            var customer = CreateCustomer();

            var customers = gateway.Get(new GetStripeCustomers());

            customers.PrintDump();

            Assert.That(customers.Count, Is.GreaterThan(0));
            Assert.That(customers.Data[0].Id, Is.Not.Null);
        }

    }
}