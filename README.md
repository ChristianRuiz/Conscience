# Conscience

There are two main Apps:
  - Aleph Site: WebApp for the company to manage the hosts
  - Hosts Mobile App: Xamarin App for iOS and Android that is connected to the Aleph Site

### Frontend configuration

You need to browse a cmd to "\Conscience\Conscience.Web\js\" and run:

```sh
$ npm install
$ npm run build
```

Optionally you can run the watcher to avoid building multple times:

```sh
$ npm run watch
```

Every time the GraphQL schema is updated on server side, you need to run:

```sh
$ npm run schema
$ npm run build
```

### Backend configuration

You need to open the "Conscience.Web.sln" with Visual Studio 2015 and build the entire solution. It will restore the NuGet packages and build the project. Once build, you can configure the Conscience.Web proejct to run with the local IIS on "http://localhost" or you can enable the IIS Express and let Visual Studio configure and start it on F5.

The urls on the environment:

| Url | Description |
| ------ | ------ |
| /GraphQL | GraphiQL environment to test queries |
| /Login | Login page (work in progress) |
| / | List of employees (work in progress) |

### GraphQL

To start testing GraphQL you need to perform a login mutation on the GraphiQL test environment ("/"):

```sh
mutation Login($userName: String!, $password: String!) {
  accounts
  {
    login(userName:$userName, password:$password)
    {
      id
    }
  }
}
```

With Query Variables:

```sh
{"userName": "arnold", "password": "123456" }
```

Once logged, you can browse the "/Home" page and you should be retreaving multiple Employees. You can also test some queries or logout using the GraphiQL interface:

```sh
query GetAll {
  employees {
    getAll {
      account {
        userName,
        id
      },
      id
    }
  }
}
```

```sh
query EmployeesQuery 
{  
	employees { ...F0  }
} 

fragment F0 on EmployeeQuery 
{  
	getAll 
	{    
		account 
		{      
			userName,      
			id    
			},    
		id  
	}
}
```

```sh
mutation Logout {
  accounts
  {
    logout
    {
      id
    }
  }
}
```

