SET NUGET=..\src\.nuget\nuget
%NUGET% setApiKey 4793bfcc-06ad-4ec6-bc48-62f514c2aa9a
%NUGET% push ServiceStack.Stripe.4.0.20.nupkg
%NUGET% push ServiceStack.Stripe.4.0.20.symbols.nupkg

%NUGET% push ServiceStack.Stripe.Pcl.4.0.20.nupkg
%NUGET% push ServiceStack.Stripe.Pcl.4.0.20.symbols.nupkg
