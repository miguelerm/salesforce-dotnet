# salesforce-dotnet
Salesforce SOAP API client for dotnet core

## Setup

```bash
PM> Install-Package CodeGardener.Salesforce.DotNet
```

## Usage

To consume the Salesforce SOAP API just create a new Instance of the SoapClient, Authenticate and make the query:

```cs
var contactId = "";
var username = "";
var password = "";
var token = "";
var contact = default(Contact);
var httpClient = new HttpClient();

using (var client = new SoapClient(httpClient)) {
    await client.LoginAsync(username, password, token);
    contact = await client.QueryAsync<Contact>($"SELECT Id, Name, AccountId FROM Contact WHERE Id = '{contactId}'").SingleOrDefault();
}

Console.WriteLine($"Contact Name: {contact.Name}");
```

## License

Licensed under the The [MIT License (MIT)](LICENSE).

