<h2>Homesickness</h2>
<h3>Deployed <a href="https://homesicknessvisualiser.azurewebsites.net/homesickness/week">here</a></h3>

<p>This is a web app that asks a free weather API every hour about the temperatures in my hometown and current city, stores the data and displays it for different intervals, chosen by the users. (The data from the sceenshot is mostly real, captured in the second half of July 2019.)</p>
<p>It's hosted on Azure. The deployed version uses</p>

- C# + .NET Core 2.2
- Azure Functions for background tasks
- Azure "SQL Database" with EntityFramework for storing data
- RazorViews + HTML, CSS and Google charts for presentation
- a logger with output that can be read from the Azure portal
