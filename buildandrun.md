# Build and Run Commands


### Docker CLI infrastructure init

-------------------------------------------------------

```
#dockerfile

#service image build and start
    docker stop coresb7
    docker rm coresb7
    docker rmi coresb7
    
    //rebuild  image
    docker build -t coresb7 .
    
    docker run -p 5003:80 --name coresb7 coresb7:latest

 #docker ocmpose
    docker compose up -d
 
#mssql
	docker pull mcr.microsoft.com/mssql/server

	docker run --name MSSQLCoreSB -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=awsedrQ1" -e "MSSQL_PID=Express" -p 1433:1433 -d mcr.microsoft.com/mssql/server:latest 

#Mongo
	docker pull mongo

	docker run --name mongoLocal -e MONGO_INITDB_ROOT_USERNAME=core -e MONGO_INITDB_ROOT_PASSWORD=core -p 27017:27017 mongo

#elastic
	docker pull docker.elastic.co/elasticsearch/elasticsearch:8.2.2 
	docker tag docker.elastic.co/elasticsearch/elasticsearch:8.2.2 elasticsearch
	docker network create elastic
	
	docker run --name elastic --net elastic -p 9200:9200 -p 9300:9300 -it elasticsearch

#kibana
	docker pull docker.elastic.co/kibana/kibana:8.2.2
	docker tag docker.elastic.co/kibana/kibana:8.2.2 kibana

	docker run --name kibana --net elastic -p 5601:5601 docker.elastic.co/kibana/kibana:8.2.2

```



### Packages to build core:

-------------------------------------------------------

``` 
dotnet add package Autofac 
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Autofac.Extensions.DependencyInjection

dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer 
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore

dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL 

dotnet add package MongoDB.Bson 
dotnet add package MongoDB.Driver 
dotnet add package MongoDB.Driver.Core 

dotnet add package NEST 

dotnet add package Elastic.Clients.Elasticsearch
dotnet add package Elasticsearch.Net 
```



### EF migration init (local folder reminder)

-------------------------------------------------------

```

dotnet ef migrations add "initial"  --context LogsContextTC --project C:\files\git\pets\CoreSB_7\CoreSBBL\CoreSBBL.csproj --startup-project C:\files\git\pets\CoreSB_7\CoreSBServer\CoreSBServer.csproj
dotnet ef database update  --context LogsContextTC --project C:\files\git\pets\CoreSB_7\CoreSBBL\CoreSBBL.csproj --startup-project C:\files\git\pets\CoreSB_7\CoreSBServer\CoreSBServer.csproj 

dotnet ef migrations add "initialGN"  --context LogsContextGN --project C:\files\git\pets\CoreSB_7\CoreSBBL\CoreSBBL.csproj --startup-project C:\files\git\pets\CoreSB_7\CoreSBServer\CoreSBServer.csproj 
dotnet ef database update  --context LogsContextGN --project C:\files\git\pets\CoreSB_7\CoreSBBL\CoreSBBL.csproj --startup-project C:\files\git\pets\CoreSB_7\CoreSBServer\CoreSBServer.csproj 
dotnet ef migrations remove  --context LogsContextGN --project C:\files\git\pets\CoreSB_7\CoreSBBL\CoreSBBL.csproj --startup-project C:\files\git\pets\CoreSB_7\CoreSBServer\CoreSBServer.csproj 

```


## nugets
-------------------------------------------------------

```

dotnet add package AutoFixture

```
