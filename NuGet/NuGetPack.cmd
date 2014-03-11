SET NUGET=..\src\.nuget\nuget
%NUGET% pack ServiceStack.Stripe\servicestack.stripe.nuspec -symbols
%NUGET% pack ServiceStack.Stripe.Pcl\servicestack.stripe.pcl.nuspec -symbols
