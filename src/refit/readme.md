# Introduction

This is a working example of refit calling a remote open API and calling a local protected web API.

`Sample.Service` is the local service
`Samples.Client` is the client that makes the calls


The client calls the remote open API and prints the result to log
The client calls the local protected API and prints the result to log

The sample makes use of user secrets hence why the configuration files are empty

This command adds a secret to the user secrets stored against the specified user secret id
```
dotnet user-secrets set --id <user-secret-id> <key> <value>
```

This command will list the user secrets stored against the specified user secret id
```
dotnet user-secrets list --id <user-secret-id>
```
