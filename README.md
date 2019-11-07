<img src="https://github.com/zsofi-gagyi/homesickness/blob/master/screenshots/3.png" width="870px"></img> 

<h2>Homesickness</h2>
<h3>Deployed <a href="https://homesicknessvisualiser.azurewebsites.net">here</a> - deactivated at the moment</h3>
<br/>
<br/>
<p>This is a web app that asks a free weather API every hour about the temperatures in my hometown and current city, stores the data and displays it for different intervals, chosen by the users. (The data from the sceenshot is mostly real, captured in the second half of July 2019.)</p>
<p>It's hosted on Azure.</p>
<p>This is a version which can be run locally. The deployed version can be found on the "deployed" branch. This version uses</p>

- C# + .NET Core
- Hangfire for scheduling background tasks
- MySQL with EntityFramework for storing data
- RazorViews + HTML, CSS and Google charts for presentation
- and has a logger which logs on the console.
