Follow [@ServiceStack](https://twitter.com/servicestack) or [view the docs](https://docs.servicestack.net), use [StackOverflow](http://stackoverflow.com/questions/ask) or the [Customer Forums](https://forums.servicestack.net/) for support.

Stripe
======

This project contains a .NET v4.5 and .NET Standard 2.0 Library containing a typed .NET client gateway for accessing [Stripe's REST API](https://stripe.com/docs/api/), updated to the latest **2018-02-28** Stripe API Version available.

It's used by [servicestack.net](http://servicestack.net/) to process merchant payments and recurring subscriptions online.

## Features

  - **Small**, typed, message-based API uses only clean DTO's and fits in a single [StripeGateway.cs](https://github.com/ServiceStack/Stripe/blob/master/src/Stripe/StripeGateway.cs)
  - **Async** every Stripe Service can be called via either Sync or Async methods
  - **Portable** profile available supporting .NET 4.5, Xamarin.iOS, Xamarin.Android and Windows Store clients
  - **Open-ended**, can use custom declarative DTO's defined in your own app to access new APIs  
  - **Testable**, implements the mockable [IRestGateway](https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack.Interfaces/IRestGateway.cs), can [return test data](https://github.com/ServiceStack/ServiceStack/blob/master/tests/ServiceStack.Common.Tests/MockRestGatewayTests.cs) with a generic [MockRestGateway](https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack/Testing/MockRestGateway.cs)
  - See [Stripe.Tests](https://github.com/ServiceStack/Stripe/tree/master/tests/Stripe.Tests) for more example usages.

## Install ServiceStack.Stripe

Install from NuGet with:

    PM> Install-Package ServiceStack.Stripe

Includes Portable Version (.NET 4.5, iOS, Android + Windows Store) 

### ASP.NET Core on .NET Framework

To use this library in [ASP.NET Core Apps running on the .NET Framework](http://docs.servicestack.net/templates-corefx), install the **.NET Standard 2.0** only NuGet package instead:

    PM> Install-Package ServiceStack.Stripe.Core

## Usage

Requires a [registered Stripe API Key](https://manage.stripe.com/register), e.g:

```csharp
var gateway = new StripeGateway("sk_test_23KlmQohLKD4dfmAvxYESZ2z");
```

Request DTO's are just clean POCO's with `[Route]` attributes defined, e.g:

```csharp
[Route("/customers/{Id}")]
public class GetStripeCustomer : IGet, IReturn<StripeCustomer>
{
    public string Id { get; set; }
}
```

The `IGet` interface marker indicates which HTTP Method Stripe expects, whilst `IReturn<StripeCustomer>` indicates what Stripe returns. 
The gateway uses this type information to provide its typed API, e.g:

```csharp
StripeCustomer customer = gateway.Get(new GetStripeCustomer { Id = customerId });
```

#### Async

```csharp
StripeCustomer customer = await gateway.GetAsync(new GetStripeCustomer { Id = customerId });
```

If you prefer, you can use the same `gateway.Send()` generic method for **all** messages as it is able to make use of 
the `IVerb` interface marker to control which HTTP method is used, e.g.

```csharp
StripeCustomer customer = gateway.Send(new GetStripeCustomer { Id = customerId });
```

#### Async

```csharp
StripeCustomer customer = await gateway.SendAsync(new GetStripeCustomer { Id = customerId });
```

Both of these calls translates to the [Retrieving a Customer](https://stripe.com/docs/api/curl#retrieve_customer) HTTP Request, Example in curl:

    curl https://api.stripe.com/v1/customers/cus_3552jPRgtQeRcK \
       -u yDOr26HsxyhpuRB3qbG07qfCmDhqutnA:

### Open-Ended, Declarative Message-based APIs

The `StripeGateway` benefits from an **Open Ended** message-based API where you're also able to use own Request DTO's to call new Stripe Services that StripeGateway has no knowledge about. E.g. The only custom code required to implement the `ChargeStripeCustomer` is this single, clean, declarative Request DTO:

```csharp
[Route("/charges")]
public class ChargeStripeCustomer : IPost, IReturn<StripeCharge>
{
    public int Amount { get; set; }
    public string Currency { get; set; }
    public string Customer { get; set; }
    public string Card { get; set; }
    public string Description { get; set; }
    public bool? Capture { get; set; }
    public int? ApplicationFee { get; set; }
}
```  

Which contains all the information needed to call the Stripe Service including the `/charges` relative url, using the **POST** HTTP method and the typed `StripeCharge` DTO it returns. To charge a Customer the Request DTO can either use the explicit `Post/PostAsync` or universal `Send/SendAsync` StripeGateway methods. 

## Documentation

These API examples follows [Stripe's API Documentation](https://stripe.com/docs/api/).

## [Charges](https://stripe.com/docs/api/curl#charges)

### Creating a new charge (charging a credit card)

```csharp
var charge = gateway.Post(new ChargeStripeCustomer
{
    Amount = 100,
    Customer = customer.Id,
    Currency = "usd",
    Description = "Test Charge Customer",
});
```

#### Async

```csharp
var charge = await gateway.PostAsync(new ChargeStripeCustomer
{
    Amount = 100,
    Customer = customer.Id,
    Currency = "usd",
    Description = "Test Charge Customer",
});
```

### Retrieving a Charge


```csharp
var charge = gateway.Get(new GetStripeCharge { ChargeId = charge.Id });
```

### Updating a Charge


```csharp
var charge = gateway.Post(new UpdateStripeCharge 
{
    ChargeId = charge.Id,
    Description = "Updated Charge Description"
});
```

### Refunding a Charge


```csharp
var refundCharge = gateway.Post(new RefundStripeCharge
{
    ChargeId = charge.Id,
});
```

### Capture a charge


```csharp
var charge = gateway.Post(new ChargeStripeCustomer
{
    Amount = 100,
    Customer = customer.Id,
    Currency = "usd",
    Description = "Test Charge Customer",
    Capture = false,  //Don't capture the charge immediately
});

//Can capture charge later with an explicit call
var captureCharge = gateway.Post(new CaptureStripeCharge
{
    ChargeId = charge.Id,
});
```

### List all Charges


```csharp
var charges = gateway.Get(new GetStripeCharges());
```

List all customer charges

```csharp
var charges = gateway.Get(new GetStripeCharges
{
    Customer = customer.Id,
});
```


## [Customers](https://stripe.com/docs/api/curl#customers)

### Creating a New Customer

```csharp
var customer = gateway.Post(new CreateStripeCustomer
{
    AccountBalance = 10000,
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
    Description = "Description",
    Email = "test@email.com",
});
```

### Creating a New Customer with a Card Token

```csharp
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
```

### Retrieving a Customer


```csharp
var customer = gateway.Get(new GetStripeCustomer { Id = customerId });
```

### Updating a Customer


```csharp
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
```

### Deleting a Customer


```csharp
var deletedRef = gateway.Delete(new DeleteStripeCustomer { Id = customer.Id });
```

### List all Customers


```csharp
var customers = gateway.Get(new GetStripeCustomers());
```

## [Cards](https://stripe.com/docs/api/curl#cards)

### Creating a new card


```csharp
var card = gateway.Post(new CreateStripeCard
{
    CustomerId = customer.Id,
    Card = new StripeCard
    {
        Name = "Test Card 2",
        Number = "5555555555554444",
        Cvc = "456",
        ExpMonth = 1,
        ExpYear = 2016,
        AddressLine1 = "1 Address Road",
        AddressLine2 = "12345",
        AddressZip = "City",
        AddressState = "NY",
        AddressCountry = "US",
    },
});
```

### Retrieving a customer's card


```csharp
var card = gateway.Get(new GetStripeCard
{
    CustomerId = customer.Id,
    CardId = card.Id,
});
```

### Updating a card


```csharp
var card = gateway.Post(new UpdateStripeCard
{
    CustomerId = customer.Id,
    CardId = customer.Cards.Data[0].Id,

    Name = "Test Card Updated",

    AddressLine1 = "1 Address Updated",
    AddressLine2 = "45321",
    AddressZip = "City",
    AddressState = "NY",
    AddressCountry = "US",

    ExpMonth = 2,
    ExpYear = 2030,
});
```

### Deleting cards


```csharp
var deletedRef = gateway.Delete(new DeleteStripeCard
{
    CustomerId = customer.Id,
    CardId = customer.Cards.Data[0].Id,
});
```

### Listing cards


```csharp
var cards = gateway.Get(new GetStripeCards { CustomerId = customer.Id });
```


## [Subscriptions](https://stripe.com/docs/api/curl#subscriptions)

### Updating the Customer's Active Subscription

```csharp
var subscription = gateway.Post(new SubscribeStripeCustomer
{
    CustomerId = customer.Id,
    Plan = plan.Id,
    Coupon = coupon.Id,
    Quantity = 1,
});
```

### Canceling a Customer's Subscription

```csharp
var cancelled = gateway.Delete(new CancelStripeSubscription
{
    CustomerId = customer.Id,
    AtPeriodEnd = false,
});
```


## [Plans](https://stripe.com/docs/api/curl#plans)

### Creating plans


```csharp
var plan = gateway.Post(new CreateStripePlan
{
    Id = "TEST-PLAN-01",
    Amount = 10000,
    Currency = "usd",
    Name = "Test Plan",
    Interval = StripePlanInterval.month,
    IntervalCount = 1,
});
```

### Retrieving a Plan


```csharp
var plan = gateway.Get(new GetStripePlan { Id = plan.Id });

```

### Updating a plan


```csharp
var updatedPlan = gateway.Post(new UpdateStripePlan
{
    Id = "TEST-PLAN-01",
    Name = "NEW PLAN UPDATED!",
});
```

### Deleting a plan


```csharp
var gateway.Delete(new DeleteStripePlan { Id = plan.Id });
```

### List all Plans


```csharp
var plans = gateway.Get(new GetStripePlans { Count = 20 });
```


## [Coupons](https://stripe.com/docs/api/curl#coupons)

### Creating coupons


```csharp
var coupon = gateway.Post(new CreateStripeCoupon
{
    Id = "TEST-COUPON-01",
    Duration = StripeCouponDuration.repeating,
    PercentOff = 20,
    Currency = "usd",
    DurationInMonths = 2,
    RedeemBy = DateTime.UtcNow.AddYears(1),
    MaxRedemptions = 10,
});
```

### Retrieving a Coupon


```csharp
var coupon = gateway.Get(new GetStripeCoupon { Id = coupon.Id });
```

### Deleting a coupon


```csharp
var deletedRef = gateway.Delete(new DeleteStripeCoupon { Id = plan.Id });
```

### List all Coupons


```csharp
var coupons = gateway.Get(new GetStripeCoupons { Count = 20 });
```


## [Discounts](https://stripe.com/docs/api/curl#discounts)

### Deleting a Discount


```csharp
var deletedRef = gateway.Delete(new DeleteStripeDiscount { CustomerId = customer.Id });
```


## [Invoices](https://stripe.com/docs/api/curl#invoices)

### Retrieving an Invoice


```csharp
var invoice = gateway.Get(new GetStripeInvoice { Id = invoice.Id });
```

### Creating an invoice


```csharp
var stripeInvoice = gateway.Post(new CreateStripeInvoice
{
    Customer = customer.Id
});
```

### Paying an invoice


```csharp
var paidInvoice = gateway.Post(new PayStripeInvoice
{
    Id = invoice.Id
});
```

### Retrieving a List of Invoices


```csharp
var invoices = gateway.Get(new GetStripeInvoices { Count = 20 });
```

Get a list of customer invoices


```csharp
var invoices = gateway.Get(new GetStripeInvoices 
{ 
    Customer = customer.Id
});
```

### Retrieving a Customer's Upcoming Invoice


```csharp
var upcomingInvoice = gateway.Get(new GetUpcomingStripeInvoice
{
    Customer = customer.Id,
});
```
