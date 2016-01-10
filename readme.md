# EuroCallPriceEnhanced.cs

This code simulates the path of an underlying stock using Geometric Brownian motion. 
Using this, the price of a European Call Option is calculated.

The code includes comments to explains in further detail what the code is doing.

A few notes regarding this version of the code:
a) The code used to ask the user to choose the # of simulations (it is currently commented out). I have set this N = 5000 since the addition of the graphing
    of the underlying price for each simulation. I found that the plotting functionality slowed the code a bit. For N = 5000,
    the code still runs in ~ 3-4 minutes. Previous versions of the code (without the plotting extra credit) would be able to run 100,000
    simulations in a much shorter timeframe.

b) The number of increments between t = 0 and t = T (expiration) is a fixed value in the code. It was not included in the code, but a
    possible method of choosing an optimal number of increments would be to choose a number (e.g. 100) to begin with and calculate
    a value for the Call Option. Then, simulate the process again using double the number of increments (so 200 in our case). Continue
    with this method until the discretization error is within a comfortable margin.
    
If you have any questions. Please feel free to contact me.
